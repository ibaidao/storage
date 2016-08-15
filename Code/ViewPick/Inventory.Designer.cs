namespace ViewPick
{
    partial class Inventory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.rbShelf = new System.Windows.Forms.RadioButton();
            this.rbProducts = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbID = new System.Windows.Forms.TextBox();
            this.rbTarget = new System.Windows.Forms.RadioButton();
            this.rbRandom = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 115);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(762, 492);
            this.dataGridView1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(660, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 69);
            this.button1.TabIndex = 1;
            this.button1.Text = "新建盘点";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // rbShelf
            // 
            this.rbShelf.AutoSize = true;
            this.rbShelf.Location = new System.Drawing.Point(29, 38);
            this.rbShelf.Name = "rbShelf";
            this.rbShelf.Size = new System.Drawing.Size(73, 19);
            this.rbShelf.TabIndex = 2;
            this.rbShelf.TabStop = true;
            this.rbShelf.Text = "按货架";
            this.rbShelf.UseVisualStyleBackColor = true;
            // 
            // rbProducts
            // 
            this.rbProducts.AutoSize = true;
            this.rbProducts.Location = new System.Drawing.Point(121, 38);
            this.rbProducts.Name = "rbProducts";
            this.rbProducts.Size = new System.Drawing.Size(73, 19);
            this.rbProducts.TabIndex = 3;
            this.rbProducts.TabStop = true;
            this.rbProducts.Text = "按商品";
            this.rbProducts.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbProducts);
            this.groupBox1.Controls.Add(this.rbShelf);
            this.groupBox1.Location = new System.Drawing.Point(44, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 79);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "盘点方式";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbID);
            this.groupBox2.Controls.Add(this.rbTarget);
            this.groupBox2.Controls.Add(this.rbRandom);
            this.groupBox2.Location = new System.Drawing.Point(301, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(312, 79);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择方式";
            // 
            // tbID
            // 
            this.tbID.Location = new System.Drawing.Point(183, 37);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(99, 25);
            this.tbID.TabIndex = 4;
            // 
            // rbTarget
            // 
            this.rbTarget.AutoSize = true;
            this.rbTarget.Location = new System.Drawing.Point(103, 38);
            this.rbTarget.Name = "rbTarget";
            this.rbTarget.Size = new System.Drawing.Size(74, 19);
            this.rbTarget.TabIndex = 3;
            this.rbTarget.TabStop = true;
            this.rbTarget.Text = "指定ID";
            this.rbTarget.UseVisualStyleBackColor = true;
            // 
            // rbRandom
            // 
            this.rbRandom.AutoSize = true;
            this.rbRandom.Location = new System.Drawing.Point(29, 38);
            this.rbRandom.Name = "rbRandom";
            this.rbRandom.Size = new System.Drawing.Size(58, 19);
            this.rbRandom.TabIndex = 2;
            this.rbRandom.TabStop = true;
            this.rbRandom.Text = "随机";
            this.rbRandom.UseVisualStyleBackColor = true;
            // 
            // Inventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 619);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Inventory";
            this.Text = "盘点";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton rbShelf;
        private System.Windows.Forms.RadioButton rbProducts;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbTarget;
        private System.Windows.Forms.RadioButton rbRandom;
        private System.Windows.Forms.TextBox tbID;
    }
}