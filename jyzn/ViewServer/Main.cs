using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models;

namespace ViewServer
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //初始地图加载
            Models.Logic.Path path = new Models.Logic.Path();

            //Floyd算法计算任意两点间最短路径
            path.Floyd();

            //导出两点间最短路径
            List<HeadNode> pathGeneral = path.GetGeneralPath(5, 18);
        }
    }
}
