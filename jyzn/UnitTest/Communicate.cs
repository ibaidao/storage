using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Core;


namespace UnitTest
{
    [TestClass]
    class Communicate
    {
        [TestMethod]
        private void ServerClientCommunicate()
        {
            Core.Communicate comm = new Core.Communicate();

            //启动服务器
            comm.StartListening();

            //编码
            Core.Protocol proCharge = new Core.Protocol();
            List<Function> funList = new List<Function>();
            proCharge.FunList = funList;
            proCharge.ByteCount = 17;
            //proCharge.NeedAnswer = AnswerFlag.YES;
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
            proCharge.ByteCount += funCharge.DataCount;
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
            proCharge.ByteCount += funCharge.DataCount;
            //编码数据
            byte[] data = null;
            Core.Coder.EncodeByteData(proCharge, ref data);

            string ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            //发送消息（需先打开服务器端口）
            comm.SendBuffer(ipAddress, data);
        }
    }
}
