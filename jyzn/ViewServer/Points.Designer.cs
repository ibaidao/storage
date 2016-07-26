namespace ViewServer
{
    partial class Points
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
            this.closePoint = new System.Windows.Forms.ToolStripMenuItem();
            this.addCharge = new System.Windows.Forms.ToolStripMenuItem();
            this.addPickStation = new System.Windows.Forms.ToolStripMenuItem();
            this.addRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.startPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closePoint,
            this.startPoint,
            this.addCharge,
            this.addPickStation,
            this.addRestore});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(169, 124);
            this.contextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenu_ItemClicked);
            // 
            // closePoint
            // 
            this.closePoint.Name = "closePoint";
            this.closePoint.Size = new System.Drawing.Size(168, 24);
            this.closePoint.Text = "关闭当前节点";
            // 
            // addCharge
            // 
            this.addCharge.Name = "addCharge";
            this.addCharge.Size = new System.Drawing.Size(168, 24);
            this.addCharge.Text = "添加充电桩";
            // 
            // addPickStation
            // 
            this.addPickStation.Name = "addPickStation";
            this.addPickStation.Size = new System.Drawing.Size(168, 24);
            this.addPickStation.Text = "添加拣货台";
            // 
            // addRestore
            // 
            this.addRestore.Name = "addRestore";
            this.addRestore.Size = new System.Drawing.Size(168, 24);
            this.addRestore.Text = "增加补货台";
            // 
            // startPoint
            // 
            this.startPoint.Name = "startPoint";
            this.startPoint.Size = new System.Drawing.Size(168, 24);
            this.startPoint.Text = "启用当前节点";
            // 
            // Points
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenu;
            this.Name = "Points";
            this.Size = new System.Drawing.Size(10, 10);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem addCharge;
        private System.Windows.Forms.ToolStripMenuItem addPickStation;
        private System.Windows.Forms.ToolStripMenuItem addRestore;
        private System.Windows.Forms.ToolStripMenuItem closePoint;
        private System.Windows.Forms.ToolStripMenuItem startPoint;
    }
}
