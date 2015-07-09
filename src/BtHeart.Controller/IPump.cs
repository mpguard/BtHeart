using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtHeart.Controller
{
    public interface IPump
    {
        void Open();
        void Close();
        event Action<byte[]> Received;
    }
}
