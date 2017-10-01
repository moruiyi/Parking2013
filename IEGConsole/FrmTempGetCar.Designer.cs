namespace IEGConsole
{
    partial class FrmTempGetCar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTempGetCar));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnTempGet = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnExt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(92, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户卡号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(93, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "出车车厅：";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(181, 45);
            this.textBox1.MaxLength = 5;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(119, 23);
            this.textBox1.TabIndex = 2;
            // 
            // btnRead
            // 
            this.btnRead.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRead.BackgroundImage")));
            this.btnRead.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRead.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRead.Location = new System.Drawing.Point(30, 176);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(90, 37);
            this.btnRead.TabIndex = 4;
            this.btnRead.Text = "读 卡";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnTempGet
            // 
            this.btnTempGet.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTempGet.BackgroundImage")));
            this.btnTempGet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTempGet.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTempGet.Location = new System.Drawing.Point(155, 176);
            this.btnTempGet.Name = "btnTempGet";
            this.btnTempGet.Size = new System.Drawing.Size(90, 37);
            this.btnTempGet.TabIndex = 5;
            this.btnTempGet.Text = "确认出车";
            this.btnTempGet.UseVisualStyleBackColor = true;
            this.btnTempGet.Click += new System.EventHandler(this.btnTempGet_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(181, 104);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(119, 22);
            this.comboBox1.TabIndex = 6;
            // 
            // btnExt
            // 
            this.btnExt.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExt.BackgroundImage")));
            this.btnExt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExt.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExt.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExt.Location = new System.Drawing.Point(279, 176);
            this.btnExt.Name = "btnExt";
            this.btnExt.Size = new System.Drawing.Size(90, 37);
            this.btnExt.TabIndex = 7;
            this.btnExt.Text = "退 出";
            this.btnExt.UseVisualStyleBackColor = true;
            this.btnExt.Click += new System.EventHandler(this.btnExt_Click);
            // 
            // FrmTempGetCar
            // 
            this.AcceptButton = this.btnTempGet;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelButton = this.btnExt;
            this.ClientSize = new System.Drawing.Size(401, 265);
            this.Controls.Add(this.btnExt);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnTempGet);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmTempGetCar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "取物操作";
            this.Load += new System.EventHandler(this.FrmTempGetCar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnTempGet;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnExt;
    }
}