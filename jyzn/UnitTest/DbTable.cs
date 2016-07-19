using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    class DbTable
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

        [TestMethod]
        public void RealShelf() {
            DbEntity.DRealShelf.Delete(new RealShelf()
            {
                ShelfID=1
            });
        }

        [TestMethod]
        public void RealOrders()
        {
            DbEntity.DRealOrders.Delete(new RealOrders()
            {
                ID = 0
            });

            DbEntity.DRealOrders.Insert(new RealOrders(){
                StaffID=1,
                ID=2234,
                ProductCount=3,
                SkuList="1,2;3,1",
                Status=0
            });
        }

        [TestMethod]
        public void Shelf()
        {
            DbEntity.DShelf.Insert(new Shelf()
            {
                Location = "11,22,33",
                Layer = 4,
                Surface = 2,
                Type = 1,
                Code = "02A211",
                Address = "01020201;01020301"
            });

            DbEntity.DShelf.Insert(new Shelf()
            {
                Location = "11,25,33",
                Layer = 4,
                Surface = 2,
                Type = 1,
                Code = "02A211",
                Address = "01020201;01020301"
            });
        }

        [TestMethod]
        public void Products()
        {

            DbEntity.DProducts.Insert(new Products()
            {
                Count = 1,
                SkuID = 1,
                ShelfID = 2,
                CellNum = 2,
                ProductName = "水杯；红色300ml",
                ProductionDate = DateTime.Parse("2015-07-01"),
                ExpireDate = DateTime.Parse("2016-12-31"),
                Specification = "20*200*2000",
                Weight = 200,
                UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"),
                SurfaceNum = 1,
                Status = 0
            });

            DbEntity.DProducts.Insert(new Products()
            {
                Count = 1,
                SkuID = 3,
                ShelfID = 2,
                CellNum = 2,
                ProductName = "水杯；红色300ml",
                ProductionDate = DateTime.Parse("2015-07-01"),
                ExpireDate = DateTime.Parse("2016-12-31"),
                Specification = "20*200*2000",
                Weight = 200,
                UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"),
                SurfaceNum = 1,
                Status = 0
            });
            DbEntity.DProducts.Insert(new Products()
            {
                Count = 1,
                SkuID = 3,
                ShelfID = 1,
                CellNum = 2,
                ProductName = "水杯；红色300ml",
                ProductionDate = DateTime.Parse("2015-07-01"),
                ExpireDate = DateTime.Parse("2016-12-31"),
                Specification = "20*200*2000",
                Weight = 200,
                UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"),
                SurfaceNum = 1,
                Status = 0
            });
        }

        [TestMethod]
        public void RealDevice()
        {
            DbEntity.DRealDevice.Insert(new RealDevice()
            {
                Location = "11,2,2",
                IPAddress = "aaaaa",
                Remarks = "dddddd",
                Status = 0
            });

        }
    }
}
