using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IEGConsole
{
    public partial class FrmReportView : Form
    {
        public FrmReportView()
        {
            InitializeComponent();
        }

        public FrmReportView(SysLog slog) :this()
        {
            try
            {
                this.Text = "操作日志报表";
                crystalReportViewer1.ReportSource = slog;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public FrmReportView(ErrLog elog) : this() 
        {
            try
            {
                this.Text = "故障日志报表";
                crystalReportViewer1.ReportSource = elog;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public FrmReportView(FixIccdPayFee flog) : this() 
        {
            try
            {
                this.Text = "定期/固定卡缴费记录报表";
                crystalReportViewer1.ReportSource = flog;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public FrmReportView(TempIccdPayFee tlog) : this() 
        {
            try
            {
                this.Text = "临时卡缴费记录报表";
                crystalReportViewer1.ReportSource = tlog;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
