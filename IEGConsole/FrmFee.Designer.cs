namespace IEGConsole
{
    partial class FrmFee
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdYear = new System.Windows.Forms.RadioButton();
            this.rdMonth = new System.Windows.Forms.RadioButton();
            this.rdSeason = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtday = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFixFee = new System.Windows.Forms.TextBox();
            this.btnsv = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdNoBusy = new System.Windows.Forms.RadioButton();
            this.rdIsBusy = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbcg = new System.Windows.Forms.RadioButton();
            this.label14 = new System.Windows.Forms.Label();
            this.rblmt = new System.Windows.Forms.RadioButton();
            this.label26 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txttm = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtfee = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtday);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtFixFee);
            this.groupBox1.Controls.Add(this.rdYear);
            this.groupBox1.Controls.Add(this.rdMonth);
            this.groupBox1.Controls.Add(this.rdSeason);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(18, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 155);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "定期/固定卡";
            // 
            // rdYear
            // 
            this.rdYear.AutoSize = true;
            this.rdYear.Enabled = false;
            this.rdYear.Location = new System.Drawing.Point(161, 35);
            this.rdYear.Name = "rdYear";
            this.rdYear.Size = new System.Drawing.Size(35, 16);
            this.rdYear.TabIndex = 29;
            this.rdYear.TabStop = true;
            this.rdYear.Text = "年";
            this.rdYear.UseVisualStyleBackColor = true;
            this.rdYear.CheckedChanged += new System.EventHandler(this.rdMonth_CheckedChanged);
            // 
            // rdMonth
            // 
            this.rdMonth.AutoSize = true;
            this.rdMonth.Enabled = false;
            this.rdMonth.Location = new System.Drawing.Point(86, 35);
            this.rdMonth.Name = "rdMonth";
            this.rdMonth.Size = new System.Drawing.Size(35, 16);
            this.rdMonth.TabIndex = 29;
            this.rdMonth.TabStop = true;
            this.rdMonth.Text = "月";
            this.rdMonth.UseVisualStyleBackColor = true;
            this.rdMonth.CheckedChanged += new System.EventHandler(this.rdMonth_CheckedChanged);
            // 
            // rdSeason
            // 
            this.rdSeason.AutoSize = true;
            this.rdSeason.Enabled = false;
            this.rdSeason.Location = new System.Drawing.Point(124, 35);
            this.rdSeason.Name = "rdSeason";
            this.rdSeason.Size = new System.Drawing.Size(35, 16);
            this.rdSeason.TabIndex = 29;
            this.rdSeason.TabStop = true;
            this.rdSeason.Text = "季";
            this.rdSeason.UseVisualStyleBackColor = true;
            this.rdSeason.CheckedChanged += new System.EventHandler(this.rdMonth_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(159, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "(天)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(159, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "(元)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "计算时间";
            // 
            // txtday
            // 
            this.txtday.Enabled = false;
            this.txtday.Location = new System.Drawing.Point(85, 65);
            this.txtday.Name = "txtday";
            this.txtday.Size = new System.Drawing.Size(71, 21);
            this.txtday.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "计费单位";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 21;
            this.label5.Text = "费    用";
            // 
            // txtFixFee
            // 
            this.txtFixFee.Location = new System.Drawing.Point(85, 97);
            this.txtFixFee.Name = "txtFixFee";
            this.txtFixFee.Size = new System.Drawing.Size(71, 21);
            this.txtFixFee.TabIndex = 22;
            // 
            // btnsv
            // 
            this.btnsv.Location = new System.Drawing.Point(84, 192);
            this.btnsv.Name = "btnsv";
            this.btnsv.Size = new System.Drawing.Size(76, 33);
            this.btnsv.TabIndex = 32;
            this.btnsv.Text = "保存";
            this.btnsv.UseVisualStyleBackColor = true;
            this.btnsv.Click += new System.EventHandler(this.btnsv_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Controls.Add(this.label26);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.txttm);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.txtfee);
            this.groupBox3.Location = new System.Drawing.Point(23, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(219, 155);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "临时卡";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdNoBusy);
            this.groupBox4.Controls.Add(this.rdIsBusy);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Enabled = false;
            this.groupBox4.Location = new System.Drawing.Point(10, 48);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 36);
            this.groupBox4.TabIndex = 35;
            this.groupBox4.TabStop = false;
            // 
            // rdNoBusy
            // 
            this.rdNoBusy.AutoSize = true;
            this.rdNoBusy.Location = new System.Drawing.Point(128, 16);
            this.rdNoBusy.Name = "rdNoBusy";
            this.rdNoBusy.Size = new System.Drawing.Size(35, 16);
            this.rdNoBusy.TabIndex = 32;
            this.rdNoBusy.TabStop = true;
            this.rdNoBusy.Text = "否";
            this.rdNoBusy.UseVisualStyleBackColor = true;
            // 
            // rdIsBusy
            // 
            this.rdIsBusy.AutoSize = true;
            this.rdIsBusy.Location = new System.Drawing.Point(75, 16);
            this.rdIsBusy.Name = "rdIsBusy";
            this.rdIsBusy.Size = new System.Drawing.Size(35, 16);
            this.rdIsBusy.TabIndex = 31;
            this.rdIsBusy.TabStop = true;
            this.rdIsBusy.Text = "是";
            this.rdIsBusy.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 30;
            this.label6.Text = "高峰时期";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(161, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "(小时)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbcg);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.rblmt);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(10, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 36);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            // 
            // rbcg
            // 
            this.rbcg.AutoSize = true;
            this.rbcg.Location = new System.Drawing.Point(128, 14);
            this.rbcg.Name = "rbcg";
            this.rbcg.Size = new System.Drawing.Size(47, 16);
            this.rbcg.TabIndex = 29;
            this.rbcg.TabStop = true;
            this.rbcg.Text = "计费";
            this.rbcg.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 21;
            this.label14.Text = "计费类型";
            // 
            // rblmt
            // 
            this.rblmt.AutoSize = true;
            this.rblmt.Location = new System.Drawing.Point(75, 14);
            this.rblmt.Name = "rblmt";
            this.rblmt.Size = new System.Drawing.Size(47, 16);
            this.rblmt.TabIndex = 29;
            this.rblmt.TabStop = true;
            this.rblmt.Text = "限额";
            this.rblmt.UseVisualStyleBackColor = true;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(156, 127);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(29, 12);
            this.label26.TabIndex = 21;
            this.label26.Text = "(元)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(26, 97);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 21;
            this.label15.Text = "计算时间";
            // 
            // txttm
            // 
            this.txttm.Enabled = false;
            this.txttm.Location = new System.Drawing.Point(85, 94);
            this.txttm.Name = "txttm";
            this.txttm.Size = new System.Drawing.Size(70, 21);
            this.txttm.TabIndex = 22;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(50, 127);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(29, 12);
            this.label17.TabIndex = 21;
            this.label17.Text = "费用";
            // 
            // txtfee
            // 
            this.txtfee.Location = new System.Drawing.Point(85, 124);
            this.txtfee.Name = "txtfee";
            this.txtfee.Size = new System.Drawing.Size(71, 21);
            this.txtfee.TabIndex = 22;
            // 
            // FrmFee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 242);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnsv);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmFee";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "计费信息";
            this.Load += new System.EventHandler(this.FrmFee_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdYear;
        private System.Windows.Forms.RadioButton rdMonth;
        private System.Windows.Forms.RadioButton rdSeason;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtday;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtFixFee;
        private System.Windows.Forms.Button btnsv;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdNoBusy;
        private System.Windows.Forms.RadioButton rdIsBusy;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rbcg;
        private System.Windows.Forms.RadioButton rblmt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txttm;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtfee;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}