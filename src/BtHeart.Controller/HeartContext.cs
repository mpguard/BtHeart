using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace BtHeart.Controller
{
    public class HeartContext
    {
        public const int F = 500; // 采样频率
        public const int JumpSec = 3; // 初始跳过3秒
        public const int ThesoldSec = 7; // 初始阈值需5秒确定
        public const int AdjustSec = 2;// 动态调整阈值，心率刷新每2秒
        public const double RefractorySec = 0.1; // 跳过200ms不应期检测
        public static double Th = 4; // 阈值比例系数

        private IPump Pump;
        private IAnalyze Analyze;
        private List<IProcess> Processes;
        private IProcess AvgFilter;
        private IProcess FirFilter;
        private IProcess IirFilter;
        private IProcess MedianFilter;
        private IProcess BandStopFilter;

        private HeartRate Rate;

        private ConcurrentQueue<double> analyzedQueue = new ConcurrentQueue<double>();
        private ConcurrentQueue<double> ecgQueue = new ConcurrentQueue<double>();
        private BackgroundWorker worker = new BackgroundWorker();
        private object syncObj = new object();

        public event Action<EcgPacket> Processed;
        public event Action<byte[]> ComReceived;
        public event Action<int?> RateAnalyzed;

        public HeartContext()
        {
            worker.DoWork += worker_DoWork;
            worker.WorkerSupportsCancellation = true;

            Pump = new Com();
            //Pump = new RandomPump();
            //Pump = new FilePump();
            //Pump.Received += Pump_Received;

            Analyze = new ComDataAnalyze(Pump);
            Analyze.Analyzed += Analyze_Analyzed;

            Rate = new DifferenceHeartRate(this);
            Rate.RateAnalyzed += Rate_RateAnalyzed;

            AvgFilter = new AvgFilterProcess();
            //FirFilter = new MyFirFilterProcess();
            //FirFilter = new LowFirFilterProcess();
            FirFilter = new BandPassFirFilterProcess();
            BandStopFilter = new BandStopFirFilterProcess();
            MedianFilter = new MedianFilterProcess();

            Processes = new List<IProcess>()
            {
                //AvgFilter,
                FirFilter,
                //BandStopFilter,
                //MedianFilter,
            };
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(!worker.CancellationPending)
            {
                if (analyzedQueue.Count >= 12)
                {
                    var data = analyzedQueue.Take(12).ToArray();
                    EcgPacket packet = new EcgPacket() { Data = data };
                    OnProcess(packet);
                    analyzedQueue.RemoveRange(12);
                }
                Thread.Sleep(2);
            }
        }
 
        private void Analyze_Analyzed(double data)
        {
            var pdata = ProcessData(data);
            analyzedQueue.Enqueue(pdata);
            //ecgQueue.Enqueue(pdata);
        }

        private void Pump_Received(byte[] buffer)
        {
            OnReceive(buffer);
        }

        private double ProcessData(double data)
        {
            Processes.ForEach(p => 
            {
                data = p.Process(data);
            });
            return data;
        }

        private void Rate_RateAnalyzed(int? rate)
        {
            OnRateAnalyze(rate);
        }

        public void Start()
        {
            Processes.ForEach(p => p.Init());

            Pump.Open();
            Analyze.Start();
            Rate.Start();

            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        public void Stop()
        {
            Pump.Close();
            Analyze.Stop();
            Rate.Stop();

            if (worker.IsBusy)
                worker.CancelAsync();
            analyzedQueue.Clear();
            ecgQueue.Clear();
        }

        private void OnProcess(EcgPacket packet)
        {
            if (Processed != null)
                Processed(packet);
        }

        private void OnReceive(byte[] buffer)
        {
            if (ComReceived != null)
                ComReceived(buffer);
        }

        private void OnRateAnalyze(int? rate)
        {
            if (RateAnalyzed != null)
                RateAnalyzed(rate);
        }

        public void SaveTxt(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                var sw = new StreamWriter(fs);
                string fileContent = string.Join(Environment.NewLine,ecgQueue.ToArray());
                sw.Write(fileContent);
                sw.Close();
            }
        }

        #region 滤波开关

        public void EnabledFilter(bool flag)
        {
            Processes.ForEach(p => p.Enabled = flag);
        }
        #endregion 
    }
}
