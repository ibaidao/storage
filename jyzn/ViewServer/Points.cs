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
        private Controller.StoreInfo viewControl;
        private Models.Graph viewGraph;
        private int locData;
        private Color pointColor;

        public Points(Models.HeadNode node,Controller.StoreInfo control, Models.Graph mapInfo)
        {
            InitializeComponent();

            this.viewControl = control;
            this.viewGraph = mapInfo;
            this.locData = node.Data;
            //左上角坐标
            this.Location=new Point(node.Location.XPos, node.Location.YPos);
            //正方形
            this.UpdatePointStyle(node.NodeType);
            this.BackColor = this.pointColor;
        }

        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "closePoint":
                    this.pointColor = Color.Red;
                    contextMenu.Items["startPoint"].Visible = true;
                    contextMenu.Items["closePoint"].Visible = false;
                    break;
                case "startPoint":
                    contextMenu.Items["startPoint"].Visible = false;
                    contextMenu.Items["closePoint"].Visible = true;
                    this.UpdatePointStyle(Models.StoreComponentType.CrossCorner);
                    break;
                case "setCharge":
                    this.viewControl.AddChargerPickStation(Models.StoreComponentType.Charger, "Charge01", this.locData);
                    this.viewControl.ChangePoint(locData, Models.StoreComponentType.Charger);
                    this.UpdatePointStyle(Models.StoreComponentType.Charger);
                    break;
                case "setPickStation":
                    this.viewControl.AddChargerPickStation(Models.StoreComponentType.PickStation, "Charge01", this.locData);
                    this.viewControl.ChangePoint(locData, Models.StoreComponentType.PickStation);
                    this.UpdatePointStyle(Models.StoreComponentType.PickStation);
                    break;
                case "setRestore":
                    this.viewControl.AddChargerPickStation(Models.StoreComponentType.RestoreStation, "Charge01", this.locData);
                    this.viewControl.ChangePoint(locData, Models.StoreComponentType.RestoreStation);
                    this.UpdatePointStyle(Models.StoreComponentType.RestoreStation);
                    break;
                default: break;
            }
            this.BackColor = this.pointColor;
        }

        /// <summary>
        /// 更新节点显示样式
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="color"></param>
        private void UpdatePointStyle(Models.StoreComponentType nodeType)
        {
            switch (nodeType)
            {
                case Models.StoreComponentType.CrossCorner://交叉路口
                    this.Size = new Size(viewGraph.PathWidth, viewGraph.PathWidth);
                    this.pointColor = Color.FromArgb(viewGraph.ColorCrossing);
                    break;
                case Models.StoreComponentType.Charger://充电桩
                    this.Size = new Size(viewGraph.SizeCharger.XPos, viewGraph.SizeCharger.YPos);
                    this.pointColor = Color.FromArgb(viewGraph.ColorCharger);
                    break;
                case Models.StoreComponentType.PickStation://拣货台
                    this.Size = new Size(viewGraph.SizePickStation.XPos, viewGraph.SizePickStation.YPos);
                    this.pointColor = Color.FromArgb(viewGraph.ColorPickStation);
                    break;
                case Models.StoreComponentType.RestoreStation://补货台
                    this.Size = new Size(viewGraph.SizeRestore.XPos, viewGraph.SizeRestore.YPos);
                    this.pointColor = Color.FromArgb(viewGraph.ColorRestore);
                    break;

                default: break;
            }
        }
    }
}
