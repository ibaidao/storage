using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Core
{
    /// <summary>
    /// 地图实时信息
    /// </summary>
    public class StoreInfo
    {
        /// <summary>
        /// 地图数据
        /// </summary>
        private static Models.Graph graph;

        static StoreInfo()
        {
            Utilities.Singleton<Core.Path>.GetInstance();
            graph = Models.GlobalVariable.RealGraphTraffic;
        }

        /// <summary>
        /// 根据节点数据找到对应节点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HeadNode GetHeadNodeByData(int data)
        {
            return graph.GetHeadNodeByData(data);
        }

        /// <summary>
        /// 地图节点列表
        /// </summary>
        /// <returns></returns>
        public List<HeadNode> GraphNodeList
        {
            get { return graph.NodeList; }
        }

        /// <summary>
        /// 获取仓库充电桩/拣货台
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Station> GetStationList(StoreComponentType type)
        {
            string strWhere = string.Format(" Type = {0} ", type);
            return DbEntity.DStation.GetEntityList(strWhere, null);
        }

        /// <summary>
        /// 获取节点信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public StorePoints GetPointInfo(int data)
        {
            return DbEntity.DStorePoints.GetSingleEntity(data);
        }

        /// <summary>
        /// 新增节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loc"></param>
        /// <param name="dataID">节点数据</param>
        /// <param name="ratio">地图缩放比例</param>
        /// <returns></returns>
        public void AddPoint(string name, Location loc, int dataID, double ratio)
        {
            loc = loc.MapConvert(ratio);
            loc.XPos += Graph.MapMarginLeftUp.XPos;
            loc.YPos += Graph.MapMarginLeftUp.YPos;

            graph.AddPoint(dataID, name, loc, StoreComponentType.CrossCorner);
        }

        /// <summary>
        /// 修改节点类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="targetType"></param>
        public void ChangePoint(int data, StoreComponentType targetType)
        {
            int nodeIdx = graph.GetIndexByData(data);
            HeadNode node = graph.NodeList[nodeIdx];
            node.NodeType = targetType;
            graph.NodeList[nodeIdx] = node;
        }

        /// <summary>
        /// 移除节点
        /// </summary>
        /// <param name="data"></param>
        public void RemovePoint(int data)
        {
            DbEntity.DStorePoints.Delete(data);
            graph.RemovePoint(data);
        }

        /// <summary>
        /// 增加路径
        /// </summary>
        /// <param name="one">一端数据</param>
        /// <param name="two">另一端</param>
        /// <param name="storeType">类型(单向/双向)</param>
        /// <param name="weight"></param>
        public void AddPath(int one, int two, StoreComponentType storeType,int weight)
        {
            if (storeType == StoreComponentType.BothPath)
                graph.AddEdge(one, two, weight);
            else
                graph.AddDirectEdge(one, two, weight);
        }

        /// <summary>
        /// 移除双向路径
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public bool RemovePath(int one, int two)
        {
            string strWhere = string.Format(" (OnePoint={0} and TwoPoint={1} or  OnePoint={1} and TwoPoint={0}) ", one, two);
            DbEntity.DStorePaths.Delete(strWhere, null);
            return graph.RemoveEdge(one, two);
        }

        /// <summary>
        /// 移除有向边
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool RemoveDirectPath(int start, int end)
        {
            string strWhere = string.Format(" OnePoint={0} and TwoPoint={1} ", start, end);
            DbEntity.DStorePaths.Delete(strWhere, null);
            return graph.RemoveDirectEdge(start, end);
        }

        /// <summary>
        /// 停止节点
        /// </summary>
        /// <param name="data"></param>
        public void StopPoint(int data)
        {
            StorePoints point = DbEntity.DStorePoints.GetSingleEntity(data);
            point.Status = (short)StoreComponentStatus.Trouble;
            DbEntity.DStorePoints.Update(point);

            graph.StopPoint(data);
        }

        /// <summary>
        /// 关闭一条边
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        public void StopEdge(int one, int two)
        {
            string strWhere = string.Format(" (OnePoint={0} and TwoPoint={1} or  OnePoint={1} and TwoPoint={0}) ", one, two);
            StorePaths path = DbEntity.DStorePaths.GetSingleEntity(strWhere, null);
            path.Status = (short)StoreComponentStatus.Trouble;
            DbEntity.DStorePaths.Update(path);

            graph.StopEdge(one, two);
        }

        /// <summary>
        /// 缩放节点坐标值
        /// </summary>
        /// <param name="ratio"></param>
        public void ExchangePointRatio(double ratio)
        {
            //缩放比例设置
            for (int i = 0; i < graph.NodeList.Count; i++)
            {
                Models.Location loc = graph.NodeList[i].Location.MapConvert(ratio);
                loc.XPos += Graph.MapMarginLeftUp.XPos;
                loc.YPos += Graph.MapMarginLeftUp.YPos;

                HeadNode node = graph.NodeList[i];
                node.Location = loc;
                graph.NodeList[i] = node;
            }
        }

        /// <summary>
        /// 根据节点ID返回对应坐标
        /// </summary>
        /// <param name="locationID"></param>
        /// <returns></returns>
        public static Location GetLocationByPointID(int locationID)
        {
            return graph.GetHeadNodeByData(locationID).Location;
        }
    }
}
