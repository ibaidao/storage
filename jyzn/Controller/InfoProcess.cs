using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Models;
using BLL;

namespace Controller
{
    /// <summary>
    /// 中央系统 对接收到信息的处理逻辑
    /// </summary>
    public class InfoProcess
    {
        private Dictionary<int, string> stationIPList = new Dictionary<int, string>();
        private Dictionary<int, string> deviceIPList = new Dictionary<int, string>();

        /// <summary>
        /// 单例检查
        /// </summary>
        private static bool instance = false;
        /// <summary>
        /// 需要在窗体进行提醒的操作
        /// </summary>
        private Action<ErrorCode> warningOnMainWindow = null;
        Action<StoreComponentType, int, Location> updateLocation = null;
        Action<StoreComponentType, int, int> updateColor = null;

        public InfoProcess(Action<ErrorCode> warningShowFun, Action<StoreComponentType, int, Location> updateItemLocation, Action<StoreComponentType, int, int> updateItemColor)
        {
            if (instance) throw new Exception(ErrorDescription.ExplainCode(ErrorCode.SingleInstance));

            instance = true;
            this.warningOnMainWindow = warningShowFun;
            this.updateLocation = updateItemLocation;
            this.updateColor = updateItemColor;

            Thread threadSystemHandler = new Thread(SystemHandlerInfo);
            threadSystemHandler.Start();

            Thread threadAsignDevice = new Thread(SystemAssignDevice);
            threadAsignDevice.Start();
        }

        #region 总监控系统 处理接收消息
        /// <summary>
        /// 系统处理接收的信息
        /// </summary>
        public void SystemHandlerInfo()
        {
            while (true)
            {
                if (Core.GlobalVariable.InteractQueue.Count == 0)
                {
                    Thread.Sleep(1000);//每秒检查队列一次，定时模式可改为消息模式
                    continue;
                }

                AssignTask(Core.GlobalVariable.InteractQueue.Dequeue());
            }
        }

        /// <summary>
        /// 分配操作任务
        /// </summary>
        /// <param name="proto"></param>
        private void AssignTask(Protocol proto)
        {
            if (proto.FunList == null || proto.FunList.Count == 0) return;
            //记录接收到的信息
            Core.Logger.WriteInteract(proto, false);
            Action<Protocol> infoHandler = null;

            switch (proto.FunList[0].Code)
            {
                #region 小车自身
                case FunctionCode.DeviceCurrentStatus: infoHandler = this.DeviceHeartBeat; break;
                case FunctionCode.DeviceLowBattery: infoHandler = this.DeviceLowBattery; break;
                case FunctionCode.DeviceUnkownTrouble: break;
                case FunctionCode.DeviceMeetBalk: break;
                case FunctionCode.DeviceOverload: break;
                #endregion

                #region 小车业务相关
                case FunctionCode.DeviceRecevieOrder4Shelf: infoHandler = this.DeviceGetOrder4Shelf; break;
                case FunctionCode.DeviceFindHoldShelf: infoHandler = this.DeviceFindShelf; break;
                case FunctionCode.DeviceGetPickStation: infoHandler = this.DeviceGetPickStation; break;
                case FunctionCode.DeviceReturnFreeShelf: infoHandler = this.DeviceReturnShelf; break;
                #endregion

                #region 拣货操作
                case FunctionCode.PickerAskForOrder: infoHandler = this.PickerStartWork; break;
                case FunctionCode.PickerFindProduct: infoHandler = this.PickerFindProduct; break;
                case FunctionCode.PickerPutProductOrder: infoHandler = this.PickerPutProductOrder; break;
                #endregion
                default: break;
            }
            infoHandler(proto);
        }

        #region 服务器操作
        /// <summary>
        /// 设备发到系统的心跳包
        /// </summary>
        /// <param name="info">包信息</param>
        private void DeviceHeartBeat(Protocol info)
        {
            //更新最新坐标
            this.UpdateItemLocation(info);
            bool nothingError = true;
            if (false)
            {//位置异常，先停止，再重新规划路线
                nothingError = false;
                Core.Communicate.SendBuffer2Client(new Protocol()
                {
                    DeviceIP = info.DeviceIP,
                    FunList = new List<Function>() 
                    { 
                        new Function() { Code = FunctionCode.SystemStopDeviceMove },
                        //new Function() { Code = FunctionCode.OrderTurnDirection,
                        // TargetInfo}                        
                    }
                }, StoreComponentType.Devices);
                if (warningOnMainWindow != null) this.warningOnMainWindow(ErrorCode.DeviceLocationError);
            }
            if (info.NeedAnswer && nothingError)
            {//正常情况，需要回复则发送回执
                Core.Communicate.SendBuffer2Client(new Protocol()
                {
                    DeviceIP = info.DeviceIP,
                    FunList = new List<Function>() 
                    { 
                        new Function() { Code = FunctionCode.SystemDefaultFeedback }
                    }
                }, StoreComponentType.Devices);
            }
            //更新当前记录
            string strWhere = string.Format(" ID = {0} ",info.FunList[0].TargetInfo);
            int changeCount = DbEntity.DDevices.Update(strWhere, new KeyValuePair<string, string>("IPAddress", info.DeviceIP));
            Models.Devices deviceInfo = BLL.Devices.GetCurrentDeviceInfoByID(info.FunList[0].TargetInfo); 
            lock (GlobalVariable.LockRealDevices)
            {
                if (deviceInfo == null)
                {

                    GlobalVariable.RealDevices.Add(DbEntity.DDevices.GetSingleEntity(info.FunList[0].TargetInfo));
                }
                else
                {
                    deviceInfo.LocationXYZ = info.FunList[0].PathPoint[0].ToString();
                }
            }
        }

        /// <summary>
        /// 设备电量低，告知系统
        /// </summary>
        /// <param name="info">包信息</param>
        private void DeviceLowBattery(Protocol info)
        {
            //更新最新坐标
            this.UpdateItemLocation(info);
            Models.Devices device = Models.GlobalVariable.RealDevices.Find(item => item.IPAddress == info.DeviceIP);
            if (device == null)
            {//警告无法定位设备/信息有误
                if (this.warningOnMainWindow != null) this.warningOnMainWindow(ErrorCode.CannotFindByID);
                return;
            }

            Function fun = new Function() { Code = FunctionCode.SystemDefaultFeedback};
            //if (device.FunctionCode == (int)Models.FunctionCode.SystemChargeDevice ||//已经在充电的路上了
            //    device.FunctionCode == (int)Models.FunctionCode.SystemMoveShelfBack || //在送返货架，则先等设备完成本次任务
            //    device.FunctionCode == (int)Models.FunctionCode.SystemMoveShelf2Station)//在搬货架到拣货台，则先等设备完成本次任务
            //{
            //    fun.Code = FunctionCode.SystemDefaultFeedback;
            //}
            //else if (device.FunctionCode == (int)Models.FunctionCode.SystemSendDevice4Shelf)
            //{//准备去运货架，则中止当前工作，去充电
            //    fun.Code = FunctionCode.SystemChargeDevice;
            //    Station station = null;
            //    if (Choice.FindClosestCharger(device.ID, ref station) == ErrorCode.OK)
            //    {
            //        fun.TargetInfo = station.ID;
            //        List<HeadNode> pathNode = Utilities.Singleton<Core.Path>.GetInstance().GetGeneralPath(device.LocationID, station.LocationID);
            //        fun.PathPoint = new List<Location>(pathNode.Count);
            //        foreach (HeadNode node in pathNode)
            //            fun.PathPoint.Add(node.Location);
            //    }
            //    //安排新空闲小车去搬货架，或者把任务放回任务队列

            //}

            Core.Communicate.SendBuffer2Client(new Protocol()
            {
                DeviceIP = info.DeviceIP,
                FunList = new List<Function>() 
                    { 
                        fun
                    }
            }, StoreComponentType.Devices);
        }

        /// <summary>
        /// 小车收到去取货架命令
        /// </summary>
        /// <param name="info"></param>
        private void DeviceGetOrder4Shelf(Protocol info)
        {
            BLL.Devices.ChangeRealDeviceStatus(info.FunList[0].TargetInfo, StoreComponentStatus.Working);
        }

