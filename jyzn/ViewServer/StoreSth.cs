using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewServer
{
    public partial class StoreSth : UserControl
    {
        private Size sthSize;

        /// <summary>
        /// 仓库内存放的物品
        /// </summary>
        /// <param name="location">左上角坐标</param>
        /// <param name="size">尺寸</param>
        /// <param name="backColor">背景色</param>
        public StoreSth(Core.Location location, Core.Location size, int backColor)
        {
            InitializeComponent();

            this.Location = new Point(location.XPos, location.YPos);
            this.Size = new Size (size.XPos, size.YPos);
            this.BackColor = Color.FromArgb(backColor);
        }        
    }
}
