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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string uname = txtUsername.Text;
            string pass = txtPassword.Text;

            string role = ""; // variable untuk menyimpan role user

            if (uname == "admin" && pass == "admin")
            {
                role = "admin";
            }
            else if (uname == "kasir" && pass == "kasir")
            {
                role = "kasir";
            }
            else
            {
                MessageBox.Show("Username atau Password salah!");
                return;
            }

            FrmMenu menu = new FrmMenu(role);
            menu.Show();
            this.Hide(); // sembunyikan login form
        }

    }
}
