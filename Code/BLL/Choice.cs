using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace BLL
{

    /// <summary>
    /// 选择策略相关模块
    /// </summary>
    public class Choice
    {
        /// <summary>
        /// 找到待选择的对象后的操作
        /// </summary>
        public event Action<object> HandlerAfterChoiceTarget;

        #region 选择订单
        /// <summary>
        /// 为拣货员选择订单
        /// </summary>
        /// <param name="staffID">拣货员ID</param>
        /// <param name="stationID">站台ID</param>
        /// <param name="orderInitCount">初始订单数量</param>
        /// <returns>新订单ID列表</returns>
        public List<Models.Orders> GetOrders4Picker(int staffID, int stationID, int orderInitCount)
        {
            int recordCount = 0, updateIdx;
            string strWhere = string.Format(" Status = {0} ",(int)StoreComponentStatus.OK);
            List<Models.Orders> orderList = DbEntity.DOrders.GetEntityList(strWhere, null, 1, orderInitCount, out recordCount);
            object realID;
            foreach (Models.Orders order in orderList)
            {
                order.PickTime = DateTime.Now;
                order.Picker = staffID;
                order.StationID = stationID;
                order.Status = (short)StoreComponentStatus.FinishWorking;
                updateIdx = DbEntity.DOrders.Update(order);
                if (updateIdx > 0)
                {//订单计入实时订单表
                    realID = DbEntity.DRealOrders.Insert(new RealOrders()
                    {
                        OrderID = order.ID,
                        StaffID = staffID,
                        StationID = stationID,
                        SkuList = order.SkuList,
                        ProductCount = order.productCount,
                        Status = (short)StoreComponentStatus.OK
                    });
                    //订单商品计入实时商品表
                    string[] skuInfos = order.SkuList.Split(';');
                    string[] skuCount;
                    foreach (string skuInfo in skuInfos)
                    {
                        skuCount = skuInfo.Split(',');
                        DbEntity.DRealProducts.Insert(new RealProducts()
                        {
                            OrderID = order.ID,
                            SkuID = int.Parse(skuCount[0]),
                            ProductCount = Int16.Parse(skuCount[1]),
                            StationID = stationID,
                            LastTime = DateTime.Now,
                            Status = (short)StoreComponentStatus.OK
                        });
                    }
                }
            }

            return orderList;
        }
        #endregion 

        #region 选择货架

        /// <summary>
        /// 根据订单列表选择货架
        /// </summary>
        /// <param name="staffPosition"></param>
        /// <param name="orderIds"></param>
        public void GetShelves(int stationId, int[] orderIds)
        {
            if (orderIds == null || orderIds.Length == 0) { Core.Logger.WriteNotice("没有新订单"); return; }

            string strWhere = string.Format(" OrderID IN ({0})", string.Join(",", orderIds));
            List<RealProducts> allSkuInfos = DbEntity.DRealProducts.GetEntityList(strWhere, null);
            //通过拣货中/去拣货台路上的货架，先过滤商品
            List<int> shelfMovingIds = new List<int>();
            lock (GlobalVariable.LockShelfMoving)
            {
                foreach (ShelfTarget shelves in GlobalVariable.ShelvesMoving)
                {//进行过滤的货架
                    if (!shelfMovingIds.Contains(shelves.Shelf.ID))
                        shelfMovingIds.Add(shelves.Shelf.ID);
                }
            }
            FilterSkuByShelves(stationId, allSkuInfos, shelfMovingIds);
            //通过待分配的货架，再过滤商品
            List<int> shelfNeedMoveIds = new List<int>();
            lock (GlobalVariable.LockShelfNeedMove)
            {
                foreach (ShelfTarget shelves in GlobalVariable.ShelvesNeedToMove)
                {//进行过滤的货架
                    if (!shelfNeedMoveIds.Contains(shelves.Shelf.ID))
                        shelfNeedMoveIds.Add(shelves.Shelf.ID);
                }
            }
            FilterSkuByShelves(stationId, allSkuInfos, shelfNeedMoveIds);

            //在移动货架中没找到的商品
            this.GetShelves(stationId, GatherInfoBySkuID(allSkuInfos));
        }

        /// <summary>
        /// 根据商品列表，从仓储区选择货架
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="skuList"></param>
        public void GetShelves(int stationId, List<RealProducts> skuList)
        {
            //剩余商品去仓储区货架中查找
            List<List<int>> shelvesList = this.GetShelfsBySkuID(skuList);
            if (shelvesList == null) { Core.Logger.WriteNotice("库存不足"); return; }
            if (shelvesList.Count == 0) { Core.Logger.WriteLog("已在拣货/到拣货台的货架中"); return; }
            List<int> shelfIds = GetAtomicItems(shelvesList);
            List<Shelf> shelfInfo = GetShelvesInfo(shelfIds);
            Station station = DbEntity.DStation.GetSingleEntity(stationId);
            int idx = GetMinDistanceShelf(shelvesList, shelfInfo, station.LocationID);
            lock (GlobalVariable.LockShelfNeedMove)
            {
                foreach (int i in shelvesList[idx])
                {
                    Shelf shelf = shelfInfo.Find(item => item.ID == i);
                    GlobalVariable.ShelvesNeedToMove.Add(new ShelfTarget(station.ID, station.LocationID, shelf.LocationID, shelf));
                    //生成去拣货台后要拣货的商品（确定货架时就确定货架要拣货的商品，而不是放在抬起货架里，是因为新订单中的商品需要从待移动的货架中过滤）
                    this.GetShelfProducts(station.ID, shelf.ID);
                }
            }
            if (HandlerAfterChoiceTarget != null)
            {
                HandlerAfterChoiceTarget(null);
            }
        }

        #region 私有子函数 - 找可用货架

        /// <summary>
        /// 在货架里过滤商品
        /// </summary>
        /// <param name="stationId">拣货台ID</param>
        /// <param name="allSkuInfos">商品SKU</param>
        /// <param name="shelfList">货架列表</param>
        private void FilterSkuByShelves(int stationId, List<RealProducts> allSkuInfos, List<int> shelfList)
        {
            if (shelfList.Count == 0) return;

            lock (GlobalVariable.LockStationShelf)
            {
                Dictionary<int, int> shelfSkuCount = new Dictionary<int, int>();//<SkuId,SkuCount>
                List<ShelfProduct> shelfProductList = GlobalVariable.StationShelfProduct;
                foreach (RealProducts pickProduct in allSkuInfos)
                {//待过滤Sku
                    if (!shelfSkuCount.ContainsKey(pickProduct.SkuID))
                        shelfSkuCount.Add(pickProduct.SkuID, 0);
                }
                foreach (ShelfProduct shelfSku in shelfProductList)
                {//货架中已分配拣货的Sku总数
                    foreach (Models.Products item in shelfSku.ProductList)
                    {
                        if (shelfSkuCount.ContainsKey(item.SkuID)) shelfSkuCount[item.SkuID]++;
                        else shelfSkuCount.Add(item.SkuID, 1);
                    }
                }
                
                List<string> skuStrList = new List<string>();
                string strShelfIds = string.Join(",", shelfList.ToArray());
                foreach (KeyValuePair<int, int> skuItem in shelfSkuCount)
                {
                    skuStrList.Add(string.Format(" (SkuID={0} AND Count>{1} AND ShelfID IN ({2})) ", skuItem.Key, skuItem.Value, strShelfIds));
                }
                List<Models.Products> productList = DbEntity.DProducts.GetEntityList(string.Join(" OR ", skuStrList.ToArray()), null);
                for (int k = 0; k < allSkuInfos.Count; k++)
                {//执行过滤
                    Models.Products itemProduct = productList.Find(item => item.SkuID == allSkuInfos[k].SkuID);
                    while (0 < allSkuInfos[k].ProductCount - allSkuInfos[k].AsignProductCount)
                    {
                        if (itemProduct == null || itemProduct.Count <= 0)
                        {//一个货架当前SKU已无，换个货架再找下
                            itemProduct = productList.Find(item => item.SkuID == allSkuInfos[k].SkuID);
                            if (itemProduct == null || itemProduct.Count <= 0)
                                break;        //当前货架中不含该Sku
                        }
                        ShelfProduct shelfProduct = shelfProductList.Find(item => item.StationID == stationId && item.ShelfID == itemProduct.ShelfID);
                        if (shelfProduct.ShelfID == 0)
                        {
                            shelfProduct = new ShelfProduct(stationId, itemProduct.ShelfID);
                            shelfProductList.Add(shelfProduct);
                        }
                        shelfProduct.ProductList.Add(itemProduct);
                        shelfProduct.OrderList.Add(allSkuInfos[k].OrderID);//订单
                        itemProduct.Count--;
                        allSkuInfos[k].AsignProductCount++;
                    }
                    if (allSkuInfos[k].AsignProductCount > 0)
                    {
                        DbEntity.DRealProducts.Update(allSkuInfos[k]);
                    }
                }
            }
        }
        
        /// <summary>
        /// 根据SkuID汇总商品列表
        /// </summary>
        /// <param name="skuList"></param>
        /// <returns></returns>
        private List<RealProducts> GatherInfoBySkuID(List<RealProducts> skuList)
        {
            if (skuList == null || skuList.Count == 0) return null;
            List<RealProducts> atomSkuList = new List<RealProducts>();
            foreach (RealProducts product in skuList)
            {
                if (product.AsignProductCount >= product.ProductCount) continue;

                RealProducts item = atomSkuList.Find(idx => idx.SkuID == product.SkuID);
                if (item != null)
                {
                    item.ProductCount += product.ProductCount;
                }
                else
                {
                    atomSkuList.Add(product);
                }
            }

            return atomSkuList;
        }

        /// <summary>
        /// 根据sku列表 获取货架列表
        /// </summary>
        /// <param name="skuList"></param>
        /// <returns></returns>
        private List<List<int>> GetShelfsBySkuID(List<RealProducts> skuList)
        {
            string strSkuId = string.Empty;
            for (int i = 0; i < skuList.Count; i++)
            {
                RealProducts product = skuList[i];
                if (product.PickProductCount < product.ProductCount)
                    strSkuId += product.SkuID + ",";
                else
                {
                    skuList.Remove(product);
                    i--;
                }
            }
            if (strSkuId == string.Empty)
                return new List<List<int>>();

            string strWhere = string.Format(" SkuID IN ({0}) AND Count > 0 ", strSkuId.Remove(strSkuId.Length - 1));
            List<Models.Products> productList = DbEntity.DProducts.GetEntityList(strWhere, null);
            if (productList == null || productList.Count == 0) return null;
            //统计每个货架 对应的商品及数量
            Dictionary<int, Dictionary<int, int>> skuShelf = CountProductByShelf(productList);
            //计算所有满足商品数量的货架组合
            return GetShelvesCombination(skuList, skuShelf);
        }

        /// <summary>
        /// 统计每个货架 对应的商品及数量
        /// </summary>
        /// <param name="productList"></param>
        /// <returns>#int, Dictionary#int, int## = #货架ID, #商品ID, 货架商品数##</returns>
        private Dictionary<int, Dictionary<int, int>> CountProductByShelf(List<Models.Products> productList)
        {
            Dictionary<int, Dictionary<int, int>> productShelf = new Dictionary<int, Dictionary<int, int>>();
            foreach (Models.Products product in productList)
            {
                if (!productShelf.ContainsKey(product.ShelfID))
                {//新货架
                    Dictionary<int, int> shelf = new Dictionary<int, int>();
                    shelf.Add(product.SkuID, product.Count);
                    productShelf.Add(product.ShelfID, shelf);
                }
                else if (!productShelf[product.ShelfID].ContainsKey(product.SkuID))
                {//新商品
                    productShelf[product.ShelfID].Add(product.SkuID, product.Count);
                }
                else
                {//已有货架和商品
                    productShelf[product.ShelfID][product.SkuID] += product.Count;
                }
            }
            return productShelf;
        }

        /// <summary>
        /// 计算所有商品满足数量的货架组合
        /// </summary>
        /// <param name="skuList">商品需求信息</param>
        /// <param name="skuShelf">货架对商品的供应（#int, Dictionary#int, int## = #货架ID, #商品ID, 货架商品数##）</param>
        /// <returns>满足条件的货架组合</returns>
        private List<List<int>> GetShelvesCombination(List<RealProducts> skuList, Dictionary<int, Dictionary<int, int>> skuShelf)
        {
            int num = (int)Math.Pow(2, skuShelf.Count);
            List<int> position = null;
            List<List<int>> result = new List<List<int>>();
            for (int i = 1; i < num; i++)
            {
                Dictionary<int, int> productCount = new Dictionary<int, int>();
                position = GetOnePosition(i);
                //汇总货架中所有商品数量
                GatherProductByShelf(productCount, skuShelf, position);

                if (CheckShelfProductNum(skuList, productCount))
                {//判断货架中商品数量是否满足数量
                    List<int> shelfIds = new List<int>();
                    foreach (int idx in position)
                        shelfIds.Add(skuShelf.ElementAt(idx - 1).Key);
                    result.Add(shelfIds);
                }
            }
            return GetAtomicCollection(result);
        }

        /// <summary>
        /// 获取一个数中所有1的位置
        /// N位数的所有组合，视为1在二进制中的位置
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private List<int> GetOnePosition(int num)
        {
            List<int> result = new List<int>();
            int tmp = num;
            for (int i = 1; tmp > 0; i++)
            {
                if ((tmp & 1) > 0)
                    result.Add(i);
                tmp >>= 1;
            }

            return result;
        }

        /// <summary>
        /// 汇总货架中所有商品数量
        /// </summary>
        /// <param name="productCount">汇总结果（#int,int# = #Sku ID, 数量#）</param>
        /// <param name="skuShelf">当前所有货架商品信息（#int, Dictionary#int, int## = #货架ID, #商品ID, 货架商品数##）</param>
        /// <param name="position">待统计的货架位置</param>
        private void GatherProductByShelf(Dictionary<int, int> productCount, Dictionary<int, Dictionary<int, int>> skuShelf, List<int> position)
        {
            foreach (int idx in position)
            {//统计组合中所有货架的商品数量
                KeyValuePair<int, Dictionary<int, int>> shelf = skuShelf.ElementAt(idx - 1);
                foreach (KeyValuePair<int, int> product in shelf.Value)
                {
                    if (productCount.ContainsKey(product.Key))
                        productCount[product.Key] += product.Value;
                    else
                        productCount.Add(product.Key, product.Value);
                }
            }
        }

        /// <summary>
        /// 判断货架中商品数量是否满足数量
        /// </summary>
        /// <param name="skuInfoList">商品需求信息</param>
        /// <param name="productCount">货架商品汇总#int,int# = #skuID, 当前货架上的数量#</param>
        /// <returns></returns>
        private bool CheckShelfProductNum(List<RealProducts> skuInfoList, Dictionary<int, int> productCount)
        {
            bool tmpFlag = true;
            foreach (RealProducts sku in skuInfoList)
            {//判断是否全部商品满足数量
                if (!productCount.ContainsKey(sku.SkuID) || productCount[sku.SkuID] < sku.ProductCount)
                {
                    tmpFlag = false;
                    break;
                }
            }

            return tmpFlag;
        }

        /// <summary>
        /// 获取集合的原子集
        /// </summary>
        /// <param name="shelfList"></param>
        /// <returns></returns>
        private List<List<int>> GetAtomicCollection(List<List<int>> itemList)
        {//长的不会是短的子集，所以，仅判断短的是否为长的子集
            if (itemList == null || itemList.Count == 0) return null;

            itemList.Sort((List<int> a, List<int> b) => b.Count - a.Count);
            foreach (List<int> item in itemList)
            {
                item.Sort((int a, int b) => b - a);
            }

            List<List<int>> result = new List<List<int>>();
            for (int i = 0; i < itemList.Count; i++)
            {//父集从长向短（从左向右）
                int j = itemList.Count - 1;
                for (; j > i; j--)
                {//子集从短向长（从右向左）
                    if (CheckListFather(itemList[i], itemList[j]))
                    {
                        i++;
                        break;
                    }
                }
                if (i == j)
                {//如果长集合不是任何短集合的父集，则为原子集
                    result.Add(itemList[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// 检验是否一个集合包含另一集合
        /// </summary>
        /// <param name="father"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        private bool CheckListFather(List<int> father, List<int> child)
        {//集合已经是有序的
            bool tmpFlag = true;
            for (int i = 0; i < child.Count; i++)
            {
                if (father[i] != child[i])
                {
                    tmpFlag = false;
                    break;
                }
            }
            return tmpFlag;
        }

        /// <summary>
        /// 获取集合中的所有元素
        /// </summary>
        /// <param name="itemList"></param>
        /// <returns></returns>
        private List<int> GetAtomicItems(List<List<int>> itemList)
        {
            if (itemList == null || itemList.Count == 0) return null;

            List<int> result = new List<int>();
            foreach (List<int> items in itemList)
            {
                foreach (int item in items)
                {
                    if (!result.Contains(item))
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
        #endregion

        #region 私有子函数 - 确定货架
        /// <summary>
        /// 获取货架信息
        /// </summary>
        /// <param name="shelfIds">货架ID</param>
        /// <returns></returns>
        private List<Shelf> GetShelvesInfo(List<int> shelfIds)
        {
            if (shelfIds == null || shelfIds.Count == 0) return null;

            string strWhere = string.Format(" ID IN ({0}) ", string.Join(",", shelfIds.ToArray()));
            List<Shelf> shelfOrigin = DbEntity.DShelf.GetEntityList(strWhere, null);

            return shelfOrigin;
        }

        /// <summary>
        /// 获取最小距离的货架集
        /// </summary>
        /// <param name="shelfIds">备选货架集合</param>
        /// <param name="shelfList">货架信息</param>
        /// <param name="target">拣货员位置（目标坐标）</param>
        /// <returns>选择的货架集合索引</returns>
        private int GetMinDistanceShelf(List<List<int>> shelfCollect, List<Shelf> shelfList, int target)
        {
            //计算每个货架的距离
            Dictionary<int, int> shelfDistance = new Dictionary<int, int>();
            foreach (Shelf shelf in shelfList)
            {
                shelfDistance.Add(shelf.ID, Core.CalcLocation.Manhattan(Core.StoreInfo.GetLocationByPointID(target), Core.StoreInfo.GetLocationByPointID(shelf.LocationID)));
            }
            //计算每个货架集合的总距离
            int[] shelfCollectDistance = new int[shelfCollect.Count];
            for (int i = 0; i < shelfCollect.Count; i++)
            {
                List<int> shelves = shelfCollect[i];
                foreach (int shelf in shelves)
                {
                    shelfCollectDistance[i] += shelfDistance[shelf];
                }
            }
            //计算距离最小的集合，相同距离选择货架最少的，都相同随机（索引靠前的集合）
            int idx = 0;
            for (int i = 1; i < shelfCollectDistance.Length; i++)
            {
                if (shelfCollectDistance[i] < shelfCollectDistance[idx] || shelfCollectDistance[i] == shelfCollectDistance[idx] && shelfCollect[i].Count < shelfCollect[idx].Count)
                {
                    idx = i;
                }
            }
            return idx;
        }

        #endregion
        #endregion

        #region 选择小车设备
        /// <summary>
        /// 获取所有空闲小车设备
        /// </summary>
        /// <returns></returns>
        private List<Models.Devices> GetAllStandbyDevices()
        {
            List<Models.Devices> result = new List<Models.Devices>();
            lock (GlobalVariable.LockRealDevices)
            {
                foreach (Models.Devices device in GlobalVariable.RealDevices)
                {
                    if (device.Status == (short)StoreComponentStatus.OK || device.Status == (short)StoreComponentStatus.Block)
                    {//空闲或充电
                        result.Add(device);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 根据设备返回最近货架
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public ErrorCode FindClosestShelf(Models.Devices device, ref ShelfTarget shelf)
        {
            List<ShelfTarget> shelves = GlobalVariable.ShelvesNeedToMove;
            if (shelves.Count == 0) return ErrorCode.CannotFindUseable;
            Location deviceLocation = Core.StoreInfo.GetLocationByPointID(device.LocationID);
            int idx = 0, minDistance = Core.CalcLocation.Manhattan(deviceLocation, Core.StoreInfo.GetLocationByPointID(shelves[idx].Source));
            for (int i = 1; i < shelves.Count; i++)
            {
                if (minDistance > Core.CalcLocation.Manhattan(deviceLocation, Core.StoreInfo.GetLocationByPointID(shelves[i].Source)))
                {
                    idx = i;
                }
            }
            lock (GlobalVariable.LockShelfNeedMove)
            {
                shelf = shelves[idx];
                shelf.Device = device;
                shelf.Status = StoreComponentStatus.PreWorking;
                shelves.RemoveAt(idx);
            }
            lock (GlobalVariable.LockShelfMoving)
            {
                GlobalVariable.ShelvesMoving.Add(shelf);
            }

            return ErrorCode.OK;
        }

        #endregion

        #region 选择货架和设备

        /// <summary>
        /// 获取一个要搬运的货架和最近的小车
        /// </summary>
        /// <param name="shelf"></param>
        /// <param name="device"></param>
        public void GetCurrentShelfDevice(Action<StoreComponentType,int,int> changeStationStatus, out ShelfTarget? shelf)
        {
            shelf = null;
            //找最近有小车的货架
            Models.Devices device = null;
            int minDistance = int.MaxValue;
            List<Models.Devices> deviceList = this.GetAllStandbyDevices();
            List<ShelfTarget> shelves = GlobalVariable.ShelvesNeedToMove.FindAll(item => item.Device == null);
            if (deviceList.Count == 0 || shelves.Count == 0) return;
            lock (GlobalVariable.LockShelfNeedMove)
            {
                foreach (Models.Devices d in deviceList)
                {
                    foreach (ShelfTarget s in shelves)
                    {
                        if (minDistance > Core.CalcLocation.Manhattan(Core.StoreInfo.GetLocationByPointID(s.Source), Location.DecodeStringInfo(d.LocationXYZ)))
                        {
                            shelf = s;
                            device = d;
                        }
                    }
                }
            }
            if (!shelf.HasValue) return;
            if (device.Status == (short)StoreComponentStatus.Block)
            {//将要离开充电桩，则恢复充电桩的可用状态
                Station charger = GlobalVariable.RealStation.Find(item => item.Type == (short)StoreComponentType.Charger && item.LocationID == device.LocationID);
                charger.Status = (short)StoreComponentStatus.OK;
                if (changeStationStatus != null)
                    changeStationStatus(StoreComponentType.Charger, charger.LocationID, (int)StoreComponentStatus.OK);
            }
            //更新货架信息
            int shelfID = shelf.Value.Shelf.ID;
            ShelfTarget tmpShelf = shelves.Find(item => item.Shelf.ID == shelfID);
            lock (Models.GlobalVariable.LockShelfNeedMove)
            {
                List<ShelfTarget> shelvesAll = GlobalVariable.ShelvesNeedToMove;
                shelvesAll.Remove(tmpShelf);
                tmpShelf = shelf.Value;
                device.Status = (short)StoreComponentStatus.Working;
                device.LocationID = Core.CalcLocation.GetLocationIDByXYZ(device.LocationXYZ);
                tmpShelf.Device = device;
                shelvesAll.Add(tmpShelf);
            }
            shelf = tmpShelf;
        }
        #endregion

        #region 选择充电桩

        /// <summary>
        /// 找最近设备的充电桩
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="station">目标充电桩</param>
        /// <returns></returns>
        public static ErrorCode FindClosestCharger(int deviceID,ref Station station)
        {
            List<Station> stationList = GlobalVariable.RealStation.FindAll(item => item.Status == (short)StoreComponentStatus.OK && item.Type == (short)StoreComponentType.Charger);
            if (stationList == null || stationList.Count == 0) return ErrorCode.CannotFindUseable;

            Models.Devices device = BLL.Devices.GetCurrentDeviceInfoByID(deviceID);
            if (device == null) return ErrorCode.CannotFindByID;

            Location deviceLocation = Core.StoreInfo.GetLocationByPointID(device.LocationID);

            int minDistance = int.MaxValue, tmpDistance = 0;
            foreach (Station item in stationList)
            {
                tmpDistance = Core.CalcLocation.Manhattan(Location.DecodeStringInfo(item.Location), deviceLocation);
                if (tmpDistance < minDistance)
                {
                    station = item;
                    minDistance = tmpDistance;
                }
            }
            return ErrorCode.OK;
        }
        #endregion

        /// <summary>
        /// 货架选定后，根据货架和拣货台，确定待拣商品及数量
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="shelfId"></param>
        public void GetShelfProducts(int stationId, int shelfId)
        {
            //找到订单中  所有未拣商品A
            string strWhere = string.Format(" StationID={0} AND Status IN ({1}) ", stationId, (short)StoreComponentStatus.OK);
            List<RealProducts> productStation = DbEntity.DRealProducts.GetEntityList(strWhere, null);
            if (productStation.Count == 0) return;
            //找到货架中 对应的订单商品B
            strWhere = string.Format(" ShelfID={0} AND SkuID IN (", shelfId);
            foreach (RealProducts sku in productStation)
                strWhere += sku.SkuID + ",";
            strWhere = string.Format("{0}) AND Count > 0 ", strWhere.Remove(strWhere.Length - 1));
            List<Models.Products> productShelf = DbEntity.DProducts.GetEntityList(strWhere, null);
            //首次检查货架时，就将所有要拣商品遍历出来
            ShelfProduct stationShelf = new ShelfProduct(stationId, shelfId);
            List<RealProducts> realProductList = productStation.FindAll(item => item.PickProductCount < item.ProductCount && item.StationID == stationId);
            if (realProductList == null || realProductList.Count == 0) return;
            foreach (RealProducts realProduct in realProductList)
            {
                while (0 < realProduct.ProductCount - realProduct.AsignProductCount)
                {
                    Models.Products product = productShelf.Find(item => item.SkuID == realProduct.SkuID && item.Count > 0);
                    if (product == null) break;
                    stationShelf.ProductList.Add(product);
                    stationShelf.OrderList.Add(realProduct.OrderID);
                    product.Count--;
                    realProduct.AsignProductCount++;
                }
                if (realProduct.AsignProductCount > 0)
                {
                    DbEntity.DRealProducts.Update(realProduct);
                }
            }
            if (stationShelf.ProductList.Count > 0)
            {
                Models.GlobalVariable.StationShelfProduct.Add(stationShelf);
            }
        }

        /// <summary>
        /// 货架到拣货台后，根据扫码商品和站台，确定拣货订单
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="shelfId"></param>
        /// <param name="productCode"></param>
        /// <param name="productId"></param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public int GetProductsOrder(int stationId,int shelfId, string productCode,out int productId, out int skuId)
        {
            productId = -1;
            skuId = -1;
            int orderId = -1;
            Models.Products product = null;
            ShelfProduct stationShelf = Models.GlobalVariable.StationShelfProduct.Find(item => item.StationID == stationId && item.ShelfID == shelfId);
            if(stationShelf.ProductList != null)
                product = stationShelf.ProductList.Find(item => item.Code == productCode);
            
            if (product != null)
            {
                productId = product.ID;
                skuId = product.SkuID;
                //可以同时拣多个商品进行优化
                orderId= stationShelf.OrderList[stationShelf.ProductList.IndexOf(product)];
            }
            return orderId;
        }
    }
}
