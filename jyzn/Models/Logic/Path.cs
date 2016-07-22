using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

namespace Models.Logic
{
    /// <summary>
    /// 路径选择
    /// </summary>
    public class Path
    {
        int[,] nodeDistance = null;//A[i][j]表示顶点i到j的路径长度
        List<int> nodeIdx = null;//将序号位置索引到指定节点
        int[,] pathNodeIdx = null;//从顶点i到j的最短路径上所经过的一个顶点


        public Path()
        {
            int storeID = 1;
            GetDefaultGraph(storeID);

            int count = GlobalVariable.RealGraphTraffic.NodeCount;
            nodeDistance=new int[count, count];
            pathNodeIdx = new int[count, count];
            nodeIdx = new List<int>(count);
        }

        /// <summary>
        /// 读取仓库初始地图
        /// </summary>
        /// <param name="storeID">仓库ID</param>
        /// <returns></returns>
        private void GetDefaultGraph(int storeID)
        {
            string strWhere = string.Format(" StoreID = {0} AND Status = 0 ", storeID);
            List<StorePoints> storePoints = DbEntity.DStorePoints.GetEntityList(strWhere, null);
            List<StorePaths> storePaths = DbEntity.DStorePaths.GetEntityList(strWhere, null);
            //解析位置节点
            foreach (StorePoints point in storePoints)
            {
                Core.Location loc= Core.Distance.DecodeStringInfo(point.Point);
                GlobalVariable.RealGraphTraffic.AddPoint(point.ID, point.Name, loc);
            }
            //解析路段
            foreach (StorePaths path in storePaths)
            {//权重默认都是1
                GlobalVariable.RealGraphTraffic.AddEdge(path.OnePoint, path.TwoPoint, 1);
            }
        }
        

        #region 最短路径算法
        /// <summary>
        /// Dijkstar算法
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>行走路径</returns>
        public List<HeadNode> Dijkstar(int start, int end)
        {
            Graph graph = GlobalVariable.RealGraphTraffic;
            if (graph == null || graph.NodeCount == 0) return null;

            List<HeadNode> pathList = new List<HeadNode>();//保存起止点间完整路径
            pathList.Add(graph.GetHeadNodeByID(start));
            Dictionary<int, int> VisitedList = new Dictionary<int, int>(graph.NodeCount);
            Dictionary<int, int> UnkownList = new Dictionary<int, int>(graph.NodeCount);            
            VisitedList.Add(start,0);
            foreach (HeadNode node in graph.NodeList)
            {
                if (node.Data == start) continue;
                UnkownList.Add(node.Data, graph.CheckEdgeDistance(node.Data, start));
            }
            //在未访问节点中，找到到已访问节点最短的节点
            int itemCount = UnkownList.Count;
            for (int i = 1; i < itemCount; i++)
            {
                int minIdx = 0, minDistance = Int32.MaxValue, tmpLen;
                //先找到当前最小的距离值
                foreach (KeyValuePair<int, int> item in UnkownList)
                {
                    if (item.Value > 0 && item.Value < minDistance)
                    {
                        minIdx = item.Key;
                        minDistance = item.Value;
                    }
                }
                pathList.Add(graph.GetHeadNodeByID(minIdx));
                VisitedList.Add(minIdx, minDistance);
                //再更新新节点减少是距离
                foreach (KeyValuePair<int, int> item in UnkownList)
                {
                    tmpLen = graph.CheckEdgeDistance(item.Value, minIdx);
                    if (tmpLen > 0 && (item.Value < 0 || tmpLen + minDistance < item.Value))
                    {
                        UnkownList[item.Key] = tmpLen + minDistance;
                    }
                }
                if (minIdx == end)
                {//终点为当前点到已选结果列表最短路径
                    break;
                }
            }
            return pathList;
        }

