using System;
using System.Collections.Generic;
using Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class Paths
    {
        [TestMethod]
        //已验证正常，由于数据改变后需要调用改变后的数据，所以不再重复测试
        public void RemovePointTest()
        {

            //初始地图加载
            Core.Path path = Utilities.Singleton<Core.Path>.GetInstance();

            Graph graph = Models.GlobalVariable.RealGraphTraffic.Clone() as Graph;
            HeadNode startNode = graph.NodeList[4], endNode = graph.NodeList[16];

            //导出两点间最短路径
            List<HeadNode> pathGeneral = path.GetGeneralPath(startNode.Data, endNode.Data);


            //移除多个节点
            List<int> pointRemove = new List<int>();
            pointRemove.Add(graph.NodeList[5].Data);
            pointRemove.Add(graph.NodeList[7].Data);
            path.StopPoints(graph, pointRemove);

            //重新计算两点间最短路径
            List<HeadNode> pathGenera3 = path.GetNewPath(graph, startNode.Data, endNode.Data);
            
            Assert.AreNotEqual(pathGeneral.Count, 0);
            Assert.AreNotEqual(pathGenera3.Count, 0);
        }

        [TestMethod]
        ////已验证正常，由于数据改变后需要调用改变后的数据，所以不再重复测试
        public void RemovePathTest()
        {
            //初始地图加载
            Core.Path path = Utilities.Singleton<Core.Path>.GetInstance();

            Graph graph = Models.GlobalVariable.RealGraphTraffic.Clone() as Graph;
            HeadNode startNode = graph.NodeList[4], endNode = graph.NodeList[16];

            //导出两点间最短路径
            List<HeadNode> pathGeneral = path.GetGeneralPath(startNode.Data, endNode.Data);

            //移除多条边
            Dictionary<int, int> pathRemove = new Dictionary<int, int>();
            pathRemove.Add(graph.NodeList[5].Data, graph.NodeList[6].Data);
            pathRemove.Add(graph.NodeList[13].Data, graph.NodeList[14].Data);
            path.StopPath(graph, pathRemove);

            //重新计算两点间最短路径
            List<HeadNode> pathGenera3 = path.GetNewPath(graph, startNode.Data, endNode.Data);

            Assert.AreNotEqual(pathGeneral.Count, 0);
            Assert.AreNotEqual(pathGenera3.Count, 0);
        }
    }
}
