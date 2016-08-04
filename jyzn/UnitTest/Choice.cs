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
            BLL.Choice c = new BLL.Choice();
            List<int> initOrder = c.GetOrders4Picker(1, 1, 1);

            Location target = new Location(11, 20, 33);
            c.GetShelves(target, initOrder);
        }

        [TestMethod]
        public void GetDevice()
        {
            Utilities.Singleton<Core.StoreInfo>.GetInstance();

            Station item = null;
            BLL.Choice.FindClosestCharger(1, ref item);

            ShelfTarget itemShelf = new ShelfTarget();
            BLL.Choice.FindClosestShelf(new RealDevice()
            {
                LocationID = 151,
                IPAddress = "aaaaa",
                Remarks = "dddddd",
                Status = 0
            }, ref itemShelf);
        }
    }
}