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
    public partial class FrmMtc : Form
    {
        public FrmMtc()
        {
            InitializeComponent();
        }

        private void FrmMtc_Load(object sender, EventArgs e)
        {
            try 
            {
                CMasterTask[] mtsks = Program.mng.GetAllMasterTaskOfHid(0);
                foreach (CMasterTask mtsk in mtsks) 
                {
                    comboBoxTskType.Items.Add(mtsk);
                }
                for (int i = 0; i < comboBoxTskType.Items.Count; i++) 
                {
                    
                }
                if (mtsks.Length > 0)
                {
                    btnFinish.Enabled = true;
                    btnReset.Enabled = true;
                }
                else
                {
                    btnFinish.Enabled = false;
                    btnReset.Enabled = false;
                }

                dateTimePicker1.Value = DateTime.Now;
            }
            catch (Exception ex) 
            {
                MessageBox.Show("加载作业异常："+ex.ToString());
            }
        }

        private void comboBoxTskType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUseHall.Text = "";
            txtIccd.Text = "";
            rtTskStatus.Text = "";
            try 
            {
                CMasterTask mtsk = (CMasterTask)comboBoxTskType.SelectedItem;                
                txtUseHall.Text = mtsk.HID.ToString() + "号车厅";
                txtIccd.Text = mtsk.ICCardCode.ToString();
                int i = 1;
                foreach (CTask tsk in mtsk.Tasks) 
                {
                    rtTskStatus.AppendText( i.ToString()+"、"+CHelper.CtaskEqpFormat(tsk.SMG)+"_"+CHelper.CtaskTypeFormat(tsk.Type)+" -"+CHelper.TaskStatusFormatting(tsk.Status)+Environment.NewLine);
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("选择作业异常："+ex.ToString());
            }
        }
        //手动复位
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxTskType.SelectedIndex > -1)
                {
                    CMasterTask mtsk = (CMasterTask)comboBoxTskType.SelectedItem;
                    int rit = Program.mng.ManualResetMasterTask(mtsk.ID);
                    if (rit == 100)
                    {
                        txtUseHall.Text = "";
                        txtIccd.Text = "";
                        rtTskStatus.Text = "";
                        comboBoxTskType.Items.Remove(mtsk);
                        MessageBox.Show("作业已复位！");
                    }
                    else 
                    {
                        MessageBox.Show("作业未复位："+rit.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("请选择操作的作业！");
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //手动完成
        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxTskType.SelectedIndex > -1)
                {
                    CMasterTask mtsk = (CMasterTask)comboBoxTskType.SelectedItem;
                    int rit = Program.mng.ManualCompleteMasterTask(mtsk.ID);
                    if (rit == 100)
                    {
                        txtUseHall.Text = "";
                        txtIccd.Text = "";
                        rtTskStatus.Text = "";
                        comboBoxTskType.Items.Remove(mtsk);
                        MessageBox.Show("作业已完成！");
                    }
                    else 
                    {
                        MessageBox.Show("未完成作业操作："+rit.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("请选择操作的作业！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //查询车位
        private void btnFind_Click(object sender, EventArgs e)
        {
            txtFindLct.Text = "";
            CLocation lct = Program.mng.SelectLocationOfIccd(txtFindIccd.Text.Trim());
            if (lct != null) 
            {
                txtFindLct.Text = lct.Address;
            }
        }

        //车位数据出库
        private void btnOut_Click(object sender, EventArgs e)
        {
            if (txtOutLct.Text.Trim() != "" && txtOutLct.Text.Length == 4)
            {
                int rit = Program.mng.ManualOutLocation(txtOutLct.Text.Trim());
                switch (rit)
                {
                    case 0:
                        MessageBox.Show("车位出库异常！");
                        break;
                    case 100:
                        MessageBox.Show("已完成车位出库！");
                        break;
                    case 101:
                        MessageBox.Show("请输入正确的车位地址！");
                        break;
                    case 102:
                        MessageBox.Show("该车位为无效车位！");
                        break;
                    case 103:
                        MessageBox.Show("该车位未存车！");
                        break;                   
                }
            }
        }

        //车位启用、禁用
        private void btnDIS_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int rit=0;
            if (btn == btnDIS) 
            {
                rit = Program.mng.ManualDisLocation(txtDisLct.Text.Trim());
            }
            else if (btn == btnEnable) 
            {
                rit = Program.mng.ManualEnableLocation(txtDisLct.Text.Trim());
            }

            switch (rit) 
            {
                case 0:
                    MessageBox.Show("车位操作异常！");
                    break;
                case 100:
                    MessageBox.Show("车位操作成功！");
                    break;
                case 101:
                    MessageBox.Show("请输入正确的车位地址！");
                    break;
                case 102:
                    MessageBox.Show("当前车位无法操作！");
                    break;              
            }
        }
        //车位数据挪移
        private void btnTran_Click(object sender, EventArgs e)
        {
            int rit = Program.mng.ManualTranspose(txtFrlct.Text.Trim(), txtToLct.Text.Trim());
            switch (rit) 
            {
                case 0:
                    MessageBox.Show("车位操作异常！");
                    break;
                case 100:
                    MessageBox.Show("完成手动数据搬移！");
                    break;
                case 101:
                    MessageBox.Show("请输入正确的车位地址！");
                    break;
                case 102:
                    MessageBox.Show("源、目的车位无法操作！");
                    break;
                case 103:
                    MessageBox.Show("源车位上无车！");
                    break;
                case 104:
                    MessageBox.Show("目的车位不可用！");
                    break;
                case 105:
                    MessageBox.Show("目的车位与车辆外形不匹配！");
                    break;
            }
        }

        //车厅查询
        private void btnHFind_Click(object sender, EventArgs e)
        {
            try
            {
                txtPositHall.Text = "";
                txtState.Text = "";
                CMasterTask[] mtsks = Program.mng.GetAllMasterTaskOfHid(0);
                string iccode = txtUseIccd.Text.Trim();
                foreach (CMasterTask mtsk in mtsks)
                {
                    if (mtsk.ICCardCode == iccode)
                    {
                        txtPositHall.Text = mtsk.HID.ToString() + "号车厅";
                        foreach (CTask tsk in mtsk.Tasks)
                        {
                            txtState.AppendText(CHelper.TaskStatusFormatting(tsk.Status) + Environment.NewLine);
                        }
                        break;
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //手动入库
        private void button1_Click(object sender, EventArgs e)
        {
            #region
            if (txtInLct.Text.Trim() == "") 
            {
                MessageBox.Show("入库车位不允许为空！");
                txtInLct.Focus();
                return;
            }
            if (txtInIccd.Text.Trim() == "") 
            {
                MessageBox.Show("用户卡号不允许为空！");
                txtInIccd.Focus();
                return;
            }
            if (txtInIccd.Text.Length != 4 || txtInLct.Text.Length != 4) 
            {
                MessageBox.Show("请输入正确的用户卡号或入库车位！");
                txtInIccd.Focus();
                return;
            }
            if (txtInCSize.Text.Trim() == ""||txtInDist.Text.Trim()=="") 
            {
                MessageBox.Show("车辆外形或车辆轴距不允许为空！");
                txtInDist.Focus();
                return;
            }
            if (txtInCSize.Text.Length != 3) 
            {
                MessageBox.Show("请输入正确的车辆外形！");
                return;
            }
            if (Convert.ToInt32(txtInDist.Text.Trim()) < 2000) 
            {
                MessageBox.Show("请输入正确的车辆轴距！");
                return;
            }
            #endregion
            int rit = Program.mng.ManualInLocation(txtInLct.Text.Trim(), txtInIccd.Text.Trim(), txtInCSize.Text.Trim(), Convert.ToInt32(txtInDist.Text.Trim()), dateTimePicker1.Value);
            if (rit == 100)
            {
                MessageBox.Show("手动入库成功！", "提示");
            }
            else
            {
                #region
                switch (rit) 
                {
                    case 101:
                        MessageBox.Show("该卡不是系统卡或该车位不是系统车位！");
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
        }


    
    }
}
