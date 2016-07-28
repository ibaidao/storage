using System;
using Utilities;
using Models.dbHandler;

namespace Models
{
    /// <summary>
    /// 各数据表外表调用实体
    /// </summary>
    public static class DbEntity
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public static DataAccess<Staff> DStaff
        {
            get
            {
                return Singleton<DataAccess<Staff>>.GetInstance();
            }
        }

        /// <summary>
        /// 货架信息表
        /// </summary>
        public static DataAccess<Shelf> DShelf
        {
            get
            {
                return Singleton<DataAccess<Shelf>>.GetInstance();
            }
        }

        /// <summary>
        /// 仓库内站点（补货/拣货台/充电桩）信息表
        /// </summary>
        public static DataAccess<Station> DStation
        {
            get
            {
                return Singleton<DataAccess<Station>>.GetInstance();
            }
        }

        /// <summary>
        /// 商品货架关系表
        /// </summary>
        public static DataAccess<SkuInfo> DSkuInfo
        {
            get
            {
                return Singleton<DataAccess<SkuInfo>>.GetInstance();
            }
        }
        
        /// <summary>
        /// 员工行为（拣货/补货）表
        /// </summary>
        public static DataAccess<RealStaff> DRealStaff
        {
            get
            {
                return Singleton<DataAccess<RealStaff>>.GetInstance();
            }
        }

        /// <summary>
        /// 实时移动货架表
        /// </summary>
        public static DataAccess<RealShelf> DRealShelf
        {
            get
            {
                return Singleton<DataAccess<RealShelf>>.GetInstance();
            }
        }

        /// <summary>
        /// 实时订单表
        /// </summary>
        public static DataAccess<RealOrders> DRealOrders
        {
            get
            {
                return Singleton<DataAccess<RealOrders>>.GetInstance();
            }
        }

        /// <summary>
        /// 实时设备位置表
        /// </summary>
        public static DataAccess<RealDevice> DRealDevice
        {
            get
            {
                return Singleton<DataAccess<RealDevice>>.GetInstance();
            }
        }

        /// <summary>
        /// 商品信息表
        /// </summary>
        public static DataAccess<Models.Products> DProducts
        {
            get
            {
                return Singleton<DataAccess<Models.Products>>.GetInstance();
            }
        }

        /// <summary>
        /// 异常记录表
        /// </summary>
        public static DataAccess<LogError> DLogError
        {
            get
            {
                return Singleton<DataAccess<LogError>>.GetInstance();
            }
        }

        /// <summary>
        /// 行为日志表
        /// </summary>
        public static DataAccess<LogAction> DLogAction
        {
            get
            {
                return Singleton<DataAccess<LogAction>>.GetInstance();
            }
        }

        /// <summary>
        /// 设备信息表
        /// </summary>
        public static DataAccess<Devices> DDevices
        {
            get
            {
                return Singleton<DataAccess<Devices>>.GetInstance();
            }
        }

        /// <summary>
        /// 仓库内位置坐标
        /// </summary>
        public static DataAccess<StorePoints> DStorePoints
        {
            get
            {
                return Singleton<DataAccess<StorePoints>>.GetInstance();
            }
        }

        /// <summary>
        /// 仓库内路线
        /// </summary>
        public static DataAccess<StorePaths> DStorePaths
        {
            get
            {
                return Singleton<DataAccess<StorePaths>>.GetInstance();
            }
        }
    }
}
