using System;

namespace Core
{
    /// <summary>
    /// 功能实体
    /// </summary>
    public class Function
    {
        /// <summary>
        /// 小车设备ID
        /// </summary>
        public Int32 DeviceID
        {
            get;
            set;
        }

        /// <summary>
        /// 目标信息（位置前两字节）
        /// </summary>
        public Int32 TargetInfo
        {
            get;
            set;
        }

        /// <summary>
        /// 数据字节数（自动统计）
        /// </summary>
        public Int16 DataCount
        {
            get;
            set;
        }

        /// <summary>
        /// 功能码
        /// </summary>
        public FunctionCode Code
        {
            get;
            set;
        }

        /// <summary>
        /// 功能名称
        /// </summary>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// 路线上的节点
        /// </summary>
        public System.Collections.Generic.List<Location> PathPoint
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 功能码
    /// </summary>
    public enum FunctionCode:byte
    {
        /// <summary>
        /// 查询设备状态
        /// </summary>
        CheckDeviceStatus = 0x10,

        /// <summary>
        /// 安排充电
        /// </summary>
        OrderCharge = 0x20,

        /// <summary>
        /// 移动到位置等待
        /// </summary>
        OrderMoveToLocation = 0x21,

        /// <summary>
        /// 去找货架
        /// </summary>
        OrderGetShelf = 0x22,

        /// <summary>
        /// 运货架到拣货台
        /// </summary>
        OrderMoveShelfToStation = 0x23,

        /// <summary>
        /// 送回货架到仓储区
        /// </summary>
        OrderMoveShelfBack = 0x24,

        /// <summary>
        /// 当前状态
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
        DeviceUnkownTrouble = 0x39
    }
}
