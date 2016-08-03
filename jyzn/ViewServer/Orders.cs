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

        }
    }
}
