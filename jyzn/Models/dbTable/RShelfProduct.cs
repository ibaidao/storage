using System;

namespace Models
{
    /// <summary>
    /// 商品货架关系表
    /// </summary>
    [Serializable]
    [PrimaryKey("ID", Generator.Native)]
    public sealed class RShelfProduct
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// SKU ID
        /// </summary>
        public String SkuID { get; set; }

        /// <summary>
        /// 货架ID
        /// </summary>
        public String ShelfID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Int16 Status { get; set; }
    }
}