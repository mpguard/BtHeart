using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtHeart.Controller
{
    public interface IAnalyze
    {
        void Start();
        void Stop();

        event Action<double> Analyzed;
    }
}
