using System;

namespace Models
{
    /// <summary>
    /// 实时（拣货/补货）商品表
    /// </summary>
    [Serializable]
    [PrimaryKey("ID", Generator.Native)]
    [IndexKey(Models.IndexType.Normal, new string[] { "StationID" })]
    public sealed class RealProducts
    {
        /// <summary>
        /// 自增序号
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 站台ID
        /// </summary>
        public Int32 StationID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public Int32 OrderID { get; set; }

        /// <summary>
        /// Sku ID
        /// </summary>
        public Int32 SkuID { get; set; }

        /// <summary>
        /// 商品待拣总数
        /// </summary>
        public Int16 ProductCount { get; set; }

        /// <summary>
        /// 已完成拣货数量
        /// </summary>
        public Int16 PickProductCount { get; set; }
       
        /// <summary>
        /// 状态
        /// </summary>
        public Int16 Status { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastTime { get; set; }
    }
}