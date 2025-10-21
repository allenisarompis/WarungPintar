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
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtKode.Text != "" && txtProduk.Text != "" && txtHarga.Text != "" && txtKategori.Text != "" && txtStok.Text != "")
                {
                    query = string.Format("INSERT INTO tbl_barang VALUES ('{0}','{1}','{2}','{3}','{4}')",
                        txtKode.Text, txtProduk.Text, txtHarga.Text, txtKategori.Text, txtStok.Text);

                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Data berhasil ditambahkan!");
                        FrmMain_Load(null, null);
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
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtKode.Text != "")
                {
                    query = "SELECT * FROM tbl_barang WHERE kode = @kode";
                    ds.Clear();
                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@kode", txtKode.Text);
                    adapter = new MySqlDataAdapter(perintah);
                    adapter.Fill(ds);
                    koneksi.Close();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        txtProduk.Text = row["nama_produk"].ToString();
                        txtHarga.Text = row["harga"].ToString();
                        txtKategori.Text = row["kategori"].ToString();
                        txtStok.Text = row["stok"].ToString();
                        dataGridView1.DataSource = ds.Tables[0];
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan!");
                        FrmMain_Load(null, null);
                    }
                }
                else
                {
                    MessageBox.Show("Masukkan kode barang untuk mencari!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                koneksi.Close();
            }
        }
      
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtKode.Text == "")
                {
                    MessageBox.Show("Masukkan kode yang ingin dihapus!");
                    return;
                }

                if (MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    query = "DELETE FROM tbl_barang WHERE kode=@kode";
                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@kode", txtKode.Text);
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Data berhasil dihapus!");
                    }
                    else
                    {
                        MessageBox.Show("Data gagal dihapus!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menghapus data: " + ex.Message);
                koneksi.Close();
            }
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
