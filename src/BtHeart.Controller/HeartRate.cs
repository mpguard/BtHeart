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
        private double D1 = 0.0; // 正差分阈值
        private double D2 = 0.0; // 负差分阈值
        private double D3 = 0.0; // 幅度阈值
        private double RR = 0.0; // RR间期
        private double HR = 0.0; // R波幅度
        private int LastRIndex = 0; // 最后R波位置
        private List<double> DList = new List<double>();
        protected int LastRate = -1; // 上一次心率
        protected int? NewRate = null; // 当前心率
        protected int ErrorCnt = 0; // 异常次数
        protected int WarningCnt = 0; // 数值变化过大的警告
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
            var yList = new List<double>();
            for (int i = 1; i < x.Count; i++)
            {
                double y = x[i] - x[i - 1];
                y = Math.Abs(y);
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
                Console.WriteLine((ecgQueue.Max() - ecgQueue.Min()).ToString());
                var ecgList = ecgQueue.ToList();
                var diffList = Diff(ecgList);

                int size = HeartContext.ThesoldSec * HeartContext.F / HeartContext.ThesoldSec;
                double[] ddThesold = new double[HeartContext.ThesoldSec];
                for (int i = 0; i < HeartContext.ThesoldSec; i++)
                {
                    var sequence = diffList.Skip(i * size).Take(size);
                    ddThesold[i] = diffList.Max();
                }

                // 确定初始阈值
                DList.AddRange(ddThesold);
                // 去除一个最高值和一个最低值，再取平均
                DList = DList.OrderByDescending(e => e).ToList();
                DList.RemoveAt(DList.Count - 1); // 去除最低值
                DList.RemoveAt(0); // 去除最高值
                double m0 = DList.Average();
                D1 = m0 * 0.4;
                D2 = m0 * 0.5;
                D3 = m0 * 2.0 / 9;

                List<int> posR = new List<int>(); // R值位置
                for (int i = 1; i < ecgList.Count; i++)
                {
                    double delta = ecgList[i] - ecgList[i - 1]; delta = Math.Abs(delta);
                    if (delta > D1)
                    {
                        i++; if (i >= ecgQueue.Count) break;
                        delta = ecgList[i] - ecgList[i - 1]; delta = Math.Abs(delta);
                        if (delta > D2)
                        {
                            i++; if (i >= ecgQueue.Count) break;
                            int endIndex = i + (int)(0.1 * HeartContext.F);
                            endIndex = endIndex >= ecgList.Count ? ecgList.Count : endIndex;
                            for (; i < endIndex; i++)
                            {
                                delta = ecgList[i] - ecgList[i - 1]; 
                                if (Math.Abs(delta) > D3 && delta < 0) // 找到R波点
                                {
                                    // 确定R波位置和RR间期
                                    int r = i - 1;
                                    posR.Add(r);
                                    i += (int)(0.1 * HeartContext.F);
                                }
                            }
                        }
                    }
                }

                // 确定RR间期和R波幅度
                if (posR.Count > 1)
                {
                    for (int i = 1; i < posR.Count; i++)
                    {
                        RR += posR[i] - posR[i - 1];
                        HR += ecgList[posR[i]];
                    }
                    RR /= (posR.Count - 1);
                    RR *= 1.1;
                    HR /= (posR.Count - 1);
                    LastRIndex = posR.Last() - ecgList.Count;
                    State = RateState.Initialized;
                }
                else // 如果RR间期无法确定，重新计算阈值
                {
                    Clear();
                    State = RateState.Uninitialized;
                    Console.WriteLine("重新计算阈值");
                }

                diffList.Clear();
                ecgList.Clear();
                ecgQueue.Clear();
            }
        }

        // 计算心率
        protected virtual bool CalcRate()
        {
            int allSize = HeartContext.AdjustSec*HeartContext.F;
            if (ecgQueue.Count >= allSize)
            {
                Console.WriteLine((ecgQueue.Max() - ecgQueue.Min()).ToString());
                var ecgList = ecgQueue.ToList();
                List<int> posR = new List<int>(); // R值位置

                int i = 1;
                for (i = 1; i < ecgList.Count; i++)
                {
                    double delta = ecgList[i] - ecgList[i - 1]; delta = Math.Abs(delta);
                    if(delta > D1)
                    {
                        i++; if (i >= ecgQueue.Count) break;
                        delta = ecgList[i] - ecgList[i - 1]; delta = Math.Abs(delta);
                        if(delta > D2)
                        {
                            i++; if (i >= ecgQueue.Count) break;
                            int endIndex = i+(int)(0.1 * HeartContext.F);
                            endIndex = endIndex >= ecgList.Count?ecgList.Count:endIndex;
                            for (; i < endIndex; i++)
                            {
                                delta = ecgList[i] - ecgList[i - 1];
                                if(Math.Abs(delta) > D3 && delta < 0 &&
                                    ecgList[i-1] > 0.8*HR && ecgList[i-1] < 1.2*HR) // 找到R波点
                                {
                                    // 确定R波位置和RR间期
                                    int r = i - 1; 
                                    posR.Add(r);

                                    int newRR = posR.Last() - LastRIndex;
                                    if (newRR >= 0.6*RR && newRR <= 1.6 * RR)
                                    {
                                        RR = (int)((RR + newRR) / 2);
                                        
                                        // 更新阈值
                                        DList = DList.OrderByDescending(e => e).ToList();
                                        DList[DList.Count - 1] = delta;
                                        double n0 = DList.Average();
                                        D1 = 0.25 * n0 + 0.1 * D1; 
                                        D2 = 0.2 * n0 + 0.2 * D1;
                                        D3 = 0.1 * n0 + 0.1 * D1;
                                    }
                                    else if (newRR < 0.6 * RR) // 检测多检,论文没有说明，暂时留空
                                    {
                                        
                                    }
                                    else if (newRR > 1.6 * RR) // 检测漏检,包括倒置R波
                                    {
                                        int pr = LastRIndex < 0 ? 0 : LastRIndex;
                                        int start = pr + (int)(0.7 * RR);
                                        int end = pr + (int)(1.2 * RR);
                                        if (end - start <= 0) // 没有序列
                                            break;

                                        // 此范围内找幅值最大的点
                                        var tempList = ecgList.Skip(start).Take(end - start).ToList();
                                        var maxHR = ecgList.Max();
                                        if (maxHR > 0.8 * HR && maxHR < 1.2 * HR)
                                        {
                                            var maxIndex = tempList.IndexOf(maxHR);
                                            i = maxIndex;
                                            r = i + 1;
                                            posR.Add(r);
                                            newRR = posR.Last() - LastRIndex;
                                            RR = (int)((RR + newRR) / 2);
                                        }
                                        else // 如果幅值最大点不满足，则找幅值最小点
                                        {
                                            var minIndex = tempList.IndexOf(tempList.Min());
                                            i = minIndex;
                                            r = i + 1;
                                            posR.Add(r);
                                            newRR = posR.Last() - LastRIndex;
                                            RR = (int)((RR + newRR) / 2);
                                        }
                                    }
                                    LastRIndex = posR.Last(); // 更新R值位置
                                    i += (int)(0.1 * HeartContext.F);
                                }
                            }
                        }
                    }
                }

                double rr = 0;
                if (posR.Count == 0)
                {
                    NewRate = null;
                }
                if (posR.Count == 1 && RR > 0)
                {
                    rr = 60 * HeartContext.F / RR;
                    NewRate = Convert.ToInt32(rr);
                }
                else if (posR.Count > 1)
                {
                    if (posR.Last() - posR.First() == 0)
                        NewRate = null;
                    else
                    {
                        rr = 60 * HeartContext.F * (posR.Count - 1) / (double)(posR.Last() - posR.First());
                        NewRate = (int)(rr);
                    }
                }
                LastRIndex -= ecgList.Count;
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
                    // 心率变化太快
                    if(Math.Abs(NewRate.Value - LastRate) >= 10)
                    {
                        NewRate = (NewRate + LastRate) / 2;
                        WarningCnt++;
                    }
                    else if (Math.Abs(NewRate.Value - LastRate) < 5)
                    {
                        if (WarningCnt > 0)
                            WarningCnt--;
                    }

                    // 心率超过范围
                    if (NewRate <= 30) 
                    {
                        NewRate = 30;
                        ErrorCnt++;
                    }
                    else if(NewRate >= 240)
                    {
                        NewRate = 240;
                        ErrorCnt++;
                    }
                }
                LastRate = NewRate.Value; // 更新旧心率值
            }
        }

        // 检测错误次数
        private void CheckError()
        {
            if (ErrorCnt >= 2 || WarningCnt >= 5)
            {
                State = RateState.Error;
            }
        }

        // 修正阈值
        private void CheckTh()
        {
            if (ErrorCnt >= 1)
            {
                if (!NewRate.HasValue)
                {
                    HeartContext.Th *= 2;
                    HeartContext.Th = HeartContext.Th >= 16 ? 2 : HeartContext.Th;
                }
                else if (NewRate >= 240)
                {
                    HeartContext.Th += 2;
                    HeartContext.Th = HeartContext.Th <= 2 ? 2 : HeartContext.Th;
                }
                else if (NewRate <= 30)
                {
                    HeartContext.Th -= 2;
                    HeartContext.Th = HeartContext.Th >= 16 ? 2 : HeartContext.Th;
                }
            }
        }

        // 清空参数
        private void Clear()
        {
            DList.Clear();
            ecgQueue.Clear();
            D1 = 0.0;
            D2 = 0.0;
            D3 = 0.0;
            ErrorCnt = 0;
            WarningCnt = 0;
            LastRate = -1;
            NewRate = null;
            State = RateState.Jumped;
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
