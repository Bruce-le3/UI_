using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;


namespace Justech
{
    public class CAMiClsBali
    {
        public static string[] arrayCycloneSN = new string[12];//存放SN数组
        public static string[] arrayFlexSN = new string[12];
        public static string[] arrayFlexSnToPict = new string[12];//用于存放图片
        public static string[] arrayFlex_Cyclone_X = new string[12];
        public static string[] arrayFlex_Cyclone_Y = new string[12];
        public static string[] arrayFlex_Cyclone_A = new string[12];
        public static string[] arrayCompound_1 = new string[12];
        public static string[] arrayCompound_2 = new string[12]; 
        public static string strCm_vendor = "1";
        public static string strCycle_Time = "";
        public static string strMachine_ID = "1001";
        public static string strOperator_ID = "1";
        public static string strMode = "0";
        public static string strTestSeriesID = "0";
        public static string strPriority = "0";
        public static string strOnline = "1";
        public static string strConnect = "";
        public static string strStart = "";
        public static string strUpLoadProuctNum = "";
        public static string PackageData = "";
        public static string PackageStart = "";       
        public static string recvData = "";//返回数据
        public static bool isSendOK = false;
        public static string upLoadPath = "D:\\UploadPDCAImage";
        public static string destLoadPath = "Z:\\blobs"; //"169.254.1.10\\Public\\blobs";//"D:\\UploadImage\\新建文件夹";
        public enum AELimitKeys
        {
            cm_vendor = 1,
            cycle_time,
            Flex_Cyclone_X,
            Flex_Cyclone_Y,
            Flex_Cyclone_A,
            pass_fail,
            coil_lead_shift,
            solder_scatter,
            cold_solder,
            UV_glue_overflow,
            UV_glue_insufficient,
            cient,
            UV_Glue_bubble,
            foreign_material,
            liner_scalding, 
            Compound_1,
            Compound_2,
            machine_ID,           
            Operator_ID,
            Mode,
            TestSeriesID,
            Priority,
            online ,   
            submit
        }
        public static void GetSNAndData()
        {
            try
            {
                if (CAMiClsVariable.isPDCAOpen)
                {
                    PackageData = null;
                    PackageStart = null;
                    strUpLoadProuctNum = null;
                    for (int i = 0; i < 12; i++)
                    {
                        if (CAMiClsVariable.isProductSelect[i] == 1)
                        {
                            if (CAMiClsVariable.strVision == "COGNEX" && CAMiClsVariable.strProduct == "P104")//Cognex
                            {
                                if (Main.frmMain.dgvData.Rows[15 + i].Cells[2].Value.ToString().Length != 16)
                                {
                                    arrayFlexSN[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[2].Value.ToString().Substring(0, 17);
                                    arrayFlexSnToPict[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[2].Value.ToString();
                                }
                                else
                                {
                                    arrayFlexSN[i] = "NoReadFlexSN";
                                    arrayFlexSnToPict[i] = "NoReadFlexSNToPicture";
                                }
                                arrayCycloneSN[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[3].Value.ToString();
                            }
                            else if (CAMiClsVariable.strVision == "COGNEX" && CAMiClsVariable.strProduct == "D4X")
                            {
                                if (Main.frmMain.dgvData.Rows[15 + i].Cells[9].Value.ToString().Length != 16)//实际为9列
                                {
                                    arrayFlexSN[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[9].Value.ToString().Substring(0, 17);//实际为9列
                                    arrayFlexSnToPict[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[9].Value.ToString();//实际为9列
                                    arrayFlex_Cyclone_X[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[4].Value.ToString();//X-Gap
                                    arrayFlex_Cyclone_Y[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[8].Value.ToString();//Y-Gap
                                    arrayFlex_Cyclone_A[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[5].Value.ToString();//A
                                    arrayCompound_1[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[7].Value.ToString();//Angle<-1deg&X≤23.55
                                    arrayCompound_2[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[6].Value.ToString();//Angle>1deg&X≥23.55
                                }
                                else
                                {
                                    arrayFlexSN[i] = "NoReadFlexSN";
                                    arrayFlexSnToPict[i] = "NoReadFlexSNToPicture";
                                }
                                arrayCycloneSN[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[10].Value.ToString();   //实际为10列                            
                            }
                            else if (CAMiClsVariable.strVision == "KEYENCE")
                            {
                                if (Main.frmMain.dgvData.Rows[15 + i].Cells[5].Value.ToString() != "1")
                                {
                                    arrayFlexSN[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[5].Value.ToString().Substring(0, 17);
                                    arrayFlexSnToPict[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[5].Value.ToString().Substring(0, 17);
                                }
                                else
                                {
                                    arrayFlexSN[i] = "NoReadFlexSN";
                                    arrayFlexSnToPict[i] = "NoReadFlexSNToPicture";
                                }
                                arrayCycloneSN[i] = Main.frmMain.dgvData.Rows[15 + i].Cells[1].Value.ToString();
                            }
                            if (Main.frmMain.dgvData.Rows[15 + i].Cells[9].Value.ToString().Length != 16)//实际为9列
                            {
                                strUpLoadProuctNum += (i + 1).ToString() + ",";
                                PackageStart += CollectStart(arrayFlexSN[i], strMode);//打包Start
                                PackageData += ConnectStr(arrayFlexSN[i], strCm_vendor, strCycle_Time, arrayFlex_Cyclone_X[i], arrayFlex_Cyclone_Y[i], arrayFlex_Cyclone_A[i],
                                                        CAMiClsVariable.productResultPDCA[i].ToString(), CAMiClsVariable.coil_lead_shift[i].ToString(), CAMiClsVariable.solder_scatter[i].ToString(), CAMiClsVariable.cold_solder[i].ToString(),
                                                       CAMiClsVariable.UV_glue_overflow[i].ToString(), CAMiClsVariable.UV_glue_insufficient[i].ToString(),CAMiClsVariable.cient[i].ToString(), CAMiClsVariable.UV_Glue_bubble[i].ToString(), CAMiClsVariable.foreign_material[i].ToString(),
                                                       CAMiClsVariable.liner_scalding[i].ToString(), arrayCompound_1[i], arrayCompound_2[i], strMachine_ID, strOperator_ID, strMode, strTestSeriesID, strPriority, strOnline);
                                //UpLoadPicture(arrayFlexSnToPict[i]);//上传图片
                                // logmsg(CAMiClsVariable.NG_ErrorCode[i].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public static void UpLoadPicture(string strPictSN)
        {
            if (CAMiClsVariable.strVision == "COGNEX")//Cognex
            {
                //upLoadPath = upLoadPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + strPictSN + ".zip";
                string sourceUpLoadPathT21 = upLoadPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + strPictSN + "_T21" + ".zip";
                string sourceUpLoadPathT22 = upLoadPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + strPictSN + "_T22" + ".zip";
                //destLoadPath=destLoadPath+"\\" + strPictSN + ".zip";
                //string destUpLoadPath = destLoadPath + "\\" + strPictSN + ".zip";
                try
                {
                    if (!Directory.Exists(destLoadPath + "\\" + strPictSN.Substring(0, 17)))
                    {
                        Directory.CreateDirectory(destLoadPath + "\\" + strPictSN.Substring(0, 17));
                        GFileOperate.CopyFile(sourceUpLoadPathT21, destLoadPath + "\\" + strPictSN.Substring(0, 17) + "\\" + strPictSN + "_T21" + ".zip");
                        GFileOperate.CopyFile(sourceUpLoadPathT22, destLoadPath + "\\" + strPictSN.Substring(0, 17) + "\\" + strPictSN + "_T22" + ".zip");
                    }
                    //GFileOperate.MoveFile(sourceUpLoadPath, destUpLoadPath);                                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else if (CAMiClsVariable.strVision == "KEYENCE")
            {
                //upLoadPath = upLoadPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + strPictSN + ".zip";
                string sourceUpLoadPath = "D:\\ImageUpLoadPDCA" + "\\" + strPictSN + ".zip";
                //destLoadPath=destLoadPath+"\\" + strPictSN + ".zip";
                string destUpLoadPath = destLoadPath + "\\" + strPictSN + ".zip";
                try
                {
                    if (!Directory.Exists(destLoadPath + "\\" + strPictSN))
                    {
                        Directory.CreateDirectory(destLoadPath + "\\" + strPictSN);
                        GFileOperate.CopyFile(sourceUpLoadPath, destLoadPath + "\\" + strPictSN + "\\" + strPictSN + ".zip");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        public static void SendPDCAData()
        {
            while (true)
            {
                if (isSendOK)
                {
                    if (CAMiClsVariable.isPDCAOpen)
                    {
                        //GetSN();
                        try
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                if (CAMiClsVariable.isProductSelect[i] == 1)
                                {
                                    UpLoadPicture(arrayFlexSnToPict[i]);//上传图片
                                    Thread.Sleep(200);//上传图片延时
                                }
                            }
                            Thread.Sleep(1000);//上传图片延时
                            logmsg("第" + strUpLoadProuctNum + "个产品数据");//记录几片产品的数据
                            PackageStart = "_{" + "\n" + PackageStart + "}";
                            PackageData = "_{" + "\n" + PackageData + "}";
                            CAMiClsVariable.cClient.ClientSend(3, PackageStart);
                            logmsg(PackageStart);
                            Thread.Sleep(200);
                            recvData = CAMiClsVariable.cClient.ClientReceive(3, 1);
                            logmsg(recvData);
                            Thread.Sleep(200);
                            CAMiClsVariable.cClient.ClientSend(3, PackageData);
                            Thread.Sleep(200);
                            logmsg(PackageData);                            
                            recvData = CAMiClsVariable.cClient.ClientReceive(3, 1);
                            Thread.Sleep(1000);
                            logmsg(recvData);
                            
                        }
                        catch (Exception exxx)
                        {
                            MessageBox.Show(exxx.ToString());
                        }
                    }
                    isSendOK = false;
                }
                Thread.Sleep(10);
                Application.DoEvents();
            }
        }
        public static string ConnectStr(string totalSN,string cmVendor, string cycleTime,string flex_cyclone_X,string flex_cyclone_Y,string flex_cyclone_A,
                                      string passFail,string coil_lead_shift, string solder_scatter,string cold_solder,string UV_glue_overflow,string UV_glue_insufficient, string cient,string UV_Glue_bubble,
                                      string foreign_material, string liner_scalding, string Compound_1, string Compound_2,string machineID, string operatorID, string mode, string testSeriesID, string priority, string online)      
        {
            if (CAMiClsVariable.strProductD5X == "D53")   ///////////////////D43 Bali上传内容 /////////////////////
            {
                strConnect = totalSN + "@pdata@" + AELimitKeys.cm_vendor + "@" + cmVendor + "@NA@NA@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.cycle_time + "@" + cycleTime + "@NA@NA@s" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Flex_Cyclone_X + "@" + flex_cyclone_X + "@23.05@24.05@mm" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Flex_Cyclone_Y + "@" + flex_cyclone_Y + "@20.07@21.07@mm" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Flex_Cyclone_A + "@" + flex_cyclone_A + "@-1.8@1.8@Deg" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.pass_fail + "@" + passFail + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.coil_lead_shift + "@" + coil_lead_shift + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.solder_scatter + "@" + solder_scatter + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.cold_solder + "@" + cold_solder + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.UV_glue_overflow + "@" + UV_glue_overflow + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.UV_glue_insufficient + "@" + UV_glue_insufficient + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.cient + "@" + cient + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.UV_Glue_bubble + "@" + UV_Glue_bubble + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.foreign_material + "@" + foreign_material + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.liner_scalding + "@" + liner_scalding + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Compound_1 + "@" + Compound_1 + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Compound_2 + "@" + Compound_2 + "@0@0@NA" + "\n"
                                         + totalSN + "@attr@"  + AELimitKeys.machine_ID + "@" + machineID + "@NA@NA@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Operator_ID + "@" + operatorID + "@NA@NA@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Mode + "@" + mode + "@NA@NA@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.TestSeriesID + "@" + testSeriesID + "@NA@NA@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Priority + "@" + priority + "@NA@NA@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.online + "@" + online + "@NA@NA@NA" + "\n"
                                         + totalSN + "@" + AELimitKeys.submit + "@" + "JST-V1.00-32.33-NN.NN-NN.NN-v1.02" + "\n";
            }
            else                                    /////////////////// D42 Bali上传内容 /////////////////////
            {
            strConnect = totalSN + "@pdata@" + AELimitKeys.cm_vendor + "@" + cmVendor + "@NA@NA@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.cycle_time + "@" + cycleTime + "@NA@NA@s" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Flex_Cyclone_X + "@" + flex_cyclone_X + "@23.05@24.05@mm" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Flex_Cyclone_Y + "@" + flex_cyclone_Y + "@20.07@21.07@mm" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Flex_Cyclone_A + "@" + flex_cyclone_A + "@-2@2@Deg" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.pass_fail + "@" + passFail + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.coil_lead_shift + "@" + coil_lead_shift + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.solder_scatter + "@" + solder_scatter + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.cold_solder + "@" + cold_solder + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.UV_glue_overflow + "@" + UV_glue_overflow + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.UV_glue_insufficient + "@" + UV_glue_insufficient + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.cient + "@" + cient + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.UV_Glue_bubble + "@" + UV_Glue_bubble + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.foreign_material + "@" + foreign_material + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.liner_scalding + "@" + liner_scalding + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Compound_1 + "@" + Compound_1 + "@0@0@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Compound_2 + "@" + Compound_2 + "@0@0@NA" + "\n"
                                         + totalSN + "@attr@"  + AELimitKeys.machine_ID + "@" + machineID + "@NA@NA@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Operator_ID + "@" + operatorID + "@NA@NA@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Mode + "@" + mode + "@NA@NA@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.TestSeriesID + "@" + testSeriesID + "@NA@NA@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.Priority + "@" + priority + "@NA@NA@NA" + "\n"
                                         + totalSN + "@pdata@" + AELimitKeys.online + "@" + online + "@NA@NA@NA" + "\n"
                                         + totalSN + "@" + AELimitKeys.submit + "@" + "JST-V1.00-32.33-NN.NN-NN.NN-v1.02" + "\n";
            }
            return strConnect;
        }
        public static string CollectStart(string totalSN, string strMode)
        {
            if (Convert.ToInt16(strMode) == 3)
            {
                strStart = totalSN + "@start" + "@audit" + "\n";
            }
            else
            {
                strStart = totalSN + "@start" + "\n";
            }
            return strStart;
        }
        public static void logmsg(string msg)   //LOG方法
        {
            try
            {
                string data = DateTime.Now.ToString("yyyyMMdd");//获得当前时间
                string logFilePath = @"D:\DATA" + "\\" + data + "BaliLog" + ".txt";
                if (!File.Exists(logFilePath))//判断日志文件是否为当天
                    File.Create(logFilePath).Close();//创建文件
                StreamWriter writer = File.AppendText(logFilePath);//文件中添加文件流
                writer.WriteLine("");
                writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + msg);
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                string data = DateTime.Now.ToString("yyyyMMdd");//获得当前时间
                string logFilePath = @"D:\DATA" + "\\" + data + "BaliLog" + ".txt";
                if (!File.Exists(logFilePath))//判断日志文件是否为当天
                    File.Create(logFilePath).Close();//创建文件
                StreamWriter writer = File.AppendText(logFilePath);
                writer.WriteLine("");
                writer.WriteLine(DateTime.Now.ToString("日志记录错误HH:mm:ss") + " " + e.Message + " " + msg);
                writer.Flush();
                writer.Close();
            }
        }       
    }
}
