using System;
using System.Collections;
using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// 系统逻辑用到的全局变量
    /// </summary>
    public static class GlobalVariable
    {
        /// <summary>
        /// 实时小车设备
        /// </summary>
        public static List<RealDevice> RealDevices
        {
            get
            {
                return Core.Singleton<List<RealDevice>>.GetInstance();
            }
        }

        /// <summary>
        /// 需要搬运的货架
        /// </summary>
        public static List<ShelfTarget> ShelvesNeedToMove
        {
            get
            {
                return Core.Singleton<List<ShelfTarget>>.GetInstance();
            }
        }

        /// <summary>
        /// 实时交通地图
        /// </summary>
        public static Graph RealGraphTraffic
        {
            get
            {
                return Core.Singleton<Graph>.GetInstance();
            }
        }
    }
}
