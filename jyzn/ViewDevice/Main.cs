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
            if (strX.Equals(string.Empty) || strY.Equals(string.Empty) || strZ.Equals(string.Empty))
            {
                MessageBox.Show("存在坐标值为空");
                return;
            }
            Core.Location loc = new Core.Location (int.Parse(strX), int.Parse(strY),int.Parse(strZ));

            List<Core.Function> functionList = new List<Core.Function>();
            List<Core.Location> locList = new List<Core.Location> ();
            locList.Add(loc);

            this.CreateFunction(functionList, locList);

            device.ReportStatus(functionList);
        }

        /// <summary>
        /// 整合功能模块
        /// </summary>
        /// <param name="functionList"></param>
        /// <param name="locList"></param>
        private void CreateFunction(List<Core.Function> functionList, List<Core.Location> locList)
        {
            string strDistance = tbDistance.Text, strVoltage = tbVoltage.Text;
             if((strDistance.Equals(string.Empty) && ckbBlock.Checked) ||
                (strVoltage.Equals(string.Empty) && ckbLowBattery.Checked))
             {
                 MessageBox.Show("存在坐标值为空");
                 return;
             }

            if (ckbBlock.Checked)
            {
                functionList.Add(new Core.Function()
                {
                    Code = Models.Logic.Status.GetDeviceFunctionByStatus(Models.RealDeviceStatus.MeetBalk),
                    TargetInfo = int.Parse(strDistance),
                    PathPoint = locList
                });
            }
            if (ckbLowBattery.Checked)
            {
                functionList.Add(new Core.Function()
                {
                    Code = Models.Logic.Status.GetDeviceFunctionByStatus(Models.RealDeviceStatus.LowBattery),
                    TargetInfo = int.Parse(strVoltage),
                    PathPoint = locList
                });
            }

            if (functionList.Count == 0)
            {
                functionList.Add(new Core.Function()
                {
                    Code = Models.Logic.Status.GetDeviceFunctionByStatus(Models.RealDeviceStatus.Standby),
                    PathPoint = locList
                });
            }

        }
    }
}
