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
        private const int PATH_WEIGHT = 1;
        /// <summary>
        /// 地图数据
        /// </summary>
        private static Models.Graph graph;
        private static Models.Logic.Path path;

        static StoreInit()
        {
            ////初始地图加载
            //
            path = Core.Singleton<Models.Logic.Path>.GetInstance();
            graph = Models.GlobalVariable.RealGraphTraffic.Clone() as Graph;
        }

        /// <summary>
        /// 新增仓库充电桩/拣货台
        /// </summary>
        /// <param name="type">拣货台/补货台/充电桩</param>
        /// <param name="locIdx">节点数据</param>
        public void AddChargerPickStation(StoreComponentType type, string code, int locIdx)
        {
            string strWhere = string.Format(" LocationID = {0} ", locIdx);
            object item = DbEntity.DStation.GetSingleEntity(strWhere, null);
            if (item == null)
            {
                DbEntity.DStation.Insert(new Station()
                {
                    Code = code,
                    LocationID = locIdx,
                    Status = 1,
                    Type = (short)type,
                    Location = graph.GetHeadNodeByData(locIdx).Location.ToString()
                });
            };

        }

        /// <summary>
        /// 增加双向路径
        /// </summary>
        /// <param name="one">一端数据</param>
        /// <param name="two">另一端</param>
        public void AddPath(int one, int two)
        {
            graph.AddEdge(one, two, PATH_WEIGHT);
        }

        /// <summary>
        /// 增加单向路径
        /// </summary>
        /// <param name="start">起点数据</param>
        /// <param name="end"></param>
        public void AddPathDirection(int start, int end)
        {
            graph.AddDirectEdge(start, end, PATH_WEIGHT);
        }

    }
}
