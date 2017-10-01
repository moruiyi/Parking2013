namespace IEGConsole
{
    partial class FrmSysLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.card = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLogReport = new System.Windows.Forms.Button();
            this.btnLogFind = new System.Windows.Forms.Button();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCard = new System.Windows.Forms.TextBox();
            this.comboBoxSLog = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerSt = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ldesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ddtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnErrReport = new System.Windows.Forms.Button();
            this.btnErrFind = new System.Windows.Forms.Button();
            this.txtErr = new System.Windows.Forms.TextBox();
            this.comboBoxErr = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dateTimePickerEend = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEst = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(694, 523);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(686, 497);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "操作日志";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(6, 113);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(672, 377);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "列表";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.card,
            this.description,
            this.recordTime});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(666, 357);
            this.dataGridView1.TabIndex = 1;
            // 
            // card
            // 
            this.card.DataPropertyName = "cards";
            this.card.HeaderText = "用户卡号";
            this.card.Name = "card";
            this.card.Width = 80;
            // 
            // description
            // 
            this.description.DataPropertyName = "ldesc";
            this.description.HeaderText = "描述";
            this.description.Name = "description";
            this.description.Width = 480;
            // 
            // recordTime
            // 
            this.recordTime.DataPropertyName = "ddtime";
            this.recordTime.HeaderText = "记录时间";
            this.recordTime.Name = "recordTime";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLogReport);
            this.groupBox1.Controls.Add(this.btnLogFind);
            this.groupBox1.Controls.Add(this.txtTotal);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtCard);
            this.groupBox1.Controls.Add(this.comboBoxSLog);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dateTimePickerEnd);
            this.groupBox1.Controls.Add(this.dateTimePickerSt);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(6, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(674, 98);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作";
            // 
            // btnLogReport
            // 
            this.btnLogReport.Location = new System.Drawing.Point(578, 56);
            this.btnLogReport.Name = "btnLogReport";
            this.btnLogReport.Size = new System.Drawing.Size(75, 23);
            this.btnLogReport.TabIndex = 24;
            this.btnLogReport.Text = "报 表";
            this.btnLogReport.UseVisualStyleBackColor = true;
            this.btnLogReport.Click += new System.EventHandler(this.btnLogReport_Click);
            // 
            // btnLogFind
            // 
            this.btnLogFind.Location = new System.Drawing.Point(471, 55);
            this.btnLogFind.Name = "btnLogFind";
            this.btnLogFind.Size = new System.Drawing.Size(75, 23);
            this.btnLogFind.TabIndex = 23;
            this.btnLogFind.Text = "查 询";
            this.btnLogFind.UseVisualStyleBackColor = true;
            this.btnLogFind.Click += new System.EventHandler(this.btnLogFind_Click);
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(515, 20);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(109, 21);
            this.txtTotal.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(468, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "总计：";
            // 
            // txtCard
            // 
            this.txtCard.Location = new System.Drawing.Point(333, 57);
            this.txtCard.Name = "txtCard";
            this.txtCard.Size = new System.Drawing.Size(110, 21);
            this.txtCard.TabIndex = 20;
            // 
            // comboBoxSLog
            // 
            this.comboBoxSLog.FormattingEnabled = true;
            this.comboBoxSLog.Items.AddRange(new object[] {
            "卡号",
            "存车",
            "取车",
            "包含",
            "全部"});
            this.comboBoxSLog.Location = new System.Drawing.Point(333, 21);
            this.comboBoxSLog.Name = "comboBoxSLog";
            this.comboBoxSLog.Size = new System.Drawing.Size(110, 20);
            this.comboBoxSLog.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(262, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "查询信息：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(262, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "查询条件：";
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(87, 57);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(163, 21);
            this.dateTimePickerEnd.TabIndex = 16;
            // 
            // dateTimePickerSt
            // 
            this.dateTimePickerSt.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePickerSt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerSt.Location = new System.Drawing.Point(87, 20);
            this.dateTimePickerSt.Name = "dateTimePickerSt";
            this.dateTimePickerSt.Size = new System.Drawing.Size(163, 21);
            this.dateTimePickerSt.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "结束时间：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "起始时间：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(686, 497);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "故障日志";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.dataGridView2);
            this.groupBox3.Location = new System.Drawing.Point(7, 131);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(673, 360);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "列表";
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.code,
            this.ldesc,
            this.ddtime});
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 17);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(667, 340);
            this.dataGridView2.TabIndex = 1;
            this.dataGridView2.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView2_CellFormatting);
            // 
            // code
            // 
            this.code.DataPropertyName = "code";
            this.code.HeaderText = "设备";
            this.code.Name = "code";
            // 
            // ldesc
            // 
            this.ldesc.DataPropertyName = "ldesc";
            this.ldesc.HeaderText = "描述";
            this.ldesc.Name = "ldesc";
            this.ldesc.Width = 450;
            // 
            // ddtime
            // 
            this.ddtime.DataPropertyName = "ddtime";
            this.ddtime.HeaderText = "记录时间";
            this.ddtime.Name = "ddtime";
            this.ddtime.Width = 110;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.btnErrReport);
            this.groupBox4.Controls.Add(this.btnErrFind);
            this.groupBox4.Controls.Add(this.txtErr);
            this.groupBox4.Controls.Add(this.comboBoxErr);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.dateTimePickerEend);
            this.groupBox4.Controls.Add(this.dateTimePickerEst);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Location = new System.Drawing.Point(7, 8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(673, 117);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "操作";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(585, 74);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "清空记录";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnErrReport
            // 
            this.btnErrReport.Location = new System.Drawing.Point(490, 74);
            this.btnErrReport.Name = "btnErrReport";
            this.btnErrReport.Size = new System.Drawing.Size(75, 23);
            this.btnErrReport.TabIndex = 12;
            this.btnErrReport.Text = "报 表";
            this.btnErrReport.UseVisualStyleBackColor = true;
            this.btnErrReport.Click += new System.EventHandler(this.btnErrReport_Click);
            // 
            // btnErrFind
            // 
            this.btnErrFind.Location = new System.Drawing.Point(490, 27);
            this.btnErrFind.Name = "btnErrFind";
            this.btnErrFind.Size = new System.Drawing.Size(75, 23);
            this.btnErrFind.TabIndex = 11;
            this.btnErrFind.Text = "查 询";
            this.btnErrFind.UseVisualStyleBackColor = true;
            this.btnErrFind.Click += new System.EventHandler(this.btnErrFind_Click);
            // 
            // txtErr
            // 
            this.txtErr.Location = new System.Drawing.Point(336, 75);
            this.txtErr.Name = "txtErr";
            this.txtErr.Size = new System.Drawing.Size(127, 21);
            this.txtErr.TabIndex = 8;
            // 
            // comboBoxErr
            // 
            this.comboBoxErr.FormattingEnabled = true;
            this.comboBoxErr.Items.AddRange(new object[] {
            "设备",
            "全部"});
            this.comboBoxErr.Location = new System.Drawing.Point(336, 30);
            this.comboBoxErr.Name = "comboBoxErr";
            this.comboBoxErr.Size = new System.Drawing.Size(127, 20);
            this.comboBoxErr.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(265, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "查询内容：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(265, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "查询条件：";
            // 
            // dateTimePickerEend
            // 
            this.dateTimePickerEend.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePickerEend.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEend.Location = new System.Drawing.Point(87, 75);
            this.dateTimePickerEend.Name = "dateTimePickerEend";
            this.dateTimePickerEend.Size = new System.Drawing.Size(163, 21);
            this.dateTimePickerEend.TabIndex = 4;
            // 
            // dateTimePickerEst
            // 
            this.dateTimePickerEst.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePickerEst.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEst.Location = new System.Drawing.Point(87, 29);
            this.dateTimePickerEst.Name = "dateTimePickerEst";
            this.dateTimePickerEst.Size = new System.Drawing.Size(163, 21);
            this.dateTimePickerEst.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 79);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "结束时间：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "起始时间：";
            // 
            // FrmSysLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 523);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "FrmSysLog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "系统日志";
            this.Load += new System.EventHandler(this.FrmSysLog_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnErrReport;
        private System.Windows.Forms.Button btnErrFind;
        private System.Windows.Forms.TextBox txtErr;
        private System.Windows.Forms.ComboBox comboBoxErr;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dateTimePickerEend;
        private System.Windows.Forms.DateTimePicker dateTimePickerEst;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnLogReport;
        private System.Windows.Forms.Button btnLogFind;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCard;
        private System.Windows.Forms.ComboBox comboBoxSLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerSt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn card;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordTime;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn ldesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn ddtime;

    }
}