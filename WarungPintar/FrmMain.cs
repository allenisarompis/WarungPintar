using MySql.Data.MySqlClient;
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
    public partial class FrmMain : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;

        public FrmMain()
        {
            alamat = "server=localhost; database=db_warung; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                query = "SELECT * FROM tbl_barang";
                ds.Clear();
                koneksi.Open();
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);
                adapter.Fill(ds);
                koneksi.Close();

                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].HeaderText = "Kode Barang";
                dataGridView1.Columns[1].HeaderText = "Nama Produk";
                dataGridView1.Columns[2].HeaderText = "Harga";
                dataGridView1.Columns[3].HeaderText = "Kategori";
                dataGridView1.Columns[4].HeaderText = "Stok";

                // Kosongkan input
                txtKode.Clear();
                txtProduk.Clear();
                txtHarga.Clear();
                txtKategori.Clear();
                txtStok.Clear();

                // Fokus ke kode
                txtKode.Focus();

                // Atur tombol
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmMenu f = new FrmMenu();
            this.Hide();           // sembunyikan form login
            f.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtKode.Text != "" && txtProduk.Text != "" && txtHarga.Text != "" &&
                    txtKategori.Text != "" && txtStok.Text != "")
                {
                    query = "INSERT INTO tbl_barang (kode, nama_produk, harga, kategori, stok) " +
                            "VALUES (@kode, @nama, @harga, @kategori, @stok)";

                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@kode", txtKode.Text);
                    perintah.Parameters.AddWithValue("@nama", txtProduk.Text);
                    perintah.Parameters.AddWithValue("@harga", txtHarga.Text);
                    perintah.Parameters.AddWithValue("@kategori", txtKategori.Text);
                    perintah.Parameters.AddWithValue("@stok", txtStok.Text);

                    koneksi.Open();
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("✅ Data berhasil ditambahkan!");
                        FrmMain_Load(null, null);
                    }
                    else
                    {
                        MessageBox.Show("❌ Gagal menambahkan data!");
                    }
                }
                else
                {
                    MessageBox.Show("⚠️ Data belum lengkap!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                koneksi.Close();
            }
            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Sudah bisa mencari dari penggalan nama
            // Sudah bisa mencari berdasarkan nama barang dan kode
            try
            {
                string kode = txtKode.Text.Trim();
                string nama = txtProduk.Text.Trim();

                if (string.IsNullOrEmpty(kode) && string.IsNullOrEmpty(nama))
                {
                    MessageBox.Show("⚠️ Masukkan kode atau nama produk untuk mencari!");
                    return;
                }

                ds.Clear();

                // 🔍 Jika user isi kode → cari berdasarkan kode
                if (!string.IsNullOrEmpty(kode) && string.IsNullOrEmpty(nama))
                {
                    query = "SELECT * FROM tbl_barang WHERE kode = @kode";
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@kode", kode);
                }
                // 🔍 Jika user isi nama → cari berdasarkan penggalan nama
                else if (string.IsNullOrEmpty(kode) && !string.IsNullOrEmpty(nama))
                {
                    query = "SELECT * FROM tbl_barang WHERE nama_produk LIKE @nama";
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@nama", "%" + nama + "%");
                }
                // 🔍 Jika user isi dua-duanya → cari berdasarkan dua-duanya
                else
                {
                    query = "SELECT * FROM tbl_barang WHERE kode = @kode OR nama_produk LIKE @nama";
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@kode", kode);
                    perintah.Parameters.AddWithValue("@nama", "%" + nama + "%");
                }

                adapter = new MySqlDataAdapter(perintah);
                koneksi.Open();
                adapter.Fill(ds);
                koneksi.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];

                    // Jika hanya 1 data ditemukan, isi textbox
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        txtKode.Text = row["kode"].ToString();
                        txtProduk.Text = row["nama_produk"].ToString();
                        txtHarga.Text = row["harga"].ToString();
                        txtKategori.Text = row["kategori"].ToString();
                        txtStok.Text = row["stok"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("❌ Data tidak ditemukan!");
                    FrmMain_Load(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                koneksi.Close();
            }

            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            // awalnya tombol untuk delete
            // ganti status barang antara "Tersedia" dan "Tak Tersedia"
            try
            {
                if (txtKode.Text == "")
                {
                    MessageBox.Show("Masukkan kode barang terlebih dahulu!");
                    return;
                }

                // Ambil status saat ini
                string statusSekarang = "";
                query = "SELECT status FROM tbl_barang WHERE kode = @kode";
                koneksi.Open();
                perintah = new MySqlCommand(query, koneksi);
                perintah.Parameters.AddWithValue("@kode", txtKode.Text);
                MySqlDataReader reader = perintah.ExecuteReader();

                if (reader.Read())
                {
                    statusSekarang = reader["status"].ToString();
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan!");
                    reader.Close();
                    koneksi.Close();
                    return;
                }
                reader.Close();

                // Tentukan status baru
                string statusBaru = (statusSekarang == "Tersedia") ? "Tak Tersedia" : "Tersedia";

                // Update status di database
                query = "UPDATE tbl_barang SET status=@status WHERE kode=@kode";
                perintah = new MySqlCommand(query, koneksi);
                perintah.Parameters.AddWithValue("@status", statusBaru);
                perintah.Parameters.AddWithValue("@kode", txtKode.Text);
                int res = perintah.ExecuteNonQuery();
                koneksi.Close();

                if (res == 1)
                {
                    MessageBox.Show("Status berhasil diubah menjadi: " + statusBaru);
                    FrmMain_Load(null, null); // refresh data
                }
                else
                {
                    MessageBox.Show("Gagal mengubah status!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                koneksi.Close();
            }
            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtKode.Text == "")
                {
                    MessageBox.Show("Cari data terlebih dahulu sebelum update!");
                    return;
                }

                query = "UPDATE tbl_barang SET nama_produk=@nama, harga=@harga, kategori=@kategori, stok=@stok WHERE kode=@kode";
                koneksi.Open();
                perintah = new MySqlCommand(query, koneksi);
                perintah.Parameters.AddWithValue("@kode", txtKode.Text);
                perintah.Parameters.AddWithValue("@nama", txtProduk.Text);
                perintah.Parameters.AddWithValue("@harga", txtHarga.Text);
                perintah.Parameters.AddWithValue("@kategori", txtKategori.Text);
                perintah.Parameters.AddWithValue("@stok", txtStok.Text);
                int res = perintah.ExecuteNonQuery();
                koneksi.Close();

                if (res == 1)
                {
                    MessageBox.Show("Data berhasil diupdate!");
                    FrmMain_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Data gagal diupdate!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal update data: " + ex.Message);
                koneksi.Close();
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            FrmAboutUs f = new FrmAboutUs();
            this.Hide(); // sembunyikan form utama (opsional)
            f.ShowDialog();
        }
    }
}
