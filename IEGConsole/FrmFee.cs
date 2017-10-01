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
    public partial class FrmFee : Form
    {
        private CTariff ctff;
        private int cID;

        public FrmFee()
        {
            InitializeComponent();
            ctff = new CTariff();
            ctff.Type = EnmICCardType.Temp;  //临时卡用
            ctff.Unit = EnmFeeUnit.Hour;
            cID = 0;
        } 

        public FrmFee(CTariff cta) : this() 
        {
            ctff = cta;
            cID = cta.ID;
        }

        private void FrmFee_Load(object sender, EventArgs e)
        {  
            if (ctff.Type == EnmICCardType.Temp)
            {
                groupBox1.Visible = false;
                groupBox3.Visible = true;
                if (cID == 0)
                {                   
                    rbcg.Checked = true;
                    rdNoBusy.Checked = true;
                }
                else 
                {
                    rblmt.Checked = ctff.FeeType == EnmFeeType.Limited ? true : false;
                    rbcg.Checked = ctff.FeeType == EnmFeeType.Charging ? true : false;
                    rbcg.Checked = ctff.FeeType == EnmFeeType.FirstCharge ? true : false;
                    if (ctff.ISbusy)
                    {
                        rdIsBusy.Checked = true;
                    }
                    else 
                    {
                        rdNoBusy.Checked = true;
                    }
                    txtfee.Text = ctff.Fee.ToString();
                    txttm.Text = ctff.Time.ToString();
                    if (ctff.Time ==0.5) 
                    {
                        txtfee.Enabled = false;
                        btnsv.Enabled = false;
                    }
                }
            }
            else  //定期卡
            {
                groupBox1.Visible = true;
                groupBox3.Visible = false;
                if (ctff.Unit == EnmFeeUnit.Month) 
                {
                    rdMonth.Checked = true;
                }
                else if (ctff.Unit == EnmFeeUnit.Season) 
                {
                    rdSeason.Checked = true;
                }
                else if (ctff.Unit == EnmFeeUnit.Year) 
                {
                    rdYear.Checked = true;
                }
                txtFixFee.Text = ctff.Fee.ToString();
            }
        }

        private void rdMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (rdMonth.Checked)
            {
                txtday.Text = "30";
            }
            else if (rdSeason.Checked)
            {
                txtday.Text = "90";
            }
            else if (rdYear.Checked)
            {
                txtday.Text = "360";
            }
        }

        private void btnsv_Click(object sender, EventArgs e)
        {
            if (cID == 0)   //添加新记录-只允许加临时卡的
            {
                //if (ctff.Type == EnmICCardType.Temp)
                //{
                //    if (rblmt.Checked)
                //    {
                //        ctff.FeeType = EnmFeeType.Limited;
                //    }
                //    else if (rbcg.Checked)
                //    {
                //        ctff.FeeType = EnmFeeType.Charging;
                //    }

                //    if (rdIsBusy.Checked)
                //    {
                //        ctff.ISbusy = true;
                //    }
                //    else if (rdNoBusy.Checked)
                //    {
                //        ctff.ISbusy = false;
                //    }

                //    ctff.Time = Convert.ToSingle(txttm.Text.Trim());
                //    ctff.Fee = Convert.ToSingle(txtfee.Text.Trim());
                //    int rit = Program.mng.InsertTariff(ctff);
                //    if (rit == 100)
                //    {
                //        this.Close();
                //    }
                //}
            }
            else  //修改
            {
                if (ctff.Type == EnmICCardType.Temp)
                {
                    if (rblmt.Checked)
                    {
                        ctff.FeeType = EnmFeeType.Limited;
                    }
                    else if (rbcg.Checked)
                    {
                        ctff.FeeType = EnmFeeType.Charging;
                    }

                    if (rdIsBusy.Checked)
                    {
                        ctff.ISbusy = true;
                    }
                    else if (rdNoBusy.Checked)
                    {
                        ctff.ISbusy = false;
                    }

                    ctff.Time = Convert.ToSingle(txttm.Text.Trim());
                    ctff.Fee = Convert.ToSingle(txtfee.Text.Trim());
                }
                else 
                {
                    if (rdMonth.Checked) 
                    {
                        ctff.Unit = EnmFeeUnit.Month;
                    }
                    else if (rdSeason.Checked) 
                    {
                        ctff.Unit = EnmFeeUnit.Season;
                    }
                    else if (rdYear.Checked) 
                    {
                        ctff.Unit = EnmFeeUnit.Year;
                    }

                    ctff.Time = Convert.ToSingle(txtday.Text);
                    ctff.Fee = Convert.ToSingle(txtFixFee.Text.Trim());
                }

                int rit = Program.mng.UpdateTariff(ctff);
                if (rit == 100) 
                {
                    this.Close();
                }
            }
        }
    }
}
