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
        private double ratioMap = 0.0;
        private Action<Models.StoreComponentType, Models.GraphConfig> UpdateMainBoard;

        public Setting(Action<Models.StoreComponentType, Models.GraphConfig> updateAfterSetting)
        {
            this.UpdateMainBoard = updateAfterSetting;

            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            this.ratioMap = Models.Graph.RatioMapZoom;
            this.tbRatio.Text = this.ratioMap.ToString();
        }

        private void rbModleStyle_Click(object sender, EventArgs e)
        {
            RadioButton rbItem = sender as RadioButton;
            double ratio = Models.Graph.RatioMapZoom;
            switch (rbItem.Name)
            {
                case "rbMap":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorStoreBack);
                    tbXPick.Text = Models.Location.MapReverse(Models.Graph.SizeGraph.XPos, ratio).ToString();
                    tbYPick.Text = Models.Location.MapReverse(Models.Graph.SizeGraph.YPos, ratio).ToString();
                    break;
                case "rbPick":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorPickStation);
                    tbXPick.Text = Models.Location.MapReverse(Models.Graph.SizePickStation.XPos, ratio).ToString();
                    tbYPick.Text = Models.Location.MapReverse(Models.Graph.SizePickStation.YPos, ratio).ToString();
                    break;
                case "rbRestore":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorRestore);
                    tbXPick.Text = Models.Location.MapReverse(Models.Graph.SizeRestore.XPos, ratio).ToString();
                    tbYPick.Text = Models.Location.MapReverse(Models.Graph.SizeRestore.YPos, ratio).ToString();
                    break;
                case "rbCharge":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorCharger);
                    tbXPick.Text = Models.Location.MapReverse(Models.Graph.SizeCharger.XPos, ratio).ToString();
                    tbYPick.Text = Models.Location.MapReverse(Models.Graph.SizeCharger.YPos, ratio).ToString();
                    break;
                case "rbDevice":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorDevice);
                    tbXPick.Text = Models.Location.MapReverse(Models.Graph.SizeDevice.XPos, ratio).ToString();
                    tbYPick.Text = Models.Location.MapReverse(Models.Graph.SizeDevice.YPos, ratio).ToString();
                    break;
                case "rbShelf":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorShelf);
                    tbXPick.Text = Models.Location.MapReverse(Models.Graph.SizeShelf.XPos, ratio).ToString();
                    tbYPick.Text = Models.Location.MapReverse(Models.Graph.SizeShelf.YPos, ratio).ToString();
                    break;
                default:
                    break;
            }
        }

        private void tbRatio_Leave(object sender, EventArgs e)
        {
            double currentValue = double.Parse((sender as TextBox).Text);
            if (currentValue - this.ratioMap > 1e-6 || this.ratioMap - currentValue > 1e-6)
            {
                if (this.UpdateMainBoard != null)
                {
                    Models.GraphConfig config = new Models.GraphConfig();
                    config.ColorIndex = (int)(currentValue * 1000);
                    this.UpdateMainBoard(Models.StoreComponentType.StoreRatio, config);
                    
                    this.ratioMap = currentValue;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (rbMap.Checked)
             {
                 Models.GraphConfig config = new Models.GraphConfig();
                 config.ColorIndex = lbPick.BackColor.ToArgb();
                 config.Width = int.Parse( tbXPick.Text);
                 config.Length = int.Parse(tbYPick.Text);

                 this.UpdateMainBoard(Models.StoreComponentType.StoreSelf, config);
             }
        }
    }
}
