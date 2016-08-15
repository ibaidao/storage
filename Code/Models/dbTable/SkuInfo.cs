using System;

namespace Models
{
    /// <summary>
    /// SKU信息表
    /// </summary>
    [Serializable]
    [PrimaryKey("ID", Generator.Native)]
    public sealed class SkuInfo
    {
        /// <summary>
        /// SKU ID
        /// </summary>
        public Int32 ID { get; set; }
        
        /// <summary>
        /// 商品名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 库存总数量
        /// </summary>
        public Int32 Count { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public String Color { get; set; }

        /// <summary>
        /// 尺寸(mm)
        /// </summary>
        public String Size { get; set; }

        /// <summary>
        /// 重量(g)
        /// </summary>
        public Decimal Weight { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public String Remarks { get; set; }
    }
}
