using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justech;
using System.Windows.Forms;
using System.Threading;

namespace Justech
{
    class CAMiClsCylinder
    {
        #region 控制气缸动作
        /// <summary>
        /// 控制气缸动作
        /// </summary>
        /// <param name="type">0：轴卡上的4输出通道；1：轴卡扩展的16输出通道；2:7432 IO卡输出</param>
        /// <param name="cardNo">代表第几张卡，从0开始</param>
        /// <param name="pointNo">代表第几个点位，从0开始</param>
        /// <param name="onOff">输出状态，0：Off；1：On</param>
        public static void GeneralClynderMotion(int type ,int cardNo ,int pointNo,int onOff)//阻挡气缸伸出
        {
           try
           { 
            switch (type)
            {
                case 0:
                     GClsMontion.WriteCardOutputBit(cardNo, pointNo, onOff);//轴卡上的4输出通道
                    break;
                case 1:
                    GClsMontion.WriteCardExtendOutputBit(cardNo, pointNo, onOff);//轴卡扩展的16输出通道
                    break;
                case 2:
                    ushort uCardNo = (ushort)cardNo;
                    ushort uPointNo = (ushort)pointNo;
                    ushort uOnOff = (ushort)onOff;
                    if(CAMiClsVariable.strIOCard=="7432")
                    {
                        GClsMontion.WriteIOCard7432OutputBit(uCardNo, uPointNo, uOnOff);//7432 IO卡输出
                    }
                    else
                    {
                        GClsMontion.WriteAPEIOCardOutputBit(cardNo, pointNo, onOff);//APE IO卡输出
                    }
                    break;
                default :
                    break;
            } 
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
           }
        }
        #endregion

