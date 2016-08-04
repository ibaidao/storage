using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// 订单相关逻辑处理
    /// </summary>
    public class Orders
    {
        /// <summary>
        /// 获取实时订单信息
        /// </summary>
        /// <param name="orderIds">订单ID</param>
        /// <returns></returns>
        public List<Models.RealOrders> GetRealOrderList(List<int> orderIds)
        {
            if (orderIds == null || orderIds.Count == 0) return null;

            string strOrderIds = string.Join(",", orderIds.ToArray<int>());
            string strWhere = string.Format(" OrderID in ({0}) ", strOrderIds);

            return Models.DbEntity.DRealOrders.GetEntityList(strWhere, null);
        }

        /// <summary>
        /// 获取一个实时订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Models.RealOrders GetRealOrder(int orderId)
        {
            string strWhere = string.Format(" OrderID = {0} ", orderId);

            return Models.DbEntity.DRealOrders.GetSingleEntity(strWhere, null);
        }

        /// <summary>
        /// 更新实时订单数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="skuId"></param>
        /// <param name="productId"></param>
        /// <param name="productCount"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public Models.ErrorCode UpdateRealOrder(int orderId, int skuId, int productId, short productCount, int deviceId)
        {
            Models.ErrorCode code = Models.ErrorCode.CannotFindUseable;

            Models.RealOrders realOrder = this.GetRealOrder(orderId);

            realOrder.PickProductCount = (short)(realOrder.PickProductCount + productCount);
            realOrder.PickDevices = realOrder.PickDevices + deviceId + ",";
            realOrder.PickProducts = realOrder.PickProducts + string.Format("{0},{1},{2};", skuId, productId, productCount);
            realOrder.Status = realOrder.PickProductCount == realOrder.ProductCount ? (short)2 : (short)1;

            if (realOrder != null)
                code =  Models.DbEntity.DRealOrders.Update(realOrder)>0?Models.ErrorCode.OK: Models.ErrorCode.DatabaseHandler;

            return code;
        }
    }
}