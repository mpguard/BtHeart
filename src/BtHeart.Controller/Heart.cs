using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BtHeart.Controller
{
    public class Heart
    {
        public static List<double> Datas = new List<double>();

        public static void ReadTextOriginal(string txt)
        {
            Datas.Clear();

            string[] lines = txt.Split(new string[] { "\n" }, StringSplitOptions.None);
            if (lines == null)
                throw new ArgumentNullException("txt");
            List<byte> bytes = new List<byte>();
            foreach (var line in lines)
            {
                string byteStr = line;
                var byteSequence = byteStr.Trim().Split(' ').Select(s => Convert.ToByte(s.Trim(), 16));
                bytes.AddRange(byteSequence);
            }

            Datas = Parse(bytes);
        }

        public static void ReadText(string txt)
        {
            Datas.Clear();

            string[] lines = txt.Split(new string[] { "\n" }, StringSplitOptions.None);
            if (lines == null)
                throw new ArgumentNullException("txt");
            List<byte> bytes = new List<byte>();
            foreach (var line in lines)
            {
                string byteStr = line;
                var byteSequence = byteStr.Trim().Split(' ').Select(s => Convert.ToByte(s.Trim(),16));
                bytes.AddRange(byteSequence);
            }

            //Datas = Parse(bytes);
            var ecgDatas = Parse(bytes);
            ecgDatas = MeanFilter(ecgDatas);
            Datas = Filter(ecgDatas);
        }

        public static void ReadTextEx(string txt)
        {
            Datas.Clear();

            string[] lines = txt.Split(new string[] { "\n" }, StringSplitOptions.None);
            if (lines == null)
                throw new ArgumentNullException("txt");
            List<byte> bytes = new List<byte>();
            foreach (var line in lines)
            {
                string byteStr = line;
                var byteSequence = byteStr.Trim().Split(' ').Select(s => Convert.ToByte(s.Trim(), 16));
                bytes.AddRange(byteSequence);
            }

            //Datas = Parse(bytes);
            var ecgDatas = Parse(bytes);
            ecgDatas = MeanFilter(ecgDatas);
            Datas = FilterEx(ecgDatas);
        }

        /// <summary>
        /// 根据蓝牙协议解析数据
        /// </summary>
        /// <returns></returns>
        private static List<double> Parse(List<byte> bytes)
        {
            List<double> datas = new List<double>();
            while (bytes.Count >= 4)
            {
                byte head1 = bytes[0];
                byte head2 = bytes[1];
                if (head1 == 0x55 && head2 == 0x55) // 数据头字节和序号字节，跳过
                {
                    bytes.RemoveRange(0, 2); // 移除头元素
                    if (bytes.Count >= 2)
                    {
                        int high = (int)bytes[0];
                        int low = (int)bytes[1];
                        int data = (high << 8) + low;
                        double hv = (double)data * 3.3 / 4095;
                        datas.Add(hv);
                    }
                    bytes.RemoveRange(0, 2); // 移除头元素
                }
                else
                {
                    bytes.RemoveAt(0);
                }
            }

            return datas;
        }

        /// <summary>
        /// 平均滤波
        /// </summary>
        /// <param name="ecgs"></param>
        /// <returns></returns>
        public static List<double> MeanFilter(List<double> ecgs)
        {
            var meanList = new List<double>();
            var ecgList = new List<double>();
            foreach (var ecg in ecgs)
            {
                meanList.Add(ecg);
                if (meanList.Count == 12)
                {
                    meanList.RemoveAt(0);
                    ecgList.Add(meanList.Average());
                }
            }
            return ecgList;
        }

        /// <summary>
        /// 低通+高通滤波
        /// </summary>
        /// <returns></returns>
        public static List<double> Filter(List<double> ecgs)
        {
            var firHList = new List<double>();
            var firLList = new List<double>();
            var ecgList = new List<double>();
            foreach(var ecg in ecgs)
            {
                firHList.Add(ecg);

                if(firHList.Count == 34)
                {
                    for(int i = 0;i <Fir.xhpH.Length;i++)
					    Fir.xhpH[i]= firHList[i]; 
				    double firecg = Fir.Filterhp1hz(Fir.xhpH[Fir.xhpH.Length-1]);
				    //ecgList.add(firecg); 
				    //ecgList.add(firecg+150);
				    firLList.Add(firecg);
				    firHList.RemoveAt(0);
                }
                if(firLList.Count == 34)
                {
                    for(int i = 0;i <Fir.xhpL.Length;i++)
						Fir.xhpL[i]= firLList[i]; 
					double firecg = Fir.Filterhp33(Fir.xhpL[Fir.xhpL.Length-1]);
					ecgList.Add(firecg); 
					//firHList.add(firecg);
                    firLList.RemoveAt(0);
                }
            }

            return ecgList;
        }

        public static List<double> FilterEx(List<double> ecgs)
        {
            var firHList = new List<double>();
            var firLList = new List<double>();
            var ecgList = new List<double>();
            foreach (var ecg in ecgs)
            {
                firHList.Add(ecg);

                if (firHList.Count == 35)
                {
                    double firecg = FirFilter.LowFilter(ecg);
                    //ecgList.add(firecg); 
                    //ecgList.add(firecg+150);
                    firLList.Add(firecg);
                    firHList.RemoveAt(0);
                }
                if (firLList.Count == 35)
                {
                    double firecg = FirFilter.HighFilter(firLList.Last());
                    ecgList.Add(firecg);
                    Console.WriteLine(firecg);
                    //firHList.add(firecg);
                    firLList.RemoveAt(0);
                }
            }

            return ecgList;
        }

        public static void SaveTxt(string txt, string filePath)
        {
            ReadTextOriginal(txt);
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                var sw = new StreamWriter(fs);
                string fileContent = string.Join(Environment.NewLine, Datas);
                sw.Write(fileContent);
                sw.Close();
            }
        }
    }
}
