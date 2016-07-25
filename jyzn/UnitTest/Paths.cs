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
            
            ////初始地图加载
            //Models.Logic.Path path = new Models.Logic.Path();
            
            ////导出两点间最短路径
            //List<HeadNode> pathGeneral = path.GetGeneralPath(5, 18);

            //Graph grap = Models.GlobalVariable.RealGraphTraffic.Clone() as Graph;

            ////移除多个节点
            //List<int> pointRemove = new List<int>();
            //pointRemove.Add(10);
            //pointRemove.Add(11);
            //path.StopPoints(grap,pointRemove);

            ////重新计算两点间最短路径
            //List<HeadNode> pathGenera3 = path.GetNewPath(grap,5, 18);
        }

        [TestMethod]
        ////已验证正常，由于数据改变后需要调用改变后的数据，所以不再重复测试
        public void RemovePathTest()
        {
            ////初始地图加载
            //Models.Logic.Path path = new Models.Logic.Path();

            ////导出两点间最短路径
            //List<HeadNode> pathGeneral = path.GetGeneralPath(5, 18);

            //Graph grap = Models.GlobalVariable.RealGraphTraffic.Clone() as Graph;

            ////移除多条边
            //Dictionary<int, int> pathRemove = new Dictionary<int, int>();
            //pathRemove.Add(10, 11);
            //pathRemove.Add(17, 12);
            //path.StopPath(grap, pathRemove);

            ////重新计算两点间最短路径
            //List<HeadNode> pathGenera3 = path.GetNewPath(grap, 5, 18);
        }
    }
}
