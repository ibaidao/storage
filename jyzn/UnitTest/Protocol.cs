﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using System.Collections.Generic;
using Models;

namespace UnitTest
{
    [TestClass]
    public class Protocol
    {
        [TestMethod]
        public void Coder()
        {
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

            byte[] data = null;
            Core.Coder.EncodeByteData(proCharge,ref data);

            //解码
            Models.Protocol pResult = new Models.Protocol();
            byte[] da2 = new byte[proCharge.ByteCount - 3];
            Array.Copy(data, 3, da2, 0, da2.Length);
            Core.Coder.DecodeByteData(pResult, da2);
        }

    }
}
