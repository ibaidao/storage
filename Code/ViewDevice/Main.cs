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
        private const string MARK_STRING_FORMAT = "{0}{1}:{2}\r\n", SEND_LABEL = "=> ", RECEIVE_LABEL = "<= ";
        private List<Location> pathList = new List<Location>();
        private readonly int deviceId;

        public Main()
        {
            InitializeComponent();

            this.gbTrouble.Enabled = false;
            this.deviceId = int.Parse(Utilities.IniFile.ReadIniData("DeviceSelf", "CarID"));

            Controller.Devices.StartListenCommunicate(ShowReceivingMessage);
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Controller.Devices device = new Controller.Devices ();
            Location loc = GetCurrentLocation();

            Protocol proto = new Protocol();
            proto.NeedAnswer = ckbBackFlag.Checked;
            List<Function> functionList = new List<Function>();
            List<Location> locList = new List<Location> ();
            locList.Add(loc);
            this.CreateFunction(functionList, locList);
            proto.FunList = functionList;

            ErrorCode code = device.ReportStatus(proto);
            if (code != ErrorCode.OK)
                MessageBox.Show(Models.ErrorDescription.ExplainCode(code));

            rtbRemark.Text += string.Format(MARK_STRING_FORMAT, SEND_LABEL, proto.FunList[0].Code, proto.FunList[0].TargetInfo);
        }

        private void btnHeart_Click(object sender, EventArgs e)
        {
            this.ReportStatus(FunctionCode.DeviceCurrentStatus);
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            if (pathList.Count > 0)
            {
                tbXValue.Text = pathList[0].XPos.ToString();
                tbYValue.Text = pathList[0].YPos.ToString();
                tbZValue.Text = pathList[0].ZPos.ToString();

                pathList.RemoveAt(0);

                this.ReportStatus(FunctionCode.DeviceCurrentStatus);
            }
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
                MessageBox.Show("异常数值为空");
                return;
            }

            if (ckbBlock.Checked)
            {
                functionList.Add(new Function()
                {
                    Code = FunctionCode.DeviceMeetBalk,
                    TargetInfo = int.Parse(strDistance),
                    PathPoint = locList
                });
            }
            if (ckbLowBattery.Checked)
            {
                functionList.Add(new Function()
                {
                    Code = FunctionCode.DeviceLowBattery,
                    TargetInfo = int.Parse(strVoltage),
                    PathPoint = locList
                });
            }

            if (functionList.Count == 0)
            {
                functionList.Add(new Function()
                {
                    Code = FunctionCode.DeviceCurrentStatus,
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
                code = FunctionCode.DeviceFindHoldShelf;
                this.tbStatus.Text = "找到";
            }
            else if (rbCanPicking.Checked)
            {
                code = FunctionCode.DeviceGetPickStation;
                this.tbStatus.Text = "到拣货台";
            }
            else if (rbFreeShelf.Checked)
            {
                code = FunctionCode.DeviceReturnFreeShelf;
                this.tbStatus.Text = "送回仓储区";
            }
            

            if (rbTrouble.Checked)
            {
                this.CreateFunctionTrouble(functionList, locList);
                this.tbStatus.Text = "故障";
            }
            else
            {
                functionList.Add(new Function()
                {
                    Code = code,
                    TargetInfo = this.deviceId,
                    PathPoint = locList
                });
            }
        }

        /// <summary>
        /// 获取坐标值
        /// </summary>
        /// <returns></returns>
        private Location GetCurrentLocation()
        {
            string strX = tbXValue.Text, strY = tbYValue.Text, strZ = tbZValue.Text;
            if (strX.Equals(string.Empty) || strY.Equals(string.Empty) || strZ.Equals(string.Empty))
            {
                MessageBox.Show("存在坐标值为空");
            }
            return new Location(int.Parse(strX), int.Parse(strY), int.Parse(strZ));
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

            Function function = proto.FunList[0];
            StringBuilder pathInfo = new StringBuilder();
            pathInfo.Append("（");
            pathInfo.Append(function.TargetInfo);
            pathInfo.Append("）");
            if (function.PathPoint != null && function.PathPoint.Count > 0)
            {
                foreach (Location loc in function.PathPoint)
                {
                    pathList.Add(loc);
                    pathInfo.Append(loc.ToString());
                    pathInfo.Append(" > ");
                }
            }
            this.rtbRemark.Text += string.Format(MARK_STRING_FORMAT, RECEIVE_LABEL, proto.FunList[0].Code, pathInfo.ToString());

            switch (function.Code)
            {
                case FunctionCode.SystemSendDevice4Shelf:
                    this.ReportStatus(FunctionCode.DeviceRecevieOrder4Shelf);
                    this.tbStatus.Text = "去找货架";
                    this.tbStatus.Tag = 3;
                    break;
                default: break;
            }
        }

        /// <summary>
        /// 组装协议包并发送
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private void ReportStatus(FunctionCode function)
        {
            Controller.Devices device = new Controller.Devices();
            Location loc = GetCurrentLocation();

            Protocol proto = new Protocol()
            {
                NeedAnswer = ckbBackFlag.Checked,
                FunList = new List<Function>() {  new Function(){ 
                    Code =  function,
                    TargetInfo = this.deviceId,
                    PathPoint =  new List<Location> (){ loc, new Location(){XPos = Convert.ToInt32(this.tbStatus.Tag)}}
                }}
            };

            ErrorCode code = device.ReportStatus(proto);
            if (code != ErrorCode.OK)
                MessageBox.Show(Models.ErrorDescription.ExplainCode(code));

            rtbRemark.Text += string.Format(MARK_STRING_FORMAT, SEND_LABEL, proto.FunList[0].Code, proto.FunList[0].TargetInfo);
        }

    }
}
