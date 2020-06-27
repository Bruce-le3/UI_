using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justech
{
    class CPK_STDEV_CALC
    {


        public static double average(List<double> averageData)
        {

          

            double average_data = averageData.Average();

            return average_data;


        }
   /// <summary>
   /// 
   /// </summary>
   /// <param name="Tu">上限</param>
   /// <param name="Tl">下限</param>
   /// <param name="data">数据</param>
   /// <returns></returns>


        public static double cpu(double Tu, double Tl, List<double> data)
        {


            StDev(data);

            average(data);
            double cpu = 0F;
            if (data.Count == 32)
            {

              
                cpu = (Tu - average(data)) / (3 * StDev(data));

            }


            return cpu;



        }

        public static double ca(double Tu, double Tl, List<double>data)
        {


            StDev(data);

            average(data);
            double ca = 0F;
            if (data.Count == 32)
            {


                ca =Math.Abs( (average(data)-(Tu+Tl)/2)/ ((Tu-Tl)/2));     //准确度

            }


            return ca;



        }

        public static double cp(double Tu, double Tl, List<double> data)
        {


            StDev(data);

            average(data);
            double cp = 0F;
            if (data.Count == 32)
            {


                cp = (Tu-Tl)/(6*StDev(data));  //精确度

            }


            return cp;



        }

        public static double cpl(double Tu, double Tl, List<double> data)
        {


            StDev(data);

            average(data);
            double cpl = 0F;
            if (data.Count == 32)
            {

                cpl = (average(data) - Tl) / (3 * StDev(data));

            }


            return cpl;



        }

        public static double cpk(double Tu, double Tl, List<double> data)
        {

            StDev(data);

            average(data);
            double cpk = 0F;
            if (data.Count == 32)
            {

                 cpk = Math.Min((Tu - average(data)) / (3 * StDev(data)), (average(data) - Tl) / (3 * StDev(data)));

            }


            return cpk;




        }

        public static double StDev(List<double> arrData) //计算标准偏差
        {
            double xSum = 0F;
            double xAvg = 0F;
            double sSum = 0F;
            double tmpStDev = 0F;
            int arrNum = arrData.Count;
            for (int i = 0; i < arrNum; i++)
            {
                xSum += arrData[i];
            }
            xAvg = xSum / arrNum;
            for (int j = 0; j < arrNum; j++)
            {
                sSum += ((arrData[j] - xAvg) * (arrData[j] - xAvg));
            }
            tmpStDev = Convert.ToSingle(Math.Sqrt((sSum / (arrNum - 1))).ToString());
            return tmpStDev;
        }


    }
}
