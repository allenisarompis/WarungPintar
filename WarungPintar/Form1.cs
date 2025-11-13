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
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("⚠️Harap isi username dan password!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                query = $"SELECT * FROM tbl_pengguna WHERE username = '{txtUsername.Text.Trim()}'";
                ds.Clear();
                koneksi.Open();
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);
                adapter.Fill(ds);
                koneksi.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow kolom = ds.Tables[0].Rows[0];
                    string passwordDB = kolom["password"].ToString();

                    if (passwordDB == txtPassword.Text.Trim())
                    {
                        // Simpan di Session
                        Session.IDPengguna = kolom["id_pengguna"].ToString();
                        Session.Username = kolom["username"].ToString();

                        if (Session.Username.ToLower() == "admin")
                            Session.Role = "admin";
                        else if (Session.Username.ToLower() == "kasir")
                            Session.Role = "kasir";
                        else
                            Session.Role = "user";

                        // buka menu
                        FrmMenu menu = new FrmMenu();
                        menu.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Username tidak ditemukan!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                koneksi.Close();
            }
        }

    }

}
