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

namespace Justech 
{
    public partial class FrmPathSelect : Form
    {
        GClsConfigFile iniFile = new GClsConfigFile(Application.StartupPath + "\\" + "Config" + "\\Setting.ini");
        public FrmPathSelect()
        {
            InitializeComponent();
        }
        FolderBrowserDialog dialog = new FolderBrowserDialog();

        private void FrmPathSelect_Load(object sender, EventArgs e)
        {
            PathRead();
        }
        #region 设置路径      
        private void btnReportFolder_Click(object sender, EventArgs e)//报表文件夹
        {
            dialog.ShowDialog();
            txtReportFolder.Text = dialog.SelectedPath;
        }

        private void btnSFCFolder_Click(object sender, EventArgs e)//SFC文件夹
        {
            dialog.ShowDialog();
            txtSFCFolder.Text = dialog.SelectedPath;
        }

        private void DataFolder_Click(object sender, EventArgs e)//数据文件夹
        {
            dialog.ShowDialog();
            txtDataFolder.Text = dialog.SelectedPath;
        }

        private void btnImageFolder_Click(object sender, EventArgs e)//图像文件夹
        {
            dialog.ShowDialog();
            txtImageFolder.Text = dialog.SelectedPath;
        }

        private void btnImageShotFolder_Click(object sender, EventArgs e)//截屏文件夹
        {
            dialog.ShowDialog();
            txtImageShotFolder.Text = dialog.SelectedPath;
        }
        private void btnProgramPath_Click(object sender, EventArgs e)//程序路径
        {
            dialog.ShowDialog();
            txtProgramPath.Text = dialog.SelectedPath;
        }
        #endregion

        public void PathRead()//路径读取
        {
            txtMachineName.Text = iniFile.GetString("MachineName", "Name", "NULL");
            txtReportFolder.Text = iniFile.GetString("ReportPath", "Path", "NULL");
            txtSFCFolder.Text = iniFile.GetString("SFCPath", "Path", "NULL");
            txtDataFolder.Text = iniFile.GetString("DataPath", "Path", "NULL");
            txtImageFolder.Text = iniFile.GetString("ImagePath", "Path", "NULL");
            txtImageShotFolder.Text = iniFile.GetString("ScreenPath", "Path", "NULL");
            txtProgramPath.Text = iniFile.GetString("ProgramPath", "Path", "NULL");
        }

        private void btnReadData_Click(object sender, EventArgs e)//Read
        {
            PathRead();
            MessageBox.Show("数据读取成功！");
        }

        private void btnWriteData_Click(object sender, EventArgs e)//Write
        {
            iniFile.WriteValue("MachineName", "Name", txtMachineName.Text.Trim());
            iniFile.WriteValue("ReportPath", "Path", txtReportFolder.Text.Trim());
            iniFile.WriteValue("SFCPath", "Path", txtSFCFolder.Text.Trim());
            iniFile.WriteValue("DataPath", "Path", txtDataFolder.Text.Trim());
            iniFile.WriteValue("ImagePath", "Path", txtImageFolder.Text.Trim());
            iniFile.WriteValue("ScreenPath", "Path", txtImageShotFolder.Text.Trim());
            iniFile.WriteValue("ProgramPath", "Path", txtProgramPath.Text.Trim());
            MessageBox.Show("写入成功！");
        }
    }
}
