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

        /// <summary>
        /// 通过订单ID获取Sku列表
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private List<SkuInfo> GetProductsByOrderID(List<int> orderId)
        {
            string strWhere = string.Format(" ID IN ({0}) ",string.Join(",", orderId.ToArray ()));
            List<RealOrders> orderList = DbEntity.DRealOrders.GetEntityList(strWhere, null);
            if (orderList == null) return null;

            List<SkuInfo> skuList = new List<SkuInfo>();
            foreach (RealOrders order in orderList)
            {
                string[] strSkus = order.SkuList.Split(';');
                foreach (string sku in strSkus)
                {
                    string[] strID = sku.Split(',');

                    skuList.Add(new SkuInfo()
                    {
                        ID = int.Parse(strID[0]),
                        Count = int.Parse(strID[1])
                    });
                }
            }
            return skuList;
        }

        /// <summary>
        /// 通过Sku ID 获取货架列表
        /// </summary>
        /// <param name="skuList">Sku 信息</param>
        /// <returns><int, Dictionary<int, int>> = <商品ID, <货架ID, 货架商品数>></returns>
        public Dictionary<int, Dictionary<int, int>> GetShelfBySkuID(List<SkuInfo> skuList)
        {
            string strSkuId = string.Empty;
            foreach (SkuInfo sku in skuList)
            {
                strSkuId += sku.ID + ",";
            }
            Dictionary<string, string> sqlParam = new Dictionary<string, string>(1);
            sqlParam.Add("SkuIds",strSkuId.Remove(strSkuId.Length - 1));
            List<Products> productList = DbEntity.DProducts.GetEntityList(" SkuID IN @(SkuIds) ", sqlParam);
            if (productList == null) return null;

            Dictionary<int, Dictionary<int, int>> skuShelf = ClassifyShelfBySkuId(productList);
            foreach (KeyValuePair<int, Dictionary<int, int>> shelf in skuShelf)
            {

            }
            
        }

        /// <summary>
        /// 根据商品SKU ID分类商品货架
        /// </summary>
        /// <param name="productList"></param>
        /// <returns></returns>
        private Dictionary<int, Dictionary<int, int>> ClassifyShelfBySkuId(List<Products> productList)
        {
            Dictionary<int, Dictionary<int, int>> productShelf = new Dictionary<int, Dictionary<int, int>>();
            foreach (Products product in productList)
            {
                if (!productShelf.ContainsKey(product.SkuID))
                {//新商品
                    Dictionary<int, int> shelf = new Dictionary<int, int>();
                    shelf.Add(product.ShelfID, product.Count);
                    productShelf.Add(product.SkuID, shelf);
                }
                else if (!productShelf[product.SkuID].ContainsKey(product.ShelfID))
                {//新货架
                    productShelf[product.SkuID].Add(product.ShelfID, product.Count);
                }
                else
                {//已有货架
                    productShelf[product.SkuID][product.ShelfID] += product.Count;
                }
            }
            return productShelf;
        }

        /// <summary>
        /// 列举满足商品数量的所有货架组合
        /// </summary>
        public List<List<int>> CombinationAllEnum(int count, Dictionary<int, int> shelf)
        {
            IOrderedEnumerable<KeyValuePair<int, int>> orderShelf =  shelf.OrderBy(s => s.Value);
            //orderShelf[2]
        }

        /// <summary>
        /// 获取一个数中所有1的位置
        /// N位数的所有组合，视为1在二进制中的位置
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<int> GetOnePosition(int num)
        {
            List<int> result = new List<int>();
            int tmp = num;
            for (int i = 1; tmp > 0; i++)
            {
                if ((tmp & 1) > 0)
                    result.Add(i);
                tmp >>= 1;
            }

            return result;
        }



        //private Dictionary<int, List<List<int>>> ExplainProductShelfs(Dictionary<int, Dictionary<int, int>> productShelf)
        //{

        //}
    }
}
