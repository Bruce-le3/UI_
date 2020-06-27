using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ControlIni;

namespace Justech 
{  
    public class GClsMethod 
    {
        string paraRecordFilePath = @"D:\DATA\参数信息记录.CSV";//记录参数修改内容文件路径
        /// <summary>
        /// 记录参数修改时间及内容
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="oldVal"></param>
        /// <param name="newVal"></param>
        public void RecordParaModification(string paraName, string oldVal, string newVal)//记录参数修改时间及内容
        {
            Directory.CreateDirectory(@"D:\DATA");
            string str = string.Empty;
            if (!File.Exists(paraRecordFilePath))
            {
                str = "Time" + "," + "Name" + "," + "Content";
                File.AppendAllText(paraRecordFilePath, str + "\n", Encoding.UTF8);
            }
            str = DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日" + DateTime.Now.ToLongTimeString() + ",  " + paraName + "," + oldVal
                + "   修改为   " + newVal;
            File.AppendAllText(paraRecordFilePath, str + "\n", Encoding.UTF8);
        }
        public static object lockLog = new object();
        public void LogRecord(string infoLog)//写log文件
        {
            lock(lockLog)
            {
                Directory.CreateDirectory(@"D:\DATA");
                string data = DateTime.Now.ToString("yyyyMMdd");//获得当前时间
                string logFilePath = @"D:\DATA" + "\\" + data + "Log" + ".txt";
                string strContentTime = DateTime.Now.ToString();
                File.AppendAllText(logFilePath, strContentTime + "，" + infoLog + "\r\n", Encoding.UTF8);
            }     
        }
        public static object lockAlm = new object();
        public void AlmRecord(string infoAlm)//写Alm文件
        {
            lock (lockAlm)
            {
                string data = DateTime.Now.ToString("yyyyMMdd");//获得当前时间
                string logFilePath = @"D:\DATA" + "\\" + data + "Alm" + ".txt";
                string strContentTime = DateTime.Now.ToString();
                File.AppendAllText(logFilePath, strContentTime + "," + infoAlm + "\r\n", Encoding.UTF8);
            }
        }
        /// <summary>
        /// 将结果写入Excel表格
        /// </summary>
        /// <param name="resultFinalData">数据</param>
        /// <param name="resultOKorNG">判断结果</param>   
        Ini iniResult = new Ini();
        public string resultData = string.Empty;//写结果时间
        public string savePath;//写结果路径
        string resultTitleName = string.Empty;
        string resultTitleUCL = string.Empty;
        string resultTitleLCL = string.Empty;
        public void RecordResultToExcel(string resultFinalData, string resultOKorNG, string configsn)//将结果写入Excel表格
        {
            try
            {
                resultData = DateTime.Now.ToString("yyyyMMdd");
                savePath = @"D:\DATA" + "\\" + resultData + ".CSV";//数据保存路径                 
                if (!File.Exists(savePath))
                {
                    string resultTitleData = "TIME,PRO.ID",strUCL =",UCL",strLCL=",LCL";
                    iniResult.filePath = CAMiClsVariable.resultIniPath;
                    int resultCount=0;
                    if (CAMiClsVariable.strVision == "COGNEX" && CAMiClsVariable.strProduct == "P104")//Cognex
                    {
                         resultCount = Convert.ToInt32(iniResult.ReadIni("Count", "sum_P104"));
                    }
                    else
                    {
                         resultCount = Convert.ToInt32(iniResult.ReadIni("Count", "sum_D4X"));
                    }

                    for (int i = 0; i < resultCount; i++ )
                    {
                        if (CAMiClsVariable.strVision == "COGNEX" && CAMiClsVariable.strProduct == "P104")//Cognex
                        {
                            resultTitleData += "," + iniResult.ReadIni("结果设置Cognex_P104", (i + 1).ToString());
                        }
                        else if (CAMiClsVariable.strVision == "COGNEX"&&CAMiClsVariable.strProduct=="D4X")
                        {
                            resultTitleData += "," + iniResult.ReadIni("结果设置Cognex_D4X", (i + 1).ToString());
                        }
                        else if (CAMiClsVariable.strVision == "KEYENCE")//Keyence
                        {
                            resultTitleData += "," + iniResult.ReadIni("结果设置Keyence", (i + 1).ToString());
                        }
                        strUCL += "," + iniResult.ReadIni("上限", (i + 1).ToString());
                        strLCL += "," + iniResult.ReadIni("下限", (i + 1).ToString());
                    }
                    resultTitleName = resultTitleData +",FinalResult";
                    resultTitleUCL = strUCL;
                    resultTitleLCL = strLCL;
                    File.AppendAllText(savePath, resultTitleName + "\n", Encoding.UTF8);
                    File.AppendAllText(savePath, resultTitleUCL + "\n", Encoding.UTF8);
                    File.AppendAllText(savePath, resultTitleLCL + "\n", Encoding.UTF8);
                }
                resultTitleName = DateTime.Now.ToString();
                //判断结果OK 还是 NG
                resultTitleName += "," + resultFinalData + configsn +"," + resultOKorNG;
                File.AppendAllText(savePath, resultTitleName + "," + "\n", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                LogRecord(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// listbox显示运行信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="lstbRunningInfo"></param>
        //string logEnabled;
        public delegate void ListBoxAddInfo(string info, ListBox lstbRunningInfo);
        private void ListBoxDisplay(string info,ListBox lstbRunningInfo)//listbox显示运行信息
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            if (lstbRunningInfo.InvokeRequired)
            {
                ListBoxAddInfo lb = new ListBoxAddInfo(ListBoxDisplay);
                lstbRunningInfo.Invoke(lb, new string[] { info });
            }
            else
            {
                lstbRunningInfo.Items.Add(currentDate + "   " + info);
                lstbRunningInfo.SelectedIndex = lstbRunningInfo.Items.Count - 1;
                if (lstbRunningInfo.Items.Count > 150)
                {
                    lstbRunningInfo.Items.Clear();
                }
                //logEnabled = myPasswordIni.IniReadValue("参数设置", "LogRecordEnable");//读取logEnabled
                //if (logEnabled.Equals("True"))
                //{
                //    myMethodToWriteLog.LogRecord(info);//将信息写入Log文件
                //}
            }
        }
    }
}
