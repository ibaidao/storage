﻿using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace BLL
{
    /// <summary>
    /// 小车设备相关的业务层逻辑
    /// </summary>
    public class Devices
    {
        /// <summary>
        /// 安排设备充电
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="chargerID"></param>
        public ErrorCode Charge(int deviceID, int chargerID)
        {
            Station station = DbEntity.DStation.GetSingleEntity(chargerID);
            if (station == null)
            {
                return ErrorCode.CannotFindByID;
            }

            return SendMessgeToDevice(FunctionCode.SystemChargeDevice, deviceID, chargerID, station.LocationID);
        }

        /// <summary>
        /// 设备移动到指定位置
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="targetID">目标位置ID</param>
        /// <returns></returns>
        public ErrorCode Move2Position(int deviceID, int targetID)
        {
            return SendMessgeToDevice(FunctionCode.SystemChargeDevice, deviceID, 0, targetID);
        }

        /// <summary>
        /// 安排设备去找货架
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="shelfID"></param>
        /// <returns></returns>
        public ErrorCode TakeShelf(int deviceID, int shelfID)
        {
            Shelf shelf = Models.GlobalVariable.RealShelves.Find(item => item.ID == shelfID);
            if (shelf == null)
            {
                return ErrorCode.CannotFindByID;
            }

            return SendMessgeToDevice(FunctionCode.SystemSendDevice4Shelf, deviceID, shelfID, shelf.LocationID);
        }

        /// <summary>
        /// 安排设备去找货架
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="shelfID"></param>
        /// <returns></returns>
        public ErrorCode TakeShelf(ShelfTarget shelf)
        {
            if (shelf.Shelf == null)
            {
                return ErrorCode.CannotFindUseable;
            }

            return SendMessgeToDevice(FunctionCode.SystemSendDevice4Shelf, shelf.Device.ID, shelf.Shelf.ID, shelf.Source);
        }

        /// <summary>
        /// 设备运货架到拣货台
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="shelfID"></param>
        /// <returns></returns>
        public ErrorCode SendShelfPickStation(int deviceID, int stationID)
        {
            Station station = DbEntity.DStation.GetSingleEntity(stationID);
            if (station == null)
            {
                return ErrorCode.CannotFindByID;
            }

            //小车状态变为正在使用
            ChangeRealDeviceStatus(deviceID,StoreComponentStatus.Working);

            return SendMessgeToDevice(FunctionCode.SystemSendDevice4Shelf, deviceID, stationID, station.LocationID);
        }

        /// <summary>
        /// 设备将货架移动到仓储区
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="shelfID">货架ID</param>
        /// <param name="targetID">目标位置ID</param>
        /// <returns></returns>
        public ErrorCode Move2Position(int deviceID, int shelfID, int targetID)
        {
            return SendMessgeToDevice(FunctionCode.SystemMoveShelfBack, deviceID, shelfID, targetID);
        }

        /// <summary>
        /// 功能打包发送
        /// </summary>
        /// <param name="code">功能码</param>
        /// <param name="deviceID">设备ID</param>
        /// <param name="targetID">目标对象ID</param>
        /// <param name="end">终点位置ID</param>
        /// <returns></returns>
        private ErrorCode SendMessgeToDevice(FunctionCode code, int deviceID, int targetID, int end)
        {
            Models.Devices device = GetCurrentDeviceInfoByID(deviceID);
            if (device == null)
            {
                return ErrorCode.CannotFindByID;
            }

            Protocol proto = new Protocol() { NeedAnswer = true };
            Function fun = this.MergeFunction(code, targetID, device.LocationID, end);
            Location deviceCurrent = Location.DecodeStringInfo(device.LocationXYZ);
            if (fun.PathPoint[0].XPos != deviceCurrent.XPos || fun.PathPoint[0].YPos != deviceCurrent.YPos || fun.PathPoint[0].ZPos != deviceCurrent.ZPos)
            {//起点没正好在节点上，则先返回节点处
                fun.PathPoint.Insert(0, deviceCurrent);
            }
            List<Function> functionList = new List<Function>();
            functionList.Add(fun);
            //暂时先单个功能发送
            proto.FunList = functionList;
            byte[] data = null;
            Core.Coder.EncodeByteData(proto, ref data);

            return Core.Communicate.SendBuffer2Client(device.IPAddress, data, StoreComponentType.Devices);
        }

        /// <summary>
        /// 整合功能码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="targetId"></param>
        /// <param name="start">起点节点编号</param>
        /// <param name="end">终点</param>
        /// <returns></returns>
        private Function MergeFunction(FunctionCode code, int targetId, int start, int end)
        {
            Function function = new Function();
            function.Code = code;
            function.TargetInfo = targetId;
            function.PathPoint = new List<Location>();

            List<HeadNode> nodeList= Utilities.Singleton<Core.Path>.GetInstance().GetGeneralPath(start, end);
            if (nodeList == null || nodeList.Count == 0)
            {
                throw new Exception("节点之间不可达");
            }
            foreach (HeadNode node in nodeList)
            {
                function.PathPoint.Add(node.Location);
            }

            return function;
        }

        /// <summary>
        /// 获取设备当前信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static Models.Devices GetCurrentDeviceInfoByID(int deviceId)
        {
            Models.Devices device;
            lock (GlobalVariable.LockRealDevices)
            {
                device = Models.GlobalVariable.RealDevices.Find(item => item.ID == deviceId);
            }
            return device;
        }

        /// <summary>
        /// 修改实时设备状态
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static void ChangeRealDeviceStatus(int deviceId, StoreComponentStatus status)
        {
            lock (GlobalVariable.LockRealDevices)
            {
                Models.Devices device = Models.GlobalVariable.RealDevices.Find(item => item.ID == deviceId);
                device.Status = (short)status;
            }
        }

        /// <summary>
        /// 修改实时设备位置
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="locId"></param>
        public static void ChangeRealDeviceLocation(int deviceId, int locId)
        {
            lock (GlobalVariable.LockRealDevices)
            {                
                Models.Devices device = Models.GlobalVariable.RealDevices.Find(item => item.ID == deviceId);
                device.LocationID = locId;
                device.LocationXYZ = Models.GlobalVariable.AllMapPoints[locId].ToString();
            }
        }
    }
}
