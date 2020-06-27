using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ControlIni;

namespace Justech 
{
    public partial class ResultSetting : Form
    {
        public ResultSetting()
        {
            InitializeComponent();
        }   
        Ini iniFile = new Ini();
        GClsMethod cMethod = new GClsMethod();
        public string oldName, oldUCL, oldLCL;
        private void ResultSetting_Load(object sender, EventArgs e)
        {
            for(int i=1;i<CAMiClsVariable .resultCount+1 ;i++)
            {
                cobID.Items.Add(i.ToString());
            }
            cobID.SelectedIndex = 0;
        }
       
        private void cobID_SelectedIndexChanged(object sender, EventArgs e)
        {
            iniFile.filePath = CAMiClsVariable.resultIniPath;
            if (CAMiClsVariable.strVision == "COGNEX")//Cognex
            {
                txtName.Text = iniFile.ReadIni("结果设置Cognex", cobID.Text);
            }
            else if (CAMiClsVariable.strVision == "KEYENCE")//Keyence
            {
                txtName.Text = iniFile.ReadIni("结果设置Keyence", cobID.Text);
            }
            txtUCL.Text = iniFile.ReadIni("上限", cobID.Text);
            txtLCL.Text = iniFile.ReadIni("下限", cobID.Text);
            oldName = txtName.Text;
            oldUCL = txtUCL.Text;
            oldLCL = txtLCL.Text;
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定写入?","提示",MessageBoxButtons.YesNo ,MessageBoxIcon.Question );
            if(result ==DialogResult.Yes )
            {
                iniFile.filePath = CAMiClsVariable.resultIniPath;
                if (CAMiClsVariable.strVision == "COGNEX")//Cognex
                {
                    iniFile.WriteIni("结果设置Cognex", cobID.Text, txtName.Text);
                }
                else if (CAMiClsVariable.strVision == "KEYENCE")//Keyence
                {
                    iniFile.WriteIni("结果设置Keyence", cobID.Text, txtName.Text);
                }
                iniFile.WriteIni("上限", cobID.Text, txtUCL.Text);
                iniFile.WriteIni("下限", cobID.Text, txtLCL.Text);
                if(oldName !=txtName .Text )
                {
                    cMethod .RecordParaModification ("第 "+cobID.Text +"个数据名", oldName, txtName.Text);
                }
                if(oldUCL !=txtUCL.Text )
                {
                    cMethod.RecordParaModification("第 " + cobID.Text + "个上限", oldUCL, txtUCL.Text);
                }
                if(oldLCL !=txtLCL .Text )
                {
                    cMethod.RecordParaModification("第 " + cobID.Text + "个下限", oldLCL, txtLCL.Text);
                }
               //
            }
        }
    }
}
