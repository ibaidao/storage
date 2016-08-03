using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    /// <summary>
    /// 商品相关
    /// </summary>
    public class Products
    {
        /// <summary>
        /// 获取所有商品SKU信息
        /// </summary>
        /// <returns></returns>
        public List<Models.SkuInfo> GetAllProducts()
        {
            return Models.DbEntity.DSkuInfo.GetEntityList();
        }
    }
}
