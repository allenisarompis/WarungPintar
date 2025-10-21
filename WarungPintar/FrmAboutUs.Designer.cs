namespace WarungPintar
{
    partial class FrmAboutUs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAboutUs));
            this.label1 = new System.Windows.Forms.Label();
            this.txtAbout = new System.Windows.Forms.TextBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(295, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "About Us";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtAbout
            // 
            this.txtAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.txtAbout.Location = new System.Drawing.Point(166, 118);
            this.txtAbout.Multiline = true;
            this.txtAbout.Name = "txtAbout";
            this.txtAbout.ReadOnly = true;
            this.txtAbout.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAbout.Size = new System.Drawing.Size(449, 273);
            this.txtAbout.TabIndex = 1;
            this.txtAbout.Text = resources.GetString("txtAbout.Text");
            this.txtAbout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAbout.TextChanged += new System.EventHandler(this.txtAbout_TextChanged);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(665, 389);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(100, 39);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // FrmAboutUs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.txtAbout);
            this.Controls.Add(this.label1);
            this.Name = "FrmAboutUs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmAboutUs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAbout;
        private System.Windows.Forms.Button btnBack;
    }
}