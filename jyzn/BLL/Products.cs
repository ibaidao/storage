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
