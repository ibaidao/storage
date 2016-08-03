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
        public Location Location;

        /// <summary>
        /// 节点名称
        /// </summary>
        public String Name;

        /// <summary>
        /// 节点类型
        /// </summary>
        public StoreComponentType NodeType;

        /// <summary>
        /// 邻接表
        /// </summary>
        public List<Edge> Edge;

        /// <summary>
        /// 状态标志
        /// </summary>
        public Boolean Status;

        public HeadNode(int idx, string name, Location loc, StoreComponentType nodeType = StoreComponentType.CrossCorner)
        {
            Data = idx;
            Location = loc;
            Status = loc.Status;
            NodeType = nodeType;
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

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Status;

        public Edge(int index, int weight, int distance, bool status)
        {
            Idx = index;
            Weight = weight;
            Distance = distance;
            Status = status;
        }
    }

    /// <summary>
    /// 参数配置结构
    /// </summary>
    public struct GraphConfig
    {
        public int ColorIndex;

        public int Length;

        public int Width;
    }

    /// <summary>
    /// 无向图
    /// </summary>
    public class Graph : ICloneable
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
            RatioMapZoom = double.Parse(Utilities.IniFile.ReadIniData(InitSection, "RatioMapZoom"));
            RatioMapSelfZoom = double.Parse(Utilities.IniFile.ReadIniData(InitSection, "RatioMapSelfZoom"));
            PathWidth = int.Parse(Utilities.IniFile.ReadIniData(InitSection, "PathWidth"));
            MapSettingShowFlag = Utilities.IniFile.ReadIniData(InitSection, "CANSETMAP").Equals("Y");

            string[] strTemp = Utilities.IniFile.ReadIniData(InitSection, "SizeMap").Split(',');
            SizeGraph = new Location(int.Parse(strTemp[0]), int.Parse(strTemp[1]), 0);
            strTemp = Utilities.IniFile.ReadIniData(InitSection, "MapMarginLeftUp").Split(',');
            MapMarginLeftUp = new Location(int.Parse(strTemp[0]), int.Parse(strTemp[1]), 0);
            strTemp = Utilities.IniFile.ReadIniData(InitSection, "SizePickStation").Split(',');
            SizePickStation = new Location(int.Parse(strTemp[0]), int.Parse(strTemp[1]), 0);
            strTemp = Utilities.IniFile.ReadIniData(InitSection, "SizeRestore").Split(',');
            SizeRestore = new Location(int.Parse(strTemp[0]), int.Parse(strTemp[1]), 0);
            strTemp = Utilities.IniFile.ReadIniData(InitSection, "SizeCharger").Split(',');
            SizeCharger = new Location(int.Parse(strTemp[0]), int.Parse(strTemp[1]), 0);
            strTemp = Utilities.IniFile.ReadIniData(InitSection, "SizeShelf").Split(',');
            SizeShelf = new Location(int.Parse(strTemp[0]), int.Parse(strTemp[1]), 0);
            strTemp = Utilities.IniFile.ReadIniData(InitSection, "SizeDevice").Split(',');
            SizeDevice = new Location(int.Parse(strTemp[0]), int.Parse(strTemp[1]), 0);

            ColorStoreBack = int.Parse(Utilities.IniFile.ReadIniData(InitSection, "ColorStoreBack"));
            ColorCharger = int.Parse(Utilities.IniFile.ReadIniData(InitSection, "ColorCharger"));
            ColorCrossing = int.Parse(Utilities.IniFile.ReadIniData(InitSection, "ColorCrossing"));
            ColorDevice = int.Parse(Utilities.IniFile.ReadIniData(InitSection, "ColorDevice"));
            ColorDeviceShelf = int.Parse(Utilities.IniFile.ReadIniData(InitSection, "ColorDeviceShelf"));
            ColorBothPath = int.Parse(Utilities.IniFile.ReadIniData(InitSection, "ColorBothPath"));
            ColorSinglePath = int.Parse(Utilities.IniFile.ReadIniData(InitSection, "ColorSinglePath"));
            ColorStopPath = int.Parse(Utilities.IniFile.ReadIniData(InitSection, "ColorStopPath"));
            ColorPickStation = int.Parse(Utilities.IniFile.ReadIniData(InitSection, "ColorPickStation"));
            ColorRestore = int.Parse(Utilities.IniFile.ReadIniData(InitSection, "ColorRestore"));
            ColorShelf = int.Parse(Utilities.IniFile.ReadIniData(InitSection, "ColorShelf"));
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
        /// <param name="pointType">节点类型</param>
        public void AddPoint(int data, string name, Location loc, StoreComponentType pointType)
        {
            this.NodeIdxList.Add(data);
            this.NodeList.Add(new HeadNode(data, name, loc, pointType));
            this.NodeCount++;
        }

        /// <summary>
        /// 移除节点
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
        /// 关闭节点
        /// </summary>
        /// <param name="data">节点数据</param>
        public void StopPoint(int data)
        {
            int nodeIdx = this.GetIndexByData(data);
            HeadNode node = this.NodeList[nodeIdx];
            List<Edge> edge;
            //先停止指向当前节点的节点（边中含有当前节点）
            foreach (Edge item in node.Edge)
            {//依次访问节点连接的所有边
                edge = this.NodeList[item.Idx].Edge;
                for (int i = 0; i < edge.Count; i++)
                {//无向边是双向的有向边替代
                    if (edge[i].Idx == nodeIdx)
                    {
                        Edge tmpEdge = edge[i];
                        tmpEdge.Status = false;
                        edge[i] = tmpEdge;
                        break;
                    }
                }
                this.EdgeCount--;
            }
            //再停止当前节点的边
            this.NodeList.RemoveAt(nodeIdx);
            for (int i = 0; i < node.Edge.Count; i++)
            {
                Edge tmpEdge = node.Edge[i];
                tmpEdge.Status = false;
                node.Edge[i] = tmpEdge;
            }
            //最后停止当前节点
            node.Status = false;
            Location tmpLoc = node.Location;
            tmpLoc.Status = false;
            node.Location = tmpLoc;
        }


        /// <summary>
        /// 增加无向边（双向路）
        /// </summary>
        /// <param name="one">一个端点</param>
        /// <param name="two"></param>
        /// <param name="weight">权重</param>
        /// <param name="status">是否有效</param>
        public void AddEdge(int one, int two, int weight, bool status = true)
        {
            int oneIdx = this.GetIndexByData(one),
                twoIdx = this.GetIndexByData(two);
            if (oneIdx < 0 || twoIdx < 0)
            {
                throw new Exception("增加边失败，节点不存在");
            }

            int length = Location.Manhattan(NodeList[oneIdx].Location, NodeList[twoIdx].Location);

            //两条双向边代表无向边
            bool edgeExists = false;
            //忽略重复添加的边
            foreach (Edge edge in NodeList[oneIdx].Edge)
                if (edge.Idx == twoIdx)
                    edgeExists = true;
            if (!edgeExists)
                NodeList[oneIdx].Edge.Add(new Edge(twoIdx, weight, length, status));

            foreach (Edge edge in NodeList[twoIdx].Edge)
                if (edge.Idx == oneIdx)
                    edgeExists = true;
            if (!edgeExists)
                NodeList[twoIdx].Edge.Add(new Edge(oneIdx, weight, length, status));

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
        /// 改变一条边的可用状态
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <param name="useable">是否可用</param>
        public void ChangeEdgeUseable(int one, int two,bool useable)
        {
            int oneIdx = GetIndexByData(one),
                twoIdx = GetIndexByData(two);
            int oneEdgeCount = NodeList[oneIdx].Edge.Count,
                twoEdgeCount = NodeList[twoIdx].Edge.Count;
            for (int i = 0; i < oneEdgeCount; i++)
            {
                if (NodeList[oneIdx].Edge[i].Idx == twoIdx)
                {
                    Edge tmpEdge = NodeList[oneIdx].Edge[i];
                    tmpEdge.Status = useable;
                    NodeList[oneIdx].Edge[i] = tmpEdge;
                    break;
                }
            }
            for (int i = 0; i < twoEdgeCount; i++)
            {
                if (NodeList[twoIdx].Edge[i].Idx == oneIdx)
                {
                    Edge tmpEdge = NodeList[twoIdx].Edge[i];
                    tmpEdge.Status = useable;
                    NodeList[twoIdx].Edge[i] = tmpEdge;
                    break;
                }
            }
        }

        /// <summary>
        /// 增加一条有向边（指定方向的路）
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <param name="weight"></param>
        /// <param name="status"></param>
        public void AddDirectEdge(int start, int end, int weight, bool status = true)
        {
            int startIdx = this.GetIndexByData(start),
                endIdx = this.GetIndexByData(end);
            int length = Location.Manhattan(NodeList[startIdx].Location, NodeList[endIdx].Location);

            bool edgeExists = false;
            //忽略重复添加的边
            foreach (Edge edge in NodeList[startIdx].Edge)
                if (edge.Idx == endIdx)
                    edgeExists = true;
            if (!edgeExists)
                NodeList[startIdx].Edge.Add(new Edge(endIdx, weight, length, status));

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
        /// 生成初始化数据
        /// </summary>
        public static void InitialMap()
        {
            //地图节点            
            #region 货架
            #region 第一排货架
            object[] s = new object[80];
            s[0] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D12", Point = "1300,500,0", StoreID = 1, Status = 0, Type = 1 });
            s[1] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A2", Point = "300,200,0", StoreID = 1, Status = 0, Type = 1 });
            s[2] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A3", Point = "400,200,0", StoreID = 1, Status = 0, Type = 1 });
            s[3] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A4", Point = "500,200,0", StoreID = 1, Status = 0, Type = 1 });
            s[4] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A5", Point = "600,200,0", StoreID = 1, Status = 0, Type = 1 });
            s[5] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A9", Point = "1000,200,0", StoreID = 1, Status = 0, Type = 1 });
            s[6] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A10", Point = "1100,200,0", StoreID = 1, Status = 0, Type = 1 });
            s[7] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A11", Point = "1200,200,0", StoreID = 1, Status = 0, Type = 1 });
            s[8] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "A12", Point = "1300,200,0", StoreID = 1, Status = 0, Type = 1 });

            s[9] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C2", Point = "300,400,0", StoreID = 1, Status = 0, Type = 1 });
            s[10] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C3", Point = "400,400,0", StoreID = 1, Status = 0, Type = 1 });
            s[11] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C4", Point = "500,400,0", StoreID = 1, Status = 0, Type = 1 });
            s[12] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C5", Point = "600,400,0", StoreID = 1, Status = 0, Type = 1 });
            s[13] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C9", Point = "1000,400,0", StoreID = 1, Status = 0, Type = 1 });
            s[14] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C10", Point = "1100,400,0", StoreID = 1, Status = 0, Type = 1 });
            s[15] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C11", Point = "1200,400,0", StoreID = 1, Status = 0, Type = 1 });
            s[16] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "C12", Point = "1300,400,0", StoreID = 1, Status = 0, Type = 1 });
            #endregion

            #region 第二排货架
            s[17] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D2", Point = "300,500,0", StoreID = 1, Status = 0, Type = 1 });
            s[18] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D3", Point = "400,500,0", StoreID = 1, Status = 0, Type = 1 });
            s[19] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D4", Point = "500,500,0", StoreID = 1, Status = 0, Type = 1 });
            s[20] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D5", Point = "600,500,0", StoreID = 1, Status = 0, Type = 1 });
            s[21] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D9", Point = "1000,500,0", StoreID = 1, Status = 0, Type = 1 });
            s[22] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D10", Point = "1100,500,0", StoreID = 1, Status = 0, Type = 1 });
            s[23] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "D11", Point = "1200,500,0", StoreID = 1, Status = 0, Type = 1 });

            s[24] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F2", Point = "300,700,0", StoreID = 1, Status = 0, Type = 1 });
            s[25] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F3", Point = "400,700,0", StoreID = 1, Status = 0, Type = 1 });
            s[26] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F4", Point = "500,700,0", StoreID = 1, Status = 0, Type = 1 });
            s[27] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F5", Point = "600,700,0", StoreID = 1, Status = 0, Type = 1 });
            s[28] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F9", Point = "1000,700,0", StoreID = 1, Status = 0, Type = 1 });
            s[29] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F10", Point = "1100,700,0", StoreID = 1, Status = 0, Type = 1 });
            s[30] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F11", Point = "1200,700,0", StoreID = 1, Status = 0, Type = 1 });
            s[31] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "F12", Point = "1300,700,0", StoreID = 1, Status = 0, Type = 1 });
            #endregion

            #region 第三排货架
            s[32] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G2", Point = "300,800,0", StoreID = 1, Status = 0, Type = 1 });
            s[33] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G3", Point = "400,800,0", StoreID = 1, Status = 0, Type = 1 });
            s[34] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G4", Point = "500,800,0", StoreID = 1, Status = 0, Type = 1 });
            s[35] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G5", Point = "600,800,0", StoreID = 1, Status = 0, Type = 1 });
            s[36] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G9", Point = "1000,800,0", StoreID = 1, Status = 0, Type = 1 });
            s[37] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G10", Point = "1100,800,0", StoreID = 1, Status = 0, Type = 1 });
            s[38] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G11", Point = "1200,800,0", StoreID = 1, Status = 0, Type = 1 });
            s[39] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "G12", Point = "1300,800,0", StoreID = 1, Status = 0, Type = 1 });

            s[40] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I2", Point = "300,1000,0", StoreID = 1, Status = 0, Type = 1 });
            s[41] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I3", Point = "400,1000,0", StoreID = 1, Status = 0, Type = 1 });
            s[42] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I4", Point = "500,1000,0", StoreID = 1, Status = 0, Type = 1 });
            s[43] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I5", Point = "600,1000,0", StoreID = 1, Status = 0, Type = 1 });
            s[44] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I9", Point = "1000,1000,0", StoreID = 1, Status = 0, Type = 1 });
            s[45] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I10", Point = "1100,1000,0", StoreID = 1, Status = 0, Type = 1 });
            s[46] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I11", Point = "1200,1000,0", StoreID = 1, Status = 0, Type = 1 });
            s[47] = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "I12", Point = "1300,1000,0", StoreID = 1, Status = 0, Type = 1 });

            for (int i = 0; i <= 47; i++)
            {
                object ss = DbEntity.DShelf.Insert(new Shelf()
                {
                    LocationID = Convert.ToInt32(s[i]),
                    Layer = 4,
                    Surface = 2,
                    Type = 1,
                    Code = "02A211",
                    Address = "01020201;01020301",
                    LocHistory = s[i].ToString()
                });
                //DbEntity.DRealShelf.Insert(new RealShelf()
                //{
                //    OrderID = string.Empty,
                //    DeviceID = 0,
                //    GetOrderTime = DateTime.Parse("2016-07-30 10:10:10"),
                //    GetShelfTime = DateTime.Parse("2016-07-30 10:9:10"),
                //    ProductID = string.Empty,
                //    ProductCount = 0,
                //    ShelfID = Convert.ToInt32(ss),
                //    SkuID = string.Empty,
                //    StationID = string.Empty,
                //    Status = 1
                //});
            }
            #endregion

            #endregion

            #region 拣货台
            object p1 = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "L4", Point = "500,1300,0", StoreID = 1, Status = 0, Type = 2 });
            object p2 = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "L11", Point = "1200,1300,0", StoreID = 1, Status = 0, Type = 2 });
            DbEntity.DStation.Insert(new Station() { LocationID = Convert.ToInt32(p1), Location = "500,1300,0", Status = (short)Models.StoreComponentStatus.OK, Type = (short)Models.StoreComponentType.PickStation, Code = "PickStation1" });
            DbEntity.DStation.Insert(new Station() { LocationID = Convert.ToInt32(p2), Location = "1200,1300,0", Status = (short)Models.StoreComponentStatus.OK, Type = (short)Models.StoreComponentType.PickStation, Code = "PickStation2" });
            #endregion

            #region 充电桩

            object c1 = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B1", Point = "200,300,0", StoreID = 1, Status = 0, Type = 4 });
            object c2 = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B13", Point = "1400,300,0", StoreID = 1, Status = 0, Type = 4 });
            object c3 = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E1", Point = "200,600,0", StoreID = 1, Status = 0, Type = 4 });
            object c4 = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E13", Point = "1400,600,0", StoreID = 1, Status = 0, Type = 4 });
            object c5 = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H1", Point = "200,900,0", StoreID = 1, Status = 0, Type = 4 });
            object c6 = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H13", Point = "1400,900,0", StoreID = 1, Status = 0, Type = 4 });
            object c7 = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J1", Point = "200,1100,0", StoreID = 1, Status = 0, Type = 4 });
            object c8 = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J13", Point = "1400,1100,0", StoreID = 1, Status = 0, Type = 4 });
            object c9 = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "K1", Point = "200,1200,0", StoreID = 1, Status = 0, Type = 4 });
            object c10 = DbEntity.DStorePoints.Insert(new StorePoints() { Name = "K13", Point = "1400,1200,0", StoreID = 1, Status = 0, Type = 4 });
            DbEntity.DStation.Insert(new Station() { LocationID = Convert.ToInt32(c1), Location = "200,300,0", Status = (short)Models.StoreComponentStatus.OK, Type = (short)Models.StoreComponentType.Charger, Code = "Charge1" });
            DbEntity.DStation.Insert(new Station() { LocationID = Convert.ToInt32(c2), Location = "1400,300,0", Status = (short)Models.StoreComponentStatus.OK, Type = (short)Models.StoreComponentType.Charger, Code = "Charge2" });
            DbEntity.DStation.Insert(new Station() { LocationID = Convert.ToInt32(c3), Location = "200,600,0", Status = (short)Models.StoreComponentStatus.OK, Type = (short)Models.StoreComponentType.Charger, Code = "Charge3" });
            DbEntity.DStation.Insert(new Station() { LocationID = Convert.ToInt32(c4), Location = "1400,600,0", Status = (short)Models.StoreComponentStatus.OK, Type = (short)Models.StoreComponentType.Charger, Code = "Charge4" });
            DbEntity.DStation.Insert(new Station() { LocationID = Convert.ToInt32(c5), Location = "200,900,0", Status = (short)Models.StoreComponentStatus.OK, Type = (short)Models.StoreComponentType.Charger, Code = "Charge5" });
            DbEntity.DStation.Insert(new Station() { LocationID = Convert.ToInt32(c6), Location = "1400,900,0", Status = (short)Models.StoreComponentStatus.OK, Type = (short)Models.StoreComponentType.Charger, Code = "Charge6" });
            DbEntity.DStation.Insert(new Station() { LocationID = Convert.ToInt32(c7), Location = "200,1100,0", Status = (short)Models.StoreComponentStatus.OK, Type = (short)Models.StoreComponentType.Charger, Code = "Charge7" });
            DbEntity.DStation.Insert(new Station() { LocationID = Convert.ToInt32(c8), Location = "1400,1100,0", Status = (short)Models.StoreComponentStatus.OK, Type = (short)Models.StoreComponentType.Charger, Code = "Charge8" });
            DbEntity.DStation.Insert(new Station() { LocationID = Convert.ToInt32(c9), Location = "200,1200,0", Status = (short)Models.StoreComponentStatus.OK, Type = (short)Models.StoreComponentType.Charger, Code = "Charge9" });
            DbEntity.DStation.Insert(new Station() { LocationID = Convert.ToInt32(c10), Location = "1400,1200,0", Status = (short)Models.StoreComponentStatus.OK, Type = (short)Models.StoreComponentType.Charger, Code = "Charge10" });
            #endregion

            #region 路口交叉点
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B2", Point = "300,300,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B3", Point = "400,300,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B4", Point = "500,300,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B5", Point = "600,300,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B9", Point = "1000,300,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B10", Point = "1100,300,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B11", Point = "1200,300,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B12", Point = "1300,300,0", StoreID = 1, Status = 0, Type = 0 });

            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E2", Point = "300,600,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E3", Point = "400,600,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E4", Point = "500,600,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E5", Point = "600,600,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E9", Point = "1000,600,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E10", Point = "1100,600,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E11", Point = "1200,600,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E12", Point = "1300,600,0", StoreID = 1, Status = 0, Type = 0 });

            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H2", Point = "300,900,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H3", Point = "400,900,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H4", Point = "500,900,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H5", Point = "600,900,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H9", Point = "1000,900,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H10", Point = "1100,900,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H11", Point = "1200,900,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H12", Point = "1300,900,0", StoreID = 1, Status = 0, Type = 0 });

            //Y轴方向中间过道
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B6", Point = "700,300,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B7", Point = "800,300,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "B8", Point = "900,300,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E6", Point = "700,600,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E7", Point = "800,600,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "E8", Point = "900,600,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H6", Point = "700,900,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H7", Point = "800,900,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "H8", Point = "900,900,0", StoreID = 1, Status = 0, Type = 0 });
            //X轴方向 拣货台过道
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J2", Point = "300,1100,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J5", Point = "600,1100,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J6", Point = "700,1100,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J7", Point = "800,1100,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J8", Point = "900,1100,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J9", Point = "1000,1100,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "J12", Point = "1300,1100,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "K2", Point = "300,1200,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "K5", Point = "600,1200,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "K9", Point = "1000,1200,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "K12", Point = "1300,1200,0", StoreID = 1, Status = 0, Type = 0 });
            //拣货台单向过道
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "L2", Point = "300,1300,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "L5", Point = "600,1300,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "L9", Point = "1000,1300,0", StoreID = 1, Status = 0, Type = 0 });
            DbEntity.DStorePoints.Insert(new StorePoints() { Name = "L12", Point = "1300,1300,0", StoreID = 1, Status = 0, Type = 0 });
            #endregion

            #region 小车设备
            object de1 = DbEntity.DDevices.Insert(new Devices() { Code = "Car1", Status = 1, IPAddress = "192.168.1.105", Manufacturer = "s", Remarks = string.Empty });
            object de2 = DbEntity.DDevices.Insert(new Devices() { Code = "Car2", Status = 1, IPAddress = "192.168.1.105:8775", Manufacturer = "s", Remarks = string.Empty });
            object de3 = DbEntity.DDevices.Insert(new Devices() { Code = "Car3", Status = 1, IPAddress = "192.168.1.105:8776", Manufacturer = "s", Remarks = string.Empty });
            object de4 = DbEntity.DDevices.Insert(new Devices() { Code = "Car4", Status = 1, IPAddress = "192.168.1.105:8778", Manufacturer = "s", Remarks = string.Empty });
            object de5 = DbEntity.DDevices.Insert(new Devices() { Code = "Car5", Status = 1, IPAddress = "192.168.1.105:8779", Manufacturer = "s", Remarks = string.Empty });
            object de6 = DbEntity.DDevices.Insert(new Devices() { Code = "Car6", Status = 1, IPAddress = "192.168.1.105:8765", Manufacturer = "s", Remarks = string.Empty });
            object de7 = DbEntity.DDevices.Insert(new Devices() { Code = "Car7", Status = 1, IPAddress = "192.168.1.105:8885", Manufacturer = "s", Remarks = string.Empty });

            DbEntity.DRealDevice.Insert(new RealDevice() { DeviceID = Convert.ToInt32(de1), Status = 0, IPAddress = "192.168.1.105", LocationXYZ = "100,100,0" });
            DbEntity.DRealDevice.Insert(new RealDevice() { DeviceID = Convert.ToInt32(de2), Status = 0, IPAddress = "192.168.1.105:8775", LocationXYZ = "100,100,0" });
            DbEntity.DRealDevice.Insert(new RealDevice() { DeviceID = Convert.ToInt32(de3), Status = 0, IPAddress = "192.168.1.105:8776", LocationXYZ = "100,100,0" });
            DbEntity.DRealDevice.Insert(new RealDevice() { DeviceID = Convert.ToInt32(de4), Status = 0, IPAddress = "192.168.1.105:8778", LocationXYZ = "100,100,0" });
            DbEntity.DRealDevice.Insert(new RealDevice() { DeviceID = Convert.ToInt32(de5), Status = 0, IPAddress = "192.168.1.105:8779", LocationXYZ = "100,100,0" });
            DbEntity.DRealDevice.Insert(new RealDevice() { DeviceID = Convert.ToInt32(de6), Status = 0, IPAddress = "192.168.1.105:8765", LocationXYZ = "100,100,0" });
            DbEntity.DRealDevice.Insert(new RealDevice() { DeviceID = Convert.ToInt32(de7), Status = 0, IPAddress = "192.168.1.105:8885", LocationXYZ = "100,100,0" });
            #endregion

            #region 员工
            object sta1 = DbEntity.DStaff.Insert(new Staff() { Name = "Suoxd1", Sex = true, Age = 21, Phone = "150150150151", Address = "深圳南山1", Job = "Software1", Auth = "11101" });
            object sta2 = DbEntity.DStaff.Insert(new Staff() { Name = "Suoxd2", Sex = true, Age = 21, Phone = "150150150152", Address = "深圳南山1", Job = "Software1", Auth = "11101" });
            #endregion

            #region 商品
            object pro1 = Models.DbEntity.DSkuInfo.Insert(new Models.SkuInfo() { Color = "红色", Count = 10, Name = "水杯2", Size = "20*200*2000", Type = "300ml", Weight = 200 });
            object pro2 = Models.DbEntity.DSkuInfo.Insert(new Models.SkuInfo() { Color = "红色", Count = 10, Name = "水杯10", Size = "20*200*2000", Type = "300ml", Weight = 200 });
            object pro3 = Models.DbEntity.DSkuInfo.Insert(new Models.SkuInfo() { Color = "红色", Count = 20, Name = "水杯11", Size = "20*200*2000", Type = "300ml", Weight = 200 });
            object pro4 = Models.DbEntity.DSkuInfo.Insert(new Models.SkuInfo() { Color = "红色", Count = 20, Name = "水杯15", Size = "20*200*2000", Type = "300ml", Weight = 200 });
            object pro5 = Models.DbEntity.DSkuInfo.Insert(new Models.SkuInfo() { Color = "红色", Count = 20, Name = "水杯41", Size = "20*200*2000", Type = "300ml", Weight = 200 });
            object pro6 = Models.DbEntity.DSkuInfo.Insert(new Models.SkuInfo() { Color = "红色", Count = 10, Name = "水杯31", Size = "20*200*2000", Type = "300ml", Weight = 200 });
            object pro7 = Models.DbEntity.DSkuInfo.Insert(new Models.SkuInfo() { Color = "红色", Count = 20, Name = "水杯", Size = "20*200*2000", Type = "300ml", Weight = 200 });

            DbEntity.DProducts.Insert(new Products() { Count = 10, SkuID = Convert.ToInt32(pro7), ShelfID = Convert.ToInt32(s[1]), CellNum = 2, ProductName = "水杯；红色300ml", ProductionDate = DateTime.Parse("2015-07-01"), ExpireDate = DateTime.Parse("2016-12-31"), Specification = "20*200*2000", Weight = 200, UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"), SurfaceNum = 1, Status = 1 });
            DbEntity.DProducts.Insert(new Products() { Count = 10, SkuID = Convert.ToInt32(pro7), ShelfID = Convert.ToInt32(s[1]), CellNum = 2, ProductName = "水杯；红色300ml", ProductionDate = DateTime.Parse("2015-07-01"), ExpireDate = DateTime.Parse("2016-12-31"), Specification = "20*200*2000", Weight = 200, UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"), SurfaceNum = 1, Status = 1 });
            DbEntity.DProducts.Insert(new Products() { Count = 10, SkuID = Convert.ToInt32(pro1), ShelfID = Convert.ToInt32(s[2]), CellNum = 2, ProductName = "水杯2；红色300ml", ProductionDate = DateTime.Parse("2015-07-01"), ExpireDate = DateTime.Parse("2016-12-31"), Specification = "20*200*2000", Weight = 200, UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"), SurfaceNum = 1, Status = 1 });
            DbEntity.DProducts.Insert(new Products() { Count = 10, SkuID = Convert.ToInt32(pro2), ShelfID = Convert.ToInt32(s[10]), CellNum = 2, ProductName = "水杯10；红色300ml", ProductionDate = DateTime.Parse("2015-07-01"), ExpireDate = DateTime.Parse("2016-12-31"), Specification = "20*200*2000", Weight = 200, UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"), SurfaceNum = 1, Status = 1 });
            DbEntity.DProducts.Insert(new Products() { Count = 10, SkuID = Convert.ToInt32(pro3), ShelfID = Convert.ToInt32(s[11]), CellNum = 2, ProductName = "水杯11；红色300ml", ProductionDate = DateTime.Parse("2015-07-01"), ExpireDate = DateTime.Parse("2016-12-31"), Specification = "20*200*2000", Weight = 200, UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"), SurfaceNum = 1, Status = 1 });
            DbEntity.DProducts.Insert(new Products() { Count = 10, SkuID = Convert.ToInt32(pro3), ShelfID = Convert.ToInt32(s[11]), CellNum = 2, ProductName = "水杯11；红色300ml", ProductionDate = DateTime.Parse("2015-07-01"), ExpireDate = DateTime.Parse("2016-12-31"), Specification = "20*200*2000", Weight = 200, UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"), SurfaceNum = 1, Status = 1 });
            DbEntity.DProducts.Insert(new Products() { Count = 10, SkuID = Convert.ToInt32(pro4), ShelfID = Convert.ToInt32(s[15]), CellNum = 2, ProductName = "水杯15；红色300ml", ProductionDate = DateTime.Parse("2015-07-01"), ExpireDate = DateTime.Parse("2016-12-31"), Specification = "20*200*2000", Weight = 200, UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"), SurfaceNum = 1, Status = 1 });
            DbEntity.DProducts.Insert(new Products() { Count = 10, SkuID = Convert.ToInt32(pro5), ShelfID = Convert.ToInt32(s[41]), CellNum = 2, ProductName = "水杯41；红色300ml", ProductionDate = DateTime.Parse("2015-07-01"), ExpireDate = DateTime.Parse("2016-12-31"), Specification = "20*200*2000", Weight = 200, UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"), SurfaceNum = 1, Status = 1 });
            DbEntity.DProducts.Insert(new Products() { Count = 10, SkuID = Convert.ToInt32(pro5), ShelfID = Convert.ToInt32(s[41]), CellNum = 2, ProductName = "水杯41；红色300ml", ProductionDate = DateTime.Parse("2015-07-01"), ExpireDate = DateTime.Parse("2016-12-31"), Specification = "20*200*2000", Weight = 200, UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"), SurfaceNum = 1, Status = 1 });
            DbEntity.DProducts.Insert(new Products() { Count = 10, SkuID = Convert.ToInt32(pro6), ShelfID = Convert.ToInt32(s[31]), CellNum = 2, ProductName = "水杯31；红色300ml", ProductionDate = DateTime.Parse("2015-07-01"), ExpireDate = DateTime.Parse("2016-12-31"), Specification = "20*200*2000", Weight = 200, UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"), SurfaceNum = 1, Status = 1 });
            DbEntity.DProducts.Insert(new Products() { Count = 10, SkuID = Convert.ToInt32(pro4), ShelfID = Convert.ToInt32(s[15]), CellNum = 2, ProductName = "水杯15；红色300ml", ProductionDate = DateTime.Parse("2015-07-01"), ExpireDate = DateTime.Parse("2016-12-31"), Specification = "20*200*2000", Weight = 200, UpShelfTime = DateTime.Parse("2016-07-01 10:51:50"), SurfaceNum = 1, Status = 1 });
            #endregion

            #region 订单
            object item = DbEntity.DOrders.Insert(new Orders() { Code = "aef44542339", SkuList = "1,2;11,1", Priority = 0, Remarks = "aaaaaa" });
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
                HeadNode node = new HeadNode();
                node.Data = this.NodeList[i].Data;
                node.Name = this.NodeList[i].Name;
                node.Location = this.NodeList[i].Location;
                node.Edge = new List<Edge>(this.NodeList[i].Edge);
                grap.NodeList[i] = node;
            }

            return grap;
        }


        #region 仓库公共系数
        /// <summary>
        /// 地图设置的总开关
        /// </summary>
        public static bool MapSettingShowFlag { get; set; }

        /// <summary>
        /// 地图缩放比例
        /// </summary>
        public static double RatioMapZoom { get; set; }

        /// <summary>
        /// 实际仓库尺寸（cm）
        /// </summary>
        public static Location SizeGraph { get; set; }


        /// <summary>
        /// 显示地图左上边界
        /// </summary>
        public static Location MapMarginLeftUp { get; set; }

        /// <summary>
        /// 实际充电桩尺寸（cm）
        /// </summary>
        public static Location SizeCharger { get; set; }

        /// <summary>
        /// 实际小车尺寸（cm）
        /// </summary>
        public static Location SizeDevice { get; set; }

        /// <summary>
        /// 实际货架尺寸（cm）
        /// </summary>
        public static Location SizeShelf { get; set; }

        /// <summary>
        /// 实际拣货台尺寸（cm）
        /// </summary>
        public static Location SizePickStation { get; set; }

        /// <summary>
        /// 实际补货台尺寸（cm）
        /// </summary>
        public static Location SizeRestore { get; set; }

        /// <summary>
        /// 道路宽度
        /// </summary>
        public static int PathWidth { get; set; }

        /// <summary>
        /// 仓库背景色
        /// </summary>
        public static int ColorStoreBack { get; set; }

        /// <summary>
        /// 货架显示背景色
        /// </summary>
        public static int ColorShelf { get; set; }

        /// <summary>
        /// 充电器显示背景色
        /// </summary>
        public static int ColorCharger { get; set; }

        /// <summary>
        /// 补货台显示背景色
        /// </summary>
        public static int ColorRestore { get; set; }

        /// <summary>
        /// 拣货台显示背景色
        /// </summary>
        public static int ColorPickStation { get; set; }

        /// <summary>
        /// 空车显示背景色
        /// </summary>
        public static int ColorDevice { get; set; }

        /// <summary>
        /// 带有货架的小车显示背景色
        /// </summary>
        public static int ColorDeviceShelf { get; set; }

        /// <summary>
        /// 双向路线显示背景色
        /// </summary>
        public static int ColorBothPath { get; set; }

        /// <summary>
        /// 单向路背景色
        /// </summary>
        public static int ColorSinglePath { get; set; }

        /// <summary>
        /// 禁止通行路线显示背景色
        /// </summary>
        public static int ColorStopPath { get; set; }

        /// <summary>
        /// 路口显示背景色
        /// </summary>
        public static int ColorCrossing { get; set; }

        /// <summary>
        /// 地图节点
        /// </summary>
        public static string InitSection
        {
            get { return "StoreMap"; }
        }

        /// <summary>
        /// 地图缩放比例
        /// </summary>
        public static double RatioMapSelfZoom
        {
            get;
            set;
        }
        #endregion

    }
}