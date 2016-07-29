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
            List<int> initOrder = c.GetOrderInitial(1);

            int orderId = c.GetOrderNew(1);
            List<int> orderList = new List<int>();
            orderList.Add(orderId);

            Location target = new Location(11, 20, 33);
            c.GetShelves(target, orderList);            
        }

        [TestMethod]
        public void GetDevice()
        {
            Utilities.Singleton<Core.StoreInfo>.GetInstance();

            Station item = null;
            BLL.Choice.FindClosestCharger(1, ref item);

            ShelfTarget itemShelf = new ShelfTarget ();
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
