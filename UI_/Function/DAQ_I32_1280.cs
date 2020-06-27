using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace Justech 
{
    unsafe class DAQ_I32
    {

        //**************************************************^**************************************************
        // 函数名: DAQ_Sys_initial
        // 返回值: DAQ_I32[OK/ERROR]
        // 输入参数:
        // 输出参数:
        //		card_id: 返回系统上控制卡存在位置.
        // 说明:
        // 例如系统内一共存在三块板卡,拨码分别为0,1,2,则CardID_InBit的值应为0x7
        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_Sys_Initial(ref int card_id);
    
        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_Sys_Terminate();

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_Sys_VerGet(DAQ_TYPE type, int card_id, ref uint ver_sw, ref uint sw_date, ref uint ver_hw, ref uint hw_date);
//-------------------------------------------GDO--------------------------------------------------------------------
        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_GDO_Set(int card_id, uint ctrl_bit);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_GDO_Get(int card_id, ref uint ctrl_bit);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_GDO_Cfg(int card_id, uint logic_bit);
 //-------------------------------------------GDI--------------------------------------------------------------------
        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_GDI_Get(int card_id, ref uint ctrl_bit);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_FIT_Cfg(int card_id, uint filter_time);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_GDI_Cfg(int card_id,  uint logic_bit);
 //----------------------------------------------ADC--------------------------------------------

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_ADC_Cfg(int card_id, int gain, int is_diff);


        //[DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        //public static extern int DAQ_ADC_Get(int card_id, int adc_id, ref double vol);
 //---------------------------扩展------------------------------------------------------
        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_EXT_ReadAddr(int card_id, int addr, ref int data);




        

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_Mul_Sys_Initial(UInt32[] card_info);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_Mul_GDO_Cfg_Get(DAQ_TYPE card_type, int card_id, ref uint logic_bit);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_Mul_GDO_Cfg(DAQ_TYPE card_type, int card_id, uint logic_bit);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_Mul_GDO_Set(DAQ_TYPE card_type, int card_id, uint ctrl_bit);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_Mul_GDO_Get(DAQ_TYPE card_type, int card_id, ref uint ctrl_bit);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_Mul_GDI_Cfg_Get(DAQ_TYPE card_type, int card_id, ref uint logic_bit);//???

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_Mul_GDI_Cfg(DAQ_TYPE card_type, int card_id, uint logic_bit);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_Mul_FIT_Cfg(DAQ_TYPE card_type, int card_id, uint filter_time);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_Mul_GDI_Get(DAQ_TYPE card_type, int card_id, ref uint ctrl_bit);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_ADC_CalStart(DAQ_TYPE card_type, int card_id, int gain, int is_diff);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_ADC_CalStaus(DAQ_TYPE card_type, int card_id, ref int doing, ref int done);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_ADC_CalStop(DAQ_TYPE card_type, int card_id);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_ADC_CalParamsGet(DAQ_TYPE card_type, int card_id, ref int gain, ref int mode,ref double min, ref double max, ref double offset);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_ADC_Get(DAQ_TYPE card_type, int card_id, int adc_id, ref double vol);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_DAC_CTRL(DAQ_TYPE card_type, int card_id, int dac_id, double vol);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_ADC_ContReadVal(DAQ_TYPE card_type, int card_id, uint dac_id, uint size, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1000)]uint[] data);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_ADC_ContReadAvgVal(DAQ_TYPE card_type, int card_id, uint dac_id, uint size, ref double avg_vol);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_ADC_ScaleVal(DAQ_TYPE card_type, int card_id, uint read_val, ref double conv_val);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_EXT_ReadAddr(DAQ_TYPE card_type, int card_id, uint addr, ref uint data);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_EXT_WriteAddr(DAQ_TYPE card_type, int card_id, uint addr, uint data);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_EXT_LogCtrl(int enabled);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DAQ_ADC_CardInformation(DAQ_TYPE card_type, int card_id, ref int is_calculate, ref DAQ_VALTAGE_RANGE valtage_range, ref DAQ_CHANNEL_MODE channel_mode);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]

        public static extern int DAQ_ADC_SetChannelMode(DAQ_TYPE card_type, int card_id, DAQ_CHANNEL_MODE channel_mode);

        [DllImport("JDAQ.dll", CallingConvention = CallingConvention.StdCall)]

        public static extern int DAQ_ADC_SetValtageRange(DAQ_TYPE card_type, int card_id, DAQ_VALTAGE_RANGE valtage_range);


        public enum DAQ_TYPE
        {
            DAQ_MIN = -1,
            DAQ_D2424,
            DAQ_D3232,
            DAQ_A3202,
            DAQ_MAX
        }

        public enum DAQ_VALTAGE_RANGE
        {
            DAQ_VALTAGE_RANGE_MIN = -1,
            DAQ_VALTAGE_RANGE_20,    // +/-10v
            DAQ_VALTAGE_RANGE_10,    // +/-5v
            DAQ_VALTAGE_RANGE_5,     // +/-2.5v
            DAQ_VALTAGE_RANGE_2_5,   // +/-1.25v
            DAQ_VALTAGE_RANGE_MAX
        }

        public enum DAQ_CHANNEL_MODE
        {
            DAQ_CHANNEL_MODE_MIN = -1,
            DAQ_CHANNEL_MODE_SINGENDED,     // 单端
            DAQ_CHANNEL_MODE_DIFFERENCE,    // 差分
            DAQ_CHANNEL_MODE_MAX
        }

    }
}
