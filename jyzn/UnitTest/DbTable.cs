using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class DbTable
    {
        [TestMethod]
        public void Users()
        {
            Assert.AreEqual(1, 1);

            DbEntity.DUser.Delete(" 1=1 ", null);

            DbEntity.DUser.Insert(new Staff() 
            {
                Name = "Suoxd",
                Sex=true,
                Age=27,
                Phone="150150150150",
                Address = "深圳南山",
                Job="Software",
                Auth="1110"
            });

            List<Staff> userList = DbEntity.DUser.GetEntityList();
        }
    }
}
