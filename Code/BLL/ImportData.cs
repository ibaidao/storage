using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// 外部导入数据
    /// </summary>
    public class ImportData
    {
        /// <summary>
        /// 导入一张新订单
        /// </summary>
        /// <param name="orderCode">订单编号</param>
        /// <param name="skuInfo">商品列表</param>
        /// <param name="productCount">商品总数</param>
        /// <returns></returns>
        public Models.ErrorCode ImportOneOrder(string orderCode, string skuInfo, short productCount)
        {
            object itemID = Models.DbEntity.DOrders.Insert(new Models.Orders()
            {
                Code = orderCode,
                SkuList = skuInfo,
                CreateTime = DateTime.Now,
                productCount = productCount
            });

            return Convert.ToInt32(itemID) > 0 ? Models.ErrorCode.OK : Models.ErrorCode.DatabaseHandler;
        }
    }
}
