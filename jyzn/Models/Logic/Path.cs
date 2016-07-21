﻿using System;
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
        private List<StorePoints> storePoints = null;
        private List<int> pointIdsList = null;

        /// <summary>
        /// 读取仓库初始地图
        /// </summary>
        /// <param name="storeID">仓库ID</param>
        /// <returns></returns>
        private void GetDefaultGraph(int storeID)
        {
            string strWhere = string.Format(" StoreID = {0} AND Status = 0 ", storeID);
            storePoints = DbEntity.DStorePoints.GetEntityList(strWhere, null);
            List<StorePaths> storePaths = DbEntity.DStorePaths.GetEntityList(strWhere, null);
            //解析位置节点
            pointIdsList = new List<int>();
            foreach (StorePoints point in storePoints)
            {
                pointIdsList.Add(point.ID);
                Core.Location loc= Core.Distance.DecodeStringInfo(point.Point);
                GlobalVariable.RealGraphTraffic.NodeList.Add(new HeadNode(point.ID, point.Name, loc));
            }
            //解析路段
            foreach (StorePaths path in storePaths)
            {//权重默认都是1
                GlobalVariable.RealGraphTraffic.AddEdge(path.OnePoint, path.TwoPoint, 1);
            }
        }

        /// <summary>
        /// 路径初始化
        /// </summary>
        public void InitialPath(int storeId)
        {
            GetDefaultGraph(storeId);
        }


        #region 最短路径算法
        /// <summary>
        /// Dijkstar算法
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public List<HeadNode> Dijkstar(int start, int end)
        {
            if (pointIdsList == null || pointIdsList.Count == 0) return null;
            //初始化
            List<HeadNode> pathList = new List<HeadNode>();
            Graph graph = GlobalVariable.RealGraphTraffic;
            pathList.Add(graph.GetHeadNodeByID(start));
            Dictionary<int,int> VisitedList = new Dictionary<int,int>(pointIdsList.Count);
            Dictionary<int, int> UnkownList = new Dictionary<int, int>(pointIdsList.Count);            
            VisitedList.Add(start,0);
            foreach (int item in pointIdsList)
            {
                if (item == start) continue;
                UnkownList.Add(item,graph.CheckEdgeDistance(item, start));
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
        /// Floyd-Warshall算法
        /// </summary>
        public void Floyd()
        {

        }

        #endregion
    }
}
