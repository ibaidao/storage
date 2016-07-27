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
            this.tbYValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbZValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbXValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.ckbBlock = new System.Windows.Forms.CheckBox();
            this.ckbLowBattery = new System.Windows.Forms.CheckBox();
            this.tbDistance = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbVoltage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gbTrouble = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbHoldShelf = new System.Windows.Forms.RadioButton();
            this.rbCanPicking = new System.Windows.Forms.RadioButton();
            this.rbFreeShelf = new System.Windows.Forms.RadioButton();
            this.rbTrouble = new System.Windows.Forms.RadioButton();
            this.rbHeart = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.gbTrouble.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            // tbYValue
            // 
            this.tbYValue.Location = new System.Drawing.Point(225, 56);
            this.tbYValue.Name = "tbYValue";
            this.tbYValue.Size = new System.Drawing.Size(71, 25);
            this.tbYValue.TabIndex = 1;
            this.tbYValue.Text = "2";
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
            // tbZValue
            // 
            this.tbZValue.Location = new System.Drawing.Point(367, 56);
            this.tbZValue.Name = "tbZValue";
            this.tbZValue.Size = new System.Drawing.Size(71, 25);
            this.tbZValue.TabIndex = 1;
            this.tbZValue.Text = "0";
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
            // tbXValue
            // 
            this.tbXValue.Location = new System.Drawing.Point(76, 56);
            this.tbXValue.Name = "tbXValue";
            this.tbXValue.Size = new System.Drawing.Size(71, 25);
            this.tbXValue.TabIndex = 1;
            this.tbXValue.Text = "1";
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
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(203, 412);
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
            this.ckbBlock.Location = new System.Drawing.Point(61, 34);
            this.ckbBlock.Name = "ckbBlock";
            this.ckbBlock.Size = new System.Drawing.Size(104, 19);
            this.ckbBlock.TabIndex = 2;
            this.ckbBlock.Text = "前面有障碍";
            this.ckbBlock.UseVisualStyleBackColor = true;
            // 
            // ckbLowBattery
            // 
            this.ckbLowBattery.AutoSize = true;
            this.ckbLowBattery.Location = new System.Drawing.Point(61, 82);
            this.ckbLowBattery.Name = "ckbLowBattery";
            this.ckbLowBattery.Size = new System.Drawing.Size(74, 19);
            this.ckbLowBattery.TabIndex = 2;
            this.ckbLowBattery.Text = "电量低";
            this.ckbLowBattery.UseVisualStyleBackColor = true;
            // 
            // tbDistance
            // 
            this.tbDistance.Location = new System.Drawing.Point(310, 32);
            this.tbDistance.Name = "tbDistance";
            this.tbDistance.Size = new System.Drawing.Size(100, 25);
            this.tbDistance.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(237, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "障碍距离";
            // 
            // tbVoltage
            // 
            this.tbVoltage.Location = new System.Drawing.Point(310, 80);
            this.tbVoltage.Name = "tbVoltage";
            this.tbVoltage.Size = new System.Drawing.Size(100, 25);
            this.tbVoltage.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(237, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "电量值";
            // 
            // gbTrouble
            // 
            this.gbTrouble.Controls.Add(this.ckbBlock);
            this.gbTrouble.Controls.Add(this.label5);
            this.gbTrouble.Controls.Add(this.ckbLowBattery);
            this.gbTrouble.Controls.Add(this.label4);
            this.gbTrouble.Controls.Add(this.tbDistance);
            this.gbTrouble.Controls.Add(this.tbVoltage);
            this.gbTrouble.Location = new System.Drawing.Point(35, 260);
            this.gbTrouble.Name = "gbTrouble";
            this.gbTrouble.Size = new System.Drawing.Size(471, 123);
            this.gbTrouble.TabIndex = 5;
            this.gbTrouble.TabStop = false;
            this.gbTrouble.Text = "异常报告";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbTrouble);
            this.groupBox3.Controls.Add(this.rbFreeShelf);
            this.groupBox3.Controls.Add(this.rbCanPicking);
            this.groupBox3.Controls.Add(this.rbHeart);
            this.groupBox3.Controls.Add(this.rbHoldShelf);
            this.groupBox3.Location = new System.Drawing.Point(35, 145);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(471, 109);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "工作报告";
            // 
            // rbHoldShelf
            // 
            this.rbHoldShelf.AutoSize = true;
            this.rbHoldShelf.Location = new System.Drawing.Point(94, 52);
            this.rbHoldShelf.Name = "rbHoldShelf";
            this.rbHoldShelf.Size = new System.Drawing.Size(88, 19);
            this.rbHoldShelf.TabIndex = 0;
            this.rbHoldShelf.TabStop = true;
            this.rbHoldShelf.Text = "抬起货架";
            this.rbHoldShelf.UseVisualStyleBackColor = true;
            this.rbHoldShelf.Click += new System.EventHandler(this.rbItem_Click);
            // 
            // rbCanPicking
            // 
            this.rbCanPicking.AutoSize = true;
            this.rbCanPicking.Location = new System.Drawing.Point(188, 52);
            this.rbCanPicking.Name = "rbCanPicking";
            this.rbCanPicking.Size = new System.Drawing.Size(88, 19);
            this.rbCanPicking.TabIndex = 0;
            this.rbCanPicking.TabStop = true;
            this.rbCanPicking.Text = "到拣货台";
            this.rbCanPicking.UseVisualStyleBackColor = true;
            this.rbCanPicking.Click += new System.EventHandler(this.rbItem_Click);
            // 
            // rbFreeShelf
            // 
            this.rbFreeShelf.AutoSize = true;
            this.rbFreeShelf.Location = new System.Drawing.Point(282, 52);
            this.rbFreeShelf.Name = "rbFreeShelf";
            this.rbFreeShelf.Size = new System.Drawing.Size(88, 19);
            this.rbFreeShelf.TabIndex = 0;
            this.rbFreeShelf.TabStop = true;
            this.rbFreeShelf.Text = "放下货架";
            this.rbFreeShelf.UseVisualStyleBackColor = true;
            this.rbFreeShelf.Click += new System.EventHandler(this.rbItem_Click);
            // 
            // rbTrouble
            // 
            this.rbTrouble.AutoSize = true;
            this.rbTrouble.Location = new System.Drawing.Point(380, 52);
            this.rbTrouble.Name = "rbTrouble";
            this.rbTrouble.Size = new System.Drawing.Size(58, 19);
            this.rbTrouble.TabIndex = 1;
            this.rbTrouble.TabStop = true;
            this.rbTrouble.Text = "异常";
            this.rbTrouble.UseVisualStyleBackColor = true;
            this.rbTrouble.Click += new System.EventHandler(this.rbItem_Click);
            // 
            // rbHeart
            // 
            this.rbHeart.AutoSize = true;
            this.rbHeart.Location = new System.Drawing.Point(30, 52);
            this.rbHeart.Name = "rbHeart";
            this.rbHeart.Size = new System.Drawing.Size(58, 19);
            this.rbHeart.TabIndex = 0;
            this.rbHeart.TabStop = true;
            this.rbHeart.Text = "心跳";
            this.rbHeart.UseVisualStyleBackColor = true;
            this.rbHeart.Click += new System.EventHandler(this.rbItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 446);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbTrouble);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.groupBox1);
            this.Name = "Main";
            this.Text = "我是小车";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbTrouble.ResumeLayout(false);
            this.gbTrouble.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.CheckBox ckbLowBattery;
        private System.Windows.Forms.TextBox tbDistance;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbVoltage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox gbTrouble;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbHoldShelf;
        private System.Windows.Forms.RadioButton rbCanPicking;
        private System.Windows.Forms.RadioButton rbFreeShelf;
        private System.Windows.Forms.RadioButton rbTrouble;
        private System.Windows.Forms.RadioButton rbHeart;
    }
}

