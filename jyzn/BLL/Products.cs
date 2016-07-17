using System;
using System.Collections.Generic;
using Models;

namespace BLL
{
    /// <summary>
    /// 商品相关的底层逻辑
    /// </summary>
    public class Products
    {
        /// <summary>
        /// 为拣货员分配新订单
        /// </summary>
        /// <param name="staffID">拣货员ID</param>
        /// <returns>订单ID</returns>
        public int GetNewOrder(int staffID)
        {
            RealOrders order =DbEntity.DRealOrders.First();
            return order.ID;
        }
    }
}
