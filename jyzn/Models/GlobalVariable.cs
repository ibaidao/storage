using System;
using System.Collections;
using System.Collections.Generic;
using Utilities;

namespace Models
{
    /// <summary>
    /// 系统逻辑用到的全局变量
    /// </summary>
    public static class GlobalVariable
    {
        /// <summary>
        /// 当前仓库ID
        /// </summary>
        public const int STORE_ID = 1;
        public static object LockShelfNeedMove;
        public static object LockShelfMoving;
        public static object LockRealDevices;

        private static List<ShelfTarget> shelfWaiting = new List<ShelfTarget>();
        private static List<ShelfTarget> shelfMoving = new List<ShelfTarget>();

        /// <summary>
        /// 实时小车设备
        /// </summary>
        public static List<RealDevice> RealDevices
        {
            get
            {
                return Singleton<List<RealDevice>>.GetInstance();
            }
        }

        /// <summary>
        /// 实时货架（位置）
        /// </summary>
        public static List<Shelf> RealShelves
        {
            get
            {
                return Singleton<List<Shelf>>.GetInstance();
            }
        }

        /// <summary>
        /// 实时充电桩/拣货台（状态）
        /// </summary>
        public static List<Station> RealStation
        {
            get
            {
                return Singleton<List<Station>>.GetInstance();
            }
        }

        /// <summary>
        /// 需要搬运的货架
        /// </summary>
        public static List<ShelfTarget> ShelvesNeedToMove
        {
            get
            {
                return shelfWaiting;
            }
        }

        /// <summary>
        /// 移动中的货架
        /// </summary>
        public static List<ShelfTarget> ShelvesMoving
        {
            get
            {
                return shelfMoving;
            }
        }

        /// <summary>
        /// 当前货架对应拣货台的商品
        /// </summary>
        public static List<ShelfProduct> StationShelfProduct
        {
            get
            {
                return Singleton<List<ShelfProduct>>.GetInstance();
            }
        }

        /// <summary>
        /// 实时交通地图
        /// </summary>
        public static Graph RealGraphTraffic
        {
            get
            {
                return Singleton<Graph>.GetInstance();
            }
        }
    }
}
