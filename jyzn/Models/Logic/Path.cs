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

        /// <summary>
        /// 读取仓库初始地图
        /// </summary>
        /// <param name="storeID">仓库ID</param>
        /// <returns></returns>
        private Graph GetDefaultGraph(int storeID)
        {
            string strWhere = string.Format(" StoreID = {0} ", storeID);
            List<StorePoints> storePoints = DbEntity.DStorePoints.GetEntityList(strWhere, null);
            List<StorePaths> storePaths = DbEntity.DStorePaths.GetEntityList(strWhere, null);
            //解析位置节点
            Graph graph = new Graph(storePoints.Count);
            Dictionary<int, Core.Location> locationForSearch = new Dictionary<int, Core.Location>();
            foreach (StorePoints point in storePoints)
            {
                Core.Location loc= Core.Distance.DecodeStringInfo(point.Point);
                graph.NodeList.Add(new HeadNode(point.ID, point.Name,loc ));
                locationForSearch.Add(point.ID, loc);
            }
            //解析路段
            foreach (StorePaths path in storePaths)
            {//权重默认都是1
                graph.AddEdge(path.OnePoint, path.TwoPoint, 1);
            }
            return graph;
        }

        /// <summary>
        /// 路径初始化
        /// </summary>
        public void InitialPath(int storeId)
        {
            Graph graph = GetDefaultGraph(storeId);
        }


        #region 最短路径算法
        /// <summary>
        /// Dijkstar算法
        /// </summary>
        public void Dijkstar()
        {

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
