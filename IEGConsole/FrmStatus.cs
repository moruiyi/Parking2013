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
    public partial class FrmStatus : Form
    {

        private Control.ControlCollection hall1CC;
        private Control.ControlCollection hall2CC;
        private Control.ControlCollection hall3CC;
        private Control.ControlCollection hall4CC;
        private Control.ControlCollection Etv1CC;
        private Control.ControlCollection Etv2CC;
      

        public FrmStatus()
        {
            InitializeComponent();
        }

        public FrmStatus(int idx) : this() 
        {
            tabControl1.SelectedIndex = idx;
        }

        private void FrmStatus_Load(object sender, EventArgs e)
        {
            this.timerSt.Enabled = false;
            hall1CC = groupBoxH1.Controls;
            hall2CC = groupBoxH2.Controls;
            hall3CC = groupBoxH3.Controls;
            hall4CC = groupBoxH4.Controls;
            Etv1CC = groupBoxEtv1.Controls;
            Etv2CC = groupBoxEtv2.Controls;

            DisplayAllErrDescp();
            InitAllStatValue();
            this.timerSt.Enabled = true;
        }

        private void timerSt_Tick(object sender, EventArgs e)
        {
            this.timerSt.Enabled = false;
            updateStatusInfo();
            this.timerSt.Enabled = true;
        }


        private void FrmStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timerSt.Enabled = false;
        }

        private void DisplayAllErrDescp()
        {
            try
            {
                //HALL1
                CErrorCode[] hall1 = Program.mng.LoadErrorCodeDescp(11);
                if (hall1 != null)
                {
                    int hc = 0;
                    int cc = 0;
                    while (hc < hall1.Length && cc < hall1CC.Count)
                    {
                        hall1CC[cc].Name = "l" + hall1[hc].StartBit;
                        hall1CC[cc].Text = hall1[hc].Description;
                        hc++;
                        cc++;
                    }
                }
                else 
                {
                    MessageBox.Show("无法连接到服务器！");
                }

                //hall2
                CErrorCode[] hall2 = Program.mng.LoadErrorCodeDescp(12);
                if (hall2 != null)
                {
                    int hc = 0;
                    int cc = 0;
                    while (hc < hall2.Length && cc < hall2CC.Count)
                    {
                        hall2CC[cc].Name = "l" + hall2[hc].StartBit;
                        hall2CC[cc].Text = hall2[hc].Description;
                        hc++;
                        cc++;
                    }
                }
                else
                {
                    MessageBox.Show("无法连接到服务器！");
                }
                //hall3
                CErrorCode[] hall3 = Program.mng.LoadErrorCodeDescp(13);
                if (hall3 != null)
                {
                    int hc = 0;
                    int cc = 0;
                    while (hc < hall3.Length && cc < hall3CC.Count)
                    {
                        hall3CC[cc].Name = "l" + hall3[hc].StartBit;
                        hall3CC[cc].Text = hall3[hc].Description;
                        hc++;
                        cc++;
                    }
                }
                else
                {
                    MessageBox.Show("无法连接到服务器！");
                }
                //hall4
                CErrorCode[] hall4 = Program.mng.LoadErrorCodeDescp(14);
                if (hall4 != null)
                {
                    int hc = 0;
                    int cc = 0;
                    while (hc < hall4.Length && cc < hall4CC.Count)
                    {
                        hall4CC[cc].Name = "l" + hall4[hc].StartBit;
                        hall4CC[cc].Text = hall4[hc].Description;
                        hc++;
                        cc++;
                    }
                }
                else
                {
                    MessageBox.Show("无法连接到服务器！");
                }
                //ETV1
                CErrorCode[] etv1 = Program.mng.LoadErrorCodeDescp(1);
                if (etv1 != null)
                {
                    int hc = 0;
                    int cc = 0;
                    while (hc < etv1.Length && cc < Etv1CC.Count)
                    {
                        Etv1CC[cc].Name = "l" + etv1[hc].StartBit;
                        Etv1CC[cc].Text = etv1[hc].Description;
                        hc++;
                        cc++;
                    }
                }
                else
                {
                    MessageBox.Show("无法连接到服务器！");
                }
                //ETV2
                CErrorCode[] etv2 = Program.mng.LoadErrorCodeDescp(2);
                if (etv2 != null)
                {
                    int hc = 0;
                    int cc = 0;
                    while (hc < etv2.Length && cc < Etv2CC.Count)
                    {
                        Etv2CC[cc].Name = "l" + etv2[hc].StartBit;
                        Etv2CC[cc].Text = etv2[hc].Description;
                        hc++;
                        cc++;
                    }
                }
                else
                {
                    MessageBox.Show("无法连接到服务器！");
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("读取报警位信息异常："+ex.ToString());
            }
        }

        private void updateStatusInfo()
        {
            try
            {                
                if (tabControl1.SelectedTab == tabPageH1)    //HALL1
                {
                    CErrorCode[] errCode = Program.mng.GetErrorCodesOfEqpID(11);
                    if (errCode != null)
                    {
                        foreach (Control cc in hall1CC)
                        {
                            CErrorCode err = Array.Find(errCode, (a => a.StartBit == Convert.ToInt16(cc.Name.Substring(1))));
                            if (err != null)
                            {
                                switch (err.Color)
                                {
                                    case 4:
                                        cc.BackColor = Color.Cyan;
                                        break;
                                    case 1:
                                        cc.BackColor = Color.Red;
                                        break;
                                    case 2:
                                        cc.BackColor = Color.Yellow;
                                        break;
                                    case 3:
                                        cc.BackColor = Color.Lime;
                                        break;
                                }
                            }
                            else
                            {
                                cc.BackColor = Color.SteelBlue;
                            }
                        }
                    }
             
                    CErrorCode[] errors = Program.mng.GetErrorCodesOfEqpID(12);
                    if (errors != null) 
                    {
                        foreach (Control cc in hall2CC) 
                        {
                            CErrorCode err = Array.Find(errors,(e=>e.StartBit==Convert.ToInt16(cc.Name.Substring(1))));
                            if (err != null) 
                            {
                                switch (err.Color) 
                                {
                                    case 4:
                                        cc.BackColor = Color.Cyan;
                                        break;
                                    case 3:
                                        cc.BackColor = Color.Lime;
                                        break;
                                    case 2:
                                        cc.BackColor = Color.Yellow;
                                        break;
                                    case 1:
                                        cc.BackColor = Color.Red;
                                        break;
                                }
                            } 
                            else
                            {
                                cc.BackColor = Color.SteelBlue;
                            }

                        }
                    }
                }
                else if (tabControl1.SelectedTab == tabPageH3) 
                {
                    CErrorCode[] errors3 = Program.mng.GetErrorCodesOfEqpID(13);
                    if (errors3 != null)
                    {
                        foreach (Control cc in hall3CC)
                        {
                            CErrorCode err = Array.Find(errors3, (e => e.StartBit == Convert.ToInt16(cc.Name.Substring(1))));
                            if (err != null)
                            {
                                switch (err.Color)
                                {
                                    case 4:
                                        cc.BackColor = Color.Cyan;
                                        break;
                                    case 3:
                                        cc.BackColor = Color.Lime;
                                        break;
                                    case 2:
                                        cc.BackColor = Color.Yellow;
                                        break;
                                    case 1:
                                        cc.BackColor = Color.Red;
                                        break;
                                }
                            }
                            else
                            {
                                cc.BackColor = Color.SteelBlue;
                            }

                        }
                    }
               
                    CErrorCode[] errors4 = Program.mng.GetErrorCodesOfEqpID(14);
                    if (errors4 != null)
                    {
                        foreach (Control cc in hall4CC)
                        {
                            CErrorCode err = Array.Find(errors4, (e => e.StartBit == Convert.ToInt16(cc.Name.Substring(1))));
                            if (err != null)
                            {
                                switch (err.Color)
                                {
                                    case 4:
                                        cc.BackColor = Color.Cyan;
                                        break;
                                    case 3:
                                        cc.BackColor = Color.Lime;
                                        break;
                                    case 2:
                                        cc.BackColor = Color.Yellow;
                                        break;
                                    case 1:
                                        cc.BackColor = Color.Red;
                                        break;
                                }
                            }
                            else
                            {
                                cc.BackColor = Color.SteelBlue;
                            }

                        }
                    }
                }
                else if (tabControl1.SelectedTab == tabPageEtv1) 
                {
                    CErrorCode[] codes = Program.mng.GetErrorCodesOfEqpID(1);
                    if (codes != null) 
                    {
                        foreach (Control cc in Etv1CC) 
                        {
                            CErrorCode er = Array.Find(codes,(e=>e.StartBit==Convert.ToInt16(cc.Name.Substring(1))));
                            if (er != null) 
                            {
                                switch (er.Color)
                                {
                                    case 4:
                                        cc.BackColor = Color.Cyan;
                                        break;
                                    case 3:
                                        cc.BackColor = Color.Lime;
                                        break;
                                    case 2:
                                        cc.BackColor = Color.Yellow;
                                        break;
                                    case 1:
                                        cc.BackColor = Color.Red;
                                        break;
                                }
                            } 
                            else 
                            {
                                cc.BackColor = Color.SteelBlue;
                            }
                        }
                    }
                }
                else if (tabControl1.SelectedTab == tabPageEtv2) 
                {
                    CErrorCode[] codes = Program.mng.GetErrorCodesOfEqpID(2);
                    if (codes != null)
                    {
                        foreach (Control cc in Etv2CC)
                        {
                            CErrorCode er = Array.Find(codes, (e => e.StartBit == Convert.ToInt16(cc.Name.Substring(1))));
                            if (er != null)
                            {
                                switch (er.Color)
                                {
                                    case 4:
                                        cc.BackColor = Color.Cyan;
                                        break;
                                    case 3:
                                        cc.BackColor = Color.Lime;
                                        break;
                                    case 2:
                                        cc.BackColor = Color.Yellow;
                                        break;
                                    case 1:
                                        cc.BackColor = Color.Red;
                                        break;
                                }
                            }
                            else
                            {
                                cc.BackColor = Color.SteelBlue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("更新报警信息异常："+ex.ToString());
            }
        }

        private void InitAllStatValue() 
        {
            try 
            {
                CErrorCode[] errCode = Program.mng.GetErrorCodesOfEqpID(11);
                if (errCode != null)
                {
                    foreach (Control cc in hall1CC)
                    {
                        CErrorCode err = Array.Find(errCode, (a => a.StartBit == Convert.ToInt16(cc.Name.Substring(1))));
                        if (err != null)
                        {
                            switch (err.Color)
                            {
                                case 4:
                                    cc.BackColor = Color.Cyan;
                                    break;
                                case 1:
                                    cc.BackColor = Color.Red;
                                    break;
                                case 2:
                                    cc.BackColor = Color.Yellow;
                                    break;
                                case 3:
                                    cc.BackColor = Color.Lime;
                                    break;
                            }
                        }
                        else
                        {
                            cc.BackColor = Color.SteelBlue;
                        }
                    }
                }

                CErrorCode[] errors = Program.mng.GetErrorCodesOfEqpID(12);
                if (errors != null)
                {
                    foreach (Control cc in hall2CC)
                    {
                        CErrorCode err = Array.Find(errors, (e => e.StartBit == Convert.ToInt16(cc.Name.Substring(1))));
                        if (err != null)
                        {
                            switch (err.Color)
                            {
                                case 4:
                                    cc.BackColor = Color.Cyan;
                                    break;
                                case 3:
                                    cc.BackColor = Color.Lime;
                                    break;
                                case 2:
                                    cc.BackColor = Color.Yellow;
                                    break;
                                case 1:
                                    cc.BackColor = Color.Red;
                                    break;
                            }
                        }
                        else
                        {
                            cc.BackColor = Color.SteelBlue;
                        }
                    }
                }

                CErrorCode[] errors3 = Program.mng.GetErrorCodesOfEqpID(13);
                if (errors3 != null)
                {
                    foreach (Control cc in hall3CC)
                    {
                        CErrorCode err = Array.Find(errors3, (e => e.StartBit == Convert.ToInt16(cc.Name.Substring(1))));
                        if (err != null)
                        {
                            switch (err.Color)
                            {
                                case 4:
                                    cc.BackColor = Color.Cyan;
                                    break;
                                case 3:
                                    cc.BackColor = Color.Lime;
                                    break;
                                case 2:
                                    cc.BackColor = Color.Yellow;
                                    break;
                                case 1:
                                    cc.BackColor = Color.Red;
                                    break;
                            }
                        }
                        else
                        {
                            cc.BackColor = Color.SteelBlue;
                        }
                    }
                }

                CErrorCode[] errors4 = Program.mng.GetErrorCodesOfEqpID(14);
                if (errors4 != null)
                {
                    foreach (Control cc in hall4CC)
                    {
                        CErrorCode err = Array.Find(errors4, (e => e.StartBit == Convert.ToInt16(cc.Name.Substring(1))));
                        if (err != null)
                        {
                            switch (err.Color)
                            {
                                case 4:
                                    cc.BackColor = Color.Cyan;
                                    break;
                                case 3:
                                    cc.BackColor = Color.Lime;
                                    break;
                                case 2:
                                    cc.BackColor = Color.Yellow;
                                    break;
                                case 1:
                                    cc.BackColor = Color.Red;
                                    break;
                            }
                        }
                        else
                        {
                            cc.BackColor = Color.SteelBlue;
                        }
                    }
                }

                CErrorCode[] codes = Program.mng.GetErrorCodesOfEqpID(1);
                if (codes != null)
                {
                    foreach (Control cc in Etv1CC)
                    {
                        CErrorCode er = Array.Find(codes, (e => e.StartBit == Convert.ToInt16(cc.Name.Substring(1))));
                        if (er != null)
                        {
                            switch (er.Color)
                            {
                                case 4:
                                    cc.BackColor = Color.Cyan;
                                    break;
                                case 3:
                                    cc.BackColor = Color.Lime;
                                    break;
                                case 2:
                                    cc.BackColor = Color.Yellow;
                                    break;
                                case 1:
                                    cc.BackColor = Color.Red;
                                    break;
                            }
                        }
                        else
                        {
                            cc.BackColor = Color.SteelBlue;
                        }
                    }
                }

                CErrorCode[] codes2 = Program.mng.GetErrorCodesOfEqpID(2);
                if (codes2 != null)
                {
                    foreach (Control cc in Etv2CC)
                    {
                        CErrorCode er = Array.Find(codes2, (e => e.StartBit == Convert.ToInt16(cc.Name.Substring(1))));
                        if (er != null)
                        {
                            switch (er.Color)
                            {
                                case 4:
                                    cc.BackColor = Color.Cyan;
                                    break;
                                case 3:
                                    cc.BackColor = Color.Lime;
                                    break;
                                case 2:
                                    cc.BackColor = Color.Yellow;
                                    break;
                                case 1:
                                    cc.BackColor = Color.Red;
                                    break;
                            }
                        }
                        else
                        {
                            cc.BackColor = Color.SteelBlue;
                        }
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
