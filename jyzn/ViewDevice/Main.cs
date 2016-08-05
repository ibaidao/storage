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


namespace ViewDevice
{
    public partial class Main : Form
    {
        private const string MARK_STRING_FORMAT = "{0}{1}{2}", SEND_LABEL = "=> ", RECEIVE_LABEL = "<= ";

        public Main()
        {
            InitializeComponent();

            this.gbTrouble.Enabled = false;

            Controller.Devices.StartListenCommunicate(ShowReceivingMessage);
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
            Location loc = new Location (int.Parse(strX), int.Parse(strY),int.Parse(strZ));

            Protocol proto = new Protocol();
            proto.NeedAnswer = false;
            List<Function> functionList = new List<Function>();
            List<Location> locList = new List<Location> ();
            locList.Add(loc);
            this.CreateFunction(functionList, locList);
            proto.FunList = functionList;

            ErrorCode code = device.ReportStatus(proto);

            rtbRemark.Text += string.Format(MARK_STRING_FORMAT, SEND_LABEL, proto.FunList[0].Code, proto.FunList[0].TargetInfo);
        }

        private void rbItem_Click(object sender, EventArgs e)
        {
            this.gbTrouble.Enabled = sender as RadioButton == rbTrouble;
        }

        /// <summary>
        /// 整合异常模块
        /// </summary>
        /// <param name="functionList"></param>
        /// <param name="locList"></param>
        private void CreateFunctionTrouble(List<Function> functionList, List<Location> locList)
        {
            string strDistance = tbDistance.Text, strVoltage = tbVoltage.Text;
            if ((strDistance.Equals(string.Empty) && ckbBlock.Checked) ||
               (strVoltage.Equals(string.Empty) && ckbLowBattery.Checked))
            {
                MessageBox.Show("存在坐标值为空");
                return;
            }

            if (ckbBlock.Checked)
            {
                functionList.Add(new Function()
                {
                    Code = Status.GetDeviceFunctionByStatus(Models.RealDeviceStatus.MeetBalk),
                    TargetInfo = int.Parse(strDistance),
                    PathPoint = locList
                });
            }
            if (ckbLowBattery.Checked)
            {
                functionList.Add(new Function()
                {
                    Code = Status.GetDeviceFunctionByStatus(Models.RealDeviceStatus.LowBattery),
                    TargetInfo = int.Parse(strVoltage),
                    PathPoint = locList
                });
            }

            if (functionList.Count == 0)
            {
                functionList.Add(new Function()
                {
                    Code = Status.GetDeviceFunctionByStatus(Models.RealDeviceStatus.Standby),
                    PathPoint = locList
                });
            }

        }
        /// <summary>
        /// 整合异常模块
        /// </summary>
        /// <param name="functionList"></param>
        /// <param name="locList"></param>
        private void CreateFunction(List<Function> functionList, List<Location> locList)
        {
            //默认是心跳包
            FunctionCode code = FunctionCode.DeviceCurrentStatus;
            if (rbHoldShelf.Checked)
            {
                code = Status.GetDeviceFunctionByStatus(Models.RealDeviceStatus.OnHoldingShelf);
            }
            else if (rbCanPicking.Checked)
            {
                code = Status.GetDeviceFunctionByStatus(Models.RealDeviceStatus.OnPickStation);
            }
            else if (rbFreeShelf.Checked)
            {
                code = Status.GetDeviceFunctionByStatus(Models.RealDeviceStatus.OnFreeShelf);
            }

            if (rbTrouble.Checked)
            {
                this.CreateFunctionTrouble(functionList, locList);
            }
            else
            {
                functionList.Add(new Function()
                {
                    Code = code,
                    TargetInfo = Convert.ToInt32(tbDeviceID.Text),
                    PathPoint = locList
                });
            }
        }

        /// <summary>
        /// 显示接收到的数据
        /// </summary>
        /// <param name="proto"></param>
        private void ShowReceivingMessage(Protocol proto)
        {
            if (this.InvokeRequired)
            {
                Action<Protocol> action = new Action<Protocol>(ShowReceivingMessage);
                this.Invoke(action, proto);
                return;
            }
            
            StringBuilder pathInfo = new StringBuilder ();
            pathInfo.Append("（");
            pathInfo.Append(proto.FunList[0].TargetInfo);
            pathInfo.Append("）");
            foreach(Location loc in proto.FunList[0].PathPoint)
            {
                pathInfo.Append(loc.ToString ());
                pathInfo.Append(" > ");
            }

            this.rtbRemark.Text += string.Format(MARK_STRING_FORMAT, RECEIVE_LABEL, proto.FunList[0].Code, pathInfo.ToString());
        }

    }
}
