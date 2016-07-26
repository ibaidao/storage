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
    public partial class Points : UserControl
    {
        private Color pointColor;
        public Points(Core.Location location,int pointSize, int colorArgb)
        {
            InitializeComponent();

            Point locUser = new Point(); 
            locUser.X = location.XPos;
            locUser.Y = location.YPos;
            this.Location=locUser;

            Size sizeUser = new Size() ;
            sizeUser.Width = pointSize;
            sizeUser.Height = pointSize;
            this.Size = sizeUser;

            this.pointColor = Color.FromArgb(colorArgb);
            this.BackColor = this.pointColor;
        }

        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "closePoint":
                    this.BackColor = Color.Red;
                    contextMenu.Items["startPoint"].Visible = true;
                    contextMenu.Items["closePoint"].Visible = false;
                    break;
                case "startPoint":
                    this.BackColor = this.pointColor;
                    contextMenu.Items["startPoint"].Visible = false;
                    contextMenu.Items["closePoint"].Visible = true;
                    break;
                case "addCharge": break;
                case "addPickStation": break;
                case "addRestore": break;
                default: break;
            }
        }
    }
}
