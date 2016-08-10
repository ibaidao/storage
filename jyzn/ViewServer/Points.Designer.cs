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
            this.startPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.setCharge = new System.Windows.Forms.ToolStripMenuItem();
            this.setPickStation = new System.Windows.Forms.ToolStripMenuItem();
            this.setRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.lbTmp = new System.Windows.Forms.Label();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closePoint,
            this.startPoint,
            this.setCharge,
            this.setPickStation,
            this.setRestore});
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
            // startPoint
            // 
            this.startPoint.Name = "startPoint";
            this.startPoint.Size = new System.Drawing.Size(168, 24);
            this.startPoint.Text = "启用当前节点";
            this.startPoint.Visible = false;
            // 
            // setCharge
            // 
            this.setCharge.Name = "setCharge";
            this.setCharge.Size = new System.Drawing.Size(168, 24);
            this.setCharge.Text = "设为充电桩";
            // 
            // setPickStation
            // 
            this.setPickStation.Name = "setPickStation";
            this.setPickStation.Size = new System.Drawing.Size(168, 24);
            this.setPickStation.Text = "设为拣货台";
            // 
            // setRestore
            // 
            this.setRestore.Name = "setRestore";
            this.setRestore.Size = new System.Drawing.Size(168, 24);
            this.setRestore.Text = "设为补货台";
            // 
            // lbTmp
            // 
            this.lbTmp.AutoSize = true;
            this.lbTmp.Location = new System.Drawing.Point(4, 4);
            this.lbTmp.Name = "lbTmp";
            this.lbTmp.Size = new System.Drawing.Size(15, 15);
            this.lbTmp.TabIndex = 1;
            this.lbTmp.Text = "A";
            // 
            // Points
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenu;
            this.Controls.Add(this.lbTmp);
            this.Name = "Points";
            this.Size = new System.Drawing.Size(45, 39);
            this.Load += new System.EventHandler(this.Points_Load);
            this.Click += new System.EventHandler(this.Points_Click);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem setCharge;
        private System.Windows.Forms.ToolStripMenuItem setPickStation;
        private System.Windows.Forms.ToolStripMenuItem setRestore;
        private System.Windows.Forms.ToolStripMenuItem closePoint;
        private System.Windows.Forms.ToolStripMenuItem startPoint;
        private System.Windows.Forms.Label lbTmp;
    }
}
