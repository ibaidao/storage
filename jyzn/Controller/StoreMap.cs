using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Controller
{
    /// <summary>
    /// 仓库地图相关操作
    /// </summary>
    public class StoreMap
    {
        private const int PATH_WEIGHT = 1;
        private BLL.StoreInfo storeDb = null;
        private static Core.StoreInfo store = null;

        static StoreMap()
        {
            store = new Core.StoreInfo();
        }

        public StoreMap(Action<ErrorCode> errshow, Action<StoreComponentType, int, Location> normalShow)
        {
            storeDb = new BLL.StoreInfo();

            BLL.InfoProcess infoHandler = new BLL.InfoProcess(errshow, normalShow);
        }

        /// <summary>
        /// 开始监听客户端通信（由于测试用例会实例化本实体，所以独立出来）
        /// </summary>
        public static void StartListenClient()
        {
            Core.Communicate.StartListening();
        }

        /// <summary>
        /// 实时增加节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loc"></param>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public ErrorCode RealtimeAddPoint(string name, Location loc, out int dataID)
        {
            //更新数据库
            ErrorCode result = storeDb.AddPoint(name, loc, out dataID);
            //更新实时地图
            if (result == ErrorCode.OK)
                store.AddPoint(name, loc, dataID, Graph.RatioMapZoom);

            return result;
        }

        /// <summary>
        /// 修改节点类型
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="data"></param>
        /// <param name="dataRemark">节点备注</param>
        public void ChangePointType(Models.StoreComponentType targetType, int data, string dataRemark="")
        {
            dataRemark = dataRemark == "" ? data.ToString() : dataRemark;
            //更新数据库
            Models.HeadNode node = store.GetHeadNodeByData(data);
            storeDb.AddChargerPickStation(targetType, dataRemark, data, node.Location);
            //更新实时地图
            store.ChangePoint(data, targetType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <param name="storeType"></param>
        /// <returns></returns>
        public ErrorCode RealtimeAddPath(int one, int two, StoreComponentType storeType)
        {
            //更新数据库
            ErrorCode result = storeDb.AddPath(one, two, storeType, PATH_WEIGHT);
            //更新实时地图
            if (result == ErrorCode.OK)
                store.AddPath(one, two, storeType, PATH_WEIGHT);

            return result;
        }

        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateIniFile(string key, string value)
        {
            storeDb.UpdateIniFile(Models.Graph.InitSection,key, value);
        }

        /// <summary>
        /// 缩放地图坐标
        /// </summary>
        public void ExchangeMapRatio()
        {
            store.ExchangePointRatio(Graph.RatioMapZoom);
        }

        /// <summary>
        /// 实时地图所有节点
        /// </summary>
        /// <returns></returns>
        public List<HeadNode> RealtimeNodeList
        {
            get { return store.GraphNodeList; }
        }
        
        /// <summary>
        /// 根据节点数据找到对应节点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HeadNode GetMapNodeByData(int data)
        {
            return store.GetHeadNodeByData(data);
        }
    }
}
