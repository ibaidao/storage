using System;
using System.Collections.Generic;
using System.Collections;
using Models;

namespace Core
{
    /// <summary>
    /// 路径选择
    /// </summary>
    public class Path
    {
        static int InstanceCount = 0;
        int[,] nodeDistance = null;//A[i][j]表示顶点i到j的路径长度
        List<int> nodeIdx = null;//将序号位置索引到指定节点数据
        int[,] pathNodeIdx = null;//从顶点i到j的最短路径上所经过的一个顶点


        public Path()
        {
            if (InstanceCount > 0) throw new Exception("单例实体");

            int storeID = 1;
            GetDefaultGraph(storeID);

            int count = Models.GlobalVariable.RealGraphTraffic.NodeCount;
            nodeDistance=new int[count, count];
            pathNodeIdx = new int[count, count];
            nodeIdx = new List<int>(count);

            //默认计算最短路径
            Floyd();

            InstanceCount++;
        }

        /// <summary>
        /// 读取仓库初始地图
        /// </summary>
        /// <param name="storeID">仓库ID</param>
        /// <returns></returns>
        private void GetDefaultGraph(int storeID)
        {
            string strWhere = string.Format(" StoreID = {0} ", storeID);
            List<StorePoints> storePoints = DbEntity.DStorePoints.GetEntityList(strWhere, null);
            List<StorePaths> storePaths = DbEntity.DStorePaths.GetEntityList(strWhere, null);
            Dictionary<int, Location> pointLoc = new Dictionary<int, Location>();
            //解析位置节点
            foreach (StorePoints point in storePoints)
            {
                Location loc = Location.DecodeStringInfo(point.Point);
                loc.Status = point.Status == (short)StoreComponentStatus.OK;
                Models.GlobalVariable.RealGraphTraffic.AddPoint(point.ID, point.Name, loc, (StoreComponentType)(point.Type));

                pointLoc.Add(point.ID, loc);
            }
            Models.GlobalVariable.AllMapPoints = pointLoc;
            //解析路段
            bool status;
            foreach (StorePaths path in storePaths)
            {//权重默认都是1
                status = path.Status == (short)StoreComponentStatus.OK;
                int length = CalcLocation.Manhattan(Models.GlobalVariable.AllMapPoints[path.OnePoint], Models.GlobalVariable.AllMapPoints[path.TwoPoint]);

                if (path.Type == (short)StoreComponentType.BothPath)
                    Models.GlobalVariable.RealGraphTraffic.AddEdge(path.OnePoint, path.TwoPoint, path.Weight,length, status);
                else if (path.Type == (short)StoreComponentType.OneWayPath)
                    Models.GlobalVariable.RealGraphTraffic.AddDirectEdge(path.OnePoint, path.TwoPoint, path.Weight, length, status);
                else
                    throw new Exception("路径类型不存在");
            }
        }

        #region 最短路径算法
        /// <summary>
        /// 重新计算最短路径（禁用/启用某路径/节点后）
        /// </summary>
        public void RefreshPath()
        {
            this.Floyd();
        }

        /// <summary>
        /// 默认最短路径
        /// </summary>
        /// <param name="start">起点数据</param>
        /// <param name="end"></param>
        /// <returns>行走路径</returns>
        public List<HeadNode> GetGeneralPath(int start, int end)
        {
            if (pathNodeIdx == null || pathNodeIdx.Length == 0 || nodeIdx == null || nodeIdx.Count == 0) return null;
            int startIdx = nodeIdx.IndexOf(start),
                endIdx = nodeIdx.IndexOf(end),
                itemIdx;
            if (startIdx < 0 || endIdx < 0) return null;

            List<int> pathIdx = new List<int>();
            List<HeadNode> pathList = new List<HeadNode>();

            pathIdx.Add(startIdx);
            GetPathNodeIndex(pathIdx, startIdx, endIdx);
            pathIdx.Add(endIdx);

            Graph graph = Models.GlobalVariable.RealGraphTraffic;
            foreach (int idx in pathIdx)
            {
                itemIdx = nodeIdx[idx];
                pathList.Add(graph.GetHeadNodeByData(itemIdx));
            }

            MergePathNode(pathList);
            return pathList;
        }

