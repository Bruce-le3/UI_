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
    public partial class Serial_Port_Test : Form
    {
        System.Drawing.Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;  //获取屏幕分辨率
        public Serial_Port_Test()
        {
            InitializeComponent();
            this.Width = rect.Width;

            this.Height = rect.Height;
        }

        private void Serial_Port_Test_Load(object sender, EventArgs e)
        {
            for (int i = 1; i < 10; i++)
            {
                cmb_PortName.Items.Add("COM" + i);
                if (i > 4 & i < 9)
                {
                    cmb_DataBits.Items.Add(i);
                }
            }
            
            cmb_BaudRate.Items.Add(9600);
            cmb_BaudRate.Items.Add(14400);
            cmb_BaudRate.Items.Add(19200);
            cmb_BaudRate.Items.Add(115200);
            cmb_Parity.Items.Add("None");
            cmb_Parity.Items.Add("Odd");
            cmb_Parity.Items.Add("Even");
            cmb_Parity.Items.Add("Mark");
            cmb_Parity.Items.Add("Space");
            cmb_StopBits.Items.Add(1);
            cmb_StopBits.Items.Add(1.5);
            cmb_StopBits.Items.Add(12);
            cmb_PortName.Text = "COM1";
            cmb_BaudRate.Text = "115200";
            cmb_Parity.Text = "None";
            cmb_DataBits.Text = "8";
            cmb_StopBits.Text = "1";

        }

        private void button1_Click(object sender, EventArgs e)
        {

            textBox1.Text = cmb_PortName.Text + "," + cmb_BaudRate.Text + "," + cmb_Parity.Text + "," + cmb_DataBits.Text + "," + cmb_StopBits.Text;
            SerialPort_.connect(textBox1.Text, textBox2.Text);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            textBox3.Text = SerialPort_.Read_data;



        }







    }
}
