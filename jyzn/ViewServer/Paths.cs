using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewServer
{
    public partial class Paths : UserControl
    {
        private const int LENGTH_DIVIDE = 8, WIDTH_DIVIDE = 2, ARROW_CUT_DIVIDE = 4, ARROW_ADD_DIVIDE = 2;
        private int lineWidth;
        private Color COLOR_BOTH = Color.DarkGray, COLOR_SINGLE = Color.LightGray;
        private Models.StoreComponentType direct;
        private Core.Location startLocation, endLocation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathType">路线类型</param>
        /// <param name="start">线左上角坐标</param>
        /// <param name="end">右下角坐标</param>
        public Paths(Models.StoreComponentType pathType, int width, Core.Location start, Core.Location end)
        {
            this.direct = pathType;
            this.lineWidth = width;
            this.startLocation = start;
            this.endLocation = end;

            InitializeComponent();

            ShowLine();

            FillProperties();
        }

        /// <summary>
        /// 画线
        /// </summary>
        private void ShowLine()
        {


            if (startLocation.XPos < endLocation.XPos || startLocation.YPos < endLocation.YPos)
            {
                this.Location = new Point(startLocation.XPos, startLocation.YPos);
            }
            else
            {
                this.Location = new Point(endLocation.XPos, endLocation.YPos);
            }

            //任选一点（起点或终点）改变坐标
            if (endLocation.YPos == startLocation.YPos) endLocation.YPos += this.lineWidth;
            else if (endLocation.XPos == startLocation.XPos) endLocation.XPos += this.lineWidth;

            this.Size = new Size(Math.Abs(startLocation.XPos - endLocation.XPos), Math.Abs(startLocation.YPos - endLocation.YPos));
        }

        /// <summary>
        /// 添加线属性
        /// </summary>
        private void FillProperties()
        {
            switch (direct)
            {
                case Models.StoreComponentType.BothPath:
                    this.BackColor = COLOR_BOTH;
                    break;

                case Models.StoreComponentType.OneWayPath:
                    this.BackColor = COLOR_SINGLE;
                    this.AddArrow();
                    break;

                default: break;
            }
        }

        /// <summary>
        /// 单线路箭头
        /// </summary>
        private void AddArrow()
        {
            int xDivide = Math.Abs(startLocation.XPos-endLocation.XPos)>Math.Abs(startLocation.YPos-endLocation.YPos)?LENGTH_DIVIDE:WIDTH_DIVIDE;
            int yDivide = xDivide==LENGTH_DIVIDE?WIDTH_DIVIDE:LENGTH_DIVIDE;
            Point start = new Point(Math.Min(startLocation.XPos, endLocation.XPos) + Math.Abs(startLocation.XPos - endLocation.XPos) / xDivide,
                        Math.Min(startLocation.YPos, endLocation.YPos) + Math.Abs(startLocation.YPos - endLocation.YPos) / yDivide);
            Point end = new Point(Math.Max(startLocation.XPos, endLocation.XPos) - Math.Abs(startLocation.XPos - endLocation.XPos) / xDivide,
                        Math.Max(startLocation.YPos, endLocation.YPos) - Math.Abs(startLocation.YPos - endLocation.YPos) / yDivide);
            //跟起点比，（X,Y）都增加
            Point arrowOneDirect = new Point(Math.Min(startLocation.XPos, endLocation.XPos) + Math.Abs(startLocation.XPos - endLocation.XPos) / ARROW_ADD_DIVIDE,
                        Math.Min(startLocation.YPos, endLocation.YPos) + Math.Abs(startLocation.YPos - endLocation.YPos) / ARROW_ADD_DIVIDE);
            //跟终点比，（X,Y）都减小
            Point arrowTwoDirect = new Point(Math.Max(startLocation.XPos, endLocation.XPos) - Math.Abs(startLocation.XPos - endLocation.XPos) / ARROW_CUT_DIVIDE,
                        Math.Max(startLocation.YPos, endLocation.YPos) - Math.Abs(startLocation.YPos - endLocation.YPos) / ARROW_CUT_DIVIDE);

            Pen pen = new Pen(Color.LightGreen, 5);
            Graphics g = this.CreateGraphics();
            g.DrawLine(pen, start, end);
            g.DrawLine(pen, end, arrowOneDirect);
            g.DrawLine(pen, end, arrowTwoDirect);
            g.Dispose();
        }
    }
}
