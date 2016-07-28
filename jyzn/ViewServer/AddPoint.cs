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
    public partial class AddPoint : Form
    {
        private Controller.StoreInfo viewControl = null;
        private Action<int> refreshMainWindow = null;

        public AddPoint(Controller.StoreInfo storeControl,Action<int> realShowPoint)
        {
            this.viewControl = storeControl;
            this.refreshMainWindow = realShowPoint;

            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string strX = tbXValue.Text, strY = tbYValue.Text, strZ = tbZValue.Text, strName = tbName.Text;
            if (strX.Equals(string.Empty) || strY.Equals(string.Empty) || strZ.Equals(string.Empty))
            {
                MessageBox.Show("存在坐标值为空");
                return;
            }
            Models.Location loc = new Models.Location(int.Parse(strX), int.Parse(strY), int.Parse(strZ));


            int nodeID;
            Core.ErrorCode code = viewControl.AddPoint(strName, loc, out nodeID);
             if (code != Core.ErrorCode.OK)
             {
                 MessageBox.Show(Core.ErrorDescription.ExplainCode(code));
             }
             else
             {
                 //刷新主界面
                 if (this.refreshMainWindow != null)
                     this.refreshMainWindow(nodeID);
             }
        }
    }
}
