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
            //根据显示比例更新仓库元素尺寸
            UpdateGraphConfig4Show();
        }

        public StoreMap()
        {
            storeDb = new BLL.StoreInfo();
        }

        #region 开启通讯
        /// <summary>
        /// 开始监听客户端通信（由于测试用例会实例化本实体，所以没写在静态构造函数中）
        /// </summary>
        public static void StartListenClient()
        {
            Core.Communicate.StartListening(StoreComponentType.MainSystem);
        }
        #endregion

        #region 改变界面展示
        /// <summary>
        /// 更新地图默认配置的显示值
        /// </summary>
        private static void UpdateGraphConfig4Show()
        {
            Graph.SizeDevice = ExchangeMapRatio(Graph.SizeDevice);
            Graph.SizeCharger = ExchangeMapRatio(Graph.SizeCharger);
            Graph.SizePickStation = ExchangeMapRatio(Graph.SizePickStation);
            Graph.SizeRestore = ExchangeMapRatio(Graph.SizeRestore);
            Graph.SizeShelf = ExchangeMapRatio(Graph.SizeShelf);
            Graph.PathWidth = ExchangeMapRatio(Graph.PathWidth);
            Graph.SizeGraph = ExchangeMapRatio(Graph.SizeGraph);
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
        public void ChangePointType(Models.StoreComponentType targetType, int data, string dataRemark = "")
        {
            dataRemark = dataRemark == "" ? data.ToString() : dataRemark;
            //更新数据库
            Models.HeadNode node = store.GetHeadNodeByData(data);
            storeDb.AddChargerPickStation(targetType, dataRemark, data, node.Location);
            //更新实时地图
            store.ChangePoint(data, targetType);
        }

        /// <summary>
        /// 增加路径
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
            {
                store.AddPath(one, two, storeType, PATH_WEIGHT);
                Utilities.Singleton<Core.Path>.GetInstance().RefreshPath();
            }

            return result;
        }

        /// <summary>
        /// 关闭一条路径
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public ErrorCode RealtimeStopPath(int one, int two)
        {
            return RealtimeChangePathStatus(one, two, StoreComponentStatus.Trouble);
        }

        /// <summary>
        /// 重新启用一条路径
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public ErrorCode RealtimeRestartPath(int one, int two)
        {
            return RealtimeChangePathStatus(one, two, StoreComponentStatus.OK);
        }

        /// <summary>
        /// 改变一条路径的可用状态
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private ErrorCode RealtimeChangePathStatus(int one, int two, StoreComponentStatus status)
        {
            ErrorCode result = storeDb.UpdatePathStatus(one, two, status);
            if (result == ErrorCode.OK)
            {
                store.ChangeEdgeStatus(one, two, status);
                Utilities.Singleton<Core.Path>.GetInstance().RefreshPath();
            }

            return result;
        }

        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateIniFile(string key, string value)
        {
            storeDb.UpdateIniFile(Models.Graph.InitSection, key, value);
        }

        #endregion

        #region 返回模型数据

        /// <summary>
        /// 实时地图所有节点
        /// </summary>
        /// <returns></returns>
        public List<HeadNode> RealtimeNodeList
        {
            get { return store.GraphNodeList; }
        }

        /// <summary>
        /// 仓库实时设备信息
        /// </summary>
        public List<Models.Devices> RealtimeDevice
        {
            get { return store.GraphDeviceList; }
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

        #endregion

        #region 缩放地图

        /// <summary>
        /// 按地图比例 改变值
        /// </summary>
        /// <param name="realValue"></param>
        /// <returns></returns>
        public static int ExchangeMapRatio(int realValue)
        {
            //实际仓库尺寸 -》 地图缩放(cm -> 像素)
            int relative = Core.CalcLocation.MapConvert(realValue, Graph.RatioMapZoom);
            //地图本身缩放
            return Core.CalcLocation.MapConvert(relative, Graph.RatioMapSelfZoom);
        }

        /// <summary>
        /// 缩放地图坐标
        /// </summary>
        /// <param name="realLoc">真实相对位置/尺寸</param>
        /// <return>变换后相对位置/尺寸</return>
        public static Location ExchangeMapRatio(Location realLoc)
        {
            //实际仓库尺寸 -》 地图缩放(cm -> 像素)
            Location loc = Core.CalcLocation.MapConvert(realLoc, Graph.RatioMapZoom);
            //地图本身缩放
            return Core.CalcLocation.MapConvert(loc, Graph.RatioMapSelfZoom);
        }

        /// <summary>
        /// 转化为地图坐标
        /// </summary>
        /// <param name="realLoc">真实坐标</param>
        /// <returns>变换后坐标</returns>
        public static Location ExchangeLocation(Location realLoc)
        {
            Models.Location loc = ExchangeMapRatio(realLoc);
            //增加相对位移
            loc.XPos += Graph.MapMarginLeftUp.XPos;
            loc.YPos += Graph.MapMarginLeftUp.YPos;
            return loc;
        }
        #endregion
    }
}