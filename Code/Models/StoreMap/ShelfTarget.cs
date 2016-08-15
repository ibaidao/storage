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
        /// 目标历史
        /// </summary>
        public string StationHistory;

        /// <summary>
        /// 返回时对应的位置(目前回原位)
        /// </summary>
        public int BackLocation;

        /// <summary>
        /// 当前状态
        /// </summary>
        public StoreComponentStatus Status;

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
            BackLocation = source;//将货架放回原来位置
            Shelf = shelf;
            Device = null;
            Status = StoreComponentStatus.OK;
            StationHistory = stationID.ToString();
        }

        public ShelfTarget(int stationID, int target, int source, Shelf shelf, Devices deviceInfo)
        {
            StationId = stationID;
            Target = target;
            Source = source;
            BackLocation = source;//将货架放回原来位置
            Shelf = shelf;
            Device = deviceInfo;
            Status = StoreComponentStatus.PreWorking;
            StationHistory = stationID.ToString();
        }
    }
}
