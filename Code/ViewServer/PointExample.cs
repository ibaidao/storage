﻿using System;
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
    public partial class PointExample : UserControl
    {
        private const int EXAMPLE_INTERVAL = 10;//示例之间间隔像素

        public PointExample(int examIdx,int colorIdx, Models.Location size, string strInfo)
        {
            InitializeComponent();
            this.pnlShape.Size = new Size(size.XPos, size.YPos);
            this.pnlShape.BackColor = Color.FromArgb(colorIdx);

            this.lbWord.Text = strInfo;
            
            int yValue = (examIdx -1) * (this.Height + EXAMPLE_INTERVAL);
            this.Location = new Point(0, yValue);
        }
    }
}
