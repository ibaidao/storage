﻿using System;

namespace Models
{
    /// <summary>
    /// 实时订单表
    /// </summary>
    [Serializable]
    [PrimaryKey("ID", Generator.Native)]
    [IndexKey(Models.IndexType.Normal, new string[] { "OrderID" })]
    public sealed class RealOrders
    {
        /// <summary>
        /// 自增序号
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public Int32 OrderID { get; set; }

        /// <summary>
        /// 商品总数
        /// </summary>
        public Int16 ProductCount { get; set; }

        /// <summary>
        /// 商品信息
        /// </summary>
        public String SkuList { get; set; }

        /// <summary>
        /// 拣货员ID
        /// </summary>
        public Int32 StaffID { get; set; }

        /// <summary>
        /// 拣货台ID
        /// </summary>
        public Int32 StationID { get; set; }

        /// <summary>
        /// 取货设备
        /// </summary>
        public String PickDevices { get; set; }

        /// <summary>
        /// 取货商品
        /// </summary>
        public String PickProducts { get; set; }

        /// <summary>
        /// 取货总数量
        /// </summary>
        public Int16 PickProductCount { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public Int16 Status { get; set; }

        /// <summary>
        /// 拣货备注
        /// </summary>
        public String PickRemark { get; set; }
    }
}