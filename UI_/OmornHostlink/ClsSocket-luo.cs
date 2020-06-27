using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Justech
{
    public static class ClsSocket
    {
        public static bool IsSocketBunding;
        //public static ParameterizedThreadStart threadStart1 = new ParameterizedThreadStart(ReceiveData);
        public static Thread SocketReceiveThread = new Thread(new ParameterizedThreadStart(ReceiveData));      
        public static Socket CreateServer(string IP, string Port)
        {
            Socket ResultSocket = null;
            Socket tempSocket = null;
            try
            {                
                if (!IsSocketBunding)
                {
                    IPAddress IP1 = IPAddress.Parse(IP);
                    IPEndPoint remoteEP = new IPEndPoint(IP1, int.Parse(Port));
                    tempSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    tempSocket.Bind(remoteEP);
                    IsSocketBunding = true;
                }
                tempSocket.Listen(1);
                ResultSocket = tempSocket.Accept();
                return ResultSocket;
            }
            catch 
            {                     
                if (tempSocket != null)
                {
                    tempSocket.Dispose();
                }
                //Log.SaveLog("Socket", "CreatServerError:" + ex.ToString());  
                return null;
            }
        }
        public static Socket CreateClient(string IP, string Port)
        {
            Socket tempSocket = null;
            try
            {
                IPAddress IP1 = IPAddress.Parse(IP);
                IPEndPoint remoteEP = new IPEndPoint(IP1, int.Parse(Port));
                tempSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tempSocket.Connect(remoteEP);
                return tempSocket;
            }
            catch 
            {
                //Log.SaveLog("Socket", "CreateClientError:" + ex.ToString()); 
                return null;
            }
        }
        public static bool SendData(Socket s, string sendStr)
        {
            try
            {
                if (CheckConnectStatus(s))
                {
                    byte[] sendBytes = Encoding.UTF8.GetBytes(sendStr);
                    int testNum = s.Send(sendBytes);
                    //Log.SaveLog("Socket", "Send:" + sendStr);    
                    return true;
                }
                return false;
            }
            catch 
            {
                //Log.SaveLog("Socket", "SendError:" + ex.ToString());    
                return false;
            }
        }
        public static void ReceiveData(object socketpara)
        {
            Socket tempSocket = socketpara as Socket;
            while (true)
            {
                try
                {
                    if (CheckConnectStatus(tempSocket))
                    {
                        string strTemp = "";
                        if (!(tempSocket.Available == 0))
                        {
                            byte[] recBytes = new byte[tempSocket.Available];
                            int lengh = tempSocket.Receive(recBytes);
                            strTemp = Encoding.UTF8.GetString(recBytes, 0, lengh);
                        }
                        if (strTemp.Length > 1)
                        {
                            //Log.SaveLog("Socket", "Receive:" + strTemp);   
                        }
                    }
                }
                catch 
                {
                    //Log.SaveLog("Socket", "ReceiveError:" + ex.ToString());   
                    break;
                }
                Thread.Sleep(10);
            }
        }            
        public static bool CheckConnectStatus(Socket tempSocket)          //检查连接状态
        {
            try
            {
                bool ConnectStatus = !((tempSocket.Poll(1000, SelectMode.SelectRead) && (tempSocket.Available == 0)) || !tempSocket.Connected);
                return ConnectStatus;
            }
            catch 
            {
                //Log.SaveLog("Socket", "CheckStatusError:" + ex.ToString());  
                return false;
            }
        }
        public static void SocketClose(Socket socket)
        {
            if (socket == null)
                return;
            if (!socket.Connected)
                return;
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {
            }
            try
            {
                socket.Close();
            }
            catch
            {
            }
        }
    }
}
