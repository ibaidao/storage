using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Core
{
    /// <summary>
    /// 位置相关逻辑
    /// </summary>
    public class CalcLocation
    {
        /// <summary>
        /// 不同方向路径对应权重
        /// </summary>
        private const Int32 pathWeightX = 1, pathWeightY = 1, pathWeightZ = 5;

        /// <summary>
        /// 根据具体坐标值字符串，获取最近节点索引
        /// </summary>
        /// <param name="locStr"></param>
        /// <returns></returns>
        public static int GetLocationIDByXYZ(string locStr)
        {
            Location loc = Location.DecodeStringInfo(locStr);
            return GetLocationIDByXYZ(loc);
        }

        /// <summary>
        /// 根据具体坐标值，获取最近节点索引
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static int GetLocationIDByXYZ(Location loc)
        {
            int idx = 0;
            Dictionary<int, Models.Location> locList = Models.GlobalVariable.AllMapPoints;
            int minDistance = int.MaxValue;
            foreach (KeyValuePair<int, Location> item in locList)
            {
                if (Manhattan(loc, item.Value) < minDistance)
                {
                    idx = item.Key;
                    loc = item.Value;
                }
            }

            return idx;
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
        /// 计算两点间的曼哈顿距离
        /// </summary>
        /// <param name="strSource">起点</param>
        /// <param name="strTarget">终点</param>
        /// <returns>距离值</returns>
        public static int Manhattan(string strSource, string strTarget)
        {
            Location source = Location.DecodeStringInfo(strSource), target = Location.DecodeStringInfo(strTarget);

            return Manhattan(source, target);
        }


        /// <summary>
        /// 仓库 -> 地图 位置/尺寸转换
        /// </summary>
        /// <param name="location"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        public static int MapConvert(int location, double ratio)
        {
            return (int)Math.Floor(location * ratio);
        }

        /// <summary>
        /// 地图 -> 仓库 位置/尺寸转换
        /// </summary>
        /// <param name="mapSize"></param>
        /// <returns></returns>
        public static int MapReverse(int mapSize, double ratio)
        {
            return (int)Math.Ceiling(mapSize / ratio);
        }

        /// <summary>
        /// 缩放地图坐标
        /// </summary>
        /// <param name="realLoc">真实坐标</param>
        /// <return>变换后坐标</return>
        public static Location ExchangeMapRatio(Location realLoc)
        {
            //实际仓库尺寸 -》 地图缩放(cm -> 像素)
            Models.Location loc = MapConvert(realLoc, Graph.RatioMapZoom);
            //地图本身缩放
            loc = MapConvert(loc, Graph.RatioMapSelfZoom);

            loc.XPos += Graph.MapMarginLeftUp.XPos;
            loc.YPos += Graph.MapMarginLeftUp.YPos;
            return loc;
        }

        /// <summary>
        /// 地图位置/尺寸转换
        /// </summary>
        /// <param name="location"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        public static Location MapConvert(Location realLoc, double ratio)
        {
            Location result;
            result.XPos = MapConvert(realLoc.XPos, ratio);
            result.YPos = MapConvert(realLoc.YPos, ratio);
            result.ZPos = MapConvert(realLoc.ZPos, ratio);
            result.Status = realLoc.Status;

            return result;
        }
    }
}
