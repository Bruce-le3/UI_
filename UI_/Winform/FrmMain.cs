using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Justech.Properties;
using System.Diagnostics;
using CommPlc;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Threading;
using ControlIni;

namespace Justech
{
    public partial class Main : Form
    {
        public static Main frmMain = new Main();
        #region 创建类实例    \*        
        GClsScreen gClsScreen = new GClsScreen();//屏幕分辨率类实例    
        public Login login = new Justech.Login();//登出界面实例    
        public Ini ini = new Ini();//ini读写实例
        CAMiClsVariable camiClsVariable = new CAMiClsVariable();//变量类实例
        GClsMethod gClsMethod = new GClsMethod();//外部方法实例
        public Stopwatch sw = new Stopwatch(); //用于记录CT
        public ClsJson clsJson = new ClsJson();
        
        public Main()
        {
            InitializeComponent();            
        }
        
        //FrmIni frmIni = new FrmIni();//初始化界面
        #endregion
        public static bool winformStatus = false;     
        System.Drawing.Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;  //获取屏幕分辨率                                       
        #region self
       
        public Thread thStreamSecondLine, thStreamFirstLine, thMainReset,thNGStreamLine;// 
        public Thread thSignalTest, thALM, thTricolourLight,thAxisAlm;//信号检测，报警检测线程,三色灯线程    
        public Thread thBali, thTossing;
        string currentTime; //记录当前时间
        #region 状态标志位
        public bool isReady = false, isRunning = false, isPreparing = false;//定义复位完成标志，运行中标志，复位中标志
        public bool isRunningPause = false, isEmergency = false, isPreparingPause = false;//定义暂停中标志位，急停中标志位
        public bool isALM = false;//定义报警标志位
        bool isEmgPush = false;//判断急停是否拍下，用于马达激磁作用
        public bool isStopPushJudge = false;//机台暂停时判断      ----------------用于log显示  
        public bool isStartPushJudge = false;//判断启动是否被按下，用于首次显示
        public bool isResetPushJudge = false;//判断复位是否被按下，用于首次显示
        public bool isMachineStop = true ;//判断暂停是否被按下，用于首次显示
        #endregion
        public int montionIoStatus0;//运动轴的IO状态
        public int montionIoStatus1;//运动轴的IO状态
        public int montionIoStatus2;//运动轴的IO状态
        public int montionIoStatus3;//运动轴的IO状态
        public int montionIoStatus4;//运动轴的IO状态
        //public Dictionary<string, string> dictFirstLine = new Dictionary<string, string>();//存储CCD1数据
        //public Dictionary<string, string> dictSecondLine = new Dictionary<string, string>();//存储CCD2数据
        //public int[] productResult = {0,0,0,0,0,0,0,0,0,0,0,0};//产品结果
        ClsRun clsRun = new ClsRun();
        ClsEmptyRun clsEmptyRun = new ClsEmptyRun();
        #endregion
        #region  Load & UnLoad & Resize                \*
        private void Main_Resize(object sender, EventArgs e)//重绘事件
        {
            try
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    winformStatus = true;
                }
                else
                {
                    winformStatus = false;
                    float newX = this.Width / CAMiClsVariable.xValues;//获得比例
                    float newY = this.Height / CAMiClsVariable.yValues;
                    gClsScreen.SetControls(newX, newY, this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }     
        private void Form1_Load(object sender, EventArgs e)
        {
            frmMain = this;
            #region     屏幕自调节 + 初始化按钮    
            try
            {
                this.Resize += new EventHandler(Main_Resize); //添加窗体拉伸重绘事件
                CAMiClsVariable.xValues = this.Width;//记录窗体初始大小
                CAMiClsVariable.yValues = this.Height;
                gClsScreen.SetTag(this);
                //初始化开始按钮状态
                tXbtnStart.status = true;
                tXbtnStart.Refresh();
                tXbtnHome.TX_image = Resources.Home;
                tXbtnPara.TX_image = Resources.Setting;
                tXbtnVision.TX_image = Resources.Camera;
                tXbtnAlm.TX_image = Resources.Alarm;
                tXbtnChart.TX_image = Resources.Data;
                tXbtnMachineName.TX_image = null;
                tXbtnStart.TX_image = Resources.Start;
                tXbtnPause.TX_image = Resources.Push;
                tXbtnStop.TX_image = Resources.Stop;
                tXbtnPause.TXcolor_start_move = Color.SkyBlue;
                tXbtnStop.TXcolor_start_move = Color.Pink;
                tXbtnMachineName.name = "JUSTECH";
                tXbtnExcelOpen.TX_image = Resources.Excel;
                tXbtnVisionOpen.TX_image = Resources.ImagePath;
                tXbtnLogin.TX_image = Resources.User;
                this.Width = rect.Width;
                this.Height = rect.Height;
                this.WindowState = FormWindowState.Maximized;
            }        
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion                      
            #region 初始化参数                     \*
            try
            {               
                timerMain.Enabled = true;//定时器开
                tabControlMain.SelectedTab = tabHome;//显示主界面  
                //frmIni.Show();
                Thread.Sleep(500);
                //---------------------初始化一些内容  
                camiClsVariable.LoadResultSetting();//读取结果设置个数--- 卡的选择和vision选择
                camiClsVariable.ReadRegularVariable();//变量初始化                                                            
                camiClsVariable.LoadErrorCode(dgvErrorCodeDiscription);//加载ErrorCode                
                camiClsVariable.LoadProductCountAndDgvSetting();//加载产量设置和datagridview                
                CAMiClsVariable.cClient.SocketConnect();//---连接相机
                GClsMontion.CardInitial();
                if (CAMiClsVariable.strIOCard == "7432")
                {
                    GClsMontion.IOCard7432Initial();
                }
                else
                {
                    GClsMontion.APEIOCardInitial();
                }
                CAMiClsMethod.ChangeResultDisplay();//初始化结果显示颜色
                SignalOff();//初始化信号
                //frmIni.Close();
                thSignalTest = new Thread(new ThreadStart(SignalJudgeRunning));//启动，暂停，复位等信号实时监测                
                thTricolourLight = new Thread(new ThreadStart(TriColorLightTwinkle));//三色灯 ，按钮灯                          
                thALM = new Thread(new ThreadStart(AlmView));//报警
                thAxisAlm = new Thread(new ThreadStart(AxisAlmTest));                
                thSignalTest.Start();
                thTricolourLight.Start();
                thALM.Start();
                thAxisAlm.Start();
                isEmergency = true;
                cobOperatorID.SelectedIndex = 0;
                MachineResultDisplay("软件启动，请复位！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion
        }                    
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try 
            {
                login.ShowDialog();
                if (login.login_password == true)
                {
                    ini.filePath = CAMiClsVariable.montionIniPath;
                    for (int i = 1; i < 13; i++)
                    {
                        gClsMethod.RecordParaModification("穴位" + i.ToString(), ini.ReadIni("穴位选择", "穴位" + i.ToString()), 1.ToString());
                        ini.WriteIni("穴位选择", "穴位" + i.ToString(), 1.ToString());
                    }
                    CAMiClsCylinder.StreamLine1VacuumOff();
                    CAMiClsCylinder.StreamLine2VacuumOff();
                    GClsMontion.WriteCardExtendOutputBit(0, 13, 0);//启动，停止，复位灯
                    GClsMontion.WriteCardExtendOutputBit(0, 14, 0);//
                    GClsMontion.WriteCardExtendOutputBit(0, 15, 0);//                    
                    AbortThread();
                    if (thALM != null)
                    {
                        thALM.Abort();
                    }
                    if (thAxisAlm != null)
                    {
                        thAxisAlm.Abort();
                    }
                    if (thSignalTest != null)
                    {
                        thSignalTest.Abort();
                    }
                    if (thTricolourLight != null)
                    {
                        thTricolourLight.Abort();
                    }
                    GClsMontion.CardClose();
                    GClsMontion.IOCard7432Close();
                    CAMiClsVariable.cClient.DisConnectClient(1);
                    CAMiClsVariable.cClient.DisConnectClient(2);                    
                    System.Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }            
            }      
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public int errNum = 0; 
        private void timerMain_Tick(object sender, EventArgs e)
        {
            currentTime = DateTime.Now.ToString();
            errNum++;
            if(errNum > 200)
            {
                errNum = 0;
                ClsJson.errorCode = -1;
            }
            Application.DoEvents();             
        }
        #endregion
        #region 信号检测                                    */
        public void PauseCheck()//检测暂停状态
        {
            while (isRunningPause)
            {
                if (isStopPushJudge == true)
                {
                    ListBoxDisplay("Stop ！！");//------------------------界面显示
                    isStopPushJudge = false;
                }
                Thread.Sleep(100);
                Application.DoEvents();
            }
            while(isPreparingPause)
            {
                if (isStopPushJudge == true)
                {
                    ListBoxDisplay("Stop ！！");//------------------------界面显示
                    isStopPushJudge = false;
                }
                Thread.Sleep(100);
                Application.DoEvents();
            }
        }
        private void AbortThread()//急停时停止复位和自动运行线程
        {
            try
            {
                if (thStreamFirstLine != null)
                {
                    thStreamFirstLine.Abort();
                }
                if (thStreamSecondLine != null)
                {
                    thStreamSecondLine.Abort();
                }
                if (thMainReset != null)
                {
                    thMainReset.Abort();
                }
                if (thNGStreamLine != null)
                {
                    thNGStreamLine.Abort();
                }
                if(thBali != null)
                {
                    thBali.Abort();
                }
                if(thTossing != null)
                {
                    thTossing.Abort();
                }
            }
            catch (Exception ex)
            {
                gClsMethod.LogRecord(ex.ToString());
            }
        }
        private void SignalOff()//用于拍急停和初始化时的信号
        {
            //中间交互信号全部off                       
            GClsMontion.WriteCardOutputBit(0, 0, 0);//SMEMA要料信号
            GClsMontion.WriteCardOutputBit(0, 0, 1);//SMEMA送料信号
            GClsMontion.WriteCardExtendOutputBit(0, 9, 0);//NG流线停掉
            GClsMontion.WriteCardExtendOutputBit(0, 6, 0);//流线1停掉
            GClsMontion.WriteCardExtendOutputBit(1, 6, 0);//流线2停掉
            GClsMontion.WriteCardExtendOutputBit(0, 12, 0);//关闭背光
            //清buffer
            CAMiClsVariable.isStreamLine2Ready = true;
            CAMiClsVariable.isStreamLine1Done = false;
            CAMiClsVariable.isProductLocateInStreamLine2 = false;
            CAMiClsVariable.isNGTrayOK = false;
            CAMiClsVariable.isProductOkNG = true;            
            GClsMontion.WriteCardExtendOutputBit(0, 13, 0);//启动灯
            GClsMontion.WriteCardExtendOutputBit(0, 14, 1);//复位灯
            GClsMontion.WriteCardExtendOutputBit(0, 15, 0);//停止灯
            CAMiClsVariable.isNGTrayOK = false;
            CAMiClsBali.isSendOK = false;
            CAMiClsVariable.isTrayChange = false;
        }
        private void SignalJudgeRunning()//各种信号判断
        {
            while (true)
            {
                try
                {
                    #region 按钮按下判断          
                    if (GClsMontion.ReadCardInputBit(0, 0) == 1)//启动按钮按下ClsMontion.ReadCardExtendInputBit(0, 0) == 1
                    {
                        if (!isStartPushJudge )
                        {
                           ListBoxDisplay("按钮--机台启动按钮按下-----------------");
                        }
                        isStartPushJudge = true;
                        isResetPushJudge = false;
                        MachineAutoRun();
                    }
                    if (GClsMontion.ReadCardInputBit(0, 2) == 1)//复位按钮按下ClsMontion.ReadCardExtendInputBit(0,2)==1
                    {
                        isStartPushJudge = false;
                        if (!isResetPushJudge)
                        {
                            ListBoxDisplay("按钮--机台复位按钮按下-----------------");
                            if (isReady & isRunning == false)//提示复位完成
                            {
                                ListBoxDisplay("机台复位已完成-----------------------");
                            }
                        }
                        isResetPushJudge = true;
                        MachineReset();
                    }                    
                    if (GClsMontion.ReadCardInputBit(0, 3) == 1 )//暂停按钮按下按钮按下(ClsMontion.ReadCardExtendInputBit(0, 3) == 1) 或(ClsMontion.ReadCardExtendInputBit(0, 4) == 0)门禁感应到  || (ClsMontion.ReadCardExtendInputBit(0,5) == 0)
                    {//|| ClsMontion.ReadCardInputBit(1, 0) == 0 || ClsMontion.ReadCardInputBit(1, 1) == 0 
                        isStartPushJudge = false;
                        isResetPushJudge = false;
                        isMachineStop = true;
                        if (isRunning)
                        {
                            isRunningPause = true;
                            isRunning = false;
                            //if (ClsMontion.ReadCardInputBit(1, 0) == 0 || ClsMontion.ReadCardInputBit(1, 1) == 0  )
                            //{
                            //    ListBoxDisplay("按钮--门禁感应感应到-----------------");
                            //    SystemStatusDisplay("机台因门禁感应暂停，请确保安全后按启动按钮继续！");
                            //    ClsVariable.errorCodeNo = 133;//门禁感应报警
                            //}
                            //else
                            //{
                            //    SystemStatusDisplay("机台暂停中");
                            //}
                            MachineStatusDisplay("Pause");
                        }                      
                        if(isPreparing)
                        {
                            isPreparing = false;
                            isPreparingPause = true;
                            MachineStatusDisplay("Pause");
                        }
                    }
                    if (CAMiClsVariable.isEntranceOpen)//门禁打开
                    {
                        if (GClsMontion.ReadCardInputBit(1, 0) == 0 || GClsMontion.ReadCardInputBit(1, 1) == 0)
                        {
                            isStartPushJudge = false;
                            isResetPushJudge = false;
                            isMachineStop = true;
                            if (isRunning)
                            {
                                isRunningPause = true;
                                isRunning = false;
                                MachineStatusDisplay("安全门打开！！！请注意安全");
                            }
                        }
                    }
                    #endregion
                    if (isALM)
                    {
                        isStartPushJudge = false;
                        isResetPushJudge = false;
                        //isMachineStop = true;
                        if (isRunning)
                        {
                            isRunningPause = true;//机台报警中暂停
                            isRunning = false;                            
                        }
                    }
                    #region 急停或轴报警时判断
                    MC1104.jmc_mc1104_get_io_status(0, out montionIoStatus0);
                    MC1104.jmc_mc1104_get_io_status(1, out montionIoStatus1);
                    MC1104.jmc_mc1104_get_io_status(2, out montionIoStatus2);
                    MC1104.jmc_mc1104_get_io_status(3, out montionIoStatus3);
                    MC1104.jmc_mc1104_get_io_status(4, out montionIoStatus4);
                    if ((montionIoStatus0 & 2) == 2 || (montionIoStatus1 & 2) == 2 || (montionIoStatus2 & 2) == 2 
                        || (montionIoStatus3 & 2) == 2 || (montionIoStatus4 & 2) == 2)//急停按钮按下或检测到急停信号  
                    {//ClsMontion.ReadCardInputBit(0, 1) == 0 ||急停按钮按下
                        if (!isEmgPush)
                        {
                            isStartPushJudge = false;
                            isResetPushJudge = false;
                            isMachineStop = true; 
                            isEmgPush = true;//急停拍下
                            isEmergency = true;
                            isALM = false;
                            isReady = false;
                            isPreparing = false;
                            isRunningPause = false;
                            isPreparingPause = false;
                            isRunning = false;
                            isStopPushJudge = false;
                            isStartPushJudge = false;//判断启动是否被按下，用于首次显示
                            isResetPushJudge = false;//判断启动是否被按下，用于首次显示
                            AbortThread();//急停时停止线程
                            SignalOff();                                                
                            if (GClsMontion.ReadCardInputBit(0, 1) == 0)
                            {
                                CAMiClsVariable.errorCodeNo = 128;//急停按下errorCode
                                
                                ClsJson.intMachineState = (int) ClsJson.MachineState.down;
                                ClsJson.strStateChangeTime = ClsJson.GetmachineTime();
                                ClsJson.strMessage = "Emergency stop button";
                                ClsJson.strCode = "F02ESSP-01-01";
                            }
                        }                        
                    }
                    else //急停松开或轴报警结束
                    {                       
                        if (isEmgPush)
                        {
                            isStartPushJudge = false;
                            isResetPushJudge = false;
                            isEmgPush = false;

                            ClsJson.strSeverity = ClsJson.errSeverity.critical.ToString();
                            ClsJson.strResolvedTime = ClsJson.GetmachineTime();
                            ClsJson.strKey = "Emg is over";

                            clsJson.UpLoadMachineState(ClsJson.intMachineState, ClsJson.strStateChangeTime, ClsJson.strMessage, ClsJson.strCode, 0);
                            Thread.Sleep(10);
                            clsJson.UpLoadMachineErrState(ClsJson.strMessage, ClsJson.strCode, ClsJson.strSeverity, ClsJson.strStateChangeTime, ClsJson.strResolvedTime, ClsJson.strKey);

                            Thread.Sleep(3000);
                            for (int i = 0; i < 5; i++)//马达激磁
                            {
                                GClsMontion.ServoOn(i, 1);
                            }
                        }
                    }
                    #endregion
                    if (isMachineStop)//判断机台是否处于暂停或者停止
                    {
                        tXbtnStart.BeginInvoke(new Action(() => { tXbtnStart.status = true; }));
                        tXbtnStart.BeginInvoke(new Action(() => { tXbtnStart.Refresh(); }));
                        tXbtnPause.BeginInvoke(new Action(() => { tXbtnPause.status = false; }));
                        tXbtnPause.BeginInvoke(new Action(() => { tXbtnPause.Refresh(); }));
                        tXbtnStop.BeginInvoke(new Action(() => { tXbtnStop.status = false; }));
                        tXbtnStop.BeginInvoke(new Action(() => { tXbtnStop.Refresh(); }));
                        isMachineStop = false;
                    }
                }
                catch (Exception ex)
                { 
                    MessageBox.Show(ex.ToString()); 
                    gClsMethod.LogRecord(ex.ToString());
                }
                Thread.Sleep(20);
                Application.DoEvents();
            }
        }      
        public void TriColorLightTwinkle()//三色灯亮
        {
            //9，10,11分别为，,yellow,green,red,第二张卡
            while (true)
            {
                try
                {
                    if (isRunningPause)//暂停时
                    {
                        //绿灯闪烁
                        if (isALM)
                        {
                            GClsMontion.TricolourLightTwinkle(11, 10, 9, "red", 1);//红灯闪烁   
                            GClsMontion.WriteCardExtendOutputBit(0, 13, 0);//
                            GClsMontion.WriteCardExtendOutputBit(0, 14, 1);//复位灯
                            GClsMontion.WriteCardExtendOutputBit(0, 15, 0);//  
                        }
                        else //非报警时---//亮启动灯或复位灯
                        {
                            GClsMontion.TricolourLightTwinkle(11, 10, 9, "green", 1);
                            GClsMontion.WriteCardExtendOutputBit(0, 13, 1);//启动灯亮
                            GClsMontion.WriteCardExtendOutputBit(0, 14, 0);//复位灯
                            GClsMontion.WriteCardExtendOutputBit(0, 15, 0);//停止灯
                        }                                                                       
                    }
                    if (isPreparingPause)//复位灯亮
                    {
                        GClsMontion.WriteCardExtendOutputBit(0, 13, 0);//
                        GClsMontion.WriteCardExtendOutputBit(0, 14, 1);//
                        GClsMontion.WriteCardExtendOutputBit(0, 15, 0);//
                    }
                    if (isRunning)//显示运行中，绿灯亮
                    {                        
                        GClsMontion.TricolourLight(11, 10, 9, 0, 1, 0, 1);//绿灯亮
                        GClsMontion.WriteCardExtendOutputBit(0, 13, 0);//
                        GClsMontion.WriteCardExtendOutputBit(0, 14, 0);//
                        GClsMontion.WriteCardExtendOutputBit(0, 15, 1);//暂停灯亮                                
                    }
                    if (isReady && isRunning == false)//机台准备好，黄灯亮
                    {                        
                        GClsMontion.TricolourLight(11, 10, 9, 0, 0, 1, 1);//黄灯亮  
                        GClsMontion.WriteCardExtendOutputBit(0, 13, 1);//
                        GClsMontion.WriteCardExtendOutputBit(0, 14, 0);//
                        GClsMontion.WriteCardExtendOutputBit(0, 15, 0);//                   
                    }
                    if (isPreparing) //机台复位中，黄灯亮
                    {
                        GClsMontion.TricolourLight(11, 10, 9, 0, 0, 1, 1);//黄灯亮 
                        GClsMontion.WriteCardExtendOutputBit(0, 13, 0);//
                        GClsMontion.WriteCardExtendOutputBit(0, 14, 0);//
                        GClsMontion.WriteCardExtendOutputBit(0, 15, 1);//
                    }                   
                    if (isEmergency)
                    {
                        GClsMontion.TricolourLight(11, 10, 9, 1, 0, 0, 1);//报警红灯亮                         
                    }
                    Thread.Sleep(20);
                    Application.DoEvents();
                }
                catch (Exception ex)
                {
                    gClsMethod.LogRecord(ex.ToString());
                }              
            }
        }
        public void AlmView()//检测alm状态
        {
            try
            {
                while (true)
                {
                    if (CAMiClsVariable.errorCodeNo != -1)
                    {
                        isALM = true;
                        if (dgvAlmDisplay.Rows.Count > 100)
                        {
                            dgvAlmDisplay.BeginInvoke(new Action(() => { dgvAlmDisplay.Rows.Clear(); }));
                        }
                        ini.filePath = CAMiClsVariable.errorCodeIniPath;
                        tabControlMain.BeginInvoke(new Action(() => { tabControlMain.SelectedTab = tabALM; }));
                        camiClsVariable.ErrorCodeDateTime();//获取errorCode的时间
                        string[] info = new string[7];
                        info[0] = CAMiClsVariable.errorData;
                        info[1] = CAMiClsVariable.errorTime;
                        info[2] = ini.ReadIni(CAMiClsVariable.errorCodeNo.ToString(), "Code");
                        info[3] = ini.ReadIni(CAMiClsVariable.errorCodeNo.ToString(), "Discription");
                        info[4] = ini.ReadIni(CAMiClsVariable.errorCodeNo.ToString(), "Reslove");
                        info[5] = ini.ReadIni(CAMiClsVariable.errorCodeNo.ToString(), "EN-Discription");
                        info[6] = ini.ReadIni(CAMiClsVariable.errorCodeNo.ToString(), "hiveCode");
                        dgvAlmDisplay.BeginInvoke(new Action(() => { dgvAlmDisplay.Rows.Insert(0, info); }));
                        lblErrorCodeDisplay.BeginInvoke(new Action(() => { lblErrorCodeDisplay.Text = "Code: " + info[2] + "-" + info[3]; }));
                        gClsMethod.AlmRecord(info[2].ToString() + "，" + info[3].ToString() + "，" + info[4].ToString());
                        //webService

                        ClsJson.intMachineState = (int)ClsJson.MachineState.plannedDowntime;
                        ClsJson.strStateChangeTime = ClsJson.GetmachineTime();
                        ClsJson.strMessage = info[5];
                        ClsJson.strCode = info[6];
                        clsJson.UpLoadMachineState(ClsJson.intMachineState, ClsJson.strStateChangeTime, ClsJson.strMessage, ClsJson.strCode, CAMiClsVariable.errorCodeNo);
                       
                        //
                        CAMiClsVariable.errorCodeNo = -1;
                    }
                    Thread.Sleep(10);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                gClsMethod.LogRecord(ex.ToString());
            }

        }
        public void AxisAlmTest()//轴报警检测
        {
            int AxisIoStatus0;//运动轴的IO状态
            int AxisIoStatus1;//运动轴的IO状态
            int AxisIoStatus2;//运动轴的IO状态
            int AxisIoStatus3;//运动轴的IO状态
            int AxisIoStatus4;//运动轴的IO状态           
            while (true)
            {
                try
                {
                    MC1104.jmc_mc1104_get_io_status(0, out AxisIoStatus0);
                    MC1104.jmc_mc1104_get_io_status(1, out AxisIoStatus1);
                    MC1104.jmc_mc1104_get_io_status(2, out AxisIoStatus2);
                    MC1104.jmc_mc1104_get_io_status(3, out AxisIoStatus3);
                    MC1104.jmc_mc1104_get_io_status(4, out AxisIoStatus4);
                    if ((AxisIoStatus0 & 2) == 2)//CCD1-X轴报警
                    {
                        CAMiClsVariable.errorCodeNo = 135;
                        Thread.Sleep(1000);
                    }
                    if ((AxisIoStatus1 & 2) == 2)//CCD1-Y轴报警
                    {
                        CAMiClsVariable.errorCodeNo = 137;
                        Thread.Sleep(1000);
                    }
                    if ((AxisIoStatus2 & 2) == 2)//CCD2-X轴报警
                    {
                        CAMiClsVariable.errorCodeNo = 139;
                        Thread.Sleep(1000);
                    }
                    if ((AxisIoStatus3 & 2) == 2)//CCD2-Y轴报警
                    {
                        CAMiClsVariable.errorCodeNo = 141;
                        Thread.Sleep(1000);
                    }
                    if ((AxisIoStatus4 & 2) == 2)//CCD2-Z轴报警
                    {
                        CAMiClsVariable.errorCodeNo = 143;
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(100);
                    Application.DoEvents();
                }
                catch (Exception ex)
                {
                    gClsMethod.LogRecord(ex.ToString());
                }
            }            
        }
        #endregion
        #region 复位和启动
        private void MachineReset()//复位程序
        {
            if (isALM)
            {
                isALM = false; //------------------------------清报警+启动,报警窗体将所有报警显示                              
                tabControlMain.BeginInvoke(new Action(() => { tabControlMain.SelectedTab = tabHome; }));//显示主界面
                dgvAlmDisplay.BeginInvoke(new Action(() => { dgvAlmDisplay.Rows.Clear(); }));//清除报警                             
            }
            if (isPreparing || isReady || isRunning)
            {
                return;
            }                       
            if (isEmergency || isPreparingPause )//急停或者复位中暂停，需复位
            {
                try
                {
                    isResetPushJudge = false; 
                    tXbtnStart.BeginInvoke(new Action(() => { tXbtnStart.status = false; }));
                    tXbtnStart.BeginInvoke(new Action(() => { tXbtnStart.Refresh(); }));
                    tXbtnPause.BeginInvoke(new Action(() => { tXbtnPause.status = true; }));
                    tXbtnPause.BeginInvoke(new Action(() => { tXbtnPause.Refresh(); }));
                    tXbtnStop.BeginInvoke(new Action(() => { tXbtnStop.status = true; }));
                    tXbtnStop.BeginInvoke(new Action(() => { tXbtnStop.Refresh(); }));
                    if (isPreparingPause)
                    {
                        try
                        {
                            thMainReset.Abort();
                        }
                        catch { }
                    }
                    isPreparingPause = false;
                    isEmergency = false;
                    isPreparing = true;//复位中标志
                    MachineStatusDisplay("Preparing");
                    thMainReset = new Thread(new ThreadStart(clsRun.ThMainReset));//机台复位  
                    thMainReset.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private void MachineAutoRun()//自动程序
        {
            if (isRunning)//正常运行中不起作用
            {
                return;
            }
            if (isPreparing)//复位中不起作用
            {
                return;
            }
            if (isRunningPause)//运行中暂停时继续执行
            {
                tXbtnStart.BeginInvoke(new Action(() => { tXbtnStart.status = false; }));
                tXbtnStart.BeginInvoke(new Action(() => { tXbtnStart.Refresh(); }));
                tXbtnPause.BeginInvoke(new Action(() => { tXbtnPause.status = true; }));
                tXbtnPause.BeginInvoke(new Action(() => { tXbtnPause.Refresh(); }));
                tXbtnStop.BeginInvoke(new Action(() => { tXbtnStop.status = true; }));
                tXbtnStop.BeginInvoke(new Action(() => { tXbtnStop.Refresh(); }));
                isRunningPause = false;
                isRunning = true;
                isStopPushJudge = true;
                MachineStatusDisplay("Running");

                clsJson.MachineRunningInfoGet();//得到running信息
            }
            if (isPreparingPause)//复位中暂停时，复位重新开始
            {                
                MachineStatusDisplay("Reset");
                MachineReset();
            }
            if (isReady & isRunning == false)//复位完成，正常启动程序
            {
                try
                {
                    tXbtnStart.BeginInvoke(new Action(() => { tXbtnStart.status = false; }));
                    tXbtnStart.BeginInvoke(new Action(() => { tXbtnStart.Refresh(); }));
                    tXbtnPause.BeginInvoke(new Action(() => { tXbtnPause.status = true; }));
                    tXbtnPause.BeginInvoke(new Action(() => { tXbtnPause.Refresh(); }));
                    tXbtnStop.BeginInvoke(new Action(() => { tXbtnStop.status = true; }));
                    tXbtnStop.BeginInvoke(new Action(() => { tXbtnStop.Refresh(); }));
                    isRunning = true;
                    isReady = false;
                    isStopPushJudge = true;
                    if(!CAMiClsVariable.isEmptyRun)//正常生产
                    {
                        thStreamFirstLine = new Thread(new ThreadStart(clsRun.ThStreamFirstLineRunning));
                        thStreamSecondLine = new Thread(new ThreadStart(clsRun.ThStreamSecondLineRunning));
                        thNGStreamLine = new Thread(new ThreadStart(clsRun.ThNGStreamLineRunning));
                        thBali = new Thread(new ThreadStart(CAMiClsBali.SendPDCAData));//发送PDCA数据
                        thBali.Start();
                        thTossing = new Thread(new ThreadStart(clsRun.ThTossing));
                        thTossing.Start();
                    }
                    else //空跑
                    {
                        thStreamFirstLine = new Thread(new ThreadStart(clsEmptyRun.ThStreamFirstLineEmptyRunning));
                        thStreamSecondLine = new Thread(new ThreadStart(clsEmptyRun.ThStreamSecondLineEmptyRunning));
                        thNGStreamLine = new Thread(new ThreadStart(clsEmptyRun.ThNGStreamLineEmptyRunning));
                    } 
                    thStreamFirstLine.Start();//启动流线1线程
                    thStreamSecondLine.Start();//启动流线2线程
                    thNGStreamLine.Start();//启动NG流线线程
                    clsJson.MachineRunningInfoGet();//得到running信息
                }
                catch (Exception ex)
                {
                    gClsMethod.LogRecord(ex.ToString());
                }
            }           
        }
       
        #endregion      
        #region  按钮按下                             */
        private void tXbtnStart_Click(object sender, EventArgs e)//start
        {
            tXbtnStart.status = false;
            tXbtnStart.Refresh();
            tXbtnPause.status = true;
            tXbtnPause.Refresh();
            tXbtnStop.status = true;
            tXbtnStop.Refresh();
            if (isEmergency)//判断是否急停拍下
            {
                ListBoxDisplay("按钮--机台复位--------------------");
                MachineReset();
            }
            else 
            {
                ListBoxDisplay("按钮--界面启动按钮按下-----------------");
                MachineAutoRun();
            }
        }
        private void tXbtnPause_Click(object sender, EventArgs e)//pause
        {
            tXbtnStart.status = true;
            tXbtnStart.Refresh();
            tXbtnPause.status = false;
            tXbtnPause.Refresh();
            tXbtnStop.status = false;
            tXbtnStop.Refresh();
            ListBoxDisplay("按钮--界面暂停按钮按下-----------------");
            if (isRunning)//运行中暂停
            {
                isRunning = false;
                isRunningPause = true;
                MachineStatusDisplay("Pause");
            }
            if (isPreparing)
            {
                isPreparing = false;
                isPreparingPause = true;
                MachineStatusDisplay("Pause");
            }
            else
            {
                return;
            }
        }
        private void tXbtnStop_Click(object sender, EventArgs e)//stop 同暂停
        {
            tXbtnStart.status = true;
            tXbtnStart.Refresh();
            tXbtnPause.status = false;
            tXbtnPause.Refresh();
            tXbtnStop.status = false;
            tXbtnStop.Refresh();
            ListBoxDisplay("按钮--界面停止按钮按下-----------------");
            if (isRunning)//运行中暂停
            {
                isRunning = false;
                isRunningPause = true;
                MachineStatusDisplay("Pause");
            }
            if (isPreparing)
            {
                isPreparing = false;
                isPreparingPause = true;
                MachineStatusDisplay("Pause");
            }
            else
            {
                return;
            }
        }
        private void btnOpenErrorCode_Click(object sender, EventArgs e)//显示errorCode界面
        {
            tabControlMain.SelectedTab = tabErrorCode;
        }       
        #endregion
        #region  界面打开                     */
        private void tXbtnHome_Click(object sender, EventArgs e)//home
        {          
            tabControlMain.SelectedTab = tabHome;
            tXbtnHome.status = true;
            tXbtnHome.Refresh();
            tXbtnPara.status = false;
            tXbtnPara.Refresh();
            tXbtnVision.status = false;
            tXbtnVision.Refresh();
            tXbtnAlm.status = false;
            tXbtnAlm.Refresh();
            tXbtnChart.status = false;
            tXbtnChart.Refresh();
            tXbtnLogin.status = false;
            tXbtnLogin.Refresh();
            tXbtnExcelOpen.status = false;
            tXbtnExcelOpen.Refresh();
            tXbtnVisionOpen.status = false;
            tXbtnVisionOpen.Refresh();
            tXbtnMachineName.status = false;
            tXbtnMachineName.Refresh();
        }
        FrmPara frmPara = null;
        private void tXbtnPara_Click(object sender, EventArgs e)//parameter
        {
            if (frmPara == null || frmPara.IsDisposed)//判断控件是否为空或被释放
            {
                frmPara = new FrmPara();
                frmPara.Show();
            }
            else
            {
                frmPara.Activate();//激活窗体并给予它焦点
            } 
            frmPara.Show();

            tXbtnHome.status = false;
            tXbtnHome.Refresh();
            tXbtnPara.status = true;
            tXbtnPara.Refresh();
            tXbtnVision.status = false;
            tXbtnVision.Refresh();
            tXbtnAlm.status = false;
            tXbtnAlm.Refresh();
            tXbtnChart.status = false;
            tXbtnChart.Refresh();
            tXbtnLogin.status = false;
            tXbtnLogin.Refresh();
            tXbtnExcelOpen.status = false;
            tXbtnExcelOpen.Refresh();
            tXbtnVisionOpen.status = false;
            tXbtnVisionOpen.Refresh();
            tXbtnMachineName.status = false;
            tXbtnMachineName.Refresh();
        
        }

        private void tXbtnVision_Click(object sender, EventArgs e)//vision
        {           
            tabControlMain.SelectedTab = tabVision;

            tXbtnHome.status = false;
            tXbtnHome.Refresh();
            tXbtnPara.status = false ;
            tXbtnPara.Refresh();
            tXbtnVision.status = true ;
            tXbtnVision.Refresh();
            tXbtnAlm.status = false;
            tXbtnAlm.Refresh();
            tXbtnChart.status = false;
            tXbtnChart.Refresh();
            tXbtnLogin.status = false;
            tXbtnLogin.Refresh();
            tXbtnExcelOpen.status = false;
            tXbtnExcelOpen.Refresh();
            tXbtnVisionOpen.status = false;
            tXbtnVisionOpen.Refresh();
            tXbtnMachineName.status = false;
            tXbtnMachineName.Refresh();
        }

        private void tXbtnAlm_Click(object sender, EventArgs e)//alm
        {           
            tabControlMain.SelectedTab = tabALM;

            tXbtnHome.status = false;
            tXbtnHome.Refresh();
            tXbtnPara.status = false ;
            tXbtnPara.Refresh();
            tXbtnVision.status = false;
            tXbtnVision.Refresh();
            tXbtnAlm.status = true ;
            tXbtnAlm.Refresh();
            tXbtnChart.status = false;
            tXbtnChart.Refresh();
            tXbtnLogin.status = false;
            tXbtnLogin.Refresh();
            tXbtnExcelOpen.status = false;
            tXbtnExcelOpen.Refresh();
            tXbtnVisionOpen.status = false;
            tXbtnVisionOpen.Refresh();
            tXbtnMachineName.status = false;
            tXbtnMachineName.Refresh();
        }

        private void tXbtnChart_Click(object sender, EventArgs e)//chart
        {
            tabControlMain.SelectedTab = tabChart;

            tXbtnHome.status = false;
            tXbtnHome.Refresh();
            tXbtnPara.status = false ;
            tXbtnPara.Refresh();
            tXbtnVision.status = false;
            tXbtnVision.Refresh();
            tXbtnAlm.status = false;
            tXbtnAlm.Refresh();
            tXbtnChart.status = true ;
            tXbtnChart.Refresh();
            tXbtnLogin.status = false;
            tXbtnLogin.Refresh();
            tXbtnExcelOpen.status = false;
            tXbtnExcelOpen.Refresh();
            tXbtnVisionOpen.status = false;
            tXbtnVisionOpen.Refresh();
            tXbtnMachineName.status = false;
            tXbtnMachineName.Refresh();
        }

        private void tXbtnLogin_Click(object sender, EventArgs e)//Loginn
        {
            tabControlMain.SelectedTab = tabLogin;

            tXbtnHome.status = false;
            tXbtnHome.Refresh();
            tXbtnPara.status = false;
            tXbtnPara.Refresh();
            tXbtnVision.status = false;
            tXbtnVision.Refresh();
            tXbtnAlm.status = false;
            tXbtnAlm.Refresh();
            tXbtnChart.status = false;
            tXbtnChart.Refresh();
            tXbtnLogin.status = true ;
            tXbtnLogin.Refresh();
            tXbtnExcelOpen.status = false;
            tXbtnExcelOpen.Refresh();
            tXbtnVisionOpen.status = false;
            tXbtnVisionOpen.Refresh();
            tXbtnMachineName.status = false;
            tXbtnMachineName.Refresh();
        }

        private void tXbtnExcelOpen_Click(object sender, EventArgs e)//open Excel
        {
            string execlFileName ="D:\\DATA";//生产数据路径
            System.Diagnostics.Process .Start(execlFileName);//打开指定文件夹

            tabControlMain.SelectedTab = tabHome;
            tXbtnHome.status = false;
            tXbtnHome.Refresh();
            tXbtnPara.status = false;
            tXbtnPara.Refresh();
            tXbtnVision.status = false;
            tXbtnVision.Refresh();
            tXbtnAlm.status = false;
            tXbtnAlm.Refresh();
            tXbtnChart.status = false;
            tXbtnChart.Refresh();
            tXbtnLogin.status = false;
            tXbtnLogin.Refresh();
            tXbtnExcelOpen.status = true ;
            tXbtnExcelOpen.Refresh();
            tXbtnVisionOpen.status = false;
            tXbtnVisionOpen.Refresh();
            tXbtnMachineName.status = false;
            tXbtnMachineName.Refresh();
        }

        private void tXbtnVisionOpen_Click(object sender, EventArgs e)//open Vision
        {
            string visionFileName = "D:\\Cognex\\Images";//图片路径
            System.Diagnostics.Process.Start(visionFileName);//打开指定文件夹

            tabControlMain.SelectedTab = tabHome;
            tXbtnHome.status = false;
            tXbtnHome.Refresh();
            tXbtnPara.status = false;
            tXbtnPara.Refresh();
            tXbtnVision.status = false;
            tXbtnVision.Refresh();
            tXbtnAlm.status = false;
            tXbtnAlm.Refresh();
            tXbtnChart.status = false;
            tXbtnChart.Refresh();
            tXbtnLogin.status = false;
            tXbtnLogin.Refresh();
            tXbtnExcelOpen.status = false;
            tXbtnExcelOpen.Refresh();
            tXbtnVisionOpen.status = true ;
            tXbtnVisionOpen.Refresh();
            tXbtnMachineName.status = false;
            tXbtnMachineName.Refresh();
        }

        private void tXbtnMachineName_Click(object sender, EventArgs e)//machineName
        {
            tabControlMain.SelectedTab = tabHome;
            tXbtnHome.status = false;
            tXbtnHome.Refresh();
            tXbtnPara.status = false;
            tXbtnPara.Refresh();
            tXbtnVision.status = false;
            tXbtnVision.Refresh();
            tXbtnAlm.status = false;
            tXbtnAlm.Refresh();
            tXbtnChart.status = false;
            tXbtnChart.Refresh();
            tXbtnLogin.status = false;
            tXbtnLogin.Refresh();
            tXbtnExcelOpen.status = false;
            tXbtnExcelOpen.Refresh();
            tXbtnVisionOpen.status = false;
            tXbtnVisionOpen.Refresh();
            tXbtnMachineName.status = true ;
            tXbtnMachineName.Refresh();
        }
        ResultSetting frmResult = null;
        private void btnOpenResultSetting_Click(object sender, EventArgs e)//打开结果设置
        {
            tXbtnHome.status = true;
            tXbtnHome.Refresh();
            tXbtnPara.status = false;
            tXbtnPara.Refresh();
            tXbtnVision.status = false;
            tXbtnVision.Refresh();
            tXbtnAlm.status = false;
            tXbtnAlm.Refresh();
            tXbtnChart.status = false;
            tXbtnChart.Refresh();
            tXbtnLogin.status = false;
            tXbtnLogin.Refresh();
            tXbtnExcelOpen.status = false;
            tXbtnExcelOpen.Refresh();
            tXbtnVisionOpen.status = false;
            tXbtnVisionOpen.Refresh();
            tXbtnMachineName.status = false;
            tXbtnMachineName.Refresh();
            tabControlMain.SelectedTab = tabHome;
            if (frmResult == null || frmResult.IsDisposed)//判断控件是否为空或被释放
            {
                frmResult = new ResultSetting();
                frmResult.Show();
            }
            else
            {
                frmResult.Activate();//激活窗体并给予它焦点
            }
            frmResult.Show();            
        }
        #endregion      
        #region 跨线程更改控件信息                                 */

        public delegate void StatusDisplay(string msg);//异步委托
        public void MachineStatusDisplay(string msg)//显示当前机台运行状态
        {
            if (lblMachineStatus.InvokeRequired)
            {
                StatusDisplay sd = new StatusDisplay(MachineStatusDisplay);
                lblMachineStatus.Invoke(sd, new string[] { msg });
            }
            else
            {
                lblMachineStatus.Text = msg;
            }
            gClsMethod.LogRecord(msg);
        }
        public void MachineResultDisplay(string msg)//显示结果NG状态信息
        {
            if(lblSystemResultStatus.InvokeRequired)
            {
                StatusDisplay ssd = new StatusDisplay(MachineResultDisplay);
                lblSystemResultStatus.Invoke(ssd, new string[] { msg });
            }
            else
            {
                lblSystemResultStatus.Text = msg;
            }
            gClsMethod.LogRecord(msg);
        }
        public void UPHRecode(string msg)//显示UPH
        {
            if (lblUPH.InvokeRequired)
            {
                StatusDisplay sd = new StatusDisplay(UPHRecode);
                lblUPH.Invoke(sd, new string[] { msg });
            }
            else
            {
                lblUPH.Text = msg;
            }
        }
        public delegate void ProductCountDisplay(Label lbl, string msg);
        public void ProductCountMeasure(Label lbl, string msg)//显示产量
        { 
            if (lbl.InvokeRequired)
            {
                ProductCountDisplay sd = new ProductCountDisplay(ProductCountMeasure);
                lbl.Invoke(sd, new object[] { lbl, msg });
            }
            else
            {
                lbl.Text = msg;
            }
        }

        public delegate void ChangeLabelColor(Label lbl, Color color);//异步委托
        public void ChangeColor(Label lbl, Color color)//改变label颜色
        {
            if (lbl.InvokeRequired)
            {
                ChangeLabelColor clc = new ChangeLabelColor(ChangeColor);
                lbl.Invoke(clc, new object[] { lbl, color });
            }
            else
            {
                lbl.BackColor = color;
                //label.BeginInvoke(new Action(() => { label.BackColor = Color.Green; }));
            }
        }
        public delegate void ListBoxAddInfo(string info);
        string logEnabled;//log写入使能信号
        public void ListBoxDisplay(string info)//listbox显示运行信息
        {
            ini.filePath = CAMiClsVariable.montionIniPath;
            if (listbRunningInfo.InvokeRequired)
            {
                ListBoxAddInfo lb = new ListBoxAddInfo(ListBoxDisplay);
                listbRunningInfo.Invoke(lb, new string[] { info });
            }
            else
            {
                listbRunningInfo.Items.Add(currentTime + "   " + info);
                listbRunningInfo.SelectedIndex = listbRunningInfo.Items.Count - 1;
                if (listbRunningInfo.Items.Count > 150)
                {
                    listbRunningInfo.Items.Clear();
                }
                logEnabled = ini.ReadIni("参数设置", "LogRecordEnable");//读取logEnabled
                if (logEnabled.Equals("True"))
                {
                    gClsMethod.LogRecord(info);//将信息写入Log文件
                }
            }
        }
        #endregion

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否产量清零？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ini.filePath = CAMiClsVariable.montionIniPath;
                ini.WriteIni("参数设置", "当前产量", "0");
                ini.WriteIni("参数设置", "良品", "0");
                ini.WriteIni("参数设置", "不良品", "0");
                ini.WriteIni("参数设置", "良率", "0");
                Main.frmMain.lblTotalCount.BeginInvoke(new Action(() => { Main.frmMain.lblTotalCount.Text = "产出：  " + " 0"; }));
                Main.frmMain.lblOKCount.BeginInvoke(new Action(() => { Main.frmMain.lblOKCount.Text = "良品：  " + " 0"; }));
                Main.frmMain.lblNGCount.BeginInvoke(new Action(() => { Main.frmMain.lblNGCount.Text = "不良品：  " + " 0"; }));
                Main.frmMain.lblYield.BeginInvoke(new Action(() => { Main.frmMain.lblYield.Text = "良率：  " + " 0" + " %"; }));
                sgbYield.BZ_NgRate = Convert.ToByte("0");
            }           
        }

        private void btnCPKGRRMode_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否切换至CPK&GRR模式？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                cobOperatorID.Enabled = true;
                CAMiClsBali.strMode = "3";
                CAMiClsBali.strOperator_ID = "1";
                CAMiClsBali.strTestSeriesID = DateTime.Now.ToString("yyyyMMddHHmmss");
                CAMiClsBali.strPriority = "-2";
                foreach (Control ctl in Main.frmMain.Controls)
                {
                    ctl.BackColor = Color.Green;
                }
            }
        }

        private void btnProductMode_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否切换至生产模式？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                cobOperatorID.Enabled = false;
                cobOperatorID.SelectedIndex = 0;
                CAMiClsBali.strMode = "0";
                CAMiClsBali.strOperator_ID = "1";
                CAMiClsBali.strTestSeriesID = "0";
                CAMiClsBali.strPriority = "0";
                foreach (Control ctl in Main.frmMain.Controls)
                {
                    ctl.BackColor = SystemColors.Window;
                }
            }
        }
        private void cobOperatorID_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cobOperatorID.SelectedIndex)
            {
                case 0:
                    CAMiClsBali.strOperator_ID = "1";
                    break;
                case 1:
                    CAMiClsBali.strOperator_ID = "2";
                    break;
                case 2:
                    CAMiClsBali.strOperator_ID = "3";
                    break;
            }
        }
        public static string configsn;
        private void txtSN_TextChanged(object sender, EventArgs e)
        {                    
            if (txtSN.Text.Length>1)
            {
            configsn = txtSN.Text;
            }            
        }       
    }
}
