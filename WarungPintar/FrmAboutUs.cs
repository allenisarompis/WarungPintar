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
    public partial class FrmAboutUs : Form
    {
        public FrmAboutUs()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FrmMain f = new FrmMain();
            this.Hide();           // sembunyikan form login
            f.ShowDialog();
        }

        private void txtAbout_TextChanged(object sender, EventArgs e)
        {
            txtAbout.ReadOnly = true;
        }
    }
}
