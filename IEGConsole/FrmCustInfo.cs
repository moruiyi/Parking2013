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
    public partial class FrmCustInfo : Form
    {
        private CCustomer cust;
        private int custID;

        public FrmCustInfo()
        {
            InitializeComponent();
            radioButtonNormal.Checked = true;
            cust = new CCustomer();
            btndel.Enabled = false;
            custID = 0;
            comboBox1.SelectedIndex = 1;
        }

        public FrmCustInfo(CCustomer customer) :this()
        {
            cust = customer;
            custID = cust.ID;

            btndel.Enabled = true;
            txtname.Text = cust.Code;
            txttel.Text = cust.Telphone;
            txtpnmb.Text = cust.PlatNumber;
            txtMobile.Text = cust.Mobile;
            txtads.Text = cust.Address;
            txtlct.Text = cust.LctAddress;
            comboBox1.SelectedIndex = (int)cust.ICCardType;
            txtcode.Text = cust.ICCardCode;
            if (cust.ICCardStat == EnmICCardStatus.Normal) 
            {
                radioButtonNormal.Checked = true;
                radioButtonLoss.Checked = false;
            }
            else if (cust.ICCardStat == EnmICCardStatus.Lost)
            {
                radioButtonNormal.Checked = false;
                radioButtonLoss.Checked = true;
            }
            else 
            {
                radioButtonNormal.Checked = false;
                radioButtonLoss.Checked = false;
            }

        }

        private void btncnl_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //保存
        private void btnsv_Click(object sender, EventArgs e)
        {
            if (txtname.Text.Trim() == "" || txtcode.Text.Trim() == "") 
            {
                MessageBox.Show("姓名、发卡编号不允许为空！");
                return;
            }
            if (comboBox1.SelectedIndex == 3 && txtlct.Text.Trim()=="") 
            {
                MessageBox.Show("请输入车位地址！");
                return;
            }

            CICCard iccd = new CICCard();
            iccd=Program.mng.SelectByUserCode(txtcode.Text.Trim());  //初步判断

            if (iccd == null) 
            {
                MessageBox.Show("发卡编号不存在！");
                return;
            }

            if (comboBox1.SelectedIndex == 3 && (Program.mng.CheckFixLocationAvail(txtlct.Text.Trim()) == false))  //初步判断
            {
                MessageBox.Show("请输入正确的车位地址！");
                return;
            }

            cust.Code = txtname.Text.Trim();
            cust.Telphone = txttel.Text.Trim();
            cust.Mobile = txtMobile.Text.Trim();            
            cust.Address = txtads.Text.Trim();

            iccd.PlatNumber = txtpnmb.Text.Trim();
            iccd.DueDtime = DateTime.Now;
            //卡状态
            if (radioButtonNormal.Checked == true)
            {
                iccd.Status = EnmICCardStatus.Normal;
            }
            else if (radioButtonLoss.Checked == true)
            {
                iccd.Status = EnmICCardStatus.Lost;
            }
            //卡类型
            if (comboBox1.SelectedIndex == 3)
            {
                iccd.Address = txtlct.Text.Trim();
                iccd.Type = EnmICCardType.FixedLocation;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                iccd.Address = "";
                iccd.Type = EnmICCardType.Fixed;
            }
            else
            {
                iccd.Address = "";
                iccd.Type = EnmICCardType.Temp;
            }

            if (custID != 0)   //更新
            { 
                iccd.CustomerID = cust.ID;
                string iccode=cust.ICCardCode;
                int rit = Program.mng.UpdateCustomInfo(cust, iccd,iccode);
                if (rit == 100)
                {
                    MessageBox.Show("保存成功！");                   
                    this.Close();
                }
                else
                {
                    #region
                    switch (rit) 
                    {
                        case 101:
                            MessageBox.Show("当前卡号已注销或挂失！");
                            break;
                        case 102:
                            MessageBox.Show("当前卡号已绑定其它用户！");
                            break;
                        case 103:
                            MessageBox.Show("操作失败，该卡已存车，请等待出车完成后再操作！");
                            break;
                        case 104:
                            MessageBox.Show("该卡已绑定了其他车位！");
                            break;                       
                    }
                    if (rit == 0) 
                    {
                        MessageBox.Show("系统异常，操作失败！");
                        this.Close();
                    }
                    #endregion
                }
            } 
            else    //新增
            {
                int rit = Program.mng.InsertCustomer(cust, iccd);
                if (rit == 100)
                {
                    MessageBox.Show("保存成功！");                   
                    this.Close();
                }
                else
                {
                    #region
                    switch (rit)
                    {
                        case 101:
                            MessageBox.Show("当前卡号已注销或挂失或已被其他顾客使用！");
                            break;
                        case 102:
                            MessageBox.Show("操作失败，该卡已存车，请等待出车完成后再操作！");
                            break;
                        case 103:
                            MessageBox.Show("该卡已绑定了其他车位！");
                            break;                      
                    }
                    if (rit == 0)
                    {
                        MessageBox.Show("系统异常，操作失败！");
                        this.Close();
                    }
                    #endregion
                }
            }
        }

        private void btndel_Click(object sender, EventArgs e)
        {
            if (custID != 0) 
            {
                int rit = Program.mng.DeleteCustomer(custID,cust.ICCardCode);
                if (rit == 100) 
                {
                    MessageBox.Show("删除成功！");
                    this.Close();
                }
                else if (rit == 101) 
                {
                    MessageBox.Show("请待此卡完成出库后，再删除！");
                }
            }
        }
               
    }
}
