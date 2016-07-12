using System;

namespace Models
{
    /// <summary>
    /// 补货/拣货台信息表
    /// </summary>
    [Serializable]
    [PrimaryKey("ID", Generator.Native)]
    public sealed class Station
    {
        /// <summary>
        /// 补货/拣货台ID
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 补货/拣货台序号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 绝对坐标X，Y
        /// </summary>
        public String Location { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Int16 Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public Int16 Type { get; set; }

        /// <summary>
        /// 订单容量
        /// </summary>
        public Int16 OrderCapacity { get; set; }
    }
}