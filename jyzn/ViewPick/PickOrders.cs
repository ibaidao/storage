using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewPick
{
    public partial class PickOrders : Form
    {
        private const int ORDER_COUNT_ONCE = 6;
        private const string PRE_PANEL_NAME = "pnBox", PRE_LABEL_ORDER_ID = "lbOrder", PRE_LABEL_ORDER_STATUS = "lbStatus";
        private Color PRODUCT_COMING = Color.DarkSlateBlue, ORDER_FINISH = Color.Red, ORDER_START_PICK = Color.SeaGreen, ORDER_EMPITY = Color.Gray;
        private bool IsPickingFlag = false;
        private readonly Controller.Orders order = null;

        public PickOrders()
        {
            InitializeComponent();

            order = new Controller.Orders();
            for (int i = 1; i <= 6; i++)
            {
                ((this.Controls.Find(string.Format("{0}{1}", PRE_PANEL_NAME,i), false)[0]) as Panel).BackColor = ORDER_EMPITY;
            }
        }

        #region 界面交互 事件
        private void btnSwitch_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Text == "开始")
            {
                btn.Text = "结束";
                IsPickingFlag = true;

                int staffId = Convert.ToInt32(tbStaff.Text);
                this.showAllOrders(order.GetStartOrders(staffId, ORDER_COUNT_ONCE));
            }
            else
            {
                btn.Text = "开始";
                IsPickingFlag = false;
            }
        }

        private void pnBox_Click(object sender, EventArgs e)
        {
            Panel panelItem = sender as Panel;
            //状态显示更新
            bool finishPick = updateOrderStatus(panelItem);
            //若完成订单，并且还没下班，并且有新的的时候，则换新订单
            if (finishPick && IsPickingFlag)
            {
                int staffId = Convert.ToInt32(tbStaff.Text);
                int orderId = order.GetNewOrders(staffId);
                if (orderId > 0)
                {//更换新订单
                    restartOrder(orderId, panelItem);
                }
                else
                {
                    panelItem.BackColor = ORDER_EMPITY;
                }
            }
        }
        #endregion 

        /// <summary>
        /// 显示所有待拣订单
        /// </summary>
        /// <param name="orderIds"></param>
        private void showAllOrders(List<int> orderIds)
        {
            List<Models.RealOrders> realOrderList = order.GetRealOrderList(orderIds);
            if(realOrderList ==null || realOrderList.Count == 0) {
            lbOrderCount.Text = "0";
                return;
            }
            lbOrderCount.Text = realOrderList.Count .ToString ();

            for (int i = 0; i < realOrderList.Count; i++)
            {
                ((this.Controls.Find(string.Format("{0}{1}",PRE_LABEL_ORDER_ID, i + 1), false)[0]) as Label).Text = realOrderList[0].OrderID.ToString();
                ((this.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_STATUS,i + 1), false)[0]) as Label).Text = string.Format("0/{0}", realOrderList[0].ProductCount);
                //lbOrder1.Text = realOrderList[0].OrderID.ToString();
                //lbStatus1.Text = string.Format("0 / {0}", realOrderList[0].ProductCount);
            }
        }

        /// <summary>
        /// 为面板换新订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="panel"></param>
        private void restartOrder(int orderId, Panel panel)
        {
            string idx = panel.Name.Substring(panel.Name.Length - 2);

            Models.RealOrders orderInfo = order.GetRealOrder(orderId);

            ((panel.Controls.Find(string.Format("{0}{1}",PRE_LABEL_ORDER_ID, idx), false)[0]) as Label).Text = orderInfo.OrderID.ToString();
            ((panel.Controls.Find(string.Format("{0}{1}",PRE_LABEL_ORDER_STATUS, idx), false)[0]) as Label).Text = string.Format("0/{0}", orderInfo.ProductCount);
        }

        /// <summary>
        /// 订单拣货后，更新状态
        /// </summary>
        /// <param name="panel"></param>
        /// <returns>是否已完成拣货</returns>
        private bool updateOrderStatus(Panel panel)
        {
            string idx = panel.Name.Substring(panel.Name.Length-2);
            Label lbStatus = panel.Controls.Find(string.Format(PRE_LABEL_ORDER_STATUS, idx), false)[0] as Label;
            //更新数量状态
            string[] itemCount = lbStatus.Text.Split('/');
            int countNow = Convert.ToInt32(itemCount[0]) + 1, countAll = Convert.ToInt32(itemCount[1]);
            lbStatus.Text = string.Format("{0}/{1}", countNow, itemCount[1]);
            //标签颜色
            panel.BackColor = ORDER_START_PICK;

            return countNow == countAll;
        }

    }
}
