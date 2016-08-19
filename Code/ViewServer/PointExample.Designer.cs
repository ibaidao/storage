namespace ViewServer
{
    partial class PointExample
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
            this.pnlShape = new System.Windows.Forms.Panel();
            this.lbWord = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pnlShape
            // 
            this.pnlShape.Location = new System.Drawing.Point(3, 3);
            this.pnlShape.Name = "pnlShape";
            this.pnlShape.Size = new System.Drawing.Size(63, 69);
            this.pnlShape.TabIndex = 0;
            // 
            // lbWord
            // 
            this.lbWord.AutoSize = true;
            this.lbWord.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbWord.Location = new System.Drawing.Point(6, 78);
            this.lbWord.Name = "lbWord";
            this.lbWord.Size = new System.Drawing.Size(19, 19);
            this.lbWord.TabIndex = 1;
            this.lbWord.Text = "d";
            // 
            // PointExample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbWord);
            this.Controls.Add(this.pnlShape);
            this.Name = "PointExample";
            this.Size = new System.Drawing.Size(72, 110);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlShape;
        private System.Windows.Forms.Label lbWord;
    }
}
