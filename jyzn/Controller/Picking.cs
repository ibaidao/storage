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

            BLL.Orders bllOrder = new BLL.Orders();
            Models.RealOrders realOrder = bllOrder.GetRealOrder(orderId);

            realOrder.PickProductCount = (short)(realOrder.PickProductCount + productCount);
            realOrder.PickDevices = realOrder.PickDevices + deviceId + ",";
            realOrder.PickProducts = realOrder.PickProducts + string.Format("{0},{1},{2};", skuId, productId, productCount);
            realOrder.Status = realOrder.PickProductCount == realOrder.ProductCount ? (short)2 : (short)1;

            return bllOrder.UpdateRealOrder(realOrder);
        }

        /// <summary>
        /// 为拣货员初始化订单列表
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="orderCount"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public List<Models.RealOrders> GetStartOrders(int staffId, int stationId, int orderCount)
        {
            BLL.Choice choice = new BLL.Choice();
            BLL.Orders order = new BLL.Orders();

            List<int> orderIds = choice.GetOrders4Picker(staffId, stationId, orderCount);
            List<Models.RealOrders> realOrderList = order.GetRealOrderList(orderIds);

            return realOrderList;
        }

        /// <summary>
        /// 为拣货员新增一张订单
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Models.RealOrders GetNewOrders(int staffId, int stationId)
        {
            Models.RealOrders order = null;

            List<Models.RealOrders> realOrderList = GetStartOrders(staffId, stationId, 1);
            if (realOrderList != null && realOrderList.Count > 0)
                order = realOrderList[0];

            return order;
        }

    }
}
