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
        private int deviceID;
        private int shelfID;

        /// <summary>
        /// 设备组件
        /// </summary>
        /// <param name="position">所在仓库位置坐标</param>
        /// <param name="deviceData">设备编号</param>
        /// <param name="shelfID">运输货架编号，空车时为0</param>
        public Devices(Location position, int deviceID, int shelfID)
        {
            this.deviceID = deviceID;
            this.shelfID = shelfID;

            InitializeComponent();

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
                return this.deviceID;
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
        }
    }
}
