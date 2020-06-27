using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Justech
{
    public partial class FrmPara : Form
    {
        public FrmPara()
        {
            InitializeComponent();
        }
        FrmMotion frmMontion = new FrmMotion();
        Grr_Test frmGrr = new Grr_Test();
        FrmClient frmClient = new FrmClient();
        Function_Select frmFunctionSelect = new Function_Select();
        private void FrmPara_Load(object sender, EventArgs e)
        {
            xvalues = this.Width;//记录窗体初始大小
            yvalues = this.Height;
            frmMontion.Dock = DockStyle.Fill;
            frmMontion.TopLevel = false; // 不是最顶层窗体
            tabPage1.Controls.Add(frmMontion);
            frmMontion.Show();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 0:
                    frmMontion.Dock = DockStyle.Fill;
                    frmMontion.TopLevel = false; // 不是最顶层窗体
                    tabPage1.Controls.Add(frmMontion);
                    frmMontion.Show();
                    break;
                case 1:
                    frmClient.Dock = DockStyle.Fill;
                    frmClient.TopLevel = false; // 不是最顶层窗体
                    tabPage2.Controls.Add(frmClient);
                    frmClient.Show();
                    break;
                case 2:
                    frmGrr.Dock = DockStyle.Fill;
                    frmGrr.TopLevel = false; // 不是最顶层窗体
                    tabPage3.Controls.Add(frmGrr);
                    frmGrr.Show();
                    break;
                case 3:
                    frmFunctionSelect.Dock = DockStyle.Fill;
                    frmFunctionSelect.TopLevel = false; // 不是最顶层窗体
                    tabPage4.Controls.Add(frmFunctionSelect);
                    frmFunctionSelect.Show();
                    break;
                default :
                    break;               
            }
        }
        #region SetControls_Size                       *\可以创建一个类文件，调用实现
        float xvalues;
        float yvalues;
        public static bool winform_Status = false;     
        private void FrmPara_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                winform_Status = true;
            }
            else
            {
                winform_Status = false;
                float newX = this.Width / xvalues;//获得比例
                float newY = this.Height / yvalues;
                SetControls(newX, newY, this);
            }
        }
        private void SetControls(float newX, float newY, Control cons)//改变控件的大小
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
        private void SetTag(Control cons)
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
        #endregion

       
    }
}
