using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// 距离相关实体
    /// </summary>
    public class Distance
    {
        /// <summary>
        /// 不同方向路径对应权重
        /// </summary>
        private const Int32 pathWeightX = 1, pathWeightY = 1, pathWeightZ = 5;

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
        /// 将位置结构体转换为逗号分隔的字符串
        /// </summary>
        /// <param name="strLocation"></param>
        /// <returns></returns>
        public static string EncodeStringInfo(Location loc)
        {
            return string.Format("{0},{1},{2}", loc.XPos, loc.YPos, loc.ZPos);
        }
    }
}
