using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControlIni;


namespace Justech
{
    public partial class Function_Select : Form
    {
        System.Drawing.Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;  //获取屏幕分辨率
        public Function_Select()
        {
            InitializeComponent();
            this.Width = rect.Width;

            this.Height = rect.Height;

        }
        public Ini iniFunctionSelect = new Ini();
        GClsMethod methodFunctionSelect = new GClsMethod();
        Main frmMain = new Main();
        private void Function_Select_Load(object sender, EventArgs e)
        {
            int productIndex;
            foreach (CheckBox cb in grbProductSelect.Controls)
            {
                productIndex = Convert.ToInt32(cb.Tag);
                cb.Checked = CAMiClsVariable.isProductSelect[productIndex - 1] == 1 ? true : false;
            }
            ckbPDCA.Checked = CAMiClsVariable.isPDCAOpen;
            ckbHIVE.Checked = CAMiClsVariable.isHIVEOpen;
            ckbEmptyRun.Checked = CAMiClsVariable.isEmptyRun;
            ckbSmema.Checked = CAMiClsVariable.isSmemaOpen;
        }

        private void btnSaveParament_Click(object sender, EventArgs e)//修改产品选择使能
        {
            DialogResult result = MessageBox.Show("请确定是否更改产品穴位选择？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniFunctionSelect.filePath = CAMiClsVariable.montionIniPath;
                int productIndex = 0, productEnable = 0;
                foreach (CheckBox cb in grbProductSelect.Controls)
                {
                    productIndex = Convert.ToInt32(cb.Tag);
                    productEnable = cb.Checked ? 1 : 0;
                    CAMiClsVariable.isProductSelect[productIndex - 1] = productEnable;
                    if (cb.Checked != (iniFunctionSelect.ReadIni("穴位选择", "穴位" + productIndex.ToString ()) == "1" ? true : false ))
                    {
                        methodFunctionSelect.RecordParaModification("穴位" + productIndex.ToString(), iniFunctionSelect.ReadIni("穴位选择", "穴位" + productIndex.ToString()), productEnable.ToString ());
                        iniFunctionSelect.WriteIni("穴位选择", "穴位" + productIndex.ToString(), productEnable.ToString());
                        CAMiClsVariable.isProductSelect[productIndex - 1] = productEnable;
                    }
                    Application.DoEvents();
                }
                CAMiClsMethod.ChangeResultDisplay();//改变颜色
            }         
        }

        private void ckbEmptyRun_CheckedChanged(object sender, EventArgs e)
        {
            if(ckbEmptyRun.Checked)
            {
                foreach(Control ctl in Main.frmMain.Controls)
                {
                    ctl.BackColor = Color.Yellow;
                    CAMiClsVariable.isEmptyRun = true;
                }
            }
            else
            {
                foreach (Control ctl in Main.frmMain.Controls)
                {
                    ctl.BackColor = SystemColors.Window;
                    CAMiClsVariable.isEmptyRun = false;
                }
            }
        }

        private void ckbPDCA_CheckedChanged(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否更改PDCA模式？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniFunctionSelect.filePath = CAMiClsVariable.configIniPath;
                if (ckbPDCA.Checked)
                {
                    CAMiClsVariable.isPDCAOpen = true;
                    iniFunctionSelect.WriteIni("PDCA", "Enable", "TRUE");
                }
                else
                {
                    CAMiClsVariable.isPDCAOpen = false;
                    iniFunctionSelect.WriteIni("PDCA", "Enable", "FALSE");
                }
            }            
        }

        private void ckbHIVE_CheckedChanged(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否更改PDCA模式？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniFunctionSelect.filePath = CAMiClsVariable.configIniPath;
                if (ckbHIVE.Checked)
                {
                    CAMiClsVariable.isHIVEOpen = true;
                    iniFunctionSelect.WriteIni("HIVE", "Enable", "TRUE");
                }
                else
                {
                    CAMiClsVariable.isHIVEOpen = false;
                    iniFunctionSelect.WriteIni("HIVE", "Enable", "FALSE");
                }
            }          
        }

        private void ckbSmema_CheckedChanged(object sender, EventArgs e)
        {
             DialogResult result = MessageBox.Show("请确定是否更改SMEMA模式？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniFunctionSelect.filePath = CAMiClsVariable.configIniPath;
                if (ckbSmema.Checked)
                {
                    CAMiClsVariable.isSmemaOpen = true;
                    iniFunctionSelect.WriteIni("SMEMA", "Enable", "TRUE");
                }
                else
                {
                    CAMiClsVariable.isSmemaOpen = false;
                    iniFunctionSelect.WriteIni("SMEMA", "Enable", "FALSE");
                }
            }
        }

        private void ckbEntrance_CheckedChanged(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确定是否打开门禁？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                iniFunctionSelect.filePath = CAMiClsVariable.configIniPath;
                if (ckbEntrance.Checked)
                {
                    CAMiClsVariable.isEntranceOpen = true;
                    iniFunctionSelect.WriteIni("ENTRANCE", "Enable", "TRUE");
                }
                else
                {
                    CAMiClsVariable.isEntranceOpen = false;
                    iniFunctionSelect.WriteIni("ENTRANCE", "Enable", "FALSE");
                }
            }
        }
    }
}
