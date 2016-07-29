using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;


namespace UnitTest
{
    [TestClass]
    public class Communicate
    {
        [TestMethod]
        public void ServerClientCommunicate()
        {
            //启动服务器
            ////测试用例执行这个代码时，进程占用资源，会导致无法重新生成，所以测试用例暂时不测试监控和收发消息
            //Core.Communicate.StartListening();

            //编码
            Models.Protocol proCharge = new Models.Protocol();
            List<Function> funList = new List<Function>();
            proCharge.FunList = funList;
            //proCharge.NeedAnswer = true;
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
            //编码数据
            byte[] data = null;
            Core.Coder.EncodeByteData(proCharge, ref data);

            string ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            //发送消息（需先打开服务器端口）
            //Core.Communicate.SendBuffer(ipAddress, data);
            
            //System.Threading.Thread.Sleep(1000);
            //Core.Communicate.StopListening();
        }
    }
}
