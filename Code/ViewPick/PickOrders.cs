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
        private Color PRODUCT_COMING = Color.DarkBlue, ORDER_FINISH = Color.Red, ORDER_START_PICK = Color.SeaGreen, ORDER_EMPITY = Color.Gray;
        private bool IsPickingFlag = false;
        private readonly int stationId;
        private int currentShelfId = 0, lastOrderIdx;
        private readonly Controller.Picking picker = null;
        private Queue<int> orderBox = new Queue<int>();
        private PickStation stationWindow = new PickStation();

        public PickOrders()
        {
            InitializeComponent();

            this.stationId = int.Parse(Utilities.IniFile.ReadIniData("StationSelf", "PickID"));
            picker = new Controller.Picking();
            picker.StartListenCommunicate(handlerServerOrder);
            for (int i = 1; i <= 6; i++)
            {
                ((this.Controls.Find(string.Format("{0}{1}", PRE_PANEL_NAME, i), true)[0]) as Panel).BackColor = ORDER_EMPITY;
                orderBox.Enqueue(i);
            }
            stationWindow.Show();
        }

        #region 界面交互 事件

        private void PickOrders_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Text == "开始")
            {
                btn.Text = "结束";
                IsPickingFlag = true;

                Models.ErrorCode result = picker.StartingPickOrders(Convert.ToInt32(tbStaff.Text), this.stationId, ORDER_COUNT_ONCE);
                if (result != Models.ErrorCode.OK)
                {
                    MessageBox.Show(Models.ErrorDescription.ExplainCode(result));
                }
            }
            else if (btn.Text == "结束" || btn.Text == "稍后结束")
            {
                IsPickingFlag = false;
                if (Convert.ToInt32(lbOrderCount.Text) > 0)
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
            updateOrderStatus(panelItem);
            //如果结束了则发送下班
            if (IsPickingFlag) return;

            bool orderFinish = true;
            for (int i = 1; i <= 6; i++)
            {
                Color tmpPnlColor = ((this.Controls.Find(string.Format("{0}{1}", PRE_PANEL_NAME, i), true)[0]) as Panel).BackColor;
                if (tmpPnlColor == ORDER_START_PICK || tmpPnlColor == PRODUCT_COMING)
                {
                    orderFinish = false;
                    break;
                }
            }
            if (orderFinish)
            {
                picker.EndingPickOrders(Convert.ToInt32(tbStaff.Text), this.stationId);
                System.Threading.Thread.Sleep(500);
                this.Close();
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

            int idx = int.Parse(panelItem.Name.Substring(panelItem.Name.Length - 1));
            panelItem.BackColor = ORDER_EMPITY;
            ((this.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_ID, idx), true)[0]) as Label).Text = "订单编号";
            ((this.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_STATUS, idx), true)[0]) as Label).Text = "拣货进度";

            if (IsPickingFlag)
            {//若完成订单，并且还没下班，并且有新的的时候，则换新订单
                int staffId = Convert.ToInt32(tbStaff.Text);
                Models.ErrorCode result = picker.StartingPickOrders(staffId, this.stationId, 1);
                if (result != Models.ErrorCode.OK)
                {//更换新订单
                    panelItem.BackColor = ORDER_EMPITY;
                    MessageBox.Show(Models.ErrorDescription.ExplainCode(result));
                }
            }
        }
        #endregion

        #region 内部方法 更新控件显示

        /// <summary>
        /// 来了新订单后，刷新显示待拣订单
        /// </summary>
        /// <param name="orderInfo">订单1编号,数量1;订单2编号,数量2</param>
        private void refreshOrdersPanel(string orderInfo)
        {
            if (orderInfo == string.Empty)
            {
                MessageBox.Show("暂无新订单");
                return;
            }
            string[] orders = orderInfo.Split(';');
            lbOrderCount.Text = (orders.Length + ORDER_COUNT_ONCE - orderBox.Count).ToString();

            for (int i = 0; i < orders.Length; i++)
            {
                int panelIdx = orderBox.Dequeue();
                string[] productCount = orders[i].Split(',');
                ((this.Controls.Find(string.Format("{0}{1}", PRE_PANEL_NAME, panelIdx), true)[0]) as Panel).BackColor = ORDER_START_PICK;
                ((this.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_ID, panelIdx), true)[0]) as Label).Text = productCount[0];
                ((this.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_STATUS, panelIdx), true)[0]) as Label).Text = string.Format("0/{0}", productCount[1]);
            }
        }

        /// <summary>
        /// 拣货员从货架拿出商品时，点亮对应订单面板
        /// </summary>
        /// <param name="orderId">订单ID，商品ID，SkuID</param>
        private void LightUpOrderPanel(string orderId)
        {
            string[] productInfo = orderId.Split(',');
            int idx;
            for (idx = 1; idx <= ORDER_COUNT_ONCE; idx++)
            {
                if ((this.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_ID, idx), true)[0] as Label).Text == productInfo[0])
                    break;
            }
            if (idx > ORDER_COUNT_ONCE)
            {
                MessageBox.Show("没找到对应订单，请确认商品是否正确");
                return;
            }
            ((this.Controls.Find(string.Format("{0}{1}", PRE_PANEL_NAME, idx), true)[0]) as Panel).BackColor = PRODUCT_COMING;
            ((this.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_STATUS, idx), true)[0]) as Label).Tag = productInfo[1];
            ((this.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_ID, idx), true)[0]) as Label).Tag = productInfo[2];
        }

        /// <summary>
        /// 显示商品的库位信息
        /// </summary>
        /// <param name="product">货架;货架剩余商品标志;商品所在库位;商品条码;货架库位信息;商品名称</param>
        private void ShowProductInfo(string product)
        {
            string strPara = string.Empty;
            if (!product.Equals(string.Empty))
            {
                int signIdx = product.IndexOf(';');
                this.currentShelfId = Convert.ToInt32(product.Substring(0, signIdx));
                strPara = product.Substring(signIdx + 1);
            }
            stationWindow.UpdateProductInfo(strPara);
        }

        /// <summary>
        /// 拣货处理结果
        /// </summary>
        /// <param name="strResult">成败,失败原因</param>
        private void ShowPickResult(string strResult)
        {
            int signIdx = strResult.IndexOf(',');
            int succ = int.Parse(strResult.Substring(0, signIdx));
            if (succ != (int)Models.StoreComponentStatus.OK)
            {
                MessageBox.Show(strResult.Substring(signIdx + 1));
                return;
            }
            //更新数量状态
            Panel panel = (this.Controls.Find(string.Format("{0}{1}", PRE_PANEL_NAME, lastOrderIdx), true)[0]) as Panel;
            Label lbStatus = panel.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_STATUS, lastOrderIdx), true)[0] as Label;
            string[] itemCount = lbStatus.Text.Split('/');
            int countNow = Convert.ToInt32(itemCount[0]) + 1, countAll = Convert.ToInt32(itemCount[1]);
            lbStatus.Text = string.Format("{0}/{1}", countNow, itemCount[1]);
            if (countNow >= countAll)
            {
                panel.BackColor = ORDER_FINISH;
                orderBox.Enqueue(lastOrderIdx);
                int orderCount = int.Parse(lbOrderCount.Text);
                lbOrderCount.Text = (orderCount - 1).ToString();
            }
            else
            {
                panel.BackColor = ORDER_START_PICK;
            }
        }

        /// <summary>
        /// 拣货放入订单箱
        /// </summary>
        /// <param name="panel"></param>
        private void updateOrderStatus(Panel panel)
        {
            lastOrderIdx = int.Parse(panel.Name.Substring(panel.Name.Length - 1));
            Label lbOrder = panel.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_ID, lastOrderIdx), true)[0] as Label;
            Label lbStatus = panel.Controls.Find(string.Format("{0}{1}", PRE_LABEL_ORDER_STATUS, lastOrderIdx), true)[0] as Label;
            //更改数据库记录
            Models.ErrorCode code = picker.PickProduct(currentShelfId, Convert.ToInt32(lbOrder.Text), Convert.ToInt32(lbStatus.Tag), Convert.ToInt32(lbOrder.Tag));
            if (code != Models.ErrorCode.OK)
            {
                MessageBox.Show(Models.ErrorDescription.ExplainCode(code));
            }
        }
        #endregion

        /// <summary>
        /// 汇报当前状态
        /// </summary>
        /// <param name="strParam"></param>
        private void reportStatus(string strParam)
        {
            int staffId = Convert.ToInt32(tbStaff.Text);
            int freeOrder = orderBox.Count;

            Models.ErrorCode result = picker.ReportStatus(staffId, this.stationId, ORDER_COUNT_ONCE, freeOrder);
            if (result != Models.ErrorCode.OK)
            {
                MessageBox.Show(Models.ErrorDescription.ExplainCode(result));
            }
        }

        private void handlerServerOrder(Models.FunctionCode funCode, string[] strParam)
        {
            if (this.InvokeRequired)
            {
                Action<Models.FunctionCode, string[]> action = new Action<Models.FunctionCode, string[]>(handlerServerOrder);
                this.Invoke(action, funCode, strParam);
                return;
            }
            switch (funCode)
            {
                case Models.FunctionCode.SystemAssignOrders://分配订单
                    this.refreshOrdersPanel(strParam[0]);
                    break;
                case Models.FunctionCode.SystemProductInfo://显示商品信息
                    this.ShowProductInfo(strParam[0]);
                    break;
                case Models.FunctionCode.SystemProductOrder://点亮商品订单
                    this.LightUpOrderPanel(strParam[0]);
                    break;
                case Models.FunctionCode.SystemPickerResult://拣货处理结果
                    this.ShowPickResult(strParam[0]);
                    this.ShowProductInfo(strParam.Length > 1?strParam[1]:string.Empty);
                    break;
                case Models.FunctionCode.SystemAskPickerStatus://系统查看状态
                    this.reportStatus(strParam[0]);
                    break;

                default: break;
            }
        }
    }
}