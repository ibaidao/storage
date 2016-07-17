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
    }
}
