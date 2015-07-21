using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace BtHeart.Controller
{
    public class HeartRate:IAnalyze
    {
        protected ConcurrentQueue<double> ecgQueue = new ConcurrentQueue<double>(); // 滤波信号
        private BackgroundWorker worker = new BackgroundWorker();

        private HeartContext Context;

        public HeartRate(HeartContext hc)
        {
            this.Context = hc;
            this.Context.Processed += Context_Processed;

            worker.DoWork += worker_DoWork;
            worker.WorkerSupportsCancellation = true;
        }

        private void Context_Processed(EcgPacket packet)
        {
            foreach (var data in packet.Data)
                ecgQueue.Enqueue(data);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(!worker.CancellationPending)
            {
                Analyze();
                Thread.Sleep(2);
            }
        }

        public virtual void Start()
        {
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        public virtual void Stop()
        {
            if (worker.IsBusy)
                worker.CancelAsync();
        }

        // 差分计算
        protected virtual List<double> Diff(List<double> x)
        {
            return x;
        }

        protected virtual void Analyze()
        {

        }

        protected void OnRateAnalyze(int? rate)
        {
            if (RateAnalyzed != null)
                RateAnalyzed(rate);
        }

        public event Action<double> Analyzed;
        public event Action<int?> RateAnalyzed;
    }

    /// <summary>
    /// 差分阈值计算心率
    /// </summary>
    public class DifferenceHeartRate:HeartRate
    {
        //protected ConcurrentQueue<double> differenceQueue = new ConcurrentQueue<double>(); // 差分阈值
        private double DDmax = 0.0; // 正差分阈值
        private double DDmin = 0.0; // 负差分阈值
        private double AAmax = 0.0; // 幅度阈值
        protected int LastRate = -1; // 上一次心率
        protected int? NewRate = null; // 当前心率
        protected int ErrorCnt = 0; // 异常次数
        protected RateState State = RateState.Jumped;

        public DifferenceHeartRate(HeartContext hc)
            :base(hc)
        {

        }

        public override void Stop()
        {
            base.Stop();
            Clear();
            State = RateState.Jumped;
        }

        protected override List<double> Diff(List<double> x)
        {
            //var yList = new List<double>();
            //for (int i = 2; i < x.Count - 2; i++)
            //{
            //    double y = x[i + 2] + x[i + 1] - x[i - 1] - x[i - 2];
            //    yList.Add(y);
            //}
            //return yList;
            var yList = new List<double>();
            for (int i = 4; i < x.Count - 4; i++)
            {
                double y = x[i + 4] + x[i + 3] + x[i + 2] + x[i + 1] -
                    x[i - 1] - x[i - 2] - x[i - 3] - x[i - 4];
                yList.Add(y);
            }
            return yList;
        }

        protected override void Analyze()
        {
            switch(State)
            {
                case RateState.Jumped:
                    Jump();
                    break;
                case RateState.Uninitialized:
                    InitThesold();
                    break;
                case RateState.Initialized:
                    if (CalcRate())
                    {
                        CheckRate();
                        CheckError();
                    }
                    break;
                case RateState.Error:
                    Clear();
                    break;
            }
            OnRateAnalyze(NewRate);
        }

        // 跳过前3秒的混乱数据
        private void Jump()
        {
            if(ecgQueue.Count >= HeartContext.JumpSec * HeartContext.F)
            {
                Clear();
                State = RateState.Uninitialized;
            }
        }

        // 初始化阈值
        protected virtual void InitThesold()
        {
            int allSize = HeartContext.ThesoldSec * HeartContext.F;
            if (ecgQueue.Count >= allSize)
            {
                var ecgList = ecgQueue.ToList();
                var diffList = Diff(ecgList);

                int size = HeartContext.ThesoldSec * HeartContext.F / 5;
                double[] ddmaxThesold = new double[5];
                double[] ddminThesold = new double[5];
                double[] aamaxThesold = new double[5];
                for (int i = 0; i < 5;i++ )
                {
                    var sequence = diffList.Skip(i * size).Take(size);
                    ddmaxThesold[i] = diffList.Max();
                    ddminThesold[i] = diffList.Min();
                    aamaxThesold[i] = ecgList.Max();
                }

                // 确定初始阈值
                DDmax = ddmaxThesold.Average() / HeartContext.Th;
                DDmin = ddminThesold.Average() / HeartContext.Th;
                AAmax = aamaxThesold.Average() / HeartContext.Th;

                diffList.Clear();
                ecgList.Clear();
                ecgQueue.Clear();
                State = RateState.Initialized;
            }
        }

        // 计算心率
        protected virtual bool CalcRate()
        {
            int allSize = HeartContext.AdjustSec*HeartContext.F;
            if (ecgQueue.Count >= allSize)
            {
                var ecgList = ecgQueue.ToList();
                var diffList = Diff(ecgList);
                DDmax = (0.8 * DDmax + 0.2 * diffList.Max())/HeartContext.Th;
                DDmin = (0.8 * DDmin + 0.2 * diffList.Min())/HeartContext.Th;
                AAmax = (0.8 * AAmax + 0.2 * ecgList.Max())/HeartContext.Th;
                Console.WriteLine(DDmax + "," + DDmin+","+AAmax);
                List<int> posR = new List<int>(); // R值位置

                for (int i = 0; i < diffList.Count - 5;i++ )
                {
                    // 满足三个条件：
                    // 1.当前点差分值大于正差分阈值
                    // 2.下一点差分值大于正差分阈值
                    // 3.当前点幅度大于幅度阈值
                    if (diffList[i] > DDmax && diffList[i + 1] > DDmax && diffList[i + 2] > DDmax
                        && diffList[i + 3] > DDmax && diffList[i + 4] > DDmax
                        && ecgList[i] > AAmax)
                    {
                        int windowSize = (int)(0.08 * HeartContext.F);// 80ms窗口
                        var tempList = diffList.Skip(i).Take(windowSize).ToList();
                        // 窗口内寻找是否存在小负差分阈值的点
                        if(tempList.Any(e => e < DDmin))
                        {
                            for(int j = 0; j < tempList.Count - 1;j++)
                            {
                                if(tempList[j+1] > 0 && tempList[j] < 0)
                                {
                                    posR.Add(i+j);
                                    i += (int)(HeartContext.RefractorySec*HeartContext.F); // 跳过不应期
                                    break;
                                }
                            }
                        }
                    }
                }

                if (posR.Count <= 1)
                    NewRate = null;
                else
                {
                    double rr = 0;
                    rr = 60*HeartContext.F*(posR.Count-1)/(double)(posR.Last()-posR.First());
                    NewRate = Convert.ToInt32(rr);
                }
                diffList.Clear();
                ecgList.Clear();
                ecgQueue.Clear();
                return true;
            }

            State = RateState.Initialized;
            return false;
        }

        // 检验心率计算
        private void CheckRate()
        {
            if(!NewRate.HasValue) // 本次和上次都未检测到心跳
            {
                ErrorCnt++;
            }
            else
            {
                if (LastRate != -1)
                {
                    if (Math.Abs(NewRate.Value - LastRate) >= 10) // 如果当前心跳与上次心跳差异太大，则取平均
                    {
                        NewRate = LastRate;
                    }
                    else if(Math.Abs(NewRate.Value - LastRate) >= 5)
                    {
                        NewRate = (NewRate + LastRate) / 2;
                    }

                    if (NewRate < 30 || NewRate > 250) // 心率超过范围
                    {
                        ErrorCnt++;
                    }
                }
                LastRate = NewRate.Value; // 更新旧心率值
            }
        }

        // 检测错误次数
        private void CheckError()
        {
            if(ErrorCnt >= 2)
            {
                State = RateState.Error;
                ErrorCnt = 0;
            }
        }

        // 清空参数
        private void Clear()
        {
            ecgQueue.Clear();
            DDmax = 0.0;
            DDmin = 0.0;
            AAmax = 0.0;
            ErrorCnt = 0;
            LastRate = -1;
            NewRate = null;
            State = RateState.Uninitialized;
        }
    }

    public class DifferenceHeartRateEx:DifferenceHeartRate
    {
        private double DDmax = 0.0; // 差分阈值

        public DifferenceHeartRateEx(HeartContext hc)
            :base(hc)
        {
            var ecgList = ecgQueue.ToList();
            var diffList = Diff(ecgList);

        }

        protected override List<double> Diff(List<double> x)
        {
            var yList = new List<double>();
            //for(int i = 1; i < x.Count;i++)
            //{
            //    double y = x[i] - x[i - 1];
            //    yList.Add(y);
            //}
            for (int i = 0; i < x.Count-3; i++)
            {
                double y = x[i+3] - x[i]; // 跳点计算
                yList.Add(y);
            }
            return yList;
        }

        protected override void InitThesold()
        {
            State = RateState.Initialized;
        }

        protected override bool CalcRate()
        {
            int allSize = HeartContext.AdjustSec * HeartContext.F;
            if (ecgQueue.Count >= allSize)
            {
                var ecgList = ecgQueue.ToList();
                var diffList = Diff(ecgList);
                var absdiffList = diffList.Select(d => Math.Abs(d)).ToList(); // 差分绝对值
                DDmax = absdiffList.Average() * HeartContext.Th;
                Console.WriteLine(DDmax);
                int[] tt = new int[diffList.Count];
                List<int> posR = new List<int>(); // R值位置

                for (int i = 0; i < absdiffList.Count; i++)
                {
                    tt[i] = absdiffList[i] > DDmax ? 1 : 0;
                }
                while(tt.Any(t => t == 1))
                {
                    for(int i = 0; i < tt.Length;i++)
                    {
                        if(tt[i] == 1)
                        {
                            // 符合阈值附近开辟80ms的窗口寻找
                            int delta = (int)(0.08 * HeartContext.F);
                            int startIndex = i - delta / 2 >= 0 ? i - delta/2 : 0;
                            var rList = diffList.Skip(startIndex).Take(delta).ToList();
                            double Rmax = rList.Max();
                            double Rmin = rList.Min();
                            // 寻找R波极值点，保存R波位置
                            if (Math.Abs(Rmax) >= Math.Abs(Rmin)) 
                            {
                                posR.Add(rList.IndexOf(Rmax) + startIndex);
                            }
                            else
                            {
                                posR.Add(rList.IndexOf(Rmin) + startIndex);
                            }

                            int period = i + (int)(HeartContext.RefractorySec * HeartContext.F);
                            for (int j = i; j < period &&j < tt.Length; j++) // 跳过不应期
                                tt[j] = 0;
                        }
                    }
                }

                if (posR.Count <= 1)
                    NewRate = null;
                else
                {
                    double rr = 0;
                    rr = 60 * HeartContext.F * (posR.Count - 1) / (double)(posR.Last() - posR.First());
                    NewRate = Convert.ToInt32(rr);
                }
                diffList.Clear();
                ecgList.Clear();
                ecgQueue.Clear();

                return true;
            }

            State = RateState.Initialized;
            return false;
        }
    }

    public enum RateState
    {
        Jumped, // 跳过初始
        Uninitialized, // 阈值未初始化
        Initialized, // 阈值初始化
        Error // 计算异常
    }
}