        #region 等待气缸动作感应
        /// <summary> 等待气缸动作感应
        /// 等待气缸动作感应
        /// </summary>
        /// <param name="type">0：轴卡上的4输入通道；1：轴卡扩展的16输入通道；2:7432 IO卡输入</param>
        /// <param name="cardNo">代表第几张卡，从0开始</param>
        /// <param name="pointNo">代表第几个点位，从0开始</param>
        /// <param name="onOff">输出状态，0：Off；1：On</param>
        /// <param name="waitTime">等待输入时间，单位ms</param>
        /// <param name="errorNo">errorcode配置文件的ID</param>
        public static void GeneralClynderMotionSensor(int type, int cardNo, int pointNo, int onOff, int waitTime, int errorNo)//阻挡气缸伸出
        {
            try 
            {
             switch (type )
             {
                case 0:
                    GClsMontion.WaitCardInputBit(cardNo, pointNo, onOff,waitTime,errorNo);//轴卡上的4输入通道
                    break;
                case 1:
                    GClsMontion.WaitCardExtendInputBit(cardNo, pointNo, onOff,waitTime,errorNo);//轴卡扩展的16输入通道
                    break;
                case 2:
                    ushort uCardNo = (ushort)cardNo;
                    ushort uPointNo = (ushort)pointNo;
                    ushort uOnOff = (ushort)onOff;
                    GClsMontion.WaitIOCard7432InputBit(uCardNo, uPointNo, uOnOff,waitTime,errorNo);//7432 IO卡输入
                    break;
                default:
                      break ;
             }
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region  流线1阻挡气缸动作

        public static void StreamLine1StopExtend()//流线1阻挡气缸伸出
        {
            GClsMontion.WriteCardExtendOutputBit(0, 0, 0);
            GClsMontion.WaitCardExtendInputBit(0, 0, 1,3000,1);//流线1阻挡气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(0, 1, 0,3000,3);//流线1阻挡气缸缩回感应
        }

        public static void StreamLine1StopRetract()//流线1阻挡气缸缩回
        {
            GClsMontion.WriteCardExtendOutputBit(0, 0, 1);
            GClsMontion.WaitCardExtendInputBit(0, 0, 0, 3000, 2);//流线1阻挡气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(0, 1, 1, 3000, 4);//流线1阻挡气缸缩回感应
        }
        #endregion

        #region  流线1顶升气缸动作

        public static void StreamLine1LiftingUp()//流线1顶升气缸伸出
        {
            GClsMontion.WriteCardExtendOutputBit(0, 1, 1);//流线1顶升气缸伸出
            GClsMontion.WriteCardExtendOutputBit(0, 2, 0);////流线1顶升气缸缩回
            GClsMontion.WaitCardExtendInputBit(0, 2, 1, 3000, 5);//流线1顶升气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(0, 3, 0, 3000, 7);//流线1顶升气缸缩回感应
        }

        public static void StreamLine1LiftingDown()//流线1顶升气缸缩回
        {
            GClsMontion.WriteCardExtendOutputBit(0, 1, 0);//流线1顶升气缸伸出
            GClsMontion.WriteCardExtendOutputBit(0, 2, 1);////流线1顶升气缸缩回
            GClsMontion.WaitCardExtendInputBit(0, 2, 0, 3000, 6);//流线1顶升气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(0, 3, 1, 3000, 8);//流线1顶升气缸缩回感应
        }
        #endregion

        #region  流线1夹紧1气缸动作

        public static void StreamLine1Clamp1Extend()//流线1夹紧1气缸伸出
        {
            GClsMontion.WriteCardExtendOutputBit(0, 3, 1);//流线1夹紧1气缸伸出
            GClsMontion.WaitCardExtendInputBit(0, 4, 1, 3000, 9);//流线1夹紧1气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(0, 5, 0, 3000, 11);//流线1夹紧1气缸缩回感应
        }

        public static void StreamLine1Clamp1Retract()//流线1夹紧1气缸缩回
        {
            GClsMontion.WriteCardExtendOutputBit(0, 3, 0);//流线1夹紧1气缸缩回
            GClsMontion.WaitCardExtendInputBit(0, 4, 0, 3000, 10);//流线1夹紧1气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(0, 5, 1, 3000, 12);//流线1夹紧1气缸缩回感应
        }
        #endregion

        #region  流线1夹紧2气缸动作

        public static void StreamLine1Clamp2Extend()//流线1夹紧2气缸伸出
        {
            GClsMontion.WriteCardExtendOutputBit(0, 4, 1);//流线1夹紧2气缸伸出
            GClsMontion.WaitCardExtendInputBit(0, 8, 1, 3000, 13);//流线1夹紧2气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(0, 9, 0, 3000, 15);//流线1夹紧2气缸缩回感应
        }

        public static void StreamLine1Clamp2Retract()//流线1夹紧2气缸缩回
        {
            GClsMontion.WriteCardExtendOutputBit(0, 4, 0);//流线1夹紧2气缸缩回
            GClsMontion.WaitCardExtendInputBit(0, 8, 0, 3000, 14);//流线1夹紧2气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(0, 9, 1, 3000, 16);//流线1夹紧2气缸缩回感应
        }
        #endregion       

        #region  流线1吸真空动作

        public static void StreamLine1VacuumOn()//流线1吸真空
        {
            GClsMontion.WriteCardExtendOutputBit(0, 10, 1);
            GClsMontion.WriteCardExtendOutputBit(0, 11, 0);
            //GClsMontion.WaitCardExtendInputBit(0, 15, 1, 3000, 21);//流线1夹紧2气缸缩回感应
        }

        public static void StreamLine1VacuumOff()//流线1破真空
        {
            GClsMontion.WriteCardExtendOutputBit(0, 10, 0);//流线1吸真空
            GClsMontion.WriteCardExtendOutputBit(0, 11, 1);//流线1破真空
            GClsMontion.WaitCardExtendInputBit(0, 15, 0, 3000, 22);//流线1夹紧2气缸缩回感应
            GClsMontion.WriteCardExtendOutputBit(0, 11, 0);//流线1吸真空感应
        }
        #endregion

        #region  流线2阻挡气缸动作

        public static void StreamLine2StopExtend()//流线2阻挡气缸伸出
        {
            GClsMontion.WriteCardExtendOutputBit(1, 0, 0);//流线2阻挡气缸伸出
            GClsMontion.WaitCardExtendInputBit(1, 0, 1, 3000, 23);//流线2阻挡气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(1, 1, 0, 3000, 25);//流线2阻挡气缸缩回感应
        }

        public static void StreamLine2StopRetract()//流线2阻挡气缸缩回
        {
            GClsMontion.WriteCardExtendOutputBit(1, 0, 1);//流线2阻挡气缸伸出
            GClsMontion.WaitCardExtendInputBit(1, 0, 0, 3000, 24);//流线2阻挡气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(1, 1, 1, 3000, 26);//流线2阻挡气缸缩回感应
        }
        #endregion

        #region  流线2顶升气缸动作

        public static void StreamLine2LiftingUp()//流线2顶升气缸伸出
        {
            GClsMontion.WriteCardExtendOutputBit(1, 1, 1);//流线2顶升气缸伸出
            //Thread.Sleep(10);
            GClsMontion.WriteCardExtendOutputBit(1, 2, 0);//流线2顶升气缸缩回
            GClsMontion.WaitCardExtendInputBit(1, 2, 1, 3000, 27);//流线2顶升气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(1, 3, 0, 3000, 29);//流线2顶升气缸缩回感应
        }

        public static void StreamLine2LiftingDown()//流线2顶升气缸缩回
        {
            GClsMontion.WriteCardExtendOutputBit(1, 1, 0);//流线2顶升气缸伸出
            //Thread.Sleep(10);
            GClsMontion.WriteCardExtendOutputBit(1, 2, 1);//流线2顶升气缸缩回
            GClsMontion.WaitCardExtendInputBit(1, 2, 0, 3000, 28);//流线2顶升气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(1, 3, 1, 3000, 30);//流线2顶升气缸缩回感应
        }
        #endregion

        #region  流线2夹紧1气缸动作

        public static void StreamLine2Clamp1Extend()//流线2夹紧1气缸伸出
        {
            GClsMontion.WriteCardExtendOutputBit(1, 3, 1);//流线2夹紧1气缸伸出
            GClsMontion.WaitCardExtendInputBit(1, 4, 1, 3000, 31);//流线2夹紧1气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(1, 5, 0, 3000, 33);//流线2夹紧1气缸缩回感应
        }

        public static void StreamLine2Clamp1Retract()//流线2夹紧1气缸缩回
        {
            GClsMontion.WriteCardExtendOutputBit(1, 3, 0);//流线2夹紧1气缸缩回
            GClsMontion.WaitCardExtendInputBit(1, 4, 0, 3000, 32);//流线2夹紧1气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(1, 5, 1, 3000, 34);//流线2夹紧1气缸缩回感应
        }
        #endregion

        #region  流线2夹紧2气缸动作

        public static void StreamLine2Clamp2Extend()//流线2夹紧2气缸伸出
        {
            GClsMontion.WriteCardExtendOutputBit(1, 4, 1);//流线2夹紧2气缸伸出
            GClsMontion.WaitCardExtendInputBit(1, 8, 1, 3000, 35);//流线2夹紧2气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(1, 9, 0, 3000, 37);//流线2夹紧2气缸缩回感应
        }

        public static void StreamLine2Clamp2Retract()//流线2夹紧2气缸缩回
        {
            GClsMontion.WriteCardExtendOutputBit(1, 4, 0);//流线2夹紧2气缸缩回
            GClsMontion.WaitCardExtendInputBit(1, 8, 0, 3000, 36);//流线2夹紧2气缸伸出感应
            GClsMontion.WaitCardExtendInputBit(1, 9, 1, 3000, 38);//流线2夹紧2气缸缩回感应
        }
        #endregion       

        #region NG夹紧气缸动作
        public static void NGLocationClampExtend()//NG夹紧气缸夹紧
        {
            GClsMontion.WriteCardExtendOutputBit(0, 9, 1);//NG夹紧气缸夹紧ON
            GClsMontion.WaitCardExtendInputBit(0, 6, 1, 3000, 145);//
            GClsMontion.WaitCardExtendInputBit(0, 7, 0, 3000, 148);//
        }
        public static void NGlocationClampRetract()//NG夹紧气缸松开
        {
            GClsMontion.WriteCardExtendOutputBit(0, 9, 0);//NG夹紧气缸夹紧Off
            GClsMontion.WaitCardExtendInputBit(0, 6, 0, 3000, 146);//
            GClsMontion.WaitCardExtendInputBit(0, 7, 1, 3000, 147);//
        }
        #endregion
        #region  流线2吸真空动作

        public static void StreamLine2VacuumOn()//流线1吸真空
        {
            GClsMontion.WriteCardExtendOutputBit(1, 14, 1); //流线2吸真空
            GClsMontion.WriteCardExtendOutputBit(1, 15, 0);//流线2破真空
            //ClsMontion.WaitCardExtendInputBit(1, 15, 1,3000,43); ;//流线2吸真空感应
        }

        public static void StreamLine2VacuumOff()//流线2破真空
        {
            GClsMontion.WriteCardExtendOutputBit(1, 14, 0); //流线2吸真空
            GClsMontion.WriteCardExtendOutputBit(1, 15, 1);//流线2破真空
            GClsMontion.WaitCardExtendInputBit(1, 15, 0, 3000, 44); ;//流线2吸真空感应          
            GClsMontion.WriteCardExtendOutputBit(1, 15, 0);//流线2破真空
        }
        #endregion

        #region  吸真空1动作

        public static void CldVacuum1On()//吸真空1
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 0, 1); //吸真空1
            GClsMontion.WriteIOCard7432OutputBit(0, 12, 0);//破真空1
            GClsMontion.WaitIOCard7432InputBit(0, 0, 1,3000,45);//吸真空1感应
        }

