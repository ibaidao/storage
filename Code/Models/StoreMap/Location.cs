using System;

namespace Models
{
    /// <summary>
    /// 坐标位置结构体
    /// </summary>
    public struct Location
    {

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="status"></param>
        public Location(Int32 x, Int32 y, Int32 z, Models.StoreComponentStatus status)
        {
            XPos = x;
            YPos = y;
            ZPos = z;
            Status = status;
        }

        public Location(Int32 x, Int32 y, Int32 z)
        {
            XPos = x;
            YPos = y;
            ZPos = z;
            Status = Models.StoreComponentStatus.OK;
        }

        /// <summary>
        /// X轴坐标
        /// </summary>
        public Int32 XPos;

        /// <summary>
        /// Y轴坐标
        /// </summary>
        public Int32 YPos;

        /// <summary>
        /// Z轴坐标
        /// </summary>
        public Int32 ZPos;

        /// <summary>
        /// 是否有效
        /// </summary>
        public Models.StoreComponentStatus Status;

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", XPos, YPos, ZPos);
        }

        /// <summary>
        /// 将逗号分隔的字符串转换为位置结构体
        /// </summary>
        /// <param name="strLocation"></param>
        /// <returns></returns>
        public static Location DecodeStringInfo(string strLocation)
        {
            Location loc = new Location();
            string[] xyz = strLocation.Split(',');

            loc.XPos = int.Parse(xyz[0]);
            loc.YPos = int.Parse(xyz[1]);
            loc.ZPos = int.Parse(xyz[2]);

            return loc;
        }
    }
}
