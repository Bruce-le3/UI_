using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Drawing.Imaging;
using System.IO.Ports;
using System.Configuration;
using Microsoft.VisualBasic;

namespace Justech
{
    class csv_err_write
    {
        public static void csv_err(string value)
        {

            string[] filedatatime = new string[4];
            filedatatime[0] = System.DateTime.Today.Year.ToString();
            filedatatime[1] = System.DateTime.Today.Month.ToString().PadLeft(2, '0');
            filedatatime[2] = System.DateTime.Today.Day.ToString().PadLeft(2, '0');
            filedatatime[3] = "-";
            string filedatas = filedatatime[0] + filedatatime[3] + filedatatime[1] + filedatatime[3] + filedatatime[2] + filedatatime[3];

            string paths = Application.StartupPath + "\\ErrLogs" + "\\" + filedatas + "err.csv";
            if (!File.Exists(paths))
            {
                FileStream fs = new FileStream(paths, FileMode.CreateNew);

                fs.Close();


                StreamWriter file_save_title = new StreamWriter(paths, true);




                file_save_title.Write("序号(次数)" + "," + "报警代码" + "," + "报警信息" + "," + "记录时间" + "," + "操作员" + "," + "解决办法" + "," + "详情" + "\r\n");

                file_save_title.Close();




            }


            StreamWriter file_save = new StreamWriter(paths, true);
            //  file_save.Write("\r\n" + System.DateTime.Now + "," + value+",", Encoding.Unicode);
            file_save.Write(value, Encoding.Unicode);
            file_save.Close();



        }


    }
}
