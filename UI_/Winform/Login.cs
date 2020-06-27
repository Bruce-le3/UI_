using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommPlc;
using Justech;
namespace Justech
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Load(object sender, EventArgs e)
        {
            label3.Hide();
            txtNewPassword.Hide();
            txtUserName.Text = GClsConfigFile.ReadConfig("config.ini", "user", "name", "1");
        }       
        private void btnModification_Click(object sender, EventArgs e)
        {
             if (btnModification.Text == "修改")
             {
                 label3.Show();
                 txtNewPassword.Show();
                 btnModification.Text = "确认";
             }
             else
             {
                 password = GClsConfigFile.ReadConfig("config.ini", "user", "password", "1");
                 if (txtPassword.Text == password)
                 {
                     GClsConfigFile.WriteConfig("config.ini", "user", "name", txtUserName.Text);
                     GClsConfigFile.WriteConfig("config.ini", "user", "password", txtNewPassword.Text);
                     MessageBox.Show("修改成功");
                     btnModification.Text = "修改";
                     label3.Hide();
                     txtNewPassword.Hide();
                 }
                 else
                 {
                     MessageBox.Show("密码错误，请重新确认");
                 }
             }
        }
        public bool login_password;
        string name = null;
        string password = null;      
        private void btnUnload_Click(object sender, EventArgs e)//退出界面
        {
            name = GClsConfigFile.ReadConfig("config.ini", "user", "name", "1");
            password = GClsConfigFile.ReadConfig("config.ini", "user", "password", "1");
            if (txtUserName.Text == name && txtPassword.Text == password)
            {
                login_password = true;
                Close();
            }
            else
            {
                MessageBox.Show("请检查密码是否正确");
                login_password = false;
            }
        }
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {           
            if (login_password == true)
            {
                e.Cancel = false;
            }
            else
            {                
                // e.Cancel = true;
            }        
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)//密码输入后按回车键执行同样的动作
        {
            if (e.KeyChar == '\r')
            {
                name = GClsConfigFile.ReadConfig("config.ini", "user", "name", "1");
                password = GClsConfigFile.ReadConfig("config.ini", "user", "password", "1");
                if (txtUserName.Text == name && txtPassword.Text == password)
                {
                    login_password = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("请检查密码是否正确");
                    login_password = false;
                }
            }
        }
    }
}
