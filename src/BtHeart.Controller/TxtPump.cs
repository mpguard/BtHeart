using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BtHeart.Controller
{
    public class TxtPump : IPump
    {
        private List<byte> txtBuffer = new List<byte>();
        private string txt;

        public TxtPump()
        {

        }

        public TxtPump(string txt)
        {
            this.txt = txt.Trim();
        }

        public void Open()
        {
            LoadTxt();
            OnReceive(txtBuffer.ToArray());
        }

        public void Close()
        {
            txtBuffer.Clear();
        }

        private void LoadTxt()
        {
            string[] lines = txt.Split(new string[] { " " }, StringSplitOptions.None);
            if (lines == null)
                throw new ArgumentNullException("txt");
            List<byte> bytes = new List<byte>();
            foreach (var line in lines)
            {
                string byteStr = line;
                var byteSequence = byteStr.Trim().Split(' ').Select(s => Convert.ToByte(s.Trim(), 16));
                txtBuffer.AddRange(byteSequence);
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
