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
        Models.Graph graph;

        public Setting(Models.Graph mapInfo)
        {
            this.graph = mapInfo;

            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            this.tbRatio.Text = graph.RatioMapZoom.ToString();

        }

        private void rbModleStyle_Click(object sender, EventArgs e)
        {
            RadioButton rbItem = sender as RadioButton;
            switch (rbItem.Name)
            {
                case "rbMap":
                    lbPick.BackColor = Color.FromArgb(graph.ColorStoreBack);
                    tbXPick.Text = graph.MapReverse( graph.SizeGraph.XPos).ToString();
                    tbYPick.Text = graph.MapReverse(graph.SizeGraph.YPos).ToString();
                    break;
                case "rbPick":
                    lbPick.BackColor = Color.FromArgb(graph.ColorPickStation);
                    tbXPick.Text = graph.MapReverse(graph.SizePickStation.XPos).ToString();
                    tbYPick.Text = graph.MapReverse(graph.SizePickStation.YPos).ToString();
                    break;
                case "rbRestore":
                    lbPick.BackColor = Color.FromArgb(graph.ColorRestore);
                    tbXPick.Text = graph.MapReverse(graph.SizeRestore.XPos).ToString();
                    tbYPick.Text = graph.MapReverse(graph.SizeRestore.YPos).ToString();
                    break;
                case "rbCharge":
                    lbPick.BackColor = Color.FromArgb(graph.ColorCharger);
                    tbXPick.Text = graph.MapReverse(graph.SizeCharger.XPos).ToString();
                    tbYPick.Text = graph.MapReverse(graph.SizeCharger.YPos).ToString();
                    break;
                case "rbDevice":
                    lbPick.BackColor = Color.FromArgb(graph.ColorDevice);
                    tbXPick.Text = graph.MapReverse(graph.SizeDevice.XPos).ToString();
                    tbYPick.Text = graph.MapReverse(graph.SizeDevice.YPos).ToString();
                    break;
                case "rbShelf":
                    lbPick.BackColor = Color.FromArgb(graph.ColorShelf);
                    tbXPick.Text = graph.MapReverse(graph.SizeShelf.XPos).ToString();
                    tbYPick.Text = graph.MapReverse(graph.SizeShelf.YPos).ToString();
                    break;
                default:
                    break;
            }
        }
    }
}
