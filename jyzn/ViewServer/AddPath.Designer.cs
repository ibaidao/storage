namespace ViewServer
{
    partial class AddPath
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
            this.rbBoth = new System.Windows.Forms.RadioButton();
            this.rbSingle = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tbWeight = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbBoth
            // 
            this.rbBoth.AutoSize = true;
            this.rbBoth.Checked = true;
            this.rbBoth.Location = new System.Drawing.Point(41, 29);
            this.rbBoth.Name = "rbBoth";
            this.rbBoth.Size = new System.Drawing.Size(58, 19);
            this.rbBoth.TabIndex = 1;
            this.rbBoth.TabStop = true;
            this.rbBoth.Text = "双向";
            this.rbBoth.UseVisualStyleBackColor = true;
            // 
            // rbSingle
            // 
            this.rbSingle.AutoSize = true;
            this.rbSingle.Location = new System.Drawing.Point(161, 29);
            this.rbSingle.Name = "rbSingle";
            this.rbSingle.Size = new System.Drawing.Size(58, 19);
            this.rbSingle.TabIndex = 1;
            this.rbSingle.Text = "单向";
            this.rbSingle.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "权重";
            // 
            // tbWeight
            // 
            this.tbWeight.Enabled = false;
            this.tbWeight.Location = new System.Drawing.Point(104, 89);
            this.tbWeight.Name = "tbWeight";
            this.tbWeight.Size = new System.Drawing.Size(100, 25);
            this.tbWeight.TabIndex = 3;
            this.tbWeight.Text = "1";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(79, 146);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(95, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "完成";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // AddPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 204);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.tbWeight);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbSingle);
            this.Controls.Add(this.rbBoth);
            this.Name = "AddPath";
            this.Text = "增加路线";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbBoth;
        private System.Windows.Forms.RadioButton rbSingle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbWeight;
        private System.Windows.Forms.Button btnStart;
    }
}