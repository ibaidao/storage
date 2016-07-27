using System;
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
        //菜单高度28，10像素边界留白
        private const int MARGIN_UP = 38, MARGIN_LEFT = 10;
        private StoreInfo store;
        private Setting setWindow = null;
        private AddPoint addPointWindow = null;
        private AddPath addPathWindow = null;
        private Models.HeadNode lastPointForNewLine;

        public Main()
        {
            InitializeComponent();

            store = new StoreInfo();
            Graph graph = store.GraphInfo;
            //仓库
            this.BackColor = Color.FromArgb(Models.Graph.ColorStoreBack);
            this.Size = new Size(Models.Graph.SizeGraph.XPos, Models.Graph.SizeGraph.YPos);
            //缩放比例设置
            for (int i = 0; i < graph.NodeList.Count; i++)
            {
                Core.Location loc = Models.Graph.MapConvert(graph.NodeList[i].Location);
                loc.XPos += MARGIN_LEFT; loc.YPos += MARGIN_UP;
                HeadNode node = graph.NodeList[i];
                node.Location = loc;
                graph.NodeList[i] = node;
            }
            //节点 + 路线
            List<Paths> pathList = new List<Paths>();
            foreach (HeadNode node in graph.NodeList)
            {
                Points p = new Points(node, store, this.RealtimeCollectPoints);
                this.Controls.Add(p);
                foreach (Edge edge in node.Edge)
                {
                    Paths paCheck = pathList.Find(item => item.StartData == node.Data && item.EndData == graph.NodeList[edge.Idx].Data ||
                        item.EndData == node.Data && item.StartData == graph.NodeList[edge.Idx].Data);
                    if (paCheck == null)
                    {//判断是为了去重（双向边仅画一次）
                        Paths pa = new Paths(StoreComponentType.OneWayPath, node, graph.NodeList[edge.Idx]);
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
        }

        #region 界面操作事件

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

        #endregion

        #region 内部调用事件

        /// <summary>
        /// 动态添加节点
        /// </summary>
        /// <param name="data"></param>
        private void RealtimeAddPoint(int data)
        {
            bool exists = false;
            //先检测是否已存在
            Graph graph = store.GraphInfo;
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

            StorePoints point = store.GetPointInfo(data);
            HeadNode nodeItem = new HeadNode();
            nodeItem.Data = data;
            nodeItem.Name = point.Name;
            nodeItem.NodeType = (StoreComponentType)point.Type;
            nodeItem.Location = Core.Distance.DecodeStringInfo(point.Point);
            nodeItem.Location = Models.Graph.MapConvert(nodeItem.Location);
            nodeItem.Location.XPos += MARGIN_LEFT;
            nodeItem.Location.YPos += MARGIN_UP;

            Points p = new Points(nodeItem, store, this.RealtimeCollectPoints);
            this.Controls.Add(p);
        }

        /// <summary>
        /// 动态搜集路线上的节点
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
                MessageBox.Show(Core.ErrorDescription.PathWithinOneAxis);
                return;
            }
            //检测原有边是否已包含新加入边
            foreach (Edge edge in lastPointForNewLine.Edge)
            {
                if (store.GraphInfo.NodeList[edge.Idx].Data == nodeData.Data)
                {
                    MessageBox.Show(Core.ErrorDescription.PathAlreadyExists);
                    return;
                }
            }
            //写入数据库
            store.AddPath(lastPointForNewLine.Data, nodeData.Data, addPathWindow.PathType);
            //更新主界面显示
            Paths path = new Paths(addPathWindow.PathType, lastPointForNewLine, nodeData);
            path.ShowLine();
            this.Controls.Add(path);
        }

        /// <summary>
        /// 动态添加路径
        /// </summary>
        private void RealtimeAddPath()
        {
            AddPathFlag = false;
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
                    this.Size = new Size(Graph.MapConvert(configInfo.Width), Graph.MapConvert(configInfo.Length));
                    break;

                default: break;
            }
        }

        #endregion
    }
}