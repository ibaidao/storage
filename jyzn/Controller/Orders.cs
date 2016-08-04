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

    }
}