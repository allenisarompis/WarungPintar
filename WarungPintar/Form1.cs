using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace WarungPintar
{
    public partial class Form1 : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public Form1()
        {
            alamat = "server=localhost; database=db_warung; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string uname = txtUsername.Text.Trim();
            string pass = txtPassword.Text.Trim();

            string id = "";
            string role = "";

            // Login manual admin dan kasir
            if (uname == "admin" && pass == "admin")
            {
                id = "1";
                role = "admin";
            }
            else if (uname == "kasir" && pass == "kasir")
            {
                id = "2";
                role = "kasir";
            }
            else
            {
                MessageBox.Show("Username atau Password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Simpan data login ke Session global
            Session.IDPengguna = id;
            Session.Username = uname;
            Session.Role = role;

            //MessageBox.Show($"Selamat datang, {uname}!", "Login Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Buka form menu 
            FrmMenu menu = new FrmMenu(role);
            menu.Show();
            this.Hide();
        }

    }

}
