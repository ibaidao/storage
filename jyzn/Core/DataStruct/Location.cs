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
