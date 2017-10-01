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
    public partial class FrmMain : Form
    {
        private readonly int ColNum = 20;  //物理列数
        private readonly int RowNum = 5;   //物理行数

        private static int vari = 0;

        public FrmMain()
        {
            InitializeComponent();

            for (int i = 0; i <= ColNum; i++) 
            {
                dataGridViewOne.Columns.Add("l" + i, i.ToString());
                dataGridViewOne.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewTwo.Columns.Add("l"+i,i.ToString());
                dataGridViewTwo.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 0; i < RowNum; i++) 
            {
                dataGridViewOne.Rows.Add();
                dataGridViewTwo.Rows.Add();
            }
            
            for (int i = 0; i < RowNum; i++) 
            {
                dataGridViewOne.Rows[i].Height = 40;
                dataGridViewTwo.Rows[i].Height = 40;
            }

            dataGridViewOne.Columns[0].HeaderText = "层";
            dataGridViewOne.Rows[0].Cells[0].Value = " 5";
            dataGridViewOne.Rows[1].Cells[0].Value = " 4";
            dataGridViewOne.Rows[2].Cells[0].Value = " 3";
            dataGridViewOne.Rows[3].Cells[0].Value = " 2";
            dataGridViewOne.Rows[4].Cells[0].Value = " 1";

            dataGridViewTwo.Columns[0].HeaderText = "层";
            dataGridViewTwo.Rows[0].Cells[0].Value = " 5";
            dataGridViewTwo.Rows[1].Cells[0].Value = " 4";
            dataGridViewTwo.Rows[2].Cells[0].Value = " 3";
            dataGridViewTwo.Rows[3].Cells[0].Value = " 2";
            dataGridViewTwo.Rows[4].Cells[0].Value = " 1";

            dataGridViewOne.Columns[0].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridViewTwo.Columns[0].DefaultCellStyle.BackColor = Color.LightGray;

            dataGridViewTwo.Rows[4].Cells[8].Value = " 1#";
            dataGridViewTwo.Rows[4].Cells[6].Value = " 2#";
            dataGridViewTwo.Rows[4].Cells[14].Value = " 3#";
            dataGridViewTwo.Rows[4].Cells[16].Value = " 4#";            
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            comboBoxType.SelectedIndex = 0;
            DialogResult result= new FrmLogin().ShowDialog();
            if (result == DialogResult.OK) 
            {
                setAuthority();
                vari = 1;

                timer1.Enabled = true;
                //更新车位及状态信息
                updateMonitior();
                dataGridViewOne.ClearSelection();
                dataGridViewTwo.ClearSelection();
            } 
            else
            {
                this.Close();                
            }            
        }      

        private void setAuthority()
        {
            btnConfig.Enabled = false;
            btnUser.Enabled = false;
            btnTempGetCar.Enabled = false;
            btnOperator.Enabled = false;
            btnManual.Enabled = false;

            if (Program.currOpr.Type == EnmOperatorType.Manager)
            {
                btnConfig.Enabled = true;                
                btnUser.Enabled = true;
                btnTempGetCar.Enabled = true;
                btnOperator.Enabled = true;
                btnManual.Enabled = true;
            }            
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (vari != 0)
            {
                this.Hide();
                DialogResult dr = new FrmExt().ShowDialog();               
                if (dr == DialogResult.Yes)
                {
                    if (new FrmLogin().ShowDialog() == DialogResult.OK)
                    {
                        e.Cancel = true;
                        this.Show();
                        setAuthority();
                    }                   
                }
                else if (dr == DialogResult.OK)
                {
                    e.Cancel = false;
                }
                else 
                {
                    e.Cancel = true;
                    this.Show();
                }
            }
        }

        /// <summary>
        /// 更新界面信息
        /// </summary>
        private void updateMonitior()
        {          
            #region  更新状态栏信息
            try
            {
                tlStripTime.Text = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss");
                tlstripUser.Text = Program.currOpr.Code;
                int total;
                int space;
                int occupy;
                int fixLct;
                int spaceBigLct;
                Program.mng.SelectLctofInfo(out total, out space, out occupy, out fixLct,out spaceBigLct);
                tlStripTotalNum.Text = total.ToString();
                tlStripSpaceLct.Text = space.ToString();
                tlStripOccupylct.Text = occupy.ToString();
                tlStripFixLct.Text = fixLct.ToString();
                tlstripBigLct.Text = spaceBigLct.ToString();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("更新状态栏信息异常：" + ex.ToString());
            }
            #endregion
            #region  更新车位信息
            try
            {
                DataGridViewCell dgvc = null;
                CLocation[] lctns = Program.mng.SelectAllLocations();
                if (lctns == null)
                {
                    return;
                }
                //1边
                foreach (DataGridViewRow dgvr in dataGridViewOne.Rows)  //获取行值
                {
                    foreach (DataGridViewCell dgvcell in dgvr.Cells)   //获取行单元格值
                    {
                        CLocation lct = Array.Find(lctns, delegate(CLocation c)
                        {
                            return c.Line == 1 && c.List == dgvcell.ColumnIndex
                                && c.Layer == 5 - dgvr.Index;
                        });
                        if (lct != null)
                        {
                            SetLocationStatusColor(lct, dgvcell);                        
                            dgvcell.Tag = lct;
                            if (lct.Status == EnmLocationStatus.Entering || lct.Status == EnmLocationStatus.Outing)
                            {
                                dgvc = dgvcell;
                            }
                        }
                        else
                        {
                            if (dgvcell.ColumnIndex != 0)
                            {
                                dgvcell.Style.BackColor = Color.Gray;
                            }
                        }
                    }
                }

                //二边
                foreach (DataGridViewRow dgvr in dataGridViewTwo.Rows)
                {
                    foreach (DataGridViewCell dgvcell in dgvr.Cells)
                    {
                        CLocation lct = Array.Find(lctns, (l => (
                            l.Line == 2 &&
                            l.Layer == 5 - dgvr.Index &&
                            l.List == dgvcell.ColumnIndex
                            )));
                        if (lct != null)
                        {
                            SetLocationStatusColor(lct, dgvcell);
                            dgvcell.Tag = lct;

                            if (lct.Status == EnmLocationStatus.Outing || lct.Status == EnmLocationStatus.Entering)
                            {
                                dgvc = dgvcell;
                            }
                        }
                        else
                        {
                            if (dgvcell.ColumnIndex != 0)
                            {
                                dgvcell.Style.BackColor = Color.Gray;
                            }
                        }
                    }
                }  
            }
            catch (Exception ex) 
            {
                MessageBox.Show("更新车位信息异常：" + ex.ToString());
            }
            #endregion                          
            #region  更新设备状态
            #region 车厅
            try
            {
                CSMG[] halls = Program.mng.SelectSMGsOfType(EnmSMGType.Hall);
                foreach (CSMG hall in halls)
                {
                    CStatCode[] HallStatCode = Program.mng.SelectStatusCodes(hall.ID);

                    string currS = "";
                    string mode = "";
                    if (HallStatCode != null)
                    {
                        if (HallStatCode[3].CurrentValue == 1)
                        {
                            currS = "进车";
                        }
                        else if (HallStatCode[3].CurrentValue == 2)
                        {
                            currS = "出车";
                        }

                        mode = CHelper.EqpModelFormatting(HallStatCode[4].CurrentValue);

                        if (HallStatCode[4].CurrentValue != 4 && hall.Available == true)
                        {
                            Program.mng.UpdateSMGStatus(hall.ID, false, EnmModel.Init);
                        }
                        else if (HallStatCode[4].CurrentValue == 4 && hall.Available == false)
                        {
                            Program.mng.UpdateSMGStatus(hall.ID, true, EnmModel.Init);
                        }
                    }

                    string setStat = "";
                    if (hall.HallType == EnmHallType.Entance)
                    {
                        setStat = "进";
                    }
                    else if (hall.HallType == EnmHallType.Exit)
                    {
                        setStat = "出";
                    }
                    else if (hall.HallType == EnmHallType.EnterorExit)
                    {
                        setStat = "进出";
                    }

                    string smode = mode + "/" + currS + "/" + setStat;
                    string sAvail = hall.Available ? "是" : "否";
                    switch (hall.ID)
                    {
                        case 11:
                            lblH1Model.Text = smode;   //模式标签
                            lblH1Avail.Text = sAvail; //可用性标签
                            if (HallStatCode != null)
                            {
                                if (HallStatCode[5].CurrentValue != 0)
                                {
                                    lblH1Auto.Text = "存车步进：";
                                    lblH1AutoStep.Text = HallStatCode[5].CurrentValue.ToString();
                                }
                                if (HallStatCode[6].CurrentValue != 0)
                                {
                                    lblH1Auto.Text = "取车步进：";
                                    lblH1AutoStep.Text = HallStatCode[6].CurrentValue.ToString();
                                }
                                if (HallStatCode[5].CurrentValue == 0 && HallStatCode[6].CurrentValue == 0)
                                {
                                    lblH1Auto.Text = "自动步进：";
                                    lblH1AutoStep.Text = "0";
                                }
                            }
                            break;
                        case 12:
                            lblH2Model.Text = smode;
                            lblH2Avail.Text = sAvail;
                            if (HallStatCode != null)
                            {
                                if (HallStatCode[5].CurrentValue != 0)
                                {
                                    lblH2Auto.Text = "存车步进：";
                                    lblH2AutoStep.Text = HallStatCode[5].CurrentValue.ToString();
                                }
                                if (HallStatCode[6].CurrentValue != 0)
                                {
                                    lblH2Auto.Text = "取车步进：";
                                    lblH2AutoStep.Text = HallStatCode[6].CurrentValue.ToString();
                                }
                                if (HallStatCode[5].CurrentValue == 0 && HallStatCode[6].CurrentValue == 0)
                                {
                                    lblH2Auto.Text = "自动步进：";
                                    lblH2AutoStep.Text = "0";
                                }
                            }
                            break;
                        case 13:
                            lblH3Model.Text = smode;
                            lblH3Avail.Text = sAvail;
                            if (HallStatCode != null)
                            {
                                if (HallStatCode[5].CurrentValue != 0)
                                {
                                    lblH3Auto.Text = "存车步进：";
                                    lblH3AutoStep.Text = HallStatCode[5].CurrentValue.ToString();
                                }
                                if (HallStatCode[6].CurrentValue != 0)
                                {
                                    lblH3Auto.Text = "取车步进：";
                                    lblH3AutoStep.Text = HallStatCode[6].CurrentValue.ToString();
                                }
                                if (HallStatCode[5].CurrentValue == 0 && HallStatCode[6].CurrentValue == 0)
                                {
                                    lblH3Auto.Text = "自动步进：";
                                    lblH3AutoStep.Text = "0";
                                }
                            }
                            break;
                        case 14:
                            lblH4Model.Text = smode;
                            lblH4Avail.Text = sAvail;
                            if (HallStatCode != null)
                            {
                                if (HallStatCode[5].CurrentValue != 0)
                                {
                                    lblH4Auto.Text = "存车步进：";
                                    lblH4AutoStep.Text = HallStatCode[5].CurrentValue.ToString();
                                }
                                if (HallStatCode[6].CurrentValue != 0)
                                {
                                    lblH4Auto.Text = "取车步进：";
                                    lblH4AutoStep.Text = HallStatCode[6].CurrentValue.ToString();
                                }
                                if (HallStatCode[5].CurrentValue == 0 && HallStatCode[6].CurrentValue == 0)
                                {
                                    lblH4Auto.Text = "自动步进：";
                                    lblH4AutoStep.Text = "0";
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("车厅更新设备状态异常："+ex.ToString());
            }
            #endregion
            #region ETV
            try
            {
                CSMG[] etvs = Program.mng.SelectSMGsOfType(EnmSMGType.ETV);
                if (etvs != null)
                {
                    foreach (CSMG smg in etvs)
                    {
                        string address = "";
                        string mode = "";
                        string autoStep = "";
                        string loadStep = "";
                        string unloadStep = "";

                        CStatCode[] etvStatCode = Program.mng.SelectStatusCodes(smg.ID);
                        if (etvStatCode != null && etvStatCode.Length > 10)
                        {
                            try
                            {
                                if (etvStatCode[1].CurrentValue != 0)
                                {
                                    address = etvStatCode[0].CurrentValue.ToString() + "边" + etvStatCode[1].CurrentValue.ToString() + "列" +
                                        etvStatCode[2].CurrentValue.ToString() + "层";
                                }
                                else
                                {
                                    if (smg.CurrAddress != null)
                                    {
                                        string caddress = smg.CurrAddress;
                                        if (caddress.Length == 4)
                                        {
                                            address = caddress.Substring(0, 1) + "边" + caddress.Substring(1, 2) + "列" + caddress.Substring(3) + "层";
                                        }
                                    }
                                }
                            }
                            catch (Exception ex) 
                            {
                                MessageBox.Show("更新地址异常："+ex.ToString());
                            }

                            try
                            {
                                mode = CHelper.EqpModelFormatting(etvStatCode[10].CurrentValue);
                                autoStep = etvStatCode[3].CurrentValue.ToString();
                                loadStep = etvStatCode[4].CurrentValue.ToString();
                                unloadStep = etvStatCode[5].CurrentValue.ToString();

                                if (etvStatCode[10].CurrentValue != 4 && smg.Available == true)
                                {
                                    Program.mng.UpdateSMGStatus(smg.ID, false, EnmModel.Init);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("更新自动步异常：" + ex.ToString());
                            }
                        }
                        string sAvail = smg.Available ? "是" : "否";

                        switch (smg.ID)
                        {
                            case 1:
                                lblEtv1CurrAddrs.Text = address;
                                lblEtv1Model.Text = mode;
                                lblEtv1Avail.Text = sAvail;
                                lblEtv1AutoStep.Text = autoStep;
                                lblEtv1Loadstep.Text = loadStep;
                                lblEtv1UnloadStep.Text = unloadStep;
                                break;
                            case 2:
                                lblEtv2CurrAddrs.Text = address;
                                lblEtv2Model.Text = mode;
                                lblEtv2Avail.Text = sAvail;
                                lblEtv2AutoStep.Text = autoStep;
                                lblEtv2Loadstep.Text = loadStep;
                                lblEtv2UnloadStep.Text = unloadStep;
                                break;
                            default:
                                break;
                        }
                        //移动标签
                        if (etvStatCode != null && etvStatCode.Length >10)
                        {                           
                            if (smg.ID == 1)
                            {
                                try
                                {
                                    if (etvStatCode[10].CurrentValue == 4)
                                    {
                                        if (smg.nIsWorking != 0)
                                        {
                                            lblEtv1Dis.BackColor = Color.Pink;
                                        }
                                        else
                                        {
                                            if (smg.MTskID != 0)
                                            {
                                                lblEtv1Dis.BackColor = Color.MistyRose;
                                            }
                                            else
                                            {
                                                lblEtv1Dis.BackColor = Color.PaleGreen;
                                            }
                                        }
                                    }
                                    else if (etvStatCode[10].CurrentValue != 4)
                                    {
                                        lblEtv1Dis.BackColor = Color.Chocolate;
                                    }
                                }
                                catch (Exception ex) 
                                {
                                    MessageBox.Show("更新标签1颜色异常："+ex.ToString());
                                }   
                      
                                Rectangle r = dataGridViewOne.GetCellDisplayRectangle(etvStatCode[1].CurrentValue == 0 ? 1 : etvStatCode[1].CurrentValue, 0, false);
                                lblEtv1Dis.Location = new Point(r.X + 29, lblEtv1Dis.Location.Y);
                            }
                            else if (smg.ID == 2)
                            {
                                try
                                {
                                    if (etvStatCode[10].CurrentValue == 4)
                                    {
                                        if (smg.nIsWorking != 0)
                                        {
                                            lblEtv2Dis.BackColor = Color.Pink;
                                        }
                                        else
                                        {
                                            if (smg.MTskID != 0)
                                            {
                                                lblEtv2Dis.BackColor = Color.MistyRose;
                                            }
                                            else
                                            {
                                                lblEtv2Dis.BackColor = Color.PaleGreen;
                                            }
                                        }
                                    }
                                    else if (etvStatCode[10].CurrentValue != 4)
                                    {
                                        lblEtv2Dis.BackColor = Color.Chocolate;
                                    }
                                }
                                catch (Exception ex) 
                                {
                                    MessageBox.Show("更新标签2颜色异常：" + ex.ToString());
                                }
                                Rectangle r = dataGridViewOne.GetCellDisplayRectangle(etvStatCode[1].CurrentValue == 0 ? 1 : etvStatCode[1].CurrentValue, 0, false);
                                lblEtv2Dis.Location = new Point(r.X + 29, lblEtv2Dis.Location.Y);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("ETV更新设备状态异常："+ex.ToString());
            }
            #endregion
            #endregion
            #region  更新作业状态
            try
            {
                #region Hall1
                int count = 0;
                count = Program.mng.GetMasterTaskCountOfHid(11);
                lblH1MtskNum.Text = count.ToString();
                if (count > 0)
                {
                    CMasterTask mtsk = Program.mng.GetCurrentMasterTaskOfSMG(11);
                    if (mtsk != null)
                    {
                        #region
                        lblH1TskType.Text = CHelper.MtskTypeFormat(mtsk.Type) + "/" + mtsk.ICCardCode;
                        if (mtsk.Tasks[0].SMGType == EnmSMGType.Hall)
                        {
                            lblH1State1.Text = CHelper.TaskStatusFormatting(mtsk.Tasks[0].Status);
                        }
                        else
                        {
                            if (mtsk.Tasks[0].SMG == 1)
                            {
                                lblH1State1.Text = "ETV1" + CHelper.TaskStatusFormatting(mtsk.Tasks[0].Status);
                            }
                            else 
                            {
                                lblH1State1.Text = "ETV2" + CHelper.TaskStatusFormatting(mtsk.Tasks[0].Status);
                            }
                        }

                        if (mtsk.Tasks.Length > 1)
                        {
                            if (mtsk.Tasks[1].SMG == 1)
                            {
                                lblH1State2.Text = "ETV1" + CHelper.TaskStatusFormatting(mtsk.Tasks[1].Status);
                            }
                            else
                            {
                                lblH1State2.Text = "ETV2" + CHelper.TaskStatusFormatting(mtsk.Tasks[1].Status);
                            }
                        }
                        else 
                        {
                            lblH1State2.Text = "待命";
                        }
                        #endregion
                    }
                    else
                    {
                        lblH1TskType.Text = "空";
                        lblH1State1.Text = "待命";
                        lblH1State2.Text = "待命";
                    }
                }
                else
                {
                    lblH1TskType.Text = "空";
                    lblH1State1.Text = "待命";
                    lblH1State2.Text = "待命";
                }
                #endregion
                #region Hall2
                int count1 = 0;
                count1 = Program.mng.GetMasterTaskCountOfHid(12);
                lblH2MtskNum.Text = count1.ToString();
                if (count1 > 0)
                {
                    CMasterTask mtsk = Program.mng.GetCurrentMasterTaskOfSMG(12);
                    if (mtsk != null)
                    {                
                        lblH2TskType.Text = CHelper.MtskTypeFormat(mtsk.Type) + "/" + mtsk.ICCardCode;
                        #region
                        if (mtsk.Tasks[0].SMGType == EnmSMGType.Hall)
                        {
                            lblH2State1.Text = CHelper.TaskStatusFormatting(mtsk.Tasks[0].Status);
                        }
                        else 
                        {
                            if (mtsk.Tasks[0].SMG == 1)
                            {
                                lblH2State1.Text = "ETV1" + CHelper.TaskStatusFormatting(mtsk.Tasks[0].Status);
                            }
                            else 
                            {
                                lblH2State1.Text = "ETV2" + CHelper.TaskStatusFormatting(mtsk.Tasks[0].Status);
                            }
                        }

                        if (mtsk.Tasks.Length > 1)
                        {
                            if (mtsk.Tasks[1].SMG == 1)
                            {
                                lblH2State2.Text = "ETV1" + CHelper.TaskStatusFormatting(mtsk.Tasks[1].Status);
                            }
                            else
                            {
                                lblH2State2.Text = "ETV2" + CHelper.TaskStatusFormatting(mtsk.Tasks[1].Status);
                            }
                        }
                        else 
                        {
                            lblH2State2.Text = "待命";
                        }
                        #endregion
                    }
                    else
                    {
                        lblH2TskType.Text = "空";
                        lblH2State1.Text = "待命";
                        lblH2State2.Text = "待命";
                    }
                }
                else
                {
                    lblH2TskType.Text = "空";
                    lblH2State1.Text = "待命";
                    lblH2State2.Text = "待命";
                }
                #endregion
                #region Hall3
                int count3 = 0;
                count3 = Program.mng.GetMasterTaskCountOfHid(13);
                lblH3MtskNum.Text = count3.ToString();
                if (count3 > 0)
                {
                    CMasterTask mtsk = Program.mng.GetCurrentMasterTaskOfSMG(13);
                    if (mtsk != null)
                    {                     
                        lblH3TskType.Text = CHelper.MtskTypeFormat(mtsk.Type) + "/" + mtsk.ICCardCode;
                        #region
                        if (mtsk.Tasks[0].SMGType == EnmSMGType.Hall)
                        {
                            lblH3State1.Text = CHelper.TaskStatusFormatting(mtsk.Tasks[0].Status);
                        }
                        else 
                        {
                            if (mtsk.Tasks[0].SMG == 1)
                            {
                                lblH3State1.Text = "ETV1" + CHelper.TaskStatusFormatting(mtsk.Tasks[0].Status);
                            }
                            else 
                            {
                                lblH3State1.Text = "ETV2" + CHelper.TaskStatusFormatting(mtsk.Tasks[0].Status);
                            }
                        }

                        if (mtsk.Tasks.Length > 1)
                        {
                            if (mtsk.Tasks[1].SMG == 1)
                            {
                                lblH3State2.Text = "ETV1" + CHelper.TaskStatusFormatting(mtsk.Tasks[1].Status);
                            }
                            else
                            {
                                lblH3State2.Text = "ETV2" + CHelper.TaskStatusFormatting(mtsk.Tasks[1].Status);
                            }
                        }
                        else 
                        {
                            lblH3State2.Text = "待命";
                        }
                        #endregion
                    }
                    else
                    {
                        lblH3TskType.Text = "空";
                        lblH3State1.Text = "待命";
                        lblH3State2.Text = "待命";
                    }
                }
                else
                {
                    lblH3TskType.Text = "空";
                    lblH3State1.Text = "待命";
                    lblH3State2.Text = "待命";
                }
                #endregion
                #region Hall4
                int count4 = 0;
                count4 = Program.mng.GetMasterTaskCountOfHid(14);
                lblH4MtskNum.Text = count4.ToString();
                if (count4 > 0)
                {
                    CMasterTask mtsk = Program.mng.GetCurrentMasterTaskOfSMG(14);
                    if (mtsk != null)
                    {                                
                        lblH4TskType.Text = CHelper.MtskTypeFormat(mtsk.Type) + "/" + mtsk.ICCardCode;
                        #region 作业状态
                        if (mtsk.Tasks[0].SMGType == EnmSMGType.Hall)
                        {
                            lblH4State1.Text = CHelper.TaskStatusFormatting(mtsk.Tasks[0].Status);
                        }
                        else 
                        {
                            if (mtsk.Tasks[0].SMG == 1)
                            {
                                lblH4State1.Text = "ETV1" + CHelper.TaskStatusFormatting(mtsk.Tasks[0].Status);
                            }
                            else 
                            {
                                lblH4State1.Text = "ETV2" + CHelper.TaskStatusFormatting(mtsk.Tasks[0].Status);
                            }
                        }

                        if (mtsk.Tasks.Length > 1)
                        {
                            if (mtsk.Tasks[1].SMG == 1)
                            {
                                lblH4State2.Text = "ETV1" + CHelper.TaskStatusFormatting(mtsk.Tasks[1].Status);
                            }
                            else
                            {
                                lblH4State2.Text = "ETV2" + CHelper.TaskStatusFormatting(mtsk.Tasks[1].Status);
                            }
                        }
                        else 
                        {
                            lblH4State2.Text = "待命";
                        }
                        #endregion
                    }
                    else
                    {
                        lblH4TskType.Text = "空";
                        lblH4State1.Text = "待命";
                        lblH4State2.Text = "待命";
                    }
                }
                else
                {
                    lblH4TskType.Text = "空";
                    lblH4State1.Text = "待命";
                    lblH4State2.Text = "待命";
                }
                #endregion

                #region ETV1
                CTask tsk1 = Program.mng.GetCurrentTaskOfSMG(1);
                if (tsk1 != null)
                {
                    lblEtv1State.Text = CHelper.TaskStatusFormatting(tsk1.Status);
                    lblEtv1To.Text = "卡" + tsk1.ICCardCode + "/源" + tsk1.FromLctAdrs + "/往" + tsk1.ToLctAdrs;
                }
                else
                {
                    lblEtv1State.Text = "待命";
                    lblEtv1To.Text = "";
                }
                #endregion
                #region ETV2
                CTask tsk2 = Program.mng.GetCurrentTaskOfSMG(2);
                if (tsk2 != null)
                {
                    lblEtv2State.Text = CHelper.TaskStatusFormatting(tsk2.Status);
                    lblEtv2To.Text = "卡" + tsk2.ICCardCode + "/源" + tsk2.FromLctAdrs + "/往" + tsk2.ToLctAdrs;
                }
                else
                {
                    lblEtv2State.Text = "待命";
                    lblEtv2To.Text = "";
                }
                #endregion
            }
            catch (Exception ex) 
            {
                MessageBox.Show("更新作业状态异常：" + ex.ToString());
            }
            #endregion
            #region   更新状态按钮颜色
            UpdateBtnInfoColor(btnHall1Info, 11);
            UpdateBtnInfoColor(btnHall2Info, 12);
            UpdateBtnInfoColor(btnHall3Info, 13);
            UpdateBtnInfoColor(btnHall4Info, 14);
            UpdateBtnInfoColor(btnEtv1Info, 1);
            UpdateBtnInfoColor(btnEtv2Info, 2);
            #endregion          
        }

        /// <summary>
        /// 设定对应车位单元格颜色
        /// </summary>
        /// <param name="lct"></param>
        /// <param name="dgvc"></param>
        private void SetLocationStatusColor(CLocation lct, DataGridViewCell dgvc) 
        {
            if (lct.Type == EnmLocationType.Normal) 
            {
                switch (lct.Status) 
                {
                    case EnmLocationStatus.Space:
                        dgvc.Style.BackColor = Color.LightYellow;
                        break;
                    case EnmLocationStatus.Occupy:
                        dgvc.Style.BackColor = Color.Purple;
                        break;
                    case EnmLocationStatus.Entering:
                        dgvc.Style.BackColor = Color.Violet;
                        break;
                    case EnmLocationStatus.Outing:
                        dgvc.Style.BackColor = Color.SkyBlue;
                        break;
                    case EnmLocationStatus.Temping:     //取物后车位被占用状态
                        dgvc.Style.BackColor = Color.Wheat;
                        break;
                    default:
                        dgvc.Style.BackColor = Color.Gray;
                        break;
                }
            }
            else if (lct.Type == EnmLocationType.Hall) 
            {
                dgvc.Style.BackColor = Color.DarkKhaki;
            }
            else if (lct.Type == EnmLocationType.Disable && lct.Status != EnmLocationStatus.Occupy) 
            {
                dgvc.Style.BackColor = Color.DarkGray;
            }
            else if (lct.Type == EnmLocationType.Invalid) 
            {
                dgvc.Style.BackColor = Color.DimGray;
            }
            dgvc.Style.SelectionBackColor = dgvc.Style.BackColor;
        }

        /// <summary>
        /// 更新状态按钮颜色
        /// </summary>       
        private void UpdateBtnInfoColor(Button btn, int smg) 
        {
            bool flg = Program.mng.JudgeEqpHasErrorBit(smg);
            if (flg)
            {
                btn.BackColor = Color.Red;
            }
            else 
            {
                btn.BackColor = Color.Lime;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            updateMonitior();
            timer1.Enabled = true;
        }        

        //单击DataGridView
        private void dataGridViewOne_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {           
            if (e.Button == MouseButtons.Left)
            {
                if (e.ColumnIndex > 0 && e.RowIndex > -1)
                {
                    DataGridView dgvw = (DataGridView)sender;
                    DataGridViewCell cell = dgvw.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    cell.Style.SelectionBackColor = cell.Style.BackColor;
                }
            }
        }
        //状态按钮
        private void btnEqpStatuInfo_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn == btnHall1Info) 
            {
                new FrmStatus(0).ShowDialog();
            }
            else if (btn == btnHall2Info)
            {
                new FrmStatus(0).ShowDialog();
            }
            else if (btn == btnHall3Info)
            {
                new FrmStatus(1).ShowDialog();
            }
            else if (btn == btnHall4Info)
            {
                new FrmStatus(1).ShowDialog();
            }
            else if (btn == btnEtv1Info) 
            {
                new FrmStatus(2).ShowDialog();
            }
            else if (btn == btnEtv2Info)
            {
                new FrmStatus(3).ShowDialog();
            }
        }

        //手动指令
        private void btnManual_Click(object sender, EventArgs e)
        {
            #region
            if (comboBoxType.SelectedIndex != -1)
            {
                if (txtFromAddrs.Text.Trim() != "" && txtToAddrs.Text.Trim() != "")
                {
                    EnmMasterTaskType mtype=EnmMasterTaskType.SaveCar;
                    if (comboBoxType.SelectedIndex == 0) 
                    {
                        mtype = EnmMasterTaskType.GetCar;
                    }
                    else if (comboBoxType.SelectedIndex == 1) 
                    {
                        mtype = EnmMasterTaskType.Move;
                    }
                    else if (comboBoxType.SelectedIndex == 2) 
                    {
                        mtype = EnmMasterTaskType.Transpose;
                    }

                    if (mtype != EnmMasterTaskType.SaveCar)
                    {
                        int hallID;
                        int rit = Program.mng.CreateManageMasterTask(txtFromAddrs.Text.Trim(), txtToAddrs.Text.Trim(), mtype, out hallID);
                        if (hallID != 0) 
                        {
                            txtOutHall.Text = hallID.ToString() + "#车厅";
                        }
                        #region
                        switch (rit) 
                        {
                            case 100:
                                MessageBox.Show("作业添加成功，请稍后！");
                                break;
                            case 101:
                                MessageBox.Show("请等待其它作业完成后，再添加！");
                                break;
                            case 102:
                                MessageBox.Show("请输入正确的源地址及目的地址！");
                                break;
                            case 103:
                                MessageBox.Show("出库时目的地址必须是车厅！");
                                break;
                            case 104:
                                MessageBox.Show("当前车厅不可用！");
                                break;
                            case 105:
                                MessageBox.Show("当前车厅不处于全自动模式！");
                                break;
                            case 110:
                                MessageBox.Show("源地址或目的地址不允许为车厅地址！");
                                break;
                            case 111:
                                MessageBox.Show("源地址或目的地址存储卡号为空！");
                                break;
                            case 112:
                                MessageBox.Show("源地址或目的地址车位不允许挪移！");
                                break;
                            case 113:
                                MessageBox.Show("目的地址车位尺寸不适合！");
                                break;
                            case 114:
                                MessageBox.Show("找不到合适的ETV！");
                                break;
                            case 120:
                                MessageBox.Show("源地址请输入ETV所在列地址！");
                                break;
                            case 121:
                                MessageBox.Show("当前ETV不处于全自动模式！");
                                break;
                            case 130:
                                MessageBox.Show("源车位的用户卡号为空！");
                                break;
                            case 131:
                                MessageBox.Show("当前车位不允许出库！");
                                break;
                            case 132:
                                MessageBox.Show("没有可用的ETV！");
                                break;
                            default:
                                MessageBox.Show(rit.ToString());
                                break;
                        }
                        #endregion
                    }
                }
                else
                {
                    MessageBox.Show("源地址及目的地址都不允许为空！");
                }
            }
            else 
            {
                MessageBox.Show("请选择正确的作业类型！");
            }
            #endregion
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            new FrmMtc().ShowDialog();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            new FrmSysConfig().ShowDialog();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            new FrmCustomer().ShowDialog();
        }

        private void btnOperator_Click(object sender, EventArgs e)
        {
            new FrmOperator().ShowDialog();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            new FrmSysLog().ShowDialog();
        }

        private void btnTempGetCar_Click(object sender, EventArgs e)
        {
            new FrmTempGetCar().ShowDialog();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            new FrmGetFee().ShowDialog();
        }

        private void 车位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CLocation lct = (CLocation)contextMenuStripLctn.Tag;
            if (lct != null)
            {
                new FrmLctTip(lct).ShowDialog();
            }
        }

        private void 车位禁用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CLocation lct = (CLocation)contextMenuStripLctn.Tag;
            if (lct != null)
            {
                string line = lct.Address.Substring(0, 1);
                string list = int.Parse(lct.Address.Substring(1, 2)).ToString();
                string layer = lct.Address.Substring(3, 1);
                DialogResult dr = MessageBox.Show("是否要禁用车位： " + line + "边" + list + "列" + layer + "层", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                if (dr == DialogResult.OK)
                {
                    DisplayHint(Program.mng.ManualDisLocation(lct.Address));
                }
            }
        }

        private void 车位启用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CLocation lct = (CLocation)contextMenuStripLctn.Tag;
            if (lct != null)
            {
                string line = lct.Address.Substring(0, 1);
                string list = int.Parse(lct.Address.Substring(1, 2)).ToString();
                string layer = lct.Address.Substring(3, 1);
                DialogResult dr = MessageBox.Show("是否要启用车位： " + line + "边" + list + "列" + layer + "层", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                if (dr == DialogResult.OK)
                {
                    DisplayHint(Program.mng.ManualEnableLocation(lct.Address));
                }
            }
        }

        private void 数据入库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CLocation lct = (CLocation)contextMenuStripLctn.Tag;
            if (lct != null)
            {
                new FrmManualIn(lct).ShowDialog();
            }
        }

        private void 数据出库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CLocation lct = (CLocation)contextMenuStripLctn.Tag;
            if (lct != null)
            {
                string line = lct.Address.Substring(0, 1);
                string list = int.Parse(lct.Address.Substring(1, 2)).ToString();
                string layer = lct.Address.Substring(3, 1);
                DialogResult dr = MessageBox.Show("请确认车位上无车辆！出库车位： " + line + "边" + list + "列" + layer + "层", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                if (dr == DialogResult.OK)
                {
                    int rit = Program.mng.ManualOutLocation(lct.Address);
                    switch (rit)
                    {
                        case 0:
                            MessageBox.Show("车位出库异常！");
                            break;
                        case 101:
                            MessageBox.Show("车位地址不正确！");
                            break;
                        case 102:
                            MessageBox.Show("该车位为无效车位或已禁用！");
                            break;
                        case 103:
                            MessageBox.Show("该车位未存车！");
                            break;
                    }
                }
            }
        }

        private void DisplayHint(int rit)
        {
            switch (rit)
            {
                case 0:
                    MessageBox.Show("车位操作异常！");
                    break;
                case 101:
                    MessageBox.Show("车位地址不正确！");
                    break;
                case 102:
                    MessageBox.Show("当前车位无法操作！");
                    break;
            }
        }

        private void dataGridViewOne_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex > 0 && e.RowIndex > -1)
                {
                    DataGridView dgv = (DataGridView)sender;
                    DataGridViewCell cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    cell.ContextMenuStrip = contextMenuStripLctn;
                    CLocation lct = (CLocation)cell.Tag;
                    if (lct != null)
                    {
                        contextMenuStripLctn.Tag = lct;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridViewTwo_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex > 0 && e.RowIndex > -1)
            {
                DataGridView dgv = (DataGridView)sender;
                CLocation lct = (CLocation)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                if (lct != null && lct.Type != EnmLocationType.Invalid)
                {
                    txtOutHall.Text = "";
                    if (txtFromAddrs.Text.Trim() == "")
                    {
                        txtFromAddrs.Text = lct.Address;
                        return;
                    }
                    if (txtToAddrs.Text.Trim() == "")
                    {
                        txtToAddrs.Text = lct.Address;
                    }
                    if (txtFromAddrs.Text.Trim() != "" && txtToAddrs.Text.Trim() != "") 
                    {
                        txtToAddrs.Text = lct.Address;
                    }
                }
            }
        }
       
    }
}
