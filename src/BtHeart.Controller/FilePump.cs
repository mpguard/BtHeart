using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace BtHeart.Controller
{
    public class FilePump:IPump
    {
        private List<byte> fileBuffer = new List<byte>();
        BackgroundWorker worker = new BackgroundWorker();
        Random random = new Random();

        public FilePump()
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

        private void LoadFile()
        {
            fileBuffer.Clear();
            var txt = File.ReadAllText("test.txt");

            string[] lines = txt.Split(new string[] { "\n" }, StringSplitOptions.None);
            if (lines == null)
                throw new ArgumentNullException("txt");
            List<byte> bytes = new List<byte>();
            foreach (var line in lines)
            {
                string byteStr = line;
                var byteSequence = byteStr.Trim().Split(' ').Select(s => Convert.ToByte(s.Trim(), 16));
                fileBuffer.AddRange(byteSequence);
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadFile();
            //while(!worker.CancellationPending)
            //{
            //    for(int i = 0; i < fileBuffer.Count-4;i+=4)
            //    {
            //        byte[] buffer = new byte[4];
            //        buffer[0] = fileBuffer[i];
            //        buffer[1] = fileBuffer[i+1];
            //        buffer[2] = fileBuffer[i+2];
            //        buffer[3] = fileBuffer[i+3];
            //        OnReceive(buffer);
            //        Thread.Sleep(2);
            //    }
            //}
            for (int i = 0; i < fileBuffer.Count - 4; i += 4)
            {
                byte[] buffer = new byte[4];
                buffer[0] = fileBuffer[i];
                buffer[1] = fileBuffer[i + 1];
                buffer[2] = fileBuffer[i + 2];
                buffer[3] = fileBuffer[i + 3];
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
