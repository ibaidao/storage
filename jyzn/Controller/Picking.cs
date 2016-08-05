using System;
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
        /// 开始监听客户端通信（由于测试用例会实例化本实体，所以没写在静态构造函数中）
        /// </summary>
        public static void StartListenCommunicate(Action<Protocol> handlerAfterReciveOrder)
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
                    handlerAfterReciveOrder(Core.GlobalVariable.InteractQueue.Dequeue());
                }
            }
        }
    }
}
