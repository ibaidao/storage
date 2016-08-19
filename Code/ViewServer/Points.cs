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
    public partial class Points : UserControl
    {
        private Controller.StoreMap viewControl;
        private int locData;
        private Models.HeadNode NodeItem;
        private Color pointColor;
        private Action<Models.HeadNode> collectPointData = null;

        public Points(Models.HeadNode node, Controller.StoreMap control, Action<Models.HeadNode> clickPoint)
        {
            this.NodeItem = node;
            this.viewControl = control;
            this.collectPointData = clickPoint;

            InitializeComponent();

            this.locData = node.Data;
            //左上角坐标
            Models.Location loc = Controller.StoreMap.ExchangeLocation(node.Location);
            if (node.NodeType == Models.StoreComponentType.PickStation)//拣货台
                this.Location = new Point(loc.XPos, loc.YPos + Models.Graph.PathWidth);
            else
                this.Location = new Point(loc.XPos, loc.YPos);
            //正方形
            this.UpdatePointShow(node.NodeType, node.Status);
            this.lbTmp.Text = node.Data.ToString();

        }

        private void Points_Load(object sender, EventArgs e)
        {
            if (!Models.Graph.MapSettingShowFlag)
            {
                this.setCharge.Visible = false;
                this.setPickStation.Visible = false;
                this.setRestore.Visible = false;
            }
        }

        /// <summary>
        /// 当前节点对应数据
        /// </summary>
        public int NodeData
        {
            get
            {
                return this.locData;
            }
        }

        /// <summary>
        /// 更新节点显示样式
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="status">当前可用状态</param>
        public void UpdatePointShow(Models.StoreComponentType nodeType, Models.StoreComponentStatus status)
        {
            switch (nodeType)
            {
                case Models.StoreComponentType.CrossCorner://交叉路口
                    this.Size = new Size(Models.Graph.PathWidth, Models.Graph.PathWidth);
                    this.pointColor = status == Models.StoreComponentStatus.Working ? Color.FromArgb(Models.Graph.ColorCrossing) : Color.Red;
                    break;
                case Models.StoreComponentType.Shelf://货架
                    this.Size = new Size(Models.Graph.SizeShelf.XPos, Models.Graph.SizeShelf.YPos);
                    this.pointColor = status == Models.StoreComponentStatus.Working ? Color.FromArgb(Models.Graph.ColorShelf) : Color.FromArgb(Models.Graph.ColorBothPath);
                    break;
                case Models.StoreComponentType.Charger://充电桩
                    this.Size = new Size(Models.Graph.SizeCharger.XPos, Models.Graph.SizeCharger.YPos);
                    this.pointColor = Color.FromArgb(Models.Graph.ColorCharger);
                    break;
                case Models.StoreComponentType.PickStation://拣货台
                    this.Size = new Size(Models.Graph.SizePickStation.XPos, Models.Graph.SizePickStation.YPos);
                    switch (status)
                    {
                        case Models.StoreComponentStatus.OK://闲置
                            this.pointColor = Color.FromArgb(Models.Graph.ColorPickStation);
                            break;
                        case Models.StoreComponentStatus.Trouble://未工作
                            this.pointColor = Color.FromArgb(Models.Graph.ColorPickStationClosed);
                            break;
                        case Models.StoreComponentStatus.Working://有订单
                            this.pointColor = Color.FromArgb(Models.Graph.ColorPickStationWorking);
                            break;
                        default: break;
                    }
                    break;
                case Models.StoreComponentType.RestoreStation://补货台
                    this.Size = new Size(Models.Graph.SizeRestore.XPos, Models.Graph.SizeRestore.YPos);
                    this.pointColor = Color.FromArgb(Models.Graph.ColorRestore);
                    break;

                default: break;
            }
            this.BackColor = this.pointColor;
        }

        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "closePoint":
                    contextMenu.Items["startPoint"].Visible = true;
                    contextMenu.Items["closePoint"].Visible = false;
                    this.UpdatePointShow(Models.StoreComponentType.CrossCorner, Models.StoreComponentStatus.Trouble);
                    break;
                case "startPoint":
                    contextMenu.Items["startPoint"].Visible = false;
                    contextMenu.Items["closePoint"].Visible = true;
                    this.UpdatePointShow(Models.StoreComponentType.CrossCorner, Models.StoreComponentStatus.Working);
                    break;
                case "setCharge":
                    this.viewControl.ChangePointType(Models.StoreComponentType.Charger, this.locData, "");
                    this.UpdatePointShow(Models.StoreComponentType.Charger, Models.StoreComponentStatus.Working);
                    break;
                case "setPickStation":
                    this.viewControl.ChangePointType(Models.StoreComponentType.PickStation, this.locData, "");
                    this.UpdatePointShow(Models.StoreComponentType.PickStation, Models.StoreComponentStatus.Trouble);
                    break;
                case "setRestore":
                    this.viewControl.ChangePointType(Models.StoreComponentType.RestoreStation, this.locData);
                    this.UpdatePointShow(Models.StoreComponentType.RestoreStation, Models.StoreComponentStatus.Trouble);
                    break;
                default: break;
            }
        }

        private void Points_Click(object sender, EventArgs e)
        {
            this.collectPointData(this.NodeItem);
        }
    }
}