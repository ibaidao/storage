namespace ViewDevice
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbXValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbZValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbYValue = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.ckbBlock = new System.Windows.Forms.CheckBox();
            this.cbkLowBattery = new System.Windows.Forms.CheckBox();
            this.ckbStandby = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbYValue);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbZValue);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbXValue);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(35, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(471, 105);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "当前坐标";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "X 轴";
            // 
            // tbXValue
            // 
            this.tbXValue.Location = new System.Drawing.Point(76, 56);
            this.tbXValue.Name = "tbXValue";
            this.tbXValue.Size = new System.Drawing.Size(71, 25);
            this.tbXValue.TabIndex = 1;
            this.tbXValue.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(323, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Z 轴";
            // 
            // tbZValue
            // 
            this.tbZValue.Location = new System.Drawing.Point(367, 56);
            this.tbZValue.Name = "tbZValue";
            this.tbZValue.Size = new System.Drawing.Size(71, 25);
            this.tbZValue.TabIndex = 1;
            this.tbZValue.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(181, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Y 轴";
            // 
            // tbYValue
            // 
            this.tbYValue.Location = new System.Drawing.Point(225, 56);
            this.tbYValue.Name = "tbYValue";
            this.tbYValue.Size = new System.Drawing.Size(71, 25);
            this.tbYValue.TabIndex = 1;
            this.tbYValue.Text = "2";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(398, 178);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "汇报";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // ckbBlock
            // 
            this.ckbBlock.AutoSize = true;
            this.ckbBlock.Location = new System.Drawing.Point(78, 178);
            this.ckbBlock.Name = "ckbBlock";
            this.ckbBlock.Size = new System.Drawing.Size(104, 19);
            this.ckbBlock.TabIndex = 2;
            this.ckbBlock.Text = "前面有障碍";
            this.ckbBlock.UseVisualStyleBackColor = true;
            // 
            // cbkLowBattery
            // 
            this.cbkLowBattery.AutoSize = true;
            this.cbkLowBattery.Location = new System.Drawing.Point(207, 178);
            this.cbkLowBattery.Name = "cbkLowBattery";
            this.cbkLowBattery.Size = new System.Drawing.Size(74, 19);
            this.cbkLowBattery.TabIndex = 2;
            this.cbkLowBattery.Text = "电量低";
            this.cbkLowBattery.UseVisualStyleBackColor = true;
            // 
            // ckbStandby
            // 
            this.ckbStandby.AutoSize = true;
            this.ckbStandby.Location = new System.Drawing.Point(302, 178);
            this.ckbStandby.Name = "ckbStandby";
            this.ckbStandby.Size = new System.Drawing.Size(59, 19);
            this.ckbStandby.TabIndex = 2;
            this.ckbStandby.Text = "候命";
            this.ckbStandby.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 243);
            this.Controls.Add(this.ckbStandby);
            this.Controls.Add(this.cbkLowBattery);
            this.Controls.Add(this.ckbBlock);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.groupBox1);
            this.Name = "Main";
            this.Text = "我是小车";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbXValue;
        private System.Windows.Forms.TextBox tbYValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbZValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.CheckBox ckbBlock;
        private System.Windows.Forms.CheckBox cbkLowBattery;
        private System.Windows.Forms.CheckBox ckbStandby;
    }
}