        /// <summary>
        /// 小车到达指定货架
        /// </summary>
        /// <param name="info"></param>
        private void DeviceFindShelf(Protocol info)
        {
            ShelfTarget shelf = this.GetShelfTargetByDeviceId(info.FunList[0].TargetInfo);
            if (shelf.Shelf == null) return;
            //生成路径
            Protocol shelfPick = new Protocol()
            {
                DeviceIP = shelf.Device.IPAddress,
                FunList = new List<Function>() { new Function() { 
                    TargetInfo = shelf.StationId,
                    Code = FunctionCode.SystemMoveShelf2Station, 
                    PathPoint = this.GetNormalPath( shelf.Source, shelf.Target)
                    } }
            };
            //小车送货架 去拣货台
            ErrorCode code = Core.Communicate.SendBuffer2Client(shelfPick, StoreComponentType.Devices);
            //货架颜色 变为道路颜色
            this.UpdateItemColor(info, StoreComponentType.Shelf, shelf.Shelf.LocationID, 0);
            //小车颜色 变为小车+货架颜色
            this.UpdateItemColor(info, StoreComponentType.ShelfDevice, shelf.Device.ID, shelf.Shelf.ID);
        }

        /// <summary>
        /// 小车到达拣货台
        /// </summary>
        /// <param name="info"></param>
        private void DeviceGetPickStation(Protocol info)
        {
            ShelfTarget shelf = this.GetShelfTargetByDeviceId(info.FunList[0].TargetInfo);
            if (shelf.Shelf == null) return;
            //找到对应货架商品
            Choice choice = new Choice();
            Models.Products product = choice.GetShelfProducts(shelf.StationId, shelf.Device.ID);
            Models.Shelf shelfInfo = DbEntity.DShelf.GetSingleEntity(product.ShelfID);
            //打包信息
            Protocol backInfo = new Protocol() { DeviceIP = info.DeviceIP, FunList = new List<Function>() };
            Function function = new Function()
            {
                Code = FunctionCode.SystemProductInfo,
                TargetInfo = shelf.Shelf.ID,
                PathPoint = new List<Location>() { new Location() { XPos = product.CellNum, YPos = product.ID } }
            };
            backInfo.FunList.Add(function);
            byte[] shelfLoc = Encoding.ASCII.GetBytes(shelfInfo.Address.Split(';')[product.SurfaceNum]);
            byte[] nameLoc = Encoding.ASCII.GetBytes(product.ProductName);
            List<Location> shelfLocList = Core.Coder.ConvertByteArray2Locations(shelfLoc);
            List<Location> nameLocList = Core.Coder.ConvertByteArray2Locations(nameLoc);
            if (shelfLocList.Count > 4 || nameLocList.Count > 5)
            {
                throw new Exception("编码字节位数不够");
            }
            foreach (Location loc in shelfLocList)
                function.PathPoint.Add(loc);
            for (int i = function.PathPoint.Count; i < 1 + 4; i++)
                function.PathPoint.Add(new Location());
            foreach (Location loc in nameLocList)
                function.PathPoint.Add(loc);
            for (int i = function.PathPoint.Count; i < 1 + 4 + 5; i++)
                function.PathPoint.Add(new Location());
            //发送给拣货台
            Core.Communicate.SendBuffer2Client(backInfo, StoreComponentType.PickStation);
        }

        /// <summary>
        /// 小车将 货架运回仓储区
        /// </summary>
        /// <param name="info"></param>
        private void DeviceReturnShelf(Protocol info)
        {
            ShelfTarget shelf = this.GetShelfTargetByDeviceId(info.FunList[0].TargetInfo);
            if (shelf.Shelf == null) return;

            //货架颜色 变为货架颜色
            this.UpdateItemColor(info, StoreComponentType.Shelf, shelf.Shelf.LocationID, 1);
            //小车颜色 变为小车颜色
            this.UpdateItemColor(info, StoreComponentType.Devices, shelf.Device.ID, 0);
            //小车状态变为可用
            BLL.Devices.ChangeRealDeviceStatus(shelf.Device.ID, StoreComponentStatus.OK);
            //分配新的搬运任务
            this.SystemAssignDevice(null);
        }

        /// <summary>
        /// 拣货员开始拣货
        /// 
        /// </summary>
        /// <param name="info"></param>
        private void PickerStartWork(Protocol info)
        {
            Function functionInfo = info.FunList[0];
            BLL.Choice choice = new BLL.Choice();
            choice.HandlerAfterChoiceTarget += SystemAssignDevice;
            BLL.Orders order = new BLL.Orders();
            //回复拣货台订单列表
            List<Models.Orders> orderList = choice.GetOrders4Picker(functionInfo.TargetInfo, functionInfo.PathPoint[0].XPos, functionInfo.PathPoint[0].YPos);
            int[] orderIds = new int[orderList.Count];
            Protocol backInfo = new Protocol()
            {
                DeviceIP = info.DeviceIP,
                NeedAnswer = false,
                FunList = new List<Function>() { new Function() { 
                    TargetInfo = functionInfo.PathPoint[0].YPos >1?1:2,
                    PathPoint = new List<Location> ()
                } }
            };
            int i = 0;
            foreach (Models.Orders orderInfo in orderList)
            {
                orderIds[i++] = orderInfo.ID;
                backInfo.FunList[0].PathPoint.Add(new Location() { XPos = orderInfo.ID, YPos = orderInfo.productCount });
            }
            Core.Communicate.SendBuffer2Client(backInfo, StoreComponentType.PickStation);
            //确定商品货架
            choice.GetShelves(functionInfo.PathPoint[0].XPos, orderIds);
            //更新监控界面
            UpdateItemColor(info, StoreComponentType.PickStation, info.FunList[0].TargetInfo, 1);
        }

