using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    class Protocol
    {
        [TestMethod]
        void Coder()
        {
            //编码
            Core.Protocol proCharge = new Core.Protocol();
            List<Function> funList = new List<Function>();
            proCharge.FunList = funList;
            //增加功能
            Function funCharge = new Function();
            funList.Add(funCharge);
            funCharge.Code = FunctionCode.OrderCharge;
            funCharge.TargetInfo = 22;
            //位置节点
            List<Location> locList = new List<Location>();
            funCharge.PathPoint = locList;
            locList.Add(new Location(1, 1, 1));
            locList.Add(new Location(1, 2, 1));
            funCharge.DataCount = (short)(funCharge.PathPoint.Count * 5 + 2);

            //第二个功能
            funCharge = new Function();
            funList.Add(funCharge);
            funCharge.Code = FunctionCode.OrderGetShelf;
            funCharge.TargetInfo = 12;
            //位置节点
            locList = new List<Location>();
            funCharge.PathPoint = locList;
            locList.Add(new Location(1, 1, 3));
            locList.Add(new Location(1, 2, 3));
            locList.Add(new Location(1, 2, 1));
            funCharge.DataCount = (short)(funCharge.PathPoint.Count * 5 + 2);

            byte[] data = new byte[1024];
            Core.Coder.EncodeInfo(proCharge, data);

            //解码
        }

    }
}
