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
        private int lengthOneStep, stationId;
        private const string CONFIG_SELF = "DeviceSelf";
        private readonly int deviceId;
        private bool reportStationFlag = true, GetInPickerFlag = false;

        public Main()
        {
            InitializeComponent();

            this.gbTrouble.Enabled = false;
            this.deviceId = int.Parse(Utilities.IniFile.ReadIniData(CONFIG_SELF, "CarID"));
            this.lengthOneStep = int.Parse(Utilities.IniFile.ReadIniData(CONFIG_SELF, "MoveSpeed"));
            string strLoc = Utilities.IniFile.ReadIniData(CONFIG_SELF, "InitalLocation");
            string[] strLocXYZ = strLoc.Split(',');
            tbXValue.Text = strLocXYZ[0];
            tbYValue.Text = strLocXYZ[1];
            tbZValue.Text = strLocXYZ[2];

            Controller.Devices.StartListenCommunicate(ShowReceivingMessage);

            this.timerPackage.Enabled = true;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.timerPackage.Enabled = false;
            Environment.Exit(0);
        }
        
        #region 界面交互事件
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (rbFreeShelf.Checked) this.tbStatus.Tag = (short)StoreComponentStatus.OK;
            else if (rbCharger.Checked) this.tbStatus.Tag = (short)StoreComponentStatus.Block;

            Protocol proto = new Protocol()
            {
                NeedAnswer = ckbBackFlag.Checked
            };
            List<Function> functionList = new List<Function>();
            List<Location> locList = new List<Location>() { this.GetCurrentLocation() };
            this.CreateFunction(functionList, locList);
            proto.FunList = functionList;

            this.SendMessage2Server(proto);
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
            else
            {
                btnSend_Click(null, null);
            }
        }

        private void btnTimer_Click(object sender, EventArgs e)
        {
            if (btnTimer.Text == "暂停")
            {
                this.timerPackage.Enabled = false;
                btnTimer.Text = "开启";
            }
            else
            {
                this.timerPackage.Enabled = true;
                btnTimer.Text = "暂停";
            }

        }

        private void rbItem_Click(object sender, EventArgs e)
        {
            this.gbTrouble.Enabled = sender as RadioButton == rbTrouble;
        }
        #endregion


        private void timerPackage_Tick(object sender, EventArgs e)
        {
            if (pathList.Count > 1)
            {
                if (rbCanPicking.Checked && !this.GetInPickerFlag && pathList.Count==2)
                {//到拣货台的最后一段路
                    this.SendMessage2Server(new Protocol()
                    {
                        NeedAnswer = ckbBackFlag.Checked,
                        FunList = new List<Function>() {  
                            new Function(){   
                                Code =  FunctionCode.DeviceAskMoveForward,    
                                TargetInfo = this.deviceId,                 
                                PathPoint =  new List<Location> (){ new Location(){XPos = this.stationId}}       
                            }
                        }
                    });
                    return;
                }

                reportStationFlag = false;
                int xCurValue = int.Parse(tbXValue.Text), yCurValue = int.Parse(tbYValue.Text), zCurValue = int.Parse(tbZValue.Text);
                int xLastValue = pathList[0].XPos, yLastValue = pathList[0].YPos, zLastValue = pathList[0].ZPos;
                int xTarValue = pathList[1].XPos, yTarValue = pathList[1].YPos, zTarValue = pathList[1].ZPos;

                if (xTarValue == xLastValue && yTarValue == yLastValue)
                {//移动Z轴
                    if ((zLastValue - zTarValue) * (zCurValue - zTarValue) > 0)
                    {//还在去目标的路上（相对终点，跟起点在同一个方向）
                        zCurValue += lengthOneStep * (zTarValue > zLastValue ? 1 : -1);
                    }
                    if ((zLastValue - zTarValue) * (zCurValue - zTarValue) <= 0)
                    {//到了终点，或者已经超过了一些（上限为单步长度）[再判断一次，防止新坐标超出指引路径]
                        zCurValue = zTarValue;
                        pathList.RemoveAt(0);//一次仅一个方向变动
                    }
                    tbZValue.Text = zCurValue.ToString();
                }
                else if (xTarValue == xLastValue && zTarValue == zLastValue)
                {//移动Y轴
                    if ((yLastValue - yTarValue) * (yCurValue - yTarValue) > 0)
                    {
                        yCurValue += lengthOneStep * (yTarValue > yLastValue ? 1 : -1);
                    }
                    if ((yLastValue - yTarValue) * (yCurValue - yTarValue) <= 0)
                    {//再判断一次，防止新坐标超出指引路径
                        yCurValue = yTarValue;
                        pathList.RemoveAt(0);
                    }
                    tbYValue.Text = yCurValue.ToString();
                }
                else if (zTarValue == zLastValue && yTarValue == yLastValue)
                {//移动X轴
                    if ((xLastValue - xTarValue) * (xCurValue - xTarValue) > 0)
                    {
                        xCurValue += lengthOneStep * (xTarValue > xLastValue ? 1 : -1);
                    }
                    if ((xLastValue - xTarValue) * (xCurValue - xTarValue) <= 0)
                    {
                        xCurValue = xTarValue;
                        pathList.RemoveAt(0);
                    }
                    tbXValue.Text = xCurValue.ToString();
                }
                this.ReportStatus(FunctionCode.DeviceCurrentStatus);
            }
            else
            {
                if (!reportStationFlag)
                {
                    btnSend_Click(null, null);
                    reportStationFlag = true;
                }
            }
        }

        #region 内部调用

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
                this.GetInPickerFlag = false;
            }
            else if (rbFreeShelf.Checked)
            {
                code = FunctionCode.DeviceReturnFreeShelf;
                this.tbStatus.Text = "送回仓储区";
            }
            else if (rbCharger.Checked)
            {
                code = FunctionCode.DeviceStartCharging;
                this.tbStatus.Text = "去充电桩";
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
                if (pathList.Count > 0) pathList.Clear();
                foreach (Location loc in function.PathPoint)
                {
                    pathList.Add(loc);
                    pathInfo.Append(loc.ToString());
                    pathInfo.Append(" > ");
                }
            }
            this.rtbRemark.Text += string.Format(MARK_STRING_FORMAT, RECEIVE_LABEL, proto.FunList[0].Code, pathInfo.ToString());
            this.rtbRemark.SelectionStart = this.rtbRemark.Text.Length;
            this.rtbRemark.ScrollToCaret();

            switch (function.Code)
            {
                case FunctionCode.SystemSendDevice4Shelf:
                    this.ReportStatus(FunctionCode.DeviceRecevieOrder4Shelf);
                    this.tbStatus.Text = "去找货架";
                    this.tbStatus.Tag = (short)StoreComponentStatus.Working;
                    this.rbHoldShelf.Checked = true;
                    break;
                case FunctionCode.SystemMoveShelf2Station:
                    this.rbCanPicking.Checked = true;
                    this.stationId = function.TargetInfo;
                    break;
                case FunctionCode.SystemMoveShelfBack:
                    this.rbFreeShelf.Checked = true;
                    break;
                case FunctionCode.SystemChargeDevice:
                    this.rbCharger.Checked = true;
                    break;
                case FunctionCode.SystemDeviceMoveForward:
                    this.GetInPickerFlag = function.TargetInfo == (short)StoreComponentStatus.OK;
                    break;
                default: break;
            }
        }

        /// <summary>
        /// 心跳包
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private void ReportStatus(FunctionCode function)
        {
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

            this.SendMessage2Server(proto);
        }

        /// <summary>
        /// 给服务器发包
        /// </summary>
        /// <param name="proto"></param>
        private void SendMessage2Server(Protocol proto)
        {
            Controller.Devices device = new Controller.Devices();
            ErrorCode code = device.ReportStatus(proto);
            if (code != ErrorCode.OK)
            {
                Core.Logger.WriteNotice("通讯失败");
            }

            this.rtbRemark.Text += string.Format(MARK_STRING_FORMAT, SEND_LABEL, proto.FunList[0].Code, proto.FunList[0].TargetInfo);
            this.rtbRemark.SelectionStart = this.rtbRemark.Text.Length;
            this.rtbRemark.ScrollToCaret();
        }

        #endregion
    }
}
