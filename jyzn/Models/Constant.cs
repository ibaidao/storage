using System;

namespace Models
{
    /// <summary>
    /// 数据表的主键类型
    /// </summary>
    public enum Generator : byte
    {
        /// <summary>
        /// GUID
        /// </summary>
        Guid,
        /// <summary>
        /// 默认-数据库自带方式
        /// </summary>
        Native,
        /// <summary>
        /// 序列-程序编写全局的序列使用
        /// </summary>
        Sequence,
        /// <summary>
        /// 程序赋值
        /// </summary>
        Assigned
    }

    /// <summary>
    /// 数据表的索引类型
    /// </summary>
    public enum IndexType : byte
    {
        /// <summary>
        /// 普通索引
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 唯一索引
        /// </summary>
        Unique = 2,

        /// <summary>
        /// 全文索引
        /// </summary>
        FullText = 4,

        /// <summary>
        /// 空间索引
        /// </summary>
        Spatial = 8,

        /// <summary>
        /// 多列索引
        /// </summary>
        MultiColumn = 16,

        /// <summary>
        /// 聚集索引
        /// </summary>
        Clustered = 32
    }

    /// <summary>
    /// 设备的实时状态
    /// </summary>
    public enum RealDeviceStatus : byte
    {
        /// <summary>
        /// 候命中
        /// </summary>
        Standby = 0x00,

        /// <summary>
        /// 前进遇到障碍
        /// </summary>
        MeetBalk,

        /// <summary>
        /// 取货中
        /// </summary>
        OnGettingShelf ,

        /// <summary>
        /// 运货中
        /// </summary>
        OnMovingShelf,

        /// <summary>
        /// 电量低
        /// </summary>
        LowBattery,

        /// <summary>
        /// 充电中
        /// </summary>
        InCharging,

        /// <summary>
        /// 故障
        /// </summary>
        UnkownTrouble,

        /// <summary>
        /// 失联
        /// </summary>
        MissingConnect
    }

    /// <summary>
    /// 仓库内元素
    /// </summary>
    public enum StoreComponentType : byte
    {
        /// <summary>
        /// 路口交叉点
        /// </summary>
        CrossCorner = 0x00,

        /// <summary>
        /// 货架
        /// </summary>
        Shelf,

        /// <summary>
        /// 拣货台
        /// </summary>
        PickStation,

        /// <summary>
        /// 拣货员
        /// </summary>
        Picker,

        /// <summary>
        /// 充电桩
        /// </summary>
        Charger,

        /// <summary>
        /// 补货台
        /// </summary>
        RestoreStation,

        /// <summary>
        /// 补货员
        /// </summary>
        Restorer,

        /// <summary>
        /// 双向路
        /// </summary>
        BothPath,

        /// <summary>
        /// 单向路
        /// </summary>
        OneWayPath

    }

    /// <summary>
    /// 仓库内元素状态
    /// </summary>
    public enum StoreComponentStatus : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        OK = 0x00,

        /// <summary>
        /// 阻塞
        /// </summary>
        Block,

        /// <summary>
        /// 故障
        /// </summary>
        Trouble 
    }

}
