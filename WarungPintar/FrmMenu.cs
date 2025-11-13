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
            // Reset Session
            Session.IDPengguna = null;
            Session.Username = null;
            Session.Role = null;

            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            FrmAboutUs f = new FrmAboutUs();
            this.Hide(); 
            f.ShowDialog();
        }

        private void btnMB_Click(object sender, EventArgs e)
        {
            FrmMain f = new FrmMain();
            this.Hide(); 
            f.ShowDialog();
        }

        public FrmMenu(string role) : this()
        {
            userRole = role;
        }
        private void FrmMenu_Load(object sender, EventArgs e)
        {
            if (Session.Role == "kasir")
            {
                // Kasir tidak bisa akses laporan keuangan
                btnLK.Enabled = false;
                btnLK.Visible = true;
            }
            else if (Session.Role == "admin")
            {
                // Admin tidak bisa akses transaksi
                btnTR.Enabled = false;
                btnTR.Visible = true;
            }
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            FrmDataUser frm = new FrmDataUser();
            frm.Show();
        }

        private void btnLK_Click(object sender, EventArgs e)
        {
            // kasir tak bisa akses laporan keuangan
            if (Session.Role == "kasir")
            {
                MessageBox.Show("Kasir tidak bisa akses ke menu Laporan Keuangan!",
                    "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FrmLaporan f = new FrmLaporan();
            this.Hide();
            f.ShowDialog();
        }

        private void btnTR_Click(object sender, EventArgs e)
        {
            // admin tak bisa akses transaksi
            if (Session.Role == "admin")
            {
                MessageBox.Show("Admin tidak bisa akses menu transaksi",
                    "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FrmTransaksi f = new FrmTransaksi();
            this.Hide(); 
            f.ShowDialog();
        }
    }
}
