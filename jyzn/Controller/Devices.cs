using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    /// <summary>
    /// 应用层 设备相关功能
    /// </summary>
    public class Devices
    {
        #region 跟小车的交互
        /// <summary>
        /// 获取指定设备
        /// </summary>
        /// <param name="deviceID"></param>
        public void GetDevice(int deviceID)
        {

        }

        /// <summary>
        /// 获取正在拣货设备
        /// </summary>
        public void GetPickingDevice()
        {

        }

        /// <summary>
        /// 获取正在充电设备
        /// </summary>
        public void GetChargingDevice()
        {

        }

        #endregion

        #region 模拟小车的操作
        private const string SERVER_IP_ADDRESS = "192.168.1.11";
        public bool ReportStatus(List<Core.Function> functionList)
        {
            Core.Protocol proto = new Core.Protocol();
            proto.NeedAnswer = true;
            proto.FunList = functionList;

            byte[] data = null;
            Core.Coder.EncodeByteData(proto, ref data);

            Core.Communicate comm = new Core.Communicate();
            return comm.SendBuffer(SERVER_IP_ADDRESS, data);
        }

        #endregion
    }
}
