namespace IEGConsole
{
    partial class FrmCustomer
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
            this.uname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iccdType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iccdStat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lctAddrs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dueDTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cdtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.custAddrs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlatNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telephone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mobile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ICCard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCadd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCFind = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.txtFinddata = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textlct = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDispose = new System.Windows.Forms.Button();
            this.btnDisLoss = new System.Windows.Forms.Button();
            this.txttype = new System.Windows.Forms.TextBox();
            this.txtStat = new System.Windows.Forms.TextBox();
            this.txtmdt = new System.Windows.Forms.TextBox();
            this.txtddt = new System.Windows.Forms.TextBox();
            this.txtldt = new System.Windows.Forms.TextBox();
            this.txtcd = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnLoss = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnMake = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.txtPhysicCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCheck = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txtUseCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.tabControl1.Size = new System.Drawing.Size(527, 435);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(519, 408);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "车主管理";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(6, 112);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(483, 268);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "用户信息";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.uname,
            this.cardcode,
            this.iccdType,
            this.iccdStat,
            this.lctAddrs,
            this.dueDTime,
            this.cdtime,
            this.custAddrs,
            this.PlatNumber,
            this.telephone,
            this.mobile,
            this.id,
            this.ICCard});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(477, 248);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            // 
            // uname
            // 
            this.uname.DataPropertyName = "Code";
            this.uname.HeaderText = "车主姓名";
            this.uname.Name = "uname";
            // 
            // cardcode
            // 
            this.cardcode.DataPropertyName = "ICCardCode";
            this.cardcode.HeaderText = "用户卡号";
            this.cardcode.Name = "cardcode";
            // 
            // iccdType
            // 
            this.iccdType.DataPropertyName = "ICCardType";
            this.iccdType.HeaderText = "IC卡类型";
            this.iccdType.Name = "iccdType";
            // 
            // iccdStat
            // 
            this.iccdStat.DataPropertyName = "ICCardStat";
            this.iccdStat.HeaderText = "IC卡状态";
            this.iccdStat.Name = "iccdStat";
            // 
            // lctAddrs
            // 
            this.lctAddrs.DataPropertyName = "LctAddress";
            this.lctAddrs.HeaderText = "绑定车位";
            this.lctAddrs.Name = "lctAddrs";
            // 
            // dueDTime
            // 
            this.dueDTime.DataPropertyName = "ICCardDueDate";
            this.dueDTime.HeaderText = "建立日期";
            this.dueDTime.Name = "dueDTime";
            // 
            // cdtime
            // 
            this.cdtime.DataPropertyName = "IccdCdTime";
            this.cdtime.HeaderText = "制卡日期";
            this.cdtime.Name = "cdtime";
            // 
            // custAddrs
            // 
            this.custAddrs.DataPropertyName = "Address";
            this.custAddrs.HeaderText = "用户住址";
            this.custAddrs.Name = "custAddrs";
            // 
            // PlatNumber
            // 
            this.PlatNumber.DataPropertyName = "PlatNumber";
            this.PlatNumber.HeaderText = "车牌号";
            this.PlatNumber.Name = "PlatNumber";
            // 
            // telephone
            // 
            this.telephone.DataPropertyName = "Telphone";
            this.telephone.HeaderText = "固定电话";
            this.telephone.Name = "telephone";
            // 
            // mobile
            // 
            this.mobile.DataPropertyName = "Mobile";
            this.mobile.HeaderText = "移动电话";
            this.mobile.Name = "mobile";
            // 
            // id
            // 
            this.id.DataPropertyName = "ID";
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.Visible = false;
            // 
            // ICCard
            // 
            this.ICCard.DataPropertyName = "ICCard";
            this.ICCard.HeaderText = "ICCard";
            this.ICCard.Name = "ICCard";
            this.ICCard.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnCadd);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnCFind);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.txtFinddata);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(483, 100);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息查询";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "查询条件：";
            // 
            // btnCadd
            // 
            this.btnCadd.Location = new System.Drawing.Point(383, 65);
            this.btnCadd.Name = "btnCadd";
            this.btnCadd.Size = new System.Drawing.Size(75, 23);
            this.btnCadd.TabIndex = 5;
            this.btnCadd.Text = "添 加";
            this.btnCadd.UseVisualStyleBackColor = true;
            this.btnCadd.Click += new System.EventHandler(this.btnCadd_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "查询数据：";
            // 
            // btnCFind
            // 
            this.btnCFind.Location = new System.Drawing.Point(279, 65);
            this.btnCFind.Name = "btnCFind";
            this.btnCFind.Size = new System.Drawing.Size(75, 23);
            this.btnCFind.TabIndex = 4;
            this.btnCFind.Text = "查 询";
            this.btnCFind.UseVisualStyleBackColor = true;
            this.btnCFind.Click += new System.EventHandler(this.btnCFind_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "车主姓名",
            "用户卡号",
            "绑定车位",
            "用户住址",
            "全部"});
            this.comboBox1.Location = new System.Drawing.Point(87, 27);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(156, 20);
            this.comboBox1.TabIndex = 2;
            // 
            // txtFinddata
            // 
            this.txtFinddata.Location = new System.Drawing.Point(87, 64);
            this.txtFinddata.Name = "txtFinddata";
            this.txtFinddata.Size = new System.Drawing.Size(156, 21);
            this.txtFinddata.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(519, 410);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "IC卡管理";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textlct);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.btnDispose);
            this.groupBox4.Controls.Add(this.btnDisLoss);
            this.groupBox4.Controls.Add(this.txttype);
            this.groupBox4.Controls.Add(this.txtStat);
            this.groupBox4.Controls.Add(this.txtmdt);
            this.groupBox4.Controls.Add(this.txtddt);
            this.groupBox4.Controls.Add(this.txtldt);
            this.groupBox4.Controls.Add(this.txtcd);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.btnLoss);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Enabled = false;
            this.groupBox4.Location = new System.Drawing.Point(15, 138);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(465, 241);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "IC卡信息";
            // 
            // textlct
            // 
            this.textlct.BackColor = System.Drawing.SystemColors.Window;
            this.textlct.Location = new System.Drawing.Point(79, 141);
            this.textlct.Name = "textlct";
            this.textlct.ReadOnly = true;
            this.textlct.Size = new System.Drawing.Size(146, 21);
            this.textlct.TabIndex = 36;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 35;
            this.label6.Text = "绑定车位";
            // 
            // btnDispose
            // 
            this.btnDispose.Location = new System.Drawing.Point(303, 185);
            this.btnDispose.Name = "btnDispose";
            this.btnDispose.Size = new System.Drawing.Size(93, 34);
            this.btnDispose.TabIndex = 34;
            this.btnDispose.Text = "注 销";
            this.btnDispose.UseVisualStyleBackColor = true;
            this.btnDispose.Click += new System.EventHandler(this.btnLoss_Click);
            // 
            // btnDisLoss
            // 
            this.btnDisLoss.Location = new System.Drawing.Point(189, 185);
            this.btnDisLoss.Name = "btnDisLoss";
            this.btnDisLoss.Size = new System.Drawing.Size(93, 34);
            this.btnDisLoss.TabIndex = 33;
            this.btnDisLoss.Text = "取消挂失";
            this.btnDisLoss.UseVisualStyleBackColor = true;
            this.btnDisLoss.Click += new System.EventHandler(this.btnLoss_Click);
            // 
            // txttype
            // 
            this.txttype.BackColor = System.Drawing.SystemColors.Window;
            this.txttype.Location = new System.Drawing.Point(79, 69);
            this.txttype.Name = "txttype";
            this.txttype.ReadOnly = true;
            this.txttype.Size = new System.Drawing.Size(145, 21);
            this.txttype.TabIndex = 25;
            // 
            // txtStat
            // 
            this.txtStat.BackColor = System.Drawing.SystemColors.Window;
            this.txtStat.Location = new System.Drawing.Point(79, 106);
            this.txtStat.Name = "txtStat";
            this.txtStat.ReadOnly = true;
            this.txtStat.Size = new System.Drawing.Size(145, 21);
            this.txtStat.TabIndex = 25;
            // 
            // txtmdt
            // 
            this.txtmdt.BackColor = System.Drawing.SystemColors.Window;
            this.txtmdt.Location = new System.Drawing.Point(302, 31);
            this.txtmdt.Name = "txtmdt";
            this.txtmdt.ReadOnly = true;
            this.txtmdt.Size = new System.Drawing.Size(145, 21);
            this.txtmdt.TabIndex = 25;
            // 
            // txtddt
            // 
            this.txtddt.BackColor = System.Drawing.SystemColors.Window;
            this.txtddt.Location = new System.Drawing.Point(302, 107);
            this.txtddt.Name = "txtddt";
            this.txtddt.ReadOnly = true;
            this.txtddt.Size = new System.Drawing.Size(145, 21);
            this.txtddt.TabIndex = 25;
            // 
            // txtldt
            // 
            this.txtldt.BackColor = System.Drawing.SystemColors.Window;
            this.txtldt.Location = new System.Drawing.Point(302, 69);
            this.txtldt.Name = "txtldt";
            this.txtldt.ReadOnly = true;
            this.txtldt.Size = new System.Drawing.Size(145, 21);
            this.txtldt.TabIndex = 25;
            // 
            // txtcd
            // 
            this.txtcd.BackColor = System.Drawing.SystemColors.Window;
            this.txtcd.Location = new System.Drawing.Point(79, 31);
            this.txtcd.Name = "txtcd";
            this.txtcd.ReadOnly = true;
            this.txtcd.Size = new System.Drawing.Size(145, 21);
            this.txtcd.TabIndex = 25;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 24;
            this.label11.Text = "卡类型";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 110);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 12);
            this.label14.TabIndex = 24;
            this.label14.Text = "卡状态";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(242, 35);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 24;
            this.label12.Text = "制卡时间";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(242, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 24;
            this.label10.Text = "制卡时间";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(242, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 24;
            this.label8.Text = "注销时间";
            // 
            // btnLoss
            // 
            this.btnLoss.Location = new System.Drawing.Point(65, 185);
            this.btnLoss.Name = "btnLoss";
            this.btnLoss.Size = new System.Drawing.Size(93, 34);
            this.btnLoss.TabIndex = 33;
            this.btnLoss.Text = "挂 失";
            this.btnLoss.UseVisualStyleBackColor = true;
            this.btnLoss.Click += new System.EventHandler(this.btnLoss_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(242, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 24;
            this.label7.Text = "挂失时间";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 24;
            this.label4.Text = "用户卡号";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.btnMake);
            this.groupBox3.Controls.Add(this.btnRead);
            this.groupBox3.Controls.Add(this.txtPhysicCode);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.btnCheck);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.txtUseCode);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(15, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(465, 113);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "查询";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(26, 86);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 12);
            this.label9.TabIndex = 1;
            this.label9.Text = "(4位数)";
            // 
            // btnMake
            // 
            this.btnMake.Location = new System.Drawing.Point(378, 25);
            this.btnMake.Name = "btnMake";
            this.btnMake.Size = new System.Drawing.Size(57, 27);
            this.btnMake.TabIndex = 35;
            this.btnMake.Text = "制卡";
            this.btnMake.UseVisualStyleBackColor = true;
            this.btnMake.Click += new System.EventHandler(this.btnMake_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(300, 25);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(57, 27);
            this.btnRead.TabIndex = 36;
            this.btnRead.Text = "读卡";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // txtPhysicCode
            // 
            this.txtPhysicCode.BackColor = System.Drawing.SystemColors.Window;
            this.txtPhysicCode.Location = new System.Drawing.Point(83, 26);
            this.txtPhysicCode.Name = "txtPhysicCode";
            this.txtPhysicCode.ReadOnly = true;
            this.txtPhysicCode.Size = new System.Drawing.Size(180, 21);
            this.txtPhysicCode.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 33;
            this.label5.Text = "物理卡号";
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(300, 66);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(57, 27);
            this.btnCheck.TabIndex = 32;
            this.btnCheck.Text = "查询";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(378, 66);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(57, 27);
            this.button3.TabIndex = 31;
            this.button3.Text = "关闭";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtUseCode
            // 
            this.txtUseCode.Location = new System.Drawing.Point(83, 67);
            this.txtUseCode.Name = "txtUseCode";
            this.txtUseCode.Size = new System.Drawing.Size(180, 21);
            this.txtUseCode.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 24;
            this.label3.Text = "用户卡号";
            // 
            // FrmCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 435);
            this.Controls.Add(this.tabControl1);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.Name = "FrmCustomer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户管理";
            this.Load += new System.EventHandler(this.FrmCustomer_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCadd;
        private System.Windows.Forms.Button btnCFind;
        private System.Windows.Forms.TextBox txtFinddata;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textlct;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnDispose;
        private System.Windows.Forms.Button btnDisLoss;
        private System.Windows.Forms.TextBox txttype;
        private System.Windows.Forms.TextBox txtStat;
        private System.Windows.Forms.TextBox txtmdt;
        private System.Windows.Forms.TextBox txtddt;
        private System.Windows.Forms.TextBox txtldt;
        private System.Windows.Forms.TextBox txtcd;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnLoss;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnMake;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.TextBox txtPhysicCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtUseCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridViewTextBoxColumn uname;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn iccdType;
        private System.Windows.Forms.DataGridViewTextBoxColumn iccdStat;
        private System.Windows.Forms.DataGridViewTextBoxColumn lctAddrs;
        private System.Windows.Forms.DataGridViewTextBoxColumn dueDTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cdtime;
        private System.Windows.Forms.DataGridViewTextBoxColumn custAddrs;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlatNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn telephone;
        private System.Windows.Forms.DataGridViewTextBoxColumn mobile;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ICCard;
    }
}