namespace R2000Demo
{
    partial class RegOperate
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.bt_RegWrite = new System.Windows.Forms.Button();
            this.bt_RegRead = new System.Windows.Forms.Button();
            this.cB_RegType = new System.Windows.Forms.ComboBox();
            this.tB_RegData = new System.Windows.Forms.TextBox();
            this.tB_RegAddr = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.bt_exit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tB_ErrorFlag = new System.Windows.Forms.TextBox();
            this.bt_GetErrorFlag = new System.Windows.Forms.Button();
            this.bt_ClearErrorFlag = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.bt_RegWrite);
            this.groupBox3.Controls.Add(this.bt_RegRead);
            this.groupBox3.Controls.Add(this.cB_RegType);
            this.groupBox3.Controls.Add(this.tB_RegData);
            this.groupBox3.Controls.Add(this.tB_RegAddr);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(6, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(344, 115);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "寄存器";
            // 
            // bt_RegWrite
            // 
            this.bt_RegWrite.Location = new System.Drawing.Point(248, 81);
            this.bt_RegWrite.Name = "bt_RegWrite";
            this.bt_RegWrite.Size = new System.Drawing.Size(75, 23);
            this.bt_RegWrite.TabIndex = 2;
            this.bt_RegWrite.Text = "Write";
            this.bt_RegWrite.UseVisualStyleBackColor = true;
            this.bt_RegWrite.Click += new System.EventHandler(this.bt_RegWrite_Click);
            // 
            // bt_RegRead
            // 
            this.bt_RegRead.Location = new System.Drawing.Point(248, 21);
            this.bt_RegRead.Name = "bt_RegRead";
            this.bt_RegRead.Size = new System.Drawing.Size(75, 23);
            this.bt_RegRead.TabIndex = 2;
            this.bt_RegRead.Text = "Read";
            this.bt_RegRead.UseVisualStyleBackColor = true;
            this.bt_RegRead.Click += new System.EventHandler(this.bt_RegRead_Click);
            // 
            // cB_RegType
            // 
            this.cB_RegType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cB_RegType.FormattingEnabled = true;
            this.cB_RegType.Items.AddRange(new object[] {
            "MAC",
            "OEM",
            "ByPass"});
            this.cB_RegType.Location = new System.Drawing.Point(101, 23);
            this.cB_RegType.Name = "cB_RegType";
            this.cB_RegType.Size = new System.Drawing.Size(100, 20);
            this.cB_RegType.TabIndex = 1;
            this.cB_RegType.SelectedIndexChanged += new System.EventHandler(this.cB_RegType_SelectedIndexChanged);
            // 
            // tB_RegData
            // 
            this.tB_RegData.Location = new System.Drawing.Point(101, 83);
            this.tB_RegData.MaxLength = 8;
            this.tB_RegData.Name = "tB_RegData";
            this.tB_RegData.Size = new System.Drawing.Size(100, 21);
            this.tB_RegData.TabIndex = 1;
            this.tB_RegData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tB_RegData_KeyPress);
            // 
            // tB_RegAddr
            // 
            this.tB_RegAddr.Location = new System.Drawing.Point(101, 51);
            this.tB_RegAddr.MaxLength = 8;
            this.tB_RegAddr.Name = "tB_RegAddr";
            this.tB_RegAddr.Size = new System.Drawing.Size(100, 21);
            this.tB_RegAddr.TabIndex = 1;
            this.tB_RegAddr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tB_RegAddr_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "寄存器值：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "寄存器地址：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "寄存器类型：";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox1);
            this.groupBox4.Controls.Add(this.bt_exit);
            this.groupBox4.Controls.Add(this.groupBox3);
            this.groupBox4.Location = new System.Drawing.Point(12, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(356, 249);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "工作区";
            // 
            // bt_exit
            // 
            this.bt_exit.Location = new System.Drawing.Point(254, 220);
            this.bt_exit.Name = "bt_exit";
            this.bt_exit.Size = new System.Drawing.Size(75, 23);
            this.bt_exit.TabIndex = 10;
            this.bt_exit.Text = "退出";
            this.bt_exit.UseVisualStyleBackColor = true;
            this.bt_exit.Click += new System.EventHandler(this.bt_exit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "错误标志：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bt_ClearErrorFlag);
            this.groupBox1.Controls.Add(this.bt_GetErrorFlag);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tB_ErrorFlag);
            this.groupBox1.Location = new System.Drawing.Point(7, 141);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(343, 73);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Error Flag";
            // 
            // tB_ErrorFlag
            // 
            this.tB_ErrorFlag.Location = new System.Drawing.Point(100, 14);
            this.tB_ErrorFlag.MaxLength = 8;
            this.tB_ErrorFlag.Name = "tB_ErrorFlag";
            this.tB_ErrorFlag.ReadOnly = true;
            this.tB_ErrorFlag.Size = new System.Drawing.Size(100, 21);
            this.tB_ErrorFlag.TabIndex = 1;
            this.tB_ErrorFlag.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tB_RegData_KeyPress);
            // 
            // bt_GetErrorFlag
            // 
            this.bt_GetErrorFlag.Location = new System.Drawing.Point(247, 12);
            this.bt_GetErrorFlag.Name = "bt_GetErrorFlag";
            this.bt_GetErrorFlag.Size = new System.Drawing.Size(75, 23);
            this.bt_GetErrorFlag.TabIndex = 2;
            this.bt_GetErrorFlag.Text = "获取";
            this.bt_GetErrorFlag.UseVisualStyleBackColor = true;
            this.bt_GetErrorFlag.Click += new System.EventHandler(this.bt_GetErrorFlag_Click);
            // 
            // bt_ClearErrorFlag
            // 
            this.bt_ClearErrorFlag.Location = new System.Drawing.Point(247, 44);
            this.bt_ClearErrorFlag.Name = "bt_ClearErrorFlag";
            this.bt_ClearErrorFlag.Size = new System.Drawing.Size(75, 23);
            this.bt_ClearErrorFlag.TabIndex = 2;
            this.bt_ClearErrorFlag.Text = "清除";
            this.bt_ClearErrorFlag.UseVisualStyleBackColor = true;
            this.bt_ClearErrorFlag.Click += new System.EventHandler(this.bt_ClearErrorFlag_Click);
            // 
            // RegOperate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 273);
            this.Controls.Add(this.groupBox4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegOperate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "寄存器";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cB_RegType;
        private System.Windows.Forms.TextBox tB_RegData;
        private System.Windows.Forms.TextBox tB_RegAddr;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bt_RegWrite;
        private System.Windows.Forms.Button bt_RegRead;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button bt_exit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bt_ClearErrorFlag;
        private System.Windows.Forms.Button bt_GetErrorFlag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tB_ErrorFlag;
    }
}