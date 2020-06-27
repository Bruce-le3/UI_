using CommPlc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace Justech
{
    class Run
    {

        //work process

        //进料定位扫码
        //流线2到位顶升
        //相机拍照
        //镭射扫描
        //产品完成流出

     

        public static  void start()
        {

            #region 检测PLC到位信号，完成，执行扫码动作，未完成，报警，等待信号完成
               PLC_in_position_signal_Status:
            
            if (PLC_in_position_signal(0,0) == true)
            {
              //  Main.home.textBox1.AppendText(Status.DATEtime + Status.Plc_Run_Staus.进料到位完成.ToString() + "\r\n");
              //  Scan_code(0);//执行扫码
             
            }
            else
            {

                MessageBox.Show("检测PLC到位信号");
                Thread.Sleep(100);  //等待100ms
                goto PLC_in_position_signal_Status;
               
            }
            #endregion

            #region 流线2顶升到位信号检测!完成，执行拍照动作，未完成，报警，等待信号完成
        
                 PLC_in_position_signal_Status1:
               if (PLC_in_position_signal(0, 1) == true)
               {
                 //  Main.home.textBox1.AppendText(Status.DATEtime + Status.Plc_Run_Staus.流线2定位完成.ToString() + "\r\n");

                   //位置根据需要进行循环增加

                   Axis_Move(0, 100, 100, 0.1); //轴1移动位置
                   Axis_Move(1, 100, 100, 0.1); //轴2移动位置
                //   Camera_photo(1); //相机拍照


               }
               else
               {
                   MessageBox.Show("流线2顶升到位信号检测");
                   Thread.Sleep(100);  //等待100ms
                   goto PLC_in_position_signal_Status1;
               }

            #endregion
            #region  等待产品完成信号
        

                      PLC_in_position_signal_Status2:
                 if (PLC_in_position_signal(0, 3) == true)
                 {

                   //  Main.home.textBox1.AppendText(Status.DATEtime + Status.Plc_Run_Staus.出料完成.ToString() + "\r\n");


                 }
                 else
                 {
                     MessageBox.Show("等待产品完成信号");
                     Thread.Sleep(100);  //等待100ms
                     goto PLC_in_position_signal_Status2;
                 }

            #endregion
            #region 产品流出

            
            #endregion

     
        }
        
        /// <summary>检测PLC到位信号
        /// 
        /// </summary>
        /// <param name="status">检测PLC到位信号状态</param>
        /// <param name="word">W位</param>
        /// <param name="bit">字位</param>
        public static  bool  PLC_in_position_signal(int word,int bit)
        {
            bool status = false; ;
            //Manual_IO._Manual_io.ReadBit(ComLink.BitArea.WR, word, bit, ref status);

            if (status == true)
            {
                return true;

            }
            else
            {
                return false ;
            }


        }
        /// <summary>定位完成扫码
      /// 
      /// </summary>
      /// <param name="Number">端口号</param>
        public static  string  Scan_code(int Number)
        {

            string IP = "";
            string Port = "";
            byte[] sendbyte = Encoding.ASCII.GetBytes("");
            string recStr = "";
            byte[] recBytes = new byte[1024];
             int lengh =0;
            Socket_conn.Socket_ClientConn.CONNECT(IP, Port, Number);
       


            switch (Number)
            {
                case 0:
                   
              

                Socket_conn.Socket_ClientConn.SOCKENCONN.Send(sendbyte);

                lengh = Socket_conn.Socket_ClientConn.SOCKENCONN.Receive(recBytes);
                    break;
                case 1:
                    Socket_conn.Socket_ClientConn.SOCKENCONN1.Send(sendbyte);
                    lengh = Socket_conn.Socket_ClientConn.SOCKENCONN1.Receive(recBytes);
                    break;
                case 2:
                    Socket_conn.Socket_ClientConn.SOCKENCONN2.Send(sendbyte);
                    lengh = Socket_conn.Socket_ClientConn.SOCKENCONN2.Receive(recBytes);
                    break;
                case 3:
                    Socket_conn.Socket_ClientConn.SOCKENCONN3.Send(sendbyte);
                    lengh = Socket_conn.Socket_ClientConn.SOCKENCONN3.Receive(recBytes);
                    break;
                case 4:
                    Socket_conn.Socket_ClientConn.SOCKENCONN4.Send(sendbyte);
                    lengh = Socket_conn.Socket_ClientConn.SOCKENCONN4.Receive(recBytes);
                    break;
                case 5:
                    Socket_conn.Socket_ClientConn.SOCKENCONN5.Send(sendbyte);
                    lengh = Socket_conn.Socket_ClientConn.SOCKENCONN5.Receive(recBytes);
                    break;
                case 6:
                    Socket_conn.Socket_ClientConn.SOCKENCONN6.Send(sendbyte);
                    lengh = Socket_conn.Socket_ClientConn.SOCKENCONN6.Receive(recBytes);
                    break;

            }

         
            recStr = Encoding.UTF8.GetString(recBytes, 0, lengh);


            return recStr;




        }

        /// <summary>相机拍照
        /// 
        /// </summary>
        /// <param name="Number">端口号</param>
        public static  string Camera_photo(int Number)
        {

            string IP = "";
            string Port = "";
            byte[] sendbyte = Encoding.ASCII.GetBytes("");
            string recStr = "";
            byte[] recBytes = new byte[1024];
            int lengh = 0;
            Socket_conn.Socket_ClientConn.CONNECT(IP, Port, Number);



            switch (Number)
            {
                case 0:



                    Socket_conn.Socket_ClientConn.SOCKENCONN.Send(sendbyte);

                    lengh = Socket_conn.Socket_ClientConn.SOCKENCONN.Receive(recBytes);
                    break;
                case 1:
                    Socket_conn.Socket_ClientConn.SOCKENCONN1.Send(sendbyte);
                    lengh = Socket_conn.Socket_ClientConn.SOCKENCONN1.Receive(recBytes);
                    break;
                case 2:
                    Socket_conn.Socket_ClientConn.SOCKENCONN2.Send(sendbyte);
                    lengh = Socket_conn.Socket_ClientConn.SOCKENCONN2.Receive(recBytes);
                    break;
                case 3:
                    Socket_conn.Socket_ClientConn.SOCKENCONN3.Send(sendbyte);
                    lengh = Socket_conn.Socket_ClientConn.SOCKENCONN3.Receive(recBytes);
                    break;
                case 4:
                    Socket_conn.Socket_ClientConn.SOCKENCONN4.Send(sendbyte);
                    lengh = Socket_conn.Socket_ClientConn.SOCKENCONN4.Receive(recBytes);
                    break;
                case 5:
                    Socket_conn.Socket_ClientConn.SOCKENCONN5.Send(sendbyte);
                    lengh = Socket_conn.Socket_ClientConn.SOCKENCONN5.Receive(recBytes);
                    break;
                case 6:
                    Socket_conn.Socket_ClientConn.SOCKENCONN6.Send(sendbyte);
                    lengh = Socket_conn.Socket_ClientConn.SOCKENCONN6.Receive(recBytes);
                    break;

            }


            recStr = Encoding.UTF8.GetString(recBytes, 0, lengh);


            return recStr;


        }
        /// <summary>轴绝对移动
        /// 
        /// </summary>
        /// <param name="Axis_Number">轴号</param>
        /// <param name="distance">距离</param>
        /// <param name="speed">速度</param>
        /// <param name="Tacc">加减速度</param>
        public static  void Axis_Move(int Axis_Number,double distance,double speed,double Tacc)
        {
            //绝对移动
            Justech.GClsMontion.AbsoluteMove(Axis_Number, distance, speed, Tacc);
            Justech.GClsMontion.WaitMotorStop(Axis_Number); //等到轴停止

        }






        






    }
}