        /// <summary>
        /// Floyd算法
        /// </summary>
        public void Floyd()
        {
            Graph graph = GlobalVariable.RealGraphTraffic;
            if (graph == null || graph.NodeCount == 0) return;
            //初始化
            int count = graph.NodeCount;
            int i, j, k;
            for (i = 0; i < count; i++)
            {//初始化各节点间的距离
                nodeIdx.Add(graph.NodeList[i].Data);
                for (j = 0; j < count; j++)
                {
                    pathNodeIdx[i, j] = -1;
                    if (i == j)
                    {
                        nodeDistance[i, j] = 0;
                        continue;
                    }
                    foreach (Edge item in graph.NodeList[i].Edge)
                    {
                        if (item.Idx == j)
                        {
                            nodeDistance[i, j] = item.Distance;
                        }
                    }
                    if (nodeDistance[i, j] == 0)
                        nodeDistance[i, j] = count * 10;//相对大（大于最长距离）一个数，表示两点之间暂不连通
                }
            }
            for (k = 0; k < count; k++)
            {
                for (i = 0; i < count; i++)
                {
                    for (j = 0; j < count; j++)
                    {
                        if (nodeDistance[i, j] > nodeDistance[i, k] + nodeDistance[k, j])
                        {// i--j   距离大于   i--k--j
                            nodeDistance[i, j] = nodeDistance[i, k] + nodeDistance[k, j];
                            pathNodeIdx[i, j] = k;//最短路径经过k
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Floyd算法算出的常规路径
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>行走路径</returns>
        public List<HeadNode> GetGeneralPath(int start, int end)
        {
            if (pathNodeIdx == null || pathNodeIdx.Length == 0 || nodeIdx == null || nodeIdx.Count == 0) return null;
            int startIdx = nodeIdx.IndexOf(start),
                endIdx = nodeIdx.IndexOf(end),
                itemIdx;
            List<int> pathIdx = new List<int>();
            List<HeadNode>pathList = new List<HeadNode> ();

            pathIdx.Add(startIdx);
            GetPathNodeIndex(pathIdx, startIdx, endIdx);
            pathIdx.Add(endIdx);

            Graph graph  = GlobalVariable.RealGraphTraffic;
            foreach (int idx in pathIdx)
            {
                itemIdx = nodeIdx[idx];
                pathList.Add(graph.GetHeadNodeByID(itemIdx));
            }
            return pathList;
        }

        /// <summary>
        /// 递归读取路径索引
        /// </summary>
        /// <param name="idx"></param>
        private void GetPathNodeIndex(List<int> nodeList,int start,int end)
        {
            if (pathNodeIdx[start, end] == -1)//没用start == end，是因为算法中对于同一个点没有做处理，路径始终保持-1
            {//遍历结束：表示没有任何点可以缩短他们距离，则他们的关系有三种可能：1，不连通；2，是同一个点；3，直接相连
                return;
            }
            int k = pathNodeIdx[start, end];
            GetPathNodeIndex(nodeList, start, k);
            nodeList.Add(k);
            GetPathNodeIndex(nodeList, k, end);
        }

        #endregion

        /// <summary>
        /// 关闭某个节点通车
        /// </summary>
        /// <param name="idx">节点</param>
        public void StopPoints(int data)
        {
            Graph graph = GlobalVariable.RealGraphTraffic;
            int nodeIdx = graph.GetIndexByData(data);
            HeadNode node = graph.NodeList[nodeIdx];
            List<Edge> edge;

            foreach (Edge item in node.Edge)
            {//依次访问节点连接的所有边
                edge = graph.NodeList[item.Idx].Edge;
                for (int i = 0; i < edge.Count; i++)
                {//无向边是双向的有向边替代
                    if (edge[i].Idx == nodeIdx)
                    {
                        edge.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 关闭一条路线
        /// </summary>
        /// <param name="edgeList">将路线分解为 有两个节点直接相连的路</param>
        /// <returns></returns>
        public bool StopPath(Dictionary<int,int> edgeList)
        {
            bool result = true;
            foreach (KeyValuePair<int, int> edge in edgeList)
            {
                result = result && GlobalVariable.RealGraphTraffic.RemoveEdge(edge.Key, edge.Value);
            }
            return result;
        }
    }
}
