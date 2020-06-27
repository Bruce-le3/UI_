using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using ControlIni;
using System.Threading;

namespace Justech 
{
    class GClsClient
    {
        Ini iniFile = new Ini();
        
        //ClsIniFile iniFile = new ClsIniFile(Application.StartupPath + "\\" + "Config" + "\\Adapter.ini");---OLD
        //ConfigFile iniFile = new ConfigFile(Application.StartupPath + "\\" + "Config" + "\\Adapter.ini");
        public  List<string> localIP = new List<string>(); //localIP
        public  List<string> remoteIP = new List<string>();//remoteIP
        public  List<string> port = new List<string>();    //port
        public  List<bool> socketStatus = new List<bool>();//网口状态

        public static Socket socketClient1;
        public static Socket socketClient2;
        public static Socket socketClient3;
        public static Socket socketClient4;
        public static Socket socketClient5;
        public static Socket socketClient6;
        public static Socket socketClient7; 
        public static Socket socketClient8;

        public static string strRemoteIP="";
        public static int connectPort = 0; 
        
        int recLength = -1;//接收数据的长度
        
        //构造函数
        public GClsClient()//加载参数 ----Client界面加载
        {
            iniFile.filePath = CAMiClsVariable.adapterIniPath;
            for (int i = 1; i < 9; i++)//加载IP地址和PORT号
            {
                localIP.Add(iniFile.ReadIni("Adapter" + i.ToString(), "LOCAL_IP"));
                remoteIP.Add(iniFile.ReadIni("Adapter" + i.ToString(), "REMOTE_IP"));
                port.Add(iniFile.ReadIni("Adapter" + i.ToString(), "LOCAL_PORT"));
            }          
        }
        public void SocketConnect()//connect
        {
            for(int i = 0 ; i < localIP.Count ; i++)
            {
                if(localIP[i] != string.Empty && remoteIP[i] != string.Empty && port[i] != string.Empty)
                {                                  
                    strRemoteIP = remoteIP[i];
                    connectPort = Convert.ToInt32(port[i]);
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(strRemoteIP), connectPort);
                    socketStatus.Add(false);
                    switch(i)
                    {
                        case 0:
                            socketClient1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            try
                            {
                                socketClient1.Connect(remoteEndPoint);                          
                                socketStatus[0] = true;
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            break;
                        case 1:
                            socketClient2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            try
                            {
                                socketClient2.Connect(remoteEndPoint);
                                socketStatus[1] = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            break;
                        case 2:
                            socketClient3 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            try
                            {
                                socketClient3.Connect(remoteEndPoint);
                                socketStatus[2] = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            break;
                        case 3:
                            socketClient4 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            try
                            {
                                socketClient4.Connect(remoteEndPoint);
                                socketStatus[3] = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            break;
                        case 4:
                            socketClient5 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            try
                            {
                                socketClient5.Connect(remoteEndPoint);
                                socketStatus[4] = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            break;
                        case 5:
                            socketClient6 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            try
                            {
                                socketClient6.Connect(remoteEndPoint);
                                socketStatus[5] = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            break;
                        case 6:
                            socketClient7 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            try
                            {
                                socketClient7.Connect(remoteEndPoint);
                                socketStatus[6] = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            break;
                        case 7:
                            socketClient8 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            try
                            {
                                socketClient8.Connect(remoteEndPoint);
                                socketStatus[7] = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            break;
                    }
                }
            }
        }
        public void ClientSend(int socketNo,string sendData) //发送数据
        {           
            try
            {
                switch (socketNo)
                {
                    case 1:
                        if (GClsClient.socketClient1 != null && GClsClient.socketClient1.Connected)
                        {
                            GClsClient.socketClient1.Send(Encoding.ASCII.GetBytes(sendData + "\r\n"));
                        }
                        else
                        {
                            MessageBox.Show("请先连接！");
                        }
                        break;
                    case 2:
                        if (GClsClient.socketClient2 != null && GClsClient.socketClient2.Connected)
                        {
                            GClsClient.socketClient2.Send(Encoding.ASCII.GetBytes(sendData + "\r\n"));
                        }
                        else
                        {
                            MessageBox.Show("请先连接！");
                        }
                        break;
                    case 3:
                        if (GClsClient.socketClient3 != null && GClsClient.socketClient3.Connected)
                        {
                            GClsClient.socketClient3.Send(Encoding.ASCII.GetBytes(sendData + "\r\n"));
                        }
                        else
                        {
                            MessageBox.Show("请先连接！");
                        }
                        break;
                    case 4:
                        if (GClsClient.socketClient4 != null && GClsClient.socketClient4.Connected)
                        {
                            GClsClient.socketClient4.Send(Encoding.ASCII.GetBytes(sendData + "\r\n"));
                        }
                        else
                        {
                            MessageBox.Show("请先连接！");
                        }
                        break;
                    case 5:
                        if (GClsClient.socketClient5 != null && GClsClient.socketClient5.Connected)
                        {
                            GClsClient.socketClient5.Send(Encoding.ASCII.GetBytes(sendData + "\r\n"));
                        }
                        else
                        {
                            MessageBox.Show("请先连接！");
                        }
                        break;
                    case 6:
                        if (GClsClient.socketClient6 != null && GClsClient.socketClient6.Connected)
                        {
                            GClsClient.socketClient6.Send(Encoding.ASCII.GetBytes(sendData + "\r\n"));
                        }
                        else
                        {
                            MessageBox.Show("请先连接！");
                        }
                        break;
                    case 7:
                        if (GClsClient.socketClient7 != null && GClsClient.socketClient7.Connected)
                        {
                            GClsClient.socketClient7.Send(Encoding.ASCII.GetBytes(sendData + "\r\n"));
                        }
                        else
                        {
                            MessageBox.Show("请先连接！");
                        }
                        break;
                    case 8:
                        if (GClsClient.socketClient8 != null && GClsClient.socketClient8.Connected)
                        {
                            GClsClient.socketClient8.Send(Encoding.ASCII.GetBytes(sendData + "\r\n"));
                        }
                        else
                        {
                            MessageBox.Show("请先连接！");
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public static object lockSocket = new object();
        public static object lockSocket1 = new object();
        public static object lockSocket2 = new object();
        public static object lockSocket3 = new object();
        public static object lockSocket4 = new object();
        public static object lockSocket5 = new object();
        public static object lockSocket6 = new object();
        public static object lockSocket7 = new object();
        public static object lockSocket8 = new object();
        public string ClientReceive(int socketNo,int clearOrReceive)//接收数据
        {
            //lock(lockSocket)
            //{
                //byte[] recBytes = new byte[tempSocket.Available];
                //int lengh = tempSocket.Receive(recBytes);
                //strTemp = Encoding.UTF8.GetString(recBytes, 0, lengh);

                string recStrLast = "";
                ////byte[] recBytes = new byte[4096];//接收数据的字符数组
                //byte[] recBytes ;//= new byte[tempSocket.Available];
                try
                {
                    switch (socketNo)
                    {
                        case 1:
                            lock (lockSocket1)
                            {
                                string recStr = "";
                                byte[] recBytes;
                                string lastData = "";
                                if (GClsClient.socketClient1 != null && GClsClient.socketClient1.Connected)
                                {
                                    if(clearOrReceive == 1)
                                    {
                                        int i = 0;//计数，用于5s没有反馈跳出循环
                                        do
                                        {
                                            i++;
                                            Thread.Sleep(10);
                                            Application.DoEvents();
                                            if (i > 600)
                                            {
                                                break;
                                            }
                                        } while (socketClient1.Available <= 0);//判断是否有数据返回                                       
                                        if (i > 600)
                                        {
                                            break;
                                        }
                                        //Thread.Sleep(10);
                                        do
                                        {
                                            recBytes = new byte[socketClient1.Available];
                                            recLength = socketClient1.Receive(recBytes);
                                            recStr += Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                            lastData = recStr.Substring(recStr.Length - 1, 1);
                                            Thread.Sleep(5);
                                            Application.DoEvents();
                                        } while (lastData != (CAMiClsVariable.strVision == "KEYENCE"? "\r" : "\n"));
                                        recStrLast = recStr;
                                    }
                                    else
                                    {
                                        if (socketClient1.Available > 0)
                                        {
                                            recBytes = new byte[socketClient1.Available];
                                            recLength = socketClient1.Receive(recBytes);
                                            recStr = Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                        }                                        
                                    }
                                }
                                break;
                            }
                        case 2:
                            lock (lockSocket2)
                            {
                                string recStr = "";
                                byte[] recBytes;
                                string lastData = "";
                                if (GClsClient.socketClient2 != null && GClsClient.socketClient2.Connected)
                                {
                                    if(clearOrReceive == 1)
                                    {
                                        int i = 0;//计数，用于5s没有反馈跳出循环
                                        do
                                        {
                                            i++;
                                            Thread.Sleep(10);
                                            Application.DoEvents();
                                            if (i > 600)
                                            {
                                                break;
                                            }
                                        } while (socketClient2.Available <= 0);//判断是否有数据返回                               
                                        if (i > 600)
                                        {
                                            break;
                                        }
                                        //Thread.Sleep(10);
                                        do
                                        {
                                            recBytes = new byte[socketClient2.Available];
                                            recLength = socketClient2.Receive(recBytes);
                                            recStr += Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                            lastData = recStr.Substring(recStr.Length - 1, 1);
                                            Thread.Sleep(5);
                                            Application.DoEvents();
                                        } while (lastData != (CAMiClsVariable.strVision == "KEYENCE" ? "\r" : "\n"));
                                        recStrLast = recStr;
                                    }
                                    else
                                    {
                                        if (socketClient2.Available > 0)
                                        {
                                            recBytes = new byte[socketClient2.Available];
                                            recLength = socketClient2.Receive(recBytes);
                                            recStr = Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                        }                                      
                                    }
                                }
                                break;
                            }
                        case 3:
                            lock (lockSocket3)
                            {
                                string recStr = "";
                                byte[] recBytes;
                                string lastData = "";
                                if (GClsClient.socketClient3 != null && GClsClient.socketClient3.Connected)
                                {
                                    if(clearOrReceive  == 1)
                                    {
                                        int i = 0;//计数，用于5s没有反馈跳出循环
                                        do
                                        {
                                            i++;
                                            Thread.Sleep(10);
                                            Application.DoEvents();
                                            if (i > 500)
                                            {
                                                break;
                                            }
                                        } while (socketClient3.Available <= 0);//判断是否有数据返回
                                        if (i > 500)
                                        {
                                            break;
                                        }
                                        Thread.Sleep(10);
                                        do
                                        {
                                            recBytes = new byte[socketClient3.Available];
                                            recLength = socketClient3.Receive(recBytes);
                                            recStr += Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                            lastData = recStr.Substring(recStr.Length - 1, 1);
                                            Thread.Sleep(5);
                                            Application.DoEvents();
                                        } while (lastData != "\n");
                                        recStrLast = recStr;
                                    }
                                    else
                                    {
                                        if (socketClient3.Available > 0)
                                        {
                                            recBytes = new byte[socketClient3.Available];
                                            recLength = socketClient3.Receive(recBytes);
                                            recStr = Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                        }                                      
                                    }
                                }
                                break;
                            }
                        case 4:
                            lock (lockSocket4)
                            {
                                string recStr = "";
                                byte[] recBytes;
                                string lastData = "";
                                if (GClsClient.socketClient4 != null && GClsClient.socketClient4.Connected)
                                {
                                    if(clearOrReceive == 1)
                                    {
                                        int i = 0;//计数，用于5s没有反馈跳出循环
                                        do
                                        {
                                            i++;
                                            Thread.Sleep(10);
                                            Application.DoEvents();
                                            if (i > 500)
                                            {
                                                break;
                                            }
                                        } while (socketClient4.Available <= 0);//判断是否有数据返回
                                        if (i > 500)
                                        {
                                            break;
                                        }
                                        Thread.Sleep(10);
                                        do
                                        {
                                            recBytes = new byte[socketClient4.Available];
                                            recLength = socketClient4.Receive(recBytes);
                                            recStr += Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                            lastData = recStr.Substring(recStr.Length - 1, 1);
                                            Thread.Sleep(5);
                                            Application.DoEvents();
                                        } while (lastData != (CAMiClsVariable.strVision == "KEYENCE" ? "\r" : "\n"));
                                        recStrLast = recStr;
                                    }
                                    else
                                    {
                                        if (socketClient4.Available > 0)
                                        {
                                            recBytes = new byte[socketClient4.Available];
                                            recLength = socketClient4.Receive(recBytes);
                                            recStr = Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                        }                                       
                                    }
                                }
                                break;
                            }
                        case 5:
                            lock (lockSocket5)
                            {
                                string recStr = "";
                                byte[] recBytes;
                                string lastData = "";
                                if (GClsClient.socketClient5 != null && GClsClient.socketClient5.Connected)
                                {
                                    if(clearOrReceive == 1)
                                    {
                                        int i = 0;//计数，用于5s没有反馈跳出循环
                                        do
                                        {
                                            i++;
                                            Thread.Sleep(10);
                                            Application.DoEvents();
                                            if (i > 500)
                                            {
                                                break;
                                            }
                                        } while (socketClient5.Available <= 0);//判断是否有数据返回
                                        if (i > 500)
                                        {
                                            break;
                                        }
                                        Thread.Sleep(10);
                                        do
                                        {
                                            recBytes = new byte[socketClient5.Available];
                                            recLength = socketClient5.Receive(recBytes);
                                            recStr += Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                            lastData = recStr.Substring(recStr.Length - 1, 1);
                                            Thread.Sleep(5);
                                            Application.DoEvents();
                                        } while (lastData != (CAMiClsVariable.strVision == "KEYENCE" ? "\r" : "\n"));
                                        recStrLast = recStr;
                                    }
                                    else
                                    {
                                        if (socketClient5.Available > 0)
                                        {
                                            recBytes = new byte[socketClient5.Available];
                                            recLength = socketClient5.Receive(recBytes);
                                            recStr = Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                        }                                       
                                    }
                                }
                                break;
                            }
                        case 6:
                            lock (lockSocket6)
                            {
                                string recStr = "";
                                byte[] recBytes;
                                string lastData = "";
                                if (GClsClient.socketClient6 != null && GClsClient.socketClient6.Connected)
                                {
                                    if(clearOrReceive == 1)
                                    {
                                        int i = 0;//计数，用于5s没有反馈跳出循环
                                        do
                                        {
                                            i++;
                                            Thread.Sleep(10);
                                            Application.DoEvents();
                                            if (i > 500)
                                            {
                                                break;
                                            }
                                        } while (socketClient6.Available <= 0);//判断是否有数据返回
                                        if (i > 500)
                                        {
                                            break;
                                        }
                                        Thread.Sleep(10);
                                        do
                                        {
                                            recBytes = new byte[socketClient6.Available];
                                            recLength = socketClient6.Receive(recBytes);
                                            recStr += Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                            lastData = recStr.Substring(recStr.Length - 1, 1);
                                            Thread.Sleep(5);
                                            Application.DoEvents();
                                        } while (lastData != (CAMiClsVariable.strVision == "KEYENCE" ? "\r" : "\n"));
                                        recStrLast = recStr;
                                    }
                                    else
                                    {
                                        if (socketClient6.Available > 0)
                                        {
                                            recBytes = new byte[socketClient6.Available];
                                            recLength = socketClient6.Receive(recBytes);
                                            recStr = Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                        }                                       
                                    }
                                }
                                break;
                            }
                        case 7:
                            lock (lockSocket7)
                            {
                                string recStr = "";
                                byte[] recBytes;
                                string lastData = "";
                                if (GClsClient.socketClient7 != null && GClsClient.socketClient7.Connected)
                                {
                                    if(clearOrReceive == 1)
                                    {
                                        int i = 0;//计数，用于5s没有反馈跳出循环
                                        do
                                        {
                                            i++;
                                            Thread.Sleep(10);
                                            Application.DoEvents();
                                            if (i > 500)
                                            {
                                                break;
                                            }
                                        } while (socketClient7.Available <= 0);//判断是否有数据返回
                                        if (i > 500)
                                        {
                                            break;
                                        }
                                        Thread.Sleep(10);
                                        do
                                        {
                                            recBytes = new byte[socketClient7.Available];
                                            recLength = socketClient7.Receive(recBytes);
                                            recStr += Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                            lastData = recStr.Substring(recStr.Length - 1, 1);
                                            Thread.Sleep(5);
                                            Application.DoEvents();
                                        } while (lastData != (CAMiClsVariable.strVision == "KEYENCE" ? "\r" : "\n"));
                                        recStrLast = recStr;
                                    }
                                    else
                                    {
                                        if (socketClient7.Available > 0)
                                        {
                                            recBytes = new byte[socketClient7.Available];
                                            recLength = socketClient7.Receive(recBytes);
                                            recStr = Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                        }                                        
                                    }
                                }
                                break;
                            }
                        case 8:
                            lock (lockSocket8)
                            {
                                string recStr = "";
                                byte[] recBytes;
                                string lastData = "";
                                if (GClsClient.socketClient8 != null && GClsClient.socketClient8.Connected)
                                {
                                    if(clearOrReceive == 1)
                                    {
                                        int i = 0;//计数，用于5s没有反馈跳出循环
                                        do
                                        {
                                            i++;
                                            Thread.Sleep(10);
                                            Application.DoEvents();
                                            if (i > 500)
                                            {
                                                break;
                                            }
                                        } while (socketClient8.Available <= 0);//判断是否有数据返回
                                        if (i > 500)
                                        {
                                            break;
                                        }
                                        Thread.Sleep(10);
                                        do
                                        {
                                            recBytes = new byte[socketClient8.Available];
                                            recLength = socketClient8.Receive(recBytes);
                                            recStr += Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                            lastData = recStr.Substring(recStr.Length - 1, 1);
                                            Thread.Sleep(5);
                                            Application.DoEvents();
                                        } while (lastData != (CAMiClsVariable.strVision == "KEYENCE" ? "\r" : "\n"));
                                        recStrLast = recStr;
                                    }
                                    else
                                    {
                                        if (socketClient8.Available > 0)
                                        {
                                            recBytes = new byte[socketClient8.Available];
                                            recLength = socketClient8.Receive(recBytes);
                                            recStr = Encoding.ASCII.GetString(recBytes, 0, recBytes.Length);
                                        }                                        
                                    }
                                }
                                break;
                            }
                    }
                    return recStrLast ;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    recStrLast = "Err";
                    return recStrLast;
                }            
            //}       
        }

        public void DisConnectClient(int socketNo)//断开socket连接
        {
            switch (socketNo)
            {
                case 1:
                    if (socketClient1 !=null && socketClient1.Connected )
                    {
                        socketClient1.Close();
                        socketStatus[0] = false;
                    }
                    break;
                case 2:
                    if (socketClient2 != null && socketClient2.Connected)
                    {
                        socketClient2.Close();
                        socketStatus[1] = false;
                    }
                    break;
                case 3:
                    if (socketClient3 != null && socketClient3.Connected)
                    {
                        socketClient3.Close();
                        socketStatus[2] = false;
                    }
                    break;
                case 4:
                    if (socketClient4 != null && socketClient4.Connected)
                    {
                        socketClient4.Close();
                        socketStatus[3] = false;
                    }
                    break;
                case 5:
                    if (socketClient5 != null && socketClient5.Connected)
                    {
                        socketClient5.Close();
                        socketStatus[4] = false;
                    }
                    break;
                case 6:
                    if (socketClient6 != null && socketClient6.Connected)
                    {
                        socketClient6.Close();
                        socketStatus[5] = false;
                    }
                    break;
                case 7:
                    if (socketClient7 != null && socketClient7.Connected)
                    {
                        socketClient7.Close();
                        socketStatus[6] = false;
                    }
                    break;
                case 8:
                    if (socketClient8 != null && socketClient8.Connected)
                    {
                        socketClient8.Close();
                        socketStatus[7] = false;
                    }
                    break;
            }
        }
    }
}