        /// <summary>
        /// 重新求解新的最短路径
        /// </summary>
        /// <param name="graph">新地图</param>
        /// <param name="start">起点数据</param>
        /// <param name="end"></param>
        /// <returns>行走路径</returns>
        public List<HeadNode> GetNewPath(Graph graph, int start, int end)
        {
            if (graph == null || graph.NodeCount == 0) return null;

            return Dijkstar(graph, start, end);
        }

        /// <summary>
        /// Dijkstar算法
        /// </summary>
        /// <param name="graph">地图数据</param>
        /// <param name="start">起点数据</param>
        /// <param name="end"></param>
        /// <returns>行走路径</returns>
        private List<HeadNode> Dijkstar(Graph graph, int start, int end)
        {
            Dictionary<int, int> visitedPreNode = new Dictionary<int, int>(),//<int,int> = <当前数据，最短路径的上个节点数据>
                                VisitedList = new Dictionary<int, int>(graph.NodeCount),//<int,int> = <数据，到起点距离>
                                UnkownList = new Dictionary<int, int>(graph.NodeCount);
            List<int> unkownIdx = new List<int>();//节点位置索引，跟UnkownList同步，仅用于协助其更新
            VisitedList.Add(start, 0);
            int startDistance;
            foreach (HeadNode node in graph.NodeList)
            {
                if (node.Data == start) continue;
                startDistance = graph.CheckEdgeDistance(node.Data, start);
                UnkownList.Add(node.Data, startDistance);
                unkownIdx.Add(node.Data);
                if (startDistance > 0) visitedPreNode.Add(node.Data, start);
            }
            //在未访问节点中，找到到已访问节点最短的节点
            int itemCount = UnkownList.Count;
            for (int i = 0; i < itemCount; i++)
            {
                int minNode = 0, minDistance = Int32.MaxValue, tmpLen;
                //先找到当前最小的距离值
                foreach (KeyValuePair<int, int> item in UnkownList)
                {
                    if (item.Value > 0 && item.Value < minDistance)
                    {
                        minNode = item.Key;
                        minDistance = item.Value;
                    }
                }
                if (minNode == 0) return null;//终点不可达
                VisitedList.Add(minNode, minDistance);
                UnkownList.Remove(minNode);
                unkownIdx.Remove(minNode);
                //再更新新节点减少的距离
                foreach (int idx in unkownIdx)
                {
                    tmpLen = graph.CheckEdgeDistance(idx, minNode);
                    if (tmpLen > 0 && (UnkownList[idx] < 0 || tmpLen + minDistance < UnkownList[idx]))
                    {
                        UnkownList[idx] = tmpLen + minDistance;
                        if (visitedPreNode.ContainsKey(idx)) visitedPreNode[idx] = minNode;
                        else visitedPreNode.Add(idx, minNode);
                    }
                }
                if (minNode == end)
                {//终点为当前点到已选结果列表最短路径
                    break;
                }
            }
            return GetPathNodeData(visitedPreNode, start, end);
        }

