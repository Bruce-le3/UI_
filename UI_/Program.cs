using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Justech
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isRuned;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, "OnlyRunOneInstance", out isRuned);
            if(isRuned)
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Main());
                    mutex.ReleaseMutex();
                }
               catch(Exception ex)
               {
                    MessageBox.Show(ex.ToString());
               }
            }
            else
            {
                MessageBox.Show("程序已启动!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }          
        }
    }
}
