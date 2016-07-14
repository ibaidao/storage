﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models;
using Core;

namespace Views
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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
            Protocol pResult = Core.Coder.DecodeHead(data);
            Core.Coder.DecodeBody(data, pResult);
        }
    }
}
