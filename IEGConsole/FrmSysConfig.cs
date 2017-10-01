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
    public partial class FrmSysConfig : Form
    {
        public FrmSysConfig()
        {
            InitializeComponent();
        }

        private void btnExt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmSysConfig_Load(object sender, EventArgs e)
        {
            try 
            {
                this.updateSMGsStat();
                this.updateHallType();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("加载状态失败!"+ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            int smgID = 0;
            switch (comboBoxID.SelectedIndex) 
            {
                case 0:
                    smgID = 11;
                    break;
                case 1:
                    smgID = 12;
                    break;
                case 2:
                    smgID = 13;
                    break;
                case 3:
                    smgID = 14;
                    break;
                case 4:
                    smgID = 1;
                    break;
                case 5:
                    smgID = 2;
                    break;
                default:
                    smgID=0;
                    break;
            }
            if (smgID == 0) 
            {
                MessageBox.Show("请选择设备！");
                return;
            }
           
            Button btn = (Button)sender;
            int rit = 0;
            if (btn == btnEnable)
            {
                rit = Program.mng.UpdateSMGAvailable(smgID, true);
            }
            else if (btn == btnDis)
            {
                rit = Program.mng.UpdateSMGAvailable(smgID, false);
            }
            if (rit == 1)
            {
                CSMG[] smgs = Program.mng.SelectAllSMGs();
                updateSMGsStat();
            }
            
        }

        private void updateSMGsStat()
        {
             #region
             CSMG[] smgs = Program.mng.SelectAllSMGs();
             
             foreach (CSMG smg in smgs)
             {
                 if (smg.ID == 11)
                 {
                     if (smg.Available)
                     {
                         lblH1.BackColor = Color.LimeGreen;
                         lblH1.Text = "是";
                     }
                     else
                     {
                         lblH1.BackColor = Color.Red;
                         lblH1.Text = "否";
                     }
                 }
                 else if (smg.ID == 12)
                 {
                     if (smg.Available)
                     {
                         lblH2.BackColor = Color.LimeGreen;
                         lblH2.Text = "是";
                     }
                     else
                     {
                         lblH2.BackColor = Color.Red;
                         lblH2.Text = "否";
                     }
                 }
                 else if (smg.ID == 13)
                 {
                     if (smg.Available)
                     {
                         lblH3.BackColor = Color.LimeGreen;
                         lblH3.Text = "是";
                     }
                     else
                     {
                         lblH3.BackColor = Color.Red;
                         lblH3.Text = "否";
                     }
                 }
                 else if (smg.ID == 14)
                 {
                     if (smg.Available)
                     {
                         lblH4.BackColor = Color.LimeGreen;
                         lblH4.Text = "是";
                     }
                     else
                     {
                         lblH4.BackColor = Color.Red;
                         lblH4.Text = "否";
                     }
                 }
                 else if (smg.ID == 1)
                 {
                     if (smg.Available)
                     {
                         lblEtv1.BackColor = Color.LimeGreen;
                         lblEtv1.Text = "是";
                     }
                     else
                     {
                         lblEtv1.BackColor = Color.Red;
                         lblEtv1.Text = "否";
                     }
                 }
                 else if (smg.ID == 2)
                 {
                     if (smg.Available)
                     {
                         lblEtv2.BackColor = Color.LimeGreen;
                         lblEtv2.Text = "是";
                     }
                     else
                     {
                         lblEtv2.BackColor = Color.Red;
                         lblEtv2.Text = "否";
                     }
                 }
             }
             #endregion
        }

        private void updateHallType()
        {
            #region
            CSMG[] smgs = Program.mng.SelectSMGsOfType(EnmSMGType.Hall);
            
            foreach (CSMG smg in smgs)
            {
                if (smg.ID == 11)
                {
                    switch (smg.HallType)
                    {
                        case EnmHallType.Entance:
                            comboBoxH1.SelectedIndex = 0;
                            break;
                        case EnmHallType.Exit:
                            comboBoxH1.SelectedIndex = 1;
                            break;
                        case EnmHallType.EnterorExit:
                            comboBoxH1.SelectedIndex = 2;
                            break;
                    }
                }
                else if (smg.ID == 12)
                {
                    switch (smg.HallType)
                    {
                        case EnmHallType.Entance:
                            comboBoxH2.SelectedIndex = 0;
                            break;
                        case EnmHallType.Exit:
                            comboBoxH2.SelectedIndex = 1;
                            break;
                        case EnmHallType.EnterorExit:
                            comboBoxH2.SelectedIndex = 2;
                            break;
                    }
                }
                else if (smg.ID == 13)
                {
                    switch (smg.HallType)
                    {
                        case EnmHallType.Entance:
                            comboBoxH3.SelectedIndex = 0;
                            break;
                        case EnmHallType.Exit:
                            comboBoxH3.SelectedIndex = 1;
                            break;
                        case EnmHallType.EnterorExit:
                            comboBoxH3.SelectedIndex = 2;
                            break;
                    }
                }
                else if (smg.ID == 14)
                {
                    switch (smg.HallType)
                    {
                        case EnmHallType.Entance:
                            comboBoxH4.SelectedIndex = 0;
                            break;
                        case EnmHallType.Exit:
                            comboBoxH4.SelectedIndex = 1;
                            break;
                        case EnmHallType.EnterorExit:
                            comboBoxH4.SelectedIndex = 2;
                            break;
                    }
                }
            }
            #endregion
        }

        private void btnCon_Click(object sender, EventArgs e)
        {
            int rit = 0; 
            if (comboBoxH1.SelectedIndex > -1) 
            {
                rit += Program.mng.SetHallType(11, comboBoxH1.SelectedIndex);
            }
            if (comboBoxH3.SelectedIndex > -1) 
            {
                rit += Program.mng.SetHallType(13, comboBoxH3.SelectedIndex);
            }

            if (rit == 0) 
            {
                MessageBox.Show("更新车厅进出类型完成！");
                updateHallType();
            }
            else if (rit < 10)
            {
                MessageBox.Show("更新时产生异常！");
            }
            else
            {
                MessageBox.Show("须待车厅作业完成后方可设置！");
            }
        }


    }
}
