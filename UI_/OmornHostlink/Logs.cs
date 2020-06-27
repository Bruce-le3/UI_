using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Logs
{
    class Log
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DirectoryFileName">文件夹路径</param>
        public static void Directory_Create(string DirectoryFileName)
        {
            if (!Directory.Exists(DirectoryFileName))
            {
                Directory.CreateDirectory(DirectoryFileName);

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strLogFileName">文件路径名称</param>
        /// <param name="MSG">写入信息</param>
        public static void Log_Set(string strLogFileName,string MSG)
        {
            string stringformat=".log";
            string DirectoryFileName = Application.StartupPath + "\\Logs\\";
            string filename = DirectoryFileName + strLogFileName + stringformat;
            byte[] mSG_data = Encoding.UTF8.GetBytes(System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:ffff") + " [1] INFO " +"- "+ MSG + "\r\n");
           

       
            Directory_Create(DirectoryFileName);  //创建文件夹
            if (!File.Exists(filename))
            {
                FileStream fs = new FileStream(filename, FileMode.CreateNew);
                fs.Close();
            }

            FileStream fsw = new FileStream(filename, FileMode.Append);
            fsw.Write(mSG_data, 0, mSG_data.Length);
          
            fsw.Close();

        }



    }
}





