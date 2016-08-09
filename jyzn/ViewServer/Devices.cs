using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models;

namespace ViewServer
{
    public partial class Devices : UserControl
    {
        private Models.Devices deviceInfo;
        private int shelfID;

        /// <summary>
        /// 设备组件
        /// </summary>
        /// <param name="device">设备</param>
        /// <param name="shelfID">运输货架编号，空车时为0</param>
        public Devices(Models.Devices device, int shelfID)
        {
            deviceInfo = device;
            this.shelfID = shelfID;

            InitializeComponent();

            Location position = Models.Location.DecodeStringInfo(device.LocationXYZ);
            Location tmpLoc = Controller.StoreMap.ExchangeLocation(position);
            this.Location = new Point(tmpLoc.XPos, tmpLoc.YPos);
            this.Size = new Size(Graph.SizeDevice.XPos, Graph.SizeDevice.YPos);
            this.UpdateDeviceColor();
        }

        /// <summary>
        /// 当前设备编号
        /// </summary>
        public int DeviceID
        {
            get
            {
                return this.deviceInfo.ID;
            }
        }

        /// <summary>
        /// 当前设备运输的货架编号
        /// </summary>
        public int ShelfID
        {
            get { return this.shelfID; }
            set
            {
                this.shelfID = value;
                this.UpdateDeviceColor();
            }
        }

        /// <summary>
        /// 设置设备位置（仓库内坐标）
        /// </summary>
        public Location DeviceLocation
        {
            set
            {
                Location tmpLoc = Controller.StoreMap.ExchangeLocation(value);
                this.Location = new Point(tmpLoc.XPos, tmpLoc.YPos);
            }
        }

        /// <summary>
        /// 更新设备显示颜色
        /// </summary>
        private void UpdateDeviceColor()
        {
            if (this.shelfID > 0)
                this.BackColor = Color.FromArgb(Graph.ColorDeviceShelf);
            else
                this.BackColor = Color.FromArgb(Graph.ColorDevice);

            switch (this.deviceInfo.Status)
            {
                case (short)StoreComponentStatus.OK:
                    this.lbInfo.Text = "闲";
                    break;
                case (short)StoreComponentStatus.Working:
                    this.lbInfo.Text = "忙";
                    break;
                case (short)StoreComponentStatus.Block:
                    this.lbInfo.Text = "充电";
                    break;

                default :break;
            }
        }
    }
}
