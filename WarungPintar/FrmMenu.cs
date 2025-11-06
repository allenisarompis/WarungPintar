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
    public partial class FrmMenu : Form
    {
        private string userRole;

        public FrmMenu()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            FrmAboutUs f = new FrmAboutUs();
            this.Hide(); // sembunyikan form utama (opsional)
            f.ShowDialog();
        }

        private void btnMB_Click(object sender, EventArgs e)
        {
            FrmMain f = new FrmMain();
            this.Hide(); // sembunyikan form utama (opsional)
            f.ShowDialog();
        }

        public FrmMenu(string role) : this()
        {
            userRole = role;
        }
        private void FrmMenu_Load(object sender, EventArgs e)
        {
            if (userRole == "kasir")
            {
                // kasir tidak bisa akses Laporan Keuangan
                btnLK.Enabled = false;
            }
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            FrmDataUser frm = new FrmDataUser();
            frm.Show();
        }

        private void btnLK_Click(object sender, EventArgs e)
        {
            
        }

        private void btnTR_Click(object sender, EventArgs e)
        {
            FrmTransaksi f = new FrmTransaksi();
            this.Hide(); // sembunyikan form utama (opsional)
            f.ShowDialog();
        }
    }
}
