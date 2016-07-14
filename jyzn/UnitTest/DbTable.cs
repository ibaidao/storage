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
        public void Staff()
        {
            DbEntity.DStaff.Insert(new Staff()
            {
                Name = "Suoxd",
                Sex = true,
                Age = 27,
                Phone = "150150150150",
                Address = "深圳南山",
                Job = "Software",
                Auth = "1110"
            });

            List<Staff> staffList = DbEntity.DStaff.GetEntityList();
            Core.Logger.WriteLog(staffList.Count.ToString ());

            DbEntity.DStaff.Delete(new Staff()
            {
                ID = 3
            });


            DbEntity.DStaff.Update(new Staff()
            {
                ID = 5,
                Age = 28
            });
        }


    }
}
