using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models;

namespace Views
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            DbEntity.DStaff.Insert(new Staff()
            {
                Name = "Suoxd",
                Sex = true,
                Age = 27,
                Phone = "150150150150",
                Address = "深圳南山",
                Job = "Software",
                Auth = "1110"
            });
            DbEntity.DStaff.Delete(new Staff()
            {
                ID = 4
            });
            DbEntity.DStaff.Update(new Staff()
            {
                ID = 5,
                Phone = "28"
            });
        }
    }
}
