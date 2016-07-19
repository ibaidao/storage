using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Core.Location Target;

        /// <summary>
        /// 货架
        /// </summary>
        public Shelf Shelf;

        /// <summary>
        /// 货架初始位置
        /// </summary>
        public Core.Location Source;

        public ShelfTarget(Core.Location target, Core.Location source, Shelf shelf)
        {
            Target = target;
            Source = source;
            Shelf = shelf;
        }
    }
}
