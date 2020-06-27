using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Justech 
{
    public partial class FrmClient : Form
    {
        public Thread thJudgeStatus;//连接状态线程
        public Thread thReceiveData;//接收数据线程
        //ClsClient classClient = new ClsClient();//实例     
        public string strClientRecv="";
        public Socket instSocketClient;
        public int indexClinet = 0;//选中的IP地址号
        public int currentIndexClient;//当前选中的IP地址号
        public FrmClient()
        {
            InitializeComponent();
        }       
        private void FrmClient_Load(object sender, EventArgs e)//load
        {           
            for (int i = 0; i < CAMiClsVariable .cClient.localIP.Count; i++)
            {
                cobLocalIP.Items.Add(CAMiClsVariable.cClient.localIP[i]);//加载LocalIP地址
            }
            cobLocalIP.SelectedIndex = 0;      
            #region 判断是否已连接         
            for (int i = 0 ; i < CAMiClsVariable .cClient.socketStatus.Count ; i++)
            {
                switch(i)
                {
                    case 0:
                        if(CAMiClsVariable .cClient.socketStatus[0])
                        {
                            lblStatusClient1.BackColor = Color.Green;
                        }
                        break;
                    case 1:
                        if (CAMiClsVariable .cClient.socketStatus[1])
                        {
                            lblStatusClient2.BackColor = Color.Green;
                        }
                        break;
                    case 2:
                        if (CAMiClsVariable .cClient.socketStatus[2])
                        {
                            lblStatusClient3.BackColor = Color.Green;
                        }
                        break;
                    case 3:
                        if (CAMiClsVariable .cClient.socketStatus[3])
                        {
                            lblStatusClient4.BackColor = Color.Green;
                        }
                        break;
                    case 4:
                        if (CAMiClsVariable .cClient.socketStatus[4])
                        {
                            lblStatusClient5.BackColor = Color.Green;
                        }
                        break;
                    case 5:
                        if (CAMiClsVariable .cClient.socketStatus[5])
                        {
                            lblStatusClient6.BackColor = Color.Green;
                        }
                        break;
                    case 6:
                        if (CAMiClsVariable .cClient.socketStatus[6])
                        {
                            lblStatusClient7.BackColor = Color.Green;
                        }
                        break;
                    case 7:
                        if (CAMiClsVariable .cClient.socketStatus[7])
                        {
                            lblStatusClient8.BackColor = Color.Green;
                        }
                        break;
                    default:
                        MessageBox.Show("网口设置超限，请检查！");
                        break;
                }
            }
            #endregion
            thJudgeStatus = new Thread(new  ThreadStart(StatusDisplay));
            thJudgeStatus.Start();
            //thReceiveData = new Thread(new ThreadStart(Client_ReceiveAccept));
            //thReceiveData.Start();
        }
        private void StatusDisplay()//socket连接状态判断
        {
            while(true)
            {
                int tag = -1;
                for (int i = 0; i < CAMiClsVariable .cClient.socketStatus.Count; i++)
                {
                    foreach(var label in groupStatus.Controls.OfType<Label>())
                    {
                        tag = Convert.ToInt32(label.Tag);
                        if(i == tag)
                        {
                            if(CAMiClsVariable .cClient.socketStatus[i])
                            {
                                label.BackColor = Color.Green;                              
                            }
                            else
                            {
                                label.BackColor = Color.Red;

                            }
                        }
                    }                  
                }
                Thread.Sleep(5);
                Application.DoEvents();
            }
        }      
        private void btnConnect_Click(object sender, EventArgs e)//socket连接
        {            
            string strRemoteIP = "";
            int connectPort = 0;
            int index=cobLocalIP.SelectedIndex;
            #region   针对每个socketClient进行判断          
            switch (index)
            {
                case 0:
                    if(GClsClient.socketClient1.Connected)
                    {
                        MessageBox.Show("已打开，请勿重复打开！");
                    }
                    else
                    {
                        try
                        {
                            strRemoteIP = CAMiClsVariable .cClient.remoteIP[index];
                            connectPort = int.Parse(CAMiClsVariable .cClient.port[index]);
                            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(strRemoteIP), connectPort);
                            GClsClient.socketClient1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            GClsClient.socketClient1.Connect(remoteEndPoint);
                            CAMiClsVariable .cClient.socketStatus[index] = true;
                            txtReceive.Text = strRemoteIP + "连接成功！！！";
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    break;
                case 1:
                    if (GClsClient.socketClient2.Connected)
                    {
                        MessageBox.Show("已打开，请勿重复打开！");
                    }
                    else
                    {
                        try
                        {
                            strRemoteIP = CAMiClsVariable .cClient.remoteIP[index];
                            connectPort = int.Parse(CAMiClsVariable .cClient.port[index]);
                            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(strRemoteIP), connectPort);
                            GClsClient.socketClient2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            GClsClient.socketClient2.Connect(remoteEndPoint);
                            CAMiClsVariable .cClient.socketStatus[index] = true;
                            txtReceive.Text = strRemoteIP + "连接成功！！！";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    break;
                case 2:
                    if (GClsClient.socketClient3.Connected)
                    {
                        MessageBox.Show("已打开，请勿重复打开！");
                    }
                    else
                    {
                        try
                        {
                            strRemoteIP = CAMiClsVariable .cClient.remoteIP[index];
                            connectPort = int.Parse(CAMiClsVariable .cClient.port[index]);
                            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(strRemoteIP), connectPort);
                            GClsClient.socketClient3 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            GClsClient.socketClient3.Connect(remoteEndPoint);
                            CAMiClsVariable .cClient.socketStatus[index] = true;
                            txtReceive.Text = strRemoteIP + "连接成功！！！";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    break;
                case 3:
                    if (GClsClient.socketClient4.Connected)
                    {
                        MessageBox.Show("已打开，请勿重复打开！");
                    }
                    else
                    {
                        try
                        {
                            strRemoteIP = CAMiClsVariable .cClient.remoteIP[index];
                            connectPort = int.Parse(CAMiClsVariable .cClient.port[index]);
                            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(strRemoteIP), connectPort);
                            GClsClient.socketClient4 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            GClsClient.socketClient4.Connect(remoteEndPoint);
                            CAMiClsVariable .cClient.socketStatus[index] = true;
                            txtReceive.Text = strRemoteIP + "连接成功！！！";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    break;
                case 4:
                    if (GClsClient.socketClient5.Connected)
                    {
                        MessageBox.Show("已打开，请勿重复打开！");
                    }
                    else
                    {
                        try
                        {
                            strRemoteIP = CAMiClsVariable .cClient.remoteIP[index];
                            connectPort = int.Parse(CAMiClsVariable .cClient.port[index]);
                            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(strRemoteIP), connectPort);
                            GClsClient.socketClient5 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            GClsClient.socketClient5.Connect(remoteEndPoint);
                            CAMiClsVariable .cClient.socketStatus[index] = true;
                            txtReceive.Text = strRemoteIP + "连接成功！！！";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    break;
                case 5:
                    if (GClsClient.socketClient6.Connected)
                    {
                        MessageBox.Show("已打开，请勿重复打开！");
                    }
                    else
                    {
                        try
                        {
                            strRemoteIP = CAMiClsVariable .cClient.remoteIP[index];
                            connectPort = int.Parse(CAMiClsVariable .cClient.port[index]);
                            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(strRemoteIP), connectPort);
                            GClsClient.socketClient6 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            GClsClient.socketClient6.Connect(remoteEndPoint);
                            CAMiClsVariable .cClient.socketStatus[index] = true;
                            txtReceive.Text = strRemoteIP + "连接成功！！！";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    break;
                case 6:
                    if (GClsClient.socketClient7.Connected)
                    {
                        MessageBox.Show("已打开，请勿重复打开！");
                    }
                    else
                    {
                        try
                        {
                            strRemoteIP = CAMiClsVariable .cClient.remoteIP[index];
                            connectPort = int.Parse(CAMiClsVariable .cClient.port[index]);
                            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(strRemoteIP), connectPort);
                            GClsClient.socketClient7 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            GClsClient.socketClient7.Connect(remoteEndPoint);
                            CAMiClsVariable .cClient.socketStatus[index] = true;
                            txtReceive.Text = strRemoteIP + "连接成功！！！";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    break;
                case 7:
                    if (GClsClient.socketClient8.Connected)
                    {
                        MessageBox.Show("已打开，请勿重复打开！");
                    }
                    else
                    {
                        try
                        {
                            strRemoteIP = CAMiClsVariable .cClient.remoteIP[index];
                            connectPort = int.Parse(CAMiClsVariable .cClient.port[index]);
                            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(strRemoteIP), connectPort);
                            GClsClient.socketClient8 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            GClsClient.socketClient8.Connect(remoteEndPoint);
                            CAMiClsVariable .cClient.socketStatus[index] = true;
                            txtReceive.Text = strRemoteIP + "连接成功！！！";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    break;
            }
            #endregion
        }
        private void btnDisconnect_Click(object sender, EventArgs e)//断开socket连接
        {
            CAMiClsVariable .cClient.DisConnectClient( indexClinet + 1);
        }
        private void Client_ReceiveAccept()//接收数据
        {
            while(true)
            {
                try
                {
                    strClientRecv = CAMiClsVariable .cClient.ClientReceive((indexClinet + 1),1);                   
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    //strClientRecv = "";
                }
                if (!string.IsNullOrWhiteSpace(strClientRecv))
                {
                    MsgShow(strClientRecv);
                    strClientRecv = "";
                }
                Thread.Sleep(5);
                Application.DoEvents();
            }                  
        }
        public delegate void AppendTextDelegate(string str);
        //为控件的InvokeRequire的值为true时，说明一个不属于创建它的线程要访问它，此时需要采用异步的方式进行操作
        private void MsgShow(string str)
        {
            if(txtReceive.InvokeRequired)
            {
                AppendTextDelegate appendText = new AppendTextDelegate(MsgShow);
                txtReceive.Invoke(appendText, new object[] { str });
            }
            else
            {
                txtReceive.Text = "";
                txtReceive.Text = str;
            }
        }       
        private void btnSend_Click(object sender, EventArgs e)//手动发送数据
        {
            CAMiClsVariable .cClient.ClientSend(cobLocalIP.SelectedIndex + 1, txtSend.Text.Trim());
            try
            {
                strClientRecv = CAMiClsVariable.cClient.ClientReceive((indexClinet + 1),1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //strClientRecv = "";
            }
            if (!string.IsNullOrWhiteSpace(strClientRecv))
            {
                MsgShow(strClientRecv);
                strClientRecv = "";
            }       
        }

        private void btnClear_Click(object sender, EventArgs e)//clear文本框
        {
            //txtSend.Text="";
            txtReceive.Text="";
        }
      
        private void cobLocalIP_SelectedIndexChanged(object sender, EventArgs e)//选择IP地址
        {
            switch(cobLocalIP.SelectedIndex)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:                   
                    txtRemoteIP.Text = CAMiClsVariable .cClient.remoteIP[cobLocalIP.SelectedIndex];
                    txtPort.Text = CAMiClsVariable .cClient.port[cobLocalIP.SelectedIndex];
                    indexClinet = cobLocalIP.SelectedIndex;
                    CAMiClsVariable.cClient.SocketConnect();
                    break;
            }
        }

        private void FrmClient_FormClosing(object sender, FormClosingEventArgs e)//关闭界面
        {
            if(thJudgeStatus!=null)
            {
                thJudgeStatus.Abort();
            }
            if (thReceiveData != null)
            {
                thReceiveData.Abort();
            }
        }       
    }
}
