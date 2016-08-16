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
        private static Models.Graph graph=null;

        static StoreInfo()
        {
            graph = Models.GlobalVariable.RealGraphTraffic;

            //初始化全局变量
            initalModelGlobalData<Devices>(DbEntity.DDevices, Models.GlobalVariable.RealDevices);
            foreach (Devices device in Models.GlobalVariable.RealDevices)
            {
                device.Status = (short)StoreComponentStatus.Trouble;
            }
            initalModelGlobalData<Shelf>(DbEntity.DShelf, Models.GlobalVariable.RealShelves);
            initalModelGlobalData<Station>(DbEntity.DStation, Models.GlobalVariable.RealStation);
        }

        /// <summary>
        /// 初始化全局变量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private static void initalModelGlobalData<T>(Models.dbHandler.DataAccess<T> source, List<T> target) where T : class
        {
            List<T> sourceList =  source.GetEntityList();
            foreach (T item in sourceList)
            {
                target.Add(item);
            }
        }

        #region 返回实时数据
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
        /// 仓库实时设备列表
        /// </summary>
        /// <returns></returns>
        public List<Devices> GraphDeviceList
        {
            get { return Models.GlobalVariable.RealDevices; }
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
        /// 根据节点ID返回对应坐标
        /// </summary>
        /// <param name="locationID"></param>
        /// <returns></returns>
        public static Location GetLocationByPointID(int locationID)
        {
            return graph.GetHeadNodeByData(locationID).Location;
        }
        #endregion 

        #region 修改实时数据

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
            loc = CalcLocation.MapConvert(loc, ratio);
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
            int length = CalcLocation.Manhattan(graph.NodeList[one].Location, graph.NodeList[two].Location);

            if (storeType == StoreComponentType.BothPath)
                graph.AddEdge(one, two, weight,length);
            else
                graph.AddDirectEdge(one, two, weight,length);
        }

        /// <summary>
        /// 移除双向路径
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public bool RemovePath(int one, int two)
        {
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
        public void ChangeEdgeStatus(int one, int two, StoreComponentStatus pathStatus)
        {
            graph.ChangeEdgeUseable(one, two, pathStatus == StoreComponentStatus.OK);
        }

        #endregion
    }
}
