using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace WarungPintar
{
    public partial class FrmNota : Form
    {
        private string idTransaksi;
        private string connStr = "server=localhost;database=db_warung;username=root;password=;";

        public FrmNota(string idTrx)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.idTransaksi = idTrx;
        }

        private void FrmNota_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDocument rpt = new ReportDocument();
                rpt.Load(Application.StartupPath + "\\NotaTransaksi.rpt");

                // Set koneksi ke MySQL
                ConnectionInfo connInfo = new ConnectionInfo
                {
                    ServerName = "localhost",
                    DatabaseName = "db_warung",
                    UserID = "root",
                    Password = ""
                };

                foreach (Table table in rpt.Database.Tables)
                {
                    TableLogOnInfo logonInfo = table.LogOnInfo;
                    logonInfo.ConnectionInfo = connInfo;
                    table.ApplyLogOnInfo(logonInfo);
                }

                // Set parameter id_transaksi
                rpt.SetParameterValue("id_transaksi", idTransaksi);

                crystalReportViewer1.ReportSource = rpt;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan nota: " + ex.Message);
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
