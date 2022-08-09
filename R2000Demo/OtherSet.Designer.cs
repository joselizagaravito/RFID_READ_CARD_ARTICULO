namespace R2000Demo
{
    partial class OtherSet
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
            this.label2 = new System.Windows.Forms.Label();
            this.bt_BaudSet = new System.Windows.Forms.Button();
            this.cbB_Baud = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_softreset = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cB_DualSaveFlag = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbB_DualAndSing = new System.Windows.Forms.ComboBox();
            this.bt_GetDual = new System.Windows.Forms.Button();
            this.bt_SetDual = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tB_RSSI = new System.Windows.Forms.TextBox();
            this.bt_GetRSSI = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tB_LocalPort = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.bt_SetNetPara = new System.Windows.Forms.Button();
            this.tB_LocalGateWay = new System.Windows.Forms.TextBox();
            this.tB_LocalIP = new System.Windows.Forms.TextBox();
            this.tB_DestPort = new System.Windows.Forms.TextBox();
            this.tB_DestIP = new System.Windows.Forms.TextBox();
            this.cB_NetWork = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cB_epctidSaveFlag = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbB_EPCTIDTOG = new System.Windows.Forms.ComboBox();
            this.bt_GetEpcTidTogether = new System.Windows.Forms.Button();
            this.bt_SetEpcTidTogether = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.bt_FactorySet = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cB_pandianSaveflag = new System.Windows.Forms.CheckBox();
            this.Bt_Setpandian = new System.Windows.Forms.Button();
            this.bt_Getpandian = new System.Windows.Forms.Button();
            this.cB_pandian = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.bt_BaudSet);
            this.groupBox1.Controls.Add(this.cbB_Baud);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 53);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "波特率设置:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(103, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "label1";
            // 
            // bt_BaudSet
            // 
            this.bt_BaudSet.Location = new System.Drawing.Point(215, 18);
            this.bt_BaudSet.Name = "bt_BaudSet";
            this.bt_BaudSet.Size = new System.Drawing.Size(75, 23);
            this.bt_BaudSet.TabIndex = 1;
            this.bt_BaudSet.Text = "设置";
            this.bt_BaudSet.UseVisualStyleBackColor = true;
            this.bt_BaudSet.Click += new System.EventHandler(this.bt_BaudSet_Click);
            // 
            // cbB_Baud
            // 
            this.cbB_Baud.FormattingEnabled = true;
            this.cbB_Baud.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400",
            "460800",
            "921600"});
            this.cbB_Baud.Location = new System.Drawing.Point(6, 20);
            this.cbB_Baud.Name = "cbB_Baud";
            this.cbB_Baud.Size = new System.Drawing.Size(90, 20);
            this.cbB_Baud.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.bt_softreset);
            this.groupBox2.Location = new System.Drawing.Point(314, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(296, 53);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "软件复位:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // bt_softreset
            // 
            this.bt_softreset.Location = new System.Drawing.Point(215, 18);
            this.bt_softreset.Name = "bt_softreset";
            this.bt_softreset.Size = new System.Drawing.Size(75, 23);
            this.bt_softreset.TabIndex = 1;
            this.bt_softreset.Text = "软件复位";
            this.bt_softreset.UseVisualStyleBackColor = true;
            this.bt_softreset.Click += new System.EventHandler(this.bt_softreset_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cB_DualSaveFlag);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.cbB_DualAndSing);
            this.groupBox3.Controls.Add(this.bt_GetDual);
            this.groupBox3.Controls.Add(this.bt_SetDual);
            this.groupBox3.Location = new System.Drawing.Point(12, 71);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(296, 78);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Dual和Single模式设置:";
            // 
            // cB_DualSaveFlag
            // 
            this.cB_DualSaveFlag.AutoSize = true;
            this.cB_DualSaveFlag.Location = new System.Drawing.Point(6, 53);
            this.cB_DualSaveFlag.Name = "cB_DualSaveFlag";
            this.cB_DualSaveFlag.Size = new System.Drawing.Size(48, 16);
            this.cB_DualSaveFlag.TabIndex = 3;
            this.cB_DualSaveFlag.Text = "保存";
            this.cB_DualSaveFlag.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(102, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 2;
            this.label10.Text = "label1";
            // 
            // cbB_DualAndSing
            // 
            this.cbB_DualAndSing.FormattingEnabled = true;
            this.cbB_DualAndSing.Items.AddRange(new object[] {
            "Dual",
            "Single"});
            this.cbB_DualAndSing.Location = new System.Drawing.Point(6, 20);
            this.cbB_DualAndSing.Name = "cbB_DualAndSing";
            this.cbB_DualAndSing.Size = new System.Drawing.Size(90, 20);
            this.cbB_DualAndSing.TabIndex = 0;
            // 
            // bt_GetDual
            // 
            this.bt_GetDual.Location = new System.Drawing.Point(215, 49);
            this.bt_GetDual.Name = "bt_GetDual";
            this.bt_GetDual.Size = new System.Drawing.Size(75, 23);
            this.bt_GetDual.TabIndex = 1;
            this.bt_GetDual.Text = "获取";
            this.bt_GetDual.UseVisualStyleBackColor = true;
            this.bt_GetDual.Click += new System.EventHandler(this.bt_GetDual_Click);
            // 
            // bt_SetDual
            // 
            this.bt_SetDual.Location = new System.Drawing.Point(215, 18);
            this.bt_SetDual.Name = "bt_SetDual";
            this.bt_SetDual.Size = new System.Drawing.Size(75, 23);
            this.bt_SetDual.TabIndex = 1;
            this.bt_SetDual.Text = "设置";
            this.bt_SetDual.UseVisualStyleBackColor = true;
            this.bt_SetDual.Click += new System.EventHandler(this.bt_SetDual_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.tB_RSSI);
            this.groupBox4.Controls.Add(this.bt_GetRSSI);
            this.groupBox4.Location = new System.Drawing.Point(314, 71);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(296, 78);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "获取环境RSSI值:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(82, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "dBm";
            // 
            // tB_RSSI
            // 
            this.tB_RSSI.Location = new System.Drawing.Point(6, 20);
            this.tB_RSSI.Name = "tB_RSSI";
            this.tB_RSSI.ReadOnly = true;
            this.tB_RSSI.Size = new System.Drawing.Size(70, 21);
            this.tB_RSSI.TabIndex = 0;
            // 
            // bt_GetRSSI
            // 
            this.bt_GetRSSI.Location = new System.Drawing.Point(215, 18);
            this.bt_GetRSSI.Name = "bt_GetRSSI";
            this.bt_GetRSSI.Size = new System.Drawing.Size(75, 23);
            this.bt_GetRSSI.TabIndex = 1;
            this.bt_GetRSSI.Text = "获取";
            this.bt_GetRSSI.UseVisualStyleBackColor = true;
            this.bt_GetRSSI.Click += new System.EventHandler(this.bt_GetRSSI_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.tB_LocalPort);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.bt_SetNetPara);
            this.groupBox5.Controls.Add(this.tB_LocalGateWay);
            this.groupBox5.Controls.Add(this.tB_LocalIP);
            this.groupBox5.Controls.Add(this.tB_DestPort);
            this.groupBox5.Controls.Add(this.tB_DestIP);
            this.groupBox5.Controls.Add(this.cB_NetWork);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Location = new System.Drawing.Point(12, 312);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(598, 88);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "网口参数设置:";
            this.groupBox5.Visible = false;
            // 
            // tB_LocalPort
            // 
            this.tB_LocalPort.Location = new System.Drawing.Point(501, 32);
            this.tB_LocalPort.Name = "tB_LocalPort";
            this.tB_LocalPort.Size = new System.Drawing.Size(90, 21);
            this.tB_LocalPort.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(6, 64);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(292, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "注意: 网口参数设置只针对带有网口通信的阅读器";
            // 
            // bt_SetNetPara
            // 
            this.bt_SetNetPara.Location = new System.Drawing.Point(517, 59);
            this.bt_SetNetPara.Name = "bt_SetNetPara";
            this.bt_SetNetPara.Size = new System.Drawing.Size(75, 23);
            this.bt_SetNetPara.TabIndex = 1;
            this.bt_SetNetPara.Text = "设置";
            this.bt_SetNetPara.UseVisualStyleBackColor = true;
            // 
            // tB_LocalGateWay
            // 
            this.tB_LocalGateWay.Location = new System.Drawing.Point(402, 32);
            this.tB_LocalGateWay.Name = "tB_LocalGateWay";
            this.tB_LocalGateWay.Size = new System.Drawing.Size(90, 21);
            this.tB_LocalGateWay.TabIndex = 0;
            // 
            // tB_LocalIP
            // 
            this.tB_LocalIP.Location = new System.Drawing.Point(303, 32);
            this.tB_LocalIP.Name = "tB_LocalIP";
            this.tB_LocalIP.Size = new System.Drawing.Size(90, 21);
            this.tB_LocalIP.TabIndex = 0;
            // 
            // tB_DestPort
            // 
            this.tB_DestPort.Location = new System.Drawing.Point(204, 32);
            this.tB_DestPort.Name = "tB_DestPort";
            this.tB_DestPort.Size = new System.Drawing.Size(90, 21);
            this.tB_DestPort.TabIndex = 0;
            // 
            // tB_DestIP
            // 
            this.tB_DestIP.Location = new System.Drawing.Point(105, 32);
            this.tB_DestIP.Name = "tB_DestIP";
            this.tB_DestIP.Size = new System.Drawing.Size(90, 21);
            this.tB_DestIP.TabIndex = 0;
            // 
            // cB_NetWork
            // 
            this.cB_NetWork.FormattingEnabled = true;
            this.cB_NetWork.Items.AddRange(new object[] {
            "UDP",
            "TCP Client",
            "UDP Server",
            "TCP Server"});
            this.cB_NetWork.Location = new System.Drawing.Point(6, 32);
            this.cB_NetWork.Name = "cB_NetWork";
            this.cB_NetWork.Size = new System.Drawing.Size(90, 20);
            this.cB_NetWork.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(499, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "本机端口:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(400, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "本机网关:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(301, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "本机IP:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(202, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "目标端口:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(103, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "目标IP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "工作方式:";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cB_epctidSaveFlag);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Controls.Add(this.cbB_EPCTIDTOG);
            this.groupBox6.Controls.Add(this.bt_GetEpcTidTogether);
            this.groupBox6.Controls.Add(this.bt_SetEpcTidTogether);
            this.groupBox6.Location = new System.Drawing.Point(12, 155);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(296, 78);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "数据上报模式设置:";
            // 
            // cB_epctidSaveFlag
            // 
            this.cB_epctidSaveFlag.AutoSize = true;
            this.cB_epctidSaveFlag.Location = new System.Drawing.Point(6, 53);
            this.cB_epctidSaveFlag.Name = "cB_epctidSaveFlag";
            this.cB_epctidSaveFlag.Size = new System.Drawing.Size(48, 16);
            this.cB_epctidSaveFlag.TabIndex = 3;
            this.cB_epctidSaveFlag.Text = "保存";
            this.cB_epctidSaveFlag.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(102, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 2;
            this.label12.Text = "label1";
            // 
            // cbB_EPCTIDTOG
            // 
            this.cbB_EPCTIDTOG.FormattingEnabled = true;
            this.cbB_EPCTIDTOG.Items.AddRange(new object[] {
            "EPC",
            "EPC+TID",
            "EPC+USR"});
            this.cbB_EPCTIDTOG.Location = new System.Drawing.Point(6, 20);
            this.cbB_EPCTIDTOG.Name = "cbB_EPCTIDTOG";
            this.cbB_EPCTIDTOG.Size = new System.Drawing.Size(90, 20);
            this.cbB_EPCTIDTOG.TabIndex = 0;
            // 
            // bt_GetEpcTidTogether
            // 
            this.bt_GetEpcTidTogether.Location = new System.Drawing.Point(215, 49);
            this.bt_GetEpcTidTogether.Name = "bt_GetEpcTidTogether";
            this.bt_GetEpcTidTogether.Size = new System.Drawing.Size(75, 23);
            this.bt_GetEpcTidTogether.TabIndex = 1;
            this.bt_GetEpcTidTogether.Text = "获取";
            this.bt_GetEpcTidTogether.UseVisualStyleBackColor = true;
            this.bt_GetEpcTidTogether.Click += new System.EventHandler(this.bt_GetEpcTidTogether_Click);
            // 
            // bt_SetEpcTidTogether
            // 
            this.bt_SetEpcTidTogether.Location = new System.Drawing.Point(215, 18);
            this.bt_SetEpcTidTogether.Name = "bt_SetEpcTidTogether";
            this.bt_SetEpcTidTogether.Size = new System.Drawing.Size(75, 23);
            this.bt_SetEpcTidTogether.TabIndex = 1;
            this.bt_SetEpcTidTogether.Text = "设置";
            this.bt_SetEpcTidTogether.UseVisualStyleBackColor = true;
            this.bt_SetEpcTidTogether.Click += new System.EventHandler(this.bt_SetEpcTidTogether_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label13);
            this.groupBox7.Controls.Add(this.bt_FactorySet);
            this.groupBox7.Location = new System.Drawing.Point(314, 155);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(296, 78);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "恢复出厂设置:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 2;
            this.label13.Text = "label1";
            // 
            // bt_FactorySet
            // 
            this.bt_FactorySet.Location = new System.Drawing.Point(215, 18);
            this.bt_FactorySet.Name = "bt_FactorySet";
            this.bt_FactorySet.Size = new System.Drawing.Size(75, 23);
            this.bt_FactorySet.TabIndex = 1;
            this.bt_FactorySet.Text = "恢复设置";
            this.bt_FactorySet.UseVisualStyleBackColor = true;
            this.bt_FactorySet.Click += new System.EventHandler(this.bt_FactorySet_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label14);
            this.groupBox8.Controls.Add(this.cB_pandianSaveflag);
            this.groupBox8.Controls.Add(this.Bt_Setpandian);
            this.groupBox8.Controls.Add(this.bt_Getpandian);
            this.groupBox8.Controls.Add(this.cB_pandian);
            this.groupBox8.Location = new System.Drawing.Point(12, 239);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(598, 67);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "盘点模式设置与获取";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(524, 34);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 12);
            this.label14.TabIndex = 4;
            this.label14.Text = "label14";
            // 
            // cB_pandianSaveflag
            // 
            this.cB_pandianSaveflag.AutoSize = true;
            this.cB_pandianSaveflag.Location = new System.Drawing.Point(271, 30);
            this.cB_pandianSaveflag.Name = "cB_pandianSaveflag";
            this.cB_pandianSaveflag.Size = new System.Drawing.Size(48, 16);
            this.cB_pandianSaveflag.TabIndex = 3;
            this.cB_pandianSaveflag.Text = "保存";
            this.cB_pandianSaveflag.UseVisualStyleBackColor = true;
            // 
            // Bt_Setpandian
            // 
            this.Bt_Setpandian.Location = new System.Drawing.Point(426, 26);
            this.Bt_Setpandian.Name = "Bt_Setpandian";
            this.Bt_Setpandian.Size = new System.Drawing.Size(75, 23);
            this.Bt_Setpandian.TabIndex = 2;
            this.Bt_Setpandian.Text = "设置";
            this.Bt_Setpandian.UseVisualStyleBackColor = true;
            this.Bt_Setpandian.Click += new System.EventHandler(this.Bt_Setpandian_Click);
            // 
            // bt_Getpandian
            // 
            this.bt_Getpandian.Location = new System.Drawing.Point(339, 26);
            this.bt_Getpandian.Name = "bt_Getpandian";
            this.bt_Getpandian.Size = new System.Drawing.Size(75, 23);
            this.bt_Getpandian.TabIndex = 1;
            this.bt_Getpandian.Text = "获取";
            this.bt_Getpandian.UseVisualStyleBackColor = true;
            this.bt_Getpandian.Click += new System.EventHandler(this.bt_Getpandian_Click);
            // 
            // cB_pandian
            // 
            this.cB_pandian.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cB_pandian.FormattingEnabled = true;
            this.cB_pandian.Items.AddRange(new object[] {
            "模式1：多标签模式",
            "模式2：快速读取模式",
            "模式3",
            "模式4",
            "模式5",
            "模式6",
            "模式7",
            "模式8",
            "模式9"});
            this.cB_pandian.Location = new System.Drawing.Point(6, 29);
            this.cB_pandian.Name = "cB_pandian";
            this.cB_pandian.Size = new System.Drawing.Size(228, 20);
            this.cB_pandian.TabIndex = 0;
            // 
            // OtherSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 312);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OtherSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "其他设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OtherSet_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bt_BaudSet;
        private System.Windows.Forms.ComboBox cbB_Baud;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_softreset;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbB_DualAndSing;
        private System.Windows.Forms.Button bt_GetDual;
        private System.Windows.Forms.Button bt_SetDual;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tB_RSSI;
        private System.Windows.Forms.Button bt_GetRSSI;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox tB_LocalPort;
        private System.Windows.Forms.Button bt_SetNetPara;
        private System.Windows.Forms.TextBox tB_LocalGateWay;
        private System.Windows.Forms.TextBox tB_LocalIP;
        private System.Windows.Forms.TextBox tB_DestPort;
        private System.Windows.Forms.TextBox tB_DestIP;
        private System.Windows.Forms.ComboBox cB_NetWork;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cB_DualSaveFlag;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox cB_epctidSaveFlag;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbB_EPCTIDTOG;
        private System.Windows.Forms.Button bt_GetEpcTidTogether;
        private System.Windows.Forms.Button bt_SetEpcTidTogether;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button bt_FactorySet;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox cB_pandianSaveflag;
        private System.Windows.Forms.Button Bt_Setpandian;
        private System.Windows.Forms.Button bt_Getpandian;
        private System.Windows.Forms.ComboBox cB_pandian;
        private System.Windows.Forms.Label label14;
    }
}