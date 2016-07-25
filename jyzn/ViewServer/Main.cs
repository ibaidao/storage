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

            //int argb = Color.Gray.ToArgb();
            //Color cp = Color.FromArgb(argb);

            StoreInit store = new StoreInit();
            Graph graph = store.GraphInfo;
            //仓库
            Size windowsSize = new Size(); ;
            windowsSize.Width = graph.MapConvert(graph.SizeGraph.XPos);
            windowsSize.Height = graph.MapConvert(graph.SizeGraph.YPos);
            this.Size = windowsSize;
            //节点 + 路线
            foreach (HeadNode node in graph.NodeList)
            {
                Core.Location loc = graph.MapConvert(node.Location);
                Points p = new Points(loc, graph.ColorCrossing);
                this.Controls.Add(p);
                foreach (Edge edge in node.Edge)
                {
                    Core.Location endLoc = graph.NodeList[edge.Idx].Location;//任选一点改变坐标
                    if (endLoc.YPos == node.Location.YPos) endLoc.XPos += graph.PathWidth;
                    else if (endLoc.XPos == node.Location.XPos) endLoc.YPos += graph.PathWidth;

                    Paths pa = new Paths(StoreComponentType.BothPath, node.Location, endLoc);
                    this.Controls.Add(pa);
                }                
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            
        }
    }
}
