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
        /// 位置索引
        /// </summary>
        public Int32 LocationID { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public String IPAddress { get; set; }

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