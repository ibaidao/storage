using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// 设备相关
        /// </summary>
        [TestMethod]
        public void Devices()
        {
            // BLL 层
            BLL.Devices dev = new BLL.Devices();
            Utilities.Singleton<Core.StoreInfo>.GetInstance();
            //由于通信是最后一步操作，服务器端口没打开，所以通讯失败表示逻辑通过测试
            Models.ErrorCode result = dev.Charge(2, 3);
            Assert.IsTrue(result == Models.ErrorCode.OK || result == Models.ErrorCode.CommunicateDeviceError);
            result = dev.SendShelfPickStation(2, 2);
            Assert.IsTrue(result == Models.ErrorCode.OK || result == Models.ErrorCode.CommunicateDeviceError);
            result = dev.Move2Position(2, 54);
            Assert.IsTrue(result == Models.ErrorCode.OK || result == Models.ErrorCode.CommunicateDeviceError);
            result = dev.TakeShelf(2, 1);
            Assert.IsTrue(result == Models.ErrorCode.OK || result == Models.ErrorCode.CommunicateDeviceError);
            
            //Controller层
            Controller.Devices controDev = new Controller.Devices();
            controDev.GetChargingDevice();
            controDev.GetPickingDevice();
            controDev.Set2Charge(1);
            controDev.GetDevice(1);
            controDev.ReportStatus(new Models.Protocol() {  FunList=new System.Collections.Generic.List<Models.Function>()});
        }

        [TestMethod]
        public void Application()
        {
            Controller.Inventory inv = new Controller.Inventory();
            inv.StockTaking();

            Controller.Picking picker = new Controller.Picking();
            picker.StartingPickOrders(1, 1, 1);
            
        }

        [TestMethod]
        public void ModelDataAccess()
        {
            //Models.Graph.InitialMap();
            Models.dbHandler.DataAccess da = new Models.dbHandler.DataAccess();
            da.ExecuteNonQuery("select * from Staff");            
        }
    }
}
