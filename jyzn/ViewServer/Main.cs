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
        Setting setWindow = null;
        Graph graph = null;
        public Main()
        {
            InitializeComponent();

            StoreInfo store = new StoreInfo();
            graph = store.GraphInfo;
            //仓库
            this.BackColor = Color.FromArgb(graph.ColorStoreBack);
            this.Size = new Size(graph.SizeGraph.XPos, graph.SizeGraph.YPos);
            //缩放比例设置
            for (int i = 0; i < graph.NodeList.Count; i++)
            {
                Core.Location loc = graph.MapConvert(graph.NodeList[i].Location);
                loc.XPos += MARGIN_LEFT; loc.YPos += MARGIN_UP;
                HeadNode node = graph.NodeList[i];
                node.Location = loc;
                graph.NodeList[i] = node;
            }
            //节点 + 路线
            List<Paths> pathList = new List<Paths>();
            foreach (HeadNode node in graph.NodeList)
            {
                Points p = new Points(node, store, graph);
                this.Controls.Add(p);
                foreach (Edge edge in node.Edge)
                {
                    Paths paCheck = pathList.Find(item => item.StartData == node.Data && item.EndData == graph.NodeList[edge.Idx].Data ||
                        item.EndData == node.Data && item.StartData == graph.NodeList[edge.Idx].Data);
                    if (paCheck == null)
                    {//判断是为了去重（双向边仅画一次）
                        Paths pa = new Paths(StoreComponentType.OneWayPath, graph.PathWidth, node, graph.NodeList[edge.Idx]);
                        pathList.Add(pa);
                    }
                    else
                    {//双向路
                       // paCheck.PathType = StoreComponentType.BothPath;
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
                setWindow = new Setting(graph);

            setWindow.ShowDialog();
        }
    }
}