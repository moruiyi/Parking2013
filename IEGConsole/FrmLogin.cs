using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IEGConsole.localhost;

namespace IEGConsole
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            txtUser.Text = "admin";
            txtPassword.Text = "6132";
        }     

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text.Trim() != "" && txtPassword.Text.Trim() != "")
            {
                COperator user = Program.mng.CheckPassword(txtUser.Text.Trim(), txtPassword.Text.Trim());
                if (user != null)
                {
                    Program.currOpr = user;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("用户名或密码输入有误，请重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Text = "";
                    txtPassword.Focus();
                }
            }
            else 
            {
                MessageBox.Show("请输入正确的用户名及密码！");
            }
        }      
     
    }
}
