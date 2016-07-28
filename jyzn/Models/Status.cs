using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 系统状态相关逻辑关系
    /// </summary>
    public class Status
    {
        /// <summary>
        /// 根据小车状态返回小车自身功能码
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static FunctionCode GetDeviceFunctionByStatus(RealDeviceStatus status)
        {
            FunctionCode function;
            switch (status)
            {
                case RealDeviceStatus.LowBattery:
                    function = FunctionCode.DeviceLowBattery;
                    break;
                case RealDeviceStatus.MeetBalk:
                    function = FunctionCode.DeviceMeetBalk;
                    break;
                case RealDeviceStatus.UnkownTrouble:
                    function = FunctionCode.DeviceUnkownTrouble;
                    break;

                case RealDeviceStatus.OnHoldingShelf:
                    function = FunctionCode.DeviceFindHoldShelf;
                    break;
                case RealDeviceStatus.OnPickStation:
                    function = FunctionCode.DeviceGetPickStation;
                    break;
                case RealDeviceStatus.OnFreeShelf:
                    function = FunctionCode.DeviceReturnFreeShelf;
                    break;

                default:
                    function = FunctionCode.DeviceCurrentStatus;
                    break;
            }
            return function;
        }
    }
}
