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
        private readonly Controller.Picking picker = null;
        private Queue<int> orderBox = new Queue<int>();

        public PickOrders()
        {
            InitializeComponent();

            picker = new Controller.Picking();

            for (int i = 1; i <= 6; i++)
            {
                ((this.Controls.Find(string.Format("{0}{1}", PRE_PANEL_NAME, i), false)[0]) as Panel).BackColor = ORDER_EMPITY;
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

                Models.ErrorCode result = picker.StartingPickOrders(Convert.ToInt32(tbStaff.Text), Convert.ToInt32(lbStation.Text), ORDER_COUNT_ONCE);
                if (result != Models.ErrorCode.OK)
                {
                    MessageBox.Show(Models.ErrorDescription.ExplainCode(result));
                }
            }
            else if (btn.Text == "结束" || btn.Text == "稍后结束")
            {
                IsPickingFlag = false;
                bool finishPick = true;
                for (int i = 1; i <= 6; i++)
                {
                    if (((this.Controls.Find(string.Format("{0}{1}", PRE_PANEL_NAME, i), false)[0]) as Panel).BackColor != ORDER_FINISH)
                    {
                        finishPick = false; break;
                    }
                }
                if (!finishPick)
                {
                    MessageBox.Show("完成当前正在拣货订单才能结束");
                    btn.Text = "稍后结束";
                }
                else
                {
                    btn.Text = "开始";
                }
            }
        }

        /// <summary>
        /// 拣货进订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnBox_Click(object sender, EventArgs e)
        {
            Panel panelItem = sender as Panel;
            //仅货架来了，拣货时点击才有效
            if (panelItem.BackColor != PRODUCT_COMING) return;
            //状态显示更新
            bool finishPick = updateOrderStatus(panelItem);
            if (finishPick)
            {
                panelItem.BackColor = ORDER_FINISH;
                orderBox.Enqueue(int.Parse(panelItem.Name.Substring(PRE_PANEL_NAME.Length)));
            }
        }

        /// <summary>
        /// 换订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnBox_DoubleClick(object sender, EventArgs e)
        {
            Panel panelItem = sender as Panel;
            if (panelItem.BackColor != ORDER_FINISH)
            {
                MessageBox.Show("订单未完成");
                return;
            }

            if (IsPickingFlag)
            {//若完成订单，并且还没下班，并且有新的的时候，则换新订单
                int staffId = Convert.ToInt32(tbStaff.Text);
                int stationId = Convert.ToInt32(lbStation.Text);

                Models.ErrorCode result = picker.StartingPickOrders(staffId, stationId, 1);
                if (result != Models.ErrorCode.OK)
                {//更换新订单
                    panelItem.BackColor = ORDER_EMPITY;
                    MessageBox.Show(Models.ErrorDescription.ExplainCode(result));
                }
            }
            else
            {
                MessageBox.Show("休息状态，不再安排新订单");
            }
        }
        #endregion

        #region 内部方法 更新控件显示

        /// <summary>
        /// 来了新订单后，刷新显示待拣订单
        /// </summary>
        /// <param name="orderInfo">订单类型;订单1编号,数量1;订单2编号,数量2</param>
        private void refreshOrdersPanel(string orderInfo)
        {
            string[] orders = orderInfo.Split(';');

            if (Convert.ToInt32(orders[0]) == 1)
            {
                if (orders.Length == 1)
                {
                    lbOrderCount.Text = "0";
                    return;
                }
                lbOrderCount.Text = (orders.Length - 1).ToString();

                for (int i = 1; i < orders.Length; i++)
                {
                    string[] productCount = orders[i].Split(',');
                    ((this.Controls.Find(string.Format("{0}{1}", PRE_PANEL_NAME, i), false)[0]) as Panel).BackColor = ORDER_START_PICK;
                    ((this.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_ID, i), false)[0]) as Label).Text = productCount[0];
                    ((this.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_STATUS, i), false)[0]) as Label).Text = string.Format("0/{0}", productCount[1]);
                    //pnBox1.BackColor = ORDER_START_PICK;
                    //lbOrder1.Text = realOrderList[0].OrderID.ToString();
                    //lbStatus1.Text = string.Format("0 / {0}", realOrderList[0].ProductCount);
                }
            }
            else
            {
                int panelIdx = orderBox.Dequeue();
                string[] productCount = orders[1].Split(',');
                ((this.Controls.Find(string.Format("{0}{1}", PRE_PANEL_NAME, panelIdx), false)[0]) as Panel).BackColor = ORDER_START_PICK;
                ((this.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_ID, panelIdx), false)[0]) as Label).Text = productCount[0];
                ((this.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_STATUS, panelIdx), false)[0]) as Label).Text = string.Format("0/{0}", productCount[1]);
            }
        }

        /// <summary>
        /// 拣货员从货架拿出商品时，点亮对应订单面板
        /// </summary>
        /// <param name="orderId"></param>
        private void LightUpOrderPanel(string orderId)
        {
            int idx;
            for (idx = 1; idx <= ORDER_COUNT_ONCE; idx++)
            {
                if ((this.Controls.Find(string.Format(PRE_LABEL_ORDER_ID, idx), false)[0] as Label).Text == orderId)
                    break;
            }
            if (idx > ORDER_COUNT_ONCE)
            {
                MessageBox.Show("没找到对应订单");
            }
            ((this.Controls.Find(string.Format("{0}{1}", PRE_PANEL_NAME, idx), false)[0]) as Panel).BackColor = PRODUCT_COMING;
        }

        /// <summary>
        /// 拣货放入订单箱后，更新订单状态
        /// </summary>
        /// <param name="panel"></param>
        /// <returns>是否已完成拣货</returns>
        private bool updateOrderStatus(Panel panel)
        {
            string idx = panel.Name.Substring(panel.Name.Length - 1);
            Label lbOrder = panel.Controls.Find(string.Format(PRE_LABEL_ORDER_ID, idx), false)[0] as Label;
            Label lbStatus = panel.Controls.Find(string.Format(PRE_LABEL_ORDER_STATUS, idx), false)[0] as Label;
            //更改数据库记录
            Models.ErrorCode code = picker.PickProduct(Convert.ToInt32(lbOrder.Text), 1, 1, 1);
            if (code != Models.ErrorCode.OK)
            {
                MessageBox.Show(Models.ErrorDescription.ExplainCode(code));
                return false;
            }
            //更新数量状态
            string[] itemCount = lbStatus.Text.Split('/');
            int countNow = Convert.ToInt32(itemCount[0]) + 1, countAll = Convert.ToInt32(itemCount[1]);
            lbStatus.Text = string.Format("{0}/{1}", countNow, itemCount[1]);
            panel.BackColor = ORDER_START_PICK;

            return countNow == countAll;
        }
        #endregion

        private void handlerServerOrder(Models.FunctionCode funCode,string strParam)
        {
            if (this.InvokeRequired)
            {
                Action<Models.FunctionCode, string> action = new Action<Models.FunctionCode, string>(handlerServerOrder);
                this.Invoke(action, funCode, strParam);
                return;
            }
            switch (funCode)
            {
                case Models.FunctionCode.SystemAssignOrders://分配订单
                    this.refreshOrdersPanel(strParam);
                    break;
                case Models.FunctionCode.SystemProductInfo://显示商品信息
                    break;
                case Models.FunctionCode.SystemProductOrder://点亮商品订单
                    this.LightUpOrderPanel(strParam);
                    break;


                default: break;
            }
        }
    }
}