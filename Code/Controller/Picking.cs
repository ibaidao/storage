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
        /// 拣货员汇报当前状态
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="stationId"></param>
        /// <param name="orderCount">总订单数</param>
        /// <param name="freeCount">空闲订单数</param>
        /// <returns></returns>
        public ErrorCode ReportStatus(int staffId, int stationId, int orderCount,int freeCount)
        {
            Protocol proto = new Protocol()
            {
                FunList = new List<Function>() { new Function() { 
                    Code = FunctionCode.PickerReportStatus,
                    TargetInfo = staffId,
                     PathPoint = new List<Location> (){new Location(){
                         XPos = stationId,
                         YPos = orderCount,
                         ZPos = freeCount
                     }}
                } }
            };
            return Core.Communicate.SendBuffer2Server(proto);
        }

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
            byte[] byteCode = Encoding.ASCII.GetBytes(productCode);
            List<Location> codeLoc = Core.Coder.ConvertByteArray2Locations(byteCode);
            List<Location> msgInfo = new List<Location>() { new Location() { XPos = byteCode.Length, YPos = productCount } };
            foreach (Location loc in codeLoc)
                msgInfo.Add(loc);
            //发送
            Protocol proto = new Protocol() { NeedAnswer = true };
            proto.FunList = new List<Function>() { new Function() { 
                    Code = FunctionCode.PickerFindProduct,
                    TargetInfo = stationId, 
                    PathPoint = msgInfo
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

        /// <summary>
        /// 拣货员结束拣货
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="orderCount"></param>
        /// <returns></returns>
        public ErrorCode EndingPickOrders(int staffId, int stationId)
        {
            Protocol proto = new Protocol()
            {
                FunList = new List<Function>() { new Function() { 
                    Code = FunctionCode.PickerStopWorking,
                    TargetInfo = stationId,
                     PathPoint = new List<Location> (){new Location(){
                         XPos = staffId
                     }}
                } }
            };
            return Core.Communicate.SendBuffer2Server(proto);
        }

        private Action<FunctionCode, string[]> handlerAfterReciveMsg;
        /// <summary>
        /// 开始监听服务器通信（由于测试用例会实例化本实体，所以没写在静态构造函数中）
        /// </summary>
        public void StartListenCommunicate(Action<FunctionCode,string[]> handlerAfterReciveOrder)
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
                if (Models.GlobalVariable.InteractQueue.Count == 0)
                {
                    System.Threading.Thread.Sleep(1000);//每秒检查队列一次，定时模式可改为消息模式
                    continue;
                }

                if (handlerAfterReciveMsg != null)
                {
                    Protocol info = Models.GlobalVariable.InteractQueue.Dequeue();
                    handlerAfterReciveMsg(info.FunList[0].Code, DecodeProtocolInfo(info));
                }
            }
        }

        /// <summary>
        /// 解码信息(多余的一层，用于隔离View和逻辑)
        /// </summary>
        /// <param name="protoInfo"></param>
        /// <returns>翻译为字符串</returns>
        private static string[] DecodeProtocolInfo(Protocol protoInfo)
        {
            string[] result = new string[protoInfo.FunList.Count];
            Function funInfo = protoInfo.FunList[0];
            switch (funInfo.Code)
            {
                case Models.FunctionCode.SystemAssignOrders://分配订单
                    result[0] = string.Empty;
                    for (int i = 0; funInfo.PathPoint != null && i < funInfo.PathPoint.Count; i++)
                    {
                        result[0] += string.Format("{0},{1};", funInfo.PathPoint[i].XPos, funInfo.PathPoint[i].YPos);
                    }
                    result[0] = result[0] != string.Empty ? result[0].Remove(result[0].Length - 1) : string.Empty;
                    break;
                case FunctionCode.SystemProductInfo:
                    result[0] = DecodeProductInfo(funInfo);
                    break;
                case FunctionCode.SystemProductOrder:
                    result[0] = string.Format("{0},{1},{2}", funInfo.TargetInfo, funInfo.PathPoint[0].XPos, funInfo.PathPoint[0].YPos);
                    break;
                case FunctionCode.SystemPickerResult:
                    string strError = string.Empty;
                    if (funInfo.TargetInfo != (int)StoreComponentStatus.OK)
                    {
                        int errByteLen = funInfo.PathPoint[0].XPos;
                        int errLocLen = errByteLen / 5 + (errByteLen % 5 == 0 ? 0 : 1);
                        strError = Encoding.Unicode.GetString(Core.Coder.ConvertLocations2ByteArray(funInfo.PathPoint, 1, errLocLen, errByteLen));
                    }
                    result[0] = string.Format("{0},{1}", funInfo.TargetInfo, strError);
                    if (protoInfo.FunList.Count > 1) result[1] = DecodeProductInfo(protoInfo.FunList[1]);
                    break;
                case FunctionCode.SystemAskPickerStatus:
                    result[0] = string.Empty;
                    break;
                default: break;
            }

            return result;
        }

        /// <summary>
        /// 解码商品信息
        /// </summary>
        /// <param name="funInfo"></param>
        /// <returns></returns>
        private static string DecodeProductInfo(Function funInfo)
        {
            Location byteLen = funInfo.PathPoint[1];
            int shelfLen = byteLen.XPos / 5 + (byteLen.XPos % 5 == 0 ? 0 : 1), 
                nameLen = byteLen.YPos / 5 + (byteLen.YPos % 5 == 0 ? 0 : 1),
                codeLen = byteLen.ZPos/5 + (byteLen.ZPos % 5 ==0?0:1);
            byte[] shelfLoc = Core.Coder.ConvertLocations2ByteArray(funInfo.PathPoint, 2, shelfLen, byteLen.XPos);
            byte[] codeLoc = Core.Coder.ConvertLocations2ByteArray(funInfo.PathPoint, 2 + shelfLen, codeLen, byteLen.ZPos);
            byte[] nameLoc = Core.Coder.ConvertLocations2ByteArray(funInfo.PathPoint, 2 + shelfLen + codeLen, nameLen, byteLen.YPos);
            string strShelfLoc = Encoding.ASCII.GetString(shelfLoc);
            string strCode = Encoding.ASCII.GetString(codeLoc);
            string strName = Encoding.Unicode.GetString(nameLoc);
            int productLoc = funInfo.PathPoint[0].XPos;
            int productFlag = funInfo.PathPoint[0].ZPos;
            int shelfId = funInfo.TargetInfo;

            return string.Format("{0};{1};{2};{3};{4};{5}", shelfId, productFlag, productLoc,strCode, strShelfLoc, strName);
        }
    }
}
