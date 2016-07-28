using System;

namespace Models
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
}
