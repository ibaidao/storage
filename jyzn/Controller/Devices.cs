﻿using System;
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
        #region 跟小车的交互
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
            
            ErrorCode result = BLL.Choice.FindClosestCharger(deviceID, station);
            if (result != ErrorCode.OK)
                return result;

            return device.Charge(deviceID, station.LocationID);
        }

        #endregion

        #region 小车
        /// <summary>
        /// 模拟小车发包
        /// </summary>
        /// <param name="functionList"></param>
        /// <returns></returns>
        public ErrorCode ReportStatus(List<Function> functionList)
        {
            string serverIPAddress = "192.168.1.11";
            return Core.Devices.ReportStatus(functionList, serverIPAddress);
        }
        #endregion
    }
}
