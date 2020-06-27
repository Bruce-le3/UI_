using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Drawing;

namespace Justech
{
    public class ClsRun
    {
        public GClsMethod myMethod = new GClsMethod();
        public CAMiClsMethod camiClsMethod = new CAMiClsMethod();
        public bool NGTrayForceOpen = false;
        public int ProductNumForT2=0;
        private int NGCount = 0, OneHourNGcount = 0;
        private int Hour = 0, minute = 0;

        public void ThMainReset()//自动---机台复位部分             
        {
            try
            {
                Main.frmMain.ListBoxDisplay("复位--机台复位开始-----------------------");
                Main.frmMain.isStopPushJudge = true;
                CAMiClsMethod.ChangeResultDisplay();//初始化结果显示颜色              
                //----------------------------------------------------------------------写复位过程
                Main.frmMain.ListBoxDisplay("复位--Z轴回原点");
                GClsMontion.GoHome(4, CAMiClsVariable.speedCCD2Z / 5, CAMiClsVariable.TaccCCD2Z);//Z轴回原点--3轴
                GClsMontion.GoHomeJudge(4);
                Main.frmMain.PauseCheck();
                Main.frmMain.ListBoxDisplay("复位--其它轴回原点");
                GClsMontion.GoHome(0, CAMiClsVariable.speedCCD1X / 5, CAMiClsVariable.TaccCCD1X);//其它轴回原点
                GClsMontion.GoHome(1, CAMiClsVariable.speedCCD1Y / 5, CAMiClsVariable.TaccCCD1Y);
                GClsMontion.GoHome(2, CAMiClsVariable.speedCCD2X / 5, CAMiClsVariable.TaccCCD2X);
                GClsMontion.GoHome(3, CAMiClsVariable.speedCCD2Y / 5, CAMiClsVariable.TaccCCD2Y);
                GClsMontion.GoHomeJudge(0);
                GClsMontion.GoHomeJudge(1);
                GClsMontion.GoHomeJudge(2);
                GClsMontion.GoHomeJudge(3);
                Main.frmMain.PauseCheck();
                GClsMontion.WriteCardExtendOutputBit(0, 12, 0);//关闭背光
                Main.frmMain.ListBoxDisplay("复位--判断真空吸是否开启");
                if (CAMiClsMethod.VaccumInitialJudge() > 0)//大于0说明有真空吸
                {
                    Main.frmMain.ListBoxDisplay("复位--真空吸开启，运动到抓料位置");
                    GClsMontion.AbsoluteMove(3, CAMiClsVariable.CCD2PickPosY, CAMiClsVariable.speedCCD2Y, CAMiClsVariable.TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                    Main.frmMain.PauseCheck();
                    Main.frmMain.ListBoxDisplay("复位--Z轴下降到抓料高度");
                    GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2PickPosZ - 10000, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                    Main.frmMain.PauseCheck();
                }
                Main.frmMain.ListBoxDisplay("复位--关闭所有真空吸");
                camiClsMethod.StreamLine2VaccumOpenOrClose(0);
                Thread.Sleep(100);
                Main.frmMain.ListBoxDisplay("复位--Z轴回零位置");
                GClsMontion.AbsoluteMove(4, 0, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                GClsMontion.WaitMotorStop(4);
                Main.frmMain.PauseCheck();
                Main.frmMain.ListBoxDisplay("复位--其它轴运动到初始位置");
                GClsMontion.AbsoluteMove(2, CAMiClsVariable.CCD2InitialPosX, CAMiClsVariable.speedCCD2X, CAMiClsVariable.TaccCCD2X);//运行到初始位置
                GClsMontion.AbsoluteMove(3, CAMiClsVariable.CCD2InitialPosY, CAMiClsVariable.speedCCD2Y, CAMiClsVariable.TaccCCD2Y);
                GClsMontion.AbsoluteMove(0, CAMiClsVariable.CCD1InitialPosX, CAMiClsVariable.speedCCD1X, CAMiClsVariable.TaccCCD1X);//运行到初始位置
                GClsMontion.AbsoluteMove(1, CAMiClsVariable.CCD1InitialPosY, CAMiClsVariable.speedCCD1Y, CAMiClsVariable.TaccCCD1Y);
                Main.frmMain.PauseCheck();
                camiClsMethod.StreamLimeReset();//流线复位
                Main.frmMain.ListBoxDisplay("复位--等待流线上无产品");
                do
                {
                    Thread.Sleep(300);
                    Application.DoEvents();
                } while (CAMiClsMethod.WaitStremLineSensorOff() != 0);
                Thread.Sleep(2000);
                Main.frmMain.PauseCheck();
                Main.frmMain.ListBoxDisplay("复位--流线停止");
                GClsMontion.WriteCardExtendOutputBit(0, 6, 0);//流线1停止
                GClsMontion.WriteCardExtendOutputBit(1, 6, 0);//流线2停止
                Main.frmMain.PauseCheck();
                Main.frmMain.ListBoxDisplay("复位--阻挡气缸伸出");
                CAMiClsCylinder.StreamLine1StopExtend();//阻挡气缸伸出
                CAMiClsCylinder.StreamLine2StopExtend();//阻挡气缸伸出
                Main.frmMain.ListBoxDisplay("复位--等待各轴运动到初始位置");
                GClsMontion.WaitMotorStop(2);
                GClsMontion.WaitMotorStop(3);
                GClsMontion.WaitMotorStop(0);
                GClsMontion.WaitMotorStop(1);
                Main.frmMain.PauseCheck();//暂停
                Main.frmMain.isPreparing = false;
                Main.frmMain.isReady = true;//复位完成标志
                Main.frmMain.isResetPushJudge = false;
                Main.frmMain.PauseCheck();
                Main.frmMain.MachineResultDisplay("机台复位完成,请启动！");
                Main.frmMain.ListBoxDisplay("复位--机台复位完成，请启动！");
                Main.frmMain.isMachineStop = true;
                for (int i = 1; i < 100; i++)//清空数据区
                {
                    for (int j = 1; j < 49; j++)
                    {
                        Main.frmMain.dgvData.Rows[i + 1].Cells[j].Value = "";//第一行为标头，不清空
                    }
                }
                Main.frmMain.ListBoxDisplay("清空结果区");
                Main.frmMain.PauseCheck();//暂停  
                for (int i = 0; i < 13; i++)
                {
                    CAMiClsVariable.strRecvCCD1Data1Array[i] = "0";
                    CAMiClsVariable.strRecvCCD1Data2Array[i] = "0";
                    CAMiClsVariable.strRecvCCD2Data1Array[i] = "0";
                }
                for (int i = 0; i < 7; i++)
                {
                    CAMiClsVariable.strRecvCCD2Data2Array[i] = "0";
                }
                CAMiClsVariable.strRecvCCD1Data = "";
                CAMiClsVariable.strRecvCCD2Data = "";
                CAMiClsVariable.recvCCD1DataNO = 0;
                CAMiClsVariable.recvCCD2DataNO = 0;
                CAMiClsVariable.isTrayChange = false;//是否更换Tray
                CAMiClsVariable.isNGTrayOK = false;

            }
            catch (Exception ex)
            {
                myMethod.LogRecord(ex.ToString());
            }
        }
        public void ThStreamFirstLineRunning()//第一条流线-自动
        {
            while (true)
            {
                try
                {
                    Main.frmMain.ListBoxDisplay("流线1--运行开始-----------------------");
                    #region 流线1进料部分
                    if (CAMiClsVariable.isSmemaOpen)//SMEMA打开时
                    {
                        Main.frmMain.ListBoxDisplay("流线1--SMEMA要料");
                        GClsMontion.WriteCardOutputBit(0, 0, 1);//SMEMA要料信号
                        Main.frmMain.ListBoxDisplay("流线1--SMEMA等待上位机送料");
                        do
                        {
                            Thread.Sleep(10);
                            Application.DoEvents();
                        } while (GClsMontion.ReadIOCard7432InputBit(0, 30) != 1);//上位机送料信号
                        Main.frmMain.ListBoxDisplay("流线1--流线1启动");
                        GClsMontion.WriteCardExtendOutputBit(0, 6, 1);//流线1启动
                    }
                    Main.frmMain.ListBoxDisplay("流线1--等待流线1入料感应感应有产品");
                    Main.frmMain.PauseCheck();//暂停
                    do//等待流线1入料感应感应到
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        Main.frmMain.PauseCheck();//暂停
                    } while (GClsMontion.ReadCardExtendInputBit(0, 10) != 1);
                    Main.frmMain.ListBoxDisplay("流线1--SMEMA要料Off");
                    GClsMontion.WriteCardOutputBit(0, 0, 0);//SMEMA要料信号
                    Main.frmMain.ListBoxDisplay("流线1--流线1启动");
                    GClsMontion.WriteCardExtendOutputBit(0, 6, 1);//流线1启动
                    for (int i = 0; i < 12; i++)//加载流线1产品选择使能
                    {
                        Main.frmMain.dgvData.Rows[27].Cells[i + 1].Value = 1;//流线1产品使能放置在27行
                    }
                    //CAMiClsVariable.CCD1productSelectNO = 12;
                    //for (int i = 0; i < 12; i++)
                    //{
                    //    CAMiClsVariable.CCD1productSelectNO += Convert.ToInt32(Main.frmMain.dgvData.Rows[27].Cells[i + 1].Value);                        
                    //}
                    //Main.frmMain.ListBoxDisplay(string.Format("流线1--流线1产品需检测的个数为{0}", CAMiClsVariable.CCD1productSelectNO));
                    CAMiClsMethod.StreamCCDDataClear(1);//清空数据存放区
                    Main.frmMain.PauseCheck();//暂停                  
                    Main.frmMain.ListBoxDisplay("流线1--等待流线1到位感应1感应");
                    do//等待流线到位感应1
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        Main.frmMain.PauseCheck();//暂停
                    } while (GClsMontion.ReadCardExtendInputBit(0, 11) != 1);
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--等待流线1到位感应2感应");
                    do//等待到位2感应
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        Main.frmMain.PauseCheck();//暂停
                    } while (GClsMontion.ReadCardExtendInputBit(0, 12) != 1);
                    Thread.Sleep(200);//先延时再停止，根据需求更改
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--停止流线1");
                    GClsMontion.WriteCardExtendOutputBit(0, 6, 0);//流线1停止
                    Main.frmMain.PauseCheck();//暂停                    
                    Main.frmMain.ListBoxDisplay("流线1--流线1阻挡气缸缩回");
                    CAMiClsCylinder.StreamLine1StopRetract();//阻挡气缸缩回
                    CAMiClsMethod.StreamTrayLocate(1);//流线1产品定位
                    Main.frmMain.PauseCheck();//暂停
                    #endregion
                    Main.frmMain.ListBoxDisplay("流线1--清空CCD1缓存数据");
                    CAMiClsVariable.cClient.ClientReceive(1, 0);
                    Main.frmMain.ListBoxDisplay("流线1--定位结束，开始拍照");
                    if (CAMiClsVariable.strVision == "KEYENCE")
                    {
                        Main.frmMain.ListBoxDisplay("流线1--打开流线1背光源");
                        GClsMontion.WriteCardExtendOutputBit(0, 12, 1);//打开背光
                    }
                    #region CCD1拍照
                    for (int i = 0; i < 24; i++)
                    {
                        //if (Main.frmMain.dgvData.Rows[27].Cells[i + 1].Value.ToString() == "1")//穴位使能打开
                        //{
                        Main.frmMain.PauseCheck();//暂停
                        Main.frmMain.ListBoxDisplay("流线1--运行到CCD1-第" + (i + 1).ToString() + "个拍照位置");
                        GClsMontion.AbsoluteMove(0, CAMiClsVariable.CCD1PosXValue[i], CAMiClsVariable.speedCCD1X, CAMiClsVariable.TaccCCD1X);
                        GClsMontion.AbsoluteMove(1, CAMiClsVariable.CCD1PosYValue[i], CAMiClsVariable.speedCCD1Y, CAMiClsVariable.TaccCCD1Y);
                        GClsMontion.WaitMotorStop(0);
                        GClsMontion.WaitMotorStop(1);
                        Main.frmMain.PauseCheck();//暂停
                        Thread.Sleep(10);
                        Main.frmMain.ListBoxDisplay("流线1--CCD1拍照触发");//拍照触发
                        Main.frmMain.PauseCheck();//暂停
                        //CAMiClsVariable.strArrayTime[i] = DateTime.Now.ToString("hhmmss");//获得当前时间                      
                        if (i % 2 == 0)
                        {
                            CAMiClsVariable.cClient.ClientSend(1, (CAMiClsVariable.strTrig1 + ",0," + (i + 1).ToString()));
                        }
                        if (i % 2 == 1)
                        {
                            CAMiClsVariable.cClient.ClientSend(1, (CAMiClsVariable.strTrig1 + ",0," + (i + 1).ToString()));
                        }
                        Main.frmMain.PauseCheck();//暂停
                        Thread.Sleep(10);
                        CAMiClsVariable.strCCD1Recv = CAMiClsMethod.ReceiveData(i, 1);
                        if (CAMiClsVariable.strVision == "COGNEX")//Cognex
                        {
                            if (CAMiClsVariable.strCCD1Recv.Substring(0, 2) != "T1")
                            {
                                MessageBox.Show("CCD1 拍照T1反馈异常");
                            }
                        }
                        else if (CAMiClsVariable.strVision == "KEYENCE")//Keyence
                        {
                            if (CAMiClsVariable.strCCD1Recv.Substring(0, 4) != "CC10")
                            {
                                MessageBox.Show("CCD1 拍照T1反馈异常");
                            }
                        }                      
                        Main.frmMain.ListBoxDisplay("流线1--等待CCD1-第" + (i + 1).ToString() + "个反馈结果");
                        //}
                        Main.frmMain.PauseCheck();//暂停                       
                    }
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("CCD1X轴/CCD1Y轴运动到初始位置");
                    GClsMontion.AbsoluteMove(0, CAMiClsVariable.CCD1InitialPosX, CAMiClsVariable.speedCCD1X, CAMiClsVariable.TaccCCD1X);
                    GClsMontion.AbsoluteMove(1, CAMiClsVariable.CCD1InitialPosY, CAMiClsVariable.speedCCD1Y, CAMiClsVariable.TaccCCD1Y);
                    Main.frmMain.ListBoxDisplay("流线1--获取CCD1数据");
                    while (CAMiClsVariable.strRecvCCD1Data2Array[11] == "0")//等待12个结果全部接收完
                    {
                        CAMiClsVariable.recvCCD1DataNO = 0;
                        CAMiClsVariable.strCCD1Recv = CAMiClsMethod.ReceiveData(0, 1);
                        Thread.Sleep(10);
                        Application.DoEvents();
                    }
                    Main.frmMain.ListBoxDisplay("流线1--提取CCD1数据到表格指定位置");
                    for (int i = 0; i < 12; i++)
                    {
                        Main.frmMain.ListBoxDisplay("流线1--等待CCD1-第" + (i + 1).ToString() + "个反馈结果");
                        //if (Convert.ToInt32(Main.frmMain.dgvData.Rows[27].Cells[i + 1].Value) == 1)//穴位使能打开
                        //{
                        CAMiClsVariable.recvCCD1DataNO = 0;
                        //for (int m = 0; m < i + 1; m++)
                        //{
                        //    CAMiClsVariable.recvCCD1DataNO += Convert.ToInt32(Main.frmMain.dgvData.Rows[27].Cells[m + 1].Value);                               
                        //}
                        CAMiClsVariable.recvCCD1DataArray = CAMiClsVariable.strRecvCCD1Data2Array[i].Split(',');//CAMiClsVariable.strRecvCCD1Data2Array[CAMiClsVariable.recvCCD1DataNO - 1].Split(',')
                        //Main.frmMain.dictFirstLine.Clear();//清空数据
                        for (int j = 0; j < CAMiClsVariable.recvCCD1DataArray.Length; j++)//将接收到的数据放到数据区中
                        {
                            //Main.frmMain.dictFirstLine.Add("第" + i.ToString() + "次拍照的第" + j.ToString() + "次结果", CAMiClsVariable.recvCCD1DataArray[j]);
                            Main.frmMain.dgvData.Rows[i + 2].Cells[j + 1].Value = CAMiClsVariable.recvCCD1DataArray[j];
                            Main.frmMain.ListBoxDisplay(string.Format("流线1--相机接收到第" + (i + 1).ToString() + "组数据的第" + (j + 1).ToString() + "个数据  {0}", CAMiClsVariable.recvCCD1DataArray[j]));
                        }
                        //}
                    }
                    #endregion
                    GClsMontion.WriteCardExtendOutputBit(0, 12, 0);//关闭背光
                    Main.frmMain.PauseCheck();//暂停
                    CAMiClsMethod.StreamTrayRelease(1);//流线1产品放行
                    Main.frmMain.ListBoxDisplay("流线1--等待流线2允许进料");
                    do
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        Main.frmMain.PauseCheck();//暂停
                    } while (!CAMiClsVariable.isStreamLine2Ready);
                    Main.frmMain.PauseCheck();//暂停
                    #region 流线1出料部分
                    Main.frmMain.ListBoxDisplay("流线1--启动流线1");
                    GClsMontion.WriteCardExtendOutputBit(0, 8, 0);//流线1高速                
                    GClsMontion.WriteCardExtendOutputBit(0, 6, 1);//流线1启动
                    CAMiClsVariable.isStreamLine1Done = true;
                    CAMiClsVariable.isStreamLine2Ready = false;
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--清空表格第15-26行，1-25列，用来存放最新的流线1数据");
                    for (int i = 15; i < 27; i++)//清空数据用来放置流线1的数据
                    {
                        for (int j = 1; j < 25; j++)
                        {
                            Main.frmMain.dgvData.Rows[i].Cells[j].Value = "";
                        }
                    }
                    Main.frmMain.ListBoxDisplay("流线1--转移数据至表格第15-26行");
                    for (int k = 0; k < 12; k++)
                    {
                        for (int m = 0; m < CAMiClsVariable.recvCCD1DataArray.Length; m++)
                        {
                            Main.frmMain.dgvData.Rows[k + 15].Cells[m + 1].Value = Main.frmMain.dgvData.Rows[k + 2].Cells[m + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[k + 2].Cells[m + 1].Value : 1;
                        }
                        if (CAMiClsVariable.strVision == "COGNEX"&&CAMiClsVariable.strProduct=="P104")//Cognex
                        {
                            CAMiClsVariable.isProductSelect[k] = 1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[k + 15].Cells[6].Value);
                        }
                        else if (CAMiClsVariable.strVision == "COGNEX"&&CAMiClsVariable.strProduct=="D4X")
                        {
                            CAMiClsVariable.isProductSelect[k] = 1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[k + 15].Cells[2].Value);
                        }
                        else if (CAMiClsVariable.strVision == "KEYENCE")
                        {
                            CAMiClsVariable.isProductSelect[k] = 1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[k + 15].Cells[3].Value);
                        }
                    }
                    for (int i = 0; i < 12; i++)//T1各产品状态
                    {
                        if (CAMiClsVariable.strProduct == "P104")
                        {
                            CAMiClsVariable.productResultT1[i] = 1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 15].Cells[6].Value);
                        }
                        else
                        {
                            CAMiClsVariable.productResultT1[i] = 1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 15].Cells[3].Value);
                        }
                        Main.frmMain.dgvData.Rows[66].Cells[i + 1].Value = CAMiClsVariable.productResultT1[i] == 1 ? "OK" : "NG";
                    }
                    Main.frmMain.PauseCheck();//暂停                   
                    Main.frmMain.ListBoxDisplay("流线1--等待流线1产品流出");
                    do//等待流线1到位感应无
                    {
                        Thread.Sleep(100);
                        Application.DoEvents();
                        Main.frmMain.PauseCheck();//暂停
                    } while (GClsMontion.ReadCardExtendInputBit(0, 11) == 1 || GClsMontion.ReadCardExtendInputBit(0, 12) == 1 ||
                             !CAMiClsVariable.isProductLocateInStreamLine2);
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--停止流线1");
                    GClsMontion.WriteCardExtendOutputBit(0, 6, 0);//流线1停止
                    CAMiClsVariable.isProductLocateInStreamLine2 = false;//将流线2产品到位信号清掉
                    Main.frmMain.ListBoxDisplay("流线1--流线1阻挡气缸伸出");
                    CAMiClsCylinder.StreamLine1StopExtend();//阻挡气缸伸出
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("等待CCD1X轴/CCD1Y轴运动到初始位置");
                    Main.frmMain.ListBoxDisplay("发送Group1");
                    CAMiClsMethod.Group(1); //发送Group1
                    GClsMontion.WaitMotorStop(0);
                    GClsMontion.WaitMotorStop(1);
                    #endregion
                }
                catch (Exception ex)
                {
                    myMethod.LogRecord(ex.ToString());
                }
            }
        }
        public void ThStreamSecondLineRunning()//第二条流线自动
        {
            while (true)
            {
                try
                {
                    Main.frmMain.ListBoxDisplay("流线2--运行开始-----------------------");
                    CAMiClsVariable.CCD2productSelectNO = 0;
                    Main.frmMain.PauseCheck();//暂停            
                    #region  流线2入料
                    Main.frmMain.ListBoxDisplay("流线2--等待流线1出料");
                    do//等待流线1完成
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        Main.frmMain.PauseCheck();//暂停
                    } while (!CAMiClsVariable.isStreamLine1Done);
                    CAMiClsVariable.isStreamLine1Done = false;
                    Main.frmMain.ListBoxDisplay("流线2--开始计时");
                    Main.frmMain.sw.Reset();
                    Main.frmMain.sw.Start();//开始计时
                    Main.frmMain.ListBoxDisplay("流线2--启动流线2");
                    GClsMontion.WriteCardExtendOutputBit(1, 8, 0);//流线2高速                   
                    GClsMontion.WriteCardExtendOutputBit(1, 6, 1);//流线2启动
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--清空结果区");
                    CAMiClsMethod.StreamCCDDataClear(2);//清空数据存放区
                    #region 加载流线2穴位功能
                    Main.frmMain.ListBoxDisplay("流线2--加载流线2穴位功能");
                    #region  加载流线2产品使能 //4,2,3,1,5,7,6,8,12,10,11,9------用于位置运动   28行
                    Main.frmMain.dgvData.Rows[28].Cells[1].Value = (CAMiClsVariable.isProductSelect[3] + CAMiClsVariable.isProductSelect[1]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[28].Cells[2].Value = Main.frmMain.dgvData.Rows[28].Cells[1].Value;
                    Main.frmMain.dgvData.Rows[28].Cells[3].Value = (CAMiClsVariable.isProductSelect[2] + CAMiClsVariable.isProductSelect[0]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[28].Cells[4].Value = Main.frmMain.dgvData.Rows[28].Cells[3].Value;
                    Main.frmMain.dgvData.Rows[28].Cells[5].Value = (CAMiClsVariable.isProductSelect[4] + CAMiClsVariable.isProductSelect[6]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[28].Cells[6].Value = Main.frmMain.dgvData.Rows[28].Cells[5].Value;
                    Main.frmMain.dgvData.Rows[28].Cells[7].Value = (CAMiClsVariable.isProductSelect[5] + CAMiClsVariable.isProductSelect[7]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[28].Cells[8].Value = Main.frmMain.dgvData.Rows[28].Cells[7].Value;
                    Main.frmMain.dgvData.Rows[28].Cells[9].Value = (CAMiClsVariable.isProductSelect[11] + CAMiClsVariable.isProductSelect[9]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[28].Cells[10].Value = Main.frmMain.dgvData.Rows[28].Cells[9].Value;
                    Main.frmMain.dgvData.Rows[28].Cells[11].Value = (CAMiClsVariable.isProductSelect[10] + CAMiClsVariable.isProductSelect[8]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[28].Cells[12].Value = Main.frmMain.dgvData.Rows[28].Cells[11].Value;
                    #endregion
                    #region 加载流线2产品使能、、4,3,2,1,5,6,7,8,12,11,10,9---按照产品在tray盘里的实际位置来定 49行
                    Main.frmMain.dgvData.Rows[49].Cells[1].Value = (CAMiClsVariable.isProductSelect[3] + CAMiClsVariable.isProductSelect[1]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[49].Cells[3].Value = Main.frmMain.dgvData.Rows[49].Cells[1].Value;
                    Main.frmMain.dgvData.Rows[49].Cells[2].Value = (CAMiClsVariable.isProductSelect[2] + CAMiClsVariable.isProductSelect[0]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[49].Cells[4].Value = Main.frmMain.dgvData.Rows[49].Cells[2].Value;
                    Main.frmMain.dgvData.Rows[49].Cells[5].Value = (CAMiClsVariable.isProductSelect[4] + CAMiClsVariable.isProductSelect[6]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[49].Cells[7].Value = Main.frmMain.dgvData.Rows[49].Cells[5].Value;
                    Main.frmMain.dgvData.Rows[49].Cells[6].Value = (CAMiClsVariable.isProductSelect[5] + CAMiClsVariable.isProductSelect[7]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[49].Cells[8].Value = Main.frmMain.dgvData.Rows[49].Cells[6].Value;
                    Main.frmMain.dgvData.Rows[49].Cells[9].Value = (CAMiClsVariable.isProductSelect[11] + CAMiClsVariable.isProductSelect[9]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[49].Cells[11].Value = Main.frmMain.dgvData.Rows[49].Cells[9].Value;
                    Main.frmMain.dgvData.Rows[49].Cells[10].Value = (CAMiClsVariable.isProductSelect[10] + CAMiClsVariable.isProductSelect[8]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[49].Cells[12].Value = Main.frmMain.dgvData.Rows[49].Cells[10].Value;
                    #endregion
                    #region   加载流线2产品使能   结果输出、、 1,2,3,4,5,6,7,8,9,10,11,12----按照正常排序  80行
                    Main.frmMain.dgvData.Rows[80].Cells[1].Value = (CAMiClsVariable.isProductSelect[2] + CAMiClsVariable.isProductSelect[0]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[80].Cells[3].Value = Main.frmMain.dgvData.Rows[80].Cells[1].Value;
                    Main.frmMain.dgvData.Rows[80].Cells[2].Value = (CAMiClsVariable.isProductSelect[3] + CAMiClsVariable.isProductSelect[1]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[80].Cells[4].Value = Main.frmMain.dgvData.Rows[80].Cells[2].Value;
                    Main.frmMain.dgvData.Rows[80].Cells[5].Value = (CAMiClsVariable.isProductSelect[4] + CAMiClsVariable.isProductSelect[6]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[80].Cells[7].Value = Main.frmMain.dgvData.Rows[80].Cells[5].Value;
                    Main.frmMain.dgvData.Rows[80].Cells[6].Value = (CAMiClsVariable.isProductSelect[5] + CAMiClsVariable.isProductSelect[7]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[80].Cells[8].Value = Main.frmMain.dgvData.Rows[80].Cells[6].Value;
                    Main.frmMain.dgvData.Rows[80].Cells[9].Value = (CAMiClsVariable.isProductSelect[10] + CAMiClsVariable.isProductSelect[8]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[80].Cells[11].Value = Main.frmMain.dgvData.Rows[80].Cells[9].Value;
                    Main.frmMain.dgvData.Rows[80].Cells[10].Value = (CAMiClsVariable.isProductSelect[11] + CAMiClsVariable.isProductSelect[9]) != 0 ? 1 : 0;
                    Main.frmMain.dgvData.Rows[80].Cells[12].Value = Main.frmMain.dgvData.Rows[80].Cells[10].Value;
                    #endregion
                    #endregion
                    for (int i = 0; i < 12; i++)
                    {
                        CAMiClsVariable.CCD2productSelectNO += Convert.ToInt32(Main.frmMain.dgvData.Rows[28].Cells[i + 1].Value);
                    }
                    Main.frmMain.ListBoxDisplay(string.Format("流线2--流线2产品需检测的个数为{0}", CAMiClsVariable.CCD2productSelectNO));
                    CAMiClsVariable.CCD2productSelectNO = CAMiClsVariable.CCD2productSelectNO / 2;
                    Main.frmMain.ListBoxDisplay("流线2--初始化结果显示颜色");
                    CAMiClsMethod.ChangeResultDisplay();//初始化结果显示颜色              
                    Main.frmMain.PauseCheck();//暂停                    
                    Main.frmMain.ListBoxDisplay("流线2--等待流线2到位感应1感应");
                    do//等待流线2到位1感应
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                    } while (GClsMontion.ReadCardExtendInputBit(1, 10) != 1);
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--等待流线2到位感应2感应");
                    do//等待到位2感应
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        Main.frmMain.PauseCheck();//暂停
                    } while (GClsMontion.ReadCardExtendInputBit(1, 11) != 1);
                    Thread.Sleep(200);//先延时再停止，根据需求更改
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--停止流线2");
                    GClsMontion.WriteCardExtendOutputBit(1, 6, 0);//流线2停止
                    CAMiClsVariable.isProductLocateInStreamLine2 = true;//流线2产品到位   
                    Main.frmMain.ListBoxDisplay("流线2--马达模组运动到抓料位置");
                    GClsMontion.AbsoluteMove(3, CAMiClsVariable.CCD2PickPosY, CAMiClsVariable.speedCCD2Y, CAMiClsVariable.TaccCCD2Y);
                    CAMiClsMethod.StreamTrayLocate(2);//流线2产品定位
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--等待Y轴运动到抓料位置");
                    GClsMontion.WaitMotorStop(3);
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--Z轴下降到抓料位置");
                    GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2PickPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                    Main.frmMain.PauseCheck();//暂停             
                    Main.frmMain.ListBoxDisplay("流线2--打开所有真空吸");
                    camiClsMethod.StreamLine2VaccumOpenOrClose(1);
                    Thread.Sleep(10);
                    Main.frmMain.PauseCheck();//暂停PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--Z轴上升到初始高度");
                    GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2InitialPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    #region  flexSN临时转移至第29行--keyence机台转移gap值到29行15列-26列
                    if (CAMiClsVariable.strVision == "COGNEX"&&CAMiClsVariable.strProduct=="P104")//Cognex
                    {
                        Main.frmMain.dgvData.Rows[29].Cells[1].Value = Main.frmMain.dgvData.Rows[18].Cells[2].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[2].Value = Main.frmMain.dgvData.Rows[16].Cells[2].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[3].Value = Main.frmMain.dgvData.Rows[17].Cells[2].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[4].Value = Main.frmMain.dgvData.Rows[15].Cells[2].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[5].Value = Main.frmMain.dgvData.Rows[19].Cells[2].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[6].Value = Main.frmMain.dgvData.Rows[21].Cells[2].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[7].Value = Main.frmMain.dgvData.Rows[20].Cells[2].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[8].Value = Main.frmMain.dgvData.Rows[22].Cells[2].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[9].Value = Main.frmMain.dgvData.Rows[26].Cells[2].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[10].Value = Main.frmMain.dgvData.Rows[24].Cells[2].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[11].Value = Main.frmMain.dgvData.Rows[25].Cells[2].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[12].Value = Main.frmMain.dgvData.Rows[23].Cells[2].Value;
                    }
                    else if (CAMiClsVariable.strVision == "COGNEX"&&CAMiClsVariable.strProduct=="D4X")
                    {
                        Main.frmMain.dgvData.Rows[29].Cells[1].Value = Main.frmMain.dgvData.Rows[18].Cells[9].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[2].Value = Main.frmMain.dgvData.Rows[16].Cells[9].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[3].Value = Main.frmMain.dgvData.Rows[17].Cells[9].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[4].Value = Main.frmMain.dgvData.Rows[15].Cells[9].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[5].Value = Main.frmMain.dgvData.Rows[19].Cells[9].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[6].Value = Main.frmMain.dgvData.Rows[21].Cells[9].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[7].Value = Main.frmMain.dgvData.Rows[20].Cells[9].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[8].Value = Main.frmMain.dgvData.Rows[22].Cells[9].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[9].Value = Main.frmMain.dgvData.Rows[26].Cells[9].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[10].Value = Main.frmMain.dgvData.Rows[24].Cells[9].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[11].Value = Main.frmMain.dgvData.Rows[25].Cells[9].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[12].Value = Main.frmMain.dgvData.Rows[23].Cells[9].Value;
                    }   
                    else if (CAMiClsVariable.strVision == "KEYENCE")//Cognex
                    {
                        Main.frmMain.dgvData.Rows[29].Cells[1].Value = Main.frmMain.dgvData.Rows[18].Cells[5].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[2].Value = Main.frmMain.dgvData.Rows[16].Cells[5].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[3].Value = Main.frmMain.dgvData.Rows[17].Cells[5].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[4].Value = Main.frmMain.dgvData.Rows[15].Cells[5].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[5].Value = Main.frmMain.dgvData.Rows[19].Cells[5].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[6].Value = Main.frmMain.dgvData.Rows[21].Cells[5].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[7].Value = Main.frmMain.dgvData.Rows[20].Cells[5].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[8].Value = Main.frmMain.dgvData.Rows[22].Cells[5].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[9].Value = Main.frmMain.dgvData.Rows[26].Cells[5].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[10].Value = Main.frmMain.dgvData.Rows[24].Cells[5].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[11].Value = Main.frmMain.dgvData.Rows[25].Cells[5].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[12].Value = Main.frmMain.dgvData.Rows[23].Cells[5].Value;
                        //转移gap值
                        Main.frmMain.dgvData.Rows[29].Cells[15].Value = Main.frmMain.dgvData.Rows[18].Cells[8].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[16].Value = Main.frmMain.dgvData.Rows[16].Cells[8].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[17].Value = Main.frmMain.dgvData.Rows[17].Cells[8].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[18].Value = Main.frmMain.dgvData.Rows[15].Cells[8].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[19].Value = Main.frmMain.dgvData.Rows[19].Cells[8].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[20].Value = Main.frmMain.dgvData.Rows[21].Cells[8].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[21].Value = Main.frmMain.dgvData.Rows[20].Cells[8].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[22].Value = Main.frmMain.dgvData.Rows[22].Cells[8].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[23].Value = Main.frmMain.dgvData.Rows[26].Cells[8].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[24].Value = Main.frmMain.dgvData.Rows[24].Cells[8].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[25].Value = Main.frmMain.dgvData.Rows[25].Cells[8].Value;
                        Main.frmMain.dgvData.Rows[29].Cells[26].Value = Main.frmMain.dgvData.Rows[23].Cells[8].Value;
                    }
                    #endregion
                    GClsMontion.WaitMotorStop(4);
                    #endregion
                    //开始拍照
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--马达运动到第一个拍照位置");
                    GClsMontion.AbsoluteMove(2, CAMiClsVariable.CCD2PosXValue[0], CAMiClsVariable.speedCCD2X, CAMiClsVariable.TaccCCD2X);
                    GClsMontion.AbsoluteMove(3, CAMiClsVariable.CCD2PosYValue[0], CAMiClsVariable.speedCCD2Y, CAMiClsVariable.TaccCCD2Y);
                    Main.frmMain.ListBoxDisplay("流线2--清空CCD2缓存数据");
                    CAMiClsVariable.cClient.ClientReceive(2, 0);
                    Main.frmMain.ListBoxDisplay("流线2--等待马达运动到初始拍照位置");
                    GClsMontion.WaitMotorStop(2);
                    GClsMontion.WaitMotorStop(3);
                    GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2TakePhotoPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                    Thread.Sleep(10);
                    #region CCD2拍照
                    ProductNumForT2 = 0;
                    Main.frmMain.ListBoxDisplay("流线2--开始拍照");
                    for (int i = 0; i < 12; i++)
                    {
                        ProductNumForT2 += Convert.ToInt32(Main.frmMain.dgvData.Rows[28].Cells[i + 1].Value);//用于判断T2是否检测
                        if (Main.frmMain.dgvData.Rows[28].Cells[i + 1].Value.ToString() == "1")//穴位有料
                        {
                            Main.frmMain.ListBoxDisplay("流线2--运行到CCD2-第" + (i + 1).ToString() + "个拍照位置");
                            GClsMontion.AbsoluteMove(2, CAMiClsVariable.CCD2PosXValue[i], CAMiClsVariable.speedCCD2X, CAMiClsVariable.TaccCCD2X);
                            GClsMontion.AbsoluteMove(3, CAMiClsVariable.CCD2PosYValue[i], CAMiClsVariable.speedCCD2Y, CAMiClsVariable.TaccCCD2Y);
                            GClsMontion.WaitMotorStop(2);
                            GClsMontion.WaitMotorStop(3);
                            //GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2TakePhotoPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                            //GClsMontion.WaitMotorStop(4);
                            Thread.Sleep(80);
                            Main.frmMain.PauseCheck();//暂停
                            Main.frmMain.ListBoxDisplay("流线2--触发拍照");//触发拍照
                            if (i % 2 == 0)
                            {
                                if (CAMiClsVariable.strVision == "COGNEX")
                                {
                                    CAMiClsVariable.cClient.ClientSend(2, (CAMiClsVariable.strTrig2 + ",0," + CAMiClsMethod.GetCavityNum(i) + ","
                                   + Main.frmMain.dgvData.Rows[29].Cells[i + 1].Value.ToString().Trim() + "," + Main.frmMain.dgvData.Rows[29].Cells[i + 2].Value.ToString().Trim()));
                                }
                                else if (CAMiClsVariable.strVision == "KEYENCE")
                                {
                                    if (i == 0 || i == 4 || i == 8) Thread.Sleep(200);
                                    CAMiClsVariable.cClient.ClientSend(2, "DSW,0,\"" + Main.frmMain.dgvData.Rows[29].Cells[i + 1].Value.ToString().Trim() + "_" + Main.frmMain.dgvData.Rows[29].Cells[i + 2].Value.ToString().Trim());
                                    Thread.Sleep(10);
                                    //CAMiClsVariable.cClient.ClientSend(2, "DSW,1,\"" + Main.frmMain.dgvData.Rows[29].Cells[i + 2].Value.ToString().Trim());
                                    //Thread.Sleep(10);
                                    CAMiClsVariable.cClient.ClientSend(2, (CAMiClsVariable.strTrig2 + ",0," + CAMiClsMethod.GetCavityNum(i) + "," + Main.frmMain.dgvData.Rows[29].Cells[15 + i].Value.ToString()
                                        + "," + Main.frmMain.dgvData.Rows[29].Cells[16 + i].Value.ToString()));
                                }
                                Thread.Sleep(10);
                                Main.frmMain.ListBoxDisplay("流线2--接收数据");
                                Main.frmMain.PauseCheck();//暂停
                                CAMiClsVariable.strCCD2Recv1 = CAMiClsMethod.ReceiveData(i, 2);
                                try
                                {
                                    if (CAMiClsVariable.strVision == "COGNEX")
                                    {
                                        if (CAMiClsVariable.strCCD2Recv1.Substring(0, 3) != "T21")
                                        {
                                            MessageBox.Show("CCD2 拍照T21反馈异常");
                                        }
                                    }
                                    else if (CAMiClsVariable.strVision == "KEYENCE")
                                    {
                                        if (CAMiClsVariable.strCCD2Recv1.Substring(0, 4) != "CC20")
                                        {
                                            MessageBox.Show("CCD2 拍照T21反馈异常");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());
                                }
                            }
                            else if (i % 2 == 1)
                            {
                                if (CAMiClsVariable.strVision == "COGNEX")
                                {
                                    CAMiClsVariable.cClient.ClientSend(2, (CAMiClsVariable.strTrig3 + ",0," + CAMiClsMethod.GetCavityNum(i) + ","
                                   + Main.frmMain.dgvData.Rows[29].Cells[i].Value.ToString().Trim() + "," + Main.frmMain.dgvData.Rows[29].Cells[i + 1].Value.ToString().Trim()));
                                }
                                else if (CAMiClsVariable.strVision == "KEYENCE")
                                {
                                    CAMiClsVariable.cClient.ClientSend(2, "DSW,0,\"" + Main.frmMain.dgvData.Rows[29].Cells[i].Value.ToString().Trim() + "_" + Main.frmMain.dgvData.Rows[29].Cells[i + 1].Value.ToString().Trim());
                                    Thread.Sleep(10);
                                    //CAMiClsVariable.cClient.ClientSend(2, "DSW,1,\"" + Main.frmMain.dgvData.Rows[29].Cells[i + 1].Value.ToString().Trim());
                                    //Thread.Sleep(10);
                                    CAMiClsVariable.cClient.ClientSend(2, (CAMiClsVariable.strTrig3 + ",0," + CAMiClsMethod.GetCavityNum(i) + "," + Main.frmMain.dgvData.Rows[29].Cells[14 + i].Value.ToString()
                                        + "," + Main.frmMain.dgvData.Rows[29].Cells[15 + i].Value.ToString()));
                                }
                                Thread.Sleep(10);
                                Main.frmMain.PauseCheck();//暂停
                                Main.frmMain.ListBoxDisplay("流线2--接收数据");
                                CAMiClsVariable.strCCD2Recv2 = CAMiClsMethod.ReceiveData(i, 2);
                                try
                                {
                                    if (CAMiClsVariable.strVision == "COGNEX")//Cognex
                                    {
                                        if (CAMiClsVariable.strCCD2Recv2.Substring(0, 3) != "T22")
                                        {
                                            MessageBox.Show("CCD2 拍照T22反馈异常");
                                        }
                                    }
                                    else if (CAMiClsVariable.strVision == "KEYENCE")//Keyence
                                    {
                                        if (CAMiClsVariable.strCCD2Recv2.Substring(0, 4) != "CC21")
                                        {
                                            MessageBox.Show("CCD2 拍照T22反馈异常");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());
                                }
                                Main.frmMain.PauseCheck();//暂停                              
                            }
                        }
                    }
                    Main.frmMain.ListBoxDisplay("流线2--Z轴运行到初始位置");
                    GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2InitialPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    Main.frmMain.ListBoxDisplay("流线2--获取CCD2数据");
                    if (ProductNumForT2 != 0)
                    {


                        while (CAMiClsVariable.strRecvCCD2Data2Array[CAMiClsVariable.CCD2productSelectNO - 1] == "0")
                        {
                            CAMiClsVariable.recvCCD2DataNO = 0;
                            CAMiClsVariable.strCCD2Recv2 = CAMiClsMethod.ReceiveData(0, 2);
                            Thread.Sleep(10);
                            Application.DoEvents();
                        }
                        for (int i = 0; i < 6; i++)
                        {
                            if (i % 2 == 0)
                            {
                                try
                                {
                                    if (Convert.ToInt32(Main.frmMain.dgvData.Rows[49].Cells[i * 2 + 1].Value) == 1)//穴位按照产品在tray盘里的实际位置来定
                                    {
                                        CAMiClsVariable.recvCCD2DataNO = 0;
                                        for (int m = 0; m < i * 2 + 1; m++)
                                        {
                                            CAMiClsVariable.recvCCD2DataNO += Convert.ToInt32(Main.frmMain.dgvData.Rows[49].Cells[m + 1].Value);
                                        }
                                        CAMiClsVariable.recvCCD2DataNO += Convert.ToInt32(Main.frmMain.dgvData.Rows[49].Cells[i * 2 + 1 + 2].Value);
                                        CAMiClsVariable.recvCCD2DataNO = CAMiClsVariable.recvCCD2DataNO / 2;
                                        CAMiClsVariable.recvCCD2DataArray = CAMiClsVariable.strRecvCCD2Data2Array[CAMiClsVariable.recvCCD2DataNO - 1].Split(',');
                                        for (int j = 0; j < CAMiClsVariable.recvCCD2DataArray.Length; j++)
                                        {
                                            if (j < 21)
                                            {
                                                Main.frmMain.dgvData.Rows[i * 2 + 30].Cells[j + 1].Value = CAMiClsVariable.recvCCD2DataArray[j];//第一组数据
                                            }
                                            else
                                            {
                                                Main.frmMain.dgvData.Rows[i * 2 + 31].Cells[j - 21 + 2].Value = CAMiClsVariable.recvCCD2DataArray[j];//换行放第二组数据
                                            }
                                        }
                                    }
                                }
                                catch (Exception err)
                                {
                                    MessageBox.Show(err.ToString());
                                }
                            }
                            else if (i % 2 == 1)
                            {
                                try
                                {
                                    if (Convert.ToInt32(Main.frmMain.dgvData.Rows[49].Cells[i * 2].Value) == 1)
                                    {
                                        CAMiClsVariable.recvCCD2DataNO = 0;
                                        for (int m = 0; m < (i + 1) * 2; m++)
                                        {
                                            CAMiClsVariable.recvCCD2DataNO += Convert.ToInt32(Main.frmMain.dgvData.Rows[49].Cells[m + 1].Value);
                                        }
                                        CAMiClsVariable.recvCCD2DataNO = CAMiClsVariable.recvCCD2DataNO / 2;
                                        CAMiClsVariable.recvCCD2DataArray = CAMiClsVariable.strRecvCCD2Data2Array[CAMiClsVariable.recvCCD2DataNO - 1].Split(',');
                                        for (int j = 0; j < CAMiClsVariable.recvCCD2DataArray.Length; j++)
                                        {
                                            if (j < 21)
                                            {
                                                Main.frmMain.dgvData.Rows[i * 2 + 30].Cells[j + 1].Value = CAMiClsVariable.recvCCD2DataArray[j];//第一组数据
                                            }
                                            else
                                            {
                                                Main.frmMain.dgvData.Rows[i * 2 + 31].Cells[j - 21 + 2].Value = CAMiClsVariable.recvCCD2DataArray[j];//换行放第二组数据
                                            }
                                        }
                                    }
                                }
                                catch (Exception exx)
                                {
                                    MessageBox.Show(exx.ToString());
                                }
                            }
                        }
                        CAMiClsVariable.recvCCD2DataNO = 0;
                    }
                    #endregion
                    Main.frmMain.ListBoxDisplay("流线2--转移数据至第51行开始的行号");
                    #region 流线2--转移数据至第51行开始的行号
                    for (int i = 0; i < 22; i++)
                    {
                        Main.frmMain.dgvData.Rows[51].Cells[i + 1].Value = Main.frmMain.dgvData.Rows[33].Cells[i + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[33].Cells[i + 1].Value : 1;
                        Main.frmMain.dgvData.Rows[52].Cells[i + 1].Value = Main.frmMain.dgvData.Rows[31].Cells[i + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[31].Cells[i + 1].Value : 1;
                        Main.frmMain.dgvData.Rows[53].Cells[i + 1].Value = Main.frmMain.dgvData.Rows[32].Cells[i + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[32].Cells[i + 1].Value : 1;
                        Main.frmMain.dgvData.Rows[54].Cells[i + 1].Value = Main.frmMain.dgvData.Rows[30].Cells[i + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[30].Cells[i + 1].Value : 1;
                        Main.frmMain.dgvData.Rows[55].Cells[i + 1].Value = Main.frmMain.dgvData.Rows[34].Cells[i + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[34].Cells[i + 1].Value : 1;
                        Main.frmMain.dgvData.Rows[56].Cells[i + 1].Value = Main.frmMain.dgvData.Rows[36].Cells[i + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[36].Cells[i + 1].Value : 1;
                        Main.frmMain.dgvData.Rows[57].Cells[i + 1].Value = Main.frmMain.dgvData.Rows[35].Cells[i + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[35].Cells[i + 1].Value : 1;
                        Main.frmMain.dgvData.Rows[58].Cells[i + 1].Value = Main.frmMain.dgvData.Rows[37].Cells[i + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[37].Cells[i + 1].Value : 1;
                        Main.frmMain.dgvData.Rows[59].Cells[i + 1].Value = Main.frmMain.dgvData.Rows[41].Cells[i + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[41].Cells[i + 1].Value : 1;
                        Main.frmMain.dgvData.Rows[60].Cells[i + 1].Value = Main.frmMain.dgvData.Rows[39].Cells[i + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[39].Cells[i + 1].Value : 1;
                        Main.frmMain.dgvData.Rows[61].Cells[i + 1].Value = Main.frmMain.dgvData.Rows[40].Cells[i + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[40].Cells[i + 1].Value : 1;
                        Main.frmMain.dgvData.Rows[62].Cells[i + 1].Value = Main.frmMain.dgvData.Rows[38].Cells[i + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[38].Cells[i + 1].Value : 1;
                    }
                    #endregion
                    //判断结果--------流线1和流线2的产品一一对应得到12个结果以作最终结果------根据对应的结果抛料，抛掉所对应的产品。
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("判断产品OKNG");
                    for (int i = 0; i < 12; i++)//每一片产品对应的结果
                    {
                        if (CAMiClsVariable.strVision == "COGNEX"&&CAMiClsVariable.strProduct=="P104")//Cognex
                        {
                            CAMiClsVariable.productResult[i] = (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 15].Cells[7].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[3].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[13].Value));
                            //CAMiClsVariable.productResultT1[i] = 1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 15].Cells[3].Value);
                            //CAMiClsVariable.productResultT21[i] = 1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[3].Value);
                            //CAMiClsVariable.productResultT22[i] = 1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[13].Value);
                            if (Main.frmMain.dgvData.Rows[15 + i].Cells[2].Value.ToString().Length == 16)
                            {
                                CAMiClsVariable.productResult[i] = 0;//没扫到flexSN判定为NG，需抛料
                            }
                        }
                        else if (CAMiClsVariable.strVision == "COGNEX"&&CAMiClsVariable.strProduct=="D4X")
                        {
                            
                            CAMiClsVariable.productResult[i] = (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 15].Cells[3].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[3].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[13].Value));
                            CAMiClsVariable.productResultPDCA[i] =1-(1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 15].Cells[3].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[3].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[13].Value));
                            CAMiClsVariable.coil_lead_shift[i] =1-(1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[4].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[14].Value));
                            CAMiClsVariable.solder_scatter[i]=1-(1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[9].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[19].Value));
                            CAMiClsVariable.cold_solder[i]=1-(1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[11].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[21].Value));
                            CAMiClsVariable.UV_glue_overflow[i]=1-(1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[7].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[17].Value));
                            CAMiClsVariable.UV_glue_insufficient[i]=1-(1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[8].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[18].Value));
                            CAMiClsVariable.UV_Glue_bubble[i]=1-(1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[6].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[16].Value));
                            CAMiClsVariable.foreign_material[i]=1-(1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[10].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[20].Value));
                            CAMiClsVariable.liner_scalding[i] = 1-(1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[5].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[15].Value));
                            CAMiClsVariable.cient[i] = 0;
                            if (Main.frmMain.dgvData.Rows[15 + i].Cells[9].Value.ToString().Length == 16)
                            {
                                CAMiClsVariable.productResult[i] = 0;//没扫到flexSN判定为NG，需抛料
                            }
                        }
                        else if (CAMiClsVariable.strVision == "KEYENCE")//Cognex
                        {
                            CAMiClsVariable.productResult[i] = (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 15].Cells[4].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[3].Value));
                            CAMiClsVariable.productResultPDCA[i] = (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 15].Cells[4].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[3].Value));
                            if (Main.frmMain.dgvData.Rows[15 + i].Cells[5].Value.ToString() == "1")
                            {
                                CAMiClsVariable.productResult[i] = 0;//没扫到flexSN判定为NG，需抛料
                            }
                        }
                        Main.frmMain.dgvData.Rows[65].Cells[i + 1].Value = CAMiClsVariable.productResult[i] == 1 ? "OK" : "NG";
                        //Main.frmMain.dgvData.Rows[66].Cells[i + 1].Value = CAMiClsVariable.productResultT1[i] == 1 ? "OK" : "NG";
                        //Main.frmMain.dgvData.Rows[67].Cells[i + 1].Value = CAMiClsVariable.productResultT21[i] == 1 ? "OK" : "NG";
                        //Main.frmMain.dgvData.Rows[68].Cells[i + 1].Value = CAMiClsVariable.productResultT22[i] == 1 ? "OK" : "NG"; 
                        if (CAMiClsVariable.isProductSelect[i] == 1)
                        {
                            if (CAMiClsVariable.productResult[i] == 0)
                            {
                                NGCount++;
                                OneHourNGcount++;
                            }
                        }
                    }
                    for (int i = 0; i < 12; i++)
                    {
                        if (Main.frmMain.dgvData.Rows[28].Cells[i + 1].Value.ToString() == "1")//穴位有料
                        {
                            CAMiClsVariable.productResultT21[i] = 1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 30].Cells[3].Value);
                            CAMiClsVariable.productResultT22[i] = 1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 30].Cells[13].Value);
                            Main.frmMain.dgvData.Rows[67].Cells[i + 1].Value = CAMiClsVariable.productResultT21[i] == 1 ? "OK" : "NG";
                            Main.frmMain.dgvData.Rows[68].Cells[i + 1].Value = CAMiClsVariable.productResultT22[i] == 1 ? "OK" : "NG";
                        }
                    }
                    Main.frmMain.sw.Stop();
                    Main.frmMain.ListBoxDisplay("流线2--计算流线2所耗时间");
                    CAMiClsBali.strCycle_Time = (Main.frmMain.sw.ElapsedMilliseconds / 1000 + 5).ToString("f2");
                    //Main.frmMain.ListBoxDisplay("流线2--上传PDCA数据");
                    //while (CAMiClsBali.isSendOK)
                    //{
                    //    Thread.Sleep(2000);
                    //    Application.DoEvents();
                    //    Main.frmMain.ListBoxDisplay("等待PDCA上传完成！！！");
                    //}
                    //GetUPloadErrorCodeToPDCA();//方法获得NG_Errorcode
                    CAMiClsBali.GetSNAndData();//转移整理上传数据
                    //if (CAMiClsVariable.strVision == "KEYENCE")
                    //{
                    //    camiClsMethod.KeyencePict();
                    //}
                    //CAMiClsBali.isSendOK = true;
                    camiClsMethod.YieldDisplay();//显示良率                
                    CAMiClsVariable.isProductOkNG = (CAMiClsVariable.isProductSelect.Sum() != CAMiClsVariable.productResult.Sum()) ? false : true;
                    Main.frmMain.PauseCheck();//暂停
                    #region 结果输出
                    Main.frmMain.ListBoxDisplay("流线2--综合结果输出到表格");
                    for (int i = 0; i < 12; i++)//综合结果输出到表格
                    {
                        if (CAMiClsVariable.isProductSelect[i] == 1)//Main.frmMain.dgvData.Rows[80].Cells[i + 1].Value.ToString() == "1"
                        {
                            if (CAMiClsVariable.strVision == "COGNEX"&&CAMiClsVariable.strProduct=="P104")//Cognex
                            {
                                CAMiClsVariable.resultFinalData = CAMiClsVariable.strVision + "," + (i + 1).ToString() + "," + Main.frmMain.dgvData.Rows[i + 15].Cells[3].Value.ToString() + "," + Main.frmMain.dgvData.Rows[i + 15].Cells[2].Value.ToString() + "," + Main.frmMain.dgvData.Rows[i + 15].Cells[1].Value.ToString() + ",";//第10列为cyclone二维码+Flex二维码+T1
                                
                            }
                            else if (CAMiClsVariable.strVision == "COGNEX"&&CAMiClsVariable.strProduct=="D4X")
                            {
                                CAMiClsVariable.resultFinalData = CAMiClsVariable.strVision + "," + (i + 1).ToString() + "," + Main.frmMain.dgvData.Rows[i + 15].Cells[10].Value.ToString() + ",";
                            }
                            else if (CAMiClsVariable.strVision == "KEYENCE")//Keyence
                            {
                                CAMiClsVariable.resultFinalData = CAMiClsVariable.strVision + "," + (i + 1).ToString() + "," + Main.frmMain.dgvData.Rows[i + 15].Cells[1].Value.ToString() + ",";
                            }
                            CAMiClsVariable.resultOKorNG = Main.frmMain.dgvData.Rows[65].Cells[i + 1].Value.ToString();
                            //每片产品综合结果从表格整合成字符串
                            if (CAMiClsVariable.strProduct == "P104")
                            {
                                for (int j = 5; j < 17; j++)//上视觉部分
                                {
                                    CAMiClsVariable.resultFinalData += Main.frmMain.dgvData.Rows[i + 15].Cells[j].Value.ToString() + ",";
                                }
                            }
                            else
                            {
                                for (int j = 1; j < 11; j++)//上视觉部分 
                                {
                                    CAMiClsVariable.resultFinalData += Main.frmMain.dgvData.Rows[i + 15].Cells[j].Value.ToString() + ",";
                                }                               
                            }
                            if (CAMiClsVariable.strVision == "COGNEX")//Cognex
                            {
                                CAMiClsVariable.resultFinalData += "T21,";
                            }
                            else if (CAMiClsVariable.strVision == "KEYENCE")//Keyence
                            {
                                CAMiClsVariable.resultFinalData += "CC21,";
                            }
                            for (int k = 2; k < 22; k++)////下视觉部分
                            {
                                CAMiClsVariable.resultFinalData = CAMiClsVariable.resultFinalData + Main.frmMain.dgvData.Rows[i + 51].Cells[k].Value.ToString() + ",";
                            }
                            myMethod.RecordResultToExcel(CAMiClsVariable.resultFinalData, CAMiClsVariable.resultOKorNG, Main.configsn);//输出结果到excel   
                        }
                    }
                    #endregion
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("刷新结果区");
                    foreach (Label lb in Main.frmMain.groupPictDisplay.Controls.OfType<Label>())
                    {
                        if (CAMiClsVariable.isProductSelect[lb.TabIndex] == 1)
                        {
                            Main.frmMain.ChangeColor(lb, CAMiClsVariable.productResult[Convert.ToInt32(lb.TabIndex)] == 1 ? Color.Green : Color.Red);//改变label颜色
                        }
                    }
                    Main.frmMain.PauseCheck();//暂停
                    #region 产品NG时
                    CAMiClsVariable.isProductOkNG = true;
                   // CAMiClsMethod.NGTrayLoad();//确认Tray有无
                    if  (GClsMontion.ReadIOCard7432InputBit(0, 29) == 0)
                    {
                        GClsMontion.WriteCardExtendOutputBit(1, 12, 1);//蜂鸣器报警
                        MessageBox.Show("请放置Tray盘");
                        Application.DoEvents();
                        Thread.Sleep(1000);
                        GClsMontion.WriteCardExtendOutputBit(1, 12, 0);//蜂鸣器报警
                    }
                    if ((GClsMontion.ReadCardExtendInputBit(1, 6) == 1) || (GClsMontion.ReadCardExtendInputBit(1, 7) == 1))
                    {
                        GClsMontion.WriteCardExtendOutputBit(1, 12, 1);//蜂鸣器报警
                        MessageBox.Show("请确认Tray是否放好");
                        Application.DoEvents();
                        Thread.Sleep(1000);
                        GClsMontion.WriteCardExtendOutputBit(1, 12, 0);//蜂鸣器报警
                    }
                    if (!CAMiClsVariable.isProductOkNG)//产品NG时
                    {
                        Main.frmMain.ListBoxDisplay("产品NG");
                        Main.frmMain.PauseCheck();//暂停
                        //Main.frmMain.ListBoxDisplay("流线2--Z轴运行到初始位置");
                        //GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2InitialPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                        GClsMontion.WaitMotorStop(4);
                        Main.frmMain.PauseCheck();//暂停
                        do//等待NG Tray 更换动作完成
                        {
                            Thread.Sleep(10);
                            Application.DoEvents();
                            Main.frmMain.PauseCheck();//暂停
                        } while (!CAMiClsVariable.isNGTrayOK);
                        do
                        {
                            Main.frmMain.ListBoxDisplay("抛料--等待isTrayChange Off");
                            do
                            {
                                Thread.Sleep(10);
                                Application.DoEvents();
                            } while (CAMiClsVariable.isTrayChange);
                            for (int i = 0; i < 12; i++)//每个穴位判断
                            {
                                if (CAMiClsVariable.isProductSelect[i] == 1)
                                {
                                    if (CAMiClsVariable.productResult[i] != 1)
                                    {
                                        if (CAMiClsMethod.TrayCheck(i) == 1)//判断Tray是否可以放料，可抛为1
                                        {
                                            camiClsMethod.MoveToTossingPos(i);//运动到抛料位置并抛料
                                            Thread.Sleep(300);
                                            CAMiClsVariable.productResult[i] = 1;
                                            Main.frmMain.ListBoxDisplay("抛料--判断是否抛满");
                                            if ((CAMiClsVariable.isTrayCaveEmpty[0] + CAMiClsVariable.isTrayCaveEmpty[7] + CAMiClsVariable.isTrayCaveEmpty[8] == "000")
                                               || (CAMiClsVariable.isTrayCaveEmpty[2] + CAMiClsVariable.isTrayCaveEmpty[5] + CAMiClsVariable.isTrayCaveEmpty[10] == "000")
                                               || (CAMiClsVariable.isTrayCaveEmpty[1] + CAMiClsVariable.isTrayCaveEmpty[6] + CAMiClsVariable.isTrayCaveEmpty[9] == "000")
                                               || (CAMiClsVariable.isTrayCaveEmpty[3] + CAMiClsVariable.isTrayCaveEmpty[4] + CAMiClsVariable.isTrayCaveEmpty[11] == "000"))
                                            {
                                                CAMiClsVariable.isTrayChange = true;
                                            }
                                        }
                                        else
                                        {
                                            CAMiClsVariable.isTrayChange = true;
                                        }
                                    }
                                }
                            }
                            Thread.Sleep(10);
                            Application.DoEvents();
                        } while (CAMiClsVariable.isTrayChange);
                        Main.frmMain.ListBoxDisplay("流线2--Z轴抛料完运行到初始位置");
                        GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2InitialPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    }
                    #endregion
                    #region 流线2出料
                    //Main.frmMain.ListBoxDisplay("流线2--Z轴运行到初始位置");
                    //GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2InitialPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--CCD2 Y轴运行到抓料位置");
                    GClsMontion.AbsoluteMove(3, CAMiClsVariable.CCD2PickPosY, CAMiClsVariable.speedCCD2Y, CAMiClsVariable.TaccCCD2Y);
                    GClsMontion.AbsoluteMove(2, CAMiClsVariable.CCD2InitialPosX, CAMiClsVariable.speedCCD2X, CAMiClsVariable.TaccCCD2X);
                    GClsMontion.WaitMotorStop(3);
                    Main.frmMain.ListBoxDisplay("流线2--Z轴运行到抓料高度");
                    GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2PickPosZ - 8 * 1000, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--关闭所有真空吸及小气缸缩回");
                    camiClsMethod.StreamLine2VaccumOpenOrClose(0);
                    Thread.Sleep(10);
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--Z轴运行到初始位置");
                    GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2InitialPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                    GClsMontion.WaitMotorStop(2);
                    Main.frmMain.ListBoxDisplay("流线2--上传PDCA数据");
                    while (CAMiClsBali.isSendOK)
                    {
                        Thread.Sleep(2000);
                        Application.DoEvents();
                        Main.frmMain.ListBoxDisplay("等待PDCA上传完成！！！");
                    }
                    if (CAMiClsVariable.strVision == "KEYENCE")
                    {
                        camiClsMethod.KeyencePict();
                    }
                    CAMiClsBali.isSendOK = true;
                    Main.frmMain.PauseCheck();//暂停
                    CAMiClsMethod.StreamTrayRelease(2);//流线2产品放行                          
                    Main.frmMain.ListBoxDisplay("流线2--启动流线2");
                    GClsMontion.WriteCardExtendOutputBit(1, 8, 0);//流线2高速                   
                    GClsMontion.WriteCardExtendOutputBit(1, 6, 1);//流线2启动
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--等待流线2产品流出");
                    do//等待到位1感应
                    {
                        Thread.Sleep(200);
                        Application.DoEvents();
                        Main.frmMain.PauseCheck();//暂停
                    } while (GClsMontion.ReadCardExtendInputBit(1, 10) == 1 || GClsMontion.ReadCardExtendInputBit(1, 11) == 1 ||
                             GClsMontion.ReadCardExtendInputBit(1, 12) == 1 || GClsMontion.ReadCardExtendInputBit(1, 13) == 1 || GClsMontion.ReadCardExtendInputBit(1, 14) == 1);
                    Thread.Sleep(10);//临时空跑延时
                    Main.frmMain.ListBoxDisplay("流线2--停止流线2");
                    GClsMontion.WriteCardExtendOutputBit(1, 6, 0);//流线2停止                    
                    Main.frmMain.ListBoxDisplay("流线2--流线2阻挡气缸伸出");
                    CAMiClsCylinder.StreamLine2StopExtend();//阻挡气缸伸出   
                    //DetectNG3Pcs(NGCount);//检测是否NG3pcs
                    //NGCount = 0;
                    //if (OneHourNGcount >= 5)
                    //{
                    //    DetectNG5Pcs("Detect");
                    //    OneHourNGcount = 0;
                    //}
                    //检测1小时内是否NG5pcs
                    Main.frmMain.ListBoxDisplay("发送Group");
                    CAMiClsMethod.Group(2);//发送Group2
                    Main.frmMain.PauseCheck();//暂停
                    CAMiClsVariable.isStreamLine2Ready = true;
                    Main.frmMain.PauseCheck();//暂停
                    #endregion
                }
                catch (Exception ex)
                {
                    myMethod.LogRecord(ex.ToString());
                }
            }
        }
        public void ThNGStreamLineRunning()//NG Tray
        {
            #region
            //while (true)
            //{
            //    try
            //    {                  
            //        Main.frmMain.ListBoxDisplay("NG--NG Tray上料");
            //        CAMiClsMethod.NGTrayLoad();
            //        Main.frmMain.ListBoxDisplay("NG--NG Tray上料完成");
            //        //CAMiClsVariable.isTrayChange = false;//是否更换Tray
            //        CAMiClsVariable.isNGTrayOK = true;
            //        Main.frmMain.ListBoxDisplay("NG--等待NG产品放置");
            //        do//等待NG料
            //        {
            //            Thread.Sleep(10);
            //            Application.DoEvents();
            //            Main.frmMain.PauseCheck();//暂停
            //        } while (CAMiClsVariable.isNGTrayOK);
            //        Main.frmMain.ListBoxDisplay("NG--NG Tray下料");
            //        CAMiClsMethod.NGTrayUnload();
            //        Main.frmMain.ListBoxDisplay("NG--等待按下NG Tray按钮");
            //        do
            //        {
            //            Thread.Sleep(10);
            //            Application.DoEvents();
            //            Main.frmMain.PauseCheck();//暂停
            //        } while (GClsMontion.ReadCardInputBit(1, 3) == 0);
            //    }
            //    catch (Exception ex)
            //    {
            //        //MessageBox.Show(ex.ToString());
            //        myMethod.LogRecord(ex.ToString());
            //    }
            //}

            //            while (true)
            //{
            #endregion
            while (true)
            {
                try
                {
                    Main.frmMain.ListBoxDisplay("NG--NG Tray上料");
                    CAMiClsMethod.NGTrayLoad();
                    Main.frmMain.ListBoxDisplay("NG--NG Tray上料完成");
                    //CAMiClsVariable.isTrayChange = false;//是否更换Tray
                    CAMiClsVariable.isNGTrayOK = true;
                    Main.frmMain.ListBoxDisplay("NG--等待NG产品放置");
                    do//等待NG料
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        Main.frmMain.PauseCheck();//暂停
                        if (GClsMontion.ReadCardInputBit(1, 3) == 1)
                        {
                            Thread.Sleep(750);
                            if (GClsMontion.ReadCardInputBit(1, 3) == 1)
                            {
                                Thread.Sleep(750);
                                if (GClsMontion.ReadCardInputBit(1, 3) == 1)
                                {
                                    Thread.Sleep(750);
                                    NGTrayForceOpen = true;
                                    //Main.frmMain.isStartPushJudge = false;
                                    //Main.frmMain.isResetPushJudge = false;
                                    //Main.frmMain.isMachineStop = true;
                                    //Main.frmMain.isRunningPause = true;
                                    //Main.frmMain.isStopPushJudge = true;
                                }
                            }
                        }
                    } while (CAMiClsVariable.isNGTrayOK && NGTrayForceOpen == false);
                    Main.frmMain.ListBoxDisplay("NG--NG Tray下料");
                    NGTrayForceOpen = false;
                    CAMiClsMethod.NGTrayUnload();
                    Main.frmMain.ListBoxDisplay("NG--等待按下NG Tray按钮");                   
                    do
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        //Main.frmMain.PauseCheck();//暂停
                    } while (GClsMontion.ReadCardInputBit(1, 3) == 0);
                    GClsMontion.WriteCardExtendOutputBit(1, 12, 0);//蜂鸣器报警
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    myMethod.LogRecord(ex.ToString());
                }

            }


        }
        public void ThTossing()
        {
            while (true)
            {
                try
                {
                    Main.frmMain.ListBoxDisplay("抛料--等待isTrayChange On");
                    do
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                    } while (!CAMiClsVariable.isTrayChange);
                    GClsMontion.WriteCardExtendOutputBit(1, 12, 1);//蜂鸣器报警
                    Main.frmMain.MachineResultDisplay("Tray盘已满，请更换！");
                    CAMiClsVariable.isNGTrayOK = false;
                    Main.frmMain.ListBoxDisplay("抛料--等待Tray更换完成");
                    do//等待NG Tray 更换动作完成
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        Main.frmMain.PauseCheck();//暂停
                    } while (!CAMiClsVariable.isNGTrayOK);
                    CAMiClsVariable.isTrayChange = false;
                    Main.frmMain.MachineResultDisplay("机台运行中");
                }
                catch (Exception ex)
                {
                    myMethod.LogRecord(ex.ToString());
                }
            }
        }    
        public void DetectNG3Pcs(int NGCount)//判断是否有连续3片NG
        {
            if (NGCount > 2)
            {
                MessageBox.Show("连续NG超过3pcs，请确认！！！");

            }
        }

        public void DetectNG5Pcs(string Mode)//判断1小时是否连续NG5Pcs
        {
            try
            {
                if (Mode == "RecordTime")
                {
                    Hour = Convert.ToInt16(DateTime.Now.ToString("HH"));
                    minute = Convert.ToInt16(DateTime.Now.ToString("mm"));
                }
                else
                {

                    if (Convert.ToInt16(DateTime.Now.ToString("HH")) - Hour == 1 || Convert.ToInt16(DateTime.Now.ToString("HH")) - Hour == -1 ||
                        Convert.ToInt16(DateTime.Now.ToString("HH")) - Hour == 23 || Convert.ToInt16(DateTime.Now.ToString("HH")) - Hour == -23)
                    {
                        if (Convert.ToInt16(DateTime.Now.ToString("mm")) - minute < 0)
                        {
                            MessageBox.Show("一小时内连续NG超过5pcs，请确认！！！");
                            Hour = Convert.ToInt16(DateTime.Now.ToString("HH"));
                            minute = Convert.ToInt16(DateTime.Now.ToString("mm"));
                        }
                    }
                    else if (Convert.ToInt16(DateTime.Now.ToString("HH")) == Hour)
                    {
                        MessageBox.Show("一小时内连续NG超过5pcs，请确认！！！");
                        Hour = Convert.ToInt16(DateTime.Now.ToString("HH"));
                        minute = Convert.ToInt16(DateTime.Now.ToString("mm"));
                    }

                }
            }
            catch
            {


            }
        }
        public static void GetUPloadErrorCodeToPDCA()//获取上传PDCAErrorCode
        {
            //0=Pass
            //1=coil lead shift         线圈偏移
            //2 = solder scatter        少锡
            //3 = cold solder           溢锡
            //4 = UV glue overflow      溢胶
            //5 = UV glue insufficient  缺胶
            //6 = UV Glue bubble        气泡
            //7 = foreign material      异物
            //8 = liner scalding        烫伤
            //9 = A>1&X>23.55           
            //10=A<-1&X<23.55
            for (int i = 0; i < CAMiClsVariable.NG_ErrorCode.Length; i++)//默认全部OK,上传0
            {
                CAMiClsVariable.NG_ErrorCode[i] = 0;
            }

            for (int i = 0; i < 12; i++)//顺便整理上传的ErrorCode
            {
                if (CAMiClsVariable.isProductSelect[i] == 1)
                {                  
                    if (Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 15].Cells[3].Value)==1)  //D4X判断第三列，104第六列
                    {
                        if (Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 15].Cells[6].Value) == 1) //A>1&X>23.55
                        {
                            CAMiClsVariable.NG_ErrorCode[i] = 9;
                        }
                        else if (Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 15].Cells[7].Value) == 1)//10=A<-1&X<23.55
                        {
                            CAMiClsVariable.NG_ErrorCode[i] = 10;
                        }
                    }
                    int j = 0;
                    while (j < 18 && CAMiClsVariable.NG_ErrorCode[i] == 0)
                    {
                        if (Convert.ToInt16(Main.frmMain.dgvData.Rows[51 + i].Cells[j + 4].Value) == 1)
                        {
                            switch (j + 4)
                            {
                                //线圈偏位
                                case 4:
                                case 14:
                                    CAMiClsVariable.NG_ErrorCode[i] = 1;
                                    break;
                                //烫伤
                                case 5:
                                case 15:
                                    CAMiClsVariable.NG_ErrorCode[i] = 8;
                                    break;
                                //气泡
                                case 6:
                                case 16:
                                    CAMiClsVariable.NG_ErrorCode[i] = 5;
                                    break;
                                //溢胶
                                case 7:
                                case 17:
                                    CAMiClsVariable.NG_ErrorCode[i] = 4;
                                    break;
                                //缺胶
                                case 8:
                                case 18:
                                    CAMiClsVariable.NG_ErrorCode[i] = 5;
                                    break;
                                //少锡
                                case 9:
                                case 19:
                                    CAMiClsVariable.NG_ErrorCode[i] = 2;
                                    break;
                                //异物
                                case 10:
                                case 20:
                                    CAMiClsVariable.NG_ErrorCode[i] = 7;
                                    break;
                                //溢锡
                                case 11:
                                case 21:
                                    CAMiClsVariable.NG_ErrorCode[i] = 3;
                                    break;
                            }
                        }
                        Thread.Sleep(10);
                        Application.DoEvents();
                        j++;
                        if (j == 8)
                        {
                            j = 10;
                        }
                    }
                }
                //CAMiClsVariable.NG_ErrorCode[i] = 9;
            }
        }

    }
}
