using System;

//货架相关模型
namespace Models
{
    /// <summary>
    /// 货架及目标地点 结构体
    /// </summary>
    public struct ShelfTarget
    {
        /// <summary>
        /// 货架初始位置
        /// </summary>
        public int Source;

        /// <summary>
        /// 货架移动目标位置
        /// </summary>
        public int Target;

        /// <summary>
        /// 货架要去的站台ID（暂时一个站台）
        /// </summary>
        public int StationId;

        /// <summary>
        /// 返回时对应的位置(目前回原位)
        /// </summary>
        public int BackLocation;

        /// <summary>
        /// 货架
        /// </summary>
        public Shelf Shelf;

        /// <summary>
        /// 对应设备ID
        /// </summary>
        public Devices Device;

        public ShelfTarget(int stationID, int target, int source, Shelf shelf)
        {
            StationId = stationID;
            Target = target;
            Source = source;
            BackLocation = source;
            Shelf = shelf;
            Device = null;
        }

        public ShelfTarget(int stationID, int target, int source, Shelf shelf, Devices deviceInfo)
        {
            StationId = stationID;
            Target = target;
            Source = source;
            BackLocation = source;
            Shelf = shelf;
            Device = deviceInfo;
        }
    }
}
