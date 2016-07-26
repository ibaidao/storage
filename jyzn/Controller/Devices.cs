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
        public bool CreateProtocol(int deviceId, Core.Location loc, Models.RealDeviceStatus status)
        {
            Core.Protocol proto = new Core.Protocol();
            proto.ByteCount = 17;
            proto.NeedAnswer = true;
            proto.FunList = new List<Core.Function>();
            proto.FunList.Add(new Core.Function() { 
                Code= Models.Logic.Status.GetDeviceFunctionByStatus(status),
                DeviceID = deviceId,
                 Name="小车1",
                  PathPoint = new List<Core.Location> ()
            });

            byte[] data = null;
            Core.Coder.EncodeByteData(proto, ref data);

            Core.Protocol p1=new Core.Protocol ();
            byte[] d1 = new byte[16];
            for (int i = 3; i < 19; i++)
                d1[i - 3] = data[i];

            Core.Coder.DecodeByteData(p1, d1,16);

            Core.Communicate comm = new Core.Communicate();
            return comm.SendBuffer(SERVER_IP_ADDRESS, data);
        }

        #endregion
    }
}
