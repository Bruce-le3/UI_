using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Drawing;
using ControlIni;
using System.IO;

namespace Justech
{
    public class CAMiClsMethod
    {
        #region  方法
        public void StreamLimeReset()//流线复位
        {
            Main.frmMain.ListBoxDisplay("复位--流线破真空");
            CAMiClsCylinder.StreamLine1VacuumOff();//破真空
            CAMiClsCylinder.StreamLine2VacuumOff();//破真空
            Main.frmMain.PauseCheck();
            Main.frmMain.ListBoxDisplay("复位--流线夹紧气缸松开");
            CAMiClsCylinder.StreamLine1Clamp2Retract();//夹紧松开                
            CAMiClsCylinder.StreamLine1Clamp1Retract();
            CAMiClsCylinder.StreamLine2Clamp2Retract();//夹紧松开                
            CAMiClsCylinder.StreamLine2Clamp1Retract();
            Main.frmMain.PauseCheck();
            Main.frmMain.ListBoxDisplay("复位--顶升气缸下降");
            CAMiClsCylinder.StreamLine1LiftingDown();//顶升下降
            CAMiClsCylinder.StreamLine2LiftingDown();//顶升下降
            Main.frmMain.PauseCheck();
            Main.frmMain.ListBoxDisplay("复位--阻挡气缸缩回");
            CAMiClsCylinder.StreamLine1StopRetract();//阻挡气缸缩回
            CAMiClsCylinder.StreamLine2StopRetract();//阻挡气缸缩回
            Main.frmMain.PauseCheck();
            Main.frmMain.ListBoxDisplay("复位--启动流线");
            GClsMontion.WriteCardExtendOutputBit(0, 8, 0);//流线1高速
            GClsMontion.WriteCardExtendOutputBit(0, 7, 0);//流线1正转
            GClsMontion.WriteCardExtendOutputBit(0, 6, 1);//流线1启动
            GClsMontion.WriteCardExtendOutputBit(1, 8, 0);//流线2高速
            GClsMontion.WriteCardExtendOutputBit(1, 7, 0);//流线2正转
            GClsMontion.WriteCardExtendOutputBit(1, 6, 1);//流线2启动
            Main.frmMain.PauseCheck();
        }
        public static int VaccumInitialJudge()//判断12个真空吸状态
        {
            int returnResult = 0;
            for (int i = 0; i < 12; i++)
            {
                if (GClsMontion.ReadIOCard7432InputBit(0, i) == 1)
                {
                    returnResult++;
                }
            }
            return returnResult;
        }
        public static int WaitStremLineSensorOff()//判断流线上感应器是否全部OFF
        {
            int returnResult = 0;
            for (int i = 0; i < 3; i++)
            {
                if (GClsMontion.ReadCardExtendInputBit(0, (10 + i)) == 1)
                {
                    returnResult++;
                }
                if (GClsMontion.ReadCardExtendInputBit(1, (10 + i)) == 1)
                {
                    returnResult++;
                }
            }
            return returnResult;
        }
        public static void StreamTrayLocate(int whichTray)//流线产品定位
        {
            switch (whichTray)
            {
                case 1: //流线1产品定位
                    Main.frmMain.ListBoxDisplay("流线1--流线1开真空");
                    CAMiClsCylinder.StreamLine1VacuumOn();
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--流线1顶升气缸顶升");
                    CAMiClsCylinder.StreamLine1LiftingUp();
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--流线1破真空");
                    CAMiClsCylinder.StreamLine1VacuumOff();
                    Main.frmMain.PauseCheck();//暂停
                    Thread.Sleep(10);//气缸连续动作增加间隔延时
                    Main.frmMain.ListBoxDisplay("流线1--流线1夹紧气缸1夹紧");
                    CAMiClsCylinder.StreamLine1Clamp1Extend();
                    Main.frmMain.PauseCheck();//暂停
                    Thread.Sleep(10);//气缸连续动作增加间隔延时
                    Main.frmMain.ListBoxDisplay("流线1--流线1夹紧气缸2夹紧");
                    CAMiClsCylinder.StreamLine1Clamp2Extend();
                    Main.frmMain.PauseCheck();//暂停
                    Thread.Sleep(10);//气缸连续动作增加间隔延时                    
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--流线1开真空");
                    CAMiClsCylinder.StreamLine1VacuumOn();
                    break;
                case 2://流线2产品定位
                    Main.frmMain.ListBoxDisplay("流线2--真空开启");
                    CAMiClsCylinder.StreamLine2VacuumOn();
                    Main.frmMain.PauseCheck();//暂停
                    Thread.Sleep(10);//气缸连续动作增加间隔延时
                    Main.frmMain.ListBoxDisplay("流线2--顶升气缸顶升");
                    CAMiClsCylinder.StreamLine2LiftingUp();
                    Main.frmMain.PauseCheck();//暂停
                    Thread.Sleep(10);//气缸连续动作增加间隔延时
                    Main.frmMain.ListBoxDisplay("流线2--破真空");
                    CAMiClsCylinder.StreamLine2VacuumOff();
                    Main.frmMain.PauseCheck();//暂停
                    Thread.Sleep(10);//气缸连续动作增加间隔延时
                    Main.frmMain.ListBoxDisplay("流线2--夹紧气缸1夹紧");
                    CAMiClsCylinder.StreamLine2Clamp1Extend();
                    Main.frmMain.PauseCheck();//暂停
                    Thread.Sleep(10);//气缸连续动作增加间隔延时
                    Main.frmMain.ListBoxDisplay("流线2--夹紧气缸2夹紧");
                    CAMiClsCylinder.StreamLine2Clamp2Extend();
                    Main.frmMain.PauseCheck();//暂停
                    Thread.Sleep(10);//气缸连续动作增加间隔延时                  
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--真空开启");
                    CAMiClsCylinder.StreamLine2VacuumOn();
                    break;
            }
        }
        public static void StreamTrayRelease(int whichTray)//流线产品放行
        {
            switch (whichTray)
            {
                case 1:
                    Main.frmMain.ListBoxDisplay("流线1--流线1夹紧气缸松开");
                    CAMiClsCylinder.StreamLine1Clamp2Retract();//夹紧松开
                    CAMiClsCylinder.StreamLine1Clamp1Retract();
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--流线1破真空");
                    CAMiClsCylinder.StreamLine1VacuumOff();//破真空
                    Main.frmMain.PauseCheck();//暂停
                    Thread.Sleep(10);//气缸连续动作增加间隔延时
                    Main.frmMain.ListBoxDisplay("流线1--流线1顶升气缸下降");
                    CAMiClsCylinder.StreamLine1LiftingDown();//顶升下降
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--流线1阻挡气缸缩回");
                    CAMiClsCylinder.StreamLine1StopRetract();//阻挡气缸缩回
                    Main.frmMain.PauseCheck();//暂停
                    break;
                case 2:
                    Main.frmMain.ListBoxDisplay("流线2--流线2夹紧气缸松开");
                    CAMiClsCylinder.StreamLine2Clamp2Retract();//夹紧松开
                    Thread.Sleep(10);//气缸连续动作增加间隔延时                    
                    CAMiClsCylinder.StreamLine2Clamp1Retract();
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--流线2破真空");
                    CAMiClsCylinder.StreamLine2VacuumOff();//破真空
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--流线2顶升气缸下降");
                    CAMiClsCylinder.StreamLine2LiftingDown();//顶升下降
                    Main.frmMain.PauseCheck();//暂停
                    Thread.Sleep(10);//气缸连续动作增加间隔延时
                    Main.frmMain.ListBoxDisplay("流线2--流线2阻挡气缸缩回");
                    CAMiClsCylinder.StreamLine2StopRetract();//阻挡气缸缩回
                    Main.frmMain.PauseCheck();//暂停
                    break;
            }

        }
        public static void StreamCCDDataClear(int whichLine)//清空数据存放区
        {
            switch (whichLine)
            {
                case 1:
                    Main.frmMain.ListBoxDisplay("流线1--清空数据存放区和结果存放区");
                    for (int i = 1; i < 12; i++)//清空流线1数据区
                    {
                        for (int j = 1; j < 25; j++)
                        {
                            Main.frmMain.dgvData.Rows[i + 1].Cells[j].Value = "";
                        }
                    }
                    for (int i = 0; i < 25; i++)//清空流线1结果
                    {
                        CAMiClsVariable.strRecvCCD1Data1Array[i] = "0";
                        CAMiClsVariable.strRecvCCD1Data2Array[i] = "0";
                    }
                    CAMiClsVariable.strRecvCCD1Data = "";
                    CAMiClsVariable.recvCCD1DataNO = 0;
                    break;
                case 2:
                    Main.frmMain.ListBoxDisplay("流线2--清空CCD2、3数据区");
                    for (int i = 29; i < 65; i++)
                    {
                        for (int j = 1; j < 25; j++)
                        {
                            Main.frmMain.dgvData.Rows[i].Cells[j].Value = "";
                        }
                    }
                    Main.frmMain.ListBoxDisplay("流线2--清空CCD2、3结果区");
                    for (int i = 0; i < 13; i++)
                    {
                        CAMiClsVariable.strRecvCCD2Data1Array[i] = "0";
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        CAMiClsVariable.strRecvCCD2Data2Array[i] = "0";
                    }
                    CAMiClsVariable.strRecvCCD2Data = "";
                    CAMiClsVariable.recvCCD2DataNO = 0;
                    break;
            }
        }
        public void StreamLine2VaccumOpenOrClose(int openOrClose)//真空吸打开或关闭
        {
            switch (openOrClose)
            {
                case 0:
                    for (int i = 0; i < 12; i++)//关闭所有真空吸
                    {
                        CAMiClsCylinder.GeneralClynderMotion(2, 0, i, 0);
                        CAMiClsCylinder.GeneralClynderMotion(2, 0, i + 12, 1);
                        CAMiClsCylinder.GeneralClynderMotionSensor(2, 0, i, 0, 3000, 44 + (i + 1) * 2);
                        CAMiClsCylinder.GeneralClynderMotion(2, 0, i + 12, 0);
                        if (CAMiClsVariable.strProduct != "P104")//当项目为D4X时
                        {
                            if (i < 8)
                            {
                                CAMiClsCylinder.GeneralClynderMotion(2, 0, i + 24, 0);//小气缸
                                CAMiClsCylinder.GeneralClynderMotionSensor(2, 0, i + 12, 0, 3000, 44 + (i + 12 + 1) * 2);
                            }
                            if (i == 8) CAMiClsCylinder.CldSmallControl9Retract();
                            if (i == 9) CAMiClsCylinder.CldSmallControl10Retract();
                            if (i == 10) CAMiClsCylinder.CldSmallControl11Retract();
                            if (i == 11) CAMiClsCylinder.CldSmallControl12Retract();
                        }
                        Main.frmMain.PauseCheck();//暂停
                    }
                    break;
                case 1:
                    for (int i = 0; i < 12; i++)//打开所有真空吸以及小气缸伸出
                    {
                        if (CAMiClsVariable.isProductSelect[i] == 1)//产品选中
                        {
                            CAMiClsCylinder.GeneralClynderMotion(2, 0, i, CAMiClsVariable.isProductSelect[i]);
                            CAMiClsCylinder.GeneralClynderMotionSensor(2, 0, i, CAMiClsVariable.isProductSelect[i], 3000, 44 + (i + 1) * 2 - CAMiClsVariable.isProductSelect[i]);
                            if (CAMiClsVariable.strProduct != "P104")//当项目为D4X时
                            {
                                if (i < 8)
                                {
                                    CAMiClsCylinder.GeneralClynderMotion(2, 0, i + 24, 1);//小气缸
                                    CAMiClsCylinder.GeneralClynderMotionSensor(2, 0, i + 12, CAMiClsVariable.isProductSelect[i], 3000, 44 + (i + 12 + 1) * 2 - CAMiClsVariable.isProductSelect[i]);
                                }
                                if (i == 8) CAMiClsCylinder.CldSmallControl9Extend();
                                if (i == 9) CAMiClsCylinder.CldSmallControl10Extend();
                                if (i == 10) CAMiClsCylinder.CldSmallControl11Extend();
                                if (i == 11) CAMiClsCylinder.CldSmallControl12Extend();
                            }
                        }
                        Main.frmMain.PauseCheck();//暂停
                    }
                    break;
            }
        }
        public void MoveToTossingPos(int num)//马达移动到抛料位置
        {
            Main.frmMain.ListBoxDisplay("流线2--CCD2 Y轴运行到抛料位置");
            GClsMontion.AbsoluteMove(3, CAMiClsVariable.CCD2TossingPosYValue[0], CAMiClsVariable.speedCCD2Y, CAMiClsVariable.TaccCCD2Y);
            GClsMontion.WaitMotorStop(3);
            Main.frmMain.ListBoxDisplay("流线2--Z轴运行到抛料位置");
            GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2flingPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
            GClsMontion.WaitMotorStop(4);
            Main.frmMain.PauseCheck();//暂停
            Main.frmMain.ListBoxDisplay("流线2--关闭抛料穴位真空吸");//一个一个抛，关闭NG产品对应真空吸
            CAMiClsCylinder.GeneralClynderMotion(2, 0, num, 0);//真空吸Off
            CAMiClsCylinder.GeneralClynderMotion(2, 0, num + 12, 1);//去真空On
            CAMiClsCylinder.GeneralClynderMotionSensor(2, 0, num, 0, 3000, 44 + (num + 1) * 2);
            CAMiClsCylinder.GeneralClynderMotion(2, 0, num + 12, 0);//去真空Off
            if (CAMiClsVariable.strProduct != "P104")//当项目为D4X时
            {
                if (num < 8)//--------------带小气缸机台部分
                {
                    CAMiClsCylinder.GeneralClynderMotion(2, 0, num + 24, 0);//小气缸
                    CAMiClsCylinder.GeneralClynderMotionSensor(2, 0, num + 12, 0, 3000, 44 + (num + 12 + 1) * 2);//小气缸感应off
                }
                if (num == 8) CAMiClsCylinder.CldSmallControl9Retract();
                if (num == 9) CAMiClsCylinder.CldSmallControl10Retract();
                if (num == 10) CAMiClsCylinder.CldSmallControl11Retract();
                if (num == 11) CAMiClsCylinder.CldSmallControl12Retract();
            }
        }

        public static string ReceiveData(int index, int client)//接收数据
        {
            try
            {
                if (client == 1)
                {
                    ++CAMiClsVariable.recvCCD1DataNO;
                    do
                    {
                        CAMiClsVariable.strRecvCCD1Data += CAMiClsVariable.cClient.ClientReceive(client,1);
                        if (CAMiClsVariable.strVision == "COGNEX")
                        {
                            CAMiClsVariable.strRecvCCD1DataArray = CAMiClsVariable.strRecvCCD1Data.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        }
                        else//Keyence
                        {
                            CAMiClsVariable.strRecvCCD1DataArray = CAMiClsVariable.strRecvCCD1Data.Split(new char[1] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        }
                        int j = 0, k = 0;
                        for (int m = 0; m < CAMiClsVariable.strRecvCCD1DataArray.Length; m++)
                        {
                            if (CAMiClsVariable.strRecvCCD1DataArray[m].Length < 20)
                            {
                                CAMiClsVariable.strRecvCCD1Data1Array[j++] = CAMiClsVariable.strRecvCCD1DataArray[m];
                            }
                            if (CAMiClsVariable.strRecvCCD1DataArray[m].Length > 20)
                            {
                                CAMiClsVariable.strRecvCCD1Data2Array[k++] = CAMiClsVariable.strRecvCCD1DataArray[m];
                            }
                        }
                        Thread.Sleep(5);
                        Application.DoEvents();
                    } while (CAMiClsVariable.strRecvCCD1Data1Array[CAMiClsVariable.recvCCD1DataNO - 1] == "0");
                    return CAMiClsVariable.strRecvCCD1Data1Array[CAMiClsVariable.recvCCD1DataNO - 1];
                }
                else
                {
                    ++CAMiClsVariable.recvCCD2DataNO;
                    do
                    {
                        CAMiClsVariable.strRecvCCD2Data += CAMiClsVariable.cClient.ClientReceive(client,1);
                        if (CAMiClsVariable.strVision == "COGNEX")
                        {
                            CAMiClsVariable.strRecvCCD2DataArray = CAMiClsVariable.strRecvCCD2Data.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        }
                        else//Keyence
                        {
                            CAMiClsVariable.strRecvCCD2DataArray = CAMiClsVariable.strRecvCCD2Data.Split(new char[1] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        }
                        int j = 0, k = 0;
                        for (int m = 0; m < CAMiClsVariable.strRecvCCD2DataArray.Length; m++)
                        {
                            if (CAMiClsVariable.strRecvCCD2DataArray[m].Length < 20)
                            {
                                if (CAMiClsVariable.strVision == "KEYENCE")
                                {
                                    if (CAMiClsVariable.strRecvCCD2DataArray[m] != "DSW")
                                    {
                                        CAMiClsVariable.strRecvCCD2Data1Array[j++] = CAMiClsVariable.strRecvCCD2DataArray[m];
                                    }     
                                }
                                else
                                {
                                    CAMiClsVariable.strRecvCCD2Data1Array[j++] = CAMiClsVariable.strRecvCCD2DataArray[m];
                                }                               
                            }
                            if (CAMiClsVariable.strRecvCCD2DataArray[m].Length > 20)
                            {
                                CAMiClsVariable.strRecvCCD2Data2Array[k++] = CAMiClsVariable.strRecvCCD2DataArray[m];
                            }
                        }
                        Thread.Sleep(5);
                        Application.DoEvents();
                    } while (CAMiClsVariable.strRecvCCD2Data1Array[CAMiClsVariable.recvCCD2DataNO - 1] == "0");
                    return CAMiClsVariable.strRecvCCD2Data1Array[CAMiClsVariable.recvCCD2DataNO - 1];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());                
                return "error";
            }
        }
        public static string GetCavityNum(int num)//获取穴位号，用于下视觉拍照
        {
            string strReturn = "";
            if (CAMiClsVariable.strVision == "COGNEX")//Cognex
            {
                switch (num)
                {
                    case 0:
                    case 1:
                        strReturn = "4_2";
                        break;
                    case 2:
                    case 3:
                        strReturn = "3_1";
                        break;
                    case 4:
                    case 5:
                        strReturn = "5_7";
                        break;
                    case 6:
                    case 7:
                        strReturn = "6_8";
                        break;
                    case 8:
                    case 9:
                        strReturn = "12_10";
                        break;
                    case 10:
                    case 11:
                        strReturn = "11_9";
                        break;
                    default:
                        strReturn = "4_2";
                        break;
                }
            }
            else if (CAMiClsVariable.strVision == "KEYENCE")//Keyence 
            {
                switch (num)
                {
                    case 0:
                    case 1:
                        strReturn = "4,2";
                        break;
                    case 2:
                    case 3:
                        strReturn = "3,1";
                        break;
                    case 4:
                    case 5:
                        strReturn = "5,7";
                        break;
                    case 6:
                    case 7:
                        strReturn = "6,8";
                        break;
                    case 8:
                    case 9:
                        strReturn = "12,10";
                        break;
                    case 10:
                    case 11:
                        strReturn = "11,9";
                        break;
                    default:
                        strReturn = "4,2";
                        break;
                }
            }          
            return strReturn;
        }
        public static void ChangeResultDisplay()//初始化结果显示
        {
            foreach (var label in Main.frmMain.groupPictDisplay.Controls.OfType<Label>())//颜色
            {
                if (CAMiClsVariable.isProductSelect[label.TabIndex] == 1)
                {
                    label.BeginInvoke(new Action(() => { label.BackColor = Color.Yellow; }));
                }
                else
                {
                    label.BeginInvoke(new Action(() => { label.BackColor = Color.GhostWhite; }));
                }
                Application.DoEvents();
            }
        }
        public static void NGTrayUnload()//NG Tray下料
        {
            Main.frmMain.ListBoxDisplay("NG夹紧气缸松开");
            CAMiClsCylinder.NGlocationClampRetract();
            MessageBox.Show("请抽出NG料盘，并取走NG产品。然后按Tray按钮。");
            while (GClsMontion.ReadCardInputBit(1, 3) == 1)
            {
                Main.frmMain.ListBoxDisplay("NG夹紧气缸夹紧");
                CAMiClsCylinder.NGLocationClampExtend();//NG夹紧气缸夹紧                    
            }
        }
        public static void NGTrayLoad()//NG Tray上料
        {          
            while (GClsMontion.ReadIOCard7432InputBit(0, 28) == 0)
            {
                MessageBox.Show("请将料盘推到位！");                
                Application.DoEvents();
                Thread.Sleep(1000);
            }
            while (GClsMontion.ReadIOCard7432InputBit(0, 29) == 0)
            {
                MessageBox.Show("请放置Tray盘");                
                Application.DoEvents();
                Thread.Sleep(1000);
            }
            for (int i = 0; i < 12; i++)
            {
                CAMiClsVariable.isTrayCaveEmpty[i] = "1";//Tray穴位是否为空,清空数据，1为空
            }
            Main.frmMain.ListBoxDisplay("NG夹紧气缸夹紧");
            CAMiClsCylinder.NGLocationClampExtend();//NG夹紧气缸夹紧
        }
        public  static int CCD2TossingPosYValueNO = 0;
        public static int TrayCheck(int TossingProductNo)//Tray抛料位置判断
        {
            int trayCheckResult = 0;//如果能抛则返回1，不能抛则返回0
            //int CCD2TossingPosYValueNO = 0;
            switch (TossingProductNo)
            {
                case 0:
                case 7:
                case 8:
                    #region
                    if (CAMiClsVariable.isTrayCaveEmpty[0] == "1")//判断穴位是否为空，1为空
                    {
                        switch (TossingProductNo)
                        {
                            case 0:
                                CCD2TossingPosYValueNO = 3;
                                break;
                            case 7:
                                CCD2TossingPosYValueNO = 2;
                                break;
                            case 8:
                                CCD2TossingPosYValueNO = 1;
                                break;
                        }
                        CAMiClsVariable.isTrayCaveEmpty[0] = "0";
                        trayCheckResult = 1;
                    }
                    else if (CAMiClsVariable.isTrayCaveEmpty[7] == "1")
                    {
                        switch (TossingProductNo)
                        {
                            case 0:
                                CCD2TossingPosYValueNO = 4;
                                break;
                            case 7:
                                CCD2TossingPosYValueNO = 3;
                                break;
                            case 8:
                                CCD2TossingPosYValueNO = 2;
                                break;
                        }
                        CAMiClsVariable.isTrayCaveEmpty[7] = "0";
                        trayCheckResult = 1;
                    }
                    else if (CAMiClsVariable.isTrayCaveEmpty[8] == "1")
                    {
                        switch (TossingProductNo)
                        {
                            case 0:
                                CCD2TossingPosYValueNO = 5;
                                break;
                            case 7:
                                CCD2TossingPosYValueNO = 4;
                                break;
                            case 8:
                                CCD2TossingPosYValueNO = 3;
                                break;
                        }
                        CAMiClsVariable.isTrayCaveEmpty[8] = "0";
                        trayCheckResult = 1;
                    }
                    else
                    {
                        trayCheckResult = 0;
                    }
                    break;
                    #endregion
                case 1:
                case 6:
                case 9:
                    #region
                    if (CAMiClsVariable.isTrayCaveEmpty[1] == "1")//判断穴位是否为空
                    {
                        switch (TossingProductNo)
                        {
                            case 1:
                                CCD2TossingPosYValueNO = 3;
                                break;
                            case 6:
                                CCD2TossingPosYValueNO = 2;
                                break;
                            case 9:
                                CCD2TossingPosYValueNO = 1;
                                break;
                        }
                        CAMiClsVariable.isTrayCaveEmpty[1] = "0";
                        trayCheckResult = 1;
                    }
                    else if (CAMiClsVariable.isTrayCaveEmpty[6] == "1")
                    {
                        switch (TossingProductNo)
                        {
                            case 1:
                                CCD2TossingPosYValueNO = 4;
                                break;
                            case 6:
                                CCD2TossingPosYValueNO = 3;
                                break;
                            case 9:
                                CCD2TossingPosYValueNO = 2;
                                break;
                        }
                        CAMiClsVariable.isTrayCaveEmpty[6] = "0";
                        trayCheckResult = 1;
                    }
                    else if (CAMiClsVariable.isTrayCaveEmpty[9] == "1")
                    {
                        switch (TossingProductNo)
                        {
                            case 1:
                                CCD2TossingPosYValueNO = 5;
                                break;
                            case 6:
                                CCD2TossingPosYValueNO = 4;
                                break;
                            case 9:
                                CCD2TossingPosYValueNO = 3;
                                break;
                        }
                        CAMiClsVariable.isTrayCaveEmpty[9] = "0";
                        trayCheckResult = 1;
                    }
                    else
                    {
                        trayCheckResult = 0;
                    }
                    break;
                    #endregion
                case 2:
                case 5:
                case 10:
                    #region
                    if (CAMiClsVariable.isTrayCaveEmpty[2] == "1")//判断穴位是否为空
                    {
                        switch (TossingProductNo)
                        {
                            case 2:
                                CCD2TossingPosYValueNO = 3;
                                break;
                            case 5:
                                CCD2TossingPosYValueNO = 2;
                                break;
                            case 10:
                                CCD2TossingPosYValueNO = 1;
                                break;
                        }
                        CAMiClsVariable.isTrayCaveEmpty[2] = "0";
                        trayCheckResult = 1;
                    }
                    else if (CAMiClsVariable.isTrayCaveEmpty[5] == "1")
                    {
                        switch (TossingProductNo)
                        {
                            case 2:
                                CCD2TossingPosYValueNO = 4;
                                break;
                            case 5:
                                CCD2TossingPosYValueNO = 3;
                                break;
                            case 10:
                                CCD2TossingPosYValueNO = 2;
                                break;
                        }
                        CAMiClsVariable.isTrayCaveEmpty[5] = "0";
                        trayCheckResult = 1;
                    }
                    else if (CAMiClsVariable.isTrayCaveEmpty[10] == "1")
                    {
                        switch (TossingProductNo)
                        {
                            case 2:
                                CCD2TossingPosYValueNO = 5;
                                break;
                            case 5:
                                CCD2TossingPosYValueNO = 4;
                                break;
                            case 10:
                                CCD2TossingPosYValueNO = 3;
                                break;
                        }
                        CAMiClsVariable.isTrayCaveEmpty[10] = "0";
                        trayCheckResult = 1;
                    }
                    else
                    {
                        trayCheckResult = 0;
                    }
                    break;
                    #endregion
                case 3:
                case 4:
                case 11:
                    #region
                    if (CAMiClsVariable.isTrayCaveEmpty[3] == "1")//判断穴位是否为空
                    {
                        switch (TossingProductNo)
                        {
                            case 3:
                                CCD2TossingPosYValueNO = 3;
                                break;
                            case 4:
                                CCD2TossingPosYValueNO = 2;
                                break;
                            case 11:
                                CCD2TossingPosYValueNO = 1;
                                break;
                        }
                        CAMiClsVariable.isTrayCaveEmpty[3] = "0";
                        trayCheckResult = 1;
                    }
                    else if (CAMiClsVariable.isTrayCaveEmpty[4] == "1")
                    {
                        switch (TossingProductNo)
                        {
                            case 3:
                                CCD2TossingPosYValueNO = 4;
                                break;
                            case 4:
                                CCD2TossingPosYValueNO = 3;
                                break;
                            case 11:
                                CCD2TossingPosYValueNO = 2;
                                break;
                        }
                        CAMiClsVariable.isTrayCaveEmpty[4] = "0";
                        trayCheckResult = 1;
                    }
                    else if (CAMiClsVariable.isTrayCaveEmpty[11] == "1")
                    {
                        switch (TossingProductNo)
                        {
                            case 3:
                                CCD2TossingPosYValueNO = 5;
                                break;
                            case 4:
                                CCD2TossingPosYValueNO = 4;
                                break;
                            case 11:
                                CCD2TossingPosYValueNO = 3;
                                break;
                        }
                        CAMiClsVariable.isTrayCaveEmpty[11] = "0";
                        trayCheckResult = 1;
                    }
                    else
                    {
                        trayCheckResult = 0;
                    }
                    break;
                    #endregion
            }
            CAMiClsVariable.CCD2TossingPosYValue[0] = CAMiClsVariable.CCD2TossingPosYValue[CCD2TossingPosYValueNO];
            return trayCheckResult;
        }     
        Ini iniRecord = new Ini();
        public void YieldDisplay()
        {
            iniRecord.filePath = CAMiClsVariable.montionIniPath;
            string dataNow = DateTime.Now.ToString("yyyyMMdd");
            int sum = 0, sumOK = 0, sumNG = 0;
            double yield = 0;
            if (!dataNow.Equals(iniRecord.ReadIni("参数设置", "日期"))) //更细当前产量
            {
                iniRecord.WriteIni("参数设置", "日期", dataNow);
                iniRecord.WriteIni("参数设置", "当前产量", "0");
                iniRecord.WriteIni("参数设置", "良品", "0");
                iniRecord.WriteIni("参数设置", "不良品", "0");
                iniRecord.WriteIni("参数设置", "良率", "0");
            }
            for(int i = 0;i < 12;i++)
            {
                if(CAMiClsVariable.isProductSelect[i] == 1)
                {
                    sum = Convert.ToInt32(iniRecord.ReadIni("参数设置", "当前产量")) + 1;
                    iniRecord.WriteIni("参数设置", "当前产量", sum.ToString());
                    sumOK = Convert.ToInt32(iniRecord.ReadIni("参数设置", "良品"));
                    sumNG = Convert.ToInt32(iniRecord.ReadIni("参数设置", "不良品"));
                    if(CAMiClsVariable.productResult[i] == 1)
                    {
                        sumOK = Convert.ToInt32(iniRecord.ReadIni("参数设置", "良品")) + 1;
                        iniRecord.WriteIni("参数设置", "良品", sumOK.ToString());
                    }
                    else
                    {
                        sumNG = Convert.ToInt32(iniRecord.ReadIni("参数设置", "不良品")) + 1;
                        iniRecord.WriteIni("参数设置", "不良品", sumNG.ToString());
                    }                
                    yield = Convert.ToDouble(sumOK) / Convert.ToDouble(sum) * 100;
                    Main.frmMain.lblTotalCount.BeginInvoke(new Action(() => { Main.frmMain.lblTotalCount.Text = "产出：  " + sum.ToString(); }));
                    Main.frmMain.lblOKCount.BeginInvoke(new Action(() => { Main.frmMain.lblOKCount.Text = "良品：  " + sumOK.ToString(); }));
                    Main.frmMain.lblNGCount.BeginInvoke(new Action(() => { Main.frmMain.lblNGCount.Text = "不良品：  " + sumNG.ToString(); }));
                    Main.frmMain.lblYield.BeginInvoke(new Action(() => { Main.frmMain.lblYield.Text = "良率：  " + yield.ToString("f2") + " %"; }));
                    iniRecord.WriteIni("参数设置", "良率", yield.ToString("f2"));
                    Main.frmMain.sgbYield.BeginInvoke(new Action(() => { Main.frmMain.sgbYield.BZ_NgRate = Convert.ToByte(100 - Convert.ToInt32(yield)); }));
                }
            }
        }

        public static void Group(int whichCCD)//发送Group
        {
            string strGroup1 = "GROUP1,12";
            string strGroup2 = "GROUP2," + (CAMiClsVariable.CCD2productSelectNO * 2).ToString();
            switch (whichCCD)//判断发送Group1  or Group2
            {
                case 1:
                    Main.frmMain.ListBoxDisplay("流线1--合并Group1字符串");
                    for (int i = 0; i < 12; i++)
                    {
                        strGroup1 = strGroup1 + "/" + Main.frmMain.dgvData.Rows[15 + i].Cells[9].Value.ToString() + "," + Main.frmMain.dgvData.Rows[66].Cells[i + 1].Value;
                    }
                    Main.frmMain.ListBoxDisplay("流线1--发送Group1");
                    CAMiClsVariable.cClient.ClientSend(1, strGroup1);
                    Thread.Sleep(10);
                    Main.frmMain.ListBoxDisplay("流线1--接收Group1反馈结果");
                    CAMiClsVariable.cClient.ClientReceive(1, 1);//可以将结果放置到某个表格里
                    break;
                case 2:
                    for (int j = 0; j < 12; j++)
                    {
                        if (Main.frmMain.dgvData.Rows[28].Cells[j + 1].Value.ToString() == "1")//穴位有料
                        {
                            if (j % 2 == 0)
                            {
                                strGroup2 = strGroup2 + "/" + "T21,0," + CAMiClsMethod.GetCavityNum(j) + "," + Main.frmMain.dgvData.Rows[29].Cells[j + 1].Value.ToString().Trim() + "," + Main.frmMain.dgvData.Rows[29].Cells[j + 2].Value.ToString().Trim() + ","
                                                            + Main.frmMain.dgvData.Rows[67].Cells[j + 1].Value.ToString().Trim() + "," + Main.frmMain.dgvData.Rows[67].Cells[j + 2].Value.ToString().Trim() + "/"
                                                            + "T22,0," + CAMiClsMethod.GetCavityNum(j) + "," + Main.frmMain.dgvData.Rows[29].Cells[j + 1].Value.ToString().Trim() + "," + Main.frmMain.dgvData.Rows[29].Cells[j + 2].Value.ToString().Trim() + ","
                                                            + Main.frmMain.dgvData.Rows[68].Cells[j + 1].Value.ToString().Trim() + "," + Main.frmMain.dgvData.Rows[68].Cells[j + 2].Value.ToString().Trim();
                            }
                        }
                    }
                        Main.frmMain.ListBoxDisplay("流线2--发送Group2");
                        CAMiClsVariable.cClient.ClientSend(2, strGroup2);
                        Thread.Sleep(10);
                        Main.frmMain.ListBoxDisplay("流线2--接收Group2反馈结果");
                        CAMiClsVariable.cClient.ClientReceive(2, 1);//可以将结果放置到某个表格里
                   
                    break;
            }
        }
        #endregion   
     
        #region keyence picture
        public string [] arrayOutTime = new string[6];
        public string [] arrayOutCount = new string[6];
        public string [] arrayInTime = new string[6]; 
        public string [] arrayInCount = new string[6];
        public string [] arrOutTime = new string[12];
        public string [] arrOutCount = new string[12];
        public string [] arrInTime = new string[12];
        public string [] arrInCount = new string[12];  
        public void GetTimeAndNum()//得到时间和次数
        {
            try
            {
                for (int i = 0; i < 6; i++)
                {    //i=0: 4&2, i=1:3&1,i=2: 5&7,i=3:6&8,i=4: 12&10, i=5: 11&9
                    arrayOutTime[i] = Main.frmMain.dgvData.Rows[i * 2 + 31].Cells[22].Value.ToString();
                    arrayOutCount[i] = Main.frmMain.dgvData.Rows[i * 2 + 31].Cells[23].Value.ToString();
                    arrayInTime[i] = Main.frmMain.dgvData.Rows[i * 2 + 31].Cells[24].Value.ToString();
                    arrayInCount[i] = Main.frmMain.dgvData.Rows[i * 2 + 31].Cells[25].Value.ToString();
                }
                for(int j =0;j < 12;j++)
                {
                   switch(j)
                   {
                       case 0:
                       case 2:
                           arrOutTime[j] = arrayOutTime[1];
                           arrOutCount[j] = arrayOutCount[1];
                           arrInTime[j] = arrayInTime[1];
                           arrInCount[j] = arrayInCount[1];
                           break;
                       case 1:
                       case 3:
                           arrOutTime[j] = arrayOutTime[0];
                           arrOutCount[j] = arrayOutCount[0];
                           arrInTime[j] = arrayInTime[0];
                           arrInCount[j] = arrayInCount[0];
                           break;
                       case 4:
                       case 6:
                           arrOutTime[j] = arrayOutTime[2];
                           arrOutCount[j] = arrayOutCount[2];
                           arrInTime[j] = arrayInTime[2];
                           arrInCount[j] = arrayInCount[2];
                           break;
                       case 5:
                       case 7:
                           arrOutTime[j] = arrayOutTime[3];
                           arrOutCount[j] = arrayOutCount[3];
                           arrInTime[j] = arrayInTime[3];
                           arrInCount[j] = arrayInCount[3];
                           break;
                       case 8:
                       case 10:
                           arrOutTime[j] = arrayOutTime[5];
                           arrOutCount[j] = arrayOutCount[5];
                           arrInTime[j] = arrayInTime[5];
                           arrInCount[j] = arrayInCount[5];
                           break;
                       case 9:
                       case 11:
                           arrOutTime[j] = arrayOutTime[4];
                           arrOutCount[j] = arrayOutCount[4];
                           arrInTime[j] = arrayInTime[4];
                           arrInCount[j] = arrayInCount[4];
                           break;
                   }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //复制图片到以条码命名的文件夹里
        public void CopyPictToSNDirectory(string DirectoryName,string sourceFile,string sourcePict,string destPict )
        {
            try
            {
                if (!Directory.Exists(DirectoryName))
                {
                    Directory.CreateDirectory(DirectoryName);
                }
                GFileOperate.CopyFile(sourceFile + "\\" + sourcePict, DirectoryName + "\\" + destPict);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        ZipFloClass clsZip = new ZipFloClass();
        public void KeyencePict()
        {
            GetTimeAndNum();//得到时间和次数
            string dictName = "";
            string SN = ""; 
            string sourceOutNormal="";
            string sourceOutUV="";
            string sourceInNormal="";
            string sourceInUV="";
            if (!Directory.Exists(@"D:\ImageUpLoadPDCA"))
            {
                Directory.CreateDirectory(@"D:\ImageUpLoadPDCA");
            }
            if (!Directory.Exists(@"D:\ImageUpLoad"))
            {
                Directory.CreateDirectory(@"D:\ImageUpLoad");
            }
            try
            {
                if (CAMiClsVariable.isPDCAOpen)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        if (CAMiClsVariable.isProductSelect[i] == 1)
                        {
                            dictName = @"D:\VDB\D43 CAMI S2\VisionDB\XG-X2700_00_01_FC_29_03_93\SD1\0999\Image" + "\\" + arrOutTime[i].Substring(0, 8) + "_" + arrOutTime[i].Substring(9, 2) + arrOutTime[i].Substring(11, 1) + "000" + "_image";
                            if(Main.frmMain.dgvData.Rows[15 + i].Cells[5].Value.ToString() != "1")
                            {
                                SN = Main.frmMain.dgvData.Rows[15 + i].Cells[5].Value.ToString().Substring(0, 17);
                            }                           
                            if (i == 3 || i == 2 || i == 4 || i == 5 || i == 11 || i == 10)
                            {
                                sourceOutNormal = arrOutTime[i].Substring(0, 8) + "_" + arrOutTime[i].Substring(9, 6) + "_" + arrOutCount[i] + "_&Cam1Img.Normal_1_4.jpg";
                                sourceOutUV = arrOutTime[i].Substring(0, 8) + "_" + arrOutTime[i].Substring(9, 6) + "_" + arrOutCount[i] + "_&Cam1_UV_1_4.jpg";
                                sourceInNormal = arrInTime[i].Substring(0, 8) + "_" + arrInTime[i].Substring(9, 6) + "_" + arrInCount[i] + "_&Cam1Img.Normal_0_4.jpg";
                                sourceInUV = arrInTime[i].Substring(0, 8) + "_" + arrInTime[i].Substring(9, 6) + "_" + arrInCount[i] + "_&Cam1_UV_0_4.jpg";
                            }
                            else if (i == 0 || i == 1 || i == 6 || i == 7 || i == 8 || i == 9)
                            {
                                try
                                {
                                    sourceOutNormal = arrOutTime[i].Substring(0, 8) + "_" + arrOutTime[i].Substring(9, 6) + "_" + arrOutCount[i] + "_&Cam2Img.Normal_1_4.jpg";
                                    sourceOutUV = arrOutTime[i].Substring(0, 8) + "_" + arrOutTime[i].Substring(9, 6) + "_" + arrOutCount[i] + "_&Cam2_UV_1_4.jpg";
                                    sourceInNormal = arrInTime[i].Substring(0, 8) + "_" + arrInTime[i].Substring(9, 6) + "_" + arrInCount[i] + "_&Cam2Img.Normal_0_4.jpg";
                                    sourceInUV = arrInTime[i].Substring(0, 8) + "_" + arrInTime[i].Substring(9, 6) + "_" + arrInCount[i] + "_&Cam2_UV_0_4.jpg";
                                }
                                catch(Exception e)
                                {
                                    MessageBox.Show(e.ToString());
                                }
                            }
                            if (Main.frmMain.dgvData.Rows[15 + i].Cells[5].Value.ToString() != "1")
                            {
                                CopyPictToSNDirectory("D:\\ImageUpLoad\\" + SN, dictName, sourceOutNormal, sourceOutNormal);
                                CopyPictToSNDirectory("D:\\ImageUpLoad\\" + SN, dictName, sourceOutUV, sourceOutUV);
                                CopyPictToSNDirectory("D:\\ImageUpLoad\\" + SN, dictName, sourceInNormal, sourceInNormal);
                                CopyPictToSNDirectory("D:\\ImageUpLoad\\" + SN, dictName, sourceInUV, sourceInUV);
                                try
                                {
                                    clsZip.ZipFile("D:\\ImageUpLoad\\" + SN, "D:\\ImageUpLoadPDCA\\" + SN + ".zip");
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show(e.ToString());
                                }
                            }                         
                        }
                    }        
                }                           
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }      
        }
        #endregion
    }
}
