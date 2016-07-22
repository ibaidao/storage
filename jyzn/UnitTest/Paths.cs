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
        public void RemovePointTest()
        {
            //初始地图加载
            Models.Logic.Path path = new Models.Logic.Path();

            //Floyd算法计算任意两点间最短路径
            path.Floyd();
            
            //导出两点间最短路径
            List<HeadNode> pathGeneral = path.GetGeneralPath(5, 18);

            //移除一个节点
            List<int> pointRemove = new List<int>();
            pointRemove.Add(10);
            pointRemove.Add(11);
            path.StopPoints(pointRemove);

            //重新计算两点间最短路径
            List<HeadNode> pathGenera3 = path.Dijkstar(5, 18);
        }
        
        [TestMethod]
        public void RemovePathTest()
        {
            //初始地图加载
            Models.Logic.Path path = new Models.Logic.Path();

            //Floyd算法计算任意两点间最短路径
            path.Floyd();

            //导出两点间最短路径
            List<HeadNode> pathGeneral = path.GetGeneralPath(5, 18);

            //移除一个节点
            Dictionary<int, int> pathRemove = new Dictionary<int, int>();
            pathRemove.Add(10, 11);
            pathRemove.Add(17, 12);
            path.StopPath(pathRemove);

            //重新计算两点间最短路径
            List<HeadNode> pathGenera3 = path.Dijkstar(5, 18);
        }
    }
}
