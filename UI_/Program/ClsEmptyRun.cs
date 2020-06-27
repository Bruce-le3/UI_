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
    public class ClsEmptyRun
    {
        public GClsMethod myEmptyMethod = new GClsMethod();
        public CAMiClsMethod camiEmptyClsMethod = new CAMiClsMethod(); 
      
        public void ThStreamFirstLineEmptyRunning()//第一条流线-自动
        {
            while (true)
            {
                try
                {
                    Main.frmMain.ListBoxDisplay("流线1--运行开始-----------------------");
                    Main.frmMain.ListBoxDisplay("流线1--等待流线1入料感应感应有产品");
                    Main.frmMain.PauseCheck();//暂停
                    //do//等待流线1入料感应感应到
                    //{
                    //    Thread.Sleep(10);
                    //    Application.DoEvents();
                    //    Main.frmMain.PauseCheck();//暂停
                    //} while (GClsMontion.ReadCardExtendInputBit(0, 10) != 1);
                    Thread.Sleep(100);
                    for (int i = 0; i < 12; i++)//加载流线1产品选择使能
                    {
                        Main.frmMain.dgvData.Rows[27].Cells[i + 1].Value = CAMiClsVariable.isProductSelect[i];//流线1产品使能放置在27行
                    }
                    //CAMiClsVariable.CCD1productSelectNO = 0;
                    //for (int i = 0; i < 12; i++)
                    //{
                    //    CAMiClsVariable.CCD1productSelectNO += Convert.ToInt32(Main.frmMain.dgvData.Rows[27].Cells[i + 1].Value);
                    //}
                    //Main.frmMain.ListBoxDisplay(string.Format("流线1--流线1产品需检测的个数为{0}", CAMiClsVariable.CCD1productSelectNO));
                    CAMiClsMethod.StreamCCDDataClear(1);//清空数据存放区
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--流线1启动");
                    GClsMontion.WriteCardExtendOutputBit(0, 6, 1);//流线1启动
                    Main.frmMain.ListBoxDisplay("流线1--等待流线1到位感应1感应");
                    //do//等待流线有料感应2
                    //{
                    //    Thread.Sleep(10);
                    //    Application.DoEvents();
                    //    Main.frmMain.PauseCheck();//暂停
                    //} while (GClsMontion.ReadCardExtendInputBit(0, 11) != 1);
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--等待流线1到位感应2感应");
                    //do//等待到位2感应
                    //{
                    //    Thread.Sleep(10);
                    //    Application.DoEvents();
                    //    Main.frmMain.PauseCheck();//暂停
                    //} while (GClsMontion.ReadCardExtendInputBit(0, 12) != 1);
                    Thread.Sleep(200);//先延时再停止，根据需求更改
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--停止流线1");
                    GClsMontion.WriteCardExtendOutputBit(0, 6, 0);//流线1停止
                    Main.frmMain.PauseCheck();//暂停                    
                    CAMiClsMethod.StreamTrayLocate(1);//流线1产品定位
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--清空CCD1缓存数据");
                    //CAMiClsVariable.cClient.ClientReceive(1, 0);
                    Main.frmMain.ListBoxDisplay("流线1--定位结束，开始拍照");
                    //开始拍照
                    #region CCD1拍照
                    for (int i = 0; i < 12; i++)
                    {
                        if (Main.frmMain.dgvData.Rows[27].Cells[i + 1].Value.ToString() == "1")//穴位使能打开
                        {
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
                            //CAMiClsVariable.cClient.ClientSend(1, (CAMiClsVariable.strTrig1 + ",0," + (i + 1).ToString()));
                            Main.frmMain.PauseCheck();//暂停
                            Thread.Sleep(10);
                            //CAMiClsVariable.strCCD1Recv = CAMiClsMethod.ReceiveData(i, 1);
                            //if (CAMiClsVariable.strCCD1Recv.Substring(0, 2) != "T1")
                            //{
                            //    MessageBox.Show("CCD1 拍照T1反馈异常");
                            //}
                            Main.frmMain.ListBoxDisplay("流线1--等待CCD1-第" + (i + 1).ToString() + "个反馈结果");
                        }
                        Main.frmMain.PauseCheck();//暂停                       
                    }
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("CCD1X轴/CCD1Y轴运动到初始位置");
                    GClsMontion.AbsoluteMove(0, CAMiClsVariable.CCD1InitialPosX, CAMiClsVariable.speedCCD1X, CAMiClsVariable.TaccCCD1X);
                    GClsMontion.AbsoluteMove(1, CAMiClsVariable.CCD1InitialPosY, CAMiClsVariable.speedCCD1Y, CAMiClsVariable.TaccCCD1Y);
                    Main.frmMain.ListBoxDisplay("流线1--获取CCD1数据");
                    //while (CAMiClsVariable.strRecvCCD1Data2Array[CAMiClsVariable.CCD1productSelectNO - 1] == "0")
                    //{
                    //    CAMiClsVariable.recvCCD1DataNO = 0;
                    //    CAMiClsVariable.strCCD1Recv = CAMiClsMethod.ReceiveData(0, 1);
                    //}
                    for (int i = 0; i < 12; i++)
                    {
                        Main.frmMain.ListBoxDisplay("流线1--等待CCD1-第" + (i + 1).ToString() + "个反馈结果");
                        if (Convert.ToInt32(Main.frmMain.dgvData.Rows[27].Cells[i + 1].Value) == 1)//穴位使能打开
                        {
                            CAMiClsVariable.recvCCD1DataNO = 0;
                            for (int m = 0; m < i + 1; m++)
                            {
                                CAMiClsVariable.recvCCD1DataNO += Convert.ToInt32(Main.frmMain.dgvData.Rows[27].Cells[m + 1].Value);
                            }
                            //CAMiClsVariable.recvCCD1DataArray = CAMiClsVariable.strRecvCCD1Data2Array[CAMiClsVariable.recvCCD1DataNO - 1].Split(',');
                            //Main.frmMain.dictFirstLine.Clear();//清空数据
                            //for (int j = 0; j < CAMiClsVariable.recvCCD1DataArray.Length; j++)
                            //{
                            //    Main.frmMain.dictFirstLine.Add("第" + i.ToString() + "次拍照的第" + j.ToString() + "次结果", CAMiClsVariable.recvCCD1DataArray[j]);
                            //    Main.frmMain.dgvData.Rows[i + 1].Cells[j + 1].Value = CAMiClsVariable.recvCCD1DataArray[j];
                            //    Main.frmMain.ListBoxDisplay(string.Format("流线1--相机接收到第" + (i + 1).ToString() + "组数据的第" + (j + 1).ToString() + "个数据  {0}", CAMiClsVariable.recvCCD1DataArray[j]));
                            //}
                        }
                    }
                    #endregion
                    Main.frmMain.PauseCheck();//暂停
                    CAMiClsMethod.StreamTrayRelease(1);//流线1产品放行
                    Main.frmMain.ListBoxDisplay("流线1--等待流线2允许进料");
                    do
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        Main.frmMain.PauseCheck();//暂停
                    } while (!CAMiClsVariable.isStreamLine2Ready);
                    Thread.Sleep(1000);
                    Main.frmMain.PauseCheck();//暂停
                    for (int i = 15; i < 27; i++)//清空数据用来放置流线1的数据
                    {
                        for (int j = 1; j < 23; j++)
                        {
                            Main.frmMain.dgvData.Rows[i].Cells[j].Value = "";
                        }
                    }
                    CAMiClsVariable.isStreamLine1Done = true;
                    CAMiClsVariable.isStreamLine2Ready = false;
                    Main.frmMain.ListBoxDisplay("流线1--转移数据至表格第15-26行");
                    //for (int k = 0; k < 12; k++)
                    //{
                    //    for (int m = 0; m < CAMiClsVariable.recvCCD1DataArray.Length; m++)
                    //    {
                    //        Main.frmMain.dgvData.Rows[k + 15].Cells[m + 1].Value = Main.frmMain.dgvData.Rows[k + 1].Cells[m + 1].Value.ToString() != "" ? Main.frmMain.dgvData.Rows[k + 1].Cells[m + 1].Value : 1;
                    //    }
                    //}
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--启动流线1");
                    GClsMontion.WriteCardExtendOutputBit(0, 8, 0);//流线1高速                
                    GClsMontion.WriteCardExtendOutputBit(0, 6, 1);//流线1启动
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线1--等待流线1产品流出");
                    Thread.Sleep(3000);
                    do//等待流线1到位感应无
                    {
                        Thread.Sleep(10);
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
                    GClsMontion.WaitMotorStop(0);
                    GClsMontion.WaitMotorStop(1);
                }
                catch (Exception ex)
                {
                    myEmptyMethod.LogRecord(ex.ToString());
                }
            }
        }
        public void ThStreamSecondLineEmptyRunning()//第二条流线自动
        {
            while (true)
            {
                try
                {
                    Main.frmMain.ListBoxDisplay("流线2--运行开始-----------------------");
                    CAMiClsVariable.CCD2productSelectNO = 0;
                    Main.frmMain.PauseCheck();//暂停                    
                    Main.frmMain.ListBoxDisplay("清空结果区");
                    CAMiClsMethod.StreamCCDDataClear(2);//清空数据存放区
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
                    for (int i = 0; i < 12; i++)
                    {
                        CAMiClsVariable.CCD2productSelectNO += Convert.ToInt32(Main.frmMain.dgvData.Rows[28].Cells[i + 1].Value);
                    }
                    Main.frmMain.ListBoxDisplay(string.Format("流线2--流线2产品需检测的个数为{0}", CAMiClsVariable.CCD2productSelectNO));
                    CAMiClsVariable.CCD2productSelectNO = CAMiClsVariable.CCD2productSelectNO / 2;
                    Main.frmMain.ListBoxDisplay("流线2--等待流线1完成");
                    Main.frmMain.PauseCheck();//暂停
                    do//等待流线1完成
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        Main.frmMain.PauseCheck();//暂停
                    } while (!CAMiClsVariable.isStreamLine1Done);
                    CAMiClsVariable.isStreamLine1Done = false;

                    CAMiClsMethod.ChangeResultDisplay();//初始化结果显示颜色              
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--启动流线2");
                    GClsMontion.WriteCardExtendOutputBit(1, 8, 0);//流线2高速                   
                    GClsMontion.WriteCardExtendOutputBit(1, 6, 1);//流线2启动
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--等待流线2到位感应1感应");
                    //do//等待流线2到位1感应
                    //{
                    //    Thread.Sleep(10);
                    //    Application.DoEvents();
                    //} while (GClsMontion.ReadCardExtendInputBit(1, 10) != 1);
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--等待流线2到位感应2感应");
                    //do//等待到位2感应
                    //{
                    //    Thread.Sleep(10);
                    //    Application.DoEvents();
                    //    Main.frmMain.PauseCheck();//暂停
                    //} while (GClsMontion.ReadCardExtendInputBit(1, 11) != 1);
                    Thread.Sleep(200);//先延时再停止，根据需求更改
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--停止流线2");
                    GClsMontion.WriteCardExtendOutputBit(1, 6, 0);//流线2停止
                    CAMiClsVariable.isProductLocateInStreamLine2 = true;//流线2产品到位                    
                    CAMiClsMethod.StreamTrayLocate(2);//流线2产品定位
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--马达模组运动到抓料位置");
                    GClsMontion.AbsoluteMove(3, CAMiClsVariable.CCD2PickPosY, CAMiClsVariable.speedCCD2Y, CAMiClsVariable.TaccCCD2Y);
                    GClsMontion.WaitMotorStop(3);
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--Z轴下降到抓料位置");
                    GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2PickPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                    Main.frmMain.PauseCheck();//暂停
                    Thread.Sleep(10);
                    #region  真空吸打开及判断
                    Main.frmMain.ListBoxDisplay("流线2--打开所有真空吸");
                    for (int i = 0; i < 12; i++)//打开所有真空吸以及小气缸伸出
                    {
                        if (CAMiClsVariable.isProductSelect[i] == 1)//产品选中
                        {
                            CAMiClsCylinder.GeneralClynderMotion(2, 0, i, CAMiClsVariable.isProductSelect[i]);
                            //CAMiClsCylinder.GeneralClynderMotionSensor(2, 0, i, CAMiClsVariable.isProductSelect[i], 3000, 44 + (i + 1) * 2 - CAMiClsVariable.isProductSelect[i]);
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
                    Thread.Sleep(500);
                    #endregion
                    Main.frmMain.PauseCheck();//暂停PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--Z轴上升到初始高度");
                    GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2InitialPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    #region  cyclone二维码临时转移至第29行
                    Main.frmMain.dgvData.Rows[29].Cells[1].Value = Main.frmMain.dgvData.Rows[18].Cells[10].Value;
                    Main.frmMain.dgvData.Rows[29].Cells[2].Value = Main.frmMain.dgvData.Rows[16].Cells[10].Value;
                    Main.frmMain.dgvData.Rows[29].Cells[3].Value = Main.frmMain.dgvData.Rows[17].Cells[10].Value;
                    Main.frmMain.dgvData.Rows[29].Cells[4].Value = Main.frmMain.dgvData.Rows[15].Cells[10].Value;
                    Main.frmMain.dgvData.Rows[29].Cells[5].Value = Main.frmMain.dgvData.Rows[19].Cells[10].Value;
                    Main.frmMain.dgvData.Rows[29].Cells[6].Value = Main.frmMain.dgvData.Rows[21].Cells[10].Value;
                    Main.frmMain.dgvData.Rows[29].Cells[7].Value = Main.frmMain.dgvData.Rows[20].Cells[10].Value;
                    Main.frmMain.dgvData.Rows[29].Cells[8].Value = Main.frmMain.dgvData.Rows[22].Cells[10].Value;
                    Main.frmMain.dgvData.Rows[29].Cells[9].Value = Main.frmMain.dgvData.Rows[26].Cells[10].Value;
                    Main.frmMain.dgvData.Rows[29].Cells[10].Value = Main.frmMain.dgvData.Rows[24].Cells[10].Value;
                    Main.frmMain.dgvData.Rows[29].Cells[11].Value = Main.frmMain.dgvData.Rows[25].Cells[10].Value;
                    Main.frmMain.dgvData.Rows[29].Cells[12].Value = Main.frmMain.dgvData.Rows[23].Cells[10].Value;
                    #endregion
                    GClsMontion.WaitMotorStop(4);
                    //开始拍照
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--清空CCD2缓存数据");
                    //CAMiClsVariable.cClient.ClientReceive(2, 0);
                    #region CCD2拍照
                    Main.frmMain.ListBoxDisplay("流线2--开始拍照");
                    for (int i = 0; i < 12; i++)
                    {
                        if (Main.frmMain.dgvData.Rows[28].Cells[i + 1].Value.ToString() == "1")
                        {
                            //运行到拍照位置
                            Main.frmMain.ListBoxDisplay("流线2--运行到CCD2-第" + (i + 1).ToString() + "个拍照位置");
                            GClsMontion.AbsoluteMove(2, CAMiClsVariable.CCD2PosXValue[i], CAMiClsVariable.speedCCD2X, CAMiClsVariable.TaccCCD2X);
                            GClsMontion.AbsoluteMove(3, CAMiClsVariable.CCD2PosYValue[i], CAMiClsVariable.speedCCD2Y, CAMiClsVariable.TaccCCD2Y);
                            GClsMontion.WaitMotorStop(2);
                            GClsMontion.WaitMotorStop(3);
                            GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2TakePhotoPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                            GClsMontion.WaitMotorStop(4);
                            Thread.Sleep(10);
                            Main.frmMain.PauseCheck();//暂停
                            Main.frmMain.ListBoxDisplay("流线2--触发拍照");//触发拍照
                            //if (i % 2 == 0)
                            //{
                            //    CAMiClsVariable.cClient.ClientSend(2, (CAMiClsVariable.strTrig2 + ",0," + CAMiClsMethod.GetCavityNum(i) + ","
                            //        + Main.frmMain.dgvData.Rows[29].Cells[i + 1].Value.ToString().Trim() + "," + Main.frmMain.dgvData.Rows[29].Cells[i + 2].Value.ToString().Trim()));//",DLC845700BPL8WX2N+067,DLC8457003VL8WX26+067"
                            //    Thread.Sleep(10);
                            //    Main.frmMain.ListBoxDisplay("流线2--接收数据");
                            //    Main.frmMain.PauseCheck();//暂停
                            //    CAMiClsVariable.strCCD2Recv1 = CAMiClsMethod.ReceiveData(i, 2);
                            //    if (CAMiClsVariable.strCCD2Recv1.Substring(0, 3) != "T21")
                            //    {
                            //        MessageBox.Show("CCD2 拍照T21反馈异常");
                            //    }
                            //}
                            //else if (i % 2 == 1)
                            //{
                            //    CAMiClsVariable.cClient.ClientSend(2, (CAMiClsVariable.strTrig3 + ",0," + CAMiClsMethod.GetCavityNum(i) + ","
                            //        + Main.frmMain.dgvData.Rows[29].Cells[i].Value.ToString().Trim() + "," + Main.frmMain.dgvData.Rows[29].Cells[i + 1].Value.ToString().Trim()));//T22...",DLC845700BPL8WX2N+067,DLC8457003VL8WX26+067"
                            //    Thread.Sleep(10);
                            //    Main.frmMain.PauseCheck();//暂停
                            //    Main.frmMain.ListBoxDisplay("流线2--接收数据");
                            //    CAMiClsVariable.strCCD2Recv2 = CAMiClsMethod.ReceiveData(i, 2);
                            //    if (CAMiClsVariable.strCCD2Recv2.Substring(0, 3) != "T22")
                            //    {
                            //        MessageBox.Show("CCD2 拍照T22反馈异常");
                            //    }
                            //    Main.frmMain.PauseCheck();//暂停
                            //    Thread.Sleep(2000);
                            //    Application.DoEvents();
                            //}
                        }
                    }
                    Main.frmMain.ListBoxDisplay("流线2--获取CCD2数据");
                    //while (CAMiClsVariable.strRecvCCD2Data2Array[CAMiClsVariable.CCD2productSelectNO - 1] == "0")
                    //{
                    //    CAMiClsVariable.recvCCD2DataNO = 0;
                    //    CAMiClsVariable.strCCD2Recv2 = CAMiClsMethod.ReceiveData(0, 2);
                    //}
                    //for (int i = 0; i < 6; i++)
                    //{
                        //T22,Status_1:CCD2视野中物料是否存在,”1”代表有, ”0”代表无;
                        //Status_2:CCD2综合判断状态, ,”1”代表OK,  ”0”代表NG
                        //Status_3:线圈偏位 内;Status_4:线圈偏位;Status_5:UV溢胶判断;Status_6:UV溢胶判断
                        //Status_7:黑点;Status_8:黑点;Status_9:烫伤;Status_10:烫伤
                        //Status_11:气泡;Status_12:气泡;Status_13:缺锡;Status_14:缺锡......Status_20:
                        //Status_21........Status_40同上
                        //2号产品得到的数据是第3号产品的数据------------------------------------------------------------
                        //int strCCD2Recv2Length = receiveData2[i].Length;//获取CCD2反馈数据长度，用于去掉最后的\n\r
                        //ClsVariable.strCCD2Recv2 = receiveData2[i].Substring(0, strCCD2Recv2Length - 1);
                        //ClsVariable.recvCCD2DataArray = ClsVariable.strCCD2Recv2.Split(',');
                        //if (i % 2 == 0)
                        //{
                        //    try
                        //    {
                        //        if (Convert.ToInt32(Main.frmMain.dgvData.Rows[49].Cells[i * 2 + 1].Value) == 1)
                        //        {
                        //            CAMiClsVariable.recvCCD2DataNO = 0;
                        //            for (int m = 0; m < i * 2 + 1; m++)
                        //            {
                        //                CAMiClsVariable.recvCCD2DataNO += Convert.ToInt32(Main.frmMain.dgvData.Rows[49].Cells[m + 1].Value);
                        //            }
                        //            CAMiClsVariable.recvCCD2DataNO += Convert.ToInt32(Main.frmMain.dgvData.Rows[49].Cells[i * 2 + 1 + 2].Value);
                        //            CAMiClsVariable.recvCCD2DataNO = CAMiClsVariable.recvCCD2DataNO / 2;
                        //            CAMiClsVariable.recvCCD2DataArray = CAMiClsVariable.strRecvCCD2Data2Array[CAMiClsVariable.recvCCD2DataNO - 1].Split(',');
                        //            Main.frmMain.dictSecondLine.Clear();//将数据存放到指定位置，以便后续使用
                        //            for (int j = 0; j < CAMiClsVariable.recvCCD2DataArray.Length; j++)
                        //            {
                        //                if (j < 21)
                        //                {
                        //                    Main.frmMain.dgvData.Rows[i * 2 + 30].Cells[j + 1].Value = CAMiClsVariable.recvCCD2DataArray[j];//第一组数据
                        //                }
                        //                else
                        //                {
                        //                    Main.frmMain.dgvData.Rows[i * 2 + 31].Cells[j - 21 + 2].Value = CAMiClsVariable.recvCCD2DataArray[j];//换行放第二组数据
                        //                }
                        //            }
                        //        }
                        //    }
                        //    catch (Exception err)
                        //    {
                        //        MessageBox.Show(err.ToString());
                        //    }
                        //}
                        //else if (i % 2 == 1)
                        //{
                        //    try
                        //    {
                        //        if (Convert.ToInt32(Main.frmMain.dgvData.Rows[49].Cells[i * 2].Value) == 1)
                        //        {
                        //            CAMiClsVariable.recvCCD2DataNO = 0;
                        //            for (int m = 0; m < (i + 1) * 2; m++)
                        //            {
                        //                CAMiClsVariable.recvCCD2DataNO += Convert.ToInt32(Main.frmMain.dgvData.Rows[49].Cells[m + 1].Value);
                        //            }
                        //            CAMiClsVariable.recvCCD2DataNO = CAMiClsVariable.recvCCD2DataNO / 2;
                        //            //CAMiClsVariable.recvCCD2DataArray = CAMiClsVariable.strRecvCCD2Data2Array[CAMiClsVariable.recvCCD2DataNO - 1].Split(',');
                        //            Main.frmMain.dictSecondLine.Clear(); //将数据存放到指定位置，以便后续使用
                        //            for (int j = 0; j < CAMiClsVariable.recvCCD2DataArray.Length; j++)
                        //            {
                        //                if (j < 21)
                        //                {
                        //                    Main.frmMain.dgvData.Rows[i * 2 + 30].Cells[j + 1].Value = CAMiClsVariable.recvCCD2DataArray[j];//第一组数据
                        //                }
                        //                else
                        //                {
                        //                    Main.frmMain.dgvData.Rows[i * 2 + 31].Cells[j - 21 + 2].Value = CAMiClsVariable.recvCCD2DataArray[j];//换行放第二组数据
                        //                }
                        //            }
                        //        }
                        //    }
                        //    catch (Exception exx)
                        //    {
                        //        MessageBox.Show(exx.ToString());
                        //    }
                        //}
                    //}
                    CAMiClsVariable.recvCCD2DataNO = 0;
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
                    //判断结果--------流线1和流线2的产品一一对应得到12个结果以作最终结果------根据对应的结果抛料，抛掉所对应的产品。--------------------------------------              
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("判断产品OKNG");
                    //for (int i = 0; i < 12; i++)//每一片产品对应的结果
                    //{
                    //    CAMiClsVariable.productResult[i] = (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 15].Cells[3].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[3].Value)) * (1 - Convert.ToInt32(Main.frmMain.dgvData.Rows[i + 51].Cells[13].Value));
                    //    Main.frmMain.dgvData.Rows[65].Cells[i + 1].Value = CAMiClsVariable.productResult[i] == 1 ? "OK" : "NG";
                    //}
                    camiEmptyClsMethod.YieldDisplay();//显示良率                
                    CAMiClsVariable.isProductOkNG = (CAMiClsVariable.isProductSelect.Sum() != CAMiClsVariable.productResult.Sum()) ? false : true;
                    CAMiClsVariable.isProductOkNG = true;
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--综合结果输出到表格");
                    for (int i = 0; i < 12; i++)//综合结果输出到表格
                    {
                        if (Main.frmMain.dgvData.Rows[80].Cells[i + 1].Value.ToString() == "1")
                        {
                            CAMiClsVariable.resultFinalData = "Cognex," + (i + 1).ToString() + "," + Main.frmMain.dgvData.Rows[i + 15].Cells[10].Value.ToString() + ",";
                            CAMiClsVariable.resultOKorNG = Main.frmMain.dgvData.Rows[65].Cells[i + 1].Value.ToString();
                            //每片产品综合结果从表格整合成字符串
                            for (int j = 1; j < 11; j++)//上视觉部分
                            {
                                CAMiClsVariable.resultFinalData += Main.frmMain.dgvData.Rows[i + 15].Cells[j].Value.ToString() + ",";
                            }
                            CAMiClsVariable.resultFinalData += "T21,";
                            for (int k = 2; k < 22; k++)////下视觉部分
                            {
                                CAMiClsVariable.resultFinalData = CAMiClsVariable.resultFinalData + Main.frmMain.dgvData.Rows[i + 51].Cells[k].Value.ToString() + ",";
                            }
                            myEmptyMethod.RecordResultToExcel(CAMiClsVariable.resultFinalData, CAMiClsVariable.resultOKorNG, Main.configsn);//输出结果到excel   
                        }
                    }
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
                    if (!CAMiClsVariable.isProductOkNG)//产品NG时
                    {
                        Main.frmMain.ListBoxDisplay("产品NG");
                        Main.frmMain.PauseCheck();//暂停
                        Main.frmMain.ListBoxDisplay("流线2--Z轴运行到初始位置");
                        GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2InitialPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
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
                            if (CAMiClsVariable.isTrayChange)
                            {
                                DialogResult result = MessageBox.Show("Tray盘已满，请更换！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.Yes)
                                {
                                    CAMiClsVariable.isNGTrayOK = false;
                                    do//等待NG Tray 更换动作完成
                                    {
                                        Thread.Sleep(10);
                                        Application.DoEvents();
                                        Main.frmMain.PauseCheck();//暂停
                                    } while (!CAMiClsVariable.isNGTrayOK);
                                    CAMiClsVariable.isTrayChange = false;
                                }
                                else
                                {
                                    CAMiClsVariable.isTrayChange = true;
                                }
                            }
                            for (int i = 0; i < 12; i++)//每个穴位判断
                            {
                                if (CAMiClsVariable.isProductSelect[i] == 1)
                                {
                                    if (CAMiClsVariable.productResult[i] != 1)
                                    {
                                        if (CAMiClsMethod.TrayCheck(i) == 1)//判断Tray是否可以放料
                                        {
                                            Main.frmMain.ListBoxDisplay("流线2--CCD2 Y轴运行到抛料位置");
                                            GClsMontion.AbsoluteMove(3, CAMiClsVariable.CCD2TossingPosYValue[0], CAMiClsVariable.speedCCD2Y, CAMiClsVariable.TaccCCD2Y);
                                            GClsMontion.WaitMotorStop(3);
                                            Main.frmMain.ListBoxDisplay("流线2--Z轴运行到抛料位置");
                                            GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2flingPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                                            GClsMontion.WaitMotorStop(4);
                                            Main.frmMain.PauseCheck();//暂停
                                            Main.frmMain.ListBoxDisplay("流线2--关闭抛料穴位真空吸");//一个一个抛，关闭NG产品对应真空吸
                                            CAMiClsCylinder.GeneralClynderMotion(2, 0, i, 0);//真空吸Off
                                            CAMiClsCylinder.GeneralClynderMotion(2, 0, i + 12, 1);//去真空On
                                            CAMiClsCylinder.GeneralClynderMotionSensor(2, 0, i, 0, 3000, 44 + (i + 1) * 2);
                                            CAMiClsCylinder.GeneralClynderMotion(2, 0, i + 12, 0);//去真空Off
                                            //if (i < 8)//--------------带小气缸机台部分
                                            //{
                                            //    CAMiClsCylinder.GeneralClynderMotion(2, 0, i + 24, 0);//小气缸
                                            //    CAMiClsCylinder.GeneralClynderMotionSensor(2, 0, i + 12, 0, 3000, 44 + (i + 12 + 1) * 2);//小气缸感应off
                                            //}
                                            //if (i == 8) CAMiClsCylinder.CldSmallControl9Retract();
                                            //if (i == 9) CAMiClsCylinder.CldSmallControl10Retract();
                                            //if (i == 10) CAMiClsCylinder.CldSmallControl11Retract();
                                            //if (i == 11) CAMiClsCylinder.CldSmallControl12Retract();
                                            Thread.Sleep(300);
                                            CAMiClsVariable.productResult[i] = 1;
                                        }
                                        else
                                        {
                                            CAMiClsVariable.isTrayChange = true;
                                        }
                                    }
                                }
                            }
                        } while (CAMiClsVariable.isTrayChange);
                    }
                    #endregion
                    Main.frmMain.ListBoxDisplay("流线2--Z轴运行到初始位置");
                    GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2InitialPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                    CAMiClsVariable.isNGTrayOK = false;
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--CCD2 Y轴运行到抓料位置");
                    GClsMontion.AbsoluteMove(3, CAMiClsVariable.CCD2PickPosY, CAMiClsVariable.speedCCD2Y, CAMiClsVariable.TaccCCD2Y);
                    GClsMontion.AbsoluteMove(2, CAMiClsVariable.CCD2InitialPosX, CAMiClsVariable.speedCCD2X, CAMiClsVariable.TaccCCD2X);
                    GClsMontion.WaitMotorStop(3);
                    Thread.Sleep(100);
                    Main.frmMain.ListBoxDisplay("流线2--Z轴运行到抓料高度");
                    GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2PickPosZ - 10 * 1000, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--关闭所有真空吸及小气缸缩回");
                    for (int i = 0; i < 12; i++)//关闭所有真空吸
                    {
                        CAMiClsCylinder.GeneralClynderMotion(2, 0, i, 0);
                        CAMiClsCylinder.GeneralClynderMotion(2, 0, i + 12, 0);
                        CAMiClsCylinder.GeneralClynderMotionSensor(2, 0, i, 0, 3000, 44 + (i + 1) * 2);
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
                    Thread.Sleep(100);
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--Z轴运行到初始位置");
                    GClsMontion.AbsoluteMove(4, CAMiClsVariable.CCD2InitialPosZ, CAMiClsVariable.speedCCD2Z, CAMiClsVariable.TaccCCD2Z);
                    GClsMontion.WaitMotorStop(4);
                    GClsMontion.WaitMotorStop(2);
                    Main.frmMain.PauseCheck();//暂停
                    CAMiClsMethod.StreamTrayRelease(2);//流线2产品放行                          
                    Main.frmMain.ListBoxDisplay("流线2--启动流线2");
                    GClsMontion.WriteCardExtendOutputBit(1, 8, 0);//流线2高速                   
                    GClsMontion.WriteCardExtendOutputBit(1, 6, 1);//流线2启动
                    Main.frmMain.PauseCheck();//暂停
                    Main.frmMain.ListBoxDisplay("流线2--等待流线2产品流出");
                    do//等待到位1感应
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        Main.frmMain.PauseCheck();//暂停
                    } while (GClsMontion.ReadCardExtendInputBit(1, 10) == 1 || GClsMontion.ReadCardExtendInputBit(1, 11) == 1 ||
                             GClsMontion.ReadCardExtendInputBit(1, 12) == 1);
                    Thread.Sleep(3000);//临时空跑延时
                    Main.frmMain.ListBoxDisplay("流线2--停止流线2");
                    GClsMontion.WriteCardExtendOutputBit(1, 6, 0);//流线2停止
                    Thread.Sleep(20);//气缸连续动作增加间隔延时
                    Main.frmMain.ListBoxDisplay("流线2--流线2阻挡气缸伸出");
                    CAMiClsCylinder.StreamLine2StopExtend();//阻挡气缸伸出                    
                    Main.frmMain.PauseCheck();//暂停
                    CAMiClsVariable.isStreamLine2Ready = true;
                    Main.frmMain.PauseCheck();//暂停
                }
                catch (Exception ex)
                {
                    myEmptyMethod.LogRecord(ex.ToString());
                }
            }
        }
        public void ThNGStreamLineEmptyRunning()//NG Tray 
        {
            //while (true)
            //{
            //    try
            //    {
            //        Main.frmMain.ListBoxDisplay("NG--NG Tray上料");
            //        CAMiClsMethod.NGTrayLoad();
            //        Main.frmMain.ListBoxDisplay("NG--NG Tray上料完成");
            //        CAMiClsVariable.isReplaceTray = false;//是否更换Tray
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
        }
    }
}
