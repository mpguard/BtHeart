using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Filtering;
using MathNet.Filtering.FIR;
using MathNet.Filtering.Median;

namespace BtHeart.Controller
{
    /// <summary>
    /// 平均滑动滤波
    /// </summary>
    public class AvgFilterProcess : IProcess
    {
        private Queue<double> meanQueue;
        public bool Enabled { get; set; }

        public AvgFilterProcess()
        {
            meanQueue = new Queue<double>();
        }

        public void Init()
        {
            meanQueue.Clear();
        }

        public double Process(double rawData)
        {
            if (!Enabled)
                return rawData;
            return GetMean(rawData);
        }

        private double GetMean(double rawData)
        {
            meanQueue.Enqueue(rawData);
            if (meanQueue.Count == 12)
            {
                meanQueue.Dequeue();
                return meanQueue.Average();
            }
            return meanQueue.Average();
            //return 0;
        }
    }

    /// <summary>
    /// 中值滤波
    /// </summary>
    public class MedianFilterProcess:IProcess
    {
        private OnlineMedianFilter MedianFilter;

        public bool Enabled { get; set; }

        public MedianFilterProcess()
        {
            MedianFilter = new OnlineMedianFilter(425);
        }

        public void Init()
        {
            MedianFilter.Reset();
        }

        public double Process(double rawData)
        {
            if (!Enabled)
                return rawData;
            return MedianFilter.ProcessSample(rawData);
        }
    }

    /// <summary>
    /// 低通滤波
    /// </summary>
    public class LowFirFilterProcess:IProcess
    {
        private OnlineFirFilter LowFirFilter;
        public bool Enabled { get; set; }

        public LowFirFilterProcess()
        {
            var lowCoefficient = FirCoefficients.LowPass(HeartContext.F, 37, 40);
            LowFirFilter = new OnlineFirFilter(lowCoefficient);
        }

        public void Init()
        {
            LowFirFilter.Reset();
        }

        public double Process(double rawData)
        {
            if (!Enabled) 
                return rawData;
            double lowecg = LowFirFilter.ProcessSample(rawData);
            return lowecg;
        }
    }

    /// <summary>
    /// 带通滤波
    /// </summary>
    public class BandPassFirFilterProcess : IProcess 
    {
        private OnlineFirFilter BandFirFilter;
        public bool Enabled { get; set; }

        public BandPassFirFilterProcess()
        {
            var bandCoefficient = FirCoefficients.BandPass(HeartContext.F, 2, 37, 50);
            BandFirFilter = new OnlineFirFilter(bandCoefficient);
        }

        public void Init()
        {
            BandFirFilter.Reset();
        }

        public double Process(double rawData)
        {
            if (!Enabled) 
                return rawData;
            double bandecg = BandFirFilter.ProcessSample(rawData);
            return bandecg;
        }
    }

    /// <summary>
    /// 带阻滤波器
    /// </summary>
    public class BandStopFirFilterProcess:IProcess
    {
        private OnlineFirFilter BandFirFilter;
        public bool Enabled { get; set; }

        public BandStopFirFilterProcess()
        {
            var bandCoefficient = FirCoefficients.BandStop(HeartContext.F, 45, 50, 40);
            BandFirFilter = new OnlineFirFilter(bandCoefficient);
        }

        public void Init()
        {
            BandFirFilter.Reset();
        }

        public double Process(double rawData)
        {
            if (!Enabled) 
                return rawData;
            double bandecg = BandFirFilter.ProcessSample(rawData);
            return bandecg;
        }
    }

    public class MyFirFilterProcess:IProcess
    {
        private List<double> firLList;
        private List<double> firHList;

        private double[] coeffshpL = {
		0.0034484059701245947f, 0.0023130087020212247f,-0.00044880860174300165f,
		-0.0047735952308836293f,-0.010073069580546861f,-0.015216272565267233f,
		-0.018649250414531603f,-0.018648444196040958f,-0.013668760470431408f,
		-0.0027177399237866954f, 0.014328291745632047f, 0.036546325548177376f, 
		0.061983239825546141f, 0.087883347748451107f, 0.11109679303698015f, 
		0.12859611901008677f, 0.13800040939621194f };
        private double[] xhpL = new double[34];

        static double[] coeffshpH = {0.017469849611047396f,0.018779590490975825f,0.020258958333441587f,0.021945408418679712f,0.023888397211794098f,
		0.026154600656794796f,0.028836114325469047f,0.032063832576597985f,0.036030271792139773f,0.041030652161473988f,0.047541901064669538f,
		0.056387841907242407f,0.069124634663347298f,0.089086971543241994f,0.12494514213171981f ,0.20849035886030709f ,0.62584398748168513f  };  //34阶滤波器系数
        public static double[] xhpH = new double[34];

        public bool Enabled { get; set; }

        public MyFirFilterProcess()
        {
            firLList = new List<double>();
            firHList = new List<double>();
        }

        public void Init()
        {
            for (int i = 0; i < xhpL.Length; i++)
                xhpL[i] = 0;
            for (int i = 0; i < xhpH.Length; i++)
                xhpH[i] = 0;
            firLList.Clear();
            firHList.Clear();
        }

        public double Process(double rawData)
        {
            if (!Enabled)
                return rawData;

            firLList.Add(rawData);
            if (firLList.Count == 34)
            {
                for (int i = 0; i < xhpL.Length; i++)
                    xhpL[i] = firLList[i];
                double firecg = LowFilter(xhpL[xhpL.Length - 1]);
                firHList.Add(firecg);
                firLList.RemoveAt(0);
            }
            if (firHList.Count == 34)
            {
                for (int i = 0; i < xhpH.Length; i++)
                    xhpH[i] = firHList[i];
                double firecg = Fir.Filterhp33(xhpH[xhpH.Length - 1]);

                firHList.RemoveAt(0);
                return firecg;
            } 
            return rawData;
        }

        private double LowFilter(double samplehp)
        {
            double z = 0.0;
            int i;

            xhpL[0] = samplehp;

            for (i = 0; i < 17; i++)
            {
                z += (coeffshpL[i]) * (xhpL[i] - xhpL[33 - i]);
            }

            for (i = 32; i >= 0; i--)
            {
                xhpL[i + 1] = xhpL[i];
            }

            return z;
        }

        private double HighFilter(double samplehp)
        {
            double z = 0.0f;
            int i;

            xhpH[0] = samplehp;

            for (i = 0; i < 17; i++)
            {
                z += (coeffshpH[i]) * (xhpH[i] + xhpH[33 - i]);
            }

            for (i = 32; i >= 0; i--)
            {
                xhpH[i + 1] = xhpH[i];
            }                           //一次移动输入量x，即x0->x1......

            return z;                                  //返回值     
        }
    }
}
