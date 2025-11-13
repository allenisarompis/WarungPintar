using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
    public partial class FrmTransaksi : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;

        private string connStr = "server=localhost;database=db_warung;username=root;password=;";
        private DataTable dtKeranjang = new DataTable();
        private DataTable dtBarang = new DataTable();

        private int totalHarga = 0;

        public FrmTransaksi()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            koneksi = new MySqlConnection(connStr);
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            FrmMenu f = new FrmMenu();
            this.Hide();
            f.ShowDialog();
        } 

        private void FrmTransaksi_Load(object sender, EventArgs e)
        {
            btnPrint.Enabled = false; // print aktif setelah save

            BuatKeranjang();
            LoadBarang();
            txtIdTransaksi.Text = GenerateIDTransaksi();
        }

        // Generate ID Transaksi
        private string GenerateIDTransaksi()
        {
            return "TRX" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void LoadBarang()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT kode, nama_produk, harga, stok, status FROM tbl_barang WHERE status = 'Tersedia'";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                dtBarang.Clear();
                da.Fill(dtBarang);
                dgvBarang.DataSource = dtBarang;
            }
        }
        private void BuatKeranjang()
        {
            dtKeranjang.Columns.Add("Kode Barang");
            dtKeranjang.Columns.Add("Nama Produk");
            dtKeranjang.Columns.Add("Harga", typeof(int));
            dtKeranjang.Columns.Add("Jumlah", typeof(int));
            dtKeranjang.Columns.Add("total", typeof(int));

            dgvTransaksi.DataSource = dtKeranjang;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvBarang.Rows[e.RowIndex];
                txtKode.Text = row.Cells["kode"].Value.ToString();
                txtProduk.Text = row.Cells["nama_produk"].Value.ToString();
                txtHarga.Text = row.Cells["harga"].Value.ToString();
            }
        }
        // dgv barang 
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtKode.Text) || string.IsNullOrEmpty(txtJumlah.Text))
                {
                    MessageBox.Show("Isi kode barang dan jumlah terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Pastikan dtKeranjang sudah memiliki kolom
                if (dtKeranjang.Columns.Count == 0)
                {
                    dtKeranjang.Columns.Add("Kode Barang");
                    dtKeranjang.Columns.Add("Nama Produk");
                    dtKeranjang.Columns.Add("Harga", typeof(int));
                    dtKeranjang.Columns.Add("Jumlah", typeof(int));
                    dtKeranjang.Columns.Add("total", typeof(int));
                    dgvTransaksi.DataSource = dtKeranjang;
                }

                // Ambil nilai dari textbox
                string kode = txtKode.Text;
                string nama = txtProduk.Text;
                int harga = int.Parse(txtHarga.Text);
                int jumlah = int.Parse(txtJumlah.Text);
                int total = harga * jumlah;

                // Cek apakah barang sudah ada di keranjang
                DataRow existingRow = dtKeranjang.AsEnumerable().FirstOrDefault(r => r.Field<string>("Kode Barang") == kode);
                if (existingRow != null)
                {
                    // Jika sudah ada, tambahkan jumlah dan subtotal
                    existingRow["Jumlah"] = Convert.ToInt32(existingRow["Jumlah"]) + jumlah;
                    existingRow["total"] = Convert.ToInt32(existingRow["Harga"]) * Convert.ToInt32(existingRow["Jumlah"]);
                }
                else
                {
                    dtKeranjang.Rows.Add(kode, nama, harga, jumlah, total);
                }

                dgvTransaksi.Refresh();
                HitungTotal();

                ClearText();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat menambah barang: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // btnAdd
        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvTransaksi.SelectedRows.Count > 0)
            {
                int index = dgvTransaksi.SelectedRows[0].Index;
                dtKeranjang.Rows.RemoveAt(index);
                HitungTotal();
            }
            else
            {
                MessageBox.Show("Pilih barang yang ingin dihapus dari transaksi!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // btnDelete
        private void HitungTotal()
        {
            int total = 0;
            foreach (DataRow row in dtKeranjang.Rows)
            {
                total += Convert.ToInt32(row["total"]);
            }
            lblTotal.Text = "Rp " + total.ToString("N0");
        }

        private void dgvTransaksi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dtKeranjang.Rows.Count == 0)
            {
                MessageBox.Show("Keranjang kosong. Tambahkan barang terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Hitung total (pastikan HitungTotal() sudah benar)
            int totalTransaksi = 0;
            foreach (DataRow r in dtKeranjang.Rows)
            {
                totalTransaksi += Convert.ToInt32(r["total"]);
            }

            string idTrx = txtIdTransaksi.Text.Trim();
            if (string.IsNullOrEmpty(idTrx))
                idTrx = GenerateIDTransaksi();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                using (MySqlTransaction trx = conn.BeginTransaction())
                {
                    try
                    {
                        // 1) Insert header ke tbl_transaksi
                        string qHeader = "INSERT INTO tbl_transaksi (id_transaksi, total, id_pengguna) VALUES (@id, @total, @id_user)";
                        using (MySqlCommand cmdH = new MySqlCommand(qHeader, conn, trx))
                        {
                            cmdH.Parameters.AddWithValue("@id", idTrx);
                            cmdH.Parameters.AddWithValue("@total", totalTransaksi);

                            // gunakan Session.IDPengguna jika tersedia; default 0 jika tidak
                            int idUser = 0;
                            if (!string.IsNullOrEmpty(Session.IDPengguna))
                                int.TryParse(Session.IDPengguna, out idUser);
                            cmdH.Parameters.AddWithValue("@id_user", idUser);

                            cmdH.ExecuteNonQuery();
                        }

                        // 2) Insert setiap baris ke tbl_detail_transaksi
                        string qDetail = @"INSERT INTO tbl_detail_transaksi
                                   (id_transaksi, kode, nama_produk, harga, jumlah, total)
                                   VALUES (@id, @kode, @nama, @harga, @jumlah, @total)";

                        foreach (DataRow row in dtKeranjang.Rows)
                        {
                            using (MySqlCommand cmdD = new MySqlCommand(qDetail, conn, trx))
                            {
                                cmdD.Parameters.AddWithValue("@id", idTrx);
                                cmdD.Parameters.AddWithValue("@kode", row["Kode Barang"].ToString());
                                cmdD.Parameters.AddWithValue("@nama", row["Nama Produk"].ToString());
                                cmdD.Parameters.AddWithValue("@harga", Convert.ToInt32(row["Harga"]));
                                cmdD.Parameters.AddWithValue("@jumlah", Convert.ToInt32(row["Jumlah"]));
                                cmdD.Parameters.AddWithValue("@total", Convert.ToInt32(row["total"]));
                                cmdD.ExecuteNonQuery();
                            }
                        }

                        // Commit apabila semua berhasil
                        trx.Commit();

                        MessageBox.Show("✅ Transaksi berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reset UI: kosongkan keranjang, update total, set id trx baru, aktifkan print
                        dtKeranjang.Clear();
                        lblTotal.Text = "Rp 0";
                        totalHarga = 0;
                        txtIdTransaksi.Text = GenerateIDTransaksi();
                        btnPrint.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        try { trx.Rollback(); } catch { }
                        MessageBox.Show("Gagal menyimpan transaksi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
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

                // Tentukan query berdasarkan input
                if (!string.IsNullOrEmpty(kode) && string.IsNullOrEmpty(nama))
                {
                    query = "SELECT kode, nama_produk, harga, stok FROM tbl_barang WHERE kode= @kode";
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@kode", kode);
                }
                else if (string.IsNullOrEmpty(kode) && !string.IsNullOrEmpty(nama))
                {
                    query = "SELECT kode, nama_produk, harga, stok FROM tbl_barang WHERE nama_produk LIKE @nama";
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@nama", "%" + nama + "%");
                }
                else
                {
                    query = "SELECT kode, nama_produk, harga, stok FROM tbl_barang WHERE kode = @kode OR nama_produk LIKE @nama";
                    perintah = new MySqlCommand(query, koneksi);
                    perintah.Parameters.AddWithValue("@kode", kode);
                    perintah.Parameters.AddWithValue("@nama", "%" + nama + "%");
                }

                adapter = new MySqlDataAdapter(perintah);
                adapter.Fill(ds);
                koneksi.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dgvBarang.DataSource = ds.Tables[0];

                    // Set header kolom agar lebih rapi
                    dgvBarang.Columns[0].HeaderText = "Kode Barang";
                    dgvBarang.Columns[1].HeaderText = "Nama Produk";
                    dgvBarang.Columns[2].HeaderText = "Harga (Rp)";
                    dgvBarang.Columns[3].HeaderText = "Stok";

                    // Jika hanya satu hasil, langsung isi textbox
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        txtKode.Text = row["kode"].ToString();
                        txtProduk.Text = row["nama_produk"].ToString();
                        txtHarga.Text = row["harga"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Barang tidak ditemukan!");
                    dgvBarang.DataSource = null;
                }

                btnAdd.Enabled = true;
                btnRefresh.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                koneksi.Close();
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdTransaksi.Text))
            {
                MessageBox.Show("Transaksi belum tersimpan atau ID kosong.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FrmNota nota = new FrmNota(txtIdTransaksi.Text);
            nota.ShowDialog();
        }

        private void ClearText()
        {
            txtKode.Clear();
            txtProduk.Clear();
            txtHarga.Clear();
            txtJumlah.Clear();
        }
    }
}
