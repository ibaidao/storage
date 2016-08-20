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
            this.OrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentOrdersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetOrdersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlExample = new System.Windows.Forms.Panel();
            this.menuTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuTop
            // 
            this.menuTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setToolStripMenuItem,
            this.addToolStripMenuItem,
            this.OrderToolStripMenuItem});
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
            // OrderToolStripMenuItem
            // 
            this.OrderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newOrderToolStripMenuItem,
            this.currentOrdersToolStripMenuItem,
            this.resetOrdersToolStripMenuItem});
            this.OrderToolStripMenuItem.Name = "OrderToolStripMenuItem";
            this.OrderToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.OrderToolStripMenuItem.Text = "订单";
            // 
            // newOrderToolStripMenuItem
            // 
            this.newOrderToolStripMenuItem.Name = "newOrderToolStripMenuItem";
            this.newOrderToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.newOrderToolStripMenuItem.Text = "新订单";
            this.newOrderToolStripMenuItem.Click += new System.EventHandler(this.newOrderToolStripMenuItem_Click_1);
            // 
            // currentOrdersToolStripMenuItem
            // 
            this.currentOrdersToolStripMenuItem.Name = "currentOrdersToolStripMenuItem";
            this.currentOrdersToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.currentOrdersToolStripMenuItem.Text = "当前订单";
            this.currentOrdersToolStripMenuItem.Click += new System.EventHandler(this.currentOrdersToolStripMenuItem_Click);
            // 
            // resetOrdersToolStripMenuItem
            // 
            this.resetOrdersToolStripMenuItem.Name = "resetOrdersToolStripMenuItem";
            this.resetOrdersToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.resetOrdersToolStripMenuItem.Text = "订单复位";
            this.resetOrdersToolStripMenuItem.Click += new System.EventHandler(this.resetOrdersToolStripMenuItem_Click);
            // 
            // pnlExample
            // 
            this.pnlExample.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlExample.Location = new System.Drawing.Point(634, 28);
            this.pnlExample.Name = "pnlExample";
            this.pnlExample.Size = new System.Drawing.Size(168, 653);
            this.pnlExample.TabIndex = 1;
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
        private System.Windows.Forms.ToolStripMenuItem OrderToolStripMenuItem;
        private System.Windows.Forms.Panel pnlExample;
        private System.Windows.Forms.ToolStripMenuItem newOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem currentOrdersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetOrdersToolStripMenuItem;

    }
}

