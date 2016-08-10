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
        /// 拣货员扫码商品
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="productCode">商品条码</param>
        /// <param name="productCount"></param>
        /// <returns></returns>
        public ErrorCode FindScanProduct(int stationId, string productCode, int productCount = 1)
        {
            ErrorCode result;

            List<Location> codeLoc = Core.Coder.ConvertByteArray2Locations(Encoding.ASCII.GetBytes(productCode));
            if (codeLoc.Count == 1)
                codeLoc.Add(new Location());
            codeLoc.Add(new Location() { XPos = productCount });

            Protocol proto = new Protocol() { NeedAnswer = true };
            proto.FunList = new List<Function>() { new Function() { 
                    Code = FunctionCode.PickerFindProduct,
                    TargetInfo = stationId, 
                    PathPoint =codeLoc
                } };

            result = Core.Communicate.SendBuffer2Server(proto);

            return result;
        }

        /// <summary>
        /// 拣货员 将拣货商品放入订单
        /// </summary>
        /// <param name="shelfId"></param>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public ErrorCode PickProduct(int shelfId, int orderId, int productId, int skuId)
        {
            Protocol proto = new Protocol() { NeedAnswer = true };
            proto.FunList = new List<Function>() { 
                new Function() { 
                    Code = FunctionCode.PickerPutProductOrder,
                    TargetInfo = shelfId,
                    PathPoint = new List<Location> (){ 
                        new Location(){XPos = orderId, YPos = productId}
                }}
            };
            return Core.Communicate.SendBuffer2Server(proto);
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
                    TargetInfo = staffId,
                     PathPoint = new List<Location> (){new Location(){
                         XPos = stationId,
                         YPos = orderCount
                     }}
                } }
            };
            return Core.Communicate.SendBuffer2Server(proto);
        }

        private Action<FunctionCode, string> handlerAfterReciveMsg;
        /// <summary>
        /// 开始监听服务器通信（由于测试用例会实例化本实体，所以没写在静态构造函数中）
        /// </summary>
        public void StartListenCommunicate(Action<FunctionCode,string> handlerAfterReciveOrder)
        {
            if (istanceFlag) throw new Exception(ErrorDescription.ExplainCode(ErrorCode.SingleInstance));

            Core.Communicate.StartListening(Models.StoreComponentType.PickStation);
            istanceFlag = true;

            handlerAfterReciveMsg = handlerAfterReciveOrder;

            System.Threading.Thread handlerMsg = new System.Threading.Thread(StartHandlerMessage);
            handlerMsg.Start();
        }


        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        private void StartHandlerMessage()
        {
            while (true)
            {
                if (Core.GlobalVariable.InteractQueue.Count == 0)
                {
                    System.Threading.Thread.Sleep(1000);//每秒检查队列一次，定时模式可改为消息模式
                    continue;
                }

                if (handlerAfterReciveMsg != null)
                {
                    Protocol info = Core.GlobalVariable.InteractQueue.Dequeue();
                    handlerAfterReciveMsg(info.FunList[0].Code, DecodeProtocolInfo(info));
                }
            }
        }

        /// <summary>
        /// 解码信息(多余的一层，用于隔离View和逻辑)
        /// </summary>
        /// <param name="protoInfo"></param>
        /// <returns>翻译为字符串</returns>
        private static string DecodeProtocolInfo(Protocol protoInfo)
        {
            string result = string.Empty;
            Function funInfo = protoInfo.FunList[0];
            switch (funInfo.Code)
            {
                case Models.FunctionCode.SystemAssignOrders://分配订单
                    result = funInfo.TargetInfo.ToString();
                    if (funInfo.PathPoint != null && funInfo.PathPoint.Count > 0)
                    {
                        for (int i = 0; i < funInfo.PathPoint.Count; i++)
                        {
                            result = string.Format("{0};{1},{2}", result, funInfo.PathPoint[i].XPos, funInfo.PathPoint[i].YPos);
                        }
                    }
                    break;
                case FunctionCode.SystemProductInfo:
                    byte[] shelfLoc = Core.Coder.ConvertLocations2ByteArray(funInfo.PathPoint, 1, 4, 20);
                    byte[] nameLoc = Core.Coder.ConvertLocations2ByteArray(funInfo.PathPoint, 5, 6, 30);
                    string strShelfLoc = Encoding.ASCII.GetString(shelfLoc);
                    string strName = Encoding.ASCII.GetString(nameLoc);
                    int productLoc = funInfo.PathPoint[0].XPos;
                    int shelfId = funInfo.TargetInfo;
                    result = string.Format("{0};{1};{2};{3}", shelfId,productLoc, strShelfLoc, strName);
                    break;
                case FunctionCode.SystemProductOrder:
                    result = string.Format("{0},{1},{2}", funInfo.TargetInfo, funInfo.PathPoint[0].XPos, funInfo.PathPoint[0].YPos);
                    break;
                case FunctionCode.SystemPickerResult:
                    string strError = Encoding.ASCII.GetString(Core.Coder.ConvertLocations2ByteArray(funInfo.PathPoint,0,6,30));
                    result = string.Format("{0},{1}", funInfo.TargetInfo, strError);
                    break;
                default: break;
            }

            return result;
        }
    }
}
