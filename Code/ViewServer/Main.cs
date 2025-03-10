﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller;
using Models;

namespace ViewServer
{
    public partial class Main : Form
    {
        private bool AddPathFlag = false;
        private StoreMap store;
        private Setting setWindow = null;
        private AddPoint addPointWindow = null;
        private AddPath addPathWindow = null;
        private InfoProcess msgHandler = null; 
        private Models.HeadNode lastPointForNewLine;        

        public Main()
        {
            InitializeComponent();

            store = new StoreMap();
            msgHandler = new InfoProcess(ShowMessageError, UpdateComponentLocation, UpdateComponentColor);
            //仓库
            this.BackColor = Color.FromArgb(Graph.ColorStoreBack);
            this.Size = new Size(Graph.SizeGraph.XPos, Graph.SizeGraph.YPos);
            //设备
            List<Models.Devices> deviceList = store.RealtimeDevice;
            foreach (Models.Devices device in deviceList)
            {
                Devices viewDevice = new Devices(device, 0);
                this.Controls.Add(viewDevice);
            }
            //节点 + 路线
            List<Paths> pathList = new List<Paths>();
            List<HeadNode> nodeList = store.RealtimeNodeList;
            foreach (HeadNode node in nodeList)
            {
                Points p = new Points(node, store, this.RealtimeCollectPoints);
                this.Controls.Add(p);
                foreach (Edge edge in node.Edge)
                {
                    Paths paCheck = pathList.Find(item => item.StartData == node.Data && item.EndData == nodeList[edge.Idx].Data ||
                        item.EndData == node.Data && item.StartData == nodeList[edge.Idx].Data);
                    if (paCheck == null)
                    {//判断是为了去重（双向边仅画一次）
                        Paths pa = new Paths(store, StoreComponentType.OneWayPath, node, nodeList[edge.Idx],edge.Status);
                        pathList.Add(pa);
                    }
                    else
                    {//双向路
                        paCheck.PathType = StoreComponentType.BothPath;
                    }
                }
            }
            foreach (Paths path in pathList)
            {
                path.ShowLine();
                this.Controls.Add(path);
            }
            //示例模块
            this.InitialExamplePanels();
        }

        #region 界面操作 用户交互事件

        private void Main_Load(object sender, EventArgs e)
        {
            if (!Models.Graph.MapSettingShowFlag)
            {
                this.menuTop.Visible = false;
            }
            StoreMap.StartListenClient();
        }

        private void setToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (setWindow == null)
                setWindow = new Setting(this.RealtimeChangeStoreInfo);

            setWindow.ShowDialog();
        }

        private void addNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (addPointWindow == null)
            {
                addPointWindow = new AddPoint(store, this.RealtimeAddPoint);
            }

