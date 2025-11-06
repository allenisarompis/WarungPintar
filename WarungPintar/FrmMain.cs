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
                dataGridView1.Columns[5].HeaderText = "Status";

                // Isi ComboBox kategori
                CBKategori.Items.Clear();
                CBKategori.Items.AddRange(new string[] { "Makanan", "Minuman", "Cemilan"});

                // Isi ComboBox status
                CBStatus.Items.Clear();
                CBStatus.Items.AddRange(new string[] { "Tersedia", "Tak Tersedia" });

                // Kosongkan input
                txtKode.Clear();
                txtProduk.Clear();
                txtHarga.Clear();
                CBKategori.SelectedIndex = -1;
                txtStok.Clear();
                CBStatus.SelectedIndex = -1;
                txtKode.Focus();

                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnClear.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                koneksi.Open();

                // Hanya isi kode → cari berdasarkan kode
                if (!string.IsNullOrEmpty(kode) && string.IsNullOrEmpty(nama))
                {
                    query = "SELECT * FROM tbl_barang WHERE kode = @kode";
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@kode", kode);
                }
                // hanya isi nama → cari berdasarkan penggalan nama
                else if (string.IsNullOrEmpty(kode) && !string.IsNullOrEmpty(nama))
                {
                    query = "SELECT * FROM tbl_barang WHERE nama_produk LIKE @nama";
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@nama", "%" + nama + "%");
                }
                // isi dua-duanya → cari berdasarkan dua-duanya
                else
                {
                    query = "SELECT * FROM tbl_barang WHERE kode = @kode OR nama_produk LIKE @nama";
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@kode", kode);
                    perintah.Parameters.AddWithValue("@nama", "%" + nama + "%");
                }

                adapter = new MySqlDataAdapter(perintah);

                adapter.Fill(ds);
                koneksi.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];

                    // Set judul kolom
                    dataGridView1.Columns[0].HeaderText = "Kode Barang";
                    dataGridView1.Columns[1].HeaderText = "Nama Produk";
                    dataGridView1.Columns[2].HeaderText = "Harga";
                    dataGridView1.Columns[3].HeaderText = "Kategori";
                    dataGridView1.Columns[4].HeaderText = "Stok";
                    dataGridView1.Columns[5].HeaderText = "Status";

                    // Jika hanya satu data → isi otomatis ke textbox dan combobox
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        txtKode.Text = row["kode"].ToString();
                        txtProduk.Text = row["nama_produk"].ToString();
                        txtHarga.Text = row["harga"].ToString();

                        txtStok.Text = row["stok"].ToString();
                        CBKategori.SelectedItem = row["kategori"].ToString();
                        CBStatus.SelectedItem = row["status"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("❌ Data tidak ditemukan!");
                    FrmMain_Load(null, null); // refresh semua tampilan
                }

                // Aktifkan tombol
                btnAdd.Enabled = true;
                btnUpdate.Enabled = true;
                btnClear.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                koneksi.Close();
            }

            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnClear.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // input
                if (string.IsNullOrWhiteSpace(txtKode.Text) ||
                    string.IsNullOrWhiteSpace(txtProduk.Text) ||
                    string.IsNullOrWhiteSpace(txtHarga.Text) ||
                    string.IsNullOrWhiteSpace(txtStok.Text) ||
                    CBKategori.SelectedIndex == -1 ||
                    CBStatus.SelectedIndex == -1)
                {
                    MessageBox.Show("⚠️ Mohon isi semua data terlebih dahulu!");
                    return;
                }

                // konfirmasi sebelum update
                DialogResult konfirmasi = MessageBox.Show(
                    "Apakah Anda yakin ingin memperbarui data ini?",
                    "Konfirmasi Update",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (konfirmasi == DialogResult.No)
                    return;

                // Query Update
                string query = @"UPDATE tbl_barang 
                         SET nama_produk = @nama, 
                             harga = @harga, 
                             kategori = @kategori, 
                             stok = @stok,
                             status = @status
                         WHERE kode = @kode";

                koneksi.Open();
                MySqlCommand perintah = new MySqlCommand(query, koneksi);
                perintah.Parameters.AddWithValue("@kode", txtKode.Text.Trim());
                perintah.Parameters.AddWithValue("@nama", txtProduk.Text.Trim());
                perintah.Parameters.AddWithValue("@harga", txtHarga.Text.Trim());
                perintah.Parameters.AddWithValue("@kategori", CBKategori.SelectedItem.ToString());
                perintah.Parameters.AddWithValue("@stok", txtStok.Text.Trim());
                perintah.Parameters.AddWithValue("@status", CBStatus.SelectedItem.ToString());

                int hasil = perintah.ExecuteNonQuery();
                koneksi.Close();

                if (hasil == 1)
                {
                    MessageBox.Show("Data berhasil diperbarui!");
                    FrmMain_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Gagal memperbarui data atau data tidak ditemukan!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                koneksi.Close();
            }

            // button
            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnClear.Enabled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtKode.Clear();
                txtProduk.Clear();
                txtHarga.Clear();
                txtStok.Clear();
                CBKategori.SelectedIndex = -1;
                CBStatus.SelectedIndex = -1;

                // Kembalikan fokus ke kode barang
                txtKode.Focus();

                // Refresh DataGridView dengan data terbaru
                ds.Clear();
                query = "SELECT * FROM tbl_barang";
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
                dataGridView1.Columns[5].HeaderText = "Status";

                // Aktifkan/Nonaktifkan tombol
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnClear.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat membersihkan form: " + ex.Message);
                koneksi.Close();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            try
            {
                // semua input terisi
                if (txtKode.Text != "" && txtProduk.Text != "" && txtHarga.Text != "" &&
                    CBKategori.SelectedIndex != -1 && txtStok.Text != "" && CBStatus.SelectedIndex != -1)
                {
                    query = "INSERT INTO tbl_barang (kode, nama_produk, harga, kategori, stok, status) " +
                            "VALUES (@kode, @nama, @harga, @kategori, @stok, @status)";

                    perintah = new MySqlCommand(query, koneksi);

                    perintah.Parameters.AddWithValue("@kode", txtKode.Text);
                    perintah.Parameters.AddWithValue("@nama", txtProduk.Text);
                    perintah.Parameters.AddWithValue("@harga", txtHarga.Text);
                    perintah.Parameters.AddWithValue("@kategori", CBKategori.SelectedItem.ToString());
                    perintah.Parameters.AddWithValue("@stok", txtStok.Text);
                    perintah.Parameters.AddWithValue("@status", CBStatus.SelectedItem.ToString());

                    koneksi.Open();
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Data berhasil ditambahkan!");
                        FrmMain_Load(null, null); // refresh dataGrid dan input
                    }
                    else
                    {
                        MessageBox.Show("Gagal menambahkan data!");
                    }
                }
                else
                {
                    MessageBox.Show("Data belum lengkap!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                koneksi.Close();
            }

            // button
            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnClear.Enabled = true;
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
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
                koneksi.Open();

                // Hanya isi kode → cari berdasarkan kode
                if (!string.IsNullOrEmpty(kode) && string.IsNullOrEmpty(nama))
                {
                    query = "SELECT * FROM tbl_barang WHERE kode = @kode";
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@kode", kode);
                }
                // hanya isi nama → cari berdasarkan penggalan nama
                else if (string.IsNullOrEmpty(kode) && !string.IsNullOrEmpty(nama))
                {
                    query = "SELECT * FROM tbl_barang WHERE nama_produk LIKE @nama";
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@nama", "%" + nama + "%");
                }
                // isi dua-duanya → cari berdasarkan dua-duanya
                else
                {
                    query = "SELECT * FROM tbl_barang WHERE kode = @kode OR nama_produk LIKE @nama";
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@kode", kode);
                    perintah.Parameters.AddWithValue("@nama", "%" + nama + "%");
                }

                adapter = new MySqlDataAdapter(perintah);
                adapter.Fill(ds);
                koneksi.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];

                    // Set judul kolom
                    dataGridView1.Columns[0].HeaderText = "Kode Barang";
                    dataGridView1.Columns[1].HeaderText = "Nama Produk";
                    dataGridView1.Columns[2].HeaderText = "Harga";
                    dataGridView1.Columns[3].HeaderText = "Kategori";
                    dataGridView1.Columns[4].HeaderText = "Stok";
                    dataGridView1.Columns[5].HeaderText = "Status";

                    // hanya satu data → isi otomatis ke textbox dan combobox
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        txtKode.Text = row["kode"].ToString();
                        txtProduk.Text = row["nama_produk"].ToString();
                        txtHarga.Text = row["harga"].ToString();
                        txtStok.Text = row["stok"].ToString();
                        CBKategori.SelectedItem = row["kategori"].ToString();
                        CBStatus.SelectedItem = row["status"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan!");
                    FrmMain_Load(null, null); // refresh tampilan
                }

                btnAdd.Enabled = true;
                btnUpdate.Enabled = true;
                btnClear.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                koneksi.Close();
            }

            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnClear.Enabled = true;
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            FrmMenu f = new FrmMenu();
            this.Hide();
            f.ShowDialog();
        }
    }
}
