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
        /// 商品总数量
        /// </summary>
        public Int16 productCount { get; set; }

        /// <summary>
        /// 状态（0未处理,1已处理）
        /// </summary>
        public Int16 Status { get; set; }

        /// <summary>
        /// 拣货员ID
        /// </summary>
        public Int32 Picker { get; set; }

        /// <summary>
        /// 拣货台ID
        /// </summary>
        public Int32 StationID { get; set; }

        /// <summary>
        /// 导入订单/下单时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 开始拣货时间
        /// </summary>
        public DateTime PickTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remarks { get; set; }
    }
}
