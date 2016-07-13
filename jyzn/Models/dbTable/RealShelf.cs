using System;

namespace Models
{
    /// <summary>
    /// 实时移动货架表
    /// </summary>
    [Serializable]
    public sealed class RealShelf
    {
        /// <summary>
        /// 货架ID
        /// </summary>
        public Int32 ShelfID { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public Int32 DeviceID { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public Int16 ProductCount { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public String ProductID { get; set; }

        /// <summary>
        /// SKU ID
        /// </summary>
        public String SkuID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public String OrderID { get; set; }

        /// <summary>
        /// 拣货台 ID
        /// </summary>
        public String StationID { get; set; }

        /// <summary>
        /// 开始取货时间
        /// </summary>
        public DateTime GetOrderTime { get; set; }

        /// <summary>
        /// 到达货架时间
        /// </summary>
        public DateTime GetShelfTime { get; set; }

        /// <summary>
        /// 搬起货架时间
        /// </summary>
        public DateTime StartTransTime { get; set; }

        /// <summary>
        /// 送达货架时间
        /// </summary>
        public DateTime SentShelfTime { get; set; }

        /// <summary>
        /// 完成拣货时间
        /// </summary>
        public DateTime FinishPickTime { get; set; }

        /// <summary>
        /// 送回货架时间
        /// </summary>
        public DateTime ReturnShelfTime { get; set; }

        /// <summary>
        /// 取货状态
        /// </summary>
        public Int16 Status { get; set; }
    }
}