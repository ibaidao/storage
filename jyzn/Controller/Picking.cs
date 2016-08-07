﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Controller
{
    /// <summary>
    /// 应用层 拣货相关功能
    /// </summary>
    public class Picking
    {
        private static bool istanceFlag = false;

        /// <summary>
        /// 执行对具体商品的拣货
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="skuId"></param>
        /// <param name="productId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public ErrorCode PickProduct(int orderId, int skuId, int productId, int deviceId)
        {
            short productCount = 1;
            ErrorCode result;

            BLL.Orders bllOrder = new BLL.Orders();

            result = bllOrder.UpdateRealOrder(orderId, skuId, productId, productCount, deviceId);
            if (result == ErrorCode.OK)
            {
                Protocol proto = new Protocol();
                proto.FunList = new List<Function>() { new Function() { 
                    Code = FunctionCode.PickerAskForOrder 
                } };

                result = Core.Communicate.SendBuffer2Server(proto);
            }

            return result;
        }

        /// <summary>
        /// 为拣货员初始化订单列表
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="orderCount"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public ErrorCode StartingPickOrders(int staffId, int stationId, int orderCount)
        {
            Protocol proto = new Protocol()
            {
                FunList = new List<Function>() { new Function() { 
                    Code = FunctionCode.PickerAskForOrder,
                    TargetInfo = stationId
                } }
            };
            return Core.Communicate.SendBuffer2Server(proto);
        }

        /// <summary>
        /// 开始监听服务器通信（由于测试用例会实例化本实体，所以没写在静态构造函数中）
        /// </summary>
        public static void StartListenCommunicate(Action<string> handlerAfterReciveOrder)
        {
            if (istanceFlag) throw new Exception(ErrorDescription.ExplainCode(ErrorCode.SingleInstance));

            Core.Communicate.StartListening(Models.StoreComponentType.PickStation);
            istanceFlag = true;
            
            while (true)
            {
                if (Core.GlobalVariable.InteractQueue.Count == 0)
                {
                    System.Threading.Thread.Sleep(1000);//每秒检查队列一次，定时模式可改为消息模式
                    continue;
                }
                if (handlerAfterReciveOrder != null)
                {
                    string strInfo = DecodeProtocolInfo(Core.GlobalVariable.InteractQueue.Dequeue());
                    handlerAfterReciveOrder(strInfo);
                }
            }
        }

        /// <summary>
        /// 解码信息
        /// </summary>
        /// <param name="protoInfo"></param>
        /// <returns>翻译为字符串</returns>
        private static string DecodeProtocolInfo(Protocol protoInfo)
        {
            string result = string.Empty;
            Function funInfo = protoInfo.FunList[0];
            switch (funInfo.Code)
            {
                case FunctionCode.SystemProductInfo:
                    byte[] shelfLoc = Core.Coder.ConvertLocations2ByteArray(funInfo.PathPoint, 1, 4, 20);
                    byte[] nameLoc = Core.Coder.ConvertLocations2ByteArray(funInfo.PathPoint, 5, 5, 30);
                    string strShelfLoc = Encoding.ASCII.GetString(shelfLoc);
                    string strName = Encoding.ASCII.GetString(nameLoc);
                    int productLoc = funInfo.PathPoint[0].XPos;
                    result = string.Format("{0};{1};{2}", productLoc, strShelfLoc, strName);
                    break;

                default: break;
            }

            return result;
        }
    }
}
