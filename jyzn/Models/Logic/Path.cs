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
        private List<StorePoints> storePoints = null;
        private List<int> pointIdsList = null;

        /// <summary>
        /// 读取仓库初始地图
        /// </summary>
        /// <param name="storeID">仓库ID</param>
        /// <returns></returns>
        private void GetDefaultGraph(int storeID)
        {
            string strWhere = string.Format(" StoreID = {0} ", storeID);
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
        public void Dijkstar(int start, int end)
        {
            if (pointIdsList == null || pointIdsList.Count == null) return;
            //初始化
            Graph graph = GlobalVariable.RealGraphTraffic;
            Dictionary<int,int> VisitedList = new Dictionary<int,int>(pointIdsList.Count);
            List<int> UnkownList = new List<int>(pointIdsList);            
            VisitedList.Add(start,0);
            UnkownList.Remove(start);
            //在未访问节点中，找到已访问节点最短的点
            foreach (int item in UnkownList)
            {
                int minIdx = 0, minDistance=Int32.MaxValue, tmpLen;
                foreach (KeyValuePair<int, int> visitor in VisitedList)
                {
                    tmpLen = graph.CheckEdgeDistance(item,visitor.Key);
                    if (tmpLen > 0 && minDistance > tmpLen)
                    {
                        minIdx = item;
                        minDistance = visitor.Value + tmpLen;
                    }
                }
            }
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
