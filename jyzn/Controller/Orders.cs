using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    /// <summary>
    /// 应用层 订单相关功能
    /// </summary>
    public class Orders
    {
        /// <summary>
        /// 获取指定实时订单
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Models.RealOrders GetRealOrder(int orderID)
        {
            BLL.Orders order = new BLL.Orders();
            return order.GetRealOrder(orderID);
        }

        /// <summary>
        /// 获取实时订单列表
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public List<Models.RealOrders> GetRealOrderList(List<int> orderIds)
        {
            if (orderIds == null || orderIds.Count == 0) return null;

            BLL.Orders order = new BLL.Orders();
            return  order.GetRealOrderList(orderIds);
        }

        /// <summary>
        /// 获取正在拣货订单
        /// </summary>
        public void GetPickingOrder()
        {

        }

        /// <summary>
        /// 导入一张新订单
        /// </summary>
        /// <param name="orderCode">订单编号</param>
        /// <param name="skuInfo">商品列表</param>
        /// <param name="productCount">商品总数</param>
        /// <returns></returns>
        public Models.ErrorCode ImportOneOrder(string orderCode, string skuInfo, short productCount)
        {
            BLL.ImportData import = new BLL.ImportData();
            return import.ImportOneOrder(orderCode, skuInfo, productCount);
        }

        /// <summary>
        /// 为拣货员初始化订单列表
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="orderCount"></param>
        /// <returns></returns>
        public List<int> GetStartOrders(int staffId,int orderCount)
        {
            BLL.Choice choice = new BLL.Choice();
            return choice.GetOrders4Picker(staffId, orderCount);
        }

        /// <summary>
        /// 为拣货员新增一张订单
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public int GetNewOrders(int staffId)
        {
            int orderId = -1;

            List<int> orderIds = GetStartOrders(staffId, 1);
            if (orderIds != null && orderIds.Count > 0)
                orderId = orderIds[0];

            return orderId;
        }
    }
}