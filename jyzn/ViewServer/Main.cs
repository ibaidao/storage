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
        public Main()
        {
            InitializeComponent();

            StoreInit store = new StoreInit();
            Graph graph = store.GraphInfo;
            //仓库
            Size windowsSize = new Size(); ;
            windowsSize.Width = graph.MapConvert(graph.SizeGraph.XPos);
            windowsSize.Height = graph.MapConvert(graph.SizeGraph.YPos);
            this.Size = windowsSize;

            //缩放比例设置
            int ratio = 40;
            for (int i = 0; i < graph.NodeList.Count; i++)
            {
                Core.Location loc = new Core.Location(graph.NodeList[i].Location.XPos * ratio, graph.NodeList[i].Location.YPos * ratio, graph.NodeList[i].Location.ZPos * ratio);
                HeadNode node = graph.NodeList[i];
                node.Location = loc;
                graph.NodeList[i] = node;
            }


            //节点 + 路线
            foreach (HeadNode node in graph.NodeList)
            {
                Core.Location loc = graph.MapConvert(node.Location);
                Points p = new Points(loc,graph.PathWidth, graph.ColorCrossing);
                this.Controls.Add(p);
                foreach (Edge edge in node.Edge)
                {
                    Paths pa = new Paths(StoreComponentType.BothPath,graph.PathWidth, node.Location, graph.NodeList[edge.Idx].Location);
                    this.Controls.Add(pa);
                }                
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            
        }
    }
}
