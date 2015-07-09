using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace BtHeart.Controller
{
    public class RandomPump:IPump
    {
        BackgroundWorker worker = new BackgroundWorker();
        Random random = new Random();

        public RandomPump()
        {
            worker.DoWork += worker_DoWork;
            worker.WorkerSupportsCancellation = true;
        }

        public void Open()
        {
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        public void Close()
        {
            if(worker.IsBusy)
                worker.CancelAsync();
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            byte i = 0;
            while(!worker.CancellationPending)
            {
                byte[] buffer = new byte[4];
                buffer[0] = 0x55;
                buffer[1] = 0x55;
                //buffer[2] = (byte)random.Next(0, 16);
                //buffer[3] = (byte)random.Next(0, 256);
                buffer[2] = 0x00;
                buffer[3] = i++;
                if (i >= 255)
                    i = 0;
                OnReceive(buffer);
                Thread.Sleep(2);
            }
        }

        private void OnReceive(byte[] buffer)
        {
            if (Received != null)
                Received(buffer);
        }

        public event Action<byte[]> Received;
    }
}