        public static void CldVacuum1Off()//破真空1
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 0, 0); //吸真空1
            GClsMontion.WriteIOCard7432OutputBit(0, 12, 1);//破真空1
            GClsMontion.WaitIOCard7432InputBit(0, 0, 0, 3000, 46);//吸真空1感应
            GClsMontion.WriteIOCard7432OutputBit(0, 12, 0);//破真空1
        }
        #endregion

        #region  吸真空2动作

        public static void CldVacuum2On()//吸真空2
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 1, 1); //吸真空2
            GClsMontion.WriteIOCard7432OutputBit(0, 13, 0);//破真空2
            GClsMontion.WaitIOCard7432InputBit(0, 1, 1,3000,47);//吸真空2感应
        }

        public static void CldVacuum2Off()//破真空2
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 1, 0); //吸真空2
            GClsMontion.WriteIOCard7432OutputBit(0, 13, 1);//破真空2
            GClsMontion.WaitIOCard7432InputBit(0, 1, 0, 3000, 48);//吸真空2感应
            GClsMontion.WriteIOCard7432OutputBit(0, 13, 0);//破真空2
        }
        #endregion

        #region  吸真空3动作

        public static void CldVacuum3On()//吸真空3
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 2, 1); //吸真空3
            GClsMontion.WriteIOCard7432OutputBit(0, 14, 0);//破真空3
            GClsMontion.WaitIOCard7432InputBit(0, 2, 1,3000,49);//吸真空3感应
        }

        public static void CldVacuum3Off()//破真空3
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 2, 0); //吸真空3
            GClsMontion.WriteIOCard7432OutputBit(0, 14, 1);//破真空3
            GClsMontion.WaitIOCard7432InputBit(0, 2, 0, 3000, 50);//吸真空3感应
            GClsMontion.WriteIOCard7432OutputBit(0, 14, 0);//破真空3
        }
        #endregion

        #region  吸真空4动作

        public static void CldVacuum4On()//吸真空4
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 3, 1); //吸真空4
            GClsMontion.WriteIOCard7432OutputBit(0, 15, 0);//破真空4
            GClsMontion.WaitIOCard7432InputBit(0, 3, 1,3000,51);//吸真空4感应
        }

        public static void CldVacuum4Off()//破真空4
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 3, 0); //吸真空4
            GClsMontion.WriteIOCard7432OutputBit(0, 15, 1);//破真空4
            GClsMontion.WaitIOCard7432InputBit(0, 3, 0, 3000, 52);//吸真空4感应
            GClsMontion.WriteIOCard7432OutputBit(0, 15, 0);//破真空4
        }
        #endregion

        #region  吸真空5动作

        public static void CldVacuum5On()//吸真空5
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 4, 1); //吸真空5
            GClsMontion.WriteIOCard7432OutputBit(0, 16, 0);//破真空5
            GClsMontion.WaitIOCard7432InputBit(0, 4, 1,3000,53);//吸真空5感应
        }

        public static void CldVacuum5Off()//破真空5
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 4, 0); //吸真空5
            GClsMontion.WriteIOCard7432OutputBit(0, 16, 1);//破真空5
            GClsMontion.WaitIOCard7432InputBit(0, 4, 0, 3000, 54);//吸真空5感应
            GClsMontion.WriteIOCard7432OutputBit(0, 16, 0);//破真空5
        }
        #endregion

        #region  吸真空6动作

        public static void CldVacuum6On()//吸真空6
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 5, 1); //吸真空6
            GClsMontion.WriteIOCard7432OutputBit(0, 17, 0);//破真空6
            GClsMontion.WaitIOCard7432InputBit(0, 5, 1,3000,55);//吸真空6感应
        }

        public static void CldVacuum6Off()//破真空6
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 5, 0); //吸真空6
            GClsMontion.WriteIOCard7432OutputBit(0, 17, 1);//破真空6
            GClsMontion.WaitIOCard7432InputBit(0, 5, 0, 3000, 56);//吸真空6感应
            GClsMontion.WriteIOCard7432OutputBit(0, 17, 0);//破真空6
        }
        #endregion

        #region  吸真空7动作

        public static void CldVacuum7On()//吸真空7
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 6, 1); //吸真空7
            GClsMontion.WriteIOCard7432OutputBit(0, 18, 0);//破真空7
            GClsMontion.WaitIOCard7432InputBit(0, 6, 1,3000,57);//吸真空7感应
        }

        public static void CldVacuum7Off()//破真空7
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 6, 0); //吸真空7
            GClsMontion.WriteIOCard7432OutputBit(0, 18, 1);//破真空7
            GClsMontion.WaitIOCard7432InputBit(0, 6, 0, 3000, 58);//吸真空7感应
            GClsMontion.WriteIOCard7432OutputBit(0, 18, 0);//破真空7
        }
        #endregion

        #region  吸真空8动作

        public static void CldVacuum8On()//吸真空8
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 7, 1); //吸真空8
            GClsMontion.WriteIOCard7432OutputBit(0, 19, 0);//破真空8
            GClsMontion.WaitIOCard7432InputBit(0, 7, 1,3000,59);//吸真空8感应
        }

        public static void CldVacuum8Off()//破真空8
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 7, 0); //吸真空8
            GClsMontion.WriteIOCard7432OutputBit(0, 19, 1);//破真空8
            GClsMontion.WaitIOCard7432InputBit(0, 7, 0, 3000, 60);//吸真空8感应
            GClsMontion.WriteIOCard7432OutputBit(0, 19, 0);//破真空8
        }
        #endregion

        #region  吸真空9动作

        public static void CldVacuum9On()//吸真空9
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 8, 1); //吸真空9
            GClsMontion.WriteIOCard7432OutputBit(0, 20, 0);//破真空9
            GClsMontion.WaitIOCard7432InputBit(0, 8, 1,3000,61);//吸真空9感应
        }

        public static void CldVacuum9Off()//破真空9
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 8, 0); //吸真空9
            GClsMontion.WriteIOCard7432OutputBit(0, 20, 1);//破真空9
            GClsMontion.WaitIOCard7432InputBit(0, 8, 0, 3000, 62);//吸真空9感应
            GClsMontion.WriteIOCard7432OutputBit(0, 20, 0);//破真空9
        }
        #endregion

        #region  吸真空10动作

        public static void CldVacuum10On()//吸真空4
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 9, 1); //吸真空10
            GClsMontion.WriteIOCard7432OutputBit(0, 21, 0);//破真空10
            GClsMontion.WaitIOCard7432InputBit(0, 9, 1,3000,63);//吸真空10感应
        }

        public static void CldVacuum10Off()//破真空10
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 9, 0); //吸真空10
            GClsMontion.WriteIOCard7432OutputBit(0, 21, 1);//破真空10
            GClsMontion.WaitIOCard7432InputBit(0, 9, 0, 3000, 64);//吸真空10感应
            GClsMontion.WriteIOCard7432OutputBit(0, 21, 0);//破真空10
        }
        #endregion

        #region  吸真空11动作

        public static void CldVacuum11On()//吸真空4
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 10, 1); //吸真空11
            GClsMontion.WriteIOCard7432OutputBit(0, 22, 0);//破真空11
            GClsMontion.WaitIOCard7432InputBit(0, 10, 1,3000,65);//吸真空11感应
        }

        public static void CldVacuum11Off()//破真空11
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 10, 0); //吸真空11
            GClsMontion.WriteIOCard7432OutputBit(0, 22, 1);//破真空11
            GClsMontion.WaitIOCard7432InputBit(0, 10, 0, 3000, 66);//吸真空11感应
            GClsMontion.WriteIOCard7432OutputBit(0, 22, 0);//破真空11
        }
        #endregion

        #region  吸真空12动作

        public static void CldVacuum12On()//吸真空2
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 11, 1); //吸真空12
            GClsMontion.WriteIOCard7432OutputBit(0, 23, 0);//破真空12
            GClsMontion.WaitIOCard7432InputBit(0, 11, 1,3000,67);//吸真空12感应
        }

        public static void CldVacuum12Off()//破真空12
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 11, 0); //吸真空12
            GClsMontion.WriteIOCard7432OutputBit(0, 23, 1);//破真空12
            GClsMontion.WaitIOCard7432InputBit(0, 11, 0, 3000, 68);//吸真空12感应
            GClsMontion.WriteIOCard7432OutputBit(0, 23, 0);//破真空12
        }
        #endregion

        #region  小气缸控制1动作

        public static void CldSmallControl1Extend()//小气缸控制1伸出
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 24, 1); //小气缸控制1伸出
            GClsMontion.WaitIOCard7432InputBit(0, 12, 1, 3000, 69);//吸排线气缸1动作感应            
        }

        public static void CldSmallControl1Retract()//小气缸控制1缩回
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 24, 0); //小气缸控制1缩回
            GClsMontion.WaitIOCard7432InputBit(0, 12, 0, 3000, 70);//吸排线气缸1动作感应         
        }
        #endregion
        #region  小气缸控制2动作

        public static void CldSmallControl2Extend()//小气缸控制2伸出
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 25, 1); //小气缸控制2伸出
            GClsMontion.WaitIOCard7432InputBit(0, 13, 1, 3000, 71);//吸排线气缸2动作感应
        }

        public static void CldSmallControl2Retract()//小气缸控制2缩回
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 25, 0); //小气缸控制2缩回
            GClsMontion.WaitIOCard7432InputBit(0, 13, 0, 3000, 72);//吸排线气缸2动作感应          
        }
        #endregion
        #region  小气缸控制3动作

        public static void CldSmallControl3Extend()//小气缸控制3伸出
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 26, 1); //小气缸控制3伸出
            GClsMontion.WaitIOCard7432InputBit(0, 14, 1, 3000, 73);//吸排线气缸3动作感应          
        }

        public static void CldSmallControl3Retract()//小气缸控制3缩回
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 26, 0); //小气缸控制3缩回
            GClsMontion.WaitIOCard7432InputBit(0, 14, 0, 3000, 74);//吸排线气缸3动作感应            
        }
        #endregion  
        #region  小气缸控制4动作

        public static void CldSmallControl4Extend()//小气缸控制4伸出
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 27, 1); //小气缸控制4伸出
            GClsMontion.WaitIOCard7432InputBit(0, 15, 1, 3000, 75);//吸排线气缸4动作感应            
        }
         
        public static void CldSmallControl4Retract()//小气缸控制4缩回
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 27, 0); //小气缸控制4缩回
            GClsMontion.WaitIOCard7432InputBit(0, 15, 0, 3000, 76);//吸排线气缸4动作感应            
        }
        #endregion  
        #region  小气缸控制5动作

        public static void CldSmallControl5Extend()//小气缸控制5伸出 
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 28, 1); //小气缸控制5伸出
            GClsMontion.WaitIOCard7432InputBit(0, 16, 1, 3000, 77);//吸排线气缸5动作感应
        }

        public static void CldSmallControl5Retract()//小气缸控制5缩回
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 28, 0); //小气缸控制5缩回
            GClsMontion.WaitIOCard7432InputBit(0, 16, 0, 3000, 78);//吸排线气缸5动作感应
        }
        #endregion  
        #region  小气缸控制6动作

        public static void CldSmallControl6Extend()//小气缸控制6伸出 
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 29, 1); //小气缸控制6伸出
            GClsMontion.WaitIOCard7432InputBit(0, 17, 1, 3000, 79);//吸排线气缸6动作感应
        }

        public static void CldSmallControl6Retract()//小气缸控制6缩回
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 29, 0); //小气缸控制6缩回
            GClsMontion.WaitIOCard7432InputBit(0, 17, 0, 3000, 80);//吸排线气缸6动作感应
        }
        #endregion  
        #region  小气缸控制7动作

        public static void CldSmallControl7Extend()//小气缸控制7伸出 
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 30, 1); //小气缸控制7伸出
            GClsMontion.WaitIOCard7432InputBit(0, 18, 1, 3000, 81);//吸排线气缸7动作感应            
        }

        public static void CldSmallControl7Retract()//小气缸控制7缩回
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 30, 0); //小气缸控制7缩回
            GClsMontion.WaitIOCard7432InputBit(0, 18, 0, 3000, 82);//吸排线气缸7动作感应
        }
        #endregion  
        #region  小气缸控制8动作

        public static void CldSmallControl8Extend()//小气缸控制8伸出 
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 31, 1); //小气缸控制8伸出
            GClsMontion.WaitIOCard7432InputBit(0, 19, 1, 3000, 83);//吸排线气缸8动作感应
        }

        public static void CldSmallControl8Retract()//小气缸控制8缩回
        {
            GClsMontion.WriteIOCard7432OutputBit(0, 31, 0); //小气缸控制8缩回
            GClsMontion.WaitIOCard7432InputBit(0, 19, 0, 3000, 84);//吸排线气缸8动作感应
        }
        #endregion  
        #region  小气缸控制9动作

        public static void CldSmallControl9Extend()//小气缸控制9伸出 
        {
            GClsMontion.WriteCardOutputBit(1, 0, 1);//小气缸控制9伸出
            GClsMontion.WaitIOCard7432InputBit(0, 20, 1, 3000, 85);//吸排线气缸9动作感应
        }

        public static void CldSmallControl9Retract()//小气缸控制9缩回 
        {
            GClsMontion.WriteCardOutputBit(1, 0, 0);//小气缸控制9伸出
            GClsMontion.WaitIOCard7432InputBit(0, 20, 0, 3000, 86);//吸排线气缸9动作感应
        }
        #endregion  
        #region  小气缸控制10动作

        public static void CldSmallControl10Extend()//小气缸控制10伸出 
        {
            GClsMontion.WriteCardOutputBit(1, 1, 1);//小气缸控制10伸出
            GClsMontion.WaitIOCard7432InputBit(0, 21, 1, 3000, 87);//吸排线气缸10动作感应
        }

        public static void CldSmallControl10Retract()//小气缸控制10缩回 
        {
            GClsMontion.WriteCardOutputBit(1, 1, 0);//小气缸控制10伸出
            GClsMontion.WaitIOCard7432InputBit(0, 21, 0, 3000, 88);//吸排线气缸10动作感应
        }
        #endregion  
        #region  小气缸控制11动作

        public static void CldSmallControl11Extend()//小气缸控制11伸出 
        {
            GClsMontion.WriteCardOutputBit(1, 2, 1);//小气缸控制11伸出
            GClsMontion.WaitIOCard7432InputBit(0, 22, 1, 3000, 89);//吸排线气缸11动作感应
        }
         
        public static void CldSmallControl11Retract()//小气缸控制11缩回 
        {
            GClsMontion.WriteCardOutputBit(1, 2, 0);//小气缸控制11伸出
            GClsMontion.WaitIOCard7432InputBit(0, 22, 0, 3000, 90);//吸排线气缸11动作感应
        }
        #endregion  
        #region  小气缸控制12动作

        public static void CldSmallControl12Extend()//小气缸控制12伸出 
        {
            GClsMontion.WriteCardOutputBit(1, 3, 1);//小气缸控制12伸出
            GClsMontion.WaitIOCard7432InputBit(0, 23, 1, 3000, 91);//吸排线气缸12动作感应
        }

        public static void CldSmallControl12Retract()//小气缸控制12缩回 
        {
            GClsMontion.WriteCardOutputBit(1, 3, 0);//小气缸控制12伸出
            GClsMontion.WaitIOCard7432InputBit(0, 23, 0, 3000, 92);//吸排线气缸12动作感应
        }
        #endregion  
    }
}
