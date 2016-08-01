using System;

namespace Models
{
    /// <summary>
    /// 实时设备位置表
    /// </summary>
    [Serializable]
    [PrimaryKey("ID", Generator.Native)]
    [IndexKey(Models.IndexType.Normal, new string[]{"DeviceID"})]
    public sealed class RealDevice
    {
        /// <summary>
        /// 自增序号
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public Int32 DeviceID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Int16 Status { get; set; }

        /// <summary>
        /// 当前功能
        /// </summary>
        public Int32 FunctionCode { get; set; }

        /// <summary>
        /// 起点位置索引
        /// </summary>
        public Int32 StartLocID { get; set; }

        /// <summary>
        /// 终点位置索引
        /// </summary>
        public Int32 EndLocID { get; set; }

        /// <summary>
        /// 当前位置索引
        /// </summary>
        public Int32 LocationID { get; set; }

        /// <summary>
        /// 当前绝对位置（处于非节点位置）
        /// </summary>
        public string LocationXYZ { get; set; }

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