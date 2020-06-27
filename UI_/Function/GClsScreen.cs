using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Justech
{
    class GClsScreen
    {
        public void SetControls(float newX, float newY, Control cons)//改变控件的大小
        {
            foreach (Control con in cons.Controls)
            {
                if (con.Tag == null) continue;
                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });
                float a = Convert.ToSingle(mytag[0]) * newX;
                con.Width = (int)a;
                a = Convert.ToSingle(mytag[1]) * newY;
                con.Height = (int)a;
                a = Convert.ToSingle(mytag[2]) * newX;
                con.Left = (int)a;
                a = Convert.ToSingle(mytag[3]) * newY;
                con.Top = (int)a;
                Single currentSize = Convert.ToSingle(mytag[4]) * newY;
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    SetControls(newX, newY, con);
                }
            }
        }
        /// <summary>
        /// 遍历窗体中控件函数
        /// </summary>
        /// <param name="cons"></param>
        public void SetTag(Control cons)
        {
            foreach (Control con in cons.Controls)  //遍历窗体中的控件,记录控件初始大小
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    SetTag(con);
                }
            }
        }
    }
}