        /// <summary>
        /// Floyd算法
        /// </summary>
        private void Floyd()
        {
            Graph graph = Models.GlobalVariable.RealGraphTraffic;
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
                    {//nodeDistance[i, j] = 0;//默认值
                        continue;
                    }
                    foreach (Edge item in graph.NodeList[i].Edge)
                    {
                        if (item.Idx == j && item.Status)
                        {
                            nodeDistance[i, j] = item.Distance;
                        }
                    }
                    if (nodeDistance[i, j] == 0)
                        nodeDistance[i, j] = count * 1000;//相对大（大于最长距离）一个数，表示两点之间暂不连通
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
        /// 根据节点索引递归读取路径索引
        /// </summary>
        /// <param name="nodeList">保存路径节点</param>
        /// <param name="start">起点的位置索引</param>
        /// <param name="end"></param>
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

        /// <summary>
        /// 根据节点数据循环读取路径
        /// </summary>
        /// <param name="visitedPreNode">#int,int# = #当前数据，最短路径的上个节点数据#</param>
        /// <param name="start">起点的数据</param>
        /// <param name="end"></param>
        /// <returns></returns>
        private List<HeadNode> GetPathNodeData(Dictionary<int, int> visitedPreNode,int start, int end)
        {
            if(visitedPreNode == null || visitedPreNode.Count ==0) return null;

            int tmpIdx = end;
            List<HeadNode> pathList = new List<HeadNode>();
            Graph graph = Models.GlobalVariable.RealGraphTraffic;

            //通过结束点向前逆序遍历
            pathList.Add(graph.GetHeadNodeByData(end));
            for (int i = 0; i < visitedPreNode.Count;i++ )
            {
                if (visitedPreNode[tmpIdx] == start)
                    break;
                pathList.Add(graph.GetHeadNodeByData(visitedPreNode[tmpIdx]));
                tmpIdx = visitedPreNode[tmpIdx];
            }
            pathList.Add(graph.GetHeadNodeByData(start));
            //反正后即为路径的正序
            pathList.Reverse();

            MergePathNode(pathList);
            return pathList;
        }

        /// <summary>
        /// 合并路径节点（删除同一条路上的中间节点）
        /// </summary>
        /// <param name="pathNode">路径上的所有节点</param>
        private void MergePathNode(List<HeadNode> pathNode)
        {
            for (int i = 1; i < pathNode.Count - 1; i++)
            {// 头尾两节点不检查
                if (pathNode[i - 1].Location.YPos == pathNode[i].Location.YPos && pathNode[i + 1].Location.YPos == pathNode[i].Location.YPos//三点仅X轴方向坐标改变
                        && pathNode[i - 1].Location.ZPos == pathNode[i].Location.ZPos && pathNode[i + 1].Location.ZPos == pathNode[i].Location.ZPos ||
                    pathNode[i - 1].Location.XPos == pathNode[i].Location.XPos && pathNode[i + 1].Location.XPos == pathNode[i].Location.XPos//Y轴
                        && pathNode[i - 1].Location.ZPos == pathNode[i].Location.ZPos && pathNode[i + 1].Location.ZPos == pathNode[i].Location.ZPos ||
                    pathNode[i - 1].Location.XPos == pathNode[i].Location.XPos && pathNode[i + 1].Location.XPos == pathNode[i].Location.XPos//Z轴
                        && pathNode[i - 1].Location.YPos == pathNode[i].Location.YPos && pathNode[i + 1].Location.YPos == pathNode[i].Location.YPos)
                {//若跟前后两节点仅一个坐标的值改变，则三点在一条路线上，则当前节点可以删除
                    pathNode.RemoveAt(i);
                    i--;
                }
            }
        }
        #endregion

        #region 关闭节点、路径

        /// <summary>
        /// 关闭某个节点通车
        /// </summary>
        /// <param name="grap">地图数据</param>
        /// <param name="dataList">节点数据</param>
        public bool StopPoints(Graph grap, List<int> dataList)
        {
            bool result = true;
            foreach (int data in dataList)
            {
                result = result && grap.RemovePoint(data);
            }
            return result;            
        }

        /// <summary>
        /// 关闭一条路线
        /// </summary>
        /// <param name="grap">地图数据</param>
        /// <param name="edgeList">将路线分解为 有两个节点直接相连的路</param>
        /// <returns></returns>
        public bool StopPath(Graph grap, Dictionary<int, int> edgeList)
        {
            bool result = true;
            foreach (KeyValuePair<int, int> edge in edgeList)
            {
                result = result && grap.RemoveEdge(edge.Key, edge.Value);
            }
            return result;
        }

        #endregion
    }
}
