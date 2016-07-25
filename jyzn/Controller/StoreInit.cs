using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Controller
{
    /// <summary>
    /// 应用层 仓库初始化
    /// </summary>
    public class StoreInit
    {
        /// <summary>
        /// 地图数据
        /// </summary>
        Models.Graph graph;

        public StoreInit()
        {
            ////初始地图加载
            //
            new Models.Logic.Path();
            graph = Models.GlobalVariable.RealGraphTraffic;

        }


    }
}
