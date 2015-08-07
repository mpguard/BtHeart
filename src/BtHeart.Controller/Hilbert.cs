using MathNet.Numerics.IntegralTransforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BtHeart.Controller
{
    public class Hilbert
    {
        public static Complex[] MatlabHilbert(double[] xr)
        {
            var x = (from sample in xr select new Complex(sample, 0)).ToArray();
            Fourier.BluesteinForward(x, FourierOptions.Default);
            var h = new double[x.Length];
            var fftLengthIsOdd = (x.Length | 1) == 1;
            if (fftLengthIsOdd)
            {
                h[0] = 1;
                for (var i = 1; i < xr.Length / 2; i++) h[i] = 2;
            }
            else
            {
                h[0] = 1;
                h[(xr.Length / 2)] = 1;
                for (var i = 1; i < xr.Length / 2; i++) h[i] = 2;
            }
            for (var i = 0; i < x.Length; i++) x[i] *= h[i];
            Fourier.BluesteinInverse(x, FourierOptions.Default);
            return x;
        }
    }
}
