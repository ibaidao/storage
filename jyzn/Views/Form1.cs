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
            DbEntity.DRealDevice.Insert(new RealDevice()
            {
                Location = "11,2,2",
                IPAddress = "aaaaa",
                Remarks = "dddddd",
                Status = 0
            });
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
