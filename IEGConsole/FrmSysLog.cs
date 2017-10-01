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
    public partial class FrmSysLog : Form
    {
        public FrmSysLog()
        {
            InitializeComponent();
        }

        private DataTable dtSysLog;
        private DataTable dtErrLog;

        private void FrmSysLog_Load(object sender, EventArgs e)
        {
            dateTimePickerSt.Value = DateTime.Now.AddDays(-1);
            dateTimePickerEnd.Value = DateTime.Now;
            dateTimePickerEst.Value = DateTime.Now.AddDays(-1);
            dateTimePickerEend.Value = DateTime.Now;
            comboBoxSLog.SelectedIndex = 3;
            comboBoxErr.SelectedIndex = 1;
        }

        private void btnLogFind_Click(object sender, EventArgs e)
        {
            try 
            {
                int rit = Program.mng.SelectSysLogs(dateTimePickerSt.Value, dateTimePickerEnd.Value,out dtSysLog);
                if (rit == 100)
                {
                    DataTable table = new DataTable();
                    DataColumn colunm;
                    DataRow row;

                    colunm = new DataColumn();
                    colunm.DataType = System.Type.GetType("System.String");
                    colunm.ColumnName = "cards";
                    table.Columns.Add(colunm);

                    colunm = new DataColumn();
                    colunm.DataType = System.Type.GetType("System.String");
                    colunm.ColumnName = "ldesc";
                    table.Columns.Add(colunm);

                    colunm = new DataColumn();
                    colunm.DataType = System.Type.GetType("System.DateTime");
                    colunm.ColumnName = "ddtime";
                    table.Columns.Add(colunm);

                    //卡号
                    if (comboBoxSLog.SelectedIndex == 0) 
                    {
                        if (txtCard.Text.Trim() == "") 
                        {
                            MessageBox.Show("请输入查询的卡号！");
                            return;
                        }
                        foreach (DataRow rw in dtSysLog.Rows) 
                        {
                            if (string.Compare(rw[0].ToString(), txtCard.Text.Trim()) == 0)
                            {
                                row = table.NewRow();
                                row["cards"] = rw["cards"];
                                row["ldesc"] = rw["ldesc"];
                                row["ddtime"] = rw["ddtime"];
                                table.Rows.Add(row);
                            }
                        }
                        dataGridView1.DataSource = table;
                        txtTotal.Text = table.Rows.Count.ToString();
                    }
                    else if (comboBoxSLog.SelectedIndex == 1)  //存车
                    {
                        foreach (DataRow rw in dtSysLog.Rows) 
                        {
                            string ss = rw[1].ToString();
                            if (ss.Contains("存车：")) 
                            {
                                row = table.NewRow();
                                row["cards"] = rw["cards"];
                                row["ldesc"] = rw["ldesc"];
                                row["ddtime"] = rw["ddtime"];
                                table.Rows.Add(row);
                            }
                            dataGridView1.DataSource = table;
                            txtTotal.Text = table.Rows.Count.ToString();
                        }
                    }
                    else if (comboBoxSLog.SelectedIndex == 2) 
                    {
                        foreach (DataRow rw in dtSysLog.Rows)
                        {
                            string ss = rw[1].ToString();
                            if (ss.Contains("取车："))
                            {
                                row = table.NewRow();
                                row["cards"] = rw["cards"];
                                row["ldesc"] = rw["ldesc"];
                                row["ddtime"] = rw["ddtime"];
                                table.Rows.Add(row);
                            }
                            dataGridView1.DataSource = table;
                            txtTotal.Text = table.Rows.Count.ToString();
                        }
                    }
                    else if (comboBoxSLog.SelectedIndex == 3)
                    {
                        if (txtCard.Text.Trim() == "")
                        {
                            MessageBox.Show("请输入查询的内容！");
                            return;
                        }
                        foreach (DataRow rw in dtSysLog.Rows)
                        {
                            if (rw[1].ToString().Contains(txtCard.Text.Trim()))
                            {
                                row = table.NewRow();
                                row["cards"] = rw["cards"];
                                row["ldesc"] = rw["ldesc"];
                                row["ddtime"] = rw["ddtime"];
                                table.Rows.Add(row);
                            }
                        }
                        dataGridView1.DataSource = table;
                        txtTotal.Text = table.Rows.Count.ToString();
                    }
                    else if (comboBoxSLog.SelectedIndex == 4)
                    {
                        foreach (DataRow rw in dtSysLog.Rows)
                        {
                            row = table.NewRow();
                            row["cards"] = rw["cards"];
                            row["ldesc"] = rw["ldesc"];
                            row["ddtime"] = rw["ddtime"];
                            table.Rows.Add(row);

                            dataGridView1.DataSource = table;
                            txtTotal.Text = table.Rows.Count.ToString();
                        }
                    }
                }
                else 
                {
                    MessageBox.Show("出现异常，无法完成操作！");
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnErrFind_Click(object sender, EventArgs e)
        {
            try
            {
                int rit = Program.mng.SelectErrLogs(dateTimePickerEst.Value,dateTimePickerEend.Value,out dtErrLog);
                if (rit == 100) 
                {
                    DataTable table = new DataTable();
                    DataColumn colunm;
                    DataRow row;

                    colunm = new DataColumn();
                    colunm.DataType = System.Type.GetType("System.String");
                    colunm.ColumnName = "code";
                    table.Columns.Add(colunm);

                    colunm = new DataColumn();
                    colunm.DataType = System.Type.GetType("System.String");
                    colunm.ColumnName = "ldesc";
                    table.Columns.Add(colunm);

                    colunm = new DataColumn();
                    colunm.DataType = System.Type.GetType("System.DateTime");
                    colunm.ColumnName = "ddtime";
                    table.Columns.Add(colunm);

                    if (comboBoxErr.SelectedIndex == 0)
                    {
                        if (txtErr.Text.Trim() == "")
                        {
                            MessageBox.Show("请输入查询内容！");
                            return;
                        }
                        foreach (DataRow rw in dtErrLog.Rows)
                        {
                            if (rw[0].ToString().Contains(txtErr.Text.Trim()))
                            {
                                row = table.NewRow();
                                row["code"] = rw["code"];
                                row["ldesc"] = rw["ldesc"];
                                row["ddtime"] = rw["ddtime"];
                                table.Rows.Add(row);
                            }
                        }
                        dataGridView2.DataSource = table;                       
                    } 
                    else if (comboBoxErr.SelectedIndex == 1) 
                    {
                        foreach (DataRow rw in dtErrLog.Rows)
                        {
                            row = table.NewRow();
                            row["code"] = rw["code"];
                            row["ldesc"] = rw["ldesc"];
                            row["ddtime"] = rw["ddtime"];
                            table.Rows.Add(row);
                        }
                        dataGridView2.DataSource = table;
                    }
                }
                else
                {
                    MessageBox.Show("出现异常，无法完成操作！");
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("出现异常，无法完成操作"+ex.ToString());
            }
        }

        private void btnLogReport_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource != null) 
            {
                SysLog slog = new SysLog();
                slog.SetDataSource(dataGridView1.DataSource);
                new FrmReportView(slog).ShowDialog();
            }
        }

        private void btnErrReport_Click(object sender, EventArgs e)
        {
            if (dataGridView2.DataSource != null) 
            {
                ErrLog elog = new ErrLog();
                elog.SetDataSource(dataGridView2.DataSource);
                new FrmReportView(elog).ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DateTime.Compare(dateTimePickerEst.Value, dateTimePickerEend.Value) > 0)
            {
                return;
            }
            DialogResult dr = MessageBox.Show("是否要永久删除指定时间段内的故障日志？", "提示", MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
            if (dr == DialogResult.OK) 
            { 
                Program.mng.DeleteErrorLogs(dateTimePickerEst.Value, dateTimePickerEend.Value);
                comboBoxErr.SelectedIndex = 1;
                btnErrFind_Click(sender, e);
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null) 
            {
                if (e.ColumnIndex == 0) 
                {
                    switch (e.Value.ToString()) 
                    {
                        case "1":
                            e.Value = "1#ETV";
                            break;
                        case "2":
                            e.Value = "2#ETV";
                            break;
                        case "11":
                            e.Value = "1#车厅";
                            break;
                        case "12":
                            e.Value = "2#车厅";
                            break;
                        case "13":
                            e.Value = "3#车厅";
                            break;
                        case "14":
                            e.Value = "4#车厅";
                            break;
                        default:
                            break;
                    }
                }
            }
        }

   
    }
}