        /// <summary>
        /// 拣货员从货架上拿下商品，并扫码
        /// </summary>
        /// <param name="info"></param>
        private void PickerFindProduct(Protocol info)
        {
            byte[] productCode = Core.Coder.ConvertLocations2ByteArray(info.FunList[0].PathPoint, 0, 2, 10);
            string strCode = Encoding.ASCII.GetString(productCode);
            int stationId = info.FunList[0].TargetInfo;

            Choice choice = new Choice();
            int productId, skuId;
            int orderId = choice.GetProductsOrder(stationId, strCode, out productId, out skuId);
            //回复拣货台信息
            Protocol backInfo = new Protocol()
            {
                DeviceIP = info.DeviceIP,
                NeedAnswer = false,
                FunList = new List<Function>() { new Function(){ 
                    TargetInfo=orderId, 
                    Code = FunctionCode.SystemProductOrder,
                    PathPoint = new List<Location> (){ new Location(){ XPos=productId, YPos=skuId }}
                }}
            };
            //发送给拣货台
            Core.Communicate.SendBuffer2Client(backInfo, StoreComponentType.PickStation);
            //检查小车是否上是否还有当前拣货台的商品
            ShelfProduct stationShelf = Models.GlobalVariable.StationShelfProduct.Find(item => item.StationID == stationId);
            if (stationShelf.ProductList.Count == 1)
            {//本次拣货已经是最后一个待拣商品，则扫码后安排小车到仓储区
                ShelfTarget shelf;
                lock (GlobalVariable.LockShelfNeedMove)
                {
                    shelf = Models.GlobalVariable.ShelvesNeedToMove.Find(item => item.Shelf.ID == stationShelf.ShelfID);
                }
                int tmpLoc = shelf.Target; shelf.Target = shelf.Source; shelf.Source = tmpLoc;//将货架放回原来位置
                Protocol backDevice = new Protocol()
                {
                    DeviceIP = shelf.Device.IPAddress,
                    FunList = new List<Function>() { new Function() { 
                        TargetInfo = shelf.Shelf.ID,
                        Code = FunctionCode.SystemMoveShelfBack, 
                        PathPoint = GetNormalPath(shelf.Source,shelf.Target) 
                    } }
                };
            }
        }

        /// <summary>
        /// 拣货员将商品放入订单分拣箱，并关闭电子标签
        /// </summary>
        /// <param name="info"></param>
        private void PickerPutProductOrder(Protocol info)
        {
            ErrorCode result;
            Function funcInfo = info.FunList[0];
            int shelfId = funcInfo.TargetInfo, orderId = funcInfo.PathPoint[0].XPos, productId = funcInfo.PathPoint[0].YPos;
            short productCount = 1;
            //同步拣货数据
            ShelfProduct shelfProduct = Models.GlobalVariable.StationShelfProduct.Find(item => item.ShelfID == shelfId);
            Models.Products product = shelfProduct.ProductList.Find(item => item.ID == productId);
            int productIdx = shelfProduct.ProductList.IndexOf(product);
            shelfProduct.ProductList.RemoveAt(productIdx);
            shelfProduct.OrderList.RemoveAt(productIdx);
            if (shelfProduct.ProductList.Count == 0)
            {//当前货架拣货完成
                Models.GlobalVariable.StationShelfProduct.Remove(shelfProduct);
            }
            //同步数据库记录
            ShelfTarget shelf;
            lock (Models.GlobalVariable.LockShelfMoving)
            {
                List<ShelfTarget> shelvesMove = Models.GlobalVariable.ShelvesMoving;
                shelf = shelvesMove.Find(item => item.Shelf.ID == shelfId);
            }
            BLL.Orders bllOrder = new BLL.Orders();
            result = bllOrder.UpdateRealOrder(orderId, productId, productCount, shelf.Device.ID);
            //回复结果
            Protocol backInfo = new Protocol()
            {
                NeedAnswer = false,
                DeviceIP = info.DeviceIP,
                FunList = new List<Function>() { new Function(){ 
                    Code = FunctionCode.SystemPickerResult, 
                    TargetInfo=result == ErrorCode.OK?0:1} }
            };
            if (result != ErrorCode.OK)
            {
                string strError = Models.ErrorDescription.ExplainCode(result);
                byte[] byteError = Encoding.ASCII.GetBytes(strError);
                backInfo.FunList[0].PathPoint = Core.Coder.ConvertByteArray2Locations(byteError);
            }
            Core.Communicate.SendBuffer2Client(backInfo, StoreComponentType.PickStation);
        }

