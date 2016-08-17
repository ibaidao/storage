namespace ViewServer
{
    partial class Devices
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lbInfo = new System.Windows.Forms.Label();
            this.lbIDs = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Location = new System.Drawing.Point(10, 36);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(0, 15);
            this.lbInfo.TabIndex = 0;
            // 
            // lbIDs
            // 
            this.lbIDs.AutoSize = true;
            this.lbIDs.Location = new System.Drawing.Point(10, 10);
            this.lbIDs.Name = "lbIDs";
            this.lbIDs.Size = new System.Drawing.Size(0, 15);
            this.lbIDs.TabIndex = 1;
            // 
            // Devices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbIDs);
            this.Controls.Add(this.lbInfo);
            this.Name = "Devices";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.Label lbIDs;
    }
}
