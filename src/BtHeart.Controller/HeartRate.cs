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
        protected List<double> Diff(List<double> x)
        {
            var yList = new List<double>();
            for(int i= 2;i < x.Count-2;i++)
            {
                double y = x[i + 2] + x[i + 1] - x[i - 1] - x[i - 2];
                yList.Add(y);
            }
            return yList;
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
        protected double DDmax = 0.0; // 正差分阈值
        protected double DDmin = 0.0; // 负差分阈值
        protected double AAmax = 0.0; // 幅度阈值
        protected int? Rate = null; // 心率
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
                    CalcRate();
                    break;
                case RateState.Error:
                    Clear();
                    break;
            }
            OnRateAnalyze(Rate);
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
        private void InitThesold()
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
        private void CalcRate()
        {
            int allSize = HeartContext.AdjustSec*HeartContext.F;
            if (ecgQueue.Count >= allSize)
            {
                var ecgList = ecgQueue.ToList();
                var diffList = Diff(ecgList);
                DDmax = (0.8 * DDmax + 0.2 * diffList.Max());
                DDmin = (0.8 * DDmin + 0.2 * diffList.Min());
                AAmax = (0.8 * AAmax + 0.2 * ecgList.Min());
                Console.WriteLine(DDmax + "," + DDmin+","+AAmax);
                List<int> posR = new List<int>(); // R值位置

                for (int i = 0; i < diffList.Count - 1;i++ )
                {
                    // 满足三个条件：
                    // 1.当前点差分值大于正差分阈值
                    // 2.下一点差分值大于正差分阈值
                    // 3.当前点幅度大于幅度阈值
                    if(diffList[i] > DDmax && diffList[i+1] > DDmax && ecgList[i] > AAmax)
                    {
                        int windowSize = (int)(0.16 * HeartContext.F);// 160ms窗口
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
                    Rate = null;
                else
                {
                    double rr = 0;
                    for (int i = 1; i < posR.Count; i++)
                    {
                        rr += posR[i] - posR[i - 1];
                    }
                    rr /= (posR.Count - 1);
                    Rate = Convert.ToInt32(60*HeartContext.F/rr);
                }
                diffList.Clear();
                ecgList.Clear();
                ecgQueue.Clear();
            }

            State = RateState.Initialized;
        }

        // 清空参数
        private void Clear()
        {
            ecgQueue.Clear();
            DDmax = 0.0;
            DDmin = 0.0;
            AAmax = 0.0;
            Rate = null;
            State = RateState.Uninitialized;
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
