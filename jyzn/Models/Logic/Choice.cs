using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Logic
{
    /// <summary>
    /// 选择策略相关模块
    /// </summary>
    public class Choice
    {
        /// <summary>
        /// 为拣货员分配新订单
        /// </summary>
        /// <param name="staffID">拣货员ID</param>
        /// <returns>新订单ID</returns>
        public int GetOrderNew(int staffID)
        {
            int orderId = -1;
            RealOrders order = DbEntity.DRealOrders.GetSingleEntity(" Status = 0 ", null);
            if (order != null)
                orderId = order.ID;

            return orderId;
        }

        /// <summary>
        /// 为拣货员初始化订单
        /// </summary>
        /// <param name="staffID">拣货员ID</param>
        /// <returns>新订单ID列表</returns>
        public List<int> GetOrderInitial(int staffID)
        {
            int recordCount = 0;
            List<int> orderIds = new List<int>();
            List<RealOrders> orderList = DbEntity.DRealOrders.GetEntityList(" Status = 0 ", null, 1, 4, out recordCount);
            foreach (RealOrders order in orderList)
            {
                orderIds.Add(order.ID);
            }

            return orderIds;
        }


        private List<Products> GetProductsByOrderID(int orderId)
        {

        }
    }
}
