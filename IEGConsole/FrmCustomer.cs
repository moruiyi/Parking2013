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
    public partial class FrmCustomer : Form
    {
        private List<CCustomer> meCustomers;
        private BindingList<CCustomer> bindCustSource;

        private static readonly int iccdCom = IEGConsole.Properties.Settings.Default.ICCardCom;

        public FrmCustomer()
        {
            InitializeComponent();
        }

        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            UpdateDataGridView();

            btnLoss.Enabled = false;
            btnDisLoss.Enabled = false;
            btnDispose.Enabled = false;
            comboBox1.SelectedIndex = 4;
        }
        //查询
        private void btnCFind_Click(object sender, EventArgs e)
        {
            try
            {                
                if (comboBox1.SelectedIndex > -1)
                {
                    List<CCustomer> custs = new List<CCustomer>();
                    string str = txtFinddata.Text.Trim();
                    if (str != "")
                    {
                        switch (comboBox1.SelectedIndex)
                        {
                            case 0:
                                custs = meCustomers.FindAll(cu => cu.Code.Contains(str));
                                break;
                            case 1:
                                custs = meCustomers.FindAll(cu => cu.ICCardCode == str);
                                break;
                            case 2:
                                custs = meCustomers.FindAll(cu => cu.LctAddress == str);
                                break;
                            case 3:
                                custs = meCustomers.FindAll(cu => cu.Address.Contains(str));
                                break;                            
                        }
                    }
                    if (comboBox1.SelectedIndex == 4)
                    {
                        custs = meCustomers;
                    }

                    dataGridView1.DataSource = new BindingList<CCustomer>(custs);
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("查询异常："+ex.ToString());
            }
        }

        private void UpdateDataGridView() 
        {
            try
            {
                meCustomers = Program.mng.SelectAllCustomer().ToList();
                bindCustSource = new BindingList<CCustomer>(meCustomers);
                dataGridView1.DataSource = bindCustSource;               
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载异常：" + ex.ToString());
            }
        }

        private void btnCadd_Click(object sender, EventArgs e)
        {
            new FrmCustInfo().ShowDialog();
            UpdateDataGridView();
        }       

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1) 
            {
                CCustomer cut = (CCustomer)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                new FrmCustInfo(cut).ShowDialog();
                UpdateDataGridView();
            }
        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //读卡
        private void btnRead_Click(object sender, EventArgs e)
        {
            txtPhysicCode.Text = "";
            txtUseCode.Text = "";
            try
            {
                CICCardRW mcIccObj = new CIcCardRWOne(iccdCom, 9600, 0, 0);
                bool isConn = false;

                try
                {
                    isConn = mcIccObj.ConnectCOM();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("操作台刷卡器建立连接异常：" + ex.ToString());
                }

                if (isConn)
                {
                    int nback = 1;
                    Int16 nICType = 0;
                    uint ICNum = 0;
                    byte[] IcData = new byte[16];
                   
                    nback = mcIccObj.RequestICCard(ref nICType); //寻卡
                    if (nback == 0)
                    {
                        nback = mcIccObj.SelectCard(ref ICNum);  //读取物理卡号
                        if (nback == 0)
                        {
                            txtPhysicCode.Text = ICNum.ToString();
                            try
                            {
                                nback = mcIccObj.ReadCard(1, 0, ref IcData);  //读取指定扇区：1，指定DB块：0 的数据
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
                                    txtUseCode.Text = data.Substring(0, 4); //4位数卡号                                
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }
                    }                
                }

                mcIccObj.disConnectCOM();  //关闭刷卡器
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //制卡
        private void btnMake_Click(object sender, EventArgs e)
        {
            if (txtUseCode.Text.Trim().Length != 4) 
            {
                MessageBox.Show("请输入正确的4位数卡号！");
                return;
            }
            txtPhysicCode.Text = "";

            CICCardRW mcIccObj = new CIcCardRWOne(iccdCom, 38400, 0, 0);
            try
            {
                bool isConn = false;
                try
                {
                    isConn = mcIccObj.ConnectCOM();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("操作台刷卡器建立连接异常：" + ex.ToString());
                }

                if (isConn)
                {
                    int nback = 0;
                    Int16 nICType = 0;
                    uint ICNum = 0;
                    byte[] Icdata = new byte[16];
                    CICCard obj = null;

                    nback = mcIccObj.RequestICCard(ref nICType); //初始化寻卡功能
                    if (nback == 0)
                    {
                        nback = mcIccObj.SelectCard(ref ICNum);//读取卡号

                        if (nback == 0)
                        {
                            txtPhysicCode.Text = ICNum.ToString();

                            obj = new CICCard();
                            obj.Code = txtUseCode.Text.Trim();
                            obj.CreateDtime = DateTime.Now;
                            obj.Type = EnmICCardType.Temp;
                            obj.Status = EnmICCardStatus.Normal;
                            obj.PhysicCode = ICNum.ToString();

                            int rit = Program.mng.InsertICCard(obj);   //插入卡号
                            if (rit == 100)
                            {
                                //向IC卡芯片写入信息
                                string str = obj.Code + "1" + DateTime.Now.ToString("yyyyMMdd") + "01";//4位数卡号、类型、制卡时间、收费标准
                                str = str + "fffffffffffffffff";
                                byte[] b = new byte[16];
                                mcIccObj.TransStringTo16Char(str, ref b);

                                mcIccObj.WriteCard(1, 0, b, ref nback);  //1为扇区，0为扇区数据块   
                                if (nback == 0)
                                {
                                    MessageBox.Show("卡操作成功！");
                                }
                                else 
                                {
                                    MessageBox.Show("录入卡成功，但写操作失败："+nback.ToString());
                                }
                            }
                            else
                            {
                                switch (rit)
                                {
                                    case 101:
                                        MessageBox.Show("存在相同的用户卡号！");
                                        break;
                                    case 102:
                                        MessageBox.Show("出现重复的用户卡号！");
                                        break;
                                    default:
                                        MessageBox.Show("制卡异常，无法完成操作！");
                                        break;
                                }
                                txtUseCode.Text = "";
                            }
                        }
                    }

                    if (nback != 0)
                    {
                        MessageBox.Show("写数据失败，代码：" + nback.ToString(), "写操作失败");
                    }   
                }

                mcIccObj.disConnectCOM();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }
        
        //IC卡查询
        private void btnCheck_Click(object sender, EventArgs e)
        {
            Control.ControlCollection coll = groupBox4.Controls;
            for (int i = 0; i < coll.Count; i++) 
            {
                if (coll[i].GetType() == typeof(TextBox)) 
                {
                    coll[i].Text = "";
                }
            }
            btnLoss.Enabled = false;
            btnDisLoss.Enabled = false;
            btnDispose.Enabled = false;
            txtPhysicCode.Text = "";
            try
            {
                CICCard iccd = Program.mng.SelectByUserCode(txtUseCode.Text.Trim());
                if (iccd != null)
                {
                    groupBox4.Enabled = true;
                    txtcd.Text = iccd.Code;
                    txtmdt.Text = iccd.CreateDtime.ToString();
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
                            type = "固定卡";
                            break;
                        default:
                            type = "";
                            break;
                    }
                    txttype.Text = type;
                    string stat = "";
                    switch (iccd.Status)
                    {
                        case EnmICCardStatus.Normal:
                            stat = "正常";
                            btnLoss.Enabled = true;
                            btnDisLoss.Enabled = false;
                            btnDispose.Enabled = true;
                            break;
                        case EnmICCardStatus.Lost:
                            stat = "挂失";
                            btnLoss.Enabled = false;
                            btnDisLoss.Enabled = true;
                            btnDispose.Enabled = false;
                            break;
                        case EnmICCardStatus.Disposed:
                            stat = "注销";
                            btnLoss.Enabled = false;
                            btnDisLoss.Enabled = false;
                            btnDispose.Enabled = false;
                            break;
                        case EnmICCardStatus.Init:
                            stat = "Init";
                            break;
                    }
                    txtStat.Text = stat;
                    txtldt.Text = iccd.LossDtime.ToString();
                    txtddt.Text = iccd.DisposeDtime.ToString();
                    if (iccd.Type == EnmICCardType.FixedLocation)
                    {
                        textlct.Text = iccd.Address;
                    }

                }
                else 
                {
                    txtUseCode.Text = "";  //不存在此卡时，清除卡号
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常：" + ex.ToString());
            }
        }
        //挂失，注销
        private void btnLoss_Click(object sender, EventArgs e)
        {
            if (txtcd.Text.Trim() != "")
            {
                Button btn = (Button)sender;
                int rit = 0;
                if (btn == btnLoss) 
                {
                    rit=Program.mng.UpdateICCardStatus(txtcd.Text.Trim(),EnmICCardStatus.Lost);
                    if (rit == 100)
                    {
                        txtStat.Text = "挂失";
                        MessageBox.Show("挂失成功！");
                        btnLoss.Enabled = false;
                        btnDisLoss.Enabled = true;
                        btnDispose.Enabled = true;
                    }
                }
                else if (btn == btnDisLoss)
                {
                    rit = Program.mng.UpdateICCardStatus(txtcd.Text.Trim(), EnmICCardStatus.Normal);
                    if (rit == 100)
                    {
                        txtStat.Text = "正常";
                        MessageBox.Show("取消挂失成功！");
                        btnLoss.Enabled = true;
                        btnDisLoss.Enabled = false;
                        btnDispose.Enabled = true;
                    }
                }
                else if (btn == btnDispose)
                {
                    rit = Program.mng.UpdateICCardStatus(txtcd.Text.Trim(), EnmICCardStatus.Disposed);
                    if (rit == 100)
                    {
                        txtStat.Text = "注销";
                        MessageBox.Show("注销成功！");
                        btnLoss.Enabled = false;
                        btnDisLoss.Enabled = false;
                        btnDispose.Enabled = false;
                    }
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null) 
            {
                if (e.Value.GetType() == typeof(EnmICCardType)) 
                {
                    switch ((EnmICCardType)e.Value) 
                    {
                        case EnmICCardType.Temp:
                            e.Value = "临时卡";
                            break;
                        case EnmICCardType.Fixed:
                            e.Value = "定期卡";
                            break;
                        case EnmICCardType.FixedLocation:
                            e.Value = "固定卡";
                            break;
                        default:
                            break;
                    }
                }
                else if (e.Value.GetType() == typeof(EnmICCardStatus)) 
                {
                    switch ((EnmICCardStatus)e.Value) 
                    {
                        case EnmICCardStatus.Normal:
                            e.Value = "正常";
                            break;
                        case EnmICCardStatus.Disposed:
                            e.Value = "注销";
                            break;
                        case EnmICCardStatus.Lost:
                            e.Value = "挂失";
                            break;
                        default:
                            break;
                    }
                }
            }
        }    
       
    }
}
