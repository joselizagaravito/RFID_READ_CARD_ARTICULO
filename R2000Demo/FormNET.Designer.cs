namespace R2000Demo
{
    partial class FormNET
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
            this.tB_Gateway = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tB_tagetip = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tB_MAC = new System.Windows.Forms.TextBox();
            this.tB_ID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tB_NAME = new System.Windows.Forms.TextBox();
            this.tB_PASSWD = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cB_iptype = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.bt_SetNetPara = new System.Windows.Forms.Button();
            this.tB_DestIP = new System.Windows.Forms.TextBox();
            this.cB_NetWork = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.tB_Gateway);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.tB_tagetip);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.tB_MAC);
            this.groupBox5.Controls.Add(this.tB_ID);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.tB_NAME);
            this.groupBox5.Controls.Add(this.tB_PASSWD);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.cB_iptype);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.bt_SetNetPara);
            this.groupBox5.Controls.Add(this.tB_DestIP);
            this.groupBox5.Controls.Add(this.cB_NetWork);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Location = new System.Drawing.Point(12, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(783, 129);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "网口参数设置:";
            // 
            // tB_Gateway
            // 
            this.tB_Gateway.Location = new System.Drawing.Point(292, 83);
            this.tB_Gateway.Name = "tB_Gateway";
            this.tB_Gateway.Size = new System.Drawing.Size(127, 21);
            this.tB_Gateway.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(290, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "Gate";
            // 
            // tB_tagetip
            // 
            this.tB_tagetip.Location = new System.Drawing.Point(10, 81);
            this.tB_tagetip.Name = "tB_tagetip";
            this.tB_tagetip.Size = new System.Drawing.Size(276, 21);
            this.tB_tagetip.TabIndex = 18;
            this.tB_tagetip.TextChanged += new System.EventHandler(this.tB_tagetip_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(137, 12);
            this.label10.TabIndex = 17;
            this.label10.Text = "Target IP/domain names";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(290, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 12);
            this.label9.TabIndex = 16;
            this.label9.Text = "MAC";
            // 
            // tB_MAC
            // 
            this.tB_MAC.Location = new System.Drawing.Point(292, 32);
            this.tB_MAC.Name = "tB_MAC";
            this.tB_MAC.Size = new System.Drawing.Size(127, 21);
            this.tB_MAC.TabIndex = 15;
            // 
            // tB_ID
            // 
            this.tB_ID.Location = new System.Drawing.Point(644, 32);
            this.tB_ID.Name = "tB_ID";
            this.tB_ID.Size = new System.Drawing.Size(100, 21);
            this.tB_ID.TabIndex = 14;
            this.tB_ID.TextChanged += new System.EventHandler(this.tB_ID_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(642, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "ID";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(527, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "Name";
            // 
            // tB_NAME
            // 
            this.tB_NAME.Location = new System.Drawing.Point(529, 32);
            this.tB_NAME.Name = "tB_NAME";
            this.tB_NAME.Size = new System.Drawing.Size(100, 21);
            this.tB_NAME.TabIndex = 11;
            this.tB_NAME.Text = "admin";
            this.tB_NAME.TextChanged += new System.EventHandler(this.tB_NAME_TextChanged);
            // 
            // tB_PASSWD
            // 
            this.tB_PASSWD.Location = new System.Drawing.Point(529, 83);
            this.tB_PASSWD.Name = "tB_PASSWD";
            this.tB_PASSWD.Size = new System.Drawing.Size(100, 21);
            this.tB_PASSWD.TabIndex = 10;
            this.tB_PASSWD.Text = "admin";
            this.tB_PASSWD.TextChanged += new System.EventHandler(this.tB_PASSWD_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(527, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(424, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(209, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP Type";
            // 
            // cB_iptype
            // 
            this.cB_iptype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cB_iptype.FormattingEnabled = true;
            this.cB_iptype.Items.AddRange(new object[] {
            "静态IP",
            "动态IP"});
            this.cB_iptype.Location = new System.Drawing.Point(201, 32);
            this.cB_iptype.Name = "cB_iptype";
            this.cB_iptype.Size = new System.Drawing.Size(85, 20);
            this.cB_iptype.TabIndex = 3;
            this.cB_iptype.TextChanged += new System.EventHandler(this.cB_iptype_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(-2, 114);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(292, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "注意: 网口参数设置只针对带有网口通信的阅读器";
            // 
            // bt_SetNetPara
            // 
            this.bt_SetNetPara.Enabled = false;
            this.bt_SetNetPara.Location = new System.Drawing.Point(686, 81);
            this.bt_SetNetPara.Name = "bt_SetNetPara";
            this.bt_SetNetPara.Size = new System.Drawing.Size(75, 23);
            this.bt_SetNetPara.TabIndex = 1;
            this.bt_SetNetPara.Text = "Set";
            this.bt_SetNetPara.UseVisualStyleBackColor = true;
            this.bt_SetNetPara.Click += new System.EventHandler(this.bt_SetNetPara_Click);
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
            this.cB_NetWork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cB_NetWork.FormattingEnabled = true;
            this.cB_NetWork.Items.AddRange(new object[] {
            "UDP Client",
            "TCP Client",
            "UDP Server",
            "TCP Server",
            "Httpd Client"});
            this.cB_NetWork.Location = new System.Drawing.Point(6, 32);
            this.cB_NetWork.Name = "cB_NetWork";
            this.cB_NetWork.Size = new System.Drawing.Size(90, 20);
            this.cB_NetWork.TabIndex = 0;
            this.cB_NetWork.SelectedIndexChanged += new System.EventHandler(this.cB_NetWork_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(103, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "IP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "Work Type:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(576, 267);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Quest";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.Location = new System.Drawing.Point(12, 148);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(558, 142);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "index";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ip";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "MAC";
            this.columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "name";
            this.columnHeader4.Width = 150;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(576, 238);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "CLEAR";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(576, 209);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "rerset";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(576, 148);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // FormNET
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 302);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox5);
            this.Name = "FormNET";
            this.Text = "网口参数设置";
            this.Load += new System.EventHandler(this.FormNET_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button bt_SetNetPara;
        private System.Windows.Forms.TextBox tB_DestIP;
        private System.Windows.Forms.ComboBox cB_NetWork;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox tB_ID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tB_NAME;
        private System.Windows.Forms.TextBox tB_PASSWD;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cB_iptype;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tB_MAC;
        private System.Windows.Forms.TextBox tB_Gateway;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tB_tagetip;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}