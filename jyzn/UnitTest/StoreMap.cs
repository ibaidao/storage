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
            ViewServer.Main main = new ViewServer.Main();

            Assert.IsTrue(main.initialFinish);
        }
    }
}
