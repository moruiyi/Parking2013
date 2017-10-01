using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IEGConsole.localhost;
using System.IO;

namespace IEGConsole
{
    public partial class FrmGetFee : Form
    {
        private readonly int iccdCom = IEGConsole.Properties.Settings.Default.ICCardCom;
        private CTempCardChargeLog tempLog;
        private CFixCardChargeLog tempFixLog;

        private List<CTariff> lstTariffs;
        private BindingList<CTariff> bindingLst;

        public FrmGetFee()
        {
            InitializeComponent();
        }
        //临时卡停车费用查询
        private void btntsrc_Click(object sender, EventArgs e)
        {
            txtitime.Text = "";
            txtotime.Text = "";
            txtStay.Text = "";
            txtrfee.Text = "";
            try 
            {
                if (txtcd.Text.Trim() == "") 
                {
                    txtcd.Focus();
                    return;
                }
                int rit = Program.mng.GetTempCardFeeInfo(txtcd.Text.Trim(), out tempLog);
                if (rit == 100)
                {
                    txtcd.Text = tempLog.ICCode;
                    txtitime.Text = tempLog.InDate.ToString();
                    txtotime.Text = tempLog.OutDate.ToString();
                    txtrfee.Text = tempLog.RecivFee.ToString("￥0.00") + "元";
                    nupafee.Value = Convert.ToDecimal(tempLog.RecivFee);
                    TimeSpan span = tempLog.OutDate - tempLog.InDate;
                    txtStay.Text = (span.Days > 0 ? span.Days + "天" : " ") + (span.Hours > 0 ? span.Hours + "小时" : " ") +
                        (span.Minutes > 0 ? span.Minutes + "分" : " ") + (span.Seconds > 0 ? span.Seconds + "秒" : " ");

                    btnOutCar.Enabled = true;

                    if (Convert.ToBoolean(Properties.Settings.Default.VTopEnable))
                    {
                        Vtop_DisplayMoney(tempLog.RecivFee.ToString());  //显示金额
                    }
                }
                else
                {
                    #region
                    switch (rit) 
                    {
                        case 101:
                            MessageBox.Show("不是本系统用卡！");
                            break;
                        case 102:
                            MessageBox.Show("该卡不是临时卡！");
                            break;
                        case 103:
                            MessageBox.Show("该卡没有存车！");
                            break;
                        case 104:
                            MessageBox.Show("该卡正在作业！");
                            break;                        
                        default:
                            MessageBox.Show("操作引发异常！");
                            break;
                    }
                    #endregion
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //查询固定卡收费
        private void btnv_Click(object sender, EventArgs e)
        {
            try
            {                
                txtFeeStandard.Text = "";
                txtIccdType.Text = "";
                txtCurrDue.Text = "";
                nudmcnt.Value = 0;

                if (txtfixcd.Text.Trim() != "")
                {
                    CICCard iccd = Program.mng.SelectByUserCode(txtfixcd.Text.Trim());
                    if (iccd != null)
                    {
                        string type = "";
                        switch (iccd.Type)
                        {
                            case EnmICCardType.Temp:
                                type = "临时卡";
                                break;
                            case EnmICCardType.Fixed:
                                type = "定期卡";
                                break;
                            case EnmICCardType.FixedLocation:
                                type = "固定车位卡";
                                break;
                        }
                        txtIccdType.Text = type;
                        txtCurrDue.Text = iccd.DueDtime.ToString();
                        btnFixFee.Enabled = true;
                        //构建收费记录
                        tempFixLog = new CFixCardChargeLog();
                        tempFixLog.ICCode = iccd.Code;
                        tempFixLog.ICType = iccd.Type;
                        tempFixLog.DueDtime = iccd.DueDtime;

                        rdMonth.Checked = true;
                        lstTariffs = new List<CTariff>(Program.mng.SelectTariff());  //加载收费标准                
                        if (lstTariffs != null)
                        {
                            CTariff tari = lstTariffs.Find(t => t.Type == tempFixLog.ICType && t.Unit == EnmFeeUnit.Month);
                            if (tari != null)
                            {
                                txtFeeStandard.Text = tari.Fee.ToString();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("该卡不是系统卡！");
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnTempRead_Click(object sender, EventArgs e)
        {
            txtcd.Text = readIccard();
        }

        #region 读卡
        private string readIccard() 
        {
            CICCardRW iccdObj = new CIcCardRWOne(iccdCom, 9600, 0, 0);
            bool isConn = false;
            string iccode = "";

            try
            {
                isConn = iccdObj.ConnectCOM();
            }
            catch (Exception ex)
            {
                MessageBox.Show("刷卡器建立异常：" + ex.ToString());
            }

            try
            {
                if (isConn)
                {
                    int nback = 1;
                    Int16 nICType = 0;
                    uint ICNum = 0;
                    byte[] IcData = new byte[16];

                    nback = iccdObj.RequestICCard(ref nICType); //寻卡
                    if (nback == 0)
                    {
                        nback = iccdObj.SelectCard(ref ICNum);  //读取物理卡号
                        if (nback == 0)
                        {
                            nback = iccdObj.ReadCard(1, 0, ref IcData);  //读取指定扇区：1，指定DB块：0 的数据
                            if (nback == 0)
                            {
                                string data = "";
                                for (int i = 0; i < IcData.Length; i++)
                                {
                                    string a = Convert.ToString(IcData[i], 16);
                                    if (a.Length < 2)
                                    {
                                        a = "0" + a;
                                    }
                                    data += a;
                                }
                                iccode = data.Substring(0, 4); //4位数卡号                            
                            }
                        }
                    }
                }
                iccdObj.disConnectCOM();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return iccode;
        }
        #endregion

        private void btnFixRead_Click(object sender, EventArgs e)
        {
            txtfixcd.Text = readIccard();
        }
        //固定卡缴费
        private void btnFixFee_Click(object sender, EventArgs e)
        {
            try
            {
                btnFixFee.Enabled = false;
                if (txtFeeStandard.Text.Trim() == "")
                {
                    MessageBox.Show("请选择收费类型或该卡是临时卡！");
                    return;
                }
                if (nudtfee.Value > 0)
                {
                    float fee = Convert.ToSingle(nudtfee.Value);
                    float feeStandard = Convert.ToSingle(txtFeeStandard.Text.Trim());
                    if (fee % feeStandard != 0)
                    {
                        MessageBox.Show("缴费金额不是收费标准的整数倍，无法完成缴费！");
                        return;
                    }
                    nudmcnt.Value = (decimal)(fee / feeStandard);//计算时间 显示
                    DialogResult dr = MessageBox.Show("是否确认缴费？", "通知", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                    if (dr == DialogResult.OK)
                    {
                        //更新收费记录
                        tempFixLog.ThisFee = fee;
                        tempFixLog.ThisDate = DateTime.Now;
                        tempFixLog.OperatorCode = Program.currOpr.Code;

                        if (rdMonth.Checked)
                        {
                            tempFixLog.FeeUnit = (int)nudmcnt.Value;
                        }
                        else if (rdSeason.Checked)
                        {
                            tempFixLog.FeeUnit = (int)nudmcnt.Value * 3;
                        }
                        else if (rdYear.Checked)
                        {
                            tempFixLog.FeeUnit = (int)nudmcnt.Value * 12;
                        }
                        tempFixLog.DueDtime = tempFixLog.DueDtime.AddMonths(tempFixLog.FeeUnit);
                        int rit = Program.mng.SetFixCardFee(tempFixLog);
                        if (rit == 100)
                        {                           
                            MessageBox.Show("缴费成功！");
                            txtLastDue.Text = txtCurrDue.Text;
                            txtCurrDue.Text = tempFixLog.DueDtime.ToString();
                        }
                    }                    
                }     
                else
                {
                    MessageBox.Show("请输入缴费金额！");
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }            
        }

        private void rdMonth_CheckedChanged(object sender, EventArgs e)
        {

            EnmFeeUnit unit = EnmFeeUnit.Init;
            if (rdMonth.Checked) 
            {
                unit = EnmFeeUnit.Month;
            }
            else if (rdSeason.Checked) 
            {
                unit = EnmFeeUnit.Season;
            }
            else if (rdYear.Checked) 
            {
                unit = EnmFeeUnit.Year;
            }
            try
            {
                lstTariffs = new List<CTariff>(Program.mng.SelectTariff());  //加载收费标准                
                if (lstTariffs != null)
                {
                    CTariff tari = lstTariffs.Find(t => t.Type == tempFixLog.ICType && t.Unit == unit);
                    if (tari != null)
                    {
                        txtFeeStandard.Text = tari.Fee.ToString();
                    }
                    else 
                    {
                        txtFeeStandard.Text = "";
                    }  
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //加载收费标准
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2) 
            {
                UpdateDateGridViewFeeStandard();
            }
        }

        private void UpdateDateGridViewFeeStandard() 
        {
            try
            {
                lstTariffs = new List<CTariff>(Program.mng.SelectTariff());
                if (lstTariffs != null)
                {
                    bindingLst = new BindingList<CTariff>(lstTariffs);
                    dataGridView2.DataSource = bindingLst;
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //取消
        private void btndel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                if (e.Value.GetType() == typeof(EnmICCardType))
                {
                    switch ((EnmICCardType)e.Value)
                    {
                        case EnmICCardType.Fixed:
                            e.Value = "定期卡";
                            break;
                        case EnmICCardType.Temp:
                            e.Value = "临时卡";
                            break;
                        case EnmICCardType.FixedLocation:
                            e.Value = "固定卡";
                            break;
                        default:
                            e.Value = "";
                            break;
                    }
                }
                else if (e.Value.GetType() == typeof(EnmFeeType))
                {
                    switch ((EnmFeeType)e.Value)
                    {
                        case EnmFeeType.Charging:
                            e.Value = "收费";
                            break;
                        case EnmFeeType.Limited:
                            e.Value = "限额";
                            break;
                        case EnmFeeType.FirstCharge:
                            e.Value = "首个小时";
                            break;
                        default:
                            e.Value = "";
                            break;
                    }
                }
                else if (e.Value.GetType() == typeof(bool))
                {
                    switch ((bool)e.Value)
                    {
                        case true:
                            e.Value = "是";
                            break;
                        case false:
                            e.Value = "否";
                            break;
                        default:
                            e.Value = "";
                            break;
                    }
                }

                else if (e.Value.GetType() == typeof(EnmFeeUnit))
                {
                    switch ((EnmFeeUnit)e.Value)
                    {
                        case EnmFeeUnit.Hour:
                            e.Value = "每小时";
                            break;
                        case EnmFeeUnit.Month:
                            e.Value = "每月";
                            break;
                        case EnmFeeUnit.Season:
                            e.Value = "每季";
                            break;
                        case EnmFeeUnit.Year:
                            e.Value = "每年";
                            break;
                        default:
                            e.Value = "";
                            break;
                    }
                }
            }
        }
        //修改收费标准
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1) 
            {
                CTariff trff = (CTariff)dataGridView2.Rows[e.RowIndex].DataBoundItem;
                new FrmFee(trff).ShowDialog();
                UpdateDateGridViewFeeStandard();
            }
        }
        //增加收费标准
        private void button12_Click(object sender, EventArgs e)
        {
            new FrmFee().ShowDialog();
            UpdateDateGridViewFeeStandard();
        }

        private void btnsrc_Click(object sender, EventArgs e)
        {
            try 
            {
                DataTable dt;
                if (comboBox1.SelectedIndex > -1)
                {
                    int rit = Program.mng.SelectFixCardChargRcds(comboBox1.SelectedIndex, textBox1.Text.Trim(), dtpst.Value, dtpen.Value, out dt);
                    if (rit == 100) 
                    {
                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnrpt_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.DataSource != null) 
                {
                    FixIccdPayFee report = new FixIccdPayFee();
                    report.SetDataSource(dataGridView1.DataSource);
                    new FrmReportView(report).ShowDialog();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt;
                if (comboBox2.SelectedIndex > -1)
                {
                    int rit = Program.mng.SelectTempCardChargRcds(comboBox2.SelectedIndex, textBox3.Text.Trim(), dtptst.Value, dtpten.Value, out dt);   
                    if (rit == 100)
                    {
                        dataGridView3.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView3.DataSource != null)
                {
                    TempIccdPayFee report = new TempIccdPayFee();
                    report.SetDataSource(dataGridView3.DataSource);
                    new FrmReportView(report).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FrmGetFee_Load(object sender, EventArgs e)
        {
            dtpst.Value = DateTime.Now.AddDays(-3);
            dtpen.Value = DateTime.Now;
            dtptst.Value = DateTime.Now.AddDays(-3);
            dtpten.Value = DateTime.Now;
            comboBox1.SelectedIndex = 2;
            comboBox2.SelectedIndex = 3;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.Value != null)
                {
                    if (e.ColumnIndex == 2)
                    {
                        switch (Convert.ToInt16(e.Value))
                        {
                            case 2:
                                e.Value = "定期卡";
                                break;
                            case 1:
                                e.Value = "临时卡";
                                break;
                            case 3:
                                e.Value = "固定卡";
                                break;
                            default:
                                e.Value = "init";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //临时卡缴费出车
        private void btnOutCar_Click(object sender, EventArgs e)
        {
            try 
            {
                if (tempLog.ICCode == txtcd.Text.Trim() && txtcd.Text.Trim() != "")
                {
                    if (nupafee.Value >= Convert.ToDecimal(tempLog.RecivFee))
                    {
                        if (MessageBox.Show("是否确认缴费？", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                        {
                            int hnmb;
                            int rit = Program.mng.CreateTempICcardOut(txtcd.Text.Trim(), out hnmb);
                            switch (rit)
                            {
                                case 100:
                                    txtHall.Text = (hnmb % 10).ToString() + "#车厅";
                                    tempLog.ActualFee = Convert.ToSingle(nupafee.Value);
                                    tempLog.OperatorCode = Program.currOpr.Code;
                                    Program.mng.InsertTempCardChargeLog(tempLog);
                                    MessageBox.Show("请到"+txtHall.Text + "等待取车！");
                                    tempLog.ICCode = "";
                                    break;
                                case 101:
                                    MessageBox.Show("找不到存车车位！");
                                    break;
                                case 102:
                                    MessageBox.Show("没有可用的ETV或车厅");
                                    break;
                                case 103:
                                    MessageBox.Show("出车厅不是全自动状态！");
                                    break;
                                case 104:
                                    MessageBox.Show("ETV不是全自动状态！");
                                    break;
                                case 105:
                                    MessageBox.Show("该卡正在作业！");
                                    break;
                                case 106:
                                    MessageBox.Show("出车厅的作业已满！");
                                    break;
                                case 107:
                                    MessageBox.Show("无法出车，该卡已挂失（注销）！");
                                    break;
                                default:
                                    MessageBox.Show("系统异常：" + rit.ToString());
                                    break;
                            }
                            btnOutCar.Enabled = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("请输入正确的金额！");
                        nupafee.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("请先查询停车费用，再确认缴费出车！");
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region 唯拓顾客显示屏
        private readonly string dispCom = Properties.Settings.Default.VTopLedCom;

        private void openSerialPort() 
        {
            try
            {
                serialPort1.PortName = dispCom;
                serialPort1.BaudRate = 2400;
                serialPort1.WriteTimeout = 500;
                if (serialPort1.IsOpen)
                    serialPort1.Close();
                serialPort1.Open();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("顾客显示器串口打开异常："+ex.ToString());
            }
        }

        private void ClearDisplay()
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    Int16 i = 12;      //清屏
                    byte[] buffer = new byte[] { Convert.ToByte(i) };
                    serialPort1.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("清屏发生异常："+ex.ToString());
            }
        }
        /// <summary>
        /// 显示收费金额
        /// </summary>
        /// <param name="money"></param>
        private void Vtop_DisplayMoney(string money) 
        {
            try
            {
                openSerialPort();  //打开串口
                if (serialPort1.IsOpen)
                {
                    ClearDisplay(); //清屏

                    byte[] dataBuf = new byte[10];
                    int a = 0;
                    foreach (char mon in money)
                    {
                        dataBuf[a++] = Convert.ToByte(mon);
                    }
                    byte[] headBuf = new byte[] { Convert.ToByte(27), Convert.ToByte(81), Convert.ToByte(65) };
                    byte[] endBuf = new byte[] { Convert.ToByte(13) };

                    byte[] buffer = new byte[a + 4];
                    Array.Copy(headBuf, buffer, headBuf.Length);
                    Array.Copy(dataBuf, 0, buffer, 3, a);
                    Array.Copy(endBuf, 0, buffer, a + 3, 1);
                    serialPort1.Write(buffer, 0, buffer.Length);

                    byte[] LedBuffer = new byte[] { Convert.ToByte(2), Convert.ToByte(76), Convert.ToByte(48), Convert.ToByte(49), Convert.ToByte(48), Convert.ToByte(48) };
                    serialPort1.Write(LedBuffer, 0, LedBuffer.Length);
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

    }
}
