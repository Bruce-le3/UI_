using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

namespace Justech
{
    class SerialPort_
    {
        public static string  Read_data;
        public static bool plcstate;
        #region 程序调用
        public    static  SerialPort rs232_port = new SerialPort();
       /// <summary>
       /// 
       /// </summary>
       /// <param name="number">序列，例如：1,2,3,4</param>
       /// <param name="Send_data">发送的内容</param>
       public static void manual_connect(int  number, string Send_data)
       {
           rs232_port.Close();
           string setting=null;
           switch (number)
           {

               case 0:
                   setting = "COM1,115200,N,8,1";
                   break;
               case 1:
                   setting = "COM2,115200,N,8,1";
                   break;
               case 2:
                   setting = "COM3,115200,N,8,1";
                   break;
           }
           string[] set = setting.Split(',');
           rs232_port.PortName = set[0];
           rs232_port.BaudRate = int.Parse(set[1]);
           switch (set[2])
           {
               case "N":
                   rs232_port.Parity = Parity.None;
                   break;
               case "O":
                   rs232_port.Parity = Parity.Odd;
                   break;
               case "E":
                   rs232_port.Parity = Parity.Even;
                   break;
               case "M":
                   rs232_port.Parity = Parity.Mark;
                   break;
               case "S":
                   rs232_port.Parity = Parity.Space;
                   break;
           }

           rs232_port.DataBits = int.Parse(set[3]);

           switch (set[4])
           {
               case "1":
                   rs232_port.StopBits = StopBits.One;
                   break;

               case "2":
                   rs232_port.StopBits = StopBits.Two;

                   break;
               default:
                   rs232_port.StopBits = StopBits.None;
                   break;
           }
           try
           {
               rs232_port.DataReceived += new SerialDataReceivedEventHandler(rs232_port_DataReceived1);
               rs232_port.Open();
               plcstate = true;
               byte[] DATA = Encoding.ASCII.GetBytes(Send_data);
               rs232_port.Write(DATA, 0, DATA.Length);
           }
           catch (Exception ex)
           {
               plcstate = false;
               MessageBox.Show(ex.Message);
           }
       }    
       private static void rs232_port_DataReceived1(object sender, SerialDataReceivedEventArgs e)
       {
           while (rs232_port.BytesToRead > 0)
           {
               Read_data = rs232_port.ReadExisting();
           }
       }
        #endregion
       #region  手动测试
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting">例如：COM1,115200,N,8,1</param>
        /// <param name="Send_data">发送内容</param>
       public static void connect(string setting,string Send_data)
        {
        
                rs232_port.Close();
                string[] set = setting.Split(',');

                rs232_port.PortName = set[0];
                rs232_port.BaudRate = int.Parse(set[1]);
                switch (set[2])
                {
                    case "N":
                        rs232_port.Parity = Parity.None;
                        break;
                    case "O":
                        rs232_port.Parity = Parity.Odd;
                        break;
                    case "E":
                        rs232_port.Parity = Parity.Even;
                        break;
                    case "M":
                        rs232_port.Parity = Parity.Mark;
                        break;
                    case "S":
                        rs232_port.Parity = Parity.Space;
                        break;
                }
                rs232_port.DataBits = int.Parse(set[3]);
                switch (set[4])
                {
                    case "1":
                        rs232_port.StopBits = StopBits.One;
                        break;
                    case "2":
                        rs232_port.StopBits = StopBits.Two;
                        break;
                    default:
                        rs232_port.StopBits = StopBits.None;
                        break;
                }
                try
                {
                       rs232_port.DataReceived += new SerialDataReceivedEventHandler(rs232_port_DataReceived);                
                        rs232_port.Open();
                        plcstate = true;
                        byte[] DATA = Encoding.ASCII.GetBytes(Send_data);
                        rs232_port.Write(DATA, 0, DATA.Length);
                }
                catch (Exception ex)
                {
                    plcstate = false;
                    MessageBox.Show(ex.Message);
                }       
        }
private static void rs232_port_DataReceived(object sender, SerialDataReceivedEventArgs e)
{
    while (rs232_port.BytesToRead>0)
    {
        Read_data = "";
        StringBuilder strRead = new StringBuilder();
        byte[] buffer = Encoding.ASCII.GetBytes(rs232_port.ReadExisting());
   
        Read_data = Encoding.ASCII.GetString(buffer);

        if (rs232_port.PortName == "COM3")
        {
            //EM4100类型
            if (buffer[2] == 48)
            {


                for (int i = 3; i < buffer.Length - 2; i++)
                {

                    strRead.Append(buffer[i]);
                }

                string s = strRead.ToString();
                Logs.Log.Log_Set("RFID"+System.DateTime.Now.ToString("yyyyMMdd"),strRead.ToString());

            }

        }     
    }

}
       #endregion

    }
}
