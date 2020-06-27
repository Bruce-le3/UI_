using CommPlc;
using Justech;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Justech
{
    class Status
    {
        #region Errcode状态设置
        public enum Plc_Run_Staus
        {
            进料到位完成,
            流线2定位完成,
            出料完成
        }

        public enum Plc_ErrcodeString
        {
            Normal,
            感应光电1,
            感应光电2,
            感应光电3,
            感应光电4,
            感应光电5,
            阻挡气缸1伸出感应,
            阻挡气缸1缩回感应,
            阻挡气缸2伸出感应,
            阻挡气缸2缩回感应,
            矫正气缸伸出感应,
            矫正气缸缩回感应,
            风扇报警信号,
            SMEMA下位机待料信号输入,
            SMEMA上位机送料信号输入,
            前门禁,
            后门禁,
            气源气压信号,
            急停信号,
        }
        public enum Axis_Io_Card_Errcodestring
        {
            Normal,
            轴1报警,
            轴2报警,
            轴3报警,
            轴4报警,
            真空感应,
            载具搬运上感应,
            载具搬运下感应,
        }
        # endregion
        #region Errcode设置
        /// <summary> plc代码从0开始，axis代码从1000开始
        /// 
        /// </summary>
        /// <param name="_num">序号</param>
        /// <param name="plc_errcodestring">报警代码</param>
        /// <param name="alarm_information">报警内容</param>
        /// <param name="datetime">报警时间</param>
        /// <param name="op">OP</param>
        /// <param name="Solution">解决方法</param>
        /// <param name="description">描述</param>
        public  static int plc_Err_num=0;  //plc感应器报警次数
        public static int axis_Err_num=0;  //轴报警次数
        public static int emg_num=0;  //急停次数
        public static int cylinder_num = 0;  //气缸次数
        static string op;
        static string Solution;   //plc解决方法
        static string description; //plc描述
        static string aSolution; //axis解决方法
        static string adescription; // axis描述
        public static void plc_Err_codeShow(int p_Number)
        {
            if (p_Number < 19)
            {
                plc_Err_num++;
                op = "OP";
                switch (p_Number)
                {
                    case 1:
                        Solution = "请检查感应光电1";
                        description = "感应光电异常";
                        break;
                    case 2:
                        Solution = "请检查感应光电2";
                        description = "感应光电异常";
                        break;
                    case 3:
                        Solution = "请检查感应光电3";
                        description = "感应光电异常";
                        break;
                    case 4:
                        Solution = "请检查感应光电4";
                        description = "感应光电异常";
                        break;
                    case 5:
                        Solution = "请检查感应光电5";
                        description = "感应光电异常";
                        break;
                    case 6:
                        Solution = "请检查阻挡气缸1伸出感应";
                        description = "阻挡气缸1伸出感应";
                        cylinder_num++;
                        break;
                    case 7:
                        Solution = "请检查阻挡气缸1缩回感应";
                        description = "阻挡气缸1缩回感应";
                        cylinder_num++;
                        break;
                    case 8:
                        Solution = "请检查阻挡气缸2伸出感应";
                        description = "阻挡气缸2伸出感应";
                        cylinder_num++;
                        break;
                    case 9:
                        Solution = "请检查阻挡气缸2缩回感应";
                        description = "阻挡气缸2缩回感应";
                        cylinder_num++;
                        break;
                    case 10:
                        Solution = "请检查矫正气缸伸出感应";
                        description = "矫正气缸伸出感应";
                        cylinder_num++;
                        break;
                    case 11:
                        Solution = "请检查矫正气缸缩回感应";
                        description = "矫正气缸缩回感应";
                        cylinder_num++;
                        break;
                    case 12:
                        Solution = "请检查风扇报警信号";
                        description = "风扇报警信号";
                        break;
                    case 13:
                        Solution = "请检查SMEMA下位机待料信号输入";
                        description = "SMEMA下位机待料信号输入";
                        break;
                    case 14:
                        Solution = "请检查SMEMA上位机送料信号输入";
                        description = "SMEMA上位机送料信号输入";
                        break;
                    case 15:
                        Solution = "请检查前门禁";
                        description = "前门禁";
                        break;
                    case 16:
                        Solution = "请检查后门禁";
                        description = "后门禁";
                        break;
                    case 17:
                        Solution = "请检查气源气压信号";
                        description = "气源气压信号";
                        break;
                    case 18:
                        Solution = "请检查急停信号";
                        description = "急停信号";
                        emg_num++;
                        break;
                }
                PLC_err_code(plc_Err_num, p_Number, Plc_ErrcodeString.Normal + p_Number, System.DateTime.Now.ToString(), op, Solution, description);
            }
        }
        public static void axis_Err_codeShow(int aNumber)
        {
            axis_Err_num++;
            op = "OP";
            switch (aNumber)
            {
                case 1:
                    aSolution = "请检查轴1报警";
                    adescription = "轴1报警";
                    break;
                case 2:
                    aSolution = "请检查轴2报警";
                    adescription = "轴2报警";
                    break;
                case 3:
                    aSolution = "请检查轴3报警";
                    adescription = "轴3报警";
                    break;
                case 4:
                    aSolution = "请检查轴4报警";
                    adescription = "轴4报警";
                    break;
                case 5:
                    aSolution = "请检查真空感应";
                    adescription = "真空感应";
                    break;
                case 6:
                    aSolution = "请检查载具搬运上感应";
                    adescription = "载具搬运上感应";
                    break;
                case 7:
                    aSolution = "请检查载具搬运下感应";
                    adescription = "载具搬运下感应";
                    break;
            }
            AXIS_err_code(axis_Err_num, aNumber + 1000, Axis_Io_Card_Errcodestring.Normal + aNumber, System.DateTime.Now.ToString(), op, aSolution, adescription);
        }
        #endregion
        #region  Errcode写入
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_num">序号</param>
        /// <param name="plc_errcodestring">报警代码</param>
        /// <param name="alarm_information">报警内容</param>
        /// <param name="datetime">报警时间</param>
        /// <param name="op">OP</param>
        /// <param name="Solution">解决方法</param>
        /// <param name="description">描述</param>

        public static void PLC_err_code(int _num, int plc_errcodestring, Status.Plc_ErrcodeString alarm_information, string datetime, string op, string Solution, string description)
        {
                //Main.machine_alarm_history.dataGridView1.Rows.Add();           
                //Main.machine_alarm_history.dataGridView1.Rows[Main.machine_alarm_history.dataGridView1.Rows.Count - 1].SetValues(_num, plc_errcodestring, alarm_information, datetime, op, Solution, description);
                //csv_err_write.csv_err(_num + "," + plc_errcodestring + "," + alarm_information + "," + datetime + "," + op + "," + Solution + "," + description + "\r\n");
                //Main.machine_alarm_history.label1.Text = "Err" + plc_errcodestring + "---" + alarm_information;
                //Main.machine_alarm_history.panel2.BackColor = Color.Red;
        }

        public static void AXIS_err_code(int a_num, int axis_io_card_errcodestring, Status.Axis_Io_Card_Errcodestring alarm_information, string datetime, string op, string Solution, string description)
        {
                //Main.machine_alarm_history.dataGridView1.Rows.Add();           
                //Main.machine_alarm_history.dataGridView1.Rows[Main.machine_alarm_history.dataGridView1.Rows.Count - 1].SetValues(a_num, axis_io_card_errcodestring, alarm_information, datetime, op, Solution, description);
                //csv_err_write.csv_err(a_num + "," + axis_io_card_errcodestring + "," + alarm_information + "," + datetime + "," + op + "," + Solution + "," + description + "\r\n");
                //Main.machine_alarm_history.label1.Text = "Err" + axis_io_card_errcodestring + "---" + alarm_information;
                //Main.machine_alarm_history.panel2.BackColor = Color.Red;
        }
        #endregion
        #region Errcode刷新读取
        //public static string strValue; //读DM区返回值
        public static readonly Color ColorOn = Color.FromArgb(30, 255, 50);     //设置On的背景颜色
        public static readonly Color ColorOff = Color.FromArgb(147, 147, 147); //设置Off的背景颜色
        public static int S_AxisIOStatus;

        public static void Refresh_AxisErr()
        {
            //获取运动轴的IO状态
            for (int i = 0; i < 4; i++)
            {
                MC1104.jmc_mc1104_get_io_status(i, out S_AxisIOStatus);
                for (int j = 0; j < 3; j++)
                {
                    Color col = ((S_AxisIOStatus >> j) & 1) == 1 ? ColorOn : ColorOff;   //根据轴上IO获取的状态，改变label的背景颜色
                    if (j == 1 & col == ColorOn)
                    {
                        axis_Err_codeShow(i + 1);
                    }
                }
            }
        }

        public static void Refresh_PlcErr()
        {
            //Manual_IO._Manual_io.ReadMultiWordValue(ComLink.WordArea.DM, 100, 1, ref strValue);
            //int convert_16 = Convert.ToInt32(strValue, 16);
            //plc_Err_codeShow(convert_16);
        }
        #endregion
    }
}
