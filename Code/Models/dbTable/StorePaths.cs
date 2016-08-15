using System;

namespace Models
{
    /// <summary>
    /// 仓库路径表
    /// </summary>
    [Serializable]
    [PrimaryKey("ID", Generator.Native)]
    public sealed class StorePaths
    {
        /// <summary>
        /// 路径ID
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 仓库序号
        /// </summary>
        public Int16 StoreID { get; set; }

        /// <summary>
        /// 位置1 ID
        /// </summary>
        public Int32 OnePoint { get; set; }

        /// <summary>
        /// 位置2 ID
        /// </summary>
        public Int32 TwoPoint { get; set; }

        /// <summary>
        /// 边权重
        /// </summary>
        public Int16 Weight { get; set; }

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