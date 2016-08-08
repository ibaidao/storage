using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 一个货架在一个拣货台的待拣商品信息
    /// </summary>
    public struct ShelfProduct
    {
        /// <summary>
        /// 拣货台ID
        /// </summary>
        public int StationID;

        /// <summary>
        /// 货架ID
        /// </summary>
        public int ShelfID;

        /// <summary>
        /// 所有当前需要拣出的商品
        /// </summary>
        public List<Products> ProductList;
        
        /// <summary>
        /// 商品及对应订单号
        /// </summary>
        public List<int> OrderList;

        public ShelfProduct(int station, int shelf)
        {
            StationID = station;
            ShelfID = shelf;
            ProductList = new List<Products>();
            OrderList = new List<int>();
        }

    }
}
