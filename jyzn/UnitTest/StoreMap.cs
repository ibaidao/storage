using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Models;

namespace UnitTest
{
    /// <summary>
    /// 地图相关
    /// </summary>
    [TestClass]
    public class StoreMap
    {
        /// <summary>
        /// 初始化地图
        /// </summary>
        [TestMethod]
        public void InitialMapData()
        {
            //ViewServer.Main main = new ViewServer.Main();
            //Assert.IsTrue(main.initialFinish);

            Controller.StoreMap store = new Controller.StoreMap(null, null);
            store.ChangePointType(StoreComponentType.RestoreStation, 101);

            Assert.AreEqual<int>(store.GetMapNodeByData(101).Data, 101);
            int data;
            //多次重复测试，同一点不允许添加是合理的
            ErrorCode result = store.RealtimeAddPoint("UT", new Location(1, 1, 1), out data);
            Assert.IsTrue(data > 0 && result == ErrorCode.OK || result == Models.ErrorCode.AddDuplicateItem);
        }
    }
}
