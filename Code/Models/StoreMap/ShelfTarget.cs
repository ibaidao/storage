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
        /// 货架要去的站台ID
        /// </summary>
        public int StationId;

        /// <summary>
        /// 换拣货站台时，保存上一个拣货台，开始拣货后清零（仅用于新到一个拣货台时，看原拣货台商品信息，仅用一次）
        /// </summary>
        public int OldStationId;

        /// <summary>
        /// 目标历史
        /// </summary>
        public string StationHistory;

        /// <summary>
        /// 返回仓储区时对应的位置(目前回原位)
        /// </summary>
        public int BackLocation;

        /// <summary>
        /// 当前状态（去拣货台路上Pre，拣货Working，拣完After）
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
            OldStationId = 0;
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
            OldStationId = 0;
        }
    }
}
