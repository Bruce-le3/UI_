using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justech
{
    class GRR
    {


        public static void XAbar_average(List<double> averageData1, ref  double average_data1)
        {

        
                average_data1 = averageData1.Average();
             

            


        }
        public static void XBbar_average(List<double> averageData2, ref  double average_data2)
        {

       
                average_data2 = averageData2.Average();
            

            


        }
        public static void XCbar_average(List<double> averageData3, ref  double average_data3)
        {


                average_data3 = averageData3.Average();
         

            


        }


        public static void RAbar_average(List<double> RaverageData1, ref  double Raverage_data1)
        {


            Raverage_data1 = RaverageData1.Average();





        }
        public static void RBbar_average(List<double> RaverageData2, ref  double Raverage_data2)
        {


            Raverage_data2 = RaverageData2.Average();





        }
        public static void RCbar_average(List<double> RaverageData3, ref  double Raverage_data3)
        {


            Raverage_data3 = RaverageData3.Average();







        }

        public static List<double> averageData1 = new List<double>();
           public static List<double>    averageData2=new List<double> ();
            public static List<double>   averageData3=new List<double> ();
        public static List<double> RaverageData1=new List<double> ();
            public static List<double>  RaverageData2=new List<double> ();
           public static List<double>   RaverageData3=new List<double> ();
        public static double averagedata1, averagedata2, averagedata3;
        public static double Raveragedata1, Raveragedata2, Raveragedata3;

        public static double diff;
        public static List<double> Rdiffs = new List<double>();
        public static double Xbardiff(ref double Raveragedata1,ref double Raveragedata2,ref double  Raveragedata3)
        {



            XAbar_average(averageData1,ref averagedata1);
            XBbar_average(averageData2, ref averagedata2);
            XCbar_average(averageData3, ref averagedata3);

            double max = Math.Max(Math.Max(averagedata1, averagedata2), averagedata3);
            double min = Math.Min(Math.Min(averagedata1, averagedata2), averagedata3);

          diff = max - min;


          for (int i = 0; i < 10; i++)
          {


                  double q1 = averageData1[i];
                  double q2 = averageData1[i + 10];
                  double q3 = averageData1[i + 20];


                  double Rmax = Math.Max(Math.Max(q1, q2), q3);
                  double Rmin = Math.Min(Math.Min(q1, q2), q3);
                  double Rdiff = Rmax - Rmin;
                  Rdiffs.Add(Rdiff);
                  Raveragedata1 = Rdiffs.Average();
               
    
          }
          Rdiffs.Clear();
          for (int i = 0; i < 10; i++)
          {


              double q1 = averageData2[i];
              double q2 = averageData2[i + 10];
              double q3 = averageData2[i + 20];


              double Rmax = Math.Max(Math.Max(q1, q2), q3);
              double Rmin = Math.Min(Math.Min(q1, q2), q3);
              double Rdiff = Rmax - Rmin;
              Rdiffs.Add(Rdiff);
              Raveragedata2 = Rdiffs.Average();
              Rdiffs.Clear();

          }
          Rdiffs.Clear();
          for (int i = 0; i < 10; i++)
          {


              double q1 = averageData3[i];
              double q2 = averageData3[i + 10];
              double q3 = averageData3[i + 20];


              double Rmax = Math.Max(Math.Max(q1, q2), q3);
              double Rmin = Math.Min(Math.Min(q1, q2), q3);
              double Rdiff = Rmax - Rmin;
              Rdiffs.Add(Rdiff);
              Raveragedata3 = Rdiffs.Average();
           

          }

          Rdiffs.Clear();


          return diff;



        }

   

     
        public static void  Rbar(ref double UCLr,ref double EV,ref double AV,ref double RR,double Trials,double D4,double K1,double K2)
        {


            Xbardiff(ref  Raveragedata1, ref Raveragedata2,ref  Raveragedata3);



            double Rbar = (Raveragedata1 + Raveragedata2 + Raveragedata3) / Trials;



            UCLr = Rbar * D4;

            EV = Rbar * K1;

            double s_AV1 = Math.Pow((diff * K2), 2);
            double s_AV2 = (Math.Pow(EV, 2))/30;
            double S_AV_SQRT =Math.Abs( s_AV1 - s_AV2);
            AV = Math.Sqrt(S_AV_SQRT);


            RR = Math.Sqrt(Math.Pow(EV, 2) + Math.Pow(AV, 2));

       


        }







    }
}
