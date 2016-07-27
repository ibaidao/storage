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
using Core;


namespace ViewDevice
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Controller.Devices device = new Controller.Devices ();
            string strX = tbXValue.Text, strY = tbYValue.Text, strZ = tbZValue.Text;
            if(strX.Equals(string.Empty) || strY.Equals(string.Empty) || strZ.Equals(string.Empty))
                return;
            Core.Location loc = new Core.Location (int.Parse(strX), int.Parse(strY),int.Parse(strZ));

            device.CreateProtocol(loc, Models.RealDeviceStatus.Standby);
        }
    }
}
