namespace R2000Demo
{
    partial class TagSpecial
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cB_RangeCt = new System.Windows.Forms.ComboBox();
            this.cB_MemoryMap = new System.Windows.Forms.ComboBox();
            this.bt_QTParaSet = new System.Windows.Forms.Button();
            this.bt_QTParaGet = new System.Windows.Forms.Button();
            this.lb_QTResult = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lb_QTResult);
            this.groupBox1.Controls.Add(this.bt_QTParaGet);
            this.groupBox1.Controls.Add(this.bt_QTParaSet);
            this.groupBox1.Controls.Add(this.cB_MemoryMap);
            this.groupBox1.Controls.Add(this.cB_RangeCt);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(570, 67);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "QT";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "距离控制";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Memory Map";
            // 
            // cB_RangeCt
            // 
            this.cB_RangeCt.FormattingEnabled = true;
            this.cB_RangeCt.Items.AddRange(new object[] {
            "Not reduces range",
            "Reduces range"});
            this.cB_RangeCt.Location = new System.Drawing.Point(65, 12);
            this.cB_RangeCt.Name = "cB_RangeCt";
            this.cB_RangeCt.Size = new System.Drawing.Size(125, 20);
            this.cB_RangeCt.TabIndex = 1;
            // 
            // cB_MemoryMap
            // 
            this.cB_MemoryMap.FormattingEnabled = true;
            this.cB_MemoryMap.Items.AddRange(new object[] {
            "Public",
            "Private"});
            this.cB_MemoryMap.Location = new System.Drawing.Point(311, 12);
            this.cB_MemoryMap.Name = "cB_MemoryMap";
            this.cB_MemoryMap.Size = new System.Drawing.Size(80, 20);
            this.cB_MemoryMap.TabIndex = 1;
            // 
            // bt_QTParaSet
            // 
            this.bt_QTParaSet.Location = new System.Drawing.Point(489, 10);
            this.bt_QTParaSet.Name = "bt_QTParaSet";
            this.bt_QTParaSet.Size = new System.Drawing.Size(75, 23);
            this.bt_QTParaSet.TabIndex = 2;
            this.bt_QTParaSet.Text = "设置";
            this.bt_QTParaSet.UseVisualStyleBackColor = true;
            this.bt_QTParaSet.Click += new System.EventHandler(this.bt_QTParaSet_Click);
            // 
            // bt_QTParaGet
            // 
            this.bt_QTParaGet.Location = new System.Drawing.Point(489, 39);
            this.bt_QTParaGet.Name = "bt_QTParaGet";
            this.bt_QTParaGet.Size = new System.Drawing.Size(75, 23);
            this.bt_QTParaGet.TabIndex = 2;
            this.bt_QTParaGet.Text = "获取";
            this.bt_QTParaGet.UseVisualStyleBackColor = true;
            // 
            // lb_QTResult
            // 
            this.lb_QTResult.AutoSize = true;
            this.lb_QTResult.Location = new System.Drawing.Point(397, 17);
            this.lb_QTResult.Name = "lb_QTResult";
            this.lb_QTResult.Size = new System.Drawing.Size(11, 12);
            this.lb_QTResult.TabIndex = 1;
            this.lb_QTResult.Text = "l";
            // 
            // TagSpecial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 572);
            this.Controls.Add(this.groupBox1);
            this.Name = "TagSpecial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "特殊功能";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bt_QTParaGet;
        private System.Windows.Forms.Button bt_QTParaSet;
        private System.Windows.Forms.ComboBox cB_MemoryMap;
        private System.Windows.Forms.ComboBox cB_RangeCt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_QTResult;
    }
}