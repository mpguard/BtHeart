using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace BtHeart.Controller
{
    public class Com:IPump
    {
        private SerialPort comPort;

        public Com()
        {
            
        }

        public void Open()
        {
            if (comPort == null)
            {
                comPort = new SerialPort();
                comPort.PortName = ConfigurationManager.AppSettings["portName"];
                comPort.BaudRate = Int32.Parse(ConfigurationManager.AppSettings["baudRate"]);
                comPort.DataBits = Int32.Parse(ConfigurationManager.AppSettings["dataBits"]);
                comPort.StopBits = StopBits.One;
                comPort.Parity = Parity.None;
                comPort.Encoding = Encoding.Default;
                comPort.DataReceived += DataReceived;
            }
            if(!comPort.IsOpen)
                comPort.Open();
        }

        public void Close()
        {
            if (comPort != null)
            {
                comPort.DataReceived -= DataReceived;
                comPort.Close();
                comPort.Dispose();
                comPort = null;
            }
        }

        public event Action<byte[]> Received;

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!comPort.IsOpen)
                return;

            int bytes = comPort.BytesToRead;
            byte[] buffer = new byte[bytes];
            int readBytes = comPort.Read(buffer, 0, bytes);
            buffer = buffer.Take(readBytes).ToArray();
            if (readBytes > 0)
            {
                OnReceive(buffer);
            }
            Thread.Sleep(2);
        }

        private void OnReceive(byte[] buffer)
        {
            if (Received != null)
                Received(buffer);
        }
    }
}
