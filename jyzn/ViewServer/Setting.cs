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
            switch (rbItem.Name)
            {
                case "rbMap":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorStoreBack);
                    tbXPick.Text = Controller.StoreMap.ExchangeMapRatio(Models.Graph.SizeGraph.XPos).ToString();
                    tbYPick.Text = Controller.StoreMap.ExchangeMapRatio(Models.Graph.SizeGraph.YPos).ToString();
                    break;
                case "rbPick":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorPickStation);
                    tbXPick.Text = Controller.StoreMap.ExchangeMapRatio(Models.Graph.SizePickStation.XPos).ToString();
                    tbYPick.Text = Controller.StoreMap.ExchangeMapRatio(Models.Graph.SizePickStation.YPos).ToString();
                    break;
                case "rbRestore":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorRestore);
                    tbXPick.Text = Controller.StoreMap.ExchangeMapRatio(Models.Graph.SizeRestore.XPos).ToString();
                    tbYPick.Text = Controller.StoreMap.ExchangeMapRatio(Models.Graph.SizeRestore.YPos).ToString();
                    break;
                case "rbCharge":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorCharger);
                    tbXPick.Text = Controller.StoreMap.ExchangeMapRatio(Models.Graph.SizeCharger.XPos).ToString();
                    tbYPick.Text = Controller.StoreMap.ExchangeMapRatio(Models.Graph.SizeCharger.YPos).ToString();
                    break;
                case "rbDevice":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorDevice);
                    tbXPick.Text = Controller.StoreMap.ExchangeMapRatio(Models.Graph.SizeDevice.XPos).ToString();
                    tbYPick.Text = Controller.StoreMap.ExchangeMapRatio(Models.Graph.SizeDevice.YPos).ToString();
                    break;
                case "rbShelf":
                    lbPick.BackColor = Color.FromArgb(Models.Graph.ColorShelf);
                    tbXPick.Text = Controller.StoreMap.ExchangeMapRatio(Models.Graph.SizeShelf.XPos).ToString();
                    tbYPick.Text = Controller.StoreMap.ExchangeMapRatio(Models.Graph.SizeShelf.YPos).ToString();
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
