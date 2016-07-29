using System;
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

            return SendMessgeToDevice(FunctionCode.OrderCharge, deviceID, chargerID, station.LocationID);
        }

        /// <summary>
        /// 设备移动到指定位置
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="targetID">目标位置ID</param>
        /// <returns></returns>
        public ErrorCode Move2Position(int deviceID, int targetID)
        {
            return SendMessgeToDevice(FunctionCode.OrderCharge, deviceID, 0, targetID);
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

            return SendMessgeToDevice(FunctionCode.OrderGetShelf,deviceID,shelfID,shelf.LocationID);
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

            return SendMessgeToDevice(FunctionCode.OrderGetShelf, deviceID, stationID, station.LocationID);
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
            return SendMessgeToDevice(FunctionCode.OrderMoveShelfBack, deviceID, shelfID, targetID);
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
            RealDevice device = Models.GlobalVariable.RealDevices.Find(item => item.ID == deviceID);
            if (device == null)
            {
                return ErrorCode.CannotFindByID;
            }

            Protocol proto = new Protocol();
            proto.NeedAnswer = true;

            Function fun = this.MergeFunction(code, targetID, device.LocationID, end);
            List<Function> functionList = new List<Function>();
            functionList.Add(fun);
            //暂时先单个功能发送
            proto.FunList = functionList;
            byte[] data = null;
            Core.Coder.EncodeByteData(proto, ref data);

            return Core.Communicate.SendBuffer(device.IPAddress, data);
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
            foreach (HeadNode node in nodeList)
            {
                function.PathPoint.Add(node.Location);
            }

            return function;
        }
    }
}
