using System;

namespace Core
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
        public Location(double x, double y, double z)
        {
            XPos = x;
            YPos = y;
            ZPos = z;
        }

        /// <summary>
        /// X轴坐标
        /// </summary>
        public double XPos;

        /// <summary>
        /// Y轴坐标
        /// </summary>
        public double YPos;

        /// <summary>
        /// Z轴坐标
        /// </summary>
        public double ZPos;
    }
}
