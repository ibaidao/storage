using System;

namespace Models
{
    /// <summary>
    /// 实时设备位置表
    /// </summary>
    [Serializable]
    public sealed class RealDevice
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Int16 Status { get; set; }

        /// <summary>
        /// 绝对位置
        /// </summary>
        public String Location { get; set; }

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime CurrentTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remarks { get; set; }
    }
}