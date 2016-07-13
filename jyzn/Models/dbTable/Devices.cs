using System;

namespace Models
{
    /// <summary>
    /// 设备信息表
    /// </summary>
    [Serializable]
    [PrimaryKey("ID", Generator.Native)]
    public sealed class Devices
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 设备序号
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
    }
}
