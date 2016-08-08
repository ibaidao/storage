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
        /// 更新实时订单数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <param name="productCount"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public Models.ErrorCode UpdateRealOrder(int orderId, int productId, short productCount, int deviceId)
        {
            int tmpId = 0;
            //商品信息表
            Models.Products product = Models.DbEntity.DProducts.GetSingleEntity(productId);
            if (product == null) { return Models.ErrorCode.CannotFindByID; }
            product.Count = (short)(product.Count - 1);
            product.DownShelfTime = DateTime.Now;
            product.Status = product.Count > 0 ? (short)3 : (short)2;
            tmpId = Models.DbEntity.DProducts.Update(product);
            if (tmpId <= 0) { return Models.ErrorCode.DatabaseHandler; }
            // 实时订单表
            string strWhere = string.Format(" OrderID = {0} ", orderId);
            Models.RealOrders realOrder = Models.DbEntity.DRealOrders.GetSingleEntity(strWhere, null);
            if (realOrder == null) { return Models.ErrorCode.CannotFindByID; }
            realOrder.PickProductCount = (short)(realOrder.PickProductCount + productCount);
            realOrder.PickDevices = realOrder.PickDevices + deviceId + ",";
            realOrder.PickProducts = realOrder.PickProducts + string.Format("{0},{1},{2};", product.SkuID, productId, productCount);
            realOrder.Status = realOrder.PickProductCount == realOrder.ProductCount ? (short)2 : (short)1;
            tmpId = Models.DbEntity.DRealOrders.Update(realOrder);
            if (tmpId <= 0) { return Models.ErrorCode.DatabaseHandler; }
            //实时商品表
            strWhere = string.Format(" OrderID={0} AND SkuID={1} ", orderId, product.SkuID);
            Models.RealProducts realProduct = Models.DbEntity.DRealProducts.GetSingleEntity(strWhere, null);
            realProduct.PickProductCount = (short)(realProduct.PickProductCount + 1);
            realProduct.LastTime = DateTime.Now;
            realProduct.Status = realProduct.PickProductCount == realProduct.ProductCount ? (short)0 : (short)1;
            tmpId = Models.DbEntity.DRealProducts.Update(realProduct);
            if (tmpId <= 0) { return Models.ErrorCode.DatabaseHandler; }

            return Models.ErrorCode.OK;
        }
    }
}