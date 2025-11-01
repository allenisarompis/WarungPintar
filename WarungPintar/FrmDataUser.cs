using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace WarungPintar
{
    public partial class FrmDataUser : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public FrmDataUser()
        {
            alamat = "server=localhost; database=db_warung; username=root; password=;";
            koneksi = new MySqlConnection(alamat);

            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            FrmMenu f = new FrmMenu();
            this.Hide();           // sembunyikan form login
            f.ShowDialog();
        }

        private void FrmDataUser_Load(object sender, EventArgs e)
        {

        }

        private void LblFoto_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = Image.FromFile(openFileDialog1.FileName);
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            LblFoto.Visible = false;
        }

    }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmMenu f = new FrmMenu();
            this.Hide();           // sembunyikan form login
            f.ShowDialog();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUsername.Text != "" && txtPassword.Text != "" && pictureBox2.Image != null)
                {
                    // Folder relatif di dalam aplikasi
                    string folderPath = Path.Combine(Application.StartupPath, "foto");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Nama file unik
                    string fileName = Guid.NewGuid().ToString() + ".jpg";
                    string filePath = Path.Combine(folderPath, fileName);

                    // Simpan gambar
                    pictureBox2.Image.Save(filePath);

                    // Query insert (pastikan jumlah kolom sesuai tabel)
                    query = "INSERT INTO tbl_pengguna (id, username, password, nama, foto) " +
                            "VALUES (@id, @username, @password, @nama, @foto)";

                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@id", txtID.Text);
                    perintah.Parameters.AddWithValue("@username", txtUsername.Text);
                    perintah.Parameters.AddWithValue("@password", txtPassword.Text);
                    perintah.Parameters.AddWithValue("@foto", fileName);

                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Insert Data Sukses!");
                        FrmDataUser_Load(null, null); // reload data jika ada DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Gagal insert data!");
                    }
                }
                else
                {
                    MessageBox.Show("Data tidak lengkap!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
}
