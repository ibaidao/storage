using System;
using System.Collections.Generic;
using Models;

namespace BLL
{
    /// <summary>
    /// 商品相关的业务层逻辑
    /// </summary>
    public class Products
    {
        /// <summary>
        /// 开始拣货时通过订单创建产品表
        /// </summary>
        /// <param name="realOrderList"></param>
        /// <returns></returns>
        public ErrorCode CreateRealProductsItems(List<RealOrders> realOrderList)
        {
            ErrorCode result = ErrorCode.OK;
            if (realOrderList == null || realOrderList.Count == 0)
                result = ErrorCode.CannotFindUseable;

            object insertIdx;
            foreach (RealOrders order in realOrderList)
            {
                string[] productList = order.SkuList.Split(';');
                foreach (string product in productList)
                {
                    string[] items = product.Split(',');
                    insertIdx = DbEntity.DRealProducts.Insert(new RealProducts()
                    {
                        OrderID = order.OrderID,
                        SkuID = int.Parse(items[0]),
                        ProductCount = short.Parse(items[1]),
                        Status = 1
                    });
                    if (Convert.ToInt32(insertIdx) <= 0)
                    {
                        result = ErrorCode.DatabaseHandler;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 根据货架和订单列表
        /// </summary>
        /// <param name="shelf"></param>
        /// <param name="orderIds"></param>
        public void GetProducts4ShelfOrders(int shelf, List<int> orderIds)
        {

        }

        /// <summary>
        /// 商品入库/上架
        /// </summary>
        /// <param name="skuID">Sku ID</param>
        /// <param name="shelfID">库位ID</param>
        public void PutInBound(int skuID, int shelfID)
        {

        }

        /// <summary>
        /// 商品出库/下架
        /// </summary>
        /// <param name="productID">商品ID</param>
        /// <param name="count">数量</param>
        public void TackOutBound(int productID, int count)
        {

        }

        /// <summary>
        /// 商品移库
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="count"></param>
        /// <param name="shelfID">目标库位ID</param>
        public void MoveShelf(int productID, int count, int shelfID)
        {

        }
    }
}
