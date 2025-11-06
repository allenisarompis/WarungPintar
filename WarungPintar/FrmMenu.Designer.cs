namespace WarungPintar
{
    partial class FrmMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMenu));
            this.btnMB = new System.Windows.Forms.Button();
            this.btnData = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnTR = new System.Windows.Forms.Button();
            this.btnLK = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMB
            // 
            this.btnMB.Location = new System.Drawing.Point(128, 149);
            this.btnMB.Name = "btnMB";
            this.btnMB.Size = new System.Drawing.Size(127, 71);
            this.btnMB.TabIndex = 0;
            this.btnMB.Text = "Manajemen Barang";
            this.btnMB.UseVisualStyleBackColor = true;
            this.btnMB.Click += new System.EventHandler(this.btnMB_Click);
            // 
            // btnData
            // 
            this.btnData.Location = new System.Drawing.Point(535, 149);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(127, 71);
            this.btnData.TabIndex = 1;
            this.btnData.Text = "User Data";
            this.btnData.UseVisualStyleBackColor = true;
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(27, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(59, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(121, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(340, 37);
            this.label1.TabIndex = 6;
            this.label1.Text = "Warung Pintar > Menu";
            // 
            // btnTR
            // 
            this.btnTR.Location = new System.Drawing.Point(128, 272);
            this.btnTR.Name = "btnTR";
            this.btnTR.Size = new System.Drawing.Size(127, 71);
            this.btnTR.TabIndex = 7;
            this.btnTR.Text = "Transaksi";
            this.btnTR.UseVisualStyleBackColor = true;
            this.btnTR.Click += new System.EventHandler(this.btnTR_Click);
            // 
            // btnLK
            // 
            this.btnLK.Location = new System.Drawing.Point(334, 149);
            this.btnLK.Name = "btnLK";
            this.btnLK.Size = new System.Drawing.Size(127, 71);
            this.btnLK.TabIndex = 8;
            this.btnLK.Text = "Laporan Keuangan";
            this.btnLK.UseVisualStyleBackColor = true;
            this.btnLK.Click += new System.EventHandler(this.btnLK_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(334, 272);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(127, 71);
            this.btnAbout.TabIndex = 10;
            this.btnAbout.Text = "About Us";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(535, 272);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(127, 71);
            this.btnLogout.TabIndex = 9;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // FrmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnLK);
            this.Controls.Add(this.btnTR);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnData);
            this.Controls.Add(this.btnMB);
            this.Name = "FrmMenu";
            this.Text = "FrmMenu";
            this.Load += new System.EventHandler(this.FrmMenu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMB;
        private System.Windows.Forms.Button btnData;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnTR;
        private System.Windows.Forms.Button btnLK;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnLogout;
    }
}