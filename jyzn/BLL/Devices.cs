using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// 小车设备相关的业务层逻辑
    /// </summary>
    public class Devices
    {
        /// <summary>
        /// 安排设备充电
        /// </summary>
        /// <param name="deviceID"></param>
        public void Charge(int deviceID)
        {

        }

        /// <summary>
        /// 去找货架
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="shelfID"></param>
        public void TakeShelf(int deviceID, int shelfID)
        {

        }

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="target"></param>
        public void Move2Position(int deviceID, Models.Location target)
        {

        }
    }
}
