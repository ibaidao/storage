using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Logic
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
        public static Core.FunctionCode GetDeviceFunctionByStatus(RealDeviceStatus status)
        {
            Core.FunctionCode function;
            switch (status)
            {
                case RealDeviceStatus.LowBattery:
                    function = Core.FunctionCode.DeviceLowBattery;
                    break;
                case RealDeviceStatus.MeetBalk:
                    function = Core.FunctionCode.DeviceMeetBalk;
                    break;
                case RealDeviceStatus.UnkownTrouble:
                    function = Core.FunctionCode.DeviceUnkownTrouble;
                    break;

                case RealDeviceStatus.OnHoldingShelf:
                    function = Core.FunctionCode.DeviceFindHoldShelf;
                    break;
                case RealDeviceStatus.OnPickStation:
                    function = Core.FunctionCode.DeviceGetPickStation;
                    break;
                case RealDeviceStatus.OnFreeShelf:
                    function = Core.FunctionCode.DeviceReturnFreeShelf;
                    break;

                default:
                    function = Core.FunctionCode.DeviceCurrentStatus;
                    break;
            }
            return function;
        }
    }
}
