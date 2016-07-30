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
    public partial class PickStation : Form
    {
        public PickStation()
        {
            InitializeComponent();
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Text == "开始")
            {
                btn.Text = "结束";
            }
            else
            {
                btn.Text = "开始";
            }
        }
    }
}
