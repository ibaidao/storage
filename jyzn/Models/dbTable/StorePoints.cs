using System;

namespace Models
{
    /// <summary>
    /// 仓库内位置坐标表
    /// </summary>
    [Serializable]
    [PrimaryKey("ID", Generator.Native)]
    public sealed class StorePoints
    {
        /// <summary>
        /// 位置ID
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 坐标名称（前期用于调试）
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 仓库序号
        /// </summary>
        public Int16 StoreID { get; set; }

        /// <summary>
        /// 绝对坐标X，Y，Z
        /// </summary>
        public String Point { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public Int16 Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Int16 Status { get; set; }
    }
}