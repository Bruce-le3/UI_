using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Justech 
{
    /// <summary> 
    /// APE_MC1104运动控制卡功能函数库 
    /// </summary> 
    public static class MC1104
    {
        #region  1.系统与初始化                                                        */

        /// <summary>
        /// 1.1  控制卡初始化
        /// 在调用板卡其功能函数前请先初始化，对系统内板卡分配资源。
        /// 以板卡上拨码区分卡号，请保证拨码编号在系统内的唯一性。
        /// CardID_InBit的第n位表示拨码为n的板卡存在于系统内，
        /// 例如系统内一共存在三块板卡,拨码分别为0,1,2,则CardID_InBit的值应为0x7
        /// </summary>
        /// <param name="Manual_ID">卡号区分以拨码方式</param>
        /// <param name="CardID_InBit">系统内板卡数量和编号</param>
        /// <returns>返回板卡初始化函数调用结果,0=normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_initial(int Manual_ID,out int CardID_InBit);


        /// <summary>  
        /// 1.2  控制卡关闭，释放系统内的控制卡资源
        /// </summary>
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_close();


        /// </summary>
        /// 1.3  获取控制卡固件、驱动、动态链接库版本
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="firmware_ver">固件版本号</param>
        /// <param name="driver_ver">驱动版本号</param>
        /// <param name="dll_ver">DLL库版本号</param>
        /// <param name="date">更新日期</param>
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_version(int CardId, out int firmware_ver,out int driver_ver,out int dll_ver,out int date);


        /// <summary>
        /// 1.4  获取板卡的生产信息
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="Bom_Rev">生产BOM号</param>
        /// <param name="FPGA_year">FPGA程序年份</param>
        /// <param name="FPGA_date">FPGA程序月/日</param>
        /// <param name="FPGA_update_times">升级序号</param>
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_fpga_version(int CardId, out int Bom_Rev, out int FPGA_year, 
                                                      out int FPGA_date, out int FPGA_update_times);     

        /// <summary>
        /// 1.5  获取板卡的序列号
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="sn">序列号</param>
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_sn(int CardId, byte[] sn);

        /// <summary>
        /// 1.5  获取板卡的序列号
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="sn">序列号</param>
        /// <returns>0=normal</returns>
        public static int jmc_mc1104_get_sn(int CardId, out string sn)
        {
            sn = "";
            byte[] byteSN = new byte[65];
            int ret = jmc_mc1104_get_sn(CardId, byteSN);
            if (ret != 0) return ret;
            int index = Array.IndexOf(byteSN, (byte)0);
            sn = Encoding.ASCII.GetString(byteSN, 0, index);
            return 0;
        }

        /// <summary>
        /// 1.6  设置控制卡安全码
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="old_secu_code">旧的安全码</param>
        /// <param name="new_secu_code">新的安全码</param>
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_security_key(int CardId, int old_secu_code, int new_secu_code);


        /// <summary>
        /// 1.7  检查控制卡安全码
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="secu_code">板卡的安全码</param>
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_check_security_key(int CardId, int secu_code);

        
        /// <summary>
        /// 1.8  重置控制卡安全码至初始值
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_reset_security_key(int CardId);

        
        /// <summary>
        /// 1.9  从文件载入控制卡配置参数
        /// 配置文件名为: MC1000.inf,系统内所有板卡的默认配置相同.
        /// </summary>
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_config_from_file();

        
        /// <summary>
        /// 1.10 将控制卡的配置参数写入文件
        /// 配置文件名为: MC1000.inf
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_write_to_file(int CardId);

        #endregion

        #region    2.脉冲输出输入设定                                                    */

        /// <summary>
        /// 2.1  设置脉冲输出模式
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="pls_outmode">脉冲输出模式</param>
        ///       0:OUT/DIR  脉冲大于0，方向大于0为正方向
        ///       1:OUT/DIR  脉冲小于0，方向大于0为正方向
        ///       2:OUT/DIR  脉冲大于0，方向小于0为正方向
        ///       3:OUT/DIR  脉冲小于0，方向小于0为正方向
        ///       4:CW/CCW  脉冲大于0为正向
        ///       5:CW/CCW  脉冲小于0为正向
        ///       6:AB  A向超前B向90°为正方向
        ///       7:AB  B向超前A向90°为正方向	
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_pls_outmode(int AxisNo,int pls_outmode);

        
        /// <summary>
        /// 2.2  获取脉冲输出模式
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="pls_outmode">脉冲输出模式</param>
        ///       0:OUT/DIR  脉冲大于0，方向大于0为正方向
        ///       1:OUT/DIR  脉冲小于0，方向大于0为正方向
        ///       2:OUT/DIR  脉冲大于0，方向小于0为正方向
        ///       3:OUT/DIR  脉冲小于0，方向小于0为正方向
        ///       4:CW/CCW  脉冲大于0为正向
        ///       5:CW/CCW  脉冲小于0为正向
        ///       6:AB  A向超前B向90°为正方向
        ///       7:AB  B向超前A向90°为正方向	
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_pls_outmode(int AxisNo, out int pls_outmode);

        
        /// <summary>
        /// 2.3  设置脉冲输入模式
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="pls_iptmode">脉冲输入模式</param>
        ///     0: 1xAB  
        ///     1: 2xAB 
        ///     2: 4xAB
        ///     3: CW/CCW
        /// <param name="pls_logic">脉冲信号逻辑</param>
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_pls_iptmode(int AxisNo, int pls_iptmode,int pls_logic);

        
        /// <summary>
        /// 2.4  获取脉冲输入模式
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="pls_iptmode">脉冲输入模式</param>
        ///     0: 1xAB  
        ///     1: 2xAB 
        ///     2: 4xAB
        ///     3: CW/CCW
        /// <param name="pls_logic">脉冲信号逻辑 </param>
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_pls_iptmode(int AxisNo,out int pls_iptmode,out int pls_logic);


        /// <summary>
        /// 2.5  设置反馈计数器输入来源
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Src">反馈计数器输入信号源</param>
        ///     0: 编码器
        ///     1: 命令脉冲
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_feedback_src(int AxisNo, int Src);

        #endregion

        #region    3.速度运动模式                                                        */

        /// <summary>
        /// 3.1  以T型速度曲线加速至目标速度持续运动
        /// 运动方向由速度的符号决定.为正时,正向运动.为负时,负向运动.
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="StrVel">启动速度[pps]</param>
        /// 启动速度为开始运动时的脉冲输出频率, 初始频率越小,启动过程越平滑.该值最小可为1.
        /// 该速度也用于误差补偿等运动,过小时会造成补偿时间过长甚至无限大(速度为0时).
        /// <param name="MaxVel">目标速度[pps]</param>
        /// 目标速度为恒速运动时的脉冲输出频率.该模式,电机会向某一方向持续运动.
        /// <param name="Tacc">加速度时间[s]</param>
        /// <returns>0=normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_tv_move(int AxisNo, double StrVel, double MaxVel,double Tacc);


        /// <summary>
        /// 3.2  以S型速度曲线加速至目标速度持续运动
        /// 运动方向由速度的符号决定.为正时,正向运动.为负时,负向运动。
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="StrVel">启动速度[pps]</param>
        /// <param name="MaxVel">目标速度[pps]</param>
        /// <param name="Tacc">加速度时间[s]</param>
        /// <param name="SVacc">加速度范围[pps]</param>
        /// 当Vacc=0时，加速端将为纯S曲线，没有直线加速段。
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_sv_move(int AxisNo, double StrVel, double MaxVel, double Tacc,double SVacc);


        /// <summary>
        /// 3.3  减速停止
        /// 减速停止时，当指定减速停止的轴正在做插补运动时,参与插补的其余轴均会停止。
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Tacc">减速度时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_sd_stop(int AxisNo,  double Tacc);


        /// <summary>
        /// 3.4  立即停止
        /// 立即停止输出脉冲，查询轴状态会显示有错误发生。
        /// 在连续运动模式下调用该接口后,会造成系统切换至错误状态。
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_emg_stop(int AxisNo);


        /// <summary>
        /// 3.5  获取当前运动轴速度
        /// 参与插补的轴速度,均返回相同的速度值
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="speed">当前速度[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_current_speed(int AxisNo, out double speed);

        
        /// <summary>
        /// 3.6  重载速度
        /// 按照jmc_mc1104_set_max_override_speed设定的最大速度.以比例的方式进行重载.
        /// 如果不设定最大速度,则按照最近一次运动的速度区间上限进行重载
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="NewVelPercent">速度百分比</param>
        /// <param name="Time">加/减速度时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_speed_override(int AxisNo, double NewVelPercent, double Time);

        
        /// <summary>
        /// 3.7  设置速度比
        /// 当前运动速度和重载后的速度必须在一个速度区间内
        /// 重载速度之前续调用该接口并将Enable设定为1，OvrdSpeed为0时以当前速度区间的最大值进行重载。
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="OvrdSpeed">速度百分比</param>
        /// OvrdSpeed为0时以当前速度区间的最大值进行重载
        ///      1:1-6553.5
        ///      2:6553.5-13107.0
        ///      3:32767.5-65535
        ///      4:1310720-327675
        ///      5:655350-1310700
        ///      6:3276750-6553500
        /// <param name="Enable">使能标示</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_max_override_speed(int AxisNo, double OvrdSpeed, int Enable);

        #endregion

        #region    4.单轴运动模式                                                        */

        /// <summary>
        /// 4.1  以T形速度曲线进行 相对运动
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Dist">相对距离（Pulse）</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_tr_move(int AxisNo, double Dist,double StrVel, double MaxVel, double Tacc,double Tdec);
        

        /// <summary>
        /// 4.2  以T形速度曲线进行 绝对运动
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="pos">绝对目标位置（Pulse）</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速度时间[s]</param>
        /// <param name="Tdec">减速度时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_ta_move(int AxisNo, double pos, double StrVel, double MaxVel, double Tacc,double Tdec);
        

        /// <summary>
        /// 4.3  以S形速度曲线进行 相对运动
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Dist">相对距离（Pulse）</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sr_move(int AxisNo, double Dist, double StrVel, double MaxVel, 
                                                    double Tacc, double Tdec,double SVacc,double SVdec);


        /// <summary>
        /// 4.4  以S形速度曲线进行 绝对 距离运动
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="pos">绝对目标位置（Pulse）</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sa_move(int AxisNo, double pos, double StrVel, double MaxVel, 
                                                    double Tacc, double Tdec, double SVacc, double SVdec);

       
        /// <summary>
        /// 4.5  设置命令脉冲信号与反馈脉冲信号比例
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="move_ratio">反馈输入与命令输出分辨率比例，此值默认为1。</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_move_ratio(int AxisNo, double move_ratio);

        #endregion

        #region    5.多轴直线插补运动                                                    */
        //插补轴只能位于同一板卡上,不同板卡上的轴不能进行插补运动.

        /// <summary>
        /// 5.1  以T形速度曲线进行 XY两轴 相对距离 直线插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="DisX">X轴运动相对距离(Pulse)</param>
        /// <param name="DisY">Y轴运动相对距离(Pulse)</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_tr_move_xy(int CardId, double DisX, double DisY,
                                                        double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 5.2  以T形速度曲线进行 ZU两轴 相对距离 直线插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="DisZ">Z轴运动相对距离(Pulse)</param>
        /// <param name="DisU">U轴运动相对距离(Pulse)</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_tr_move_zu(int CardId, double DisZ, double DisU, 
                                                            double StrVel, double MaxVel, double Tacc, double Tdec);

        
        /// <summary>
        /// 5.3  以T形速度曲线进行 XY两轴 绝对位置 直线插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="PosX">X轴运动绝对位置(Pulse)</param>
        /// <param name="PosY">Y轴运动绝对位置(Pulse)</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_ta_move_xy(int CardId, double PosX, double PosY, 
                                                        double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 5.4  以T形速度曲线进行 ZU两轴 绝对位置 直线插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="PosZ">Z轴运动绝对位置(Pulse)</param>
        /// <param name="PosU">U轴运动绝对位置(Pulse)</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_ta_move_zu(int CardId, double PosZ, double PosU,     
                                                        double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 5.5  以S形速度曲线进行 XY两轴 相对距离 直线插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="PosZ">Z轴运动绝对位置(Pulse)</param>
        /// <param name="PosU">U轴运动绝对位置(Pulse)</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sr_move_xy(int CardId, double DisX, double DisY,
                                                        double StrVel, double MaxVel, double Tacc, double Tdec, 
                                                        double SVacc, double SVdec);

 
        /// <summary>
        /// 5.6  以S形速度曲线进行 ZU两轴 相对距离 直线插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="PosZ">Z轴运动绝对位置(Pulse)</param>
        /// <param name="PosU">U轴运动绝对位置(Pulse)</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sr_move_zu(int CardId, double DisZ, double DisU,
                                                        double StrVel, double MaxVel, double Tacc, double Tdec,
                                                        double SVacc, double SVdec);


        /// <summary>
        /// 5.7  以S形速度曲线进行 XY两轴 绝对位置 直线插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="PosX">X轴运动绝对位置(Pulse)</param>
        /// <param name="PosY">Y轴运动绝对位置(Pulse)</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sa_move_xy(int CardId, double PosX, double PosY,
                                                        double StrVel, double MaxVel, double Tacc, double Tdec,
                                                        double SVacc, double SVdec);

       
        /// <summary>
        /// 5.8  以S形速度曲线进行 ZU两轴 绝对位置 直线插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="PosZ">Z轴运动绝对位置(Pulse)</param>
        /// <param name="PosU">U轴运动绝对位置(Pulse)</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sa_move_zu(int CardId, double PosZ, double PosU,
                                                        double StrVel, double MaxVel, double Tacc, double Tdec,
                                                        double SVacc, double SVdec);


        /// <summary>
        /// 5.9  以T形速度曲线进行 任意两轴 相对距离 直线插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="DistArray">直线插补运动的相对距离数组</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_tr_line2(int[] AxisArray,double[] DistArray, 
                                                    double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 5.10  以T形速度曲线进行 任意两轴 绝对位置 直线插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="PosArray">直线插补运动的绝对位置数组</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_ta_line2(int[] AxisArray, double[] PosArray,  
                                                    double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 5.11  以S形速度曲线进行 任意两轴 相对距离 直线插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="DistArray">直线插补运动的相对距离数组</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sr_line2(int[] AxisArray, double[] DistArray,  
                                                    double StrVel, double MaxVel, double Tacc, double Tdec,
                                                    double SVacc,double SVdec);


        /// <summary>
        /// 5.12  以S形速度曲线进行 任意两轴 绝对位置 直线插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="PosArray">直线插补运动的绝对位置数组</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sa_line2(int[] AxisArray, double[] PosArray,  
                                                    double StrVel, double MaxVel, double Tacc, double Tdec,
                                                    double SVacc,double SVdec);


        /// <summary>
        /// 5.13  以T形速度曲线进行 任意三轴 相对距离 直线插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="DistArray">直线插补运动的相对距离数组</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_tr_line3(int[] AxisArray, double[] DistArray, 
                                                    double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 5.14  以T形速度曲线进行 任意三轴 绝对位置 直线插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="PosArray">直线插补运动的绝对位置数组</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_ta_line3(int[] AxisArray, double[] PosArray,  
                                                    double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 5.15  以S形速度曲线进行 任意三轴 相对距离 直线插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="DistArray">直线插补运动的相对距离数组</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sr_line3(int[] AxisArray, double[] DistArray,  
                                                    double StrVel, double MaxVel, double Tacc, double Tdec, 
                                                    double SVacc, double SVdec);


        /// <summary>
        /// 5.16  以S形速度曲线进行 任意三轴 绝对位置 直线插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="PosArray">直线插补运动的绝对位置数组</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sa_line3(int[] AxisArray, double[] PosArray,  
                                                    double StrVel, double MaxVel, double Tacc, double Tdec,     
                                                    double SVacc, double SVdec);


        /// <summary>
        /// 5.17  以T形速度曲线进行 四轴 相对距离 直线插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="DistArray">直线插补运动的相对距离数组</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_tr_line4(int[] AxisArray, double[] DistArray,  
                                                    double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 5.18  以T形速度曲线进行 四轴 绝对位置 直线插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="PosArray">直线插补运动的绝对位置数组</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_ta_line4(int[] AxisArray, double[] PosArray, 
                                                    double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 5.19  以S形速度曲线进行 四轴 相对距离 直线插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="DistArray">直线插补运动的相对距离数组</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sr_line4(int[] AxisArray, double[] DistArray,  
                                                    double StrVel, double MaxVel, double Tacc, double Tdec, 
                                                    double SVacc, double SVdec);


        /// <summary>
        /// 5.20  以S形速度曲线进行 四轴 绝对位置 直线插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="PosArray">直线插补运动的绝对位置数组</param>
        /// <param name="StrVel">启始速度[pps]</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sa_line4(int[] AxisArray,  double[] PosArray, 
                                                    double StrVel, double MaxVel, double Tacc, double Tdec, 
                                                    double SVacc, double SVdec);
        #endregion

        #region    6.两轴圆弧插补运动                                                    */

        /// <summary>
        /// 6.1  以T形速度曲线进行 XY两轴 相对距离 圆弧插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="OffsetCx">起点至X轴相对圆心距离</param>
        /// <param name="OffsetCy">起点至Y轴相对圆心距离</param>
        /// <param name="OffsetEx">起点至X轴相对终点距离</param>
        /// <param name="OffsetEy">起点至Y轴相对终点距离</param>
        /// <param name="CW_CCW">运动方向[0=顺时针,1=逆时针]</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_tr_arc_xy(int CardId,double OffsetCx, double OffsetCy, 
                                                       double OffsetEx, double OffsetEy, int CW_CCW , 
                                                        double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 6.2  以T形速度曲线进行 XY两轴 绝对距离 圆弧插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="CenterX">圆心X轴坐标</param>
        /// <param name="CenterY">圆心Y轴坐标</param>
        /// <param name="EndX">终点X轴坐标</param>
        /// <param name="EndY">终点Y轴坐标</param>
        /// <param name="CW_CCW">运动方向[0=顺时针,1=逆时针]</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_ta_arc_xy(int CardId, double CenterX, double CenterY, 
                                                        double EndX, double EndY, int CW_CCW, 
                                                        double StrVel, double MaxVel, double Tacc, double Tdec);

       
        /// <summary>
        /// 6.3  以S形速度曲线进行 XY两轴 相对距离 圆弧插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="OffsetCx">起点至X轴相对圆心距离</param>
        /// <param name="OffsetCy">起点至Y轴相对圆心距离</param>
        /// <param name="OffsetEx">起点至X轴相对终点距离</param>
        /// <param name="OffsetEy">起点至Y轴相对终点距离</param>
        /// <param name="CW_CCW">运动方向[0=顺时针,1=逆时针]</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sr_arc_xy(int CardId, double OffsetCx, double OffsetCy, double OffsetEx, double OffsetEy, 
                                                        int CW_CCW, double StrVel, double MaxVel, double Tacc, double Tdec,double SVacc,double SVdec);


        /// <summary>
        /// 6.4  以S形速度曲线进行 XY两轴 绝对距离 圆弧插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="CenterX">圆心X轴坐标</param>
        /// <param name="CenterY">圆心Y轴坐标</param>
        /// <param name="EndX">终点X轴坐标</param>
        /// <param name="EndY">终点Y轴坐标</param>
        /// <param name="CW_CCW">运动方向[0=顺时针,1=逆时针]</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sa_arc_xy(int CardId, double CenterX, double CenterY, double EndX, double EndY, 
                                                        int CW_CCW, double StrVel, double MaxVel, double Tacc, double Tdec,double SVacc,double SVdec);


        /// <summary>
        /// 6.5  以T形速度曲线进行 ZU两轴 相对距离 圆弧插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="OffsetCz">起点至圆心Z轴相对距离</param>
        /// <param name="OffsetCu">起点至圆心U轴相对距离</param>
        /// <param name="OffsetEz">终点至圆心Z轴相对距离</param>
        /// <param name="OffsetEu">终点至圆心U轴相对距离</param>
        /// <param name="CW_CCW">运动方向[0=顺时针,1=逆时针]</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_tr_arc_zu(int CardId, double OffsetCz, double OffsetCu, double OffsetEz, double OffsetEu, 
                                                        int CW_CCW, double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 6.6  以T形速度曲线进行 ZU两轴 绝对距离 圆弧插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="CenterZ">圆心X轴坐标</param>
        /// <param name="CenterU">圆心Y轴坐标</param>
        /// <param name="EndZ">终点X轴坐标</param>
        /// <param name="EndU">终点Y轴坐标</param>
        /// <param name="CW_CCW">运动方向[0=顺时针,1=逆时针]</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_ta_arc_zu(int CardId, double CenterZ, double CenterU, double EndZ, double EndU, 
                                                        int CW_CCW, double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 6.7  以S形速度曲线进行 ZU两轴 相对距离 圆弧插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="OffsetCz">起点至圆心Z轴相对距离</param>
        /// <param name="OffsetCu">起点至圆心U轴相对距离</param>
        /// <param name="OffsetEz">终点至圆心Z轴相对距离</param>
        /// <param name="OffsetEu">终点至圆心U轴相对距离</param>
        /// <param name="CW_CCW">运动方向[0=顺时针,1=逆时针]</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sr_arc_zu(int CardId, double OffsetCz, double OffsetCu, double OffsetEz, double OffsetEu, 
                                                        int CW_CCW, double StrVel, double MaxVel, double Tacc, double Tdec, 
                                                        double SVacc, double SVdec);
        
        
        /// <summary>
        /// 6.8  以S形速度曲线进行 ZU两轴 绝对距离 圆弧插补
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="CenterZ">圆心Z轴坐标</param>
        /// <param name="CenterU">圆心U轴坐标</param>
        /// <param name="EndZ">终点Z轴坐标</param>
        /// <param name="EndU">终点U轴坐标</param>
        /// <param name="CW_CCW">运动方向[0=顺时针,1=逆时针]</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sa_arc_zu(int CardId, double CenterZ, double CenterU, double EndZ, double EndU, 
                                                        int CW_CCW, double StrVel, double MaxVel, double Tacc, double Tdec,
                                                        double SVacc, double SVdec);


        /// <summary>
        /// 6.9  以T形速度曲线进行 任意两轴 相对距离 圆弧插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="OffsetCenter">起点至圆心相对距离</param>
        /// <param name="OffsetEnd">起点至终点相对距离</param>
        /// <param name="CW_CCW">运动方向[0=顺时针,1=逆时针]</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_tr_arc2(int[] AxisArray,  double[] OffsetCenter,  double[] OffsetEnd,  
                                                    int CW_CCW, double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 6.10  以T形速度曲线进行 任意两轴 绝对距离 圆弧插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="CenterPos">圆心坐标</param>
        /// <param name="OffsetPos">终点坐标</param>
        /// <param name="CW_CCW">运动方向[0=顺时针,1=逆时针]</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_ta_arc2(int[] AxisArray,  double[] CenterPos,  double[] OffsetPos, 
                                                    int CW_CCW, double StrVel, double MaxVel, double Tacc, double Tdec);


        /// <summary>
        /// 6.11  以S形速度曲线进行 任意两轴 相对距离 圆弧插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="OffsetCenter">起点至圆心相对距离</param>
        /// <param name="OffsetEnd">起点至终点相对距离</param>
        /// <param name="CW_CCW">运动方向[0=顺时针,1=逆时针]</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sr_arc2(int[] AxisArray,  double[] OffsetCenter,  double[] OffsetEnd, 
                                                    int CW_CCW, double StrVel, double MaxVel, 
                                                    double Tacc, double Tdec,double SVacc,double SVdec);


        /// <summary>
        /// 6.12  以S形速度曲线进行 任意两轴 绝对距离 圆弧插补
        /// </summary>
        /// <param name="AxisArray">执行插补运动的运动轴数组</param>
        /// <param name="CenterPos">圆心坐标</param>
        /// <param name="EndPos">终点坐标</param>
        /// <param name="CW_CCW">运动方向[0=顺时针,1=逆时针]</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度[pps]</param>
        /// <param name="Tacc">加速时间[s]</param>
        /// <param name="Tdec">减速时间[s]</param>
        /// <param name="SVacc">加速S曲线范围[pps]</param>
        /// <param name="SVdec">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_start_sa_arc2( int[] AxisArray,  double[] CenterPos,  double[] EndPos, 
                                                    int CW_CCW, double StrVel, double MaxVel, double Tacc, double Tdec, 
                                                    double SVacc, double SVdec);
        #endregion

        #region    7.归零运动                                                            */

        /// <summary>
        /// 7.1  设置回原点运动模式与信号逻辑
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="home_mode">归零方式[0-12]</param>
        /// <param name="ez_logic">EZ信号逻辑[1:正逻辑 0:负逻辑]</param>
        /// <param name="ez_count">回零时的EZ计数</param>
        /// <param name="erc_out">回零后自动产生ERC信号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_home_config(int AxisNo, int home_mode, int ez_logic, 
                                                                    int  ez_count, int erc_out);


        /// <summary>
        /// 7.2  进行回原点运动
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度</param>
        /// <param name="Tacc">加速时间</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_home_move(int AxisNo, double StrVel, double MaxVel, double Tacc);


        /// <summary>
        /// 7.3  进行原点位置搜索
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="StrVel">起始速度</param>
        /// <param name="MaxVel">最大速度</param>
        /// <param name="Tacc">加速时间</param>
        /// <param name="ORGOffset">原点偏移量</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_home_search(int AxisNo, double StrVel, double MaxVel, double Tacc,double ORGOffset);

        #endregion

        #region    8.手轮脉冲模式                                                        */

        /// <summary>
        /// 8.1  设置手轮脉冲发生器输入信号模式
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="InputMode">输入信号的模式</param>
        ///         0:1xAB 
        ///         1:2xAB 
        ///         2:4xAB 
        ///         3:CW/CCW
        /// <param name="Inverse">取反标示</param>
        ///         0:  不取反
        ///         1:  取反
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_pulser_iptmode(int AxisNo, int InputMode,int Inverse);


        /// <summary>
        /// 8.2  关闭脉冲发生器输入信号
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Disable">禁止输入标志</param>
        ///     1: 禁止 
        ///     0: 不禁止
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_disable_pulser_input(int AxisNo, int Disable);


        /// <summary>
        /// 8.3  由脉冲发生器进行 速度模式 运动
        /// 手脉连续运动.板卡只要接收到手脉信号则发出脉冲控制电机运动
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="SpeedLimit">运动速度限制(PPS)</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_pulser_vmove(int AxisNo, double SpeedLimit);


        /// <summary>
        /// 8.4  由脉冲发生器进行 位置模式 运动
        /// 手脉定长运动.板卡接收到手脉信号则控制电机完成一个定长运动,多余的脉冲无效
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Dist">运动相对距离(Pulse)</param>
        /// <param name="SpeedLimit">运动速度限制(PPS)</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_pulser_pmove(int AxisNo, double Dist,double SpeedLimit);


        /// <summary>
        /// 8.5  设定手轮脉冲发生器输入与命令脉冲输出比例
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="DivF">脉冲分割率[1-2047]</param>
        /// <param name="MultiF">脉冲倍率[0-31]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_pulser_ratio(int AxisNo, int DivF,int MultiF);

        #endregion

        #region    9.运动状态                                                            */

        /// <summary>
        /// 9.1  获取轴运动状态
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <returns>返回轴状态,常用来判断轴是否还在运动</returns>
        ///      bit0-bit3:表示轴的运动状体
        ///          0000 - Normal stopped condition
        ///          0001 - Waiting for DR
        ///          0010 - Waiting for CSTA input
        ///          0011 - Waiting for an internal synchronous signal
        ///          0100 - Waiting for another axis to stop
        ///          0101 - Waiting for a completion of ERC timer
        ///          0110 - Waiting for a completion of direction change timer
        ///          0111 - Correcting backlash
        ///          1000 - Waiting for PA/PB input
        ///          1001 - Feeding at FA constant speed
        ///          1010 - Feeding at FL constant speed
        ///          1011 - Accelerating
        ///          1100 - Feeding at FH constant speed
        ///          1101 - Decelerating
        ///          1110 - Waiting for INP input
        ///          1111 - Others (controlling start)
        ///      bit4:ALM输入有效时为1
        ///      bit5:+EL输入有效时为1 
        ///      bit6:-EL输入有效时为1 
        ///      bit7:CEMG输入有效时为1 
        ///      bit8:CSTP输入有效时为1 
        ///      bit9:ERC输入有效时为1 
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_motion_done(int AxisNo);


        /// <summary>
        /// 9.2  获取运动轴主状态
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="main_status">主状态, 为一个16位的寄存器,各位表示的含义如下:</param>
        ///          bit0:   写入一条启动指令后变为1，操作被停止后变为0
        ///          bit1:   启动脉冲输出后为1，运动停止后变为0
        ///          bit2:   运动停止中断标志
        ///          bit3:   写入一条启动指令后变为0，操作停止后变为1
        ///          bit4:   当发生错误中断时为1
        ///          bit5:   当发生中断时为1
        ///          bit6-bit7: 执行或停止的序号
        ///          bit8:   当比较器1的条件满足时为1(软件限位的正限位触发)
        ///          bit9:   当比较器2的条件满足时为1(软件限位的负限位触发)
        ///          bit10:  当比较器3的条件满足时为1(误差超过限制)
        ///          bit11:  当比较器4的条件满足时为1
        ///          bit12:  当比较器5的条件满足时为1
        ///          bit13:  重新设置目标位置的操作不能被执行时为1，读取主状态后变为0
        ///          bit14:  运动类预置寄存器都为满时，为1
        ///          bit15:  当比较器5的预置寄存器为满时，为1
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_main_status(int AxisNo, out int main_status);


        /// <summary>
        /// 9.3  获取副状态
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="sub_status">副状态,为一个16位的寄存器,各位表示的含义如下:</param>
        ///          bit0-bit7:  P0-P7的状态
        ///          bit8:       加速过程中为1
        ///          bit9:       减速过程中为1   
        ///          bit10:      匀速过程中为1
        ///          bit11:      ALM输入有效时为1
        ///          bit12:      +EL输入有效时为1 
        ///          bit13:      -EL输入有效时为1 
        ///          bit14:      ORG输入有效时为1 
        ///          bit15:      SD输入有效时为1
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_sub_status(int AxisNo, out int sub_status);


        /// <summary>
        /// 9.4  获取错误状态
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="error_status">错误状态,为一个32位的寄存器,仅0-17位有意义,各位表示的含义如下:</param>
        ///          bit0:       比较器1条件触发而停止
        ///          bit1:       比较器2条件触发而停止
        ///          bit2:       比较器3条件触发而停止 
        ///          bit3:       比较器4条件触发而停止 
        ///          bit4:       比较器5条件触发而停止 
        ///          bit5:       +EL触发而停止
        ///          bit6:       -EL触发而停止  
        ///          bit7:       ALM触发而停止   
        ///          bit8:       CSTP停止
        ///          bit9:       CEMG停止
        ///          bit10:      SD减速停止
        ///          bit11:      预留      
        ///          bit12:      插补数据错误
        ///          bit13:      因插补运动中其他轴错误而停止
        ///          bit14:      PA/PB输入溢出
        ///          bit15:      插补中定位计数器溢出
        ///          bit16:      EA/EB输入错误(运动不停止)
        ///          bit17:      PA/PB输入错误(运动不停止) 
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_error_status(int AxisNo, out int error_status);


        /// <summary>
        /// 9.5  获取扩展的状态
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="ext_status">扩展状态,为一个32位的寄存器,仅0-21位有意义,各位表示的含义如下:</param>
        ///         bit0-bit3:  操作状态
        ///         bit4:       运动方向[0:正,1:负]
        ///         bit5:       1:CSTA输入有效   
        ///         bit6:       1:CSTP输入有效
        ///         bit7:       1:CEMG输入有效
        ///         bit8:       1:PCS输入有效
        ///         bit9:       1:ERC输入有效   
        ///         bit10:      1:EZ输入有效   
        ///         bit11:      1:+DR输入有效
        ///         bit12:      1:-DR输入有效
        ///         bit13:      1:CLR输入有效
        ///         bit14:      1:LTC输入有效
        ///         bit15:      1:SD输入有效
        ///         bit16:      1:INP输入有效
        ///         bit17:      预留  
        ///         bit18-bit19:监控比较器5预置寄存器
        ///         bit20-bit21:监控预置寄存器(不包括RCMP5)的操作状态 
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_ext_status(int AxisNo, out int ext_status);

        #endregion

        #region    10.运动界面I/O                                                        */

        /// <summary>
        /// 10.1  获取运动轴的IO状态
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="io_sts">io状态</param>
        ///      bit0:RDY 
        ///      bit1:ALM
        ///      bit2:PEL
        ///      bit3:MEL
        ///      bit4:ORG
        ///      bit5:DIR
        ///      bit6:EMG
        ///      bit7:
        ///      bit8:ERC
        ///      bit9:EZ
        ///      bit10:CLR
        ///      bit11:SD
        ///      bit12:
        ///      bit13:INP
        ///      bit14:SVON 
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_io_status(int AxisNo, out int io_sts);


        /// <summary>
        /// 10.2  设定SVON信号ON/OFF
        /// 伺服激磁输出信号
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="on_off">On_Off： 0=OFF, 1=ON</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_servo(int AxisNo, int on_off);


        /// <summary>
        /// 10.3  获取SVON信号状态
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="on_off">On_Off： 0=OFF, 1=ON</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_servo(int AxisNo, out int on_off);


        /// <summary>
        /// 10.4  设置CLR信号模式
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="clr_mode">CLR信号模式</param>
        ///          0:下降沿
        ///          1:上升沿
        ///          2:低电平
        ///          3:高电平 
        /// <param name="targetCounterInBit">CLR有效时清除的计数器</param>
        ///          bit0:计数器1
        ///          bit1:计数器2
        ///          bit2:计数器3
        ///          bit3:计数器4 
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_clr_mode(int AxisNo, int clr_mode,int targetCounterInBit);

        
        /// <summary>
        /// 10.5  设置INP逻辑与操作模式
        /// INP信号为控制器反馈的位置就绪信号,表示运动已经结束.伺服电机在运动结束后会有一段稳定过程,
        /// 表现出运动后有一小段时间的延迟.如果在连续运动时,不想等待该信号,请将inp_enable设定为0 
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="inp_enable">使能标志</param>
        /// <param name="inp_logic">信号逻辑</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_inp(int AxisNo, int inp_enable,int inp_logic);


        /// <summary>
        /// 10.6  获取INP信号的设置
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="inp_enable">是否使用INP信号</param>
        /// <param name="inp_logic">INP信号逻辑</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_inp(int AxisNo, out int inp_enable, out int inp_logic);

        
        /// <summary>
        /// 10.7  设置ALM逻辑与操作模式
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="alm_logic">信号逻辑[0:负逻辑,1:正逻辑]</param>
        /// <param name="alm_mode">信号模式[0:立即停止,1:减速停止]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_alm(int AxisNo, int alm_logic, int alm_mode);

        
        /// <summary>
        /// 10.8  获取ALM信号状态
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="alm_logic">ALM信号的逻辑[0:负逻辑,1:正逻辑就]</param>
        /// <param name="alm_mode">ALM信号的停止方式[0:立即停止,1:减速停止]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_alm(int AxisNo, out int alm_logic, out int alm_mode);

        
        /// <summary>
        /// 10.9  设置ERC信号逻辑与时序
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="erc_logic">ERC信号逻辑[0=负逻辑,1=正逻辑]</param>
        /// <param name="erc_pulse_width">ERC信号宽度</param>
        /// [0=12us,1=102us,2=409us,3=1.6ms,4=13ms,5=52ms,6=104ms,7=电平输出]
        /// <param name="erc_mode">ERC信号模式</param>
        /// [0=不使用ERC,1=EL,ALM,EMG有效时输出ERC,2=归零后输出ERC,3= 1和2]
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_erc(int AxisNo, int erc_logic , int erc_pulse_width, int erc_mode);


        /// <summary>
        /// 10.10  获取ERC信号逻辑与模式
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="erc_logic">ERC信号逻辑[0=负逻辑,1=正逻辑]</param>
        /// <param name="erc_pulse_width">ERC信号宽度</param>
        /// [0=12us,1=102us,2=409us,3=1.6ms,4=13ms,5=52ms,6=104ms,7=电平输出]
        /// <param name="erc_mode">ERC信号模式</param>
        /// [0=不使用ERC,1=EL,ALM,EMG有效时输出ERC,2=归零后输出ERC,3= 1和2]
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_get_erc(int AxisNo, out int erc_logic, out int erc_pulse_width, out int erc_mode);

        
        /// <summary>
        /// 10.11  手动输出一个ERC信号
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_set_erc_out(int AxisNo);


        /// <summary>
        /// 10.12  清除ERC信号
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_clr_erc(int AxisNo); 


        /// <summary>
        /// 10.13  设置SD逻辑与操作模式
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="sd_logic">SD逻辑</param>
        /// <param name="sd_latch">SD锁存</param>
        /// <param name="sd_mode">SD信号模式</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_set_sd(int AxisNo, int sd_logic, int sd_latch, int sd_mode);

        
        /// <summary>
        /// 10.14  获取SD信号的设置
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="sd_logic">SD逻辑</param>
        /// <param name="sd_latch">SD锁存</param>
        /// <param name="sd_mode">SD信号模式</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_get_sd(int AxisNo, out int sd_logic, out int sd_latch, out int sd_mode);

        
        /// <summary>
        /// 10.15  启动停止SD信号功能
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="enable">使能标志</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_enable_sd(int AxisNo, int enable);
        

        /// <summary>
        /// 10.16  设置PEL/MEL限位信号逻辑
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Logic">限位逻辑[0: 低电平;1: 高电平]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_set_el_logic(int AxisNo, int Logic);

        
        /// <summary>
        /// 10.17  设置PEL/MEL限位信号触发时停止方式
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="limit_mode">极限触发时处理方式</param>
        /// 0:立即停止;1:减速停止
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_set_el_mode(int AxisNo, int limit_mode);

        
        /// <summary>
        /// 10.18  获取PEL/MEL限位信号触发时停止方式
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="el_logic">极限触发时处理方式</param>
        ///  0:立即停止;1:减速停止
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_get_el_mode(int AxisNo, out int el_logic);


        /// <summary>
        /// 10.19  设置ORG信号逻辑
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="org_logic">原点感应器逻辑</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_org(int AxisNo, int org_logic);


        /// <summary>
        /// 10.20  获取ORG信号逻辑
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="org_logic">原点感应器逻辑</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_org(int AxisNo, out int org_logic);


        /// <summary>
        /// 10.21  设置EZ信号逻辑
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="ez_logic">EZ信号逻辑</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_ez(int AxisNo, int ez_logic);


        /// <summary>
        /// 10.22  获取EZ信号逻辑
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="ez_logic">EZ信号逻辑</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_ez(int AxisNo, out int ez_logic);

        #endregion

        #region    11.中断控制                                                           */

        /// <summary>
        /// 11.1  启动/停止中断功能
        /// </summary>
        /// <param name="CardId">轴号</param>
        /// <param name="Flag">使能中断[1=使能中断,0=关闭中断]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_int_control(int CardId, int Flag);

        
        /// <summary>
        /// 11.2  等待错误中断事件
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="TimeOut_ms">查询事件ms</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_wait_error_interrupt(int AxisNo, int TimeOut_ms);


        /// <summary>
        /// 11.3  等待运动中断事件
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="IntFactorBitNo">选择中断标志</param>
        ///          bit0:自动停止
        ///          bit1:缓存中第二条命令开始执行
        ///          bit2:可以写入预置寄存器
        ///          bit3:比较器5可以写入预置寄存器
        ///          bit4:开始加速
        ///          bit5:停止加速
        ///          bit6:开始减速
        ///          bit7:停止减速
        ///          bit8:比较器1满足条件
        ///          bit9:比较器2满足条件
        ///          bit10:比较器3满足条件
        ///          bit11:比较器4满足条件
        ///          bit12:比较器5满足条件
        ///          bit13:CLR清除计数器
        ///          bit14:LTC信号锁存
        ///          bit15:ORG信号锁存
        ///          bit16:SD信号ON
        ///          bit17:+DR input changes.
        ///          bit18:-DR input changes.
        ///          bit19:the #CSTA input turns ON.
        ///          bit20:DO0位置比较触发
        ///          bit21:DO1位置比较触发
        ///          bit22:DO2位置比较触发
        ///          bit23:DO3位置比较触发 
        /// <param name="TimeOut_ms">超时ms</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_wait_motion_interrupt(int AxisNo, int IntFactorBitNo, int TimeOut_ms);

        
        /// <summary>
        /// 11.4  选择运动中断事件
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="int_factor">中断源</param>
        ///          bit0:自动停止
        ///          bit1:缓存中第二条命令开始执行
        ///          bit2:可以写入预置寄存器
        ///          bit3:比较器5可以写入预置寄存器
        ///          bit4:开始加速
        ///          bit5:停止加速
        ///          bit6:开始减速
        ///          bit7:停止减速
        ///          bit8:比较器1满足条件
        ///          bit9:比较器2满足条件
        ///          bit10:比较器3满足条件
        ///          bit11:比较器4满足条件
        ///          bit12:比较器5满足条件
        ///          bit13:CLR清除计数器
        ///          bit14:LTC信号锁存
        ///          bit15:ORG信号锁存
        ///          bit16:SD信号ON
        ///          bit17:+DR input changes.
        ///          bit18:-DR input changes.
        ///          bit19:the #CSTA input turns ON.
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_set_motion_int_factor(int AxisNo, int int_factor);

        #endregion

        #region    12.位置控制与计数器                                                   */

        /// <summary>
        /// 12.1  获取反馈位置计数器值
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Pos">反馈位置数值</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_position(int AxisNo, out double Pos);

        
        /// <summary>
        /// 12.2  设置反馈位置计数器值
        /// </summary>
        /// <param name="AxisNo">轴号 </param>
        /// <param name="Pos">反馈位置数值</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_position(int AxisNo, double Pos);

        
        /// <summary>
        /// 12.3  获取指令脉冲计数器值
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Cmd">指令位置脉冲数值</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_command(int AxisNo, out double Cmd);

        
        /// <summary>
        /// 12.4  设置指令脉冲计数器值
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Cmd">指令位置脉冲数值</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_set_command(int AxisNo, int Cmd);

        
        /// <summary>
        /// 12.5  获取偏差计数器值
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="error">位置误差数值</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_get_error_counter(int AxisNo, out int error);

        
        /// <summary>
        /// 12.6  偏差计数器清零
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_reset_error_counter(int AxisNo);

        
        /// <summary>
        /// 12.7  设置通用计数器信号源以及数值
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="CntSrc">计数器数据源</param>
        ///          0:输出脉冲
        ///          1:EA/EB输入
        ///          2:PA/PB输入
        ///          3:基础时钟的1/2
        /// <param name="CntValue">计数器数值</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_set_general_counter(int AxisNo, int CntSrc,  double  CntValue);

        
        /// <summary>
        /// 12.8  获取通用计数器值
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="CntValue">计数器数值</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_get_general_counter(int AxisNo, out double CntValue);

        
        /// <summary>
        /// 12.9  设置锁存条件
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="LtcSrc">锁存条件</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_set_latch_source(int AxisNo, int LtcSrc);

        
        /// <summary>
        /// 12.10  获取锁存条件
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="LtcSrc">锁存条件</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_get_latch_source(int AxisNo, out int LtcSrc);

        
        /// <summary>
        /// 12.11  获取锁存数据
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="CounterNo">计数器号</param>
        /// <param name="Pos">锁存数据</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_get_latch_data(int AxisNo, int CounterNo, out double Pos);


        /// <summary>
        /// 12.12  设置锁存信号逻辑
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="LtcLogic">锁存信号逻辑</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_ltc_logic(int AxisNo, int LtcLogic);

        
        /// <summary>
        /// 12.13  获取锁存信号逻辑
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="LtcLogic">锁存信号逻辑</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_ltc_logic(int AxisNo, out int LtcLogic);


        /// <summary>
        /// 12.14  取消所有预置寄存器中的值
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_reset_pre_register(int AxisNo);

        #endregion

        #region    13.多轴同步操作                                                       */

        /// <summary>
        /// 13.1  设置为T形速度曲线 多轴同步 相对运动
        /// </summary>
        /// <param name="TotalAxes">同步运动轴总数</param>
        /// <param name="AxisArray">运动轴数组</param>
        /// <param name="DistA">运动距离</param>
        /// <param name="StrVelA">启始速度</param>
        /// <param name="MaxVelA">最大速度</param>
        /// <param name="TaccA">加速时间</param>
        /// <param name="TdecA">减速时间</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_set_tr_move_all(int TotalAxes, int AxisArray,double DistA, 
                                                        double StrVelA , double MaxVelA, 
                                                        double TaccA, double TdecA);

        
        /// <summary>
        /// 13.2  设置为T形速度曲线 多轴同步 绝对运动
        /// </summary>
        /// <param name="TotalAxes">同步运动轴总数</param>
        /// <param name="AxisArray">运动轴数组</param>
        /// <param name="PosA">目标点坐标</param>
        /// <param name="StrVelA">启始速度</param>
        /// <param name="MaxVelA">最大速度</param>
        /// <param name="TaccA">加速时间</param>
        /// <param name="TdecA">减速时间</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_ta_move_all(int TotalAxes,  int AxisArray,  double PosA,
                                                     double StrVelA,  double MaxVelA,  double TaccA,  double TdecA);

        
        /// <summary>
        /// 13.3  设置为S形速度曲线 多轴同步 相对运动
        /// </summary>
        /// <param name="TotalAxis">同步运动轴总数</param>
        /// <param name="AxisArray">运动轴数组</param>
        /// <param name="DistA">运动距离</param>
        /// <param name="StrVelA">启始速度</param>
        /// <param name="MaxVelA">最大速度</param>
        /// <param name="TaccA">加速时间</param>
        /// <param name="TdecA">减速时间</param>
        /// <param name="SVaccA">加速S曲线范围[pps]</param>
        /// <param name="SVdecA">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_set_sr_move_all(int TotalAxis,  int AxisArray,  double DistA, 
                                                     double StrVelA,  double MaxVelA,  double TaccA,  double TdecA,
                                                     double SVaccA,  double SVdecA);

        
        /// <summary>
        /// 13.4  设置为S形速度曲线 多轴同步 绝对运动
        /// </summary>
        /// <param name="TotalAxis">同步运动轴总数</param>
        /// <param name="AxisArray">运动轴数组</param>
        /// <param name="PosA">目标点坐标</param>
        /// <param name="StrVelA">启始速度</param>
        /// <param name="MaxVelA">最大速度</param>
        /// <param name="TaccA">加速时间</param>
        /// <param name="TdecA">减速时间</param>
        /// <param name="SVaccA">加速S曲线范围[pps]</param>
        /// <param name="SVdecA">减速S曲线范围[pps]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_sa_move_all(int TotalAxis,  int AxisArray,  double PosA,
                                                     double StrVelA,  double MaxVelA,  double TaccA,  double TdecA,
                                                     double SVaccA,  double SVdecA);
        

        /// <summary>
        /// 13.5  开始多轴同步运动
        /// </summary>
        /// <param name="FirstAxisNo">首轴编号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_start_move_all(int FirstAxisNo);

        
        /// <summary>
        /// 13.6  多轴同步停止
        /// </summary>
        /// <param name="FirstAxisNo">首轴编号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_stop_move_all(int FirstAxisNo);

        #endregion

        #region   14.多功能输入输出信号                                                */

        /// <summary>
        /// 14.1  设置4路多功能输出状态
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="DoValue">多功能输出通道号</param>
        ///      bit0: DO0
        ///      bit1: DO1
        ///      bit2: DO2
        ///      bit3: DO3
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_multi_DO(int CardId, int DoValue);
        
        
        /// <summary>
        /// 14.2  获取4路多功能输出状态
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="DoValue">多功能输出通道号</param>
        ///      bit0: DO0
        ///      bit1: DO1
        ///      bit2: DO2
        ///      bit3: DO3
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_get_multi_DO(int CardId, out int DoValue);

        
        /// <summary>
        /// 14.3  设置4路多功能输出信号的功能
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="Channel">输出通道号</param>
        /// <param name="SelectFunc">功能选项：</param>
        ///      0: 通用输出
        ///      1: 时间触发脉冲
        ///      2: 手动触发脉冲
        ///      3: 位置比较触发
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_multi_DO_func(int CardId, int Channel, int SelectFunc);

        
        /// <summary>
        /// 14.4  获取4路多功能输出信号的功能
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="Channel">输出通道号</param>
        /// <param name="SelectFunc">功能选项：</param>
        ///      0: 通用输出
        ///      1: 时间触发脉冲
        ///      2: 手动触发脉冲
        ///      3: 位置比较触发
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_multi_DO_func(int CardId, int Channel, out int SelectFunc);

        
        /// <summary>
        /// 14.5  获取4路多功能输入的状态
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="DiValue">多功能输入通道号</param>
        ///       bit0: DI0
        ///       bit1: DI1
        ///       bit2: DI2
        ///       bit3: DI3
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_get_multi_DI(int CardId, out int DiValue);
        
        
        /// <summary>
        /// 14.6  设置4路多功能输入信号的功能
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="Channel">输入通道号</param>
        /// <param name="Select">功能选项：</param>
        ///      0: general input
        ///      1: SD
        ///      2: CLR
        ///      3: LTC
        ///      4: EMG
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_multi_DI_func(int CardId, int Channel, int Select);

        
        /// <summary>
        /// 14.7  获取4路多功能输入信号的功能
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="Channel">输入通道号</param>
        /// <param name="SelectFunc">功能选项：</param>
        ///       0: general input
        ///       1: SD
        ///       2: CLR
        ///       3: LTC
        ///       4: EMG
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_multi_DI_func(int CardId, int Channel, out int SelectFunc);

        #endregion

        #region   15.软极限                                                            */

        /// <summary>
        /// 15.1  关闭软极限功能
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_disable_soft_limit(int AxisNo);

       
        /// <summary>
        /// 15.2  开启软极限功能
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Action">处理方式:</param>
        ///         0:只产生中断
        ///         1:立即停止
        ///         2:减速停止
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_enable_soft_limit(int AxisNo, int Action);

       
        /// <summary>
        /// 15.3  设置软极限数值
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="PlusLimit">正限位</param>
        /// <param name="MinusLimit">负限位</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_set_soft_limit(int AxisNo, int PlusLimit, int MinusLimit);

        
        /// <summary>
        /// 15.4  查询软限位
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="PlusLimit_sts">正限位</param>
        /// <param name="MinusLimit_sts">负限位</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_check_soft_limit(int AxisNo, out int PlusLimit_sts, out int MinusLimit_sts);

        #endregion

        #region   16.背隙补偿与震动抑制                                                */

        /// <summary>
        /// 16.1  背隙补偿
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="CompPulse">补偿脉冲</param>
        /// <param name="Mode">模式</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_backlash_comp(int AxisNo, int CompPulse, int Mode);

        
        /// <summary>
        /// 16.2  震动抑制
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="ReverseTime">反向脉冲延迟时间, 0-65535 单位 1.6us</param>
        /// <param name="ForwardTime">正向脉冲延迟时间, 0-65535 单位 1.6us</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_suppress_vibration(int AxisNo, int ReverseTime, int ForwardTime);

        
        /// <summary>
        /// 16.3  设置补偿速度
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="FA_Speed">补偿速度</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_set_fa_speed(int AxisNo,  double  FA_Speed);

        #endregion

        #region     17.扩展通用输入输出信号                                              */

        /// <summary>
        /// 17.1  获取16通道扩展 输入端口 的状态
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="map">bit[0]~bit[15]分别表示16个扩展输入的状态</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_GDI(int CardId, out int map);


        /// <summary>
        /// 17.2  设置16通道扩展 输入端口 的逻辑
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="map">bit[0]~bit[15]分别表示16个扩展输入的逻辑</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_GDI_logic(int CardId, int map);

        
        /// <summary>
        /// 17.3  获取16通道扩展 输入端口 的逻辑
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="map">bit[0]~bit[15]分别表示16个扩展输入的逻辑</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_GDI_logic(int CardId, out int map);

        
        /// <summary>
        /// 17.4  设置16通道扩展 输出端口 的状态
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="map">16输出通道号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_GDO(int CardId, int map);

        
        /// <summary>
        /// 17.5  获取16通道扩展  输出端口 的状态
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="map">bit[0]~bit[15]分别表示16个扩展输出的状态</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_GDO(int CardId, out int map);


        /// <summary>
        /// 17.6  设置16通道扩展 输出端口 的逻辑
        /// </summary>
        /// <param name="CardId">卡号 </param>
        /// <param name="map">bit[0]~bit[15]分别表示16个扩展输出的逻辑</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_GDO_logic(int CardId, int map);


        /// <summary>
        /// 17.7  获取16通道扩展 输出端口 的逻辑
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="map">bit[0]~bit[15]分别表示16个扩展输出的逻辑</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_GDO_logic(int CardId, out int map);

        #endregion

        #region     18.连续运动                                                          */
            
        /// <summary>
        /// 18.1  启动两轴平面连续运动
        /// </summary>
        /// <param name="AxisNo1">参与平面连续运动的第一个轴</param>
        /// <param name="AxisNo2">参与平面连续运动的第二个轴</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_enable_continue_move_plane(int AxisNo1, int AxisNo2);

        
        /// <summary>
        /// 18.2  4轴连续直线运动
        /// 不需要运动的轴,请将该轴的运动距离设为0
        /// </summary>
        /// <param name="CardId">参与4轴连续直线运动的板卡编号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_enable_continue_move_line(int CardId);


        /// <summary>
        /// 18.3  停止系统内的连续运动
        /// </summary>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_disable_contiue_move();

        
        /// <summary>
        /// 18.4  获取命令缓存还能存入多少行命令
        /// </summary>
        /// <param name="length">命令缓存剩余空间</param>
        /// <param name="status">连续运动的状态:</param>
        ///     0: 连续运动完成或未开始
        ///     1: 正在进行连续运动
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_check_continuous_buffer(out int length, out int status);

        
        /// <summary>
        /// 18.5  获取当前执行的命令在命令缓存中的编号
        /// </summary>
        /// <returns>当前命令执行的在缓存中的编号</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_continuous_index();

        
        /// <summary>
        /// 18.6  暂停运动并延时
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="milliSecond">暂停时间[ms]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_dwell_move(int AxisNo,  double  milliSecond);

        #endregion

        #region     19.位置比较触发                                                      */
        
        /// <summary>
        /// 19.1  设置FIFO触发
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Type">0脉冲,1电平</param>
        /// <param name="Logic">脉冲逻辑</param>
        /// <param name="Width">脉宽</param>
        /// <param name="Interrupt">是否同步发送中断</param>
        /// <param name="DoNo">DO号[0,1,2,3]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_set_pos_compare_trigger(int AxisNo, int Type,int Logic, 
                                                                int Width, int Interrupt, int DoNo);

        
        /// <summary>
        /// 19.2  获取FIFO状态
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Status">FIFO当前状态:</param>
        ///     bit[0]~bit[2]:编码器加计数溢出次数
        ///     bit[3]~bit[5]:编码器减计数溢出次数
        ///     bit[6]:编码器加计数溢出错误
        ///     bit[7]:编码器减计数溢出错误
        ///     bit[8]~bit[15]:Fifo内数据个数
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_FIFO_status(int AxisNo, out int Status);

        
        /// <summary>
        /// 19.3  向FIFO中添加比较点
        /// </summary>
        /// <param name="AxisNo"> 轴号</param>
        /// <param name="Pos">比较点</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_add_FIFO(int AxisNo, int Pos);


        /// <summary>
        /// 19.4  删除全部比较点数据
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_clear_FIFO(int AxisNo);


        /// <summary>
        /// 19.5  获取触发脉冲数量
        /// </summary>
        /// <param name="CardID">卡号</param>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Count">Trigger计数</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_trigger_count(int CardID,int AxisNo, out int Count);

        
        /// <summary>
        /// 19.6  清除trigger计数
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="ClearB">清零标记：</param>
        ///     bit[0]清除通道1
        ///     bit[1]清除通道2
        ///     bit[2]清除通道3
        ///     bit[3]清除通道4
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_clear_trigger_count(int CardId, int ClearB);


        /// <summary>
        /// 19.7  获取比较触发的数据源计数
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <param name="Count">Trigger计数</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_pos_compare_source(int AxisNo, out int Count);


        /// <summary>
        /// 19.8  清除比较触发的数据源计数
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_clear_pos_compare_source(int AxisNo);      
        
        
        /// <summary>
        /// 19.9  指定端口发送脉冲
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="Logic">脉冲逻辑</param>
        /// <param name="Width">脉宽</param>
        /// <param name="Interrupt">是否同步发送中断</param>
        /// <param name="DoNo">DO号[0,1,2,3]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_set_manul_trigger(int CardId, int Logic, int Width, int Interrupt, int DoNo);
              
        
        /// <summary>
        /// 19.10  指定端口以固定时间间隔发送脉冲
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="Logic">脉冲逻辑</param>
        /// <param name="Width">脉宽</param>
        /// <param name="Interval">脉冲时间间隔</param>
        /// <param name="Interrupt">是否同步发送中断</param>
        /// <param name="DoNo">DO号[0,1,2,3]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_enable_timer_trigger(int CardId, int Logic, int Width, 
                                                            int Interval, int Interrupt, int DoNo);
        
        /// <summary>
        /// 19.11  关闭固定时间间隔脉冲发送
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="DoNo">DO号[0,1,2,3]</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_disable_timer_trigger(int CardId, int DoNo);

        #endregion

        #region     20.错误日志                                                          */
        /// <summary>
        /// 20.1  开启TRACE功能
        /// </summary>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_enable_log();

        /// <summary>
        /// 20.2  关闭TRACE功能
        /// </summary>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
         public static extern int jmc_mc1104_disable_log();

        
        /// <summary>
        /// 20.3  获取最后发生的错误
        /// </summary>
        /// <param name="err">错误描述</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_last_error(byte[]  err);

        /// <summary>
        /// 20.3  获取最后发生的错误
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="err">错误描述</param>
        /// <returns>0: normal</returns>
        public static int jmc_mc1104_get_last_error(int CardId, out string err)
        {
            err = "";
            byte[] byteerr = new byte[1024];
            int ret = jmc_mc1104_get_last_error(byteerr);
            if (ret != 0)  {  return ret;  }
            int index = Array.IndexOf(byteerr, (byte)0);
            err = Encoding.ASCII.GetString(byteerr, 0, index);
            return 0;
        }


        /// <summary>
        /// 20.4  清除错误标志
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_clear_error(int AxisNo);


        /// <summary>
        /// 20.5  清除事件信息
        /// </summary>
        /// <param name="AxisNo">轴号</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_clear_event(int AxisNo);

        
        /// <summary>
        /// 20.6  控制板卡指示灯
        /// </summary>
        /// <param name="CardId">卡号</param>
        /// <param name="value">指示灯状态,0=灭,1=亮</param>
        /// <returns>0: normal</returns>
        [DllImport("MC1104.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int jmc_mc1104_ctrl_light(int CardId, uint value);  


        /// <summary>
        /// 20.7  返回系统状态
        /// </summary>
        /// <returns>系统状态</returns>
        [DllImport("MC1104.dll",CallingConvention=CallingConvention.StdCall)]
        public static extern int jmc_mc1104_get_sys_state();

        #endregion

        #region     21.数据类型
        /// <summary>
        /// 根据Int类型的值，返回用1或0(对应True或Flase)填充的数组
        /// <remarks>从右侧开始向左索引(0~31)</remarks>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<bool> GetBitList(int value)
        {
            var list = new List<bool>(32);
            for (var i = 0; i <= 31; i++)
            {
                var val = 1 << i;
                list.Add((value & val) == val);
            }
            return list;
        }

        /// <summary>
        /// 返回Int数据中某一位是否为1
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index">32位数据的从右向左的偏移位索引(0~31)</param>
        /// <returns>true表示该位为1，false表示该位为0</returns>
        public static bool GetBitValue(int value, ushort index)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index"); //索引出错
            var val = 1 << index;
            return (value & val) == val;
        }

        /// <summary>
        /// 设定Int数据中某一位的值
        /// </summary>
        /// <param name="value">位设定前的值</param>
        /// <param name="index">32位数据的从右向左的偏移位索引(0~31)</param>
        /// <param name="bitValue">true设该位为1,false设为0</param>
        /// <returns>返回位设定后的值</returns>
        public static int SetBitValue(int value, ushort index, bool bitValue)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index"); //索引出错
            var val = 1 << index;
            return bitValue ? (value | val) : (value & ~val);
        }
        #endregion

    }
    /// <summary>
    /// MC1104卡报警信息
    /// </summary>
    //public static class MC1104_ErrCode
    //{
    //    public static Dictionary<int ,string> ErrCode=new Dictionary<int,string>();

    //    public  MC1104_ErrCode()
    //    {
    //        ErrCode.Add(0, "OK");

    //        ErrCode.Add(-10000, "Parameter_Error");
    //        ErrCode.Add(-10001, "TimeOut_Error");

    //        ErrCode.Add(-11000, "Card_Not_Found");
    //        ErrCode.Add(-11001, "CardId_Error");
    //        ErrCode.Add(-11002, "CardId_Repeated");
    //        ErrCode.Add(-11003, "ILLEGAL_Call");
    //        ErrCode.Add(-11004, "Continue_Move");
    //        ErrCode.Add(-11005, "Thread_Stop");
    //        ErrCode.Add(-11006, "Thread_Start");
    //        ErrCode.Add(-11007, "Read_File  ");
    //        ErrCode.Add(-11008, "Buffer_Full ");
    //        ErrCode.Add(-12000, "PCI_BUS_Error");

    //        ErrCode.Add(-13000, "Write_Register_Error");

    //    }
    //}

}
