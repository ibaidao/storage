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
        /// 货架移动目标位置
        /// </summary>
        public Location Target;

        /// <summary>
        /// 货架
        /// </summary>
        public Shelf Shelf;

        /// <summary>
        /// 货架初始位置
        /// </summary>
        public Location Source;

        public ShelfTarget(Location target, Location source, Shelf shelf)
        {
            Target = target;
            Source = source;
            Shelf = shelf;
        }
    }
}
