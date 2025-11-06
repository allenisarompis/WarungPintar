using MySql.Data.MySqlClient;
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
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace WarungPintar
{
    public partial class FrmDataUser : Form
    {
        private MySqlConnection koneksi;
        private string connectionString = "server=localhost;database=db_warung;username=root;password=;";

        private string connStr = "server=localhost; database=db_warung; username=root; password=;";
        private string fotoPathAsli = ""; // untuk menyimpan foto dari DB
        private string fotoPathBaru = ""; // untuk foto baru yang dipilih


        public FrmDataUser()
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

        private void FrmDataUser_Load(object sender, EventArgs e)
        {
            txtID.Text = Session.IDPengguna;
            txtUsername.Text = Session.Username;
            txtPassword.Text = "";

            LoadDataPengguna();

        }
        private void LoadDataPengguna()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT foto FROM tbl_pengguna WHERE id_pengguna = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", Session.IDPengguna);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            fotoPathAsli = result.ToString();

                            if (File.Exists(fotoPathAsli))
                            {
                                picFoto.Image = Image.FromFile(fotoPathAsli);
                                LblFoto.Visible = false;
                            }
                            else
                            {
                                picFoto.Image = null;
                                LblFoto.Visible = true;
                            }
                        }
                        else
                        {
                            picFoto.Image = null;
                            LblFoto.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat memuat data pengguna: " + ex.Message);
            }
        }

        private void LblFoto_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fotoPathBaru = openFileDialog1.FileName;
                picFoto.Image = Image.FromFile(fotoPathBaru);
                picFoto.SizeMode = PictureBoxSizeMode.StretchImage;
                LblFoto.Visible = false;
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            FrmMenu f = new FrmMenu();
            this.Hide();        
            f.ShowDialog();
        } // menu button

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPassword.Clear();
            fotoPathBaru = "";
            LoadDataPengguna();
            MessageBox.Show("Perubahan dibatalkan dan data telah direfresh.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string passwordBaru = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(passwordBaru))
            {
                MessageBox.Show("Masukkan password baru terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string pathSimpanFoto = fotoPathAsli;

            // Jika user memilih foto baru, salin ke folder project
            if (!string.IsNullOrEmpty(fotoPathBaru))
            {
                string folderTujuan = Path.Combine(Application.StartupPath, "FotoPengguna");
                if (!Directory.Exists(folderTujuan))
                    Directory.CreateDirectory(folderTujuan);

                string namaFileBaru = $"user_{Session.IDPengguna}_{Path.GetFileName(fotoPathBaru)}";
                pathSimpanFoto = Path.Combine(folderTujuan, namaFileBaru);

                File.Copy(fotoPathBaru, pathSimpanFoto, true);
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = "UPDATE tbl_pengguna SET password = @pass, foto = @foto WHERE id_pengguna = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@pass", passwordBaru);
                        cmd.Parameters.AddWithValue("@foto", pathSimpanFoto);
                        cmd.Parameters.AddWithValue("@id", Session.IDPengguna);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("✅ Perubahan berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Clear();
                fotoPathBaru = "";
                LoadDataPengguna();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat menyimpan perubahan: " + ex.Message);
            }
        }

    }
}
