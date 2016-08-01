using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 异常信息描述
    /// </summary>
    public struct ErrorDescription
    {
        #region 常规通用
        public const string OK = "正常";
        public const string UNKOWN_TOUBLE = "未识别的异常";
        public const string CANNOT_FIND_BY_ID = "通过ID索引找不到指定对象";
        public const string CANNOT_FIND_USEABLE = "找不到当前可用设备";
        #endregion
        
        #region 数据库相关
        public const string DATABASE_HANDLER = "数据库执行失败";
        public const string ADD_DUPLICATE_ITEM = "增加重复记录";
        #endregion

        #region 仓库相关
        public const string PATH_WITHIN_ONE_AXIS = "路径不能是斜线";
        public const string PATH_ALREADY_EXISTS = "路径已经存在";
        #endregion

        #region 设备相关
        public const string COMMUNICATE_DEVICE_ERROR = "设备通信失败";
        public const string DEVICE_LOCATION_ERROR = "设备当前位置有误";
        public const string DEVICE_LOW_BATTERY = "设备电量过低";
        public const string DEVICE_OVERLOAD = "设备超载";
        public const string DEVICE_UNSTABLE = "设备货物不稳";
        #endregion

        /// <summary>
        /// 解析错误编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ExplainCode(ErrorCode code)
        {
            string result = string.Empty;
            switch (code)
            {
                case ErrorCode.OK: result = OK; break;
                case ErrorCode.UnKownTrouble: result = UNKOWN_TOUBLE; break;
                case ErrorCode.DatabaseHandler: result = DATABASE_HANDLER; break;
                case ErrorCode.AddDuplicateItem: result = ADD_DUPLICATE_ITEM; break;
                case ErrorCode.PathWithinOneAxis: result = PATH_WITHIN_ONE_AXIS; break;
                case ErrorCode.PathAlreadyExists: result = PATH_ALREADY_EXISTS; break;
                case ErrorCode.CommunicateDeviceError: result = COMMUNICATE_DEVICE_ERROR; break;
                case ErrorCode.CannotFindByID: result = CANNOT_FIND_BY_ID; break;
                case ErrorCode.CannotFindUseable: result = CANNOT_FIND_USEABLE; break;
                case ErrorCode.DeviceLocationError: result = DEVICE_LOCATION_ERROR; break;
                case ErrorCode.DeviceLowBattery: result = DEVICE_LOW_BATTERY; break;
                case ErrorCode.DeviceOverload: result = DEVICE_OVERLOAD; break;
                case ErrorCode.DeviceUnstable: result = DEVICE_UNSTABLE; break;
        
                default: break;
            }
            return result;
        }
    }

    /// <summary>
    /// 异常信息编码
    /// </summary>
    public enum ErrorCode
    {
        #region 常规通用
        /// <summary>
        /// 无异常
        /// </summary>
        OK = 0x00,

        UnKownTrouble,
        CannotFindByID,
        CannotFindUseable,
        #endregion

        #region 数据库相关
        DatabaseHandler = 0x300,
        AddDuplicateItem,
        #endregion

        #region 仓库相关
        PathWithinOneAxis = 0x400,
        PathAlreadyExists,
        #endregion

        #region 设备相关
        CommunicateDeviceError,
        DeviceLocationError,
        DeviceLowBattery,
        DeviceOverload,
        DeviceUnstable
        #endregion
    }
}