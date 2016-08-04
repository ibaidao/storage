using System;

namespace Models
{
    /// <summary>
    /// 员工行为记录（拣货/补货/登入/登出）表
    /// </summary>
    [Serializable]
    public sealed class LogStaff
    {
        /// <summary>
        /// 自增序号
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        public Int32 StaffID { get; set; }

        /// <summary>
        /// 行为类型
        /// </summary>
        public Int16 Type { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public Int32 ProductID { get; set; }

        /// <summary>
        /// SKU ID
        /// </summary>
        public Int32 SkuID { get; set; }

        /// <summary>
        /// 所在站台ID
        /// </summary>
        public Int16 StationID { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime ActionTime { get; set; }
    }
}