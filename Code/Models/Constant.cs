﻿using System;

namespace Models
{
    #region 数据库
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
    #endregion 

    #region 仓库

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
        OneWayPath,

        /// <summary>
        /// 仓库自身
        /// </summary>
        StoreSelf,

        /// <summary>
        /// 仓库比例尺
        /// </summary>
        StoreRatio,

        /// <summary>
        /// 设备
        /// </summary>
        Devices,

        /// <summary>
        /// 运着货架的设备
        /// </summary>
        ShelfDevice,

        /// <summary>
        /// 总控系统
        /// </summary>
        MainSystem

    }
    
    /// <summary>
    /// 仓库内元素状态
    /// </summary>
    public enum StoreComponentStatus : byte
    {
        /// <summary>
        /// 正常/空闲
        /// </summary>
        OK = 0x00,

        /// <summary>
        /// 准备工作
        /// </summary>
        PreWorking = 0x02,

        /// <summary>
        /// 工作中/处理中
        /// </summary>
        Working = 0x03,

        /// <summary>
        /// 完成本职工作
        /// </summary>
        AfterWorking,

        /// <summary>
        /// 已完成
        /// </summary>
        FinishWorking = 0x05,

        /// <summary>
        /// 阻塞
        /// </summary>
        Block = 0x08,

        /// <summary>
        /// 故障
        /// </summary>
        Trouble = 0x09
    }

    /// <summary>
    /// 命令功能码
    /// </summary>
    public enum FunctionCode : byte
    {
        /// <summary>
        /// 无任何命令
        /// </summary>
        NONE = 0x00,

        /// <summary>
        /// 查询设备状态
        /// </summary>
        CheckDeviceStatus = 0x10,

        /// <summary>
        /// 系统默认回执
        /// </summary>
        SystemDefaultFeedback = 0x11,

        /// <summary>
        /// 命令小车停止移动
        /// </summary>
        SystemStopDeviceMove = 0x12,

        /// <summary>
        /// 命令小车转动方向
        /// </summary>
        SystemTurnDeviceDirection = 0x13,

        /// <summary>
        /// 安排充电
        /// </summary>
        SystemChargeDevice = 0x20,

        /// <summary>
        /// 移动到位置等待
        /// </summary>
        SystemMoveDevice2Location = 0x21,

        /// <summary>
        /// 去找货架
        /// </summary>
        SystemSendDevice4Shelf = 0x22,

        /// <summary>
        /// 运货架到拣货台
        /// </summary>
        SystemMoveShelf2Station = 0x23,

        /// <summary>
        /// 送回货架到仓储区
        /// </summary>
        SystemMoveShelfBack = 0x24,

        /// <summary>
        /// 告知小车，是否可以前行
        /// </summary>
        SystemDeviceMoveForward = 0x25,

        /// <summary>
        /// 当前状态/心跳
        /// </summary>
        DeviceCurrentStatus = 0x30,

        /// <summary>
        /// 电量低
        /// </summary>
        DeviceLowBattery = 0x31,

        /// <summary>
        /// 遇到障碍
        /// </summary>
        DeviceMeetBalk = 0x32,

        /// <summary>
        /// 超载
        /// </summary>
        DeviceOverload = 0x33,

        /// <summary>
        /// 货物不稳
        /// </summary>
        DeviceUnStable = 0x34,

        /// <summary>
        /// 未知异常
        /// </summary>
        DeviceUnkownTrouble = 0x39,

        /// <summary>
        /// 小车开始充电
        /// </summary>
        DeviceStartCharging = 0x40,

        /// <summary>
        /// 小车找到并抬起货架
        /// </summary>
        DeviceFindHoldShelf = 0x41,

        /// <summary>
        /// 小车将货架运到拣货台
        /// </summary>
        DeviceGetPickStation = 0x42,

        /// <summary>
        /// 小车运货架到仓储区指定位置，并放下货架
        /// </summary>
        DeviceReturnFreeShelf = 0x43,

        /// <summary>
        /// 小车收到取货架命令，开始去找货架
        /// </summary>
        DeviceRecevieOrder4Shelf = 0x44,

        /// <summary>
        /// 小车询问是否可以前行
        /// </summary>
        DeviceAskMoveForward = 0x45,

        /// <summary>
        /// 拣货员汇报当前拣货状态
        /// </summary>
        PickerReportStatus = 0x50,

        /// <summary>
        /// 拣货员申请新订单
        /// </summary>
        PickerAskForOrder = 0x51,

        /// <summary>
        /// 拣货员扫码商品
        /// </summary>
        PickerFindProduct = 0x52,

        /// <summary>
        /// 拣货员将商品放入订单箱
        /// </summary>
        PickerPutProductOrder = 0x53,

        /// <summary>
        /// 拣货员结束拣货
        /// </summary>
        PickerStopWorking = 0x55,

        /// <summary>
        /// 系统询问拣货台状态（确认是否可以接受新订单）
        /// </summary>
        SystemAskPickerStatus = 0x60,

        /// <summary>
        /// 上位机为拣货员分配订单
        /// </summary>
        SystemAssignOrders = 0x61,

        /// <summary>
        /// 上位机告诉拣货员 当前商品对应订单
        /// </summary>
        SystemProductOrder = 0x62,

        /// <summary>
        /// 上位机告诉拣货员 货架待拣商品对应信息
        /// </summary>
        SystemProductInfo = 0x63,

        /// <summary>
        /// 上位机告诉拣货台，将商品放入订单箱操作是否成功
        /// </summary>
        SystemPickerResult = 0x64
    }
    #endregion
}
