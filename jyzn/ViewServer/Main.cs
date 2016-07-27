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
        //菜单高度28，10像素边界留白
        private const int MARGIN_UP = 38, MARGIN_LEFT = 10;
        private StoreInfo store;
        private Setting setWindow = null;
        private AddPoint addPointWindow = null;

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
                Points p = new Points(node, store);
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

        private void setToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (setWindow == null)
                setWindow = new Setting();

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

        /// <summary>
        /// 动态添加节点
        /// </summary>
        /// <param name="data"></param>
        private void RealtimeAddPoint(int data)
        {
            bool exists =false;
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
            
            StorePoints point= store.GetPointInfo(data);
            HeadNode nodeItem = new HeadNode();
            nodeItem.Data = data;
            nodeItem.Name = point.Name;
            nodeItem.NodeType = (StoreComponentType)point.Type;
            nodeItem.Location = Core.Distance.DecodeStringInfo(point.Point);
            nodeItem.Location = Models.Graph.MapConvert(nodeItem.Location);
            nodeItem.Location.XPos += MARGIN_LEFT;
            nodeItem.Location.YPos += MARGIN_UP;
            
            Points p = new Points(nodeItem, store);
            this.Controls.Add(p);
        }
    }
}