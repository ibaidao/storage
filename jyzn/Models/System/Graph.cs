using System;
using System.Collections.Generic;

//无向图/路径相关模型
namespace Models
{
    /// <summary>
    /// 邻接表头
    /// </summary>
    public struct HeadNode
    {
        /// <summary>
        /// 节点数据
        /// </summary>
        public int Data;

        /// <summary>
        /// 节点绝对坐标
        /// </summary>
        public Core.Location Location;

        /// <summary>
        /// 节点名称
        /// </summary>
        public String Name;

        /// <summary>
        /// 邻接表
        /// </summary>
        public List<Edge> Edge;

        public HeadNode(int idx, string name, Core.Location loc)
        {
            Data = idx;
            Location = loc;
            Name = name;
            Edge = new List<Edge>();
        }

    }

    /// <summary>
    /// 邻接表边属性
    /// </summary>
    public struct Edge
    {
        /// <summary>
        /// 节点索引
        /// </summary>
        public int Idx;

        /// <summary>
        /// 边权重
        /// </summary>
        public int Weight;

        /// <summary>
        /// 边长（两节点间距）
        /// </summary>
        public int Distance;

        public Edge(int index, int weight, int distance)
        {
            Idx = index;
            Weight = weight;
            Distance = distance;
        }
    }

    /// <summary>
    /// 无向图
    /// </summary>
    public class Graph
    {
        private List<HeadNode> nodeList;

        public Graph()
        {
            this.nodeList = new List<HeadNode>();
        }

        /// <summary>
        /// 用指定数量的节点初始化图
        /// </summary>
        /// <param name="nodeCount">节点数</param>
        public Graph(int nodeCount)
        {
            this.NodeCount = nodeCount;
            this.EdgeCount = 0;
            this.nodeList = new List<HeadNode>(nodeCount);
            for (int i = 0; i < nodeCount; i++)
            {
                nodeList[i] = new HeadNode();
            }
        }

        /// <summary>
        /// 邻接表结构
        /// </summary>
        public List<HeadNode> NodeList
        {
            get { return this.nodeList; }
        }

        /// <summary>
        /// 节点数
        /// </summary>
        public int NodeCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 边数
        /// </summary>
        public int EdgeCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 增加无向边
        /// </summary>
        /// <param name="one">一个端点</param>
        /// <param name="two"></param>
        /// <param name="weight">权重</param>
        public void AddEdge(int one, int two, int weight)
        {
            int length = Core.Distance.Manhattan(nodeList[one].Location, nodeList[two].Location);

            //两条双向边代表无向边
            nodeList[one].Edge.Add(new Edge(two, weight, length));
            nodeList[two].Edge.Add(new Edge(one, weight, length));

            this.EdgeCount++;
        }

        /// <summary>
        /// 检查两点间直连距离
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns>未直连返回-1</returns>
        public int CheckEdgeDistance(int one, int two)
        {
            int result = -1;
            foreach (HeadNode node in NodeList)
            {
                if (node.Data != one) continue;

                foreach(Edge item in node.Edge)
                {
                    if (item.Idx == two)
                    {
                        result = item.Distance;
                        break;
                    }
                }
                break;
            }
            return result;
        }
    }
}
