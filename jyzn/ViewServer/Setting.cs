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
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            this.tbRatio.Text = Models.Graph.RatioMapZoom.ToString();
        }

        private void rbModleStyle_Click(object sender, EventArgs e)
        {
            RadioButton rbItem = sender as RadioButton;
            switch (rbItem.Name)
            {
                case "rbMap":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorStoreBack);
                    tbXPick.Text = Models.Graph.MapReverse(Models.Graph.SizeGraph.XPos).ToString();
                    tbYPick.Text = Models.Graph.MapReverse(Models.Graph.SizeGraph.YPos).ToString();
                    break;
                case "rbPick":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorPickStation);
                    tbXPick.Text = Models.Graph.MapReverse(Models.Graph.SizePickStation.XPos).ToString();
                    tbYPick.Text = Models.Graph.MapReverse(Models.Graph.SizePickStation.YPos).ToString();
                    break;
                case "rbRestore":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorRestore);
                    tbXPick.Text = Models.Graph.MapReverse(Models.Graph.SizeRestore.XPos).ToString();
                    tbYPick.Text = Models.Graph.MapReverse(Models.Graph.SizeRestore.YPos).ToString();
                    break;
                case "rbCharge":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorCharger);
                    tbXPick.Text = Models.Graph.MapReverse(Models.Graph.SizeCharger.XPos).ToString();
                    tbYPick.Text = Models.Graph.MapReverse(Models.Graph.SizeCharger.YPos).ToString();
                    break;
                case "rbDevice":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorDevice);
                    tbXPick.Text = Models.Graph.MapReverse(Models.Graph.SizeDevice.XPos).ToString();
                    tbYPick.Text = Models.Graph.MapReverse(Models.Graph.SizeDevice.YPos).ToString();
                    break;
                case "rbShelf":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorShelf);
                    tbXPick.Text = Models.Graph.MapReverse(Models.Graph.SizeShelf.XPos).ToString();
                    tbYPick.Text = Models.Graph.MapReverse(Models.Graph.SizeShelf.YPos).ToString();
                    break;
                default:
                    break;
            }
        }
    }
}
