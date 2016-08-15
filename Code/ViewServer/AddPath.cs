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
    public partial class AddPath : Form
    {
        private Action refreshMainWindow;

        public AddPath(Action realShowPath)
        {
            this.refreshMainWindow = realShowPath;

            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.refreshMainWindow == null) return;
            this.refreshMainWindow();

            this.Close();
        }

        /// <summary>
        /// 当前选中的路径类型
        /// </summary>
        public Models.StoreComponentType PathType
        {
            get
            {
                return rbBoth.Checked ? Models.StoreComponentType.BothPath : Models.StoreComponentType.OneWayPath;
            }
        }
    }
}
