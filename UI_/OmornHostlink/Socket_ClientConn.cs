using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

namespace Socket_conn
{
    class Socket_ClientConn
    {
        public static string LOCAL_IP; //本地IP
        public static int LOCAL_PORT; //本地端口号

        //public static string REMOTE_IP; //服务器IP
        //public static int REMOTE_PORT; //服务器端口号
        //public static bool socket_status; //返回状态

        public static Socket SOCKENCONN;
        public static Socket SOCKENCONN1;
        public static Socket SOCKENCONN2;
        public static Socket SOCKENCONN3;
        public static Socket SOCKENCONN4;
        public static Socket SOCKENCONN5;
        public static Socket SOCKENCONN6;
        public static Socket SOCKENCONN7;
        public static Socket SOCKENCONN8;
        public static Socket SOCKENCONN9;


        public static int
         adapter1 = 0,
        adapter2 = 0,
        adapter3 = 0,
        adapter4 = 0,
        adapter5 = 0,
        adapter6 = 0,
        adapter7 = 0,
        adapter8 = 0,
        adapter9 = 0,
        adapter10 = 0,
        adapter11 = 0;


        public static int socket_status_count = 0;

        public static void CONNECT(string localip,string localport,int i)

        {



               Socket_ClientConn.LOCAL_IP = localip;
               Socket_ClientConn.LOCAL_PORT = int.Parse(localport);
                IPAddress IPADD = IPAddress.Parse(LOCAL_IP);


                IPEndPoint IPEND = new IPEndPoint(IPADD, LOCAL_PORT);

                switch (i)
                {


                    case 0:

                        SOCKENCONN = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //    SOCKENCONN.ReceiveTimeout = 30000;
                    //    SOCKENCONN.SendTimeout = 3000;
                        try
                        {


                            SOCKENCONN.Connect(IPEND);
                            adapter1 = 1;


                            socket_status_count = 1;

                        }
                        catch (Exception EX)
                        {
                            MessageBox.Show(EX.Message);
                            socket_status_count = 0;

                        }
                        break;
                    case 1:
                        SOCKENCONN1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {


                            SOCKENCONN1.Connect(IPEND);
                            adapter2 = 2;



                        }
                        catch (Exception EX)
                        {
                            MessageBox.Show(EX.Message);

                        }
                        break;
                    case 2:
                        SOCKENCONN2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {


                            SOCKENCONN2.Connect(IPEND);
                            adapter3 = 3;



                        }
                        catch (Exception EX)
                        {
                            MessageBox.Show(EX.Message);

                        }
                        break;
                    case 3:
                        SOCKENCONN3 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {


                            SOCKENCONN3.Connect(IPEND);
                            adapter4 = 4;


                        }
                        catch (Exception EX)
                        {
                            MessageBox.Show(EX.Message);

                        }
                        break;
                    case 4:
                        SOCKENCONN4 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {


                            SOCKENCONN4.Connect(IPEND);
                            adapter5 = 5;



                        }
                        catch (Exception EX)
                        {
                            MessageBox.Show(EX.Message);

                        }
                        break;
                    case 5:
                        SOCKENCONN5 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {


                            SOCKENCONN5.Connect(IPEND);
                            adapter6 = 6;



                        }
                        catch (Exception EX)
                        {
                            MessageBox.Show(EX.Message);

                        }
                        break;
                    case 6:
                        SOCKENCONN6 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {


                            SOCKENCONN6.Connect(IPEND);
                            adapter7 = 7;



                        }
                        catch (Exception EX)
                        {
                            MessageBox.Show(EX.Message);

                        }
                        break;
                    case 7:
                        SOCKENCONN7 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {


                            SOCKENCONN7.Connect(IPEND);
                            adapter8 = 8;



                        }
                        catch (Exception EX)
                        {
                            MessageBox.Show(EX.Message);

                        }
                        break;
                    case 8:
                        SOCKENCONN8 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {


                            SOCKENCONN8.Connect(IPEND);
                            adapter9 = 9;



                        }
                        catch (Exception EX)
                        {
                            MessageBox.Show(EX.Message);

                        }
                        break;
                    case 9:
                        SOCKENCONN9 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {


                            SOCKENCONN9.Connect(IPEND);
                            socket_status_count = 10;



                        }
                        catch (Exception EX)
                        {
                            MessageBox.Show(EX.Message);

                        }
                        break;
                }




            








        }

  
       



    }
}
