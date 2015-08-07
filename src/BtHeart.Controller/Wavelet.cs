using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtHeart.Controller
{
    public class Wavelet
    {
        /**
 * @brief 小波变换之分解
 * @param sourceData 源数据
 * @param dataLen 源数据长度
 * @param db 过滤器类型
 * @param cA 分解后的近似部分序列-低频部分
 * @param cD 分解后的细节部分序列-高频部分
 * @return
 *         正常则返回分解后序列的数据长度，错误则返回-1
 */
        public static int dwt(double[] sourceData, int dataLen, WaveFilter db, out double[] cA, out double[] cD)
        {
            //TODO
            //db.lowFilterDec = new double[] { 2, 3, 1 };
            //db.highFilterDec = new double[] { 2, 3, 1 };
            //db.length = 3;

            cA = null;
            cD = null;
            if (dataLen < 2)
                return -1;
            if ((null == sourceData) || (null == db))
                return -1;

            int filterLen = db.length;
            int n, k, p;
            int decLen = (dataLen + filterLen - 1) / 2;
            cA = new double[decLen];
            cD = new double[decLen];
            double tmp = 0;

            for (n = 0; n < decLen; n++)
            {
                cA[n] = 0;
                cD[n] = 0;
                for (k = 0; k < filterLen; k++)
                {
                    p = 2 * n - k + 1;

                    //信号边沿对称延拓
                    if ((p < 0) && (p >= -filterLen + 1))
                        tmp = sourceData[-p - 1];
                    else if ((p > dataLen - 1) && (p <= dataLen + filterLen - 2))
                        tmp = sourceData[2 * dataLen - p - 1];
                    else if ((p >= 0) && (p < dataLen))
                        tmp = sourceData[p];
                    else
                        tmp = 0;

                    //分解后的近似部分序列-低频部分
                    cA[n] += db.lowFilterDec[k] * tmp;

                    //分解后的细节部分序列-高频部分
                    cD[n] += db.highFilterDec[k] * tmp;
                }
            }
            return decLen;
        }

        public static void Idwt(double[] cA, double[] cD, int cALength, WaveFilter db, out double[] recData)
        {
            recData = null;
            if ((null == cA) || (null == cD) || null == db)
                return;
            int filterLen = db.length;

            int n, k, p;
            int recLen = 2 * cALength - filterLen + 1;
            if ((recLen + filterLen - 1) % 2 == 0)
                recLen += 1;
            recData = new double[recLen];
            for (n = 0; n < recLen; n++)
            {
                recData[n] = 0;
                for (k = 0; k < cALength; k++)
                {
                    p = n - 2 * k + filterLen - 2;

                    //信号重构
                    if ((p >= 0) && (p < filterLen))
                    {
                        recData[n] += db.lowFilterRec[p] * cA[k] + db.highFilterRec[p] * cD[k];
                        //cout<<"recData["<<n<<"]="<<recData[n]<<endl;
                    }
                }
            }
        }
    }
}
