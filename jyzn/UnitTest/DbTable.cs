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
            DbEntity.DUser.Delete(" 1=1 ", null);

            DbEntity.DUser.Insert(new Users() 
            {
                Name = "Suoxd",
                Sex=true,
                Age=27,
                Phone="150150150150",
                Address = "深圳南山",
                Job="Software",
                Auth="1110"
            });

            List<Users> userList = DbEntity.DUser.GetEntityList();
        }
    }
}
