using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ControlIni;
using System.Drawing;

namespace Justech
{ 
    class CAMiClsVariable
    {
        #region 变量定义                                          \*
        #region 参数路径      \*
        Ini iniVariable = new Ini();
        public static string montionIniPath = Application.StartupPath + "\\Config" + "\\" + "montion.ini";
        public static string adapterIniPath = Application.StartupPath + "\\Config" + "\\" + "Adapter.ini";
        public static string resultIniPath = Application.StartupPath + "\\Config" + "\\" + "result.ini";
        public static string errorCodeIniPath = Application.StartupPath + "\\Config" + "\\" + "ErrorCode.ini";
        public static string configIniPath = Application.StartupPath + "\\Config" + "\\" + "config.ini";  
      
        #endregion    
        #region    errorCode & 产品结果变量
        //errorCode
        public static string errorData, errorTime; //定义errorCode的日期和时间
        public static int errorCodeNo = -1;//errorCode初始值
        //产品结果
        public static int[] isProductSelect = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//机台产品选择判断               
        public static int[] productResult = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//机台产品结果\
        public static int[] productResultPDCA = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//机台产品结果\
        public static int[] productResultT1 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//T1结果
        public static int[] productResultT21 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//T21结果
        public static int[] productResultT22 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//T22结果
        public static int resultCount;//结果设置个数
        public static int[] coil_lead_shift = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] solder_scatter = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] cold_solder = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] UV_glue_overflow = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] UV_glue_insufficient = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] UV_Glue_bubble = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] foreign_material = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] liner_scalding = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] cient = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };



        #endregion
        #region bool型变量
        public static bool isStreamLine2Ready = true;//流线2等待信号
        public static bool isStreamLine1Done = false;//流线1完成信号
        public static bool isProductLocateInStreamLine2 = false; //产品流线2到位信号
        public static bool isProductOkNG = true;//机台产品OKNG判断
        public static bool isNGTrayOK = false;//机台NG Tray可以放料判断
        //public static bool isReplaceTray = false;//是否更换tray
        public static bool isEmptyRun = false;//是否空跑
        public static bool isPDCAOpen = false;
        public static bool isHIVEOpen = false;
        public static bool isSmemaOpen = false;
        public static bool isEntranceOpen = false;
        public static bool isTrayChange = false;
        #endregion      
        #region 相机数据变量
        //指令触发
        public static string strTrig1;// T1
        public static string strTrig2; //T22
        public static string strTrig3; //T23
        //CCD1
        public static string strCCD1Recv = "";//相机CCD1数据接收         
        public static string[] recvCCD1DataArray;//将收到的数据用数组保存起来          
        //CCD2   
        public static string strCCD2Recv1 = "";//相机CCD2数据接收1
        public static string strCCD2Recv2 = "";//相机CCD2数据接收2 
        public static string[] recvCCD2DataArray;//将收到的数据用数组保存起来  

        public static string[] strArrayTime;//创建time数据变量
        #endregion
        #region 脉冲当量  速度   加减速          \*
        public static double pulseAxis0, pulseAxis1, pulseAxis2, pulseAxis3, pulseAxis4;//读脉冲当量值  
        //public static double speedCCD1HomeX, speedCCD1HomeY, speedCCD2HomeX, speedCCD2HomeY, speedCCD2HomeZ;//回原点速度 
        public static double speedCCD1X, speedCCD1Y, speedCCD2X, speedCCD2Y, speedCCD2Z;//运行速度        
        public static double TaccCCD1X, TaccCCD1Y, TaccCCD2X, TaccCCD2Y, TaccCCD2Z; //加减速
        #endregion
        #region 轴运动位置                             \*
        public static double CCD2TakePhotoPosZ; //CCD2拍照位置
        public static double CCD2PickPosY, CCD2PickPosZ;//CCD2取料位置---CCD2PickPosX,
        public static double CCD2flingPosY, CCD2flingPosZ;//抛料位置
        public static double CCD2InitialPosX, CCD2InitialPosY, CCD2InitialPosZ;//CCD2初始位置
        public static double CCD1InitialPosX, CCD1InitialPosY;//CCD1初始位置
        //CCD1的12个拍照位置
        public static double[] CCD1PosXValue = new double[24];
        public static double[] CCD1PosYValue = new double[24];
        //CCD2的12个拍照位置
        public static double[] CCD2PosXValue = new double[12];
        public static double[] CCD2PosYValue = new double[12];
        //CCD1和CCD2的位置间距--X,Y
        public static double CCD1GapX, CCD1GapY, CCD1GapX2,CCD1GapY2;
        public static double CCD2GapY, CCD2GapX1, CCD2GapX2;
        //CCD2的5个抛料位置
        public static double[] CCD2TossingPosYValue = new double[6];
        #endregion
        #region  其它                 \*
        public static short cardRegId; //全局引用---7432
        public static string strIOCard;//IO 卡的名字
        public static string strVision;//vision选择
        public static string strProduct;//项目选择
        public static string webServiceEmgPath;//webService 地址
        public static string webServiceStatePath;//webService 地址 
        public static float xValues;//记录窗体初始大小
        public static float yValues;        
        public static GClsClient cClient = new GClsClient();//公共客户端实例
        public static double CT = 0;//记录CT 
        public static string strTossing;//
        public static int[] NG_ErrorCode = new int[12];//储存NG_ErrorCode
        public static string strProductD5X;//项目选择
        
        #endregion
        #region 变量
        public static string resultFinalData = "";
        public static string resultOKorNG = "";
        public static string strRecvCCD1Data = "";
        public static int recvCCD1DataNO = 0;//CCD1数据接收个数
        public static string[] strRecvCCD1DataArray;//将收到的数据用数组保存起来
        public static string[] strRecvCCD1Data1Array = { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };//将收到的数据用数组保存起来
        public static string[] strRecvCCD1Data2Array = { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };//将收到的数据用数组保存起来
        public static string strRecvCCD2Data = "";
        public static int recvCCD2DataNO = 0;
        public static string[] strRecvCCD2DataArray;//将收到的数据用数组保存起来
        public static string[] strRecvCCD2Data1Array = { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };//将收到的数据用数组保存起来
        public static string[] strRecvCCD2Data2Array = { "0", "0", "0", "0", "0", "0", "0" };//将收到的数据用数组保存起来

        //public static int CCD1productSelectNO = 0;//产品选择个数
        public static int CCD2productSelectNO = 0;//CCD2产品选择个数

        public static string[] isTrayCaveEmpty = { "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1" };//Tray穴位是否为空，1为空
        #endregion
        #endregion

        #region 变量名加载           \*
        public void ReadRegularVariable()//读取固定变量值
        {
            iniVariable.filePath = montionIniPath;           
            #region 脉冲当量    速度   加减速
            //读脉冲当量值 
            pulseAxis0 = Convert.ToDouble(iniVariable.ReadIni("脉冲当量", "Axis0"));
            pulseAxis1 = Convert.ToDouble(iniVariable.ReadIni("脉冲当量", "Axis1"));
            pulseAxis2 = Convert.ToDouble(iniVariable.ReadIni("脉冲当量", "Axis2"));
            pulseAxis3 = Convert.ToDouble(iniVariable.ReadIni("脉冲当量", "Axis3"));
            pulseAxis4 = Convert.ToDouble(iniVariable.ReadIni("脉冲当量", "Axis4"));
            //读速度
            speedCCD1X = Convert.ToDouble(iniVariable.ReadIni("速度", "CCD1-X速度")) / pulseAxis0;
            speedCCD1Y = Convert.ToDouble(iniVariable.ReadIni("速度", "CCD1-Y速度")) / pulseAxis1;
            speedCCD2X = Convert.ToDouble(iniVariable.ReadIni("速度", "CCD2-X速度")) / pulseAxis2;
            speedCCD2Y = Convert.ToDouble(iniVariable.ReadIni("速度", "CCD2-Y速度")) / pulseAxis3;
            speedCCD2Z = Convert.ToDouble(iniVariable.ReadIni("速度", "CCD2-Z速度")) / pulseAxis4;
            //读加减速
            TaccCCD1X = Convert.ToDouble(iniVariable.ReadIni("速度", "CCD1-X加减速"));
            TaccCCD1Y = Convert.ToDouble(iniVariable.ReadIni("速度", "CCD1-Y加减速"));
            TaccCCD2X = Convert.ToDouble(iniVariable.ReadIni("速度", "CCD2-X加减速"));
            TaccCCD2Y = Convert.ToDouble(iniVariable.ReadIni("速度", "CCD2-Y加减速"));
            TaccCCD2Z = Convert.ToDouble(iniVariable.ReadIni("速度", "CCD2-Z加减速"));
            #endregion
            #region  轴运动位置     位置间距                 \*
            CCD1InitialPosX = Convert.ToDouble(iniVariable.ReadIni("CCD1-X轴", "初始位置")) / pulseAxis0;
            CCD1InitialPosY = Convert.ToDouble(iniVariable.ReadIni("CCD1-Y轴", "初始位置")) / pulseAxis1;
            CCD2InitialPosX = Convert.ToDouble(iniVariable.ReadIni("CCD2-X轴", "初始位置")) / pulseAxis2;
            CCD2InitialPosY = Convert.ToDouble(iniVariable.ReadIni("CCD2-Y轴", "初始位置")) / pulseAxis3;
            CCD2InitialPosZ = Convert.ToDouble(iniVariable.ReadIni("CCD2-Z轴", "初始位置")) / pulseAxis4;
            CCD2flingPosY = Convert.ToDouble(iniVariable.ReadIni("CCD2-Y轴", "抛料位置")) / pulseAxis3;
            for (int i = 0; i < 6; i++)
            {
                if (CAMiClsVariable.strTossing == "1")//抛料位置数
                {
                    CCD2TossingPosYValue[i] = Convert.ToDouble(iniVariable.ReadIni("CCD2-Y轴", "抛料位置0")) / pulseAxis3;
                }
                else if (CAMiClsVariable.strTossing == "5")//抛料位置数
                {
                    CCD2TossingPosYValue[i] = Convert.ToDouble(iniVariable.ReadIni("CCD2-Y轴", "抛料位置" + i.ToString())) / pulseAxis3;
                }
            }
            CCD2flingPosZ = Convert.ToDouble(iniVariable.ReadIni("CCD2-Z轴", "抛料位置")) / pulseAxis4;
            CCD2PickPosY = Convert.ToDouble(iniVariable.ReadIni("CCD2-Y轴", "抓料位置")) / pulseAxis3;
            CCD2PickPosZ = Convert.ToDouble(iniVariable.ReadIni("CCD2-Z轴", "抓料位置")) / pulseAxis4;
            CCD2TakePhotoPosZ = Convert.ToDouble(iniVariable.ReadIni("CCD2-Z轴", "拍照位置")) / pulseAxis4;
            //获取间距
            CCD1GapX = Convert.ToDouble(iniVariable.ReadIni("间距", "CCD1间距X")) / pulseAxis0;
            CCD1GapX2 = Convert.ToDouble(iniVariable.ReadIni("间距", "CCD1间距X2")) / pulseAxis0;
            CCD1GapY = Convert.ToDouble(iniVariable.ReadIni("间距", "CCD1间距Y")) / pulseAxis1;
            CCD1GapY2 = Convert.ToDouble(iniVariable.ReadIni("间距", "CCD1间距Y2")) / pulseAxis1;
            CCD2GapY = Convert.ToDouble(iniVariable.ReadIni("间距", "CCD2间距Y")) / pulseAxis3;
            CCD2GapX1 = Convert.ToDouble(iniVariable.ReadIni("间距", "CCD2间距X1")) / pulseAxis2;
            CCD2GapX2 = Convert.ToDouble(iniVariable.ReadIni("间距", "CCD2间距X2")) / pulseAxis2;
            //获取拍照位置
            for (int i = 0; i < 24; i++)
            {
                CCD1PosXValue[i] = Convert.ToDouble(iniVariable.ReadIni("CCD1-X轴", "拍照位置" + (i + 1).ToString())) / pulseAxis0;
                CCD1PosYValue[i] = Convert.ToDouble(iniVariable.ReadIni("CCD1-Y轴", "拍照位置" + (i + 1).ToString())) / pulseAxis1;
            }
                for (int i = 0; i < 12; i++)
                {
                CCD2PosXValue[i] = Convert.ToDouble(iniVariable.ReadIni("CCD2-X轴", "拍照位置" + (i + 1).ToString())) / pulseAxis2;
                CCD2PosYValue[i] = Convert.ToDouble(iniVariable.ReadIni("CCD2-Y轴", "拍照位置" + (i + 1).ToString())) / pulseAxis3;
            }
            #endregion
            #region 客户端指令加载
            if(strVision == "COGNEX")// Cognex
            {
                strTrig1 = iniVariable.ReadIni("客户端", "TRIG1");//T1
                strTrig2 = iniVariable.ReadIni("客户端", "TRIG2");//T21
                strTrig3 = iniVariable.ReadIni("客户端", "TRIG3");//T22
            }
            else if (strVision == "KEYENCE")// Keyence
            {
                strTrig1 = iniVariable.ReadIni("客户端", "TRIG11");
                strTrig2 = iniVariable.ReadIni("客户端", "TRIG22");
                strTrig3 = iniVariable.ReadIni("客户端", "TRIG33");
            }
            #endregion
            //读脉产品选择使能  
            for (int i = 0; i < 12; i++)
            {
                isProductSelect[i] = Convert.ToInt32(iniVariable.ReadIni("穴位选择", "穴位" + (i+1).ToString()));
            }           
        }   
        public void LoadErrorCode(DataGridView dgv)//加载errorCode
        {
            iniVariable.filePath = errorCodeIniPath;
            int count = Convert.ToInt32(iniVariable.ReadIni("errorCodeCount", "Count"));//
            for(int i = 0;i < count;i++)
            { 
                dgv.Rows.Add();
                dgv.Rows[i].Cells[0].Value = (i + 1).ToString();
                dgv.Rows[i].Cells[1].Value = iniVariable.ReadIni((i + 1).ToString(), "Code");
                dgv.Rows[i].Cells[2].Value = iniVariable.ReadIni((i + 1).ToString(), "Discription");
                dgv.Rows[i].Cells[3].Value = iniVariable.ReadIni((i + 1).ToString(), "Reslove");
            }
        }

        public void ErrorCodeDateTime()//获取errorCode的时间
        {
            DateTime errDateTime = DateTime.Now;
            CAMiClsVariable.errorData = string.Format("{0}/{1}/{2}", errDateTime.Year, errDateTime.Month, errDateTime.Day);
            CAMiClsVariable.errorTime = string.Format("{0}:{1}:{2}", errDateTime.Hour, errDateTime.Minute, errDateTime.Second);
        }

        public void LoadResultSetting()//加载结果设置个数
        {
            iniVariable.filePath = resultIniPath;
            if (CAMiClsVariable.strProduct == "P104")
            {
                resultCount = Convert.ToInt32(iniVariable.ReadIni("Count", "sum_P104"));
            }
            else
            {
                resultCount = Convert.ToInt32(iniVariable.ReadIni("Count", "sum_D4X"));
            }           
            //加载IO卡和视觉选项
            iniVariable.filePath = configIniPath;
            strIOCard = iniVariable.ReadIni("IOCard", "Card");
            strVision = iniVariable.ReadIni("Vision", "TYPE");//Cognex/Keyence
            strProduct = iniVariable.ReadIni("Product", "Project");//项目选择 P104或D4X
            webServiceEmgPath = iniVariable.ReadIni("WebserviceEmg", "Url");//Url=http://10.0.0.2:5008/v5/capture/errordata
            webServiceStatePath = iniVariable.ReadIni("WebserviceState", "Url");//Url=http://10.0.0.2:5008/v5/capture/machinedata
            isPDCAOpen = (iniVariable.ReadIni("PDCA", "Enable") == "FALSE") ? false : true;
            isHIVEOpen = (iniVariable.ReadIni("HIVE", "Enable") == "FALSE") ? false : true;
            isSmemaOpen = (iniVariable.ReadIni("SMEMA", "Enable") == "FALSE") ? false : true;
            isEntranceOpen = (iniVariable.ReadIni("ENTRANCE", "Enable") == "FALSE") ? false : true;
            strTossing = iniVariable.ReadIni("Tossing", "PositionNumber");//抛料位置1或5
            strProductD5X = iniVariable.ReadIni("ProductD5X", "ProjectD5X");//项目选择 D42或D43
        }
        public void LoadProductCountAndDgvSetting()//加载产量设置和datagridview
        {
            try
            {
                if (!Directory.Exists("D:\\DATA"))
                {
                    Directory.CreateDirectory("D:\\DATA");
                }
                iniVariable.filePath = CAMiClsVariable.montionIniPath;//获取路径
                string data = DateTime.Now.ToString("yyyyMMdd");
                if (!data.Equals(iniVariable.ReadIni("参数设置", "日期"))) //更细当前产量
                {
                    iniVariable.WriteIni("参数设置", "日期", data);
                    iniVariable.WriteIni("参数设置", "当前产量", "0");
                    iniVariable.WriteIni("参数设置", "良品", "0");
                    iniVariable.WriteIni("参数设置", "不良品", "0");
                    iniVariable.WriteIni("参数设置", "良率", "0");
                }
                Main.frmMain.lblTotalCount.Text = "产出：  " + iniVariable.ReadIni("参数设置", "当前产量");
                Main.frmMain.lblOKCount.Text = "良品：  " + iniVariable.ReadIni("参数设置", "良品");
                Main.frmMain.lblNGCount.Text = "不良品：" + iniVariable.ReadIni("参数设置", "不良品");
                Main.frmMain.lblYield.Text = "良率：  " + iniVariable.ReadIni("参数设置", "良率") + " %";
                Main.frmMain.sgbYield.BZ_NgRate = Convert.ToByte(100 - Convert.ToInt32(Convert.ToDouble(iniVariable.ReadIni("参数设置", "良率"))));
                //加载datagridview
                Main.frmMain.dgvData.Rows.Clear();
                Main.frmMain.dgvData.ColumnCount = 50;
                Main.frmMain.dgvData.Rows.Add(100);
                for (int i = 0; i < 100; i++)//给行列增加序列号
                {
                    if (i < 50)
                    {
                        Main.frmMain.dgvData.Rows[0].Cells[i].Value = i.ToString();
                    }
                    Main.frmMain.dgvData.Rows[i + 1].Cells[0].Value = (i + 1).ToString();
                }
                for (int j = 1; j < 36; j++)//在第一行显示结果
                {
                    iniVariable.filePath = CAMiClsVariable.resultIniPath;
                    if (CAMiClsVariable.strVision == "COGNEX"&&strProduct=="P104")//Cognex
                    {
                        //Main.frmMain.dgvData.Rows[1].Cells[j].Value = iniVariable.ReadIni("结果设置Cognex_P104", (j + 2).ToString());
                    }
                    else if (CAMiClsVariable.strVision == "COGNEX"&&strProduct=="D4X")
                    {
                        Main.frmMain.dgvData.Rows[1].Cells[j].Value = iniVariable.ReadIni("结果设置Cognex_D4X", (j + 2).ToString());
                    }

                    else if (CAMiClsVariable.strVision == "KEYENCE")//Keyence
                    {
                        Main.frmMain.dgvData.Rows[1].Cells[j].Value = iniVariable.ReadIni("结果设置Keyence", (j + 2).ToString());
                    }
                }
                foreach (var label in Main.frmMain.groupPictDisplay.Controls.OfType<Label>())//颜色
                {
                    label.BeginInvoke(new Action(() => { label.BackColor = Color.GhostWhite; }));
                    Application.DoEvents();
                }      
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
       
        #endregion
        
    }
} 
