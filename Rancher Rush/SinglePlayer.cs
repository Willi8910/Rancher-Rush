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
    public partial class SinglePlayer : Form
    {
       
        Timer Bla = new Timer(); // untuk masukkan timer ke class
        Aksi Reaksi = new Aksi(); // class

        int wak = 0;
        // untuk buat simpan binatang
        Domba Sheep = new Domba();
        Domba Ber = new Domba();
        Domba Bar = new Domba();
        Domba Shee = new Domba();
        Domba She = new Domba();
        Domba Be = new Domba();
        Domba Ba = new Domba();

        // listnya
        //List<Label> ListLabel; // label
        List<PictureBox> ListPort; // untuk simpan portal
        List<PictureBox> Power;

        // simpan segala spec hewan

        int kecilAtas = 10000;
        int kecilBawa = 10000;
        int kecilKiri = 10000;
        int kecilKanan = 10000;

        string User;
        string path = Application.StartupPath + "/";
        Menu frmMenu;

        public SinglePlayer()
        {
            InitializeComponent();
        }
        public SinglePlayer(string username)
        {
            InitializeComponent();
            User = username;
            frmMenu = new Menu(User);
        }

        private void SinglePlayer_Load(object sender, EventArgs e)
        {
            Power = new List<PictureBox> { o1, o2, o3, o4, o5 };

            foreach (PictureBox Lab in Power)
            {
                PowerUp PPU = new PowerUp();
                PPU.Pict = Lab;
                Reaksi.ListPower.Add(PPU);
            }

            Pan.Top = 0;
            Pan.Left = 0;
            //panel1.Visible = false;

            Waktu.Start();
            // ini untuk buat kotak di sekitar binatangnya(nanti untuk "Gerak Kotak")


            // add listnya harus di sini
            Reaksi.ListLabel = new List<Label> { l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12, l13,l14, lo, lop };
            ListPort = new List<PictureBox> { c1, c2, c3, c4,c5 };

            //pemilihan hewan serta specnya
            Random Pilih = new Random(); // pilih random cepatnya
            int Cepat; // kecepatan lari hewan
            Cepat = Pilih.Next(3, 8);
            Cepat = 5;
            Reaksi.Hitung(Sheep, Cepat, p2, ListPort);
            Cepat = 6;
            Reaksi.Hitung(Ber, Cepat, Player, ListPort);
            Cepat = 7;
            Reaksi.Hitung(Bar, Cepat, AltPlayer, ListPort);
            Cepat = 6;
            Reaksi.Hitung(Shee, Cepat, s1, ListPort);
            Cepat = 7;
            Reaksi.Hitung(She, Cepat, s2, ListPort);


           // Detail.Left = 35; // ini nda jelas hanya untuk pengecekn bug

            Jalan.Start(); // timer_1 di pemanggilnya wkwk
            PowerUPp.Start();
            Play.ImageLocation = path + characterTableAdapter1.PilihPlayer(User);

        }

        private void SinglePlayer_KeyDown(object sender, KeyEventArgs e)
        {
            #region Power
            for (int i = 0; i < Reaksi.ListPower.Count; i++)
            {
                if (Play.Bounds.IntersectsWith(Reaksi.ListPower[i].Pict.Bounds) && Reaksi.ListPower[i].Pict.Visible == true)
                {
                    Reaksi.ListPower[i].Pict.Visible = false;
                    if (Reaksi.ListPower[i].Pow == 1)
                    {
                        Reaksi.UseIncSpeed();
                    }
                    if (Reaksi.ListPower[i].Pow == 2)
                    {
                        Reaksi.UseDecSpeed();
                    }
                    if (Reaksi.ListPower[i].Pow == 3)
                    {
                        Reaksi.UseNoSpeed();
                    }
                }
            }
            #endregion

            #region Gerak
            // ini untuk buat playernya bisa gerak di dalam panel
            int Minus = -Pan.Top;
            int Verti = -Pan.Left;

            #region Shortcut kecepatan
            if (e.KeyCode == Keys.E)
            {
                Reaksi.UseDecSpeed();
            }
            if (e.KeyCode == Keys.R)
            {
                Reaksi.UseIncSpeed();
            }
            if (e.KeyCode == Keys.S)
            {
                Reaksi.UseNoSpeed();
            }
            #endregion

            #region Kanan, Kiri, Atas Bawah
            if (e.KeyCode == Keys.Left)
            {
                Reaksi.GerakKiri(Play, this.ClientSize.Width / 2, ref kecilAtas, ref kecilBawa, ref kecilKanan, ref kecilKiri, Pan, l1, path + characterTableAdapter1.PilihKiri(User));
            }
            if (e.KeyCode == Keys.Right)
            {
                Reaksi.GerakKanan(this.ClientSize.Width, Play, this.ClientSize.Width / 2, ref kecilAtas, ref kecilBawa, ref kecilKanan, ref kecilKiri, Pan, l3, path + characterTableAdapter1.PilihKanan(User));
            }
            if (e.KeyCode == Keys.Up)
            {
                Reaksi.GerakAtas(Play, this.ClientSize.Height / 2, ref kecilAtas, ref kecilBawa, ref kecilKanan, ref kecilKiri, Pan, l2);
            }
            if (e.KeyCode == Keys.Down)
            {
                Reaksi.GerakBawah(this.ClientSize.Height, Play, this.ClientSize.Height / 2, ref kecilAtas, ref kecilBawa, ref kecilKanan, ref kecilKiri, Pan, l4);
            }
            #endregion

            #endregion

            // pengecekan collis dgn player
            if (e.KeyCode == Keys.Enter)
            {
                //Reaksi.SpeedUpPlayer();
                for (int i = 0; i < Reaksi.ListDomba.Count; i++)
                {
                    Reaksi.ListDomba[i].Lamb.Visible = true;
                }
            }
        }

        private void Jalan_Tick(object sender, EventArgs e)
        {
            int Minus = -Pan.Top;
            int Verti = -Pan.Left;

            Reaksi.Move(ListPort, Play);
        }

        private void PowerUPp_Tick(object sender, EventArgs e)
        {
            Random Acak = new Random();
            int Powe = Acak.Next(1, 4);
            int Who = Acak.Next(0, Reaksi.ListPower.Count);
            Reaksi.ListPower[Who].Pict.Visible = true;

            if (Powe == 1)
            {
                Reaksi.ListPower[Who].Pow = 1;
                Reaksi.ListPower[Who].lab.Text = "Increase";
                Reaksi.ListPower[Who].Pict.Image = Properties.Resources.Speed;
            }
            if (Powe == 2)
            {
                Reaksi.ListPower[Who].Pow = 2;
                Reaksi.ListPower[Who].lab.Text = "Decrease";
                Reaksi.ListPower[Who].Pict.Image = Properties.Resources.Slow;
            }
            if (Powe == 3)
            {
                Reaksi.ListPower[Who].Pow = 3;
                Reaksi.ListPower[Who].lab.Text = "Stop";
                Reaksi.ListPower[Who].Pict.Image = Properties.Resources.Stop;
            }
        }

        private void SinglePlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void Waktu_Tick(object sender, EventArgs e)
        {
            wak += 1;
            if(wak == 3)
            {
                MessageBox.Show("Kamu Kalah");
                frmMenu.Show();
                this.Hide();
            }

        }

    }
}
