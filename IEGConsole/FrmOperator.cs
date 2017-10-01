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
    public partial class FrmOperator : Form
    {
        private COperator currOprt;
        private List<COperator> oprts;
        private BindingList<COperator> bindOprtSource; 

        public FrmOperator()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmOperator_Load(object sender, EventArgs e)
        {
            currOprt = Program.currOpr;
            comboBox1.SelectedIndex = 0;            

            oprts = Program.mng.SelectAllOperators().ToList();
            updateDataGridView();
        }

        private void updateDataGridView() 
        {
            try
            {
                bindOprtSource = new BindingList<COperator>(oprts);
                dataGridView1.DataSource = bindOprtSource;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }           
        }

        //删除
        private void btnDel_Click(object sender, EventArgs e)
        {
            int idx = dataGridView1.CurrentRow.Index;
            if (idx < 0)
            {
                return;
            }

            string code = this.dataGridView1["code", idx].Value.ToString();
            if (code == currOprt.Code) 
            {
                MessageBox.Show("该用户正在使用中！");
                return;
            }
            if (code == "admin") 
            {
                MessageBox.Show("超级管理员不允许被删除！");
                return;
            }
            COperator oprt = oprts.Find(o => o.Code == code);
            if (oprt != null)
            {
                int rit = Program.mng.DeleteOprt(code);
                if (rit == 100)
                {
                    oprts.Remove(oprt);
                    updateDataGridView();
                }
            }
        }
        //修改密码
        private void btnMidify_Click(object sender, EventArgs e)
        {
            if (currOprt.Code == "admin") 
            {
                MessageBox.Show("超级管理员不允许被更改！");
                return;
            }
            if (txtOriPWD.Text.Trim() == "" || txtNewPwd.Text.Trim() == "" || txtNewPwdCon.Text.Trim() == "") 
            {
                MessageBox.Show("不允许为空！");
                return;
            }
            if (currOprt.Password != txtOriPWD.Text.Trim()) 
            {
                MessageBox.Show("原密码输入有误！");
                txtOriPWD.Text = "";
                txtOriPWD.Focus();
                return;
            }
            if (txtNewPwd.Text.Trim() != txtNewPwdCon.Text.Trim()) 
            {
                MessageBox.Show("新密码两次输入不一致！");
                return;
            }
            if (txtNewPwdCon.Text.Trim().Length > 6) 
            {
                MessageBox.Show("密码只允许最多6个字符！");
                return;
            }
            int rit = Program.mng.UpdateOperatorPassword(currOprt.Code, txtNewPwdCon.Text.Trim());
            if (rit == 100) 
            {               
                MessageBox.Show("密码重置成功！");
                txtOriPWD.Text = "";
                txtNewPwd.Text = "";
                txtNewPwdCon.Text = "";                
            }
        }

        private void btnUCon_Click(object sender, EventArgs e)
        {
            if (txtUCode.Text.Trim() == ""||txtUPwdCon.Text.Trim()=="") 
            {
                MessageBox.Show("用户名或密码不允许为空！");
                return;
            }

            if (comboBox1.SelectedIndex == -1) 
            {
                MessageBox.Show("请选择用户权限！");
                return;
            }

            COperator opt = oprts.Find(p => p.Code == txtUCode.Text.Trim());
            if (opt != null) 
            {
                MessageBox.Show("当前用户名已存在，请输入别的！");
                txtUCode.Text = "";
                return;
            }

            COperator oprt = new COperator();
            oprt.Code = txtUCode.Text.Trim();
            oprt.Password = txtUPwdCon.Text.Trim();
            if (txtUName.Text.Trim() != "") 
            {
                oprt.Name = txtUName.Text.Trim();
            }
            if (comboBox1.SelectedIndex == 0)
            {
                oprt.Type = EnmOperatorType.Operator;
            }
            else 
            {
                oprt.Type = EnmOperatorType.Manager;
            }

            int rit = Program.mng.InsertOperator(oprt);
            if (rit == 100) 
            {
                oprts.Add(oprt);
                MessageBox.Show("添加管理员完成！");
                txtUCode.Clear();
                txtUPwdCon.Clear();

                updateDataGridView();
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                if (e.Value.GetType() == typeof(EnmOperatorType))
                {
                    switch ((EnmOperatorType)e.Value)
                    {
                        case EnmOperatorType.Manager:
                            e.Value = "管理员";
                            break;
                        case EnmOperatorType.Operator:
                            e.Value = "操作员";
                            break;
                        default:
                            break;
                    }
                }
            }
        }


     
    }
}