            addPointWindow.ShowDialog();
        }

        private void addPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //此处可以打开多个窗口，可以通过单例模式控制始终只有一个活动窗体
            addPathWindow = new AddPath(this.RealtimeAddPath);
            addPathWindow.Show();

            AddPathFlag = true;
        }

        private void currentOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderList orderWindow = new OrderList();
            orderWindow.Show();
        }

        private void resetOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool freePickStation = true;
            Control[] items = this.Controls.Find("Points", true);
            foreach (Control item in items)
            {
                Points point = item as Points;
                if (point.NodeType != StoreComponentType.PickStation) continue;
                if (point.BackColor != Color.FromArgb(Graph.ColorPickStationClosed))
                {
                    freePickStation = false;
                    break;
                }
            }

            if (freePickStation)
            {
                Controller.Orders oder = new Controller.Orders();
                oder.InitialOldOrder();
                MessageBox.Show("成功：所有订单变为未拣货");
            }
            else
            {
                MessageBox.Show("复位失败：有拣货台在工作");
            }


        }

        private void newOrderToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Orders orderWindow = new Orders(msgHandler.NewOrdersComing);
            orderWindow.Show();
        }

        #endregion

        #region 内部调用 改变窗体显示

        /// <summary>
        /// 动态添加节点
        /// </summary>
        /// <param name="data"></param>
        private void RealtimeAddPoint(int data)
        {
            bool exists = false;
            //先检测是否已存在
            foreach (Control item in this.Controls)
            {
                Type itemType = item.GetType();
                if (itemType.Namespace == this.GetType().Namespace && itemType.Name == "Points"
                    && (item as Points).NodeData == data)
                {
                    exists = true;
                    break;
                }
            }

            if (exists) return;

            HeadNode nodeItem = store.GetMapNodeByData(data);
            Points p = new Points(nodeItem, store, this.RealtimeCollectPoints);

            this.Controls.Add(p);
        }

        /// <summary>
        /// 动态搜集路线上的节点(用于实时新增路线)
        /// </summary>
        /// <param name="data"></param>
        private void RealtimeCollectPoints(Models.HeadNode nodeData)
        {
            //普通点击，不是连线
            if (!AddPathFlag) return;
            //首次点击，作为线段的起点
            if (lastPointForNewLine.Data == 0)
            {
                lastPointForNewLine = nodeData;
                return;
            }
            //检测是否发生斜对角连线
            if (lastPointForNewLine.Location.XPos != nodeData.Location.XPos && lastPointForNewLine.Location.YPos != nodeData.Location.YPos)
            {
                MessageBox.Show(Models.ErrorDescription.PATH_WITHIN_ONE_AXIS);
                return;
            }
            //检测原有边是否已包含新加入边
            List<HeadNode> nodeList = store.RealtimeNodeList;
            foreach (Edge edge in lastPointForNewLine.Edge)
            {
                if (nodeList[edge.Idx].Data == nodeData.Data)
                {
                    MessageBox.Show(Models.ErrorDescription.PATH_ALREADY_EXISTS);
                    return;
                }
            }
            //写入数据库
            ErrorCode addResult = store.RealtimeAddPath(lastPointForNewLine.Data, nodeData.Data, addPathWindow.PathType);
            if (addResult != ErrorCode.OK)
            {
                MessageBox.Show(ErrorDescription.ExplainCode(addResult));
                return;
            }
            //更新主界面显示
            Paths path = new Paths(store, addPathWindow.PathType, lastPointForNewLine, nodeData, true);
            path.ShowLine();
            this.Controls.Add(path);
            lastPointForNewLine = nodeData;
        }

        /// <summary>
        /// 动态添加路径
        /// </summary>
        private void RealtimeAddPath()
        {
            AddPathFlag = false;
            lastPointForNewLine.Data = 0;
        }

        /// <summary>
        /// 动态改变仓库状态信息
        /// </summary>
        /// <param name="ratio"></param>
        private void RealtimeChangeStoreInfo(Models.StoreComponentType storeType, Models.GraphConfig configInfo)
        {
            switch (storeType)
            {
                case StoreComponentType.StoreRatio://更新比例尺涉及东西比较多，先不允许动态修改
                    //更新配置文件
                    store.UpdateIniFile("RatioMapZoom", (configInfo.ColorIndex / 1000.0).ToString());
                    //更新窗体显示
                    break;
                case StoreComponentType.StoreSelf:
                    //更新配置文件
                    store.UpdateIniFile("SizeMap", string.Format("{0},{1}", configInfo.Length, configInfo.Width));
                    store.UpdateIniFile("ColorStoreBack", configInfo.ColorIndex.ToString());
                    //更新窗体显示
                    this.BackColor = Color.FromArgb(configInfo.ColorIndex);
                    this.Size = new Size(StoreMap.ExchangeMapRatio(configInfo.Width), StoreMap.ExchangeMapRatio(configInfo.Length));
                    break;

                default: break;
            }
        }

        /// <summary>
        /// 更新元素显示位置
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="itemID"></param>
        /// <param name="itemLoc"></param>
        private void UpdateComponentLocation(StoreComponentType itemType, int itemID, Location itemLoc)
        {
            if (this.InvokeRequired)
            {
                Action<StoreComponentType, int, Location> updateLocation = new Action<StoreComponentType, int, Location>(UpdateComponentLocation);
                this.Invoke(updateLocation, new object[] {itemType,itemID,itemLoc });
                return;
            }
            if (itemType == StoreComponentType.Devices || itemType == StoreComponentType.ShelfDevice)
            {
                Control[] items = this.Controls.Find("Devices", true);
                foreach (Control item in items)
                {
                    Devices device = item as Devices;
                    if (device.DeviceID != itemID) continue;
                    device.DeviceLocation = itemLoc;
                }
            }
            else
            {
                ShowMessageError(ErrorCode.CannotFindByID);
            }

        }

        /// <summary>
        /// 更新元素的显示颜色
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="itemID"></param>
        /// <param name="itemParam"></param>
        private void UpdateComponentColor(StoreComponentType itemType, int itemID, int itemParam)
        {
            if (this.InvokeRequired)
            {
                Action<StoreComponentType, int, int> updateColor = new Action<StoreComponentType, int, int>(UpdateComponentColor);
                this.Invoke(updateColor, new object[] { itemType, itemID, itemParam });
                return;
            }
            
            if (itemType == StoreComponentType.Devices || itemType == StoreComponentType.ShelfDevice)
            {//设备，设备+货架
                Control[] items= this.Controls.Find("Devices", true);
                foreach (Control item in items)
                {
                    Devices device = item as Devices;
                    if (device.DeviceID != itemID) continue;
                    device.ShelfID = itemParam;
                    break;
                }
            }
            else if (itemType == StoreComponentType.PickStation || itemType == StoreComponentType.RestoreStation || itemType == StoreComponentType.Shelf || itemType == StoreComponentType.Charger)
            {//节点，拣货台，补货台,货架，充电桩
                Control[] items = this.Controls.Find("Points", true);
                foreach (Control item in items)
                {
                    Points point = item as Points;
                    if (point.NodeData != itemID) continue;
                    StoreComponentStatus status = itemParam > 0?StoreComponentStatus.Working:itemParam==0? StoreComponentStatus.OK: StoreComponentStatus.Trouble;
                    point.UpdatePointShow(itemType, status);
                    break;
                }
            }
            else
            {
                ShowMessageError(ErrorCode.CannotFindByID);
            }
        }

        /// <summary>
        /// 弹窗显示错误/警告信息
        /// </summary>
        /// <param name="code"></param>
        private void ShowMessageError(ErrorCode code)
        {
            MessageBox.Show(Models.ErrorDescription.ExplainCode(code));
        }

        /// <summary>
        /// 整理示例模块
        /// </summary>
        private void InitialExamplePanels()
        {
            int width = 80, height = 120,marginTop = 20,itemIdx = 1;
            List<PointExample> allExamples = new List<PointExample>();

            Location pathWidth = new Models.Location(Graph.PathWidth, Graph.PathWidth, 0);
            Font labelFont = new System.Drawing.Font("黑体", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134))); 
            Label lbTypeName = new Label();
            lbTypeName.Font = labelFont;
            lbTypeName.Text = "货架";
            lbTypeName.Location = new Point(0, height * itemIdx + marginTop);
            lbTypeName.Width = width;
            this.pnlExample.Controls.Add(lbTypeName);
            PointExample example = new PointExample(1,1, Graph.ColorShelf, Graph.SizeShelf, "");
            allExamples.Add(example);

            itemIdx = 2;
            lbTypeName = new Label();
            lbTypeName.Font = labelFont;
            lbTypeName.Text = "小车";
            lbTypeName.Location = new Point(0, height * itemIdx + marginTop);
            lbTypeName.Width = width;
            this.pnlExample.Controls.Add(lbTypeName);
            example = new PointExample(itemIdx, 1, Graph.ColorDevice, Graph.SizeDevice, "空车");
            allExamples.Add(example);
            example = new PointExample(itemIdx, 2, Graph.ColorDeviceShelf, Graph.SizeDevice, "带货架");
            allExamples.Add(example);

            itemIdx = 3;
            lbTypeName = new Label();
            lbTypeName.Font = labelFont;
            lbTypeName.Text = "拣货台";
            lbTypeName.Location = new Point(0, height * itemIdx + marginTop);
            lbTypeName.Width = width;
            this.pnlExample.Controls.Add(lbTypeName);
            example = new PointExample(itemIdx, 1, Graph.ColorPickStationClosed, Graph.SizePickStation, "未启用");
            allExamples.Add(example);
            example = new PointExample(itemIdx, 2, Graph.ColorPickStation, Graph.SizePickStation, "空闲");
            allExamples.Add(example);
            example = new PointExample(itemIdx, 3, Graph.ColorPickStationWorking, Graph.SizePickStation, "工作");
            allExamples.Add(example);

            itemIdx = 4;
            lbTypeName = new Label();
            lbTypeName.Font = labelFont;
            lbTypeName.Text = "充电桩";
            lbTypeName.Location = new Point(0, height * itemIdx + marginTop);
            lbTypeName.Width = width;
            this.pnlExample.Controls.Add(lbTypeName);
            example = new PointExample(itemIdx, 1, Graph.ColorCharger, Graph.SizeCharger, "空闲");
            allExamples.Add(example);
            example = new PointExample(itemIdx, 2, Graph.ColorChargerWorking, Graph.SizeCharger, "忙碌");
            allExamples.Add(example);

            itemIdx = 5;
            lbTypeName = new Label();
            lbTypeName.Font = labelFont;
            lbTypeName.Text = "道路";
            lbTypeName.Location = new Point(0, height * itemIdx + marginTop);
            lbTypeName.Width = width;
            this.pnlExample.Controls.Add(lbTypeName);
            example = new PointExample(itemIdx, 1, Graph.ColorBothPath, pathWidth, "双向");
            allExamples.Add(example);
            example = new PointExample(itemIdx, 2, Graph.ColorSinglePath, pathWidth, "单向");
            allExamples.Add(example);

            itemIdx = 6;
            lbTypeName = new Label();
            lbTypeName.Font = labelFont;
            lbTypeName.Text = "补货台";
            lbTypeName.Location = new Point(0, height * itemIdx + marginTop);
            lbTypeName.Width = width;
            this.pnlExample.Controls.Add(lbTypeName);
            example = new PointExample(itemIdx, 1, Graph.ColorRestore, Graph.SizeRestore, "");
            allExamples.Add(example);

            foreach (PointExample exampleWindow in allExamples)
                this.pnlExample.Controls.Add(exampleWindow);
            this.pnlExample.Width = example.Width * 5;
        }
        #endregion
    }
}