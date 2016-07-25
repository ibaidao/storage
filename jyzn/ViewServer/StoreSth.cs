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

        /// <summary>
        /// 仓库内存放的物品
        /// </summary>
        /// <param name="location">左上角坐标</param>
        /// <param name="size">尺寸</param>
        /// <param name="background">背景色</param>
        public StoreSth(Point location, Size size, Color background)
        {
            InitializeComponent();

            this.Size = size;
            this.Location = location;
            this.BackColor = background;
        }

        /// <summary>
        /// 物品显示背景色
        /// </summary>
        public Color Background
        {
            set
            {
                this.BackColor = value;
                this.Refresh();
            }
        }
        
    }
}
