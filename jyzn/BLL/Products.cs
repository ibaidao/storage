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
        /// <returns>新订单ID</returns>
        public int GetNewOrder(int staffID)
        {
            RealOrders order =DbEntity.DRealOrders.GetSingleEntity(" Status == 0 ",null);

            return order.ID;
        }

        /// <summary>
        /// 为拣货员初始化订单
        /// </summary>
        /// <param name="staffID">拣货员ID</param>
        /// <returns>新订单ID列表</returns>
        public List<int> GetInitialOrder(int staffID)
        {
            int recordCount = 0;
            List<int> orderIds = new List<int>();
            List<RealOrders> orderList = DbEntity.DRealOrders.GetEntityList(" Status == 0 ", null, 1, 10,out recordCount);
            foreach (RealOrders order in orderList)
            {
                orderIds.Add(order.ID);
            }

            return orderIds;
        }
    }
}
