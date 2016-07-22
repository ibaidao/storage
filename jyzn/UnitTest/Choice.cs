using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class Choice
    {
        [TestMethod]
        public void GetShelves()
        {
            Models.Logic.Choice c = new Models.Logic.Choice();
            int orderId = c.GetOrderNew(1);
            List<int> orderList = new List<int>();
            orderList.Add(orderId);

            Core.Location target = new Core.Location(11, 20, 33);
            c.GetShelves(target, orderList);
        }

        public void GetDevice()
        {

            new Models.Logic.Choice().FindClosestShelf(new RealDevice()
            {
                Location = "11,20,33",
                IPAddress = "aaaaa",
                Remarks = "dddddd",
                Status = 0
            });
        }
    }
}
