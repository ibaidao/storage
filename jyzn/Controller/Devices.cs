using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Controller
{
    /// <summary>
    /// 应用层 设备相关功能
    /// </summary>
    public class Devices
    {
        #region 系统跟小车的交互

        public void Set2PickShelf()
        {
            ShelfTarget? shelf = null;
            RealDevice device = null;
            BLL.Choice choice = new BLL.Choice();
            choice.GetCurrentShelfDevice(out shelf, out device);
            //安排小车去取货架
            
        }

        /// <summary>
        /// 获取指定设备
        /// </summary>
        /// <param name="deviceID"></param>
        public void GetDevice(int deviceID)
        {

        }

        /// <summary>
        /// 获取正在拣货设备
        /// </summary>
        public void GetPickingDevice()
        {

        }

        /// <summary>
        /// 获取正在充电设备
        /// </summary>
        public void GetChargingDevice()
        {

        }

        /// <summary>
        /// 安排小车取充电
        /// </summary>
        /// <param name="deviceID"></param>
        public ErrorCode Set2Charge(int deviceID)
        {
            BLL.Devices device = new BLL.Devices();
            Station station = null;
            
            ErrorCode result = BLL.Choice.FindClosestCharger(deviceID, ref station);
            if (result != ErrorCode.OK)
                return result;

            return device.Charge(deviceID, station.LocationID);
        }

        #endregion

        #region 模拟小车
        /// <summary>
        /// 模拟小车发包
        /// </summary>
        /// <param name="proto"></param>
        /// <returns></returns>
        public ErrorCode ReportStatus(Protocol proto)
        {
            //服务器IP
            proto.DeviceIP = "192.168.1.105";
            return Core.Communicate.SendBuffer2Server(proto);
        }
        #endregion
    }
}