        /// <summary>
        /// 计算一条路径
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private List<Location> GetNormalPath(int source, int target)
        {
            Core.Path path = new Core.Path();
            List<HeadNode> nodeList = path.GetGeneralPath(source, target);
            List<Location> locList = new List<Location>();
            foreach (HeadNode node in nodeList)
            {
                locList.Add(node.Location);
            }

            return locList;
        }

        /// <summary>
        /// 根据设备ID获取当前移动中货架信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        private ShelfTarget GetShelfTargetByDeviceId(int deviceId)
        {
            ShelfTarget shelf;
            lock (Models.GlobalVariable.LockShelfMoving)
            {
                List<ShelfTarget> shelfList = Models.GlobalVariable.ShelvesMoving;
                shelf = shelfList.Find(item => item.Device.ID == deviceId);
            }
            return shelf;
        }
        #endregion

        #region 界面回调函数

        /// <summary>
        /// 更新设备显示位置
        /// </summary>
        /// <param name="info">包信息</param>
        private void UpdateItemLocation(Protocol info)
        {
            //RealDevice device = Models.GlobalVariable.RealDevices.Find(item => item.IPAddress == info.DeviceIP);
            //if (device == null)
            //{
            //    throw new Exception(ErrorCode.CannotFindByID.ToString());
            //}

            //if (this.updateLocation != null)
            //{
            //    this.updateLocation(StoreComponentType.Devices, device.DeviceID, info.FunList[0].PathPoint[0]);
            //}

            #region 由于通过动态端口无法识别小车，所以通过保留参数识别
            if (deviceIPList.ContainsValue(info.DeviceIP))
            {
                if (deviceIPList.ContainsKey(info.FunList[0].TargetInfo))
                    deviceIPList.Remove(info.FunList[0].TargetInfo);
                deviceIPList.Add(info.FunList[0].TargetInfo, info.DeviceIP);
            }
            if (this.updateLocation != null)
            {
                this.updateLocation(StoreComponentType.Devices, info.FunList[0].TargetInfo, info.FunList[0].PathPoint[0]);
            }
            #endregion
        }

        /// <summary>
        /// 更新显示颜色
        /// </summary>
        /// <param name="info"></param>
        private void UpdateItemColor(Protocol info, StoreComponentType itemType, int idxId, int colorFlag)
        {
            #region 由于通过动态端口无法识别站台，所以通过保留参数识别
            if (stationIPList.ContainsValue(info.DeviceIP))
            {
                if (stationIPList.ContainsKey(info.FunList[0].TargetInfo))
                    stationIPList.Remove(info.FunList[0].TargetInfo);
                stationIPList.Add(info.FunList[0].TargetInfo, info.DeviceIP);
            }
            #endregion
            if (this.updateColor != null)
            {
                this.updateColor(itemType, idxId, colorFlag);
            }
        }

        #endregion

        #endregion

        #region 总监控系统 安排小车搬运货架
        /// <summary>
        /// 系统处理接收的信息
        /// </summary>
        /// <param name="item">暂时无用（统一接口）</param>
        private void SystemAssignDevice(object item)
        {
            Choice choice = new Choice ();
            BLL.Devices device = new BLL.Devices();

            while (true)
            {
                ShelfTarget? shelfTarget = null;
                choice.GetCurrentShelfDevice(out shelfTarget);
                if (!shelfTarget.HasValue) break;

                ErrorCode code = device.TakeShelf(shelfTarget.Value);
                if (code != ErrorCode.OK)
                {
                    throw new Exception(ErrorDescription.ExplainCode(code));
                }
            }
        }
        #endregion
    }
}