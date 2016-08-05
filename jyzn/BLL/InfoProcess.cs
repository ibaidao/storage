using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Models;

namespace BLL
{
    /// <summary>
    /// 对接收到信息的处理逻辑
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
            if (instance) throw new Exception("信息处理线程重复定义");

            instance = true;
            this.CheckDeviceMessageFlag = true;
            this.warningOnMainWindow = warningShowFun;
            this.updateLocation = updateItemLocation;
            this.updateColor = updateItemColor;

            Thread threadSystemHandler = new Thread(SystemHandlerInfo);
            threadSystemHandler.Start();

            Thread threadAsignDevice = new Thread(SystemAssignDevice);
            threadAsignDevice.Start();
        }

        /// <summary>
        /// 检查设备信息队列
        /// </summary>
        public bool CheckDeviceMessageFlag
        {
            get;
            set;
        }

        #region 总监控系统 处理接收消息
        /// <summary>
        /// 系统处理接收的信息
        /// </summary>
        public void SystemHandlerInfo()
        {
            while (CheckDeviceMessageFlag)
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

            switch (proto.FunList[0].Code)
            {
                #region 小车自身
                case FunctionCode.DeviceCurrentStatus:
                    this.DeviceHeartBeat(proto);
                    break;
                case FunctionCode.DeviceLowBattery:
                    this.DeviceLowBattery(proto);
                    break;
                case FunctionCode.DeviceUnkownTrouble: break;
                case FunctionCode.DeviceMeetBalk: break;
                case FunctionCode.DeviceOverload: break;
                #endregion

                #region 小车业务相关
                case FunctionCode.DeviceFindHoldShelf: break;
                case FunctionCode.DeviceGetPickStation: break;
                case FunctionCode.DeviceReturnFreeShelf: break;
                #endregion

                #region 拣货操作
                case FunctionCode.PickerStartWork:
                    this.PickerStartWork(proto); 
                    break;
                case FunctionCode.PickerFindProduct:
                    this.PickerFindProduct(proto); 
                    break;
                case FunctionCode.PickerPutProductOrder:
                    this.PickerPutProductOrder(proto); 
                    break;
                #endregion
                default: break;
            }
        }

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
                Core.Communicate.SendBuffer2Device(new Protocol()
                {
                    DeviceIP = info.DeviceIP,
                    FunList = new List<Function>() 
                    { 
                        new Function() { Code = FunctionCode.OrderStopMove },
                        //new Function() { Code = FunctionCode.OrderTurnDirection,
                        // TargetInfo}                        
                    }
                });
                if (warningOnMainWindow != null) this.warningOnMainWindow(ErrorCode.DeviceLocationError);
            }
            if (info.NeedAnswer && nothingError)
            {//正常情况，需要回复则发送回执
                Core.Communicate.SendBuffer2Device(new Protocol()
                {
                    DeviceIP = info.DeviceIP,
                    FunList = new List<Function>() 
                    { 
                        new Function() { Code = FunctionCode.SystemDefaultFeedback }
                    }
                });
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
            RealDevice device = Models.GlobalVariable.RealDevices.Find(item => item.IPAddress == info.DeviceIP);
            if (device == null)
            {//警告无法定位设备/信息有误
                if (this.warningOnMainWindow != null) this.warningOnMainWindow(ErrorCode.CannotFindByID);
                return;
            }

            Function fun = new Function();
            if (device.FunctionCode == (int)Models.FunctionCode.OrderCharge ||//已经在充电的路上了
                device.FunctionCode == (int)Models.FunctionCode.OrderMoveShelfBack || //在送返货架，则先等设备完成本次任务
                device.FunctionCode == (int)Models.FunctionCode.OrderMoveShelfToStation)//在搬货架到拣货台，则先等设备完成本次任务
            {
                fun.Code = FunctionCode.SystemDefaultFeedback;
            }
            else if (device.FunctionCode == (int)Models.FunctionCode.OrderGetShelf)
            {//准备去运货架，则中止当前工作，去充电
                fun.Code = FunctionCode.OrderCharge;
                Station station = null;
                if (Choice.FindClosestCharger(device.DeviceID, ref station) == ErrorCode.OK)
                {
                    fun.TargetInfo = station.ID;
                    List<HeadNode> pathNode = Utilities.Singleton<Core.Path>.GetInstance().GetGeneralPath(device.LocationID, station.LocationID);
                    fun.PathPoint = new List<Location>(pathNode.Count);
                    foreach (HeadNode node in pathNode)
                        fun.PathPoint.Add(node.Location);
                }
                //安排新空闲小车去搬货架，或者把任务放回任务队列

            }

            Core.Communicate.SendBuffer2Device(new Protocol()
            {
                DeviceIP = info.DeviceIP,
                FunList = new List<Function>() 
                    { 
                        fun
                    }
            });
        }

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
        private void UpdateItemColor(Protocol info)
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
                this.updateColor(StoreComponentType.PickStation, info.FunList[0].TargetInfo, 1);
            }
        }

        /// <summary>
        /// 拣货员开始拣货
        /// 
        /// </summary>
        /// <param name="info"></param>
        private void PickerStartWork(Protocol info)
        {
            UpdateItemColor(info);
        }

        /// <summary>
        /// 拣货员从货架上拿下商品，并扫码
        /// </summary>
        /// <param name="info"></param>
        private void PickerFindProduct(Protocol info)
        {
            //
        }

        /// <summary>
        /// 拣货员将商品放入订单分拣箱，并关闭电子标签
        /// </summary>
        /// <param name="info"></param>
        private void PickerPutProductOrder(Protocol info)
        {

        }
        #endregion

        #region 总监控系统 安排小车搬运货架
        /// <summary>
        /// 系统处理接收的信息
        /// </summary>
        public void SystemAssignDevice()
        {
            Choice choice = new Choice ();
            Devices device = new Devices();
            while (CheckDeviceMessageFlag)
            {
                if (Models.GlobalVariable.ShelvesNeedToMove.Count == 0)
                {
                    Thread.Sleep(1000);//每秒检查队列一次，定时模式可改为消息模式
                    continue;
                }

                ShelfTarget? shelfTarget=null;
                RealDevice realDevice = null;
                choice.GetCurrentShelfDevice(out shelfTarget,out realDevice);

                ErrorCode code = device.TakeShelf(realDevice.DeviceID, shelfTarget.Value);
                if (code != ErrorCode.OK)
                {
                    throw new Exception(ErrorDescription.ExplainCode(code));
                }
            }
        }
        #endregion
    }
}