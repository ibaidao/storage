using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewServer
{
    public partial class OrderList : Form
    {
        public OrderList()
        {
            InitializeComponent();
        }

        struct OrderInfoShow
        {
            public string 订单编号 { get; set; }
            public string 订单状态 { get; set; }
            public int 优先级 { get; set; }
            public int 商品总数 { get; set; }
            public DateTime 订单时间 { get; set; }
            public string 备注 { get; set; }
        }

        private void OrderList_Load(object sender, EventArgs e)
        {

            Controller.Orders controlOrder = new Controller.Orders();
            List<Models.Orders> orderList =controlOrder.GetOrderList();
            List<OrderInfoShow> orderShow = new List<OrderInfoShow> ();
            foreach (Models.Orders order in orderList)
            {
                OrderInfoShow orderInfo = new OrderInfoShow()
                {
                    订单编号 = order.Code,
                    订单状态 = order.Status == (short)Models.StoreComponentStatus.OK ? "未处理" : "拣货中/已完成",
                    商品总数 = order.productCount,
                    优先级 = order.Priority,
                    订单时间 = order.CreateTime,
                    备注 = order.Remarks
                };
                orderShow.Add(orderInfo);
            }

            this.dgvOrderList.DataSource = orderShow;
        }
    }
}
