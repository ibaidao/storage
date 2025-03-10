﻿using System;

namespace Models
{
    /// <summary>
    /// 货架信息表
    /// </summary>
    [Serializable]
    [PrimaryKey("ID", Generator.Native)]
    public sealed class Shelf
    {
        /// <summary>
        /// 货架ID
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 货架编码
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 坐标位置索引
        /// </summary>
        public Int32 LocationID { get; set; }

        /// <summary>
        /// 位置历史
        /// </summary>
        public String LocHistory { get; set; }

        /// <summary>
        /// 货架层数
        /// </summary>
        public Int16 Layer { get; set; }

        /// <summary>
        /// 货架面数
        /// </summary>
        public Int16 Surface { get; set; }

        /// <summary>
        /// 各面每层格数
        /// </summary>
        public String Address { get; set; }

        /// <summary>
        /// 货架类型
        /// </summary>
        public Int16 Type { get; set; }
    }
}
