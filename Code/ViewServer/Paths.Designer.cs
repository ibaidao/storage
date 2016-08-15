namespace ViewServer
{
    partial class Paths
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
            this.components = new System.ComponentModel.Container();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.StopPath = new System.Windows.Forms.ToolStripMenuItem();
            this.StartPath = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StopPath,
            this.StartPath});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(169, 52);
            this.contextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenu_ItemClicked);
            // 
            // StopPath
            // 
            this.StopPath.Name = "StopPath";
            this.StopPath.Size = new System.Drawing.Size(168, 24);
            this.StopPath.Text = "关闭当前路段";
            // 
            // StartPath
            // 
            this.StartPath.Name = "StartPath";
            this.StartPath.Size = new System.Drawing.Size(168, 24);
            this.StartPath.Text = "启用当前路段";
            this.StartPath.Visible = false;
            // 
            // Paths
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenu;
            this.Name = "Paths";
            this.Size = new System.Drawing.Size(100, 10);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Paths_Paint);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem StopPath;
        private System.Windows.Forms.ToolStripMenuItem StartPath;
    }
}
