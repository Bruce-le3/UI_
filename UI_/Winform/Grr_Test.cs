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
    public partial class Grr_Test : Form
    {
        System.Drawing.Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;  //获取屏幕分辨率
        public Grr_Test()
        {
            InitializeComponent();
            this.Width = rect.Width;

            this.Height = rect.Height;
        }
        List<double> t1 = new List<double>();
        List<double> t2 = new List<double>();
        List<double> t3 = new List<double>();
        List<double> t4 = new List<double>();
        List<double> t5 = new List<double>();
        List<double> t6 = new List<double>();
  
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length>90)
            {

            string s1 = textBox1.Text.Replace("\r\n", ",");
       


            string[] f1 = s1.Split(',');


            t1.Clear();
            t2.Clear();
            t3.Clear();
            GRR.averageData1.Clear();
            GRR.averageData2.Clear();
            GRR.averageData3.Clear();

            for(int i=0;i<30;i++)
            {
                t1.Add(Convert.ToDouble(f1[i]));
                t2.Add(Convert.ToDouble(f1[i+30]));
                t3.Add(Convert.ToDouble(f1[i+60]));

            }
        
            GRR.averageData1 = t1;
            GRR.averageData2 = t2;
            GRR.averageData3 = t3;
            button1.Enabled = false;
            textBox1.ReadOnly = true;
            }

        }

      
        double ucl, ev, av, rr;
        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                GRR.Rbar(ref ucl, ref ev, ref av, ref rr, (double)nmb_Trials.Value, (double)nmb_D4.Value, (double)nmb_K1.Value, (double)nmb_k2.Value);


                textBox4.Text = ucl.ToString();
                textBox8.Text = ev.ToString();
                textBox9.Text = av.ToString();
                textBox10.Text = rr.ToString();


                double Repeatability = (ev / (double)numericUpDown1.Value) * 100;

                textBox2.Text = Repeatability.ToString();

                double Reproducibility = (av / (double)numericUpDown1.Value) * 100;

                textBox3.Text = Reproducibility.ToString();
                double Gage_RR = (rr / (double)numericUpDown1.Value) * 100;

                textBox5.Text = Gage_RR.ToString();

                if (Gage_RR > (double)nmb_SPC.Value)
                {
                    textBox5.BackColor = Color.Red;
                    label14.Text = "FAIL";
                    label14.Font = new System.Drawing.Font("宋体", 20, FontStyle.Bold);
                    label14.ForeColor = Color.Red;
                }
                else
                {
                    textBox5.BackColor = Color.Green;
                    label14.Text = "PASS";
                    label14.Font = new System.Drawing.Font("宋体", 20, FontStyle.Bold);
                    label14.ForeColor = Color.Green;
                }




                button1.Enabled = true;
                textBox1.ReadOnly = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        
          
        }
        //#region SetControls_Size
        //float xvalues;
        //float yvalues;
        //private void MainForm_Resize(object sender, EventArgs e)//重绘事件
        //{

        //    float newX = this.Width / xvalues;//获得比例
        //    float newY = this.Height / yvalues;
        //    SetControls(newX, newY, this);




        //}

        //private void SetControls(float newX, float newY, Control cons)//改变控件的大小
        //{

        //    foreach (Control con in cons.Controls)
        //    {
        //        try
        //        {
        //            string[] mytag = con.Tag.ToString().Split(new char[] { ':' });
        //            float a = Convert.ToSingle(mytag[0]) * newX;
        //            con.Width = (int)a;
        //            a = Convert.ToSingle(mytag[1]) * newY;
        //            con.Height = (int)a;
        //            a = Convert.ToSingle(mytag[2]) * newX;
        //            con.Left = (int)a;
        //            a = Convert.ToSingle(mytag[3]) * newY;
        //            con.Top = (int)a;
        //            Single currentSize = Convert.ToSingle(mytag[4]) * newY;
        //            con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
        //            if (con.Controls.Count > 0)
        //            {
        //                SetControls(newX, newY, con);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            // MessageBox.Show(ex.Message);
        //        }
        //    }

        //}
        ///// <summary>
        ///// 遍历窗体中控件函数
        ///// </summary>
        ///// <param name="cons"></param>
        //private void SetTag(Control cons)
        //{
        //    foreach (Control con in cons.Controls)  //遍历窗体中的控件,记录控件初始大小
        //    {

        //        con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;

        //        if (con.Controls.Count > 0)
        //        {

        //            SetTag(con);

        //        }

        //    }


        //}

        //#endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            //this.Resize += new EventHandler(MainForm_Resize); //添加窗体拉伸重绘事件
            //xvalues = this.Width;//记录窗体初始大小
            //yvalues = this.Height;
            //SetTag(this);
    

            numericUpDown1.Value = 1;
            nmb_SPC.Value = 10m;
            nmb_Trials.Value = 3;
            nmb_D4.Value = 2.58m;
            nmb_K1.Value = 3.05m;
            nmb_k2.Value=2.70m;

        }

        private void nmb_Trials_ValueChanged(object sender, EventArgs e)
        {

            if (nmb_Trials.Value == 2)
            {
                nmb_D4.Value = 3.27m;
                nmb_K1.Value = 4.56m;
                nmb_k2.Value = 3.65m;
            }
            else
            {
                nmb_D4.Value = 2.58m;
                nmb_K1.Value = 3.05m;
                nmb_k2.Value = 2.70m;
            }

        }

       
    }
}
