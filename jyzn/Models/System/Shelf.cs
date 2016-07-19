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
        public Core.Location Target;

        public Shelf Shelf;

        public ShelfTarget(Core.Location target, Shelf shelf)
        {
            Target = target;
            Shelf = shelf;
        }
    }
}
