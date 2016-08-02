using System;

namespace Models
{
    /// <summary>
    /// 订单信息表
    /// </summary>
    [Serializable]
    [PrimaryKey("ID", Generator.Native)]
    public sealed class Orders
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// SKU 信息
        /// </summary>
        public String SkuList { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public Int16 Priority { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remarks { get; set; }
    }
}
