namespace Rancher_Rush
{
    partial class SelectChara
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
            this.btnPlay = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.pBKanan = new System.Windows.Forms.PictureBox();
            this.pBKiri = new System.Windows.Forms.PictureBox();
            this.pBUtama = new System.Windows.Forms.PictureBox();
            this.lblKanan = new System.Windows.Forms.Label();
            this.lblKiri = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.characterTableAdapter = new Rancher_Rush.GameDataSetTableAdapters.CharacterTableAdapter();
            this.playerTableAdapter = new Rancher_Rush.GameDataSetTableAdapters.PlayerTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.pBKanan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBKiri)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBUtama)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(416, 348);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(87, 39);
            this.btnPlay.TabIndex = 15;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(179, 306);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(145, 20);
            this.txtName.TabIndex = 14;
            // 
            // pBKanan
            // 
            this.pBKanan.BackColor = System.Drawing.Color.Transparent;
            this.pBKanan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pBKanan.Location = new System.Drawing.Point(346, 110);
            this.pBKanan.Name = "pBKanan";
            this.pBKanan.Size = new System.Drawing.Size(104, 102);
            this.pBKanan.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pBKanan.TabIndex = 13;
            this.pBKanan.TabStop = false;
            // 
            // pBKiri
            // 
            this.pBKiri.BackColor = System.Drawing.Color.Transparent;
            this.pBKiri.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pBKiri.Location = new System.Drawing.Point(58, 110);
            this.pBKiri.Name = "pBKiri";
            this.pBKiri.Size = new System.Drawing.Size(104, 102);
            this.pBKiri.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pBKiri.TabIndex = 12;
            this.pBKiri.TabStop = false;
            // 
            // pBUtama
            // 
            this.pBUtama.BackColor = System.Drawing.Color.Transparent;
            this.pBUtama.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pBUtama.Location = new System.Drawing.Point(193, 177);
            this.pBUtama.Name = "pBUtama";
            this.pBUtama.Size = new System.Drawing.Size(120, 116);
            this.pBUtama.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBUtama.TabIndex = 11;
            this.pBUtama.TabStop = false;
            // 
            // lblKanan
            // 
            this.lblKanan.AutoSize = true;
            this.lblKanan.BackColor = System.Drawing.Color.Transparent;
            this.lblKanan.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKanan.Location = new System.Drawing.Point(321, 293);
            this.lblKanan.Name = "lblKanan";
            this.lblKanan.Size = new System.Drawing.Size(43, 46);
            this.lblKanan.TabIndex = 10;
            this.lblKanan.Text = ">";
            this.lblKanan.Click += new System.EventHandler(this.lblKanan_Click);
            // 
            // lblKiri
            // 
            this.lblKiri.AutoSize = true;
            this.lblKiri.BackColor = System.Drawing.Color.Transparent;
            this.lblKiri.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKiri.Location = new System.Drawing.Point(144, 293);
            this.lblKiri.Name = "lblKiri";
            this.lblKiri.Size = new System.Drawing.Size(43, 46);
            this.lblKiri.TabIndex = 9;
            this.lblKiri.Text = "<";
            this.lblKiri.Click += new System.EventHandler(this.lblKiri_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(153, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "Select Your Character";
            // 
            // characterTableAdapter
            // 
            this.characterTableAdapter.ClearBeforeFill = true;
            // 
            // playerTableAdapter
            // 
            this.playerTableAdapter.ClearBeforeFill = true;
            // 
            // SelectChara
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Rancher_Rush.Properties.Resources.Brewster_Blue_Gauzy_Texture_Wallpaper_P15472879;
            this.ClientSize = new System.Drawing.Size(526, 407);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.pBKanan);
            this.Controls.Add(this.pBKiri);
            this.Controls.Add(this.pBUtama);
            this.Controls.Add(this.lblKanan);
            this.Controls.Add(this.lblKiri);
            this.Controls.Add(this.label1);
            this.Name = "SelectChara";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectChara_FormClosing);
            this.Load += new System.EventHandler(this.SelectChara_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pBKanan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBKiri)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBUtama)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.PictureBox pBKanan;
        private System.Windows.Forms.PictureBox pBKiri;
        private System.Windows.Forms.PictureBox pBUtama;
        private System.Windows.Forms.Label lblKanan;
        private System.Windows.Forms.Label lblKiri;
        private System.Windows.Forms.Label label1;
        private GameDataSetTableAdapters.CharacterTableAdapter characterTableAdapter;
        private GameDataSetTableAdapters.PlayerTableAdapter playerTableAdapter;
    }
}