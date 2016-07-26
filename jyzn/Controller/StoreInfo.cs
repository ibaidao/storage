using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Controller
{
    /// <summary>
    /// 应用层 对仓库执行的操作
    /// </summary>
    public class StoreInfo
    {
        private const int PATH_WEIGHT = 1;
        /// <summary>
        /// 地图数据
        /// </summary>
        private static Models.Graph graph;

        static StoreInfo()
        {
            Core.Singleton<Models.Logic.Path>.GetInstance();
            graph = Models.GlobalVariable.RealGraphTraffic;
        }

        /// <summary>
        /// 仓库地图信息
        /// </summary>
        public Graph GraphInfo
        {
            get { return graph; }
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
        /// 新增仓库充电桩/拣货台
        /// </summary>
        /// <param name="type">拣货台/补货台/充电桩</param>
        /// <param name="locIdx">节点数据</param>
        public void AddChargerPickStation(StoreComponentType type, string code, int locIdx)
        {
            string strWhere = string.Format(" LocationID = {0} ", locIdx);
            object item = DbEntity.DStation.GetSingleEntity(strWhere, null);
            if (item == null)
            {
                DbEntity.DStation.Insert(new Station()
                {
                    Code = code,
                    LocationID = locIdx,
                    Status = 1,
                    Type = (short)type,
                    Location = graph.GetHeadNodeByData(locIdx).Location.ToString()
                });
                //修改节点类型
                StorePoints point = DbEntity.DStorePoints.GetSingleEntity(strWhere,null);
                point.Type = (short)type;
                DbEntity.DStorePoints.Update(point);
            };
        }

        /// <summary>
        /// 新增节点
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <param name="loc"></param>
        public void AddPoint(int data, string name, Core.Location loc)
        {
            DbEntity.DStorePoints.Insert(new StorePoints() {
                Name = name,
                Point = loc.ToString (), 
                StoreID = Models.GlobalVariable.STORE_ID,
                Status = 0,
                Type = (short)StoreComponentType.CrossCorner
            });
            graph.AddPoint(data, name, loc);
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
        /// 增加双向路径
        /// </summary>
        /// <param name="one">一端数据</param>
        /// <param name="two">另一端</param>
        public void AddPath(int one, int two)
        {
            DbEntity.DStorePaths.Insert(new StorePaths()
            {
                Weight = PATH_WEIGHT,
                Status = (short)StoreComponentStatus.OK,
                StoreID = Models.GlobalVariable.STORE_ID,
                OnePoint = one,
                TwoPoint = two,
                Type = (short)StoreComponentType.BothPath
            });
            graph.AddEdge(one, two, PATH_WEIGHT);
        }

        /// <summary>
        /// 增加单向路径
        /// </summary>
        /// <param name="start">起点数据</param>
        /// <param name="end"></param>
        public void AddPathDirection(int start, int end)
        {
            DbEntity.DStorePaths.Insert(new StorePaths()
            {
                Weight = PATH_WEIGHT,
                Status = (short)StoreComponentStatus.OK,
                StoreID = Models.GlobalVariable.STORE_ID,
                OnePoint = start,
                TwoPoint = end,
                Type = (short)StoreComponentType.OneWayPath
            });
            graph.AddDirectEdge(start, end, PATH_WEIGHT);
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

    }
}
