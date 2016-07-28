using System;

namespace Models
{
    /// <summary>
    /// 坐标位置结构体
    /// </summary>
    public struct Location
    {
        /// <summary>
        /// 不同方向路径对应权重
        /// </summary>
        private const Int32 pathWeightX = 1, pathWeightY = 1, pathWeightZ = 5;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="status"></param>
        public Location(Int32 x, Int32 y, Int32 z, Boolean status)
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
            Status = true;
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
        public Boolean Status;

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", XPos, YPos, ZPos);
        }

        /// <summary>
        /// 计算两点间的曼哈顿距离
        /// </summary>
        /// <param name="source">起点</param>
        /// <param name="target">终点</param>
        /// <returns>距离值</returns>
        public static int Manhattan(Location source, Location target)
        {
            int len = 0;

            len += pathWeightX * Math.Abs(target.XPos - source.XPos);

            len += pathWeightY * Math.Abs(target.YPos - source.YPos);

            len += pathWeightZ * Math.Abs(target.ZPos - source.ZPos);

            return len;
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

        /// <summary>
        /// 计算两点间的曼哈顿距离
        /// </summary>
        /// <param name="strSource">起点</param>
        /// <param name="strTarget">终点</param>
        /// <returns>距离值</returns>
        public static int Manhattan(string strSource, string strTarget)
        {
            Location source = DecodeStringInfo(strSource), target = DecodeStringInfo(strTarget);

            return Manhattan(source, target);
        }
    }

    ///// <summary>
    ///// 两组整数结构
    ///// </summary>
    //public struct XYPair
    //{
    //    int X;
    //    int Y;
    //    public XYPair(int x, int y)
    //    {
    //        X = x; 
    //        Y = y;
    //    }
    //}
}
