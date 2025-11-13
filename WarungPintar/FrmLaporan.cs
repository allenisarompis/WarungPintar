using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WarungPintar
{
    public partial class FrmLaporan : Form
    {
        private string connStr = "server=localhost;database=db_warung;username=root;password=;";
        public FrmLaporan()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void FrmLaporan_Load(object sender, EventArgs e)
        {
            RefreshView();
        }
        private void LoadSemuaData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT id_transaksi, tanggal, total FROM tbl_transaksi ORDER BY tanggal ASC";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvLaporan.DataSource = dt;

                    HitungTotal(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmMenu f = new FrmMenu();
            this.Hide();
            f.ShowDialog();
        } // menu

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string tglMulai = dtpTanggalMulai.Value.ToString("yyyy-MM-dd");
                string tglSelesai = dtpTanggalSelesai.Value.ToString("yyyy-MM-dd");

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = $@"
                        SELECT id_transaksi, tanggal, total 
                        FROM tbl_transaksi
                        WHERE DATE(tanggal) BETWEEN '{tglMulai}' AND '{tglSelesai}'
                        ORDER BY tanggal ASC";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvLaporan.DataSource = dt;
                    HitungTotal(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat laporan: " + ex.Message);
            }
        }
        private void HitungTotal(DataTable dt)
        {
            int totalSemua = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalSemua += Convert.ToInt32(row["total"]);
            }

            lblTotal.Text = $"Rp.{totalSemua:N0}";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshView();
        }
        private void RefreshView()
        {
            dgvLaporan.DataSource = null;
            dgvLaporan.Rows.Clear();

            dtpTanggalMulai.Value = DateTime.Now;
            dtpTanggalSelesai.Value = DateTime.Now;

            lblTotal.Text = "Rp. 0";
        }
    }
}