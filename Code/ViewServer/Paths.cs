using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models;

namespace ViewServer
{
    public partial class Paths : UserControl
    {
        private const int LENGTH_DIVIDE = 8, WIDTH_DIVIDE = 2, ARROW_DIVIDE = 2;
        private const int ARROW_LINE_WIDTH = 5, ARROW_HEAD_WIDTH = 3;
        private bool useable;
        private Color COLOR_ARROW = Color.Green;
        private StoreComponentType direct;
        private Location startLocation, endLocation;
        private HeadNode startNode, endNode;
        private Controller.StoreMap viewControl;

        /// <summary>
        /// 路径
        /// </summary>
        /// <param name="control">路径控制器</param>
        /// <param name="pathType">路线类型</param>
        /// <param name="start">线左上角坐标</param>
        /// <param name="end">右下角坐标</param>
        /// <param name="status">路径可用状态</param>
        public Paths(Controller.StoreMap control, StoreComponentType pathType, HeadNode start, HeadNode end, bool status)
        {
            this.viewControl = control;
            this.direct = pathType;
            this.startNode = start;
            this.endNode = end;
            this.startLocation = Controller.StoreMap.ExchangeLocation(this.startNode.Location);
            this.endLocation = Controller.StoreMap.ExchangeLocation(this.endNode.Location);
            this.useable = status;

            InitializeComponent();

            if (!this.useable) 
            {
                contextMenu.Items["StartPath"].Visible = true;
                contextMenu.Items["StopPath"].Visible = false;
            }
        }

        /// <summary>
        /// 起点数据
        /// </summary>
        public int StartData
        {
            get { return this.startNode.Data; }
        }

        /// <summary>
        /// 终点数据
        /// </summary>
        public int EndData
        {
            get { return this.endNode.Data; }
        }

        public Models.StoreComponentType PathType
        {
            get { return this.direct; }
            set { this.direct = value; }
        }

        /// <summary>
        /// 线段展现
        /// </summary>
        public void ShowLine()
        {
            //确定左上角的坐标
            if (startLocation.XPos < endLocation.XPos || startLocation.YPos < endLocation.YPos)
            {
                this.Location = new Point(startLocation.XPos, startLocation.YPos);
            }
            else
            {
                this.Location = new Point(endLocation.XPos, endLocation.YPos);
            }
            //任选一点（起点或终点）改变坐标，形成矩形
            if (endLocation.YPos == startLocation.YPos) endLocation.YPos += Models.Graph.PathWidth;
            else if (endLocation.XPos == startLocation.XPos) endLocation.XPos += Models.Graph.PathWidth;
            this.Size = new Size(Math.Abs(startLocation.XPos - endLocation.XPos), Math.Abs(startLocation.YPos - endLocation.YPos));
            //填充矩形，形成实线
            FillProperties();
        }

        /// <summary>
        /// 添加线属性
        /// </summary>
        private void FillProperties()
        {
            if (!useable)
            {//不可用路径
                this.BackColor = Color.FromArgb(Models.Graph.ColorStopPath);
                return;
            }
            
            switch (direct)
            {//单、双向路径
                case Models.StoreComponentType.BothPath:
                    this.BackColor = Color.FromArgb(Models.Graph.ColorBothPath);
                    break;

                case Models.StoreComponentType.OneWayPath:
                    this.BackColor = Color.FromArgb(Models.Graph.ColorSinglePath);
                    break;

                default: break;
            }
        }

        private void Paths_Paint(object sender, PaintEventArgs e)
        {
            if (this.PathType != Models.StoreComponentType.OneWayPath) return;

            //单线路绘制线条
            int xDivide = Math.Abs(startLocation.XPos - endLocation.XPos) > Math.Abs(startLocation.YPos - endLocation.YPos) ? LENGTH_DIVIDE : WIDTH_DIVIDE;
            int yDivide = xDivide == LENGTH_DIVIDE ? WIDTH_DIVIDE : LENGTH_DIVIDE;
            //基于整个面板坐标系下的坐标
            Point start = new Point(Math.Min(startLocation.XPos, endLocation.XPos) + Math.Abs(startLocation.XPos - endLocation.XPos) / xDivide,
                        Math.Min(startLocation.YPos, endLocation.YPos) + Math.Abs(startLocation.YPos - endLocation.YPos) / yDivide);
            Point end = new Point(Math.Max(startLocation.XPos, endLocation.XPos) - Math.Abs(startLocation.XPos - endLocation.XPos) / xDivide,
                        Math.Max(startLocation.YPos, endLocation.YPos) - Math.Abs(startLocation.YPos - endLocation.YPos) / yDivide);
            //平移到当前线条坐标系中显示
            Point startShow = new Point(start.X-startLocation.XPos, start.Y-startLocation.YPos);
            Point endShow = new Point(end.X - startLocation.XPos, end.Y - startLocation.YPos);

            Pen pen = new Pen(COLOR_ARROW, ARROW_LINE_WIDTH);
            System.Drawing.Drawing2D.AdjustableArrowCap lineArrow = new System.Drawing.Drawing2D.AdjustableArrowCap(ARROW_HEAD_WIDTH, ARROW_HEAD_WIDTH, true);
            pen.CustomEndCap = lineArrow;
            this.CreateGraphics().DrawLine(pen, startShow, endShow);
        }

        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "StopPath":
                    this.BackColor = Color.Red;
                    contextMenu.Items["StartPath"].Visible = true;
                    contextMenu.Items["StopPath"].Visible = false;
                    //修改数据
                    this.viewControl.RealtimeStopPath(this.startNode.Data, this.endNode.Data);
                    break;
                case "StartPath":
                    this.BackColor = this.PathType == Models.StoreComponentType.OneWayPath ? Color.FromArgb(Models.Graph.ColorSinglePath) : Color.FromArgb(Models.Graph.ColorBothPath);
                    contextMenu.Items["StopPath"].Visible = true;
                    contextMenu.Items["StartPath"].Visible = false;
                    //修改数据
                    this.viewControl.RealtimeRestartPath(this.startNode.Data, this.endNode.Data);
                    break;
                default: break;
            }
            
        }
    }
}
