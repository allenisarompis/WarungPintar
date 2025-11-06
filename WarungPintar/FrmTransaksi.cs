using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarungPintar
{
    public partial class FrmTransaksi : Form
    {
        public FrmTransaksi()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            FrmMenu f = new FrmMenu();
            this.Hide();           // sembunyikan form login
            f.ShowDialog();
        }
    }
}
