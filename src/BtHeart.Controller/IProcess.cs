using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtHeart.Controller
{
    public interface IProcess
    {
        bool Enabled { get; set; }
        void Init();
        double Process(double rawData);
    }
}
