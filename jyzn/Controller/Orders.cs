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
        /// 获取指定订单
        /// </summary>
        /// <param name="orderID"></param>
        public void GetOrder(int orderID)
        {

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
        public Models.ErrorCode ImportOneOrder(string orderCode, string skuInfo)
        {
            object itemID = Models.DbEntity.DOrders.Insert(new Models.Orders()
            {
                Code = orderCode,
                SkuList = skuInfo,
                 CreateTime=DateTime.Now
            });
            return Convert.ToInt32(itemID) > 0 ? Models.ErrorCode.OK : Models.ErrorCode.DatabaseHandler;
        }
    }
}
