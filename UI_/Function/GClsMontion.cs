using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Justech
{ 
    class GClsMontion
    {      
        #region 运动部分                                              */
        public static void WaitMotorStop(int axisNo)// 等待马达停止
        {
            while (MC1104.jmc_mc1104_motion_done(axisNo) != 0)
            {
                Thread.Sleep(100);
                Application.DoEvents();
            }
        }
        public static void CheckPara(double speed, double Tacc)//判断速度和加速度的值是否为0
        {
            if (Tacc == 0) Tacc = 0.1;
            if (speed == 0) speed = 1000;
        }
        public static void AbsoluteMove(int axisNo, double distance, double speed, double Tacc)//绝对移动
        {
            CheckPara(speed, Tacc);
            //MC1104.jmc_mc1104_start_sa_move(axisNo, distance, 10, speed, acc, acc, 2, 2);
            MC1104.jmc_mc1104_start_ta_move(axisNo, distance, 10, speed, Tacc, Tacc);
        }
        public static void RelativeMove(int axisNo, double distance, double speed, double Tacc)//相对移动
        {
            CheckPara(speed, Tacc);
            //MC1104.jmc_mc1104_start_tr_move(axisNo, distance,10, speed, Tacc, Tacc);
            MC1104.jmc_mc1104_start_sr_move(axisNo, distance, 10, speed, Tacc, Tacc, 2, 2);
        }
        public static void ContinueMove(int axisNo, double speed, double Tacc)//连续运动，根据速度的符号来判定运动方向
        {
            CheckPara(speed, Tacc);
            MC1104.jmc_mc1104_sv_move(axisNo, 10, speed, Tacc, 2);
        }
        public static void StopAxis(int axisNo,double Tacc)//停止轴动
        {
            MC1104.jmc_mc1104_sd_stop(axisNo, Tacc);
        }
        public static void GoHome(int axisNo, double speed, double Tacc)//回原点
        {
            CheckPara(speed, Tacc);
            MC1104.jmc_mc1104_home_search(axisNo, 0, -speed, Tacc, 1000.0);//orgoffset不能为0           
            //WaitMotorStop(axisNo);
            //int montionIoStatus;//运动轴的IO状态
            //do
            //{
            //    MC1104.jmc_mc1104_get_io_status(axisNo, out montionIoStatus);
            //    Thread.Sleep(100);
            //    Application.DoEvents();
            //} while (((montionIoStatus & 16) != 16));
        }
        public static void GoHomeJudge(int axisNo)
        {
            WaitMotorStop(axisNo);
            int montionIoStatus;//运动轴的IO状态
            do
            {
                MC1104.jmc_mc1104_get_io_status(axisNo, out montionIoStatus);
                Thread.Sleep(100);
                Application.DoEvents();
            } while (((montionIoStatus & 16) != 16));
            MC1104.jmc_mc1104_set_position(axisNo, 0);
        }
        #endregion
        #region 运动控制卡IO读写                                      */       
        public static void CardInitial() //初始化运动控制卡
        {
            int rtn = 0;
            //int rtn1 = 0;
            int CardID_InBit = 0;
            //int CardID_InBit1 = 0;
            rtn = MC1104.jmc_mc1104_initial(0, out CardID_InBit);			//初始化完成后card_in_sys=1[bit0=1]
            //rtn1 = MC1104.jmc_mc1104_initial(1, out CardID_InBit1);			//初始化完成后card_in_sys=1[bit0=1]
            if (rtn != 0 )//&& rtn1 != 0
            {
                MessageBox.Show("没有找到MC1104板卡，请检测后重试！");
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    MC1104.jmc_mc1104_set_alm(i, 1, 0);         //报警信号逻辑，报警停止模式     
                    MC1104.jmc_mc1104_set_el_logic(i, 1);		//设置EL逻辑
                    MC1104.jmc_mc1104_set_org(i, 0);			//设置ORG逻辑
                    MC1104.jmc_mc1104_set_fa_speed(i, 2000);       //搜索原点低速度                    
                    MC1104.jmc_mc1104_set_inp(i, 1, 1);			//设定INP信号

                    MC1104.jmc_mc1104_set_pls_outmode(i, 2);      //脉冲输出模式                   
                    MC1104.jmc_mc1104_set_home_config(i, 1, 0, 0, 0);//设置回原点参数
                    MC1104.jmc_mc1104_set_servo(i, 1);            //伺服激磁
                }
            }
            MC1104.jmc_mc1104_disable_log();//关闭log功能
        }
        public static void CardClose()//释放轴卡
        {
            MC1104.jmc_mc1104_close();
        }
        public static double GetCurrentPosition(int axisNo) //读取当前轴位置
        {
            double positionAxis = 0;//各轴反馈的脉冲值
            //MC1104.jmc_mc1104_get_command(axisNo, out positionAxis);
            MC1104.jmc_mc1104_get_position(axisNo, out positionAxis);
            return positionAxis;
        }
        public static void ServoOn(int axisNo, int onOff)//伺服激磁
        {
            MC1104.jmc_mc1104_set_servo(axisNo, onOff);
        }
        public static int ReadCardInputBit(int cardNo, int cardBitInputNo)//读4路输入点位的状态
        {
            int cardDiValue = 0;
            bool cardInputBitStatus;
            int result;
            MC1104.jmc_mc1104_get_multi_DI(cardNo, out cardDiValue);
            cardInputBitStatus = (cardDiValue & (1 << cardBitInputNo)) != 0;
            result = cardInputBitStatus ? 1 : 0;
            return result;
        }
        public static int ReadCardExtendInputBit(int cardNo, int cardExtendBitInputNo)//读通用16路输入点位状态
        {          
            int diValue = 0;
            bool inputBitStatus;
            int result;
            MC1104.jmc_mc1104_get_GDI(cardNo, out diValue);
            inputBitStatus = (diValue & (1 << cardExtendBitInputNo)) != 0;
            //inputBitStatus = ((diValue >> cardExtendBitInputNo) & 1) != 0;
            result = inputBitStatus ? 1 : 0;//返回当前点位电平状态，0或1
            return result;
        }
        public static int ReadCardOutputBit(int cardNo, int cardBitOutputNo)//读4路输出点位状态
        {
            int cardDoValue = 0;
            bool cardDoStatus;
            int result;
            MC1104.jmc_mc1104_get_multi_DO(cardNo, out cardDoValue);
            cardDoStatus = (cardDoValue & (1 << cardBitOutputNo)) != 0;
            result = cardDoStatus ? 1 : 0;//返回当前点位电平状态，0或1
            return result;
        }
        public static int ReadCardExtendOutputBit(int cardNo, int cardExtendBitOutputNo)//读通用16路输出点位状态
        {
            int doValue = 0;
            bool outputStatus;
            int result;
            MC1104.jmc_mc1104_get_GDO(cardNo, out doValue);
            outputStatus = (doValue & (1 << cardExtendBitOutputNo)) != 0;
            result = outputStatus ? 1 : 0;//返回当前点位电平状态，0或1
            return result;
        }
        public static void WaitCardInputBit(int cardNo, int cardBitInputNo, int onOff,int waitTime,int errorNo)//等待4路输入点为OnOff
        {
            int i = 0;
            do
            {
                Thread.Sleep(10);
                i++;
                Application.DoEvents();
                if (i > (waitTime / 10))
                {
                    CAMiClsVariable.errorCodeNo = errorNo;//将errorCode的NO号给出
                    i = 0;
                }
            } while (ReadCardInputBit(cardNo,cardBitInputNo) != onOff);
        }
        public static void WaitCardExtendInputBit(int cardNo, int cardExtendBitInputNo, int onOff,int waitTime,int errorNo)//等待16路通用输入点为onOff
        {
            int i = 0;
            do
            {              
                Thread.Sleep(10);
                i++;
                Application.DoEvents();
                if (i > (waitTime / 10) )
                {
                    CAMiClsVariable.errorCodeNo = errorNo;//将errorCode的NO号给出
                    i = 0;
                }
            } while (ReadCardExtendInputBit(cardNo,cardExtendBitInputNo) != onOff);
        }
        public static void WriteCardOutputBit(int cardNo, int cardBitDoNo, int status)//4路输出Set 或 Reset
        {          
            int cardDoCurrentValue = 0;
            int doValue = 0;
            MC1104.jmc_mc1104_get_multi_DO(cardNo, out cardDoCurrentValue);
            if (status == 1)//Set
            {
                doValue = cardDoCurrentValue | (1 << cardBitDoNo);
            }
            else if (status == 0)//Reset
            {
                doValue = cardDoCurrentValue & (15 - (1 << cardBitDoNo));
            }
            MC1104.jmc_mc1104_set_multi_DO(cardNo, doValue);                                         
        }

        public static object lockWriteCardExtendOutputBit = new object();
        public static void WriteCardExtendOutputBit(int cardNo, int cardExtendBitOutputNo, int status)//通用16路输出Set or Reset
        {
            lock (lockWriteCardExtendOutputBit)
            { 
            int doCurrentValue = 0;
            int doValue = 0;
            MC1104.jmc_mc1104_get_GDO(cardNo, out doCurrentValue);
            if (status == 1)//Set
            {
                doValue = doCurrentValue | (1 << cardExtendBitOutputNo);
            }
            else if (status == 0)//Reset
            {
                doValue = doCurrentValue & (65535 - (1 << cardExtendBitOutputNo));
            }
            MC1104.jmc_mc1104_set_GDO(cardNo, doValue);
            }
        }
        #endregion
        #region 凌华7432卡读写IO 
        public static void IOCard7432Initial() //初始化IO扩展卡
        {
            //ushort _cardType = 0;
            //for (ushort i = 0; i < 16; i++)
            //{
            //    //在下拉框中添加 systemID
            //    if (DASK.Register_Card(DASK.PCI_7432, i) >= 0)
            //    {
            //        //cmbCardSystemId.Items.Add(i);
            //        _cardType = DASK.PCI_7432;
            //    }
            //    else
            //    {
            //        MessageBox.Show("7432卡初始化失败！");
            //    }
            //}
            //_cardRegId = DASK.Register_Card(_cardType, Convert.ToUInt16(cmbCardSystemId.SelectedItem)); //实列化卡片
            //short  _cardRegId = DASK.Register_Card(16, 0); //实列化卡片
            //if (_cardRegId < 0)
            //{
            //    MessageBox.Show("7432卡初始化失败！");
            //}
             ushort _cardType=0;
             //short cardRegId; //全局引用
            for (ushort i = 0; i < 16; i++)
            {
                //在下拉框中添加 systemID
                if (DASK.Register_Card(DASK.PCI_7432, i) >= 0)
                {
                    //cmbCardSystemId.Items.Add(i);
                    _cardType = DASK.PCI_7432;
                }
            }
           CAMiClsVariable.cardRegId = DASK.Register_Card(_cardType, 0); //实列化卡片
        }
        public static void IOCard7432Close()//释放IO卡
        {
            short ret;
            short m_dev = DASK.Register_Card(DASK.PCI_7432, 0);
            if (m_dev >= 0)
            {
                ret = DASK.Release_Card((ushort)m_dev);
            }
        }
        public static int ReadIOCard7432InputBit(ushort cardID, int cardInputBit)//读扩展IO卡输入点
        {        
             if(CAMiClsVariable.strIOCard == "7432")
             {
                 uint diValue = 0;
                 bool inputBitStatus;
                 int result;
                 DASK.DI_ReadPort((ushort)CAMiClsVariable.cardRegId, 0, out diValue);
                 inputBitStatus = (diValue & (1 << cardInputBit)) != 0;
                 result = inputBitStatus ? 1 : 0;
                 return result;
             }
             else//APE IO卡
             {
                 uint diValue = 0;
                 bool inputBitStatus;
                 int result;
                 DAQ_I32.DAQ_Mul_GDI_Get(DAQ_I32.DAQ_TYPE.DAQ_D3232,(int)cardID, ref diValue);
                 inputBitStatus = (diValue & (1 << cardInputBit)) != 0;
                 result = inputBitStatus ? 1 : 0;//返回当前点位电平状态，0或1
                 return result;
             }          
        }
        public static int ReadIOCard7432OutputBit(ushort cardID, int cardOutputBit)//读扩展IO卡输出点 
        {
            if(CAMiClsVariable.strIOCard == "7432")
            {
                uint doValue = 0;
                bool outputBitStatus; ;
                int result;
                DASK.DO_ReadPort((ushort)CAMiClsVariable.cardRegId, 0, out doValue);
                outputBitStatus = (doValue & (1 << cardOutputBit)) != 0;
                result = outputBitStatus ? 1 : 0;
                return result; 
            }
            else//APE IO卡
            {
                uint doValue = 0;
                bool outputStatus;
                int result;
                DAQ_I32.DAQ_Mul_GDO_Get(DAQ_I32.DAQ_TYPE.DAQ_D3232, (int)cardID, ref doValue);
                outputStatus = (doValue & (1 << cardOutputBit)) != 0;
                result = outputStatus ? 1 : 0;//返回当前点位电平状态，0或1
                return result;
            }
        }
        public static void WaitIOCard7432InputBit(ushort cardID, int cardInputBit, int onOff, int waitTime, int errorNo)
        {
            int i = 0;
            if(CAMiClsVariable.strIOCard == "7432")
            {
                do
                {
                    Thread.Sleep(10);
                    i++;
                    Application.DoEvents();
                    if (i > (waitTime / 10))
                    {
                        CAMiClsVariable.errorCodeNo = errorNo;//将errorCode的NO号给出
                        i = 0;
                    }
                } while (ReadIOCard7432InputBit((ushort)CAMiClsVariable.cardRegId, cardInputBit) != onOff);
            }
            else//APE IO卡
            {
                do
                {
                    Thread.Sleep(10);
                    i++;
                    Application.DoEvents();
                    if (i > (waitTime / 10))
                    {
                        CAMiClsVariable.errorCodeNo = errorNo;//将errorCode的NO号给出
                        i = 0;
                    }
                } while (ReadAPEIOCardInputBit((int)cardID, cardInputBit) != onOff);
            }           
        }
        public static void WriteIOCard7432OutputBit(ushort cardID, ushort cardDoBit, ushort status)
        {
            if(CAMiClsVariable.strIOCard == "7432")
            {
                DASK.DO_WriteLine((ushort)CAMiClsVariable.cardRegId, 0, cardDoBit, status);
            }
            else
            {
                uint doCurrentValue = 0;
                uint doValue = 0;
                DAQ_I32.DAQ_Mul_GDO_Get(DAQ_I32.DAQ_TYPE.DAQ_D3232,(int)cardID, ref doCurrentValue);
                if (status == 1)//Set
                {
                    doValue = (uint)(doCurrentValue |  (uint)(1 << cardDoBit));
                }
                else if (status == 0)//Reset
                {
                    doValue = (uint)(doCurrentValue & (4294967295 - (1 << cardDoBit)));
                }
                DAQ_I32.DAQ_Mul_GDO_Set(DAQ_I32.DAQ_TYPE.DAQ_D3232,(int)cardID, doValue);      
            }
        }
        #endregion
        #region APE IO卡IO读写*/
        public static int aPECardIOID;
        public static void APEIOCardInitial() //初始化IO扩展卡
        {
            int rtn = -1;
            uint[] cardIOID = new UInt32[3];//卡号为0        
            rtn = DAQ_I32.DAQ_Mul_Sys_Initial(cardIOID);
            aPECardIOID = 4;
            if (rtn != 0)
            {
                MessageBox.Show("没有找到IO卡，请检测后重试！");
            }
            else
            {
                //DAQ_I32.DAQ_GDO_Cfg(cardIOID, 0xFFFFFF);//DO正逻辑
                DAQ_I32.DAQ_Mul_GDO_Cfg(DAQ_I32.DAQ_TYPE.DAQ_D3232, 0, 0x0);//DO负逻辑
                //DAQ_I32.DAQ_GDI_Cfg(cardIOID, 0xFFFFFF);//DI正逻辑
                DAQ_I32.DAQ_Mul_GDI_Cfg(DAQ_I32.DAQ_TYPE.DAQ_D3232, 0, 0);//DI负逻辑
            }
        }
        public static void APEIOCardClose()//释放IO卡
        {
            DAQ_I32.DAQ_Sys_Terminate();  
        }
        public static int ReadAPEIOCardInputBit(int cardNo, int cardInputBit)//读扩展IO卡输入点
        {
            uint diValue = 0;
            bool inputBitStatus;
            int result;
            DAQ_I32.DAQ_Mul_GDI_Get(DAQ_I32.DAQ_TYPE.DAQ_D3232,cardNo, ref diValue);
            inputBitStatus = (diValue & (1 << cardInputBit)) != 0;
            result = inputBitStatus ? 1 : 0;//返回当前点位电平状态，0或1
            return result;
        }
        public static int ReadAPEIOCardOutputBit(int cardNo,int cardOutputBit)//读扩展IO卡输出点 
        { 
            uint doValue = 0;
            bool outputStatus;
            int result;
            DAQ_I32.DAQ_Mul_GDO_Get(DAQ_I32.DAQ_TYPE.DAQ_D3232,cardNo, ref doValue);
            outputStatus = (doValue & (1 << cardOutputBit)) != 0;
            result = outputStatus ? 1 : 0;//返回当前点位电平状态，0或1
            return result;
        }
        public static void WaitAPEIOCardInputBit(int cardNo, int cardInputBit, int onOff, int waitTime, int errorNo)//等待32路通用输入点为onOff
        {            
            int i = 0;
            do
            {
                Thread.Sleep(10);
                i++;
                Application.DoEvents();
                if (i > (waitTime / 10))
                {
                    CAMiClsVariable.errorCodeNo = errorNo;//将errorCode的NO号给出
                    i = 0;
                }
            } while (ReadAPEIOCardInputBit(cardNo, cardInputBit) != onOff);
        }
        public static void WriteAPEIOCardOutputBit(int cardNo,int cardDoBit,int status)//扩展IO卡输出点Set or Reset
        {          
            uint doCurrentValue = 0;
            uint doValue = 0;
            DAQ_I32.DAQ_Mul_GDO_Get(DAQ_I32.DAQ_TYPE.DAQ_D3232,cardNo, ref doCurrentValue);
            if (status == 1)//Set
            {
                doValue = (uint)(doCurrentValue | (uint)(1 << cardDoBit));
            }
            else if (status == 0)//Reset
            {
                doValue = (uint)(doCurrentValue & (4294967295 - (1 << cardDoBit)));
            }
            DAQ_I32.DAQ_Mul_GDO_Set(DAQ_I32.DAQ_TYPE.DAQ_D3232, cardNo, doValue);           
        }
        #endregion
        #region 其它                                                  */        
        public static void Buzzer(int buzzerBit)//蜂鸣器响,蜂鸣器点位在卡的16路输出点中选取
        {
            bool buzzer = true;
            int countBuzzer = 0;
            while (buzzer)
            {
                if (ReadCardExtendOutputBit(0, buzzerBit) == 0)
                {
                    WriteCardExtendOutputBit(0, buzzerBit, 1);
                }
                else
                {
                    WriteCardExtendOutputBit(0, buzzerBit, 0);
                }
                Thread.Sleep(1000);
                countBuzzer++;
                if (countBuzzer >= 6)
                {
                    buzzer = false;
                    WriteCardExtendOutputBit(0, buzzerBit, 0);
                }
            }
        }
        public static void TricolourLight(int redBit, int greenBit, int yellowBit, int redStatus, int greenStatus, int yellowStatus, int cardId)  //三色灯亮，点位在卡的16路输出点中选取
        {
            WriteCardExtendOutputBit(cardId, redBit, redStatus);   //红灯
            WriteCardExtendOutputBit(cardId, greenBit, greenStatus); //绿灯
            WriteCardExtendOutputBit(cardId, yellowBit, yellowStatus);//黄灯           
        }
        public static void TricolourLightTwinkle(int redBit, int greenBit, int yellowBit, string colorLight,int cardId) //三色灯闪烁,点位在卡的16路输出点中选取
        {
            Thread.Sleep(500);
            switch (colorLight)
            {
                case "green":
                    WriteCardExtendOutputBit(cardId,yellowBit, 0);//黄灯灭
                    WriteCardExtendOutputBit(cardId,redBit, 0);//红灯灭
                    if (ReadCardExtendOutputBit(cardId, greenBit) == 0) //绿灯闪烁
                    {
                        Thread.Sleep(500);
                        WriteCardExtendOutputBit(cardId, greenBit, 1);
                    }
                    else
                    {
                        Thread.Sleep(500);
                        WriteCardExtendOutputBit(cardId, greenBit, 0);
                    }
                    break;
                case "yellow":
                    WriteCardExtendOutputBit(cardId,greenBit, 0);//绿灯灭
                    WriteCardExtendOutputBit(cardId,redBit, 0);//红灯灭
                    if (ReadCardExtendOutputBit(cardId, yellowBit) == 0) //黄灯闪烁
                    {
                        Thread.Sleep(500);
                        WriteCardExtendOutputBit(cardId, yellowBit, 1);
                    }
                    else
                    {
                        Thread.Sleep(500);
                        WriteCardExtendOutputBit(cardId, yellowBit, 0);
                    }
                    break;
                case "red":
                    WriteCardExtendOutputBit(cardId,greenBit, 0);//绿灯灭
                    WriteCardExtendOutputBit(cardId,yellowBit, 0);//黄灯灭
                    if (ReadCardExtendOutputBit(cardId, redBit) == 0) //红灯闪烁
                    {
                        Thread.Sleep(500);
                        WriteCardExtendOutputBit(cardId, redBit, 1);
                    }
                    else
                    {
                        Thread.Sleep(500);
                        WriteCardExtendOutputBit(cardId, redBit, 0);
                    }
                    break;
            }
        }
        #endregion
    }
}
