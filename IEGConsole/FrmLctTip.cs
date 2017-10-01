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
    public partial class FrmLctTip : Form
    {
        public FrmLctTip()
        { 
            InitializeComponent();
        }

        public FrmLctTip(CLocation lct)
            : this()
        {
            try
            {
                lblAddress.Text = lct.Address;
                lblSize.Text = lct.Size;
                if (lct.Type == EnmLocationType.Normal)
                {
                    if (lct.Status == EnmLocationStatus.Space || lct.Status == EnmLocationStatus.Init)
                    {
                        lblStatus.Text = "空闲";
                        lblIccode.Text = "";
                        lblType.Text = "";
                        lblIndtime.Text = "";
                        lblCarSize.Text = "";
                        lblDistance.Text = "";
                    }
                    else
                    {
                        if (lct.ICCardCode != "")
                        {
                            CICCard iccd = Program.mng.SelectByUserCode(lct.ICCardCode);
                            lblIccode.Text = iccd.Code;
                            string type = "";
                            if (iccd.Type == EnmICCardType.Temp)
                            {
                                type = "临时卡";
                            }
                            else if (iccd.Type == EnmICCardType.Fixed)
                            {
                                type = "定期卡";
                            }
                            else if (iccd.Type == EnmICCardType.FixedLocation)
                            {
                                type = "固定车位卡";
                            }
                            lblType.Text = type;
                        }
                        else
                        {
                            lblType.Text = "";
                            lblIccode.Text = "";
                        }
                        string status = "";
                        if (lct.Status == EnmLocationStatus.Entering)
                        {
                            status = "正在入库";
                        }
                        else if (lct.Status == EnmLocationStatus.Outing)
                        {
                            status = "正在出库";
                        }
                        else if (lct.Status == EnmLocationStatus.Occupy)
                        {
                            status = "占用";
                        }
                        else if (lct.Status == EnmLocationStatus.Temping)
                        {
                            status = "取物锁定";
                        }
                        lblStatus.Text = status;
                        lblIndtime.Text = lct.InDate.ToString();
                        lblCarSize.Text = lct.CarSize;
                        lblDistance.Text = lct.Distance.ToString();
                    }
                }
                else
                {
                    lblStatus.Text = "不可用";
                    lblIccode.Text = "";
                    lblType.Text = "";
                    lblIndtime.Text = "";
                    lblCarSize.Text = "";
                    lblDistance.Text = "";

                    if (lct.Status != EnmLocationStatus.Space && lct.ICCardCode != "")
                    {
                        CICCard iccd = Program.mng.SelectByUserCode(lct.ICCardCode);
                        lblIccode.Text = iccd.Code;
                        string type = "";
                        if (iccd.Type == EnmICCardType.Temp)
                        {
                            type = "临时卡";
                        }
                        else if (iccd.Type == EnmICCardType.Fixed)
                        {
                            type = "定期卡";
                        }
                        else if (iccd.Type == EnmICCardType.FixedLocation)
                        {
                            type = "固定车位卡";
                        }
                        lblType.Text = type;
                        lblIndtime.Text = lct.InDate.ToString();
                        lblCarSize.Text = lct.CarSize;
                        lblDistance.Text = lct.Distance.ToString();
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }     
    }
}
