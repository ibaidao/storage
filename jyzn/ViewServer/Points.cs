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

            this.BackColor = Color.FromArgb(colorArgb);
        }
    }
}
