namespace IEGConsole
{
    partial class FrmCustInfo
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
            this.radioButtonLoss = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonNormal = new System.Windows.Forms.RadioButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnsv = new System.Windows.Forms.Button();
            this.btndel = new System.Windows.Forms.Button();
            this.btncnl = new System.Windows.Forms.Button();
            this.txtads = new System.Windows.Forms.TextBox();
            this.txtMobile = new System.Windows.Forms.TextBox();
            this.txttel = new System.Windows.Forms.TextBox();
            this.txtlct = new System.Windows.Forms.TextBox();
            this.txtcode = new System.Windows.Forms.TextBox();
            this.txtpnmb = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBoxA = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBoxA.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBoxA);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.btnsv);
            this.groupBox1.Controls.Add(this.btndel);
            this.groupBox1.Controls.Add(this.btncnl);
            this.groupBox1.Controls.Add(this.txtads);
            this.groupBox1.Controls.Add(this.txtMobile);
            this.groupBox1.Controls.Add(this.txttel);
            this.groupBox1.Controls.Add(this.txtlct);
            this.groupBox1.Controls.Add(this.txtcode);
            this.groupBox1.Controls.Add(this.txtpnmb);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtname);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(8, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 209);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "顾客信息";
            // 
            // radioButtonLoss
            // 
            this.radioButtonLoss.AutoSize = true;
            this.radioButtonLoss.Location = new System.Drawing.Point(124, 16);
            this.radioButtonLoss.Name = "radioButtonLoss";
            this.radioButtonLoss.Size = new System.Drawing.Size(47, 16);
            this.radioButtonLoss.TabIndex = 30;
            this.radioButtonLoss.TabStop = true;
            this.radioButtonLoss.Text = "挂失";
            this.radioButtonLoss.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "卡状态";
            // 
            // radioButtonNormal
            // 
            this.radioButtonNormal.AutoSize = true;
            this.radioButtonNormal.Location = new System.Drawing.Point(71, 16);
            this.radioButtonNormal.Name = "radioButtonNormal";
            this.radioButtonNormal.Size = new System.Drawing.Size(47, 16);
            this.radioButtonNormal.TabIndex = 30;
            this.radioButtonNormal.TabStop = true;
            this.radioButtonNormal.Text = "正常";
            this.radioButtonNormal.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "init",
            "临时卡",
            "定期卡",
            "固定车位卡"});
            this.comboBox1.Location = new System.Drawing.Point(72, 142);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(125, 20);
            this.comboBox1.TabIndex = 33;
            // 
            // btnsv
            // 
            this.btnsv.Location = new System.Drawing.Point(221, 165);
            this.btnsv.Name = "btnsv";
            this.btnsv.Size = new System.Drawing.Size(51, 27);
            this.btnsv.TabIndex = 32;
            this.btnsv.Text = "保存";
            this.btnsv.UseVisualStyleBackColor = true;
            this.btnsv.Click += new System.EventHandler(this.btnsv_Click);
            // 
            // btndel
            // 
            this.btndel.Location = new System.Drawing.Point(299, 165);
            this.btndel.Name = "btndel";
            this.btndel.Size = new System.Drawing.Size(51, 27);
            this.btndel.TabIndex = 32;
            this.btndel.Text = "删除";
            this.btndel.UseVisualStyleBackColor = true;
            this.btndel.Click += new System.EventHandler(this.btndel_Click);
            // 
            // btncnl
            // 
            this.btncnl.Location = new System.Drawing.Point(374, 165);
            this.btncnl.Name = "btncnl";
            this.btncnl.Size = new System.Drawing.Size(51, 27);
            this.btncnl.TabIndex = 31;
            this.btncnl.Text = "取消";
            this.btncnl.UseVisualStyleBackColor = true;
            this.btncnl.Click += new System.EventHandler(this.btncnl_Click);
            // 
            // txtads
            // 
            this.txtads.Location = new System.Drawing.Point(72, 83);
            this.txtads.Name = "txtads";
            this.txtads.Size = new System.Drawing.Size(345, 21);
            this.txtads.TabIndex = 26;
            // 
            // txtMobile
            // 
            this.txtMobile.Location = new System.Drawing.Point(278, 55);
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Size = new System.Drawing.Size(139, 21);
            this.txtMobile.TabIndex = 28;
            // 
            // txttel
            // 
            this.txttel.Location = new System.Drawing.Point(278, 25);
            this.txttel.Name = "txttel";
            this.txttel.Size = new System.Drawing.Size(139, 21);
            this.txttel.TabIndex = 28;
            // 
            // txtlct
            // 
            this.txtlct.Location = new System.Drawing.Point(72, 171);
            this.txtlct.Name = "txtlct";
            this.txtlct.Size = new System.Drawing.Size(125, 21);
            this.txtlct.TabIndex = 27;
            // 
            // txtcode
            // 
            this.txtcode.Location = new System.Drawing.Point(72, 112);
            this.txtcode.Name = "txtcode";
            this.txtcode.Size = new System.Drawing.Size(125, 21);
            this.txtcode.TabIndex = 27;
            // 
            // txtpnmb
            // 
            this.txtpnmb.Location = new System.Drawing.Point(72, 54);
            this.txtpnmb.Name = "txtpnmb";
            this.txtpnmb.Size = new System.Drawing.Size(125, 21);
            this.txtpnmb.TabIndex = 27;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(37, 87);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 19;
            this.label8.Text = "住址";
            // 
            // txtname
            // 
            this.txtname.Location = new System.Drawing.Point(72, 25);
            this.txtname.Name = "txtname";
            this.txtname.Size = new System.Drawing.Size(125, 21);
            this.txtname.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(219, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "移动电话";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(219, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "住宅电话";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "卡类型";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 175);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 20;
            this.label9.Text = "分配车位";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "发卡编号";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "车牌号";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 24;
            this.label6.Text = "姓名";
            // 
            // groupBoxA
            // 
            this.groupBoxA.Controls.Add(this.radioButtonLoss);
            this.groupBoxA.Controls.Add(this.label3);
            this.groupBoxA.Controls.Add(this.radioButtonNormal);
            this.groupBoxA.Location = new System.Drawing.Point(217, 106);
            this.groupBoxA.Name = "groupBoxA";
            this.groupBoxA.Size = new System.Drawing.Size(200, 40);
            this.groupBoxA.TabIndex = 34;
            this.groupBoxA.TabStop = false;
            // 
            // FrmCustInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 265);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCustInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "顾客信息";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxA.ResumeLayout(false);
            this.groupBoxA.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonLoss;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButtonNormal;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnsv;
        private System.Windows.Forms.Button btndel;
        private System.Windows.Forms.Button btncnl;
        private System.Windows.Forms.TextBox txtads;
        private System.Windows.Forms.TextBox txtMobile;
        private System.Windows.Forms.TextBox txttel;
        private System.Windows.Forms.TextBox txtlct;
        private System.Windows.Forms.TextBox txtcode;
        private System.Windows.Forms.TextBox txtpnmb;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBoxA;
    }
}