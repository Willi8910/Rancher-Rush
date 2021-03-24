namespace Rancher_Rush
{
    partial class frmJoin
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
            this.textIp = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textIp
            // 
            this.textIp.Location = new System.Drawing.Point(12, 12);
            this.textIp.Name = "textIp";
            this.textIp.Size = new System.Drawing.Size(260, 20);
            this.textIp.TabIndex = 8;
            this.textIp.Text = "Enter Server Ip Address";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 38);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(260, 23);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // frmJoin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Rancher_Rush.Properties.Resources.Brewster_Blue_Gauzy_Texture_Wallpaper_P15472879;
            this.ClientSize = new System.Drawing.Size(284, 79);
            this.Controls.Add(this.textIp);
            this.Controls.Add(this.btnStart);
            this.Name = "frmJoin";
            this.Text = "Join";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmJoin_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textIp;
        private System.Windows.Forms.Button btnStart;
    }
}