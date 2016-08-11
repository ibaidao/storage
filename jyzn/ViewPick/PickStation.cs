﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewPick
{
    public partial class PickStation : Form
    {
        private Color SHELF_STRUCT_COLOR = Color.SeaGreen, PRODUCT_LOCTION_COLOR = Color.DarkBlue;

        public PickStation()
        {
            InitializeComponent();
        }

        private void btnPick_Click(object sender, EventArgs e)
        {
            int stationId = Convert.ToInt32(lbStation.Text);
            string productCode = this.tbProduct.Text;
            Controller.Picking picker = new Controller.Picking();
            Models.ErrorCode code = picker.FindScanProduct(stationId, productCode);
            if (code != Models.ErrorCode.OK)
            {
                MessageBox.Show(Models.ErrorDescription.ExplainCode(code));
            }
            this.tbProduct.Text = string.Empty;
        }

        /// <summary>
        /// 更新界面显示
        /// </summary>
        /// <param name="strInfo">商品所在库位;货架库位信息;商品名称</param>
        public void UpdateProductInfo(string strInfo)
        {
            if (this.InvokeRequired)
            {
                Action<string> updateShow = new Action<string>(UpdateProductInfo);
                this.Invoke(updateShow);
                return;
            }

            string[] strArray = strInfo.Split(';');
            int productLoc = Convert.ToInt32(strArray[0]);
            string shelfLoc = strArray[1];
            int layer = shelfLoc.Length / 2;
            int[] locs = new int[layer];
            for (int i = 0; i < layer; i++)
                locs[i] = Convert.ToInt32(shelfLoc.Substring(i * 2, 2));
            this.DrawShelf(locs, productLoc);

            string productName = string.Join(";", strArray, 2, strArray.Length - 2);
            this.lbName.Text = productName;
        }

        /// <summary>
        /// 绘制货架库位
        /// </summary>
        /// <param name="shelfLoc"></param>
        private void DrawShelf(int[] locInfo, int productLoc)
        {
            int lenWidth = 10, panelMargin = 20;//像素
            int[] cellWidth = new int[locInfo.Length];//每层的单格像素
            int width = this.pnShelf.Width - panelMargin * 2, height = this.pnShelf.Height - panelMargin * 2;
            Graphics graph = this.pnShelf.CreateGraphics();
            graph.Clear(this.BackColor);
            SolidBrush brush = new SolidBrush(SHELF_STRUCT_COLOR);
            //画外框
            graph.FillRectangle(brush, panelMargin, panelMargin, width, lenWidth);//上
            graph.FillRectangle(brush, panelMargin + width - lenWidth, panelMargin, lenWidth, height - lenWidth);//右
            graph.FillRectangle(brush, panelMargin, panelMargin + height - lenWidth, width, lenWidth);//下
            graph.FillRectangle(brush, panelMargin, panelMargin + lenWidth, lenWidth, height - lenWidth * 2);//左
            //画内格
            int layerHeigh = (height - (locInfo.Length + 1) * lenWidth) / locInfo.Length;//上下边框 +1 = -1 + 2
            int k = 0;
            foreach (int loc in locInfo)
            {//层
                k++;
                graph.FillRectangle(brush, panelMargin + lenWidth, panelMargin + (lenWidth + layerHeigh) * k, width - 2 * lenWidth, lenWidth);
                cellWidth[k - 1] = (width - (loc + 1) * lenWidth) / loc;
                for (int i = 1; i < loc; i++)
                {//格
                    graph.FillRectangle(brush, panelMargin + (lenWidth + cellWidth[k - 1]) * i, panelMargin + (lenWidth + layerHeigh) * k - layerHeigh, lenWidth, layerHeigh);
                }
            }
            //标记商品所在位置
            SolidBrush brushProduct = new SolidBrush(PRODUCT_LOCTION_COLOR);
            int tmpLoc = productLoc;
            for (k = locInfo.Length - 1; k >= 0 && tmpLoc > 0; k--)
                tmpLoc -= locInfo[k];
            tmpLoc += locInfo[++k];//k为层序号（0开始：从下往上），tmpLoc为格序号（1开始：从左往右）
            graph.FillRectangle(brushProduct, panelMargin + (lenWidth + cellWidth[k]) * tmpLoc - cellWidth[k], panelMargin + (lenWidth + layerHeigh) * (locInfo.Length - k) + lenWidth, cellWidth[locInfo.Length - k], layerHeigh);
        }
    }
}