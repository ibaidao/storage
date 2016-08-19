namespace ViewServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuTop = new System.Windows.Forms.MenuStrip();
            this.setToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlExample = new System.Windows.Forms.Panel();
            this.orderListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuTop
            // 
            this.menuTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setToolStripMenuItem,
            this.addToolStripMenuItem,
            this.newOrderToolStripMenuItem,
            this.orderListToolStripMenuItem});
            this.menuTop.Location = new System.Drawing.Point(0, 0);
            this.menuTop.Name = "menuTop";
            this.menuTop.Size = new System.Drawing.Size(802, 28);
            this.menuTop.TabIndex = 0;
            this.menuTop.Text = "menuStrip1";
            // 
            // setToolStripMenuItem
            // 
            this.setToolStripMenuItem.Name = "setToolStripMenuItem";
            this.setToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.setToolStripMenuItem.Text = "设置";
            this.setToolStripMenuItem.Click += new System.EventHandler(this.setToolStripMenuItem_Click);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNodeToolStripMenuItem,
            this.addPathToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.addToolStripMenuItem.Text = "添加";
            // 
            // addNodeToolStripMenuItem
            // 
            this.addNodeToolStripMenuItem.Name = "addNodeToolStripMenuItem";
            this.addNodeToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.addNodeToolStripMenuItem.Text = "添加节点";
            this.addNodeToolStripMenuItem.Click += new System.EventHandler(this.addNodeToolStripMenuItem_Click);
            // 
            // addPathToolStripMenuItem
            // 
            this.addPathToolStripMenuItem.Name = "addPathToolStripMenuItem";
            this.addPathToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.addPathToolStripMenuItem.Text = "添加路径";
            this.addPathToolStripMenuItem.Click += new System.EventHandler(this.addPathToolStripMenuItem_Click);
            // 
            // newOrderToolStripMenuItem
            // 
            this.newOrderToolStripMenuItem.Name = "newOrderToolStripMenuItem";
            this.newOrderToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.newOrderToolStripMenuItem.Text = "下单";
            this.newOrderToolStripMenuItem.Click += new System.EventHandler(this.newOrderToolStripMenuItem_Click);
            // 
            // pnlExample
            // 
            this.pnlExample.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlExample.Location = new System.Drawing.Point(634, 28);
            this.pnlExample.Name = "pnlExample";
            this.pnlExample.Size = new System.Drawing.Size(168, 653);
            this.pnlExample.TabIndex = 1;
            // 
            // orderListToolStripMenuItem
            // 
            this.orderListToolStripMenuItem.Name = "orderListToolStripMenuItem";
            this.orderListToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.orderListToolStripMenuItem.Text = "当前订单";
            this.orderListToolStripMenuItem.Click += new System.EventHandler(this.orderListToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 681);
            this.Controls.Add(this.pnlExample);
            this.Controls.Add(this.menuTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuTop;
            this.Name = "Main";
            this.Text = "总系统";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuTop.ResumeLayout(false);
            this.menuTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuTop;
        private System.Windows.Forms.ToolStripMenuItem setToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newOrderToolStripMenuItem;
        private System.Windows.Forms.Panel pnlExample;
        private System.Windows.Forms.ToolStripMenuItem orderListToolStripMenuItem;

    }
}

