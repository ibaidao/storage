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
        public int Target;

        /// <summary>
        /// 货架
        /// </summary>
        public Shelf Shelf;

        /// <summary>
        /// 货架初始位置
        /// </summary>
        public int Source;

        public ShelfTarget(int target, int source, Shelf shelf)
        {
            Target = target;
            Source = source;
            Shelf = shelf;
        }
    }
}
