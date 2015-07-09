using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BtHeart.Controller
{
    public class DataAnalyze:IAnalyze
    {
        protected ConcurrentQueue<byte> bufferQueue = new ConcurrentQueue<byte>();
        private BackgroundWorker worker = new BackgroundWorker();

        private IPump Pump;

        public DataAnalyze(IPump pump)
        {
            Pump = pump;
            Pump.Received += Pump_Received;

            worker.DoWork += worker_DoWork;
            worker.WorkerSupportsCancellation = true;
        }

        public void Start()
        {
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        public void Stop()
        {
            if(worker.IsBusy)
                worker.CancelAsync();
            bufferQueue.Clear();
        }

        public event Action<double> Analyzed;

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(!worker.CancellationPending)
            {
                Analyze();
                Thread.Sleep(2);
            }
        }

        protected virtual void Analyze()
        {

        }

        protected void OnAnalyze(double data)
        {
            if (Analyzed != null)
                Analyzed(data);
        }

        private void Pump_Received(byte[] buffer)
        {
            foreach (var b in buffer)
                bufferQueue.Enqueue(b);
        }
    }

    public class ComDataAnalyze:DataAnalyze
    {
        public ComDataAnalyze(IPump pump)
            :base(pump)
        {

        }

        protected override void Analyze()
        {
            bool flag = false;
            while (bufferQueue.Count >= 4 && !flag)
            {
                byte head1 = 0x00,head2 = 0x00;
                if (bufferQueue.TryDequeue(out head1) &&
                    bufferQueue.TryDequeue(out head2))
                {
                    if (head1 == 0x55 && head2 == 0x55)
                    {
                        byte bh = 0x00, bl = 0x00;
                        if (bufferQueue.TryDequeue(out bh) &&
                            bufferQueue.TryDequeue(out bl))
                        {
                            int high = (int)bh;
                            int low = (int)bl;
                            double hv = ((high << 8) + low) * 3.3 / 4095;
                            OnAnalyze(hv);
                        }
                    }
                }
            }
        }
    }
}
