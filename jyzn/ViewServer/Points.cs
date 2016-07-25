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
        private const int POINT_SIZE = 5;
        public Points(Core.Location location, int colorArgb)
        {
            InitializeComponent();

            Point locUser = new Point(); 
            locUser.X = location.XPos;
            locUser.Y = location.YPos;
            this.Location=locUser;

            Size sizeUser = new Size() ;
            sizeUser.Width = POINT_SIZE;
            sizeUser.Height = POINT_SIZE;
            this.Size = sizeUser;

            this.BackColor = Color.FromArgb(colorArgb);
        }
    }
}
