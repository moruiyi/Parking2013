namespace IEGConsole
{
    partial class FrmMtc
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
            this.btnReset = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.rtTskStatus = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtIccd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUseHall = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxTskType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnEnable = new System.Windows.Forms.Button();
            this.btnDIS = new System.Windows.Forms.Button();
            this.txtDisLct = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnTran = new System.Windows.Forms.Button();
            this.txtToLct = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFrlct = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnOut = new System.Windows.Forms.Button();
            this.txtOutLct = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.txtFindLct = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFindIccd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
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
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnHFind = new System.Windows.Forms.Button();
            this.txtState = new System.Windows.Forms.TextBox();
            this.txtPositHall = new System.Windows.Forms.TextBox();
            this.txtUseIccd = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(372, 341);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnReset);
            this.tabPage1.Controls.Add(this.btnFinish);
            this.tabPage1.Controls.Add(this.rtTskStatus);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtIccd);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtUseHall);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.comboBoxTskType);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(364, 316);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "故障处理";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(240, 261);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 34);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "手动复位";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(119, 261);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 34);
            this.btnFinish.TabIndex = 9;
            this.btnFinish.Text = "手动完成";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // rtTskStatus
            // 
            this.rtTskStatus.BackColor = System.Drawing.SystemColors.Window;
            this.rtTskStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtTskStatus.Location = new System.Drawing.Point(100, 135);
            this.rtTskStatus.Name = "rtTskStatus";
            this.rtTskStatus.ReadOnly = true;
            this.rtTskStatus.Size = new System.Drawing.Size(243, 106);
            this.rtTskStatus.TabIndex = 8;
            this.rtTskStatus.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(29, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "作业状态：";
            // 
            // txtIccd
            // 
            this.txtIccd.Location = new System.Drawing.Point(100, 95);
            this.txtIccd.Name = "txtIccd";
            this.txtIccd.Size = new System.Drawing.Size(196, 21);
            this.txtIccd.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(29, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "用户卡号：";
            // 
            // txtUseHall
            // 
            this.txtUseHall.Location = new System.Drawing.Point(100, 57);
            this.txtUseHall.Name = "txtUseHall";
            this.txtUseHall.Size = new System.Drawing.Size(196, 21);
            this.txtUseHall.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(29, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "使用车厅：";
            // 
            // comboBoxTskType
            // 
            this.comboBoxTskType.DisplayMember = "Type";
            this.comboBoxTskType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTskType.FormattingEnabled = true;
            this.comboBoxTskType.Location = new System.Drawing.Point(100, 20);
            this.comboBoxTskType.Name = "comboBoxTskType";
            this.comboBoxTskType.Size = new System.Drawing.Size(196, 20);
            this.comboBoxTskType.TabIndex = 2;
            this.comboBoxTskType.ValueMember = "Type";
            this.comboBoxTskType.SelectedIndexChanged += new System.EventHandler(this.comboBoxTskType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(29, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "作业类型：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(364, 316);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "车位维护";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnEnable);
            this.groupBox4.Controls.Add(this.btnDIS);
            this.groupBox4.Controls.Add(this.txtDisLct);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Location = new System.Drawing.Point(4, 246);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(355, 65);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "禁用车位";
            // 
            // btnEnable
            // 
            this.btnEnable.Location = new System.Drawing.Point(276, 21);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(65, 27);
            this.btnEnable.TabIndex = 9;
            this.btnEnable.Text = "启 用";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnDIS_Click);
            // 
            // btnDIS
            // 
            this.btnDIS.Location = new System.Drawing.Point(195, 21);
            this.btnDIS.Name = "btnDIS";
            this.btnDIS.Size = new System.Drawing.Size(65, 27);
            this.btnDIS.TabIndex = 7;
            this.btnDIS.Text = "禁 用";
            this.btnDIS.UseVisualStyleBackColor = true;
            this.btnDIS.Click += new System.EventHandler(this.btnDIS_Click);
            // 
            // txtDisLct
            // 
            this.txtDisLct.Location = new System.Drawing.Point(79, 25);
            this.txtDisLct.Name = "txtDisLct";
            this.txtDisLct.Size = new System.Drawing.Size(100, 21);
            this.txtDisLct.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 31);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 6;
            this.label10.Text = "对应车位：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnTran);
            this.groupBox3.Controls.Add(this.txtToLct);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtFrlct);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(4, 154);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(355, 85);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "手动挪移";
            // 
            // btnTran
            // 
            this.btnTran.Location = new System.Drawing.Point(235, 33);
            this.btnTran.Name = "btnTran";
            this.btnTran.Size = new System.Drawing.Size(65, 28);
            this.btnTran.TabIndex = 8;
            this.btnTran.Text = "挪 移";
            this.btnTran.UseVisualStyleBackColor = true;
            this.btnTran.Click += new System.EventHandler(this.btnTran_Click);
            // 
            // txtToLct
            // 
            this.txtToLct.Location = new System.Drawing.Point(93, 52);
            this.txtToLct.Name = "txtToLct";
            this.txtToLct.Size = new System.Drawing.Size(100, 21);
            this.txtToLct.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 6;
            this.label9.Text = "目的车位：";
            // 
            // txtFrlct
            // 
            this.txtFrlct.Location = new System.Drawing.Point(93, 14);
            this.txtFrlct.Name = "txtFrlct";
            this.txtFrlct.Size = new System.Drawing.Size(100, 21);
            this.txtFrlct.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(34, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 4;
            this.label8.Text = "源车位：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnOut);
            this.groupBox2.Controls.Add(this.txtOutLct);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(2, 91);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(357, 57);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "手动出库";
            // 
            // btnOut
            // 
            this.btnOut.Location = new System.Drawing.Point(237, 16);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(65, 27);
            this.btnOut.TabIndex = 5;
            this.btnOut.Text = "出 库";
            this.btnOut.UseVisualStyleBackColor = true;
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // txtOutLct
            // 
            this.txtOutLct.Location = new System.Drawing.Point(95, 20);
            this.txtOutLct.Name = "txtOutLct";
            this.txtOutLct.Size = new System.Drawing.Size(100, 21);
            this.txtOutLct.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "对应车位：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnFind);
            this.groupBox1.Controls.Add(this.txtFindLct);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtFindIccd);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(2, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 78);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询车位";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(237, 26);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(65, 26);
            this.btnFind.TabIndex = 4;
            this.btnFind.Text = "查 询";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtFindLct
            // 
            this.txtFindLct.Location = new System.Drawing.Point(95, 45);
            this.txtFindLct.Name = "txtFindLct";
            this.txtFindLct.Size = new System.Drawing.Size(100, 21);
            this.txtFindLct.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "对应车位：";
            // 
            // txtFindIccd
            // 
            this.txtFindIccd.Location = new System.Drawing.Point(95, 17);
            this.txtFindIccd.MaxLength = 5;
            this.txtFindIccd.Name = "txtFindIccd";
            this.txtFindIccd.Size = new System.Drawing.Size(100, 21);
            this.txtFindIccd.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "用户卡号：";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox5);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(364, 316);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "手动入库";
            this.tabPage3.UseVisualStyleBackColor = true;
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
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(358, 310);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            // 
            // btnIn
            // 
            this.btnIn.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnIn.Location = new System.Drawing.Point(133, 241);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(93, 35);
            this.btnIn.TabIndex = 14;
            this.btnIn.Text = "手动入库";
            this.btnIn.UseVisualStyleBackColor = true;
            this.btnIn.Click += new System.EventHandler(this.button1_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(124, 194);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(165, 21);
            this.dateTimePicker1.TabIndex = 13;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(53, 198);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 12);
            this.label15.TabIndex = 12;
            this.label15.Text = "入库时间：";
            // 
            // txtInCSize
            // 
            this.txtInCSize.Location = new System.Drawing.Point(124, 110);
            this.txtInCSize.Name = "txtInCSize";
            this.txtInCSize.Size = new System.Drawing.Size(165, 21);
            this.txtInCSize.TabIndex = 11;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(53, 116);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 10;
            this.label13.Text = "车辆外形：";
            // 
            // txtInDist
            // 
            this.txtInDist.Location = new System.Drawing.Point(124, 152);
            this.txtInDist.Name = "txtInDist";
            this.txtInDist.Size = new System.Drawing.Size(165, 21);
            this.txtInDist.TabIndex = 9;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(53, 159);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 8;
            this.label14.Text = "车辆轴距：";
            // 
            // txtInLct
            // 
            this.txtInLct.Location = new System.Drawing.Point(124, 28);
            this.txtInLct.MaxLength = 4;
            this.txtInLct.Name = "txtInLct";
            this.txtInLct.Size = new System.Drawing.Size(165, 21);
            this.txtInLct.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(53, 34);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 6;
            this.label11.Text = "入库车位：";
            // 
            // txtInIccd
            // 
            this.txtInIccd.Location = new System.Drawing.Point(124, 68);
            this.txtInIccd.MaxLength = 5;
            this.txtInIccd.Name = "txtInIccd";
            this.txtInIccd.Size = new System.Drawing.Size(165, 21);
            this.txtInIccd.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(53, 75);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 4;
            this.label12.Text = "用户卡号：";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox6);
            this.tabPage4.Location = new System.Drawing.Point(4, 21);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(364, 316);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "车厅查询";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnHFind);
            this.groupBox6.Controls.Add(this.txtState);
            this.groupBox6.Controls.Add(this.txtPositHall);
            this.groupBox6.Controls.Add(this.txtUseIccd);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.label18);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(358, 310);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            // 
            // btnHFind
            // 
            this.btnHFind.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnHFind.Location = new System.Drawing.Point(162, 226);
            this.btnHFind.Name = "btnHFind";
            this.btnHFind.Size = new System.Drawing.Size(91, 33);
            this.btnHFind.TabIndex = 31;
            this.btnHFind.Text = "查  找";
            this.btnHFind.UseVisualStyleBackColor = true;
            this.btnHFind.Click += new System.EventHandler(this.btnHFind_Click);
            // 
            // txtState
            // 
            this.txtState.BackColor = System.Drawing.SystemColors.Window;
            this.txtState.Location = new System.Drawing.Point(126, 115);
            this.txtState.Multiline = true;
            this.txtState.Name = "txtState";
            this.txtState.ReadOnly = true;
            this.txtState.Size = new System.Drawing.Size(178, 87);
            this.txtState.TabIndex = 30;
            // 
            // txtPositHall
            // 
            this.txtPositHall.BackColor = System.Drawing.SystemColors.Window;
            this.txtPositHall.Location = new System.Drawing.Point(126, 76);
            this.txtPositHall.Name = "txtPositHall";
            this.txtPositHall.ReadOnly = true;
            this.txtPositHall.Size = new System.Drawing.Size(150, 21);
            this.txtPositHall.TabIndex = 28;
            // 
            // txtUseIccd
            // 
            this.txtUseIccd.BackColor = System.Drawing.SystemColors.Window;
            this.txtUseIccd.Location = new System.Drawing.Point(126, 36);
            this.txtUseIccd.MaxLength = 5;
            this.txtUseIccd.Name = "txtUseIccd";
            this.txtUseIccd.Size = new System.Drawing.Size(150, 21);
            this.txtUseIccd.TabIndex = 26;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(56, 129);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 12);
            this.label17.TabIndex = 29;
            this.label17.Text = "作业状态";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(56, 79);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 27;
            this.label16.Text = "所在车厅";
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(48, 35);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(72, 31);
            this.label18.TabIndex = 25;
            this.label18.Text = " 用户卡号  （临时卡）";
            // 
            // FrmMtc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 341);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMtc";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "系统维护";
            this.Load += new System.EventHandler(this.FrmMtc_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox txtUseHall;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtIccd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.RichTextBox rtTskStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox txtFindLct;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFindIccd;
        private System.Windows.Forms.Button btnDIS;
        private System.Windows.Forms.TextBox txtDisLct;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnTran;
        private System.Windows.Forms.TextBox txtToLct;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtFrlct;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnOut;
        private System.Windows.Forms.TextBox txtOutLct;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnEnable;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtInCSize;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtInDist;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtInLct;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtInIccd;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnHFind;
        private System.Windows.Forms.TextBox txtState;
        private System.Windows.Forms.TextBox txtPositHall;
        private System.Windows.Forms.TextBox txtUseIccd;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox comboBoxTskType;
    }
}