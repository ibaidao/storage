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

        [TestMethod]
        public void StorePoints()
        {
            //节点
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A0", Point = "5,0,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B0", Point = "10,0,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C0", Point = "15,0,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "X0", Point = "0,5,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "XA", Point = "5,5,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "XB", Point = "10,5,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "XC", Point = "15,5,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "X1", Point = "20,5,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "Y0", Point = "0,10,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "YA", Point = "5,10,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "YB", Point = "10,10,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "YC", Point = "15,10,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "Y1", Point = "20,10,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "Z0", Point = "0,15,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "ZA", Point = "5,15,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "ZB", Point = "10,15,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "ZC", Point = "15,15,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "Z1", Point = "20,15,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A1", Point = "5,20,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B1", Point = "10,20,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C1", Point = "15,20,0", StoreID = 1, Status = 0, Type = 5 });
        }

        [TestMethod]
        public void StorePaths()
        {
            //边
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 1, TwoPoint = 5, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 2, TwoPoint = 6, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 3, TwoPoint = 7, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 4, TwoPoint = 5, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 5, TwoPoint = 6, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 6, TwoPoint = 7, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 7, TwoPoint = 8, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 5, TwoPoint = 10, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 6, TwoPoint = 11, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 7, TwoPoint = 12, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 9, TwoPoint = 10, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 10, TwoPoint = 11, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 11, TwoPoint = 12, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 12, TwoPoint = 13, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 10, TwoPoint = 15, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 11, TwoPoint = 16, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 12, TwoPoint = 17, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 14, TwoPoint = 15, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 15, TwoPoint = 16, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 16, TwoPoint = 17, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 17, TwoPoint = 18, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 15, TwoPoint = 19, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 16, TwoPoint = 20, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 17, TwoPoint = 21, StoreID = 1, Type = 3, Status = 0 });



        }
    }
}
