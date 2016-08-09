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
            List<Orders> initOrder = c.GetOrders4Picker(1, 1, 1);
            int[] orderIds = new int[initOrder.Count];
            int i = 0;
            foreach (Models.Orders orderInfo in initOrder)
            {
                orderIds[i++] = orderInfo.ID;
            }
            c.GetShelves(1, orderIds);
        }

        [TestMethod]
        public void GetDevice()
        {
            Utilities.Singleton<Core.StoreInfo>.GetInstance();

            Station item = null;
            BLL.Choice.FindClosestCharger(1, ref item);

            ShelfTarget itemShelf = new ShelfTarget();
            BLL.Choice choice = new BLL.Choice ();

            choice.FindClosestShelf(new Devices()
            {
                LocationID = 151,
                IPAddress = "aaaaa",
                Remarks = "dddddd",
                Status = 0
            }, ref itemShelf);
        }
    }
}