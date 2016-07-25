using System;
using System.Collections.Generic;

//无向图/路径相关模型
namespace Models
{
    /// <summary>
    /// 邻接表头
    /// </summary>
    public struct HeadNode
    {
        /// <summary>
        /// 节点数据
        /// </summary>
        public int Data;

        /// <summary>
        /// 节点绝对坐标
        /// </summary>
        public Core.Location Location;

        /// <summary>
        /// 节点名称
        /// </summary>
        public String Name;

        /// <summary>
        /// 邻接表
        /// </summary>
        public List<Edge> Edge;

        public HeadNode(int idx, string name, Core.Location loc)
        {
            Data = idx;
            Location = loc;
            Name = name;
            Edge = new List<Edge>();
        }

    }

    /// <summary>
    /// 邻接表边属性
    /// </summary>
    public struct Edge
    {
        /// <summary>
        /// 节点索引
        /// </summary>
        public int Idx;

        /// <summary>
        /// 边权重
        /// </summary>
        public int Weight;

        /// <summary>
        /// 边长（两节点间距）
        /// </summary>
        public int Distance;

        public Edge(int index, int weight, int distance)
        {
            Idx = index;
            Weight = weight;
            Distance = distance;
        }
    }

    /// <summary>
    /// 由两组数组成的节点
    /// </summary>
    public struct XYPair
    {
        int XValue;
        int YValue;

        public XYPair(int x, int y)
        {
            XValue = x;
            YValue = y;
        }
    }

    /// <summary>
    /// 无向图
    /// </summary>
    public class Graph:ICloneable
    {
        public Graph()
        {
            this.NodeList = new List<HeadNode>();
            this.NodeIdxList = new List<int>();
            InitalDefaultValue();
        }

        /// <summary>
        /// 用指定数量的节点初始化图
        /// </summary>
        /// <param name="nodeCount">节点数</param>
        public Graph(int nodeCount)
        {
            this.NodeCount = nodeCount;
            this.EdgeCount = 0;
            this.NodeList = new List<HeadNode>(nodeCount);
            this.NodeIdxList = new List<int>(nodeCount);
            for (int i = 0; i < nodeCount; i++)
            {
                NodeList[i] = new HeadNode();
            }
            InitalDefaultValue();
        }

        /// <summary>
        /// 地图默认属性初始化
        /// </summary>
        private void InitalDefaultValue()
        {
            this.RatioMapZoomIn = 0.01;
            this.SizeGraph = new XYPair(2000, 2000);
            this.SizePickStation = new XYPair(100, 100);
            this.SizeCharger = new XYPair(100, 100);
            this.SizeShelf = new XYPair(90, 90);
            this.SizeDevice = new XYPair(80, 80);
            this.ColorCharger = ConsoleColor.Gray;
            this.ColorCrossing = ConsoleColor.Gray;
            this.ColorDevice = ConsoleColor.Gray;
            this.ColorDeviceShelf = ConsoleColor.Gray;
            this.ColorPath = ConsoleColor.Gray;
            this.ColorPickStation = ConsoleColor.Gray;
            this.ColorShelf = ConsoleColor.Gray;
        }

        /// <summary>
        /// 邻接表结构
        /// </summary>
        public List<HeadNode> NodeList { get; set; }

        /// <summary>
        /// 用于索引位置对应的节点
        /// </summary>
        public List<int> NodeIdxList { get; set; }

        /// <summary>
        /// 地图缩放比例
        /// </summary>
        public double RatioMapZoomIn { get; set; }

        /// <summary>
        /// 实际地图尺寸（cm）
        /// </summary>
        public XYPair SizeGraph { get; set; }

        /// <summary>
        /// 实际充电桩尺寸（cm）
        /// </summary>
        public XYPair SizeCharger { get; set; }

        /// <summary>
        /// 实际小车尺寸（cm）
        /// </summary>
        public XYPair SizeDevice { get; set; }

        /// <summary>
        /// 实际货架尺寸（cm）
        /// </summary>
        public XYPair SizeShelf { get; set; }

        /// <summary>
        /// 实际拣货台尺寸（cm）
        /// </summary>
        public XYPair SizePickStation { get; set; }

        /// <summary>
        /// 货架显示背景色
        /// </summary>
        private ConsoleColor ColorShelf { get; set; }

        /// <summary>
        /// 充电器显示背景色
        /// </summary>
        private ConsoleColor ColorCharger { get; set; }

        /// <summary>
        /// 打包台显示背景色
        /// </summary>
        private ConsoleColor ColorPickStation { get; set; }

        /// <summary>
        /// 空车显示背景色
        /// </summary>
        private ConsoleColor ColorDevice { get; set; }

        /// <summary>
        /// 带有货架的小车显示背景色
        /// </summary>
        private ConsoleColor ColorDeviceShelf { get; set; }

        /// <summary>
        /// 路线显示背景色
        /// </summary>
        private ConsoleColor ColorPath { get; set; }

        /// <summary>
        /// 路口显示背景色
        /// </summary>
        private ConsoleColor ColorCrossing { get; set; }

        /// <summary>
        /// 节点数
        /// </summary>
        public int NodeCount { get; private set; }

        /// <summary>
        /// 边数
        /// </summary>
        public int EdgeCount { get; private set; }

        /// <summary>
        /// 增加节点
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="name">备注名称</param>
        /// <param name="loc">对应坐标位置</param>
        public void AddPoint(int data, string name, Core.Location loc)
        {
            this.NodeIdxList.Add(data);
            this.NodeList.Add(new HeadNode(data, name, loc));
            this.NodeCount++;
        }

        /// <summary>
        /// 移除/关闭 节点
        /// </summary>
        /// <param name="data">节点数据</param>
        /// <returns></returns>
        public bool RemovePoint(int data)
        {
            int removeCount = 0, nodeIdx = this.GetIndexByData(data);
            HeadNode node = this.NodeList[nodeIdx];
            List<Edge> edge;
            //先删除指向当前节点的节点（边中含有当前节点）
            foreach (Edge item in node.Edge)
            {//依次访问节点连接的所有边
                edge = this.NodeList[item.Idx].Edge;
                for (int i = 0; i < edge.Count; i++)
                {//无向边是双向的有向边替代
                    if (edge[i].Idx == nodeIdx)
                    {
                        edge.RemoveAt(i);
                        removeCount++;
                        break;
                    }
                }
                this.EdgeCount--;
            }
            //再移除当前节点
            this.NodeList.RemoveAt(nodeIdx);
            this.NodeIdxList.RemoveAt(nodeIdx);
            this.NodeCount--;
            //最后所有边索引中，大于等于被删节点的减1
            Edge tmpEdge;
            for (int i = 0; i < NodeCount; i++)
            {//所有
                edge = NodeList[i].Edge;
                for (int j = 0; j < edge.Count; j++)
                {
                    if (edge[j].Idx >= nodeIdx)
                    {
                        tmpEdge = edge[j];
                        tmpEdge.Idx--;
                        edge[j] = tmpEdge;
                    }
                }
            }
            return removeCount == node.Edge.Count;
        }

        /// <summary>
        /// 增加无向边（双向路）
        /// </summary>
        /// <param name="one">一个端点</param>
        /// <param name="two"></param>
        /// <param name="weight">权重</param>
        public void AddEdge(int one, int two, int weight)
        {
            int oneIdx = this.GetIndexByData(one),
                twoIdx = this.GetIndexByData(two);
            if (oneIdx < 0 || twoIdx < 0)
            {
                throw new Exception("增加边失败，节点不存在");
            }

            int length = Core.Distance.Manhattan(NodeList[oneIdx].Location, NodeList[twoIdx].Location);

            //两条双向边代表无向边
            bool edgeExists = false;
            //忽略重复添加的边
            foreach (Edge edge in NodeList[oneIdx].Edge)
                if (edge.Idx == twoIdx)
                    edgeExists = true;
            if(!edgeExists)
                NodeList[oneIdx].Edge.Add(new Edge(twoIdx, weight, length));

            foreach (Edge edge in NodeList[twoIdx].Edge)
                if (edge.Idx == oneIdx)
                    edgeExists = true;
            if (!edgeExists)
                NodeList[twoIdx].Edge.Add(new Edge(oneIdx, weight, length));

            this.EdgeCount += 2;
        }

        /// <summary>
        /// 移除无向边（双向路）
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public bool RemoveEdge(int one, int two)
        {
            int oneIdx = GetIndexByData(one),
                twoIdx = GetIndexByData(two),
                count = 0;
            int oneEdgeCount = NodeList[oneIdx].Edge.Count,
                twoEdgeCount = NodeList[twoIdx].Edge.Count;
            for (int i = 0; i < oneEdgeCount; i++)
            {
                if (NodeList[oneIdx].Edge[i].Idx == twoIdx)
                {
                    NodeList[oneIdx].Edge.RemoveAt(i);
                    this.EdgeCount--;
                    count++;
                    break;
                }
            }
            for (int i = 0; i < twoEdgeCount; i++)
            {
                if (NodeList[twoIdx].Edge[i].Idx == oneIdx)
                {
                    NodeList[twoIdx].Edge.RemoveAt(i);
                    this.EdgeCount--;
                    count++;
                    break;
                }
            }

            return count == 2;
        }

        /// <summary>
        /// 增加一条有向边（指定方向的路）
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <param name="weight"></param>
        public void AddDirectEdge(int start, int end, int weight)
        {
            int startIdx = this.GetIndexByData(start),
                endIdx = this.GetIndexByData(end);
            int length = Core.Distance.Manhattan(NodeList[startIdx].Location, NodeList[endIdx].Location);

            bool edgeExists = false;
            //忽略重复添加的边
            foreach (Edge edge in NodeList[startIdx].Edge)
                if (edge.Idx == endIdx)
                    edgeExists = true;
            if (!edgeExists)
                NodeList[startIdx].Edge.Add(new Edge(endIdx, weight, length));

            this.EdgeCount++;
        }

        /// <summary>
        /// 移除一条有向边
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <returns></returns>
        public bool RemoveDirectEdge(int start, int end)
        {
            bool result = false;
            int startIdx = GetIndexByData(start),
                endIdx = GetIndexByData(end);
            int count = NodeList[startIdx].Edge.Count;
            for (int i = 0; i < count; i++)
            {
                if (NodeList[startIdx].Edge[i].Idx == endIdx)
                {
                    NodeList[startIdx].Edge.RemoveAt(i);
                    this.EdgeCount--;
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 检查两点间直连距离
        /// </summary>
        /// <param name="one">节点数据</param>
        /// <param name="two"></param>
        /// <returns>未直连返回-1</returns>
        public int CheckEdgeDistance(int one, int two)
        {
            int result = -1;
            foreach (HeadNode node in NodeList)
            {
                if (node.Data != one) continue;

                foreach (Edge item in node.Edge)
                {
                    if (NodeList[item.Idx].Data == two)
                    {
                        result = item.Distance;
                        break;
                    }
                }
                break;
            }
            return result;
        }

        /// <summary>
        /// 通过节点数据获取节点位置信息
        /// </summary>
        /// <param name="data">节点数据</param>
        /// <returns></returns>
        public HeadNode GetHeadNodeByData(int data)
        {
            return NodeList[NodeIdxList.IndexOf(data)];
            //HeadNode result = new HeadNode();
            //foreach (HeadNode node in nodeList)
            //{
            //    if (node.Data == data)
            //    {
            //        result = node;
            //        break;
            //    }
            //}
            //return result;
        }

        /// <summary>
        /// 根据节点数据获取索引
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int GetIndexByData(int data)
        {
            return NodeIdxList.IndexOf(data);
            //int result = -1;
            //for (int i = 0; i < nodeList.Count; i++)
            //{
            //    if (nodeList[i].Data == data)
            //    {
            //        result = i;
            //        break;
            //    }
            //}
            //return result;
        }

        /// <summary>
        /// 生成初始化地图（生成地图阶段使用）
        /// </summary>
        private void InitialMap()
        {//地图节点            
            #region 货架
            #region 第一排货架
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A2", Point = "300,200,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A3", Point = "400,200,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A4", Point = "500,200,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A5", Point = "600,200,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A9", Point = "1000,200,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A10", Point = "1100,200,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A11", Point = "1200,200,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A12", Point = "1300,200,0", StoreID = 1, Status = 0, Type = 1 });


            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B2", Point = "300,300,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B3", Point = "400,300,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B4", Point = "500,300,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B5", Point = "600,300,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B9", Point = "1000,300,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B10", Point = "1100,300,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B11", Point = "1200,300,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B12", Point = "1300,300,0", StoreID = 1, Status = 0, Type = 1 });

            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C2", Point = "300,400,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C3", Point = "400,400,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C4", Point = "500,400,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C5", Point = "600,400,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C9", Point = "1000,400,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C10", Point = "1100,400,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C11", Point = "1200,400,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C12", Point = "1300,400,0", StoreID = 1, Status = 0, Type = 1 });
            #endregion

            #region 第二排货架
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D2", Point = "300,500,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D3", Point = "400,500,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D4", Point = "500,500,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D5", Point = "600,500,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D9", Point = "1000,500,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D10", Point = "1100,500,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D11", Point = "1200,500,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D12", Point = "1300,500,0", StoreID = 1, Status = 0, Type = 1 });


            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E2", Point = "300,600,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E3", Point = "400,600,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E4", Point = "500,600,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E5", Point = "600,600,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E9", Point = "1000,600,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E10", Point = "1100,600,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E11", Point = "1200,600,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E12", Point = "1300,600,0", StoreID = 1, Status = 0, Type = 1 });

            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F2", Point = "300,700,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F3", Point = "400,700,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F4", Point = "500,700,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F5", Point = "600,700,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F9", Point = "1000,700,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F10", Point = "1100,700,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F11", Point = "1200,700,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F12", Point = "1300,700,0", StoreID = 1, Status = 0, Type = 1 });
            #endregion

            #region 第三排货架
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G2", Point = "300,800,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G3", Point = "400,800,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G4", Point = "500,800,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G5", Point = "600,800,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G9", Point = "1000,800,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G10", Point = "1100,800,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G11", Point = "1200,800,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G12", Point = "1300,800,0", StoreID = 1, Status = 0, Type = 1 });


            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H2", Point = "300,900,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H3", Point = "400,900,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H4", Point = "500,900,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H5", Point = "600,900,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H9", Point = "1000,900,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H10", Point = "1100,900,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H11", Point = "1200,900,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H12", Point = "1300,900,0", StoreID = 1, Status = 0, Type = 1 });

            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I2", Point = "300,1000,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I3", Point = "400,1000,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I4", Point = "500,1000,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I5", Point = "600,1000,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I9", Point = "1000,1000,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I10", Point = "1100,1000,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I11", Point = "1200,1000,0", StoreID = 1, Status = 0, Type = 1 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I12", Point = "1300,1000,0", StoreID = 1, Status = 0, Type = 1 });
            #endregion

            #endregion

            #region 拣货台
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "L4", Point = "500,1300,0", StoreID = 1, Status = 0, Type = 2 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "L11", Point = "1200,1300,0", StoreID = 1, Status = 0, Type = 2 });
            #endregion

            #region 充电桩

            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B1", Point = "200,300,0", StoreID = 1, Status = 0, Type = 4 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B13", Point = "1400,300,0", StoreID = 1, Status = 0, Type = 4 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E1", Point = "200,600,0", StoreID = 1, Status = 0, Type = 4 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E13", Point = "1400,600,0", StoreID = 1, Status = 0, Type = 4 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H1", Point = "200,900,0", StoreID = 1, Status = 0, Type = 4 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H13", Point = "1400,900,0", StoreID = 1, Status = 0, Type = 4 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J1", Point = "200,1100,0", StoreID = 1, Status = 0, Type = 4 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J13", Point = "1400,1100,0", StoreID = 1, Status = 0, Type = 4 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "K1", Point = "200,1200,0", StoreID = 1, Status = 0, Type = 4 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "K13", Point = "1400,1200,0", StoreID = 1, Status = 0, Type = 4 });

            #endregion

            #region 路口交叉点
            //Y轴方向中间过道
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B6", Point = "700,300,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B7", Point = "800,300,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B8", Point = "900,300,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E6", Point = "700,600,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E7", Point = "800,600,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E8", Point = "900,600,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H6", Point = "700,900,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H7", Point = "800,900,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H8", Point = "900,900,0", StoreID = 1, Status = 0, Type = 5 });
            //X轴方向 拣货台过道
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J2", Point = "300,1100,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J5", Point = "600,1100,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J9", Point = "1000,1100,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J12", Point = "1300,1100,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "K2", Point = "300,1200,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "K5", Point = "600,1200,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "K9", Point = "1000,1200,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "K12", Point = "1300,1200,0", StoreID = 1, Status = 0, Type = 5 });
            //拣货台单向过道
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "L2", Point = "300,1300,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "L5", Point = "600,1300,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "L9", Point = "1000,1300,0", StoreID = 1, Status = 0, Type = 5 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "L12", Point = "1300,1300,0", StoreID = 1, Status = 0, Type = 5 });
            #endregion
        }

        /// <summary>
        /// 深复制
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Graph grap = this.MemberwiseClone() as Graph;
            grap.NodeIdxList = new List<int>(this.NodeIdxList);
            grap.NodeList = new List<HeadNode>(this.NodeList);
            for (int i = 0; i < this.NodeCount; i++)
            {
                HeadNode node=new HeadNode ();
                node.Data = this.NodeList[i].Data;
                node.Name = this.NodeList[i].Name;
                node.Location = this.NodeList[i].Location;
                node.Edge = new List<Edge>(this.NodeList[i].Edge);
                grap.NodeList[i] = node;
            }
            
            return grap;
        }
    }
}