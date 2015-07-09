using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Filtering;
using MathNet.Filtering.FIR;

namespace BtHeart.Controller
{
    public class Fir
    {
        public static void FIRfilter(double[] fs, int num, int length)
        {
            int i, j, win;
            double[] x = new double[num];
            double sum = 0.0f;
            double[] h = {-0.0023f,-0.0036f,-0.0056f,-0.0056f,0.0010f,0.0185f,
				0.0482f,0.0866f,0.1255f,0.1545f,
				0.1653f,0.1545f,0.1255f,0.0866f,0.0482f,0.0185f,
				0.0010f,-0.0056f,-0.0056f,-0.0036f,-0.0023f};
            win = num - length;
            for (i = 0; i < num; i++)
                x[i] = fs[i];
            for (i = 0; i < num; i++)
            {
                sum = 0.0f;
                for (j = 0; j < length; j++)
                {
                    if (i > j)
                        sum += h[j] * x[i - j];
                }
                fs[i] = sum;
            }
            for (i = 0; i < win; i++)
            {
                fs[i] = fs[i + length];
                //考虑到前20个点为不完全累加和，故抛去前20个点。
            }
        }

        /*static double[] coeffshp = {0.003445f,0.00231f,-0.00045f,-0.0048f,-0.0100f,
            -0.0152f,-0.0186f,-0.0186f,-0.0137f,-0.0027f,0.0143f,0.0365f,0.0619f,0.0879f,0.1111f,
            0.1286f,0.1380f};/*,0.1380f,0.1286f,0.1111f,0.0879f,0.0619f,0.0365f,
            0.0143f,-0.0027f,-0.0137f,-0.0186f,-0.0186f,-0.0152f,-0.0100f,0.0048f,
            -0.00045f,0.00231f,0.003445f};  //33阶滤波器系数*/

        static double[] coeffshpL = {
		0.0034484059701245947f, 0.0023130087020212247f,-0.00044880860174300165f,
		-0.0047735952308836293f,-0.010073069580546861f,-0.015216272565267233f,
		-0.018649250414531603f,-0.018648444196040958f,-0.013668760470431408f,
		-0.0027177399237866954f, 0.014328291745632047f, 0.036546325548177376f, 
		0.061983239825546141f, 0.087883347748451107f, 0.11109679303698015f, 
		0.12859611901008677f, 0.13800040939621194f };

        public static double[] xhpL = new double[34];
        public static double Filterhp33(double samplehp)
        {
            double z = 0.0f;
            int i;

            xhpL[0] = samplehp;

            for (i = 0; i < 17; i++)
            {
                z += (coeffshpL[i]) * (xhpL[i] + xhpL[33 - i]);
            }

            for (i = 32; i >= 0; i--)
            {
                xhpL[i + 1] = xhpL[i];
            }                           //一次移动输入量x，即x0->x1......

            return z;                                  //返回值     

        }

        //FIR34 HP 1HZ
        static double[] coeffshpH = {0.017469849611047396f,0.018779590490975825f,0.020258958333441587f,0.021945408418679712f,0.023888397211794098f,
		0.026154600656794796f,0.028836114325469047f,0.032063832576597985f,0.036030271792139773f,0.041030652161473988f,0.047541901064669538f,
		0.056387841907242407f,0.069124634663347298f,0.089086971543241994f,0.12494514213171981f ,0.20849035886030709f ,0.62584398748168513f  };  //34阶滤波器系数
        public static double[] xhpH = new double[34];
        public static double Filterhp1hz(double samplehp)
        {
            double z = 0.0;
            int i;

            xhpH[0] = samplehp;

            for (i = 0; i < 17; i++)
            {
                z += (coeffshpH[i]) * (xhpH[i] - xhpH[33 - i]);
            }

            for (i = 32; i >= 0; i--)
            {
                xhpH[i + 1] = xhpH[i];
            }

            return z;
        }
    }

    public class FirFilter
    {
        static OnlineFirFilter LowFirFilter;
        static OnlineFirFilter HighFirFilter;

        static FirFilter()
        {
            var lc = FirCoefficients.LowPass(500, 37, 17);
            Console.WriteLine("低通："+string.Join(",",lc));
            LowFirFilter = new OnlineFirFilter(lc);

            var hc = FirCoefficients.HighPass(500, 1, 17);
            HighFirFilter = new OnlineFirFilter(hc);
            Console.WriteLine("高通：" + string.Join(",", hc));
        }

        public static void Init()
        {
            LowFirFilter.Reset();
            HighFirFilter.Reset();
        }

        public static double LowFilter(double sample)
        {
            return LowFirFilter.ProcessSample(sample);
        }

        public static double HighFilter(double sample)
        {
            return LowFirFilter.ProcessSample(sample);
        }
    }
}
