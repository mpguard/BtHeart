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
        private IPump Pump;
        private IAnalyze Analyze;
        private List<IProcess> Processes;
        private IProcess AvgFilter;
        private IProcess FirFilter;
        private IProcess MedianFilter;

        private ConcurrentQueue<double> analyzedQueue = new ConcurrentQueue<double>();
        private ConcurrentQueue<double> ecgQueue = new ConcurrentQueue<double>();
        private BackgroundWorker worker = new BackgroundWorker();
        private object syncObj = new object();

        public event Action<EcgPacket> Processed;
        public event Action<byte[]> ComReceived;

        public HeartContext()
        {
            worker.DoWork += worker_DoWork;
            worker.WorkerSupportsCancellation = true;

            //Pump = new Com();
            //Pump = new RandomPump();
            Pump = new FilePump();
            Pump.Received += Pump_Received;

            Analyze = new ComDataAnalyze(Pump);
            Analyze.Analyzed += Analyze_Analyzed;

            AvgFilter = new AvgFilterProcess();
            //FirFilter = new MyFirFilterProcess();
            FirFilter = new FirFilterProcess();
            MedianFilter = new MedianFilterProcess();

            Processes = new List<IProcess>()
            {
                AvgFilter,
                FirFilter,
                MedianFilter,
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
            ecgQueue.Enqueue(pdata);
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

        public void Start()
        {
            Processes.ForEach(p => p.Init());

            Pump.Open();
            Analyze.Start();

            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        public void Stop()
        {
            Pump.Close();
            Analyze.Stop();

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
            AvgFilter.Enabled = flag;
            FirFilter.Enabled = flag;
        }
        #endregion 
    }
}
