namespace ViewPick
{
    partial class PickStation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PickStation));
            this.pnShelf = new System.Windows.Forms.Panel();
            this.lbName = new System.Windows.Forms.Label();
            this.btnPick = new System.Windows.Forms.Button();
            this.tbProduct = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbShelf = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pnShelf
            // 
            this.pnShelf.Location = new System.Drawing.Point(80, 66);
            this.pnShelf.Name = "pnShelf";
            this.pnShelf.Size = new System.Drawing.Size(358, 457);
            this.pnShelf.TabIndex = 0;
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(492, 140);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(67, 15);
            this.lbName.TabIndex = 1;
            this.lbName.Text = "商品名称";
            // 
            // btnPick
            // 
            this.btnPick.Location = new System.Drawing.Point(494, 488);
            this.btnPick.Name = "btnPick";
            this.btnPick.Size = new System.Drawing.Size(75, 23);
            this.btnPick.TabIndex = 3;
            this.btnPick.Text = "拣货";
            this.btnPick.UseVisualStyleBackColor = true;
            this.btnPick.Click += new System.EventHandler(this.btnPick_Click);
            // 
            // tbProduct
            // 
            this.tbProduct.Location = new System.Drawing.Point(564, 422);
            this.tbProduct.Name = "tbProduct";
            this.tbProduct.Size = new System.Drawing.Size(135, 25);
            this.tbProduct.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(491, 425);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "商品条码";
            // 
            // lbShelf
            // 
            this.lbShelf.AutoSize = true;
            this.lbShelf.Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbShelf.ForeColor = System.Drawing.Color.Red;
            this.lbShelf.Location = new System.Drawing.Point(491, 268);
            this.lbShelf.Name = "lbShelf";
            this.lbShelf.Size = new System.Drawing.Size(263, 22);
            this.lbShelf.TabIndex = 6;
            this.lbShelf.Text = "当前货架，最后一个商品";
            this.lbShelf.Visible = false;
            // 
            // PickStation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 626);
            this.Controls.Add(this.lbShelf);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbProduct);
            this.Controls.Add(this.btnPick);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.pnShelf);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PickStation";
            this.Text = "拣货台";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnShelf;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Button btnPick;
        private System.Windows.Forms.TextBox tbProduct;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbShelf;

    }
}

