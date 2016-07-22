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
        private List<int> nodeIdxList;//用于索引位置对应的节点
        private List<HeadNode> nodeList;

        public Graph()
        {
            this.nodeList = new List<HeadNode>();
            this.nodeIdxList = new List<int>();
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
            this.nodeIdxList = new List<int>(nodeCount);
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
        /// 增加节点
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="name">备注名称</param>
        /// <param name="loc">对应坐标位置</param>
        public void AddPoint(int data, string name, Core.Location loc)
        {
            this.nodeIdxList.Add(data);
            this.nodeList.Add(new HeadNode(data, name, loc));
            this.NodeCount++;
        }

        /// <summary>
        /// 移除/关闭 节点
        /// </summary>
        /// <param name="data">节点数据</param>
        /// <returns></returns>
        public bool RemovePoint(int data)
        {
            int removeCount = 0, nodeIdx = this.GetIndexByData(data);
            HeadNode node = this.nodeList[nodeIdx];
            List<Edge> edge;
            //先删除指向当前节点的节点（边中含有当前节点）
            foreach (Edge item in node.Edge)
            {//依次访问节点连接的所有边
                edge = this.NodeList[item.Idx].Edge;
                for (int i = 0; i < edge.Count; i++)
                {//无向边是双向的有向边替代
                    if (edge[i].Idx == nodeIdx)
                    {
                        edge.RemoveAt(i);
                        removeCount++;
                        break;
                    }
                }
                this.EdgeCount--;
            }
            //再移除当前节点
            this.nodeList.RemoveAt(nodeIdx);
            this.NodeCount--;
            //最后所有边索引中，大于等于被删节点的减1
            Edge tmpEdge;
            for(int i=0;i<NodeCount;i++)
            {//所有
                edge = nodeList[i].Edge;
                for (int j = 0; j < edge.Count; j++)
                {
                    if (edge[j].Idx >= nodeIdx)
                    {
                        tmpEdge = edge[j];
                        tmpEdge.Idx--;
                        edge[j] = tmpEdge;
                    }
                }
            }
            return removeCount == node.Edge.Count;
        }

        /// <summary>
        /// 增加无向边
        /// </summary>
        /// <param name="one">一个端点</param>
        /// <param name="two"></param>
        /// <param name="weight">权重</param>
        public void AddEdge(int one, int two, int weight)
        {

            int oneIdx = this.GetIndexByData(one),
                twoIdx = this.GetIndexByData(two);
            int length = Core.Distance.Manhattan(nodeList[oneIdx].Location, nodeList[twoIdx].Location);

            //两条双向边代表无向边
            nodeList[oneIdx].Edge.Add(new Edge(twoIdx, weight, length));
            nodeList[twoIdx].Edge.Add(new Edge(oneIdx, weight, length));

            this.EdgeCount++;
        }

        /// <summary>
        /// 移除一条路
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public bool RemoveEdge(int one, int two)
        {
            int oneIdx = GetIndexByData(one),
                twoIdx = GetIndexByData(two),
                count = 0;
            int oneEdgeCount = nodeList[oneIdx].Edge.Count,
                twoEdgeCount = nodeList[twoIdx].Edge.Count;
            for (int i = 0; i < oneEdgeCount; i++)
            {
                if (nodeList[oneIdx].Edge[i].Idx == twoIdx)
                {
                    nodeList[oneIdx].Edge.RemoveAt(i);
                    count++;
                    break;
                }
            }
            for (int i = 0; i < twoEdgeCount; i++)
            {
                if (nodeList[twoIdx].Edge[i].Idx == oneIdx)
                {
                    nodeList[twoIdx].Edge.RemoveAt(i);
                    count++;
                    break;
                }
            }
            return count == 2;
        }

        /// <summary>
        /// 检查两点间直连距离
        /// </summary>
        /// <param name="one">节点数据</param>
        /// <param name="two"></param>
        /// <returns>未直连返回-1</returns>
        public int CheckEdgeDistance(int one, int two)
        {
            int result = -1;
            foreach (HeadNode node in nodeList)
            {
                if (node.Data != one) continue;

                foreach(Edge item in node.Edge)
                {
                    if (nodeList[item.Idx].Data == two)
                    {
                        result = item.Distance;
                        break;
                    }
                }
                break;
            }
            return result;
        }

        /// <summary>
        /// 通过节点数据获取节点位置信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HeadNode GetHeadNodeByID(int data)
        {
            return nodeList[nodeIdxList[data]];
            //HeadNode result = new HeadNode();
            //foreach (HeadNode node in nodeList)
            //{
            //    if (node.Data == data)
            //    {
            //        result = node;
            //        break;
            //    }
            //}
            //return result;
        }

        /// <summary>
        /// 根据节点数据获取索引
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int GetIndexByData(int data)
        {
            return nodeIdxList.IndexOf(data);
            //int result = -1;
            //for (int i = 0; i < nodeList.Count; i++)
            //{
            //    if (nodeList[i].Data == data)
            //    {
            //        result = i;
            //        break;
            //    }
            //}
            //return result;
        }
       
    }
}
