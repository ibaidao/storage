﻿namespace ViewServer
{
    partial class Setting
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
            this.tbXMap = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbYMap = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cdShow = new System.Windows.Forms.ColorDialog();
            this.lbPick = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbRatio = new System.Windows.Forms.TextBox();
            this.rbMap = new System.Windows.Forms.RadioButton();
            this.rbPick = new System.Windows.Forms.RadioButton();
            this.rbCharge = new System.Windows.Forms.RadioButton();
            this.rbDevice = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbShelf = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbXMap
            // 
            this.tbXMap.Location = new System.Drawing.Point(223, 94);
            this.tbXMap.Name = "tbXMap";
            this.tbXMap.Size = new System.Drawing.Size(57, 25);
            this.tbXMap.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(150, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "横向长度";
            // 
            // tbYMap
            // 
            this.tbYMap.Location = new System.Drawing.Point(329, 94);
            this.tbYMap.Name = "tbYMap";
            this.tbYMap.Size = new System.Drawing.Size(57, 25);
            this.tbYMap.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(286, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "纵向";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "显示颜色";
            // 
            // lbPick
            // 
            this.lbPick.AutoSize = true;
            this.lbPick.Location = new System.Drawing.Point(99, 97);
            this.lbPick.Name = "lbPick";
            this.lbPick.Size = new System.Drawing.Size(0, 15);
            this.lbPick.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "仓库缩放比例";
            // 
            // tbRatio
            // 
            this.tbRatio.Location = new System.Drawing.Point(152, 42);
            this.tbRatio.Name = "tbRatio";
            this.tbRatio.Size = new System.Drawing.Size(100, 25);
            this.tbRatio.TabIndex = 5;
            // 
            // rbMap
            // 
            this.rbMap.AutoSize = true;
            this.rbMap.Location = new System.Drawing.Point(34, 41);
            this.rbMap.Name = "rbMap";
            this.rbMap.Size = new System.Drawing.Size(58, 19);
            this.rbMap.TabIndex = 7;
            this.rbMap.TabStop = true;
            this.rbMap.Text = "仓库";
            this.rbMap.UseVisualStyleBackColor = true;
            // 
            // rbPick
            // 
            this.rbPick.AutoSize = true;
            this.rbPick.Location = new System.Drawing.Point(98, 41);
            this.rbPick.Name = "rbPick";
            this.rbPick.Size = new System.Drawing.Size(73, 19);
            this.rbPick.TabIndex = 7;
            this.rbPick.TabStop = true;
            this.rbPick.Text = "拣货台";
            this.rbPick.UseVisualStyleBackColor = true;
            // 
            // rbCharge
            // 
            this.rbCharge.AutoSize = true;
            this.rbCharge.Location = new System.Drawing.Point(177, 41);
            this.rbCharge.Name = "rbCharge";
            this.rbCharge.Size = new System.Drawing.Size(73, 19);
            this.rbCharge.TabIndex = 7;
            this.rbCharge.TabStop = true;
            this.rbCharge.Text = "充电桩";
            this.rbCharge.UseVisualStyleBackColor = true;
            // 
            // rbDevice
            // 
            this.rbDevice.AutoSize = true;
            this.rbDevice.Location = new System.Drawing.Point(256, 41);
            this.rbDevice.Name = "rbDevice";
            this.rbDevice.Size = new System.Drawing.Size(58, 19);
            this.rbDevice.TabIndex = 7;
            this.rbDevice.TabStop = true;
            this.rbDevice.Text = "设备";
            this.rbDevice.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.rbShelf);
            this.groupBox1.Controls.Add(this.rbDevice);
            this.groupBox1.Controls.Add(this.tbYMap);
            this.groupBox1.Controls.Add(this.rbCharge);
            this.groupBox1.Controls.Add(this.tbXMap);
            this.groupBox1.Controls.Add(this.rbPick);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.rbMap);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lbPick);
            this.groupBox1.Location = new System.Drawing.Point(27, 98);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(417, 138);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "仓库内各模块样式";
            // 
            // rbShelf
            // 
            this.rbShelf.AutoSize = true;
            this.rbShelf.Location = new System.Drawing.Point(320, 41);
            this.rbShelf.Name = "rbShelf";
            this.rbShelf.Size = new System.Drawing.Size(58, 19);
            this.rbShelf.TabIndex = 7;
            this.rbShelf.TabStop = true;
            this.rbShelf.Text = "货架";
            this.rbShelf.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.radioButton4);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.radioButton5);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(27, 242);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(417, 121);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "颜色设置";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(192, 41);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(96, 19);
            this.radioButton3.TabIndex = 7;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "货架+设备";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(113, 41);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(73, 19);
            this.radioButton4.TabIndex = 7;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "单向路";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(82, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "显示颜色";
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(34, 41);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(73, 19);
            this.radioButton5.TabIndex = 7;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "双向路";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(155, 87);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 15);
            this.label8.TabIndex = 4;
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 394);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbRatio);
            this.Name = "Setting";
            this.Text = "Setting";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbXMap;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbYMap;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColorDialog cdShow;
        private System.Windows.Forms.Label lbPick;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbRatio;
        private System.Windows.Forms.RadioButton rbMap;
        private System.Windows.Forms.RadioButton rbPick;
        private System.Windows.Forms.RadioButton rbCharge;
        private System.Windows.Forms.RadioButton rbDevice;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbShelf;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.Label label8;
    }
}