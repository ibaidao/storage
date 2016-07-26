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

            StoreInfo store = new StoreInfo();
            Graph graph = store.GraphInfo;
            //仓库
            this.Size = new Size(graph.SizeGraph.XPos, graph.SizeGraph.YPos); 
            //缩放比例设置
            for (int i = 0; i < graph.NodeList.Count; i++)
            {
                Core.Location loc = graph.MapConvert(graph.NodeList[i].Location);
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
                        paCheck.PathType = StoreComponentType.BothPath;
                    }
                }                
            }
            foreach (Paths path in pathList)
            {
                path.ShowLine();
                this.Controls.Add(path);
            }
            this.AddStoreSomething(store, graph, StoreComponentType.PickStation);//拣货台
            this.AddStoreSomething(store, graph, StoreComponentType.Charger);//充电桩
            this.AddStoreSomething(store, graph, StoreComponentType.RestoreStation);//补货台
        }

        /// <summary>
        /// 增加仓库内模块
        /// </summary>
        /// <param name="store"></param>
        /// <param name="graph"></param>
        /// <param name="type"></param>
        private void AddStoreSomething(StoreInfo store,Graph graph, StoreComponentType type)
        {
            List<Station> pickStation = store.GetStationList(StoreComponentType.PickStation);
            foreach (Station item in pickStation)
            {
                StoreSth s = new StoreSth(Core.Distance.DecodeStringInfo(item.Location), graph.SizePickStation, graph.ColorPickStation);
                this.Controls.Add(s);
            }
        }
    }
}
