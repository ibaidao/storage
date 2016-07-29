using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Core
{
    /// <summary>
    /// 小车设备的底层操作
    /// </summary>
    public class Devices
    {
        #region 模拟小车的操作
        public static ErrorCode ReportStatus(List<Function> functionList, string deviceIP)
        {
            Protocol proto = new Protocol();
            proto.NeedAnswer = true;
            proto.FunList = functionList;

            byte[] data = null;
            Coder.EncodeByteData(proto, ref data);

            return Communicate.SendBuffer(deviceIP, data);
        }

        #endregion
    }
}
