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
    public partial class FrmTempGetCar : Form
    {
        private readonly int iccdCom = IEGConsole.Properties.Settings.Default.ICCardCom;

        public FrmTempGetCar()
        {
            InitializeComponent();
        }

        private void FrmTempGetCar_Load(object sender, EventArgs e)
        {
            CSMG[] halls = Program.mng.SelectSMGsOfType(EnmSMGType.Hall);
            foreach (CSMG smg in halls) 
            {
                if (smg.HallType == EnmHallType.EnterorExit) 
                {
                    comboBox1.Items.Add((smg.ID % 10).ToString() + "#车厅");
                }
            }
            if (comboBox1.Items.Count > 0) 
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            CICCardRW iccdObj = new CIcCardRWOne(iccdCom, 9600, 0, 0);
            bool isConn = false;

            try 
            {
                isConn = iccdObj.ConnectCOM();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("刷卡器建立异常："+ex.ToString());
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
                                textBox1.Text = data.Substring(0, 4); //4位数卡号                            
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
        }

        private void btnTempGet_Click(object sender, EventArgs e)
        {            
            if (textBox1.Text.Trim() == "") 
            {
                return;
            }
            if (comboBox1.SelectedIndex == -1) 
            {
                MessageBox.Show("请选择出车车厅！");
                comboBox1.Text = "";
                return;
            }
            int hallID = 0;
            if (comboBox1.SelectedItem.ToString() == "1#车厅") 
            {
                hallID = 11;
            }
            else if (comboBox1.SelectedItem.ToString() == "3#车厅") 
            {
                hallID = 13;
            }
            if (hallID != 0)
            {
                int rit = Program.mng.DealOTempGetCar(textBox1.Text.Trim(), hallID);
                #region
                switch (rit) 
                {
                    case 100:
                        MessageBox.Show("请至"+(hallID%10).ToString()+"#车厅等待出车！");
                        break;
                    case 101:
                        MessageBox.Show("请确认当前卡号已存车！");
                        break;
                    case 102:
                        MessageBox.Show("所选车厅模式不是进出车厅！");
                        break;
                    case 103:
                        MessageBox.Show("所选车厅不处于全自动状态！");
                        break;
                    case 104:
                        MessageBox.Show("当前车厅不可接收新指令，无法添加作业！");
                        break;
                    case 105:
                        MessageBox.Show("该卡正在作业，请稍后！");
                        break;
                    case 106:
                        MessageBox.Show("请等待车厅的其他取车作业完成后，再进行！");
                        break;
                    case 107:
                        MessageBox.Show("该卡不是第统卡！");
                        break;
                    case 108:
                        MessageBox.Show("没有可用的ETV！"); 
                        break;
                    case 109:
                        MessageBox.Show("该卡不存在！");
                        break;
                    case 110:
                        MessageBox.Show("该卡已挂失（注销）！");
                        break;
                    default :
                        MessageBox.Show("操作引发异常！");
                        break;
                }
                #endregion
            }
            else 
            {
                MessageBox.Show("没有可用车厅！");
            } 
        }

        private void btnExt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
