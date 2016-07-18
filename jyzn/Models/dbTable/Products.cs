using System;

namespace Models
{
    /// <summary>
    /// 商品信息表
    /// </summary>
    [Serializable]
    [PrimaryKey("ID", Generator.Native)]
    public sealed class Products
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 货架面号
        /// </summary>
        public Int16 SurfaceNum { get; set; }

        /// <summary>
        /// 库位号
        /// </summary>
        public Int16 CellNum { get; set; }

        /// <summary>
        /// SKU ID
        /// </summary>
        public Int32 SkuID { get; set; }

        /// <summary>
        /// 货架ID
        /// </summary>
        public Int32 ShelfID { get; set; }

        /// <summary>
        /// 商品名称及规格
        /// </summary>
        public String ProductName { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// 过期日期
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 尺寸规格(mm)
        /// </summary>
        public String Specification { get; set; }

        /// <summary>
        /// 重量(g)
        /// </summary>
        public Decimal Weight { get; set; }

        /// <summary>
        /// 上架时间
        /// </summary>
        public DateTime UpShelfTime { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime DownShelfTime { get; set; }

        /// <summary>
        /// 商品状态
        /// </summary>
        public Int16 Status { get; set; }
    }
}
