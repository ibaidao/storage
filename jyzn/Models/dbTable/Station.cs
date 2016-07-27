using System;

namespace Models
{
    /// <summary>
    /// 仓库内站点（补货/拣货台/充电桩）信息表
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
        /// 仓库ID
        /// </summary>
        public Int16 StoreID { get; set; }

        /// <summary>
        /// 位置索引
        /// </summary>
        public Int32 LocationID { get; set; }

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
        /// 备注
        /// </summary>
        public String Remarks { get; set; }
    }
}