using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    /// <summary>
    /// 应用层 拣货相关功能
    /// </summary>
    public class Picking
    {
        private const string SERVER_IP = "192.168.1.105";

        /// <summary>
        /// 执行对具体商品的拣货
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="skuId"></param>
        /// <param name="productId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public Models.ErrorCode PickProduct(int orderId, int skuId, int productId, int deviceId)
        {
            short productCount = 1;
            Models.ErrorCode result;

            BLL.Orders bllOrder = new BLL.Orders();

            result = bllOrder.UpdateRealOrder(orderId, skuId, productId, productCount, deviceId);
            if (result == Models.ErrorCode.OK)
            {//跟服务器建立连接，用来识别当前客户端的IP端口
                Models.Protocol proto = new Models.Protocol();
                proto.FunList = new List<Models.Function>() { new Models.Function() { 
                    Code = Models.FunctionCode.PickerStartWork 
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
        public Models.ErrorCode StartingPickOrders(int staffId, int stationId, int orderCount, out List<Models.RealOrders> realOrderList)
        {
            realOrderList = null;
            //跟服务器建立连接，用来识别当前客户端的IP端口
            Models.Protocol proto = new Models.Protocol()
            {
                DeviceIP = SERVER_IP,
                FunList = new List<Models.Function>() { new Models.Function() { 
                    Code = Models.FunctionCode.PickerStartWork,
                } }
            };
            Models.ErrorCode result = Core.Communicate.SendBuffer2Server(proto);

            if (result == Models.ErrorCode.OK)
            {
                BLL.Choice choice = new BLL.Choice();
                BLL.Orders order = new BLL.Orders();

                List<int> orderIds = choice.GetOrders4Picker(staffId, stationId, orderCount);
                realOrderList = order.GetRealOrderList(orderIds);
            }

            return result;
        }

        /// <summary>
        /// 为拣货员新增一张订单
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Models.ErrorCode RestartNewOrders(int staffId, int stationId, out Models.RealOrders orderInfo)
        {
            orderInfo = null;

            Models.ErrorCode result = Models.ErrorCode.OK;
            BLL.Choice choice = new BLL.Choice();
            BLL.Orders order = new BLL.Orders();

            List<int> orderIds = choice.GetOrders4Picker(staffId, stationId, 1);
            if (orderIds == null || orderIds.Count == 0)
            {
                result = Models.ErrorCode.CannotFindUseable;
            }
            else
            {
                orderInfo = order.GetRealOrder(orderIds[0]);
            }

            return result;
        }
    }
}
