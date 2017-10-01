namespace IEGConsole
{
    partial class FrmManualIn
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnIn = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label15 = new System.Windows.Forms.Label();
            this.txtInCSize = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtInDist = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtInLct = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtInIccd = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnIn);
            this.groupBox5.Controls.Add(this.dateTimePicker1);
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.txtInCSize);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.txtInDist);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.txtInLct);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.txtInIccd);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Location = new System.Drawing.Point(12, 11);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(312, 262);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            // 
            // btnIn
            // 
            this.btnIn.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnIn.Location = new System.Drawing.Point(116, 204);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(87, 39);
            this.btnIn.TabIndex = 14;
            this.btnIn.Text = "手动入库";
            this.btnIn.UseVisualStyleBackColor = true;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(105, 160);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(145, 21);
            this.dateTimePicker1.TabIndex = 13;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(34, 164);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 12);
            this.label15.TabIndex = 12;
            this.label15.Text = "入库时间：";
            // 
            // txtInCSize
            // 
            this.txtInCSize.Location = new System.Drawing.Point(105, 88);
            this.txtInCSize.MaxLength = 3;
            this.txtInCSize.Name = "txtInCSize";
            this.txtInCSize.Size = new System.Drawing.Size(145, 21);
            this.txtInCSize.TabIndex = 11;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(34, 94);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 10;
            this.label13.Text = "车辆外形：";
            // 
            // txtInDist
            // 
            this.txtInDist.Location = new System.Drawing.Point(105, 124);
            this.txtInDist.Name = "txtInDist";
            this.txtInDist.Size = new System.Drawing.Size(145, 21);
            this.txtInDist.TabIndex = 9;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(34, 131);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 8;
            this.label14.Text = "车辆轴距：";
            // 
            // txtInLct
            // 
            this.txtInLct.Location = new System.Drawing.Point(105, 20);
            this.txtInLct.MaxLength = 4;
            this.txtInLct.Name = "txtInLct";
            this.txtInLct.Size = new System.Drawing.Size(145, 21);
            this.txtInLct.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(34, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 6;
            this.label11.Text = "入库车位：";
            // 
            // txtInIccd
            // 
            this.txtInIccd.Location = new System.Drawing.Point(105, 54);
            this.txtInIccd.MaxLength = 4;
            this.txtInIccd.Name = "txtInIccd";
            this.txtInIccd.Size = new System.Drawing.Size(145, 21);
            this.txtInIccd.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(34, 61);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 4;
            this.label12.Text = "用户卡号：";
            // 
            // FrmManualIn
            // 
            this.AcceptButton = this.btnIn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 286);
            this.Controls.Add(this.groupBox5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmManualIn";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "手动数据入库";
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtInCSize;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtInDist;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtInLct;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtInIccd;
        private System.Windows.Forms.Label label12;
    }
}