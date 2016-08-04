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
    public partial class Orders : Form
    {
        public Orders()
        {
            InitializeComponent();
        }

        private void Orders_Load(object sender, EventArgs e)
        {
            Controller.Products controlProduct = new Controller.Products();

            this.dgvProducts.DataSource = controlProduct.GetAllProducts();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Dictionary<int,int> orderProducts = new Dictionary<int,int> ();
            foreach (DataGridViewRow dr in this.dgvProducts.Rows)
            {
                DataGridViewCheckBoxCell itemCheck = dr.Cells["itemCheck"] as DataGridViewCheckBoxCell;
                if (Convert.ToBoolean(itemCheck.Value))
                {
                    DataGridViewTextBoxCell itemNum = dr.Cells["itemNum"] as DataGridViewTextBoxCell;
                    int num = Convert.ToInt32(itemNum.Value);
                    if (num > 0)
                    {
                        DataGridViewTextBoxCell itemID = dr.Cells["ID"] as DataGridViewTextBoxCell;
                        int product = Convert.ToInt32(itemID.Value);
                        orderProducts.Add(product, num);
                    }
                    itemCheck.Value = false;
                    itemNum.Value = 0;
                }
            }

            if (orderProducts.Count > 0)
            {
                short productCount = 0;
                string orderCode = DateTime.Now.ToString("yyyyMMddHHmmss");
                string skuInfo = string.Empty;
                foreach (int idx in orderProducts.Keys)
                {
                    skuInfo += string.Format("{0},{1};", idx, orderProducts[idx]);
                    productCount += (short)orderProducts[idx];
                }

                Controller.Orders controlOrder = new Controller.Orders();
                Models.ErrorCode result = controlOrder.ImportOneOrder(orderCode, skuInfo.Remove(skuInfo.Length - 1), productCount);

                MessageBox.Show(Models.ErrorDescription.ExplainCode(result));
            }
            else
            {
                MessageBox.Show("商品数量为0");
            }
        }
    }
}
