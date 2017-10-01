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
    public partial class FrmManualIn : Form
    {
        public FrmManualIn()
        {
            InitializeComponent();
        }

        public FrmManualIn(CLocation lct)
            : this()
        {
            txtInLct.Text = lct.Address;
            txtInCSize.Text = lct.Size;
            this.Text = "手动数据入库 -车位：" + lct.Address.Substring(0, 1) + "边" + Convert.ToInt32(lct.Address.Substring(1, 2)) + "列" + lct.Address.Substring(3, 1) + "层";
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            try
            {
                #region 条件判断
                if (txtInLct.Text.Trim().Length != 4)
                {
                    MessageBox.Show("请输入4位数车位地址！");
                    txtInLct.Focus();
                    return;
                }
                if (txtInIccd.Text.Trim().Length != 4)
                {
                    MessageBox.Show("请输入4位数卡号！");
                    txtInIccd.Focus();
                    return;
                }
                if (txtInCSize.Text.Trim() == "")
                {
                    MessageBox.Show("请输入车辆外形！");
                    txtInCSize.Focus();
                    return;
                }
                if (txtInDist.Text.Trim() == "")
                {
                    MessageBox.Show("请输入车辆轴距！");
                    txtInDist.Focus();
                    return;
                }
                #endregion
                int rit = Program.mng.ManualInLocation(txtInLct.Text.Trim(), txtInIccd.Text.Trim(),
                    txtInCSize.Text.Trim(), Convert.ToInt32(txtInDist.Text.Trim()), dateTimePicker1.Value);
                #region
                switch (rit)
                {
                    case 101:
                        MessageBox.Show("该卡不是系统卡或该车位不存在！");
                        break;
                    case 102:
                        MessageBox.Show("该车位类型不对或该车位已被入库！");
                        break;
                    case 103:
                        MessageBox.Show("车位上有车！");
                        break;
                    case 104:
                        MessageBox.Show("该卡已存车，请输入别的卡号！");
                        break;
                    case 105:
                        MessageBox.Show("车位尺寸与车辆尺寸不匹配！");
                        break;
                    case 106:
                        MessageBox.Show("操作失败，该卡已注销（挂失）！");
                        break;
                    case 0:
                        MessageBox.Show("操作引发异常！");
                        break;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.Close();
        }
    }
}
