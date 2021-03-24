using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rancher_Rush
{
    public partial class SelectChara : Form
    {
        private string user;
        public SelectChara()
        {
            InitializeComponent();
        }

        public SelectChara(string username)
        {
            InitializeComponent();
            user = username;
        }

        int Urut = 1;
        string Pilihan;
        string path = Application.StartupPath + "/";

        private void SelectChara_Load(object sender, EventArgs e)
        {
            pBUtama.ImageLocation = path + characterTableAdapter.SelectGambar("P1");
            pBKiri.ImageLocation = path + characterTableAdapter.SelectGambar("P2"); 
            pBKanan.ImageLocation = path + characterTableAdapter.SelectGambar("P3");
            Pilihan = "P1";
        }

        private void lblKanan_Click(object sender, EventArgs e)
        {
            if (Urut == 1)
            {
                pBUtama.ImageLocation = path + characterTableAdapter.SelectGambar("P3");
                pBKiri.ImageLocation = path + characterTableAdapter.SelectGambar("P1");
                pBKanan.ImageLocation = path + characterTableAdapter.SelectGambar("P2");
                Pilihan = "P3";
                Urut++;
            }
            else if (Urut == 2)
            {
                pBUtama.ImageLocation = path + characterTableAdapter.SelectGambar("P2");
                pBKiri.ImageLocation = path + characterTableAdapter.SelectGambar("P3");
                pBKanan.ImageLocation = path + characterTableAdapter.SelectGambar("P1");
                Pilihan = "P2";
                Urut++;
            }
            else if (Urut == 3)
            {
                pBUtama.ImageLocation = path + characterTableAdapter.SelectGambar("P1");
                pBKiri.ImageLocation = path + characterTableAdapter.SelectGambar("P2");
                pBKanan.ImageLocation = path + characterTableAdapter.SelectGambar("P3");
                Pilihan = "P1";
                Urut = 1;
            }
            
        }

        private void lblKiri_Click(object sender, EventArgs e)
        {
            if (Urut == 1)
            {
                pBUtama.ImageLocation = path + characterTableAdapter.SelectGambar("P3");
                pBKiri.ImageLocation = path + characterTableAdapter.SelectGambar("P1");
                pBKanan.ImageLocation = path + characterTableAdapter.SelectGambar("P2");
                Pilihan = "P3";
                Urut = 3;
            }
            else if (Urut == 2)
            {
                pBUtama.ImageLocation = path + characterTableAdapter.SelectGambar("P2");
                pBKiri.ImageLocation = path + characterTableAdapter.SelectGambar("P3");
                pBKanan.ImageLocation = path + characterTableAdapter.SelectGambar("P1");
                Pilihan = "P2";
                Urut--;
            }
            else if (Urut == 3)
            {
                pBUtama.ImageLocation = path + characterTableAdapter.SelectGambar("P1");
                pBKiri.ImageLocation = path + characterTableAdapter.SelectGambar("P2");
                pBKanan.ImageLocation = path + characterTableAdapter.SelectGambar("P3");
                Pilihan = "P1";
                Urut--;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            playerTableAdapter.InsertCharacter(txtName.Text, 5, 5, 1, Pilihan, user);
            Menu frmOpen = new Menu(user);
            frmOpen.Owner = this;
            this.Hide();
            frmOpen.ShowDialog();
        }

        private void SelectChara_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }
    }
}
