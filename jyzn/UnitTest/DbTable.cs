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
            object item = DbEntity.DStaff.Insert(new Staff()
            {
                Name = "Suoxd1",
                Sex = true,
                Age = 21,
                Phone = "150150150153",
                Address = "深圳南山1",
                Job = "Software1",
                Auth = "11101"
            });

            int itemID = Convert.ToInt32(item);
            DbEntity.DStaff.Update(new Staff()
            {
                ID = itemID,
                Age = 28
            });

            Staff itemSelect = DbEntity.DStaff.GetSingleEntity(itemID);
            Assert.AreEqual<int>(itemSelect.Age, 28);
            Assert.AreEqual<string>(itemSelect.Phone, "150150150153");

            DbEntity.DStaff.Delete(new Staff()
            {
                ID = itemID
            });
            itemSelect = DbEntity.DStaff.GetSingleEntity(itemID);
            Assert.IsNull(itemSelect);   
        }

        [TestMethod]
        public void RealShelf()
        {
            object item = DbEntity.DRealShelf.Insert(new RealShelf()
            {
                OrderID = "1,2",
                DeviceID = 1,
                GetOrderTime = DateTime.Parse("2016-07-30 10:10:10"),
                GetShelfTime = DateTime.Parse("2016-07-30 10:9:10"),
                ProductID = "1,2;3,1",
                ProductCount = 3,
                ShelfID = 2,
                SkuID = "2,3",
                StationID = "3",
                Status = 1
            });

            int itemID = Convert.ToInt32(item);
            DbEntity.DRealShelf.Update(new RealShelf()
            {
                ID = itemID,
                DeviceID = 10,
                ProductCount=30,
                FinishPickTime = DateTime.Parse("2016-07-30 10:11:10")
            });

            RealShelf itemSelect = DbEntity.DRealShelf.GetSingleEntity(itemID);
            Assert.AreEqual<int>(itemSelect.DeviceID, 10);
            Assert.AreEqual<DateTime>(itemSelect.FinishPickTime, DateTime.Parse("2016-07-30 10:11:10"));

            DbEntity.DRealShelf.Delete(new RealShelf()
            {
                ID = itemID
            }); 
            itemSelect = DbEntity.DRealShelf.GetSingleEntity(itemID);
            Assert.IsNull(itemSelect);
        }

        [TestMethod]
        public void RealOrders()
        {
            object item = DbEntity.DRealOrders.Insert(new RealOrders()
            {
                StaffID = 1,
                ID = 2339,
                ProductCount = 3,
                SkuList = "1,2;3,1",
                Status = 0,
                PickDevices = "1",
                PickProductCount = 2,
                PickProducts = "aaaaaa"
            });

            int itemID = Convert.ToInt32(item);
            DbEntity.DRealOrders.Update(new RealOrders()
            {
                ID = itemID,
                StaffID = 10,
                ProductCount=3
            });

            RealOrders itemSelect = DbEntity.DRealOrders.GetSingleEntity(itemID);
            Assert.AreEqual<int>(itemSelect.StaffID, 10);
            Assert.AreEqual<string>(itemSelect.PickProducts, "aaaaaa");

            DbEntity.DRealOrders.Delete(new RealOrders()
            {
                ID = itemID
            }); 
            itemSelect = DbEntity.DRealOrders.GetSingleEntity(itemID);
            Assert.IsNull(itemSelect);

        }

        [TestMethod]
        public void Shelf()
        {
            object item = DbEntity.DShelf.Insert(new Shelf()
            {
                LocationID = 150,
                Layer = 4,
                Surface = 2,
                Type = 1,
                Code = "02A211",
                Address = "01020201;01020301",
                LocHistory="123,150"
            });

            int itemID = Convert.ToInt32(item);
            DbEntity.DShelf.Update(new Shelf()
            {
                ID = itemID,
                LocationID = 10,
                Layer = 3
            });

            Shelf itemSelect = DbEntity.DShelf.GetSingleEntity(itemID);
            Assert.AreEqual<int>(itemSelect.LocationID, 10);
            Assert.AreEqual<string>(itemSelect.Code, "02A211");

            DbEntity.DShelf.Delete(new Shelf()
            {
                ID = itemID
            });
            itemSelect = DbEntity.DShelf.GetSingleEntity(itemID);
            Assert.IsNull(itemSelect);
        }

        [TestMethod]
        public void Products()
        {
            object skuID = DbEntity.DSkuInfo.Insert(new SkuInfo() {
                 Color="红色",
                 Count = 200,
                 Name = "水杯",
                 Size = "20*200*2000",
                 Type = "300ml",
                 Weight=200
            });

            int intskuID = Convert.ToInt32(skuID);
            DbEntity.DSkuInfo.Update(new SkuInfo()
            {
                ID = intskuID,
                Count = 210,
                Type = "301ml"
            });

            SkuInfo skuSelect = DbEntity.DSkuInfo.GetSingleEntity(intskuID);
            Assert.AreEqual<int>(skuSelect.Count, 210);
            Assert.AreEqual<string>(skuSelect.Color, "红色");
            Assert.AreEqual<string>(skuSelect.Type, "301ml");

            object item = DbEntity.DProducts.Insert(new Products()
            {
                Count = 1,
                Code="fefe",
                SkuID = intskuID,
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

            int itemID = Convert.ToInt32(item);
            DbEntity.DProducts.Update(new Products()
            {
                ID = itemID,
                Count = 10,
                SkuID = 3,
                DownShelfTime = DateTime.Parse("2016-07-30 10:51:50")
            });

            Products itemSelect = DbEntity.DProducts.GetSingleEntity(itemID);
            Assert.AreEqual<int>(itemSelect.Count, 10);
            Assert.AreEqual<DateTime>(itemSelect.DownShelfTime, DateTime.Parse("2016-07-30 10:51:50"));
            Assert.AreEqual<string>(itemSelect.Specification, "20*200*2000");

            DbEntity.DProducts.Delete(new Products()
            {
                ID = itemID
            });
            itemSelect = DbEntity.DProducts.GetSingleEntity(itemID);
            Assert.IsNull(itemSelect);

            DbEntity.DSkuInfo.Delete(new SkuInfo()
            {
                ID = intskuID
            });
            skuSelect = DbEntity.DSkuInfo.GetSingleEntity(intskuID);
            Assert.IsNull(skuSelect);
        }

        [TestMethod]
        public void Device()
        {
            object item = DbEntity.DDevices.Insert(new Models.Devices()
            {
                LocationID = 151,
                IPAddress = "aaaaa",
                Remarks = "dddddd",
                LocationXYZ="1,2,3",
                Status = 0
            });

            int itemID = Convert.ToInt32(item);
            DbEntity.DDevices.Update(new Models.Devices()
            {
                ID = itemID,
                IPAddress = "192.168.10.12",
                LocationID = 10
            });

            Models.Devices itemSelect = DbEntity.DDevices.GetSingleEntity(itemID);
            Assert.AreEqual<int>(itemSelect.LocationID, 10);
            Assert.AreEqual<string>(itemSelect.IPAddress, "192.168.10.12");
            Assert.AreEqual<string>(itemSelect.Remarks, "dddddd");

            DbEntity.DDevices.Delete(new Models.Devices()
            {
                ID = itemID
            });
            itemSelect = DbEntity.DDevices.GetSingleEntity(itemID);
            Assert.IsNull(itemSelect);
        }

        [TestMethod]
        public void StorePoints()
        {
            //节点
            object item = DbEntity.DStorePoints.Insert(new StorePoints()
            {
                Name = "A0",
                Point = "5,0,0",
                StoreID = 1,
                Status = 0,
                Type = 5
            });

            int itemID = Convert.ToInt32(item);
            DbEntity.DStorePoints.Update(new StorePoints()
            {
                ID = itemID,
                Name = "AQ",
                StoreID = 10
            });

            StorePoints itemSelect = DbEntity.DStorePoints.GetSingleEntity(itemID);
            Assert.AreEqual<int>(itemSelect.StoreID, 10);
            Assert.AreEqual<string>(itemSelect.Name, "AQ");
            Assert.AreEqual<string>(itemSelect.Point, "5,0,0");

            DbEntity.DStorePoints.Delete(new StorePoints()
            {
                ID = itemID
            });
            itemSelect = DbEntity.DStorePoints.GetSingleEntity(itemID);
            Assert.IsNull(itemSelect);
        }

        [TestMethod]
        public void StorePaths()
        {
            //插入不存在点的边，会导致地图初始化失败
            object item = DbEntity.DStorePaths.Insert(new StorePaths()
            {
                OnePoint = 1,
                TwoPoint = 5,
                StoreID = 1,
                Type = 3,
                Status = 0
            });

            int itemID = Convert.ToInt32(item);
            DbEntity.DStorePaths.Update(new StorePaths()
            {
                ID = itemID,
                TwoPoint = 10
            });

            StorePaths itemSelect = DbEntity.DStorePaths.GetSingleEntity(itemID);
            Assert.AreEqual<int>(itemSelect.TwoPoint, 10);

            DbEntity.DStorePaths.Delete(new StorePaths()
            {
                ID = itemID
            });
            itemSelect = DbEntity.DStorePaths.GetSingleEntity(itemID);
            Assert.IsNull(itemSelect);
        }

        [TestMethod]
        public void Station()
        {
            object item = DbEntity.DStation.Insert(new Station()
            {
                LocationID = 143,
                Status = (short)Models.StoreComponentStatus.OK,
                Type = (short)Models.StoreComponentType.Charger,
                Code = "Charge"
            });

            int itemID = Convert.ToInt32(item);
            DbEntity.DStation.Update(new Station()
            {
                ID = itemID,
                LocationID = 10,
                Type = (short)Models.StoreComponentType.PickStation
            });

            Station itemSelect = DbEntity.DStation.GetSingleEntity(itemID);
            Assert.AreEqual<int>(itemSelect.LocationID, 10);
            Assert.AreEqual<short>(itemSelect.Type, (short)Models.StoreComponentType.PickStation);

            DbEntity.DStation.Delete(new Station()
            {
                ID = itemID
            });
            itemSelect = DbEntity.DStation.GetSingleEntity(itemID);
            Assert.IsNull(itemSelect);
        }
    }
}