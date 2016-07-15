using System;
using System.Net;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models;
using Core;

namespace Views
{
    public partial class Form1 : Form
    {
        Communicate comm = new Communicate();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            comm.StartListening();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            byte [] data = new byte[]{0x22,0x33,0x44};

            comm.SendBuffer(ipAddress, data);
        }
    }
}
