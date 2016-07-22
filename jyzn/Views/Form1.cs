using System;
using System.Collections.Generic;
using System.Net;
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
        static int i = 2235;
        Communicate comm = new Communicate();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 12, TwoPoint = 17, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 14, TwoPoint = 15, StoreID = 1, Type = 3, Status = 0 });
            DbEntity.DStorePaths.Insert(new StorePaths() { OnePoint = 15, TwoPoint = 16, StoreID = 1, Type = 3, Status = 0 });
            Models.Logic.Path path = new Models.Logic.Path();
            //path.Floyd();
            //List<HeadNode> pathGeneral = path.GetGeneralPath(5, 18);

            //List<HeadNode> pathGenera2 = path.Dijkstar(5, 18);

            List<int> pointRemove = new List<int>();
            pointRemove.Add(10);
            pointRemove.Add(11);
            path.StopPoints(pointRemove);
            List<HeadNode> pathGenera3 = path.Dijkstar(5, 18);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DbEntity.DRealOrders.Insert(new RealOrders()
            {
                StaffID = 1,
                ID = i++,
                ProductCount = 3,
                SkuList = "1,2;3,1",
                Status = 0,
                PickDevices=string.Empty,
                PickProducts =string.Empty
            });
        }
    }
}
