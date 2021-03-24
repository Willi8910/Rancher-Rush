using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Rancher_Rush
{
    public class Domba // btw nama domba karena rencananya dulu hanya domba semua
    {
        public PictureBox Lamb = new PictureBox(); // simpan gambar hewannya
        public int Mark = 0; // untuk simpan jenis pergerakan yang hewan punyai seperti jalan kiri, kanan atau sebegaianya
        public int Speed; // kecepatan lari hewan
        public int OrSpeed;

        #region Berhubungan dengan pergerakan / berubah arah, dll
        // simpan index terdekat dari labelnya(lihat metodnya)
        public int tandaKanan = 0;
        public int tandaKiri = 0;
        public int tandabawa = 0;
        public int tandaatas = 0;

        // dipake untuk cek kata tanda di atas kalo berubah ato tidak jadi di simpan di sini urutnya yang belum berubah
        // baru jalan kan kalo beda(Mbelo benar)
        // dipake di method cek belok

        // yang penting nanti tandakanan cek kanan sama atau tidak kalo beda artinya berubah lalu mengerjakan methodnya
        public int kanan = 0;
        public int bawa = 0;
        public int kiri = 0;
        public int atas = 0;

        // ini untuk menunjukkan arah mana yang bisa di lewati
        public bool MAtas = false;
        public bool MBawa = false;
        public bool MKiri = false;
        public bool MKanan = false;

        // ini berhubungan dengan kiri kanan atas bawah tapi sudah tidak dipake lagi...
        // dulu fungsinya kalo kanan beda dgn tandakanan jadi berkanan true baru bisa belok kanan
        // sudah digantikan oleh  bool Mbelo di bawah
        public bool BerKanan = false,
                    BerKiri = false,
                    BerAtas = false,
                    BerBawah = false;

        public int Counter = 0; // untuk hitung berapa kali tambahnya sampai tingkat tertentu baru bisa berbalik

        // ini untuk bantu hitung label yang terdekat dengan hewan
        public int kecilatas = 10000, kecilbawa = 10000, kecilkiri = 10000, kecilkanan = 10000;

        // ini untuk arah bergeraknya
        public bool MajuKa = false, MajuKi = false, MajuAt = false, MajuBa = false;

        public bool Jalan = true; // ini cek bisa jalan atau tidak
        public bool Mbelo = true; // ini cek kalau bisa belok atau tidak(pengganti dari yang tidak dipake di atas)

        //ini untuk batasi yang mana bisa dilewati atau tidak
        public int LimKiri = 0;
        public int LimAtas = 0;

        // ini kalo belok posisinya berubah supaya hewannya tetap di tengah
        public int KKTop;
        public int KKLeft;
        public int ABTop;
        public int ABLeft;

        #endregion

        #region Simpanan Gambar dari aksi tertentu
        // semua gambar animasinya dari hewan(aksinya)
        public PictureBox Ki = new PictureBox();
        public PictureBox Ka = new PictureBox();
        public PictureBox JumpKi = new PictureBox();
        public PictureBox JumpKa = new PictureBox();
        public PictureBox TransKi = new PictureBox();
        public PictureBox TransKa = new PictureBox();
        public PictureBox TransBack = new PictureBox();

        // ini untuk bantu ubah animasinya arah ke kanan atau ke kiri
        // kalo nda gambarnya terulang terus(mis: kalo timer intervalnya 25)
        // jadi kalo tdk ada hit kanankiri tiap 25 milsec gambarnya ulang
        public int HitKanan = 0;
        public int HitKiri = 0;

        #endregion

        #region Ketika Hewannya akan di Portalkan

        public Timer Tim = new Timer(); // simpan timer pribadi
        Timer PortalBack = new Timer(); // timernya ketika hewannya kembali muncul di portal yang lainnya
        public List<PictureBox> ListPor = new List<PictureBox>(); // simpan portalnya yang ada... sebenarnya bisa di simpan di class aksi tapi entah kenapa ada bug(kalo dipanggil isinya tidak ada) jadi tidak bisa oleh karena ituuuuu... aku simpan di class saja

        public bool HitPort = true; // atur bisa ato tidak transport
        public PictureBox Portal = new PictureBox(); // simpan portalnya supaya gampang pilih portal yang bersangkutan
        public bool DoPort = false; // cek kalo nanti sudah masuk portal atau tidak(ada di methodnya dalam timer)
        int Coun = 0; // hitung berapa kali timernya sudah bekerja

        public void CreateTimer()
        {
            Tim.Tick += Tim_Tick;
            PortalBack.Tick += PortalBack_Tick;
            PortalBack.Interval = 2000;
        } // membuat eventhandler timer baru

        void Tim_Tick(object sender, EventArgs e)
        {
            // kalo sudah terjadi 2 kali maka timernya stop
            // timer ini berguna untuk pengaturan transport

            Coun++;
            Hepil();

            if (Coun == 2)
            {
                Tim.Stop();
                Coun = 0;
            }
        }

        void Hepil()
        {
            if (DoPort == false) // waktu belom transport
            {
                DoPort = true;
                HePin(); // pengaturan transport
            }
            else if (DoPort == true) // sudah transport
            {
                Revert(); // kembalikan seperti awal 
                DoPort = false; // kembalikan ke false
                Lamb.Image = Ka.Image; // ubah gambarnya kembali lari
                Jalan = true; // buat bisa jalan
                HitPort = false; // buat agar tidak masuk portal lagi karena ketika pindah dia pasti kenna portal
            }
        }

        void HePin()
        {
            Portal.Visible = true; // supaya kelihatan lagi
            Portal.Image = Properties.Resources.PortBack; // ganti gambarnya
            PortalBack.Start(); // kembalikan gambarnya 

            // simpan index portalnya
            int Meh = 0;
            for (int i = 0; i < ListPor.Count; i++)
            {
                if (Portal == ListPor[i])
                {
                    Meh = i;
                }
            }

            // pemilihan acak yang di transport
            Random Ack = new Random();
            int k = Ack.Next(0, ListPor.Count);
            int Lim = ListPor.Count - 1;
            // ini supaya dia tdk transport ke tempat yang sama
            // tapi kalo tempatnya yang paling terakhirnya countnya diubah kurang indexnya
            if (k == Meh)
            {
                if (k == Lim)
                {
                    k--;
                }
                else
                {
                    k++;
                }
            }

            if (ListPor.Count == 1)
            {
                k = 0;
            }

            Lamb.Location = ListPor[k].Location; // buat sam atempatnya dengan portal yang di mau
            Lamb.Image = TransBack.Image; // ganti gambarnya
        }

        void Revert()
        {
            // kembalikan semua seperti awal
            tandaKanan = 0;
            tandaKiri = 0;
            tandabawa = 0;
            tandaatas = 0;
            kanan = 0;
            bawa = 0;
            kiri = 0;
            atas = 0;
            MAtas = false;
            MBawa = false;
            MKiri = false;
            MKanan = false;
            BerKanan = false;
            BerKiri = false;
            BerAtas = false;
            BerBawah = false;
            Counter = 0;
            kecilatas = 10000; kecilbawa = 10000; kecilkiri = 10000; kecilkanan = 10000;
            MajuKa = false; MajuKi = false; MajuAt = false; MajuBa = false;
            HitKanan = 0;
            HitKiri = 0;
            Mbelo = true;
        }

        void PortalBack_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < ListPor.Count; i++)
            {
                if (ListPor[i].Visible == true) // semua yang kelihatan
                {
                    ListPor[i].Image = Properties.Resources.Portal;
                }
            }
            PortalBack.Stop(); //  hentikan setelah 1 kali jalan
        }
        #endregion
    }
    public class Aksi
    {
        public List<PowerUp> ListPower = new List<PowerUp>();

        public int CatchAni = 0;
        public bool Catch = false;

        Timer IncSpeed = new Timer();
        Timer DecSpeed = new Timer();
        Timer NoSpeed = new Timer();

        public bool Easy = false;
        public bool Medium = true;
        public bool Hard = false;

        public int SPdPlayer = 5;

        int tandaatas = 0;
        int tandabawa = 0;
        int tandaKanan = 0;
        int tandaKiri = 0;

        // listnya
        public List<Label> ListLabel;
        public List<Domba> ListDomba = new List<Domba>();
        int asd = 10000; //untuk hitung selisih gerak collision   

        public bool SpawnPower = false;
        public bool DelPower = false;
        public int ind = 0;

        public Aksi ()
        {
            DecSpeed.Tick += DecSpeed_Tick;
            IncSpeed.Tick += IncSpeed_Tick;
            NoSpeed.Tick += NoSpeed_Tick;
        }

        #region Pengaturan Power Up
        public void UseNoSpeed()
        {
            NoSpeed.Interval = 3000;
            NoSpeed.Start();
            foreach(Domba Dom in ListDomba)
            {
                Dom.Speed = 0;
            }
        }
        void NoSpeed_Tick(object sender, EventArgs e)
        {
            NoSpeed.Stop();
            foreach (Domba Dom in ListDomba)
            {
                Dom.Speed = Dom.OrSpeed;
            }
        }
        public void UseIncSpeed()
        {
            IncSpeed.Interval = 6500;
            IncSpeed.Start();
            SPdPlayer = 8;
        }
        void IncSpeed_Tick(object sender, EventArgs e)
        {
            IncSpeed.Stop();
            SPdPlayer = 5;
        }
        public void UseDecSpeed()
        {
            DecSpeed.Interval = 5000;
            DecSpeed.Start();
            foreach (Domba Dom in ListDomba)
            {
                Dom.Speed -= 2;
            }

        }
        void DecSpeed_Tick(object sender, EventArgs e)
        {
            DecSpeed.Stop();
            foreach (Domba Dom in ListDomba)
            {
                Dom.Speed = Dom.OrSpeed;
            }
        }
        #endregion

        public void Hitung(Domba Bla, int Kec, PictureBox Pil, List<PictureBox> ListPort)
        {
            // ini  semua pemilihan bagian hewan dengan setiap specnya
            #region Domba Putih
            if (Kec == 3)
            {
                // domba putih
                Bla.Tim.Interval = 1100;
                Bla.Ki.Image = Properties.Resources.D2Ki;
                Bla.Ka.Image = Properties.Resources.D2Ka;
                Bla.JumpKa.Image = Properties.Resources.D2JumpKa;
                Bla.JumpKi.Image = Properties.Resources.D2JumpKi;
                Bla.TransKi.Image = Properties.Resources.D2TransKi;
                Bla.TransKa.Image = Properties.Resources.D2TransKa;
                Bla.TransBack.Image = Properties.Resources.D2Back;
                Pil.Width = 40;
                Pil.Height = 40;
                Bla.LimAtas = 33;
                Bla.LimKiri = 40;
                Bla.KKTop = 5;
                Bla.KKLeft = 15;
                Bla.ABTop = 15;
                Bla.ABLeft = 5;
            }
            #endregion

            #region Domba Hitam
            else if (Kec == 4)
            {
                // domba hitam
                Bla.Tim.Interval = 600;
                Bla.Ki.Image = Properties.Resources.D1Ki;
                Bla.Ka.Image = Properties.Resources.D1Ka;
                Bla.JumpKa.Image = Properties.Resources.D1JumpKa;
                Bla.JumpKi.Image = Properties.Resources.D1JumpKi;
                Bla.TransKi.Image = Properties.Resources.D1TransKi;
                Bla.TransKa.Image = Properties.Resources.D1TransKa;
                Bla.TransBack.Image = Properties.Resources.D1Back;
                Pil.Width = 50;
                Pil.Height = 40;
                Bla.LimAtas = 40;
                Bla.LimKiri = 40;
                Bla.KKTop = 5;
                Bla.KKLeft = 10;
                Bla.ABTop = 15;
                Bla.ABLeft = 5;
            }
            #endregion

            #region Kambing
            else if (Kec == 5)
            {
                // kambing
                Bla.Tim.Interval = 1000;
                Bla.Ki.Image = Properties.Resources.D3Ki;
                Bla.Ka.Image = Properties.Resources.D3Ka;
                Bla.JumpKa.Image = Properties.Resources.D3JumpKa;
                Bla.JumpKi.Image = Properties.Resources.D3JumpKi;
                Bla.TransKi.Image = Properties.Resources.D3TransKi;
                Bla.TransKa.Image = Properties.Resources.D3TransKa;
                Bla.TransBack.Image = Properties.Resources.D3Back;
                Pil.Width = 40;
                Pil.Height = 50;
                Bla.KKTop = 5;
                Bla.KKLeft = 10;
                Bla.ABTop = 10;
                Bla.ABLeft = 5;
                Bla.LimAtas = 26;
                Bla.LimKiri = 40;
            }
            #endregion

            #region Kuda
            else if (Kec == 7)
            {
                // kuda
                Bla.Tim.Interval = 1400;
                Bla.Ki.Image = Properties.Resources.D4Ki;
                Bla.Ka.Image = Properties.Resources.D4Ka;
                Bla.JumpKa.Image = Properties.Resources.D4JumpKa;
                Bla.JumpKi.Image = Properties.Resources.D4JumpKi;
                Bla.TransKi.Image = Properties.Resources.D4TransKi;
                Bla.TransKa.Image = Properties.Resources.D4TransKa;
                Bla.TransBack.Image = Properties.Resources.D4Back;
                Pil.Width = 50;
                Pil.Height = 40;
                Bla.LimAtas = 35;
                Bla.LimKiri = 30;
                Bla.KKTop = 5;
                Bla.KKLeft = 10;
                Bla.ABTop = 10;
                Bla.ABLeft = 5;
            }
            #endregion

            #region Ayam Raksasa
            else if (Kec == 6)
            {
                // chocobo(kalo yang main FF) ato supupunya ayam lah wkwk
                Bla.Tim.Interval = 1200;
                Bla.Ki.Image = Properties.Resources.D5Ki;
                Bla.Ka.Image = Properties.Resources.D5Ka;
                Bla.JumpKa.Image = Properties.Resources.D5JumpKa;
                Bla.JumpKi.Image = Properties.Resources.D5JumpKi;
                Bla.TransKi.Image = Properties.Resources.D5TransKi;
                Bla.TransKa.Image = Properties.Resources.D5TransKa;
                Bla.TransBack.Image = Properties.Resources.D5Back;
                Pil.Width = 45;
                Pil.Height = 45;
                Bla.LimAtas = 30;
                Bla.LimKiri = 30;
                Bla.KKTop = 5;
                Bla.KKLeft = 10;
                Bla.ABTop = 10;
                Bla.ABLeft = 5;
            }
            #endregion
            Bla.ListPor = ListPort;
            if (ListPort.Count >= 1)
            {
                Bla.Portal = ListPort[0]; // untuk kalo eror geraknya dikembalikan ke awal
            }
            Bla.Lamb = Pil; // masukkan picbox dari form jadi hewannya
            Pil.Image = Bla.Ki.Image; // ubah gambarnya
            Bla.Speed = Kec; // taro kecepatannya
            Bla.OrSpeed = Bla.Speed;
            ListDomba.Add(Bla); // masukkan ke list
            Bla.CreateTimer();
        }

        public void Move(List<PictureBox> ListPort)
        {

            // mulai jalan tiap hewan
            for (int i = 0; i < ListDomba.Count; i++)
            {             
                // ini untuk transport hewan
                for (int j = 0; j < ListPort.Count; j++)
                {
                    // cek bisa atau tidak mentransfer
                    if (ListDomba[i].Lamb.Bounds.IntersectsWith(ListPort[j].Bounds) && ListPort[j].Visible == true && ListDomba[i].HitPort == true && ListPort[j].Visible == true)
                    {
                        ListDomba[i].HitPort = false; // ini supaya tidak looping forever di portal
                        ListDomba[i].Jalan = false; // ini supaya tidak jalan binatang ketika di portal

                        // ini supaya sama posisinya dengan portal
                        ListDomba[i].Lamb.Left = ListPort[j].Left;
                        ListDomba[i].Lamb.Top = ListPort[j].Top;

                        ListPort[j].Visible = false; // buat portalnya tidak kelihatan
                        ListDomba[i].Portal = ListPort[j]; // masukkan portal yang sama ke list hewan, karena indexnya portal susah diambil jadi dimasukkan saja ke list hewan

                        // ubah gambar ke transport
                        if (ListDomba[i].MajuKi == true || ListDomba[i].MajuAt == true)
                        {
                            ListDomba[i].Lamb.Image = ListDomba[i].TransKi.Image;
                            ListDomba[i].Mark = 5;
                        }
                        else if (ListDomba[i].MajuKa == true || ListDomba[i].MajuBa == true)
                        {
                            ListDomba[i].Lamb.Image = ListDomba[i].TransKa.Image;
                            ListDomba[i].Mark = 6;
                        }

                        //start timernya
                        ListDomba[i].Tim.Start();


                    }
                }

                // untuk jalan
                if (ListDomba[i].Jalan == true)
                {
                    ListDomba[i].Jalan = false; // ada alasannya... di bawah
                    for (int j = 0; j < ListPort.Count; j++)
                    {
                        // cek kalo dia kenna portal tapi hitport salah
                        if (ListDomba[i].HitPort == false && ListDomba[i].Lamb.Bounds.IntersectsWith(ListPort[j].Bounds))
                        {
                            // jalankan hewannya
                            GerakDomba(ref ListDomba[i].Mark, ListDomba[i].KKTop, ListDomba[i].KKLeft, ListDomba[i].ABTop, ListDomba[i].ABLeft, ListDomba[i].LimKiri, ListDomba[i].LimAtas,
                                ref ListDomba[i].HitKanan, ref ListDomba[i].HitKiri, ListDomba[i].Ka, ListDomba[i].Ki, ListDomba[i].JumpKa,
                                ListDomba[i].JumpKi, ListDomba[i].TransKa, ListDomba[i].TransKi, ref ListDomba[i].Speed, ref ListDomba[i].Counter,
                                ListDomba[i].Lamb, ListLabel, ref ListDomba[i].MajuKa, ref ListDomba[i].MajuKi, ref ListDomba[i].MajuBa,
                                ref ListDomba[i].MajuAt, ref ListDomba[i].BerKanan, ref ListDomba[i].BerKiri, ref ListDomba[i].BerAtas,
                                ref ListDomba[i].BerBawah, ref ListDomba[i].Mbelo, ref ListDomba[i].tandaKanan, ref ListDomba[i].tandaKiri,
                                ref ListDomba[i].tandabawa, ref ListDomba[i].tandaatas, ref ListDomba[i].kanan, ref ListDomba[i].bawa,
                                ref ListDomba[i].kiri, ref ListDomba[i].atas, ref ListDomba[i].kecilkanan, ref ListDomba[i].kecilbawa,
                                ref ListDomba[i].kecilkiri, ref ListDomba[i].kecilatas, ref ListDomba[i].MAtas, ref  ListDomba[i].MBawa,
                                ref ListDomba[i].MKiri, ref ListDomba[i].MKanan);
                            ListDomba[i].Jalan = true; // dibenarkan kembali

                            // di sini di cek kalo hewannya masih sentuh atau tidak portalnya kalo tidak hitportnya jadi benar 
                            if (ListDomba[i].Lamb.Bounds.IntersectsWith(ListPort[j].Bounds))
                            { }
                            else
                            {
                                ListDomba[i].HitPort = true;
                            }
                        }
                    }

                    // ini terjadi kalo di atas salah semua
                    if (ListDomba[i].Jalan == false)
                    {
                        // jalan yang di sini dengan yang di atas itu sama(Reaksi gerak domba)
                        GerakDomba(ref ListDomba[i].Mark, ListDomba[i].KKTop, ListDomba[i].KKLeft, ListDomba[i].ABTop, ListDomba[i].ABLeft, ListDomba[i].LimKiri,
                            ListDomba[i].LimAtas, ref ListDomba[i].HitKanan, ref ListDomba[i].HitKiri, ListDomba[i].Ka, ListDomba[i].Ki,
                            ListDomba[i].JumpKa, ListDomba[i].JumpKi, ListDomba[i].TransKa, ListDomba[i].TransKi, ref ListDomba[i].Speed,
                            ref ListDomba[i].Counter, ListDomba[i].Lamb, ListLabel, ref ListDomba[i].MajuKa, ref ListDomba[i].MajuKi,
                            ref ListDomba[i].MajuBa, ref ListDomba[i].MajuAt, ref ListDomba[i].BerKanan, ref ListDomba[i].BerKiri,
                            ref ListDomba[i].BerAtas, ref ListDomba[i].BerBawah, ref ListDomba[i].Mbelo, ref ListDomba[i].tandaKanan,
                            ref ListDomba[i].tandaKiri, ref ListDomba[i].tandabawa, ref ListDomba[i].tandaatas, ref ListDomba[i].kanan,
                            ref ListDomba[i].bawa, ref ListDomba[i].kiri, ref ListDomba[i].atas, ref ListDomba[i].kecilkanan, ref ListDomba[i].kecilbawa,
                            ref ListDomba[i].kecilkiri, ref ListDomba[i].kecilatas, ref ListDomba[i].MAtas, ref  ListDomba[i].MBawa,
                            ref ListDomba[i].MKiri, ref ListDomba[i].MKanan);
                    }
                    ListDomba[i].Jalan = true; // di benarkan kembali

                    //for (int k = 0; k < Player.Count; k++)
                    //{
                    //    if (Player[k].Bounds.IntersectsWith(ListDomba[i].Lamb.Bounds))
                    //    {
                    //        ListDomba[i].Lamb.Visible = false;
                    //        //ListDomba.RemoveAt(i);
                    //        break;
                    //    }
                    //}
                }
            }
        }

        //Method DI bAwah uintuk yang tdk ada playernya(Bergerak Bebas)
        public void GerakDomba(ref int ActMark, int KKTop,int KKLeft, int ABTop,int ABLeft, int LimKiri, int LimAtas,ref int HitKanan, ref int HitKiri,
            PictureBox Ka, PictureBox Ki, PictureBox JumpKa, PictureBox JumpKi, PictureBox TransKa, PictureBox TransKi, ref int Spd, ref int Count, 
            PictureBox Player, List<Label> ListLabel, ref bool MajuKa, ref bool MajuKi, ref bool MajuBa, ref bool MajuAt, ref bool BerKanan, 
            ref bool BerKiri, ref bool BerAtas, ref bool BerBawah, ref bool Mark, ref int tandaKanan, ref int tandaKiri, ref int tandabawa, 
            ref int tandaatas, ref int kanan, ref int bawa, ref int kiri, ref int atas, ref int kecilkanan, ref int kecilbawa, ref int kecilkiri, 
            ref int kecilatas, ref bool MAtas, ref bool MBawa, ref bool MKiri, ref bool MKanan)
        {
            // Cek label mana yang paling dekat dengan hewan, baru simpan indexnya
            #region cek terdekat
            for (int i = 0; i < ListLabel.Count; i++)
            {
                if (kecilkanan > (ListLabel[i].Left) - Player.Right && (ListLabel[i].Left) - Player.Right >= 0 && (ListLabel[i].Top) < Player.Bottom && (ListLabel[i].Bottom) > Player.Top && ListLabel[i].Visible == true)
                {
                    kecilkanan = (ListLabel[i].Left) - Player.Right;
                    tandaKanan = i;
                }
                if (kecilbawa > (ListLabel[i].Top) - Player.Bottom && (ListLabel[i].Top) - Player.Bottom >= 0 && (ListLabel[i].Left) < Player.Right && (ListLabel[i].Right) > Player.Left && ListLabel[i].Visible == true)
                {
                    kecilbawa = (ListLabel[i].Top) - Player.Bottom;
                    tandabawa = i;
                }
                if (kecilkiri > Player.Left - (ListLabel[i].Right) && Player.Left - (ListLabel[i].Right) >= 0 && (ListLabel[i].Top) < Player.Bottom && (ListLabel[i].Bottom) > Player.Top && ListLabel[i].Visible == true)
                {
                    kecilkiri = Player.Left - (ListLabel[i].Right);
                    tandaKiri = i;
                }
                if (kecilatas > Player.Top - (ListLabel[i].Bottom) && Player.Top - (ListLabel[i].Bottom) >= 0 && (ListLabel[i].Left) < Player.Right && (ListLabel[i].Right) > Player.Left && ListLabel[i].Visible == true)
                {
                    kecilatas = Player.Top - (ListLabel[i].Bottom);
                    tandaatas = i;
                }
            }
            #endregion

            // Cek apakah bisa belok kiri kanan atas ato bawah
            #region cek belok
            if (kecilkiri >= LimKiri)
            {
                MKiri = true;
            }
            else if (kecilkiri < LimKiri)
            {
                MKiri = false;
            }

            if (kecilkanan >= LimKiri)
            {
                MKanan = true;
            }
            else if (kecilkanan < LimKiri)
            {
                MKanan = false;
            }

            if (kecilbawa >= LimAtas)
            {
                MBawa = true;
            }
            else if (kecilbawa < LimAtas)
            {
                MBawa = false;
            }

            if (kecilatas >= LimAtas)
            {
                MAtas = true;
            }
            else if (kecilatas < LimAtas)
            {
                MAtas = false;
            }
            #endregion

            // Pengaturan beloknya
            #region Belokkkkk
            // ini kalo pertama kali jalan jadi pemilihannya acak
            if (MajuKa == false && MajuKi == false && MajuBa == false && MajuAt == false)
            {
                Random Pilih = new Random();
                int Acak = Pilih.Next(1, 5);
                PemilihanAcak(Acak, ref MajuKa, ref MajuKi, ref MajuBa, ref MajuAt, ref MAtas, ref  MBawa, ref MKiri, ref MKanan);
                //MajuKi = true;
                
                // masukkan nilai di int ataskanan dkk
                atas = tandaatas;
                bawa = tandabawa;
                kiri = tandaKiri;
                kanan = tandaKanan;
                Player.Image = Ki.Image;
                ActMark = 1;
            }

            // ini kalo sudah jalan
            if (MajuKa == true || MajuKi == true || MajuBa == true || MajuAt == true)
            {
                // ini untuk pilih acak 
                Random Pilih = new Random();
                int Acak = Pilih.Next(1, 4); // pemilihan perempatan
                int Acak2 = Pilih.Next(1, 3); // pemilihan pertigaan

                // cek kalo sudah bisa belok ato tdk, kalo bisa: "Mark" true
                CekBelok(ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas, ref Mark);
                
                // kalo jalan dari arah atas
                if (MajuAt == true && Mark == true)
                {
                    // di bawah ini semuanya kemungkinan yang ada kalo bergerak ke atas lalu ada perbelokan
                    // ini kalo perbelokan kiri
                    if (MKiri == true && MAtas == false && MKanan == false)
                    {
                        // arahnya lihat di bawah                              sini!!(Maksunya betulan bawahnya pas) ada kata kiri artinya arahnya itu
                        BerBalik(ref Count, Spd, Player, -ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                        // hanya perbelokan kanan
                    else if (MKanan == true && MAtas == false && MKiri == false)
                    {
                        BerBalik(ref Count, Spd, Player, -ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                        // pertigaan antara depan dan kiri
                    else if (MKiri == true && MAtas == true && MKanan == false)
                    {
                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            // kalo begini artinya maju tetap
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false; // di falsekan supaya tidak belok lagi
                         
                            // di kasi samakan supaya marknya tdk benar(lihat method "cek belok")
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                        // pertigaan depan kanan
                    else if (MKanan == true && MAtas == true && MKiri == false)
                    {
                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 1)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                            BerKiri = true;
                        }
                    }
                        // pertigaan kiri dan kanan
                    else if (MKanan == true && MAtas == false && MKiri == true)
                    {
                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }

                    }
                        // perempatan
                    else if (MKanan == true && MAtas == true && MKiri == true)
                    {
                        if (Acak == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 2)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 3)
                        {
                            // sama di atas
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                        // ini kalo jalan buntu
                    else if (MKiri == false && MAtas == false && MKanan == false)
                    {
                        Count++; // ini supaya tidak langsung putar harus lebih dekat dulu
                        if (Count > LimAtas / 8) // ada sampai nilai tertentu supaya bisa belok kembali
                        {
                            MajuBa = true;
                            MajuAt = false;
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = true;
                            Count = 0;
                        }
                    }
                }

                    // sama dengan di atas
                else if (MajuBa == true && Mark == true)
                {
                    if (MKiri == true && MBawa == false && MKanan == false)
                    {
                        BerBalik(ref Count, Spd, Player, +ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);

                    }
                    else if (MKanan == true && MBawa == false && MKiri == false)
                    {
                        BerBalik(ref Count, Spd, Player, +ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    else if (MKiri == true && MBawa == true && MKanan == false)
                    {
                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                        }
                    }
                    else if (MKanan == true && MBawa == true && MKiri == false)
                    {
                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 1)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    else if (MKanan == true && MBawa == false && MKiri == true)
                    {
                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }

                    }
                    else if (MKanan == true && MBawa == true && MKiri == true)
                    {
                        if (Acak == 1)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 3)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }

                    else if (MKiri == false && MBawa == false && MKanan == false)
                    {
                        Count++;
                        if (Count > LimAtas / 8)
                        {
                            MajuBa = false;
                            MajuAt = true; ;
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = true;
                            Count = 0;
                        }
                    }
                }
                // sama dengan di atas
                else if (MajuKa == true && Mark == true)
                {
                    if (MAtas == true && MKanan == false && MBawa == false)
                    {
                        BerBalik(ref Count, Spd, Player, +KKTop, +KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);

                    }
                    else if (MBawa == true && MKanan == false && MAtas == false)
                    {
                        BerBalik(ref Count, Spd, Player, -KKTop, +KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    else if (MBawa == true && MKanan == true && MAtas == false)
                    {
                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -KKTop, +KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    else if (MBawa == false && MKanan == true && MAtas == true)
                    {
                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +KKTop, +KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 1)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    else if (MBawa == true && MKanan == false && MAtas == true)
                    {
                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -KKTop, +KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            BerAtas = true;
                        }
                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +KKTop, +KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                    }
                    else if (MBawa == true && MKanan == true && MAtas == true)
                    {
                        if (Acak == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -KKTop, +KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +KKTop, +KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 3)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }

                    else if (MBawa == false && MKanan == false && MAtas == false)
                    {
                        Count++;
                        if (Count > LimKiri / 8)
                        {
                            MajuKa = false;
                            MajuKi = true;
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = true;
                            Count = 0;
                        }
                    }
                }
                // sama dengan di atas
                else if (MajuKi == true)
                {
                    if (Mark == true)
                    {
                        if (MBawa == true && MKiri == false && MAtas == false)
                        {
                            BerBalik(ref Count, Spd, Player, -KKTop, -KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        else if (MAtas == true && MKiri == false && MBawa == false)
                        {
                            BerBalik(ref Count, Spd, Player, +KKTop, -KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        else if (MBawa == true && MKiri == true && MAtas == false)
                        {
                            if (Acak2 == 1)
                            {
                                BerBalik(ref Count, Spd, Player, -KKTop, -KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak2 == 2)
                            {
                                BerBawah = true;
                                BerAtas = true;
                                BerKanan = true;
                                BerKiri = true;
                                Mark = false;
                                kanan = tandaKanan;
                                atas = tandaatas;
                                bawa = tandabawa;
                                kiri = tandaKiri;
                            }
                        }
                        else if (MBawa == false && MKiri == true && MAtas == true)
                        {
                            if (Acak2 == 2)
                            {
                                BerBalik(ref Count, Spd, Player, +KKTop, -KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak2 == 1)
                            {
                                BerBawah = true;
                                BerAtas = true;
                                BerKanan = true;
                                BerKiri = true;
                                Mark = false;
                                kanan = tandaKanan;
                                atas = tandaatas;
                                bawa = tandabawa;
                                kiri = tandaKiri;
                                
                            }
                        }
                        else if (MBawa == true && MKiri == false && MAtas == true)
                        {
                            if (Acak2 == 1)
                            {
                                BerBalik(ref Count, Spd, Player, -KKTop, -KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak2 == 2)
                            {
                                BerBalik(ref Count, Spd, Player, +KKTop, -KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                        }
                        else if (MBawa == true && MKiri == true && MAtas == true)
                        {
                            if (Acak == 1)
                            {
                                BerBalik(ref Count, Spd, Player, -KKTop, -KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak == 2)
                            {
                                BerBalik(ref Count, Spd, Player, +KKTop, -KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak == 3)
                            {
                                BerBawah = true;
                                BerAtas = true;
                                BerKanan = true;
                                BerKiri = true;
                                Mark = false;
                                kanan = tandaKanan;
                                atas = tandaatas;
                                bawa = tandabawa;
                                kiri = tandaKiri;
                            }

                        }
                        else if (MBawa == false && MKiri == false && MAtas == false)
                        {
                            Count++;
                            if (Count > LimKiri / 8)
                            {
                                MajuKi = false;
                                MajuKa = true;
                                BerBawah = true;
                                BerAtas = true;
                                BerKanan = true;
                                BerKiri = true;
                                Mark = true;
                                Count = 0;
                            }
                        }
                    }
                }
            }
            if (MajuAt == true)
            {
                Player.Top -= Spd; // kecepatannya
                // kembalikan selisihnya seperti semula
                kecilbawa = asd;
                kecilkanan = asd;
                kecilkiri = asd;
            }
            else if (MajuBa == true)
            {
                Player.Top += Spd;
                kecilatas = asd;
                kecilkanan = asd;
                kecilkiri = asd;
            }
            else if (MajuKi == true)
            {
                Player.Left -= Spd;
                kecilbawa = asd;
                kecilkanan = asd;
                kecilatas = asd;

                // ini supaya animasi arah jalannya benar
                if (HitKiri == 0)
                {
                    HitKiri++;
                    HitKanan = 0;
                    Player.Image = Ki.Image;
                    ActMark = 1;
                }

                // ini kalo kenna label supaya kelihatan loncat
                for (int i = 0; i < ListLabel.Count; i++)
                {
                    if (Player.Bounds.IntersectsWith(ListLabel[i].Bounds))
                    {
                        Player.Image = JumpKi.Image;
                        ActMark = 3;
                        HitKiri = 0;
                    }
                }
            }
            else if (MajuKa == true)
            {
                Player.Left += Spd;
                kecilbawa = asd;
                kecilatas = asd;
                kecilkiri = asd;
                if (HitKanan == 0)
                {
                    HitKanan++;
                    HitKiri = 0;
                    Player.Image = Ka.Image;
                    ActMark = 2;
                }
                for (int i = 0; i < ListLabel.Count; i++)
                {
                    if (Player.Bounds.IntersectsWith(ListLabel[i].Bounds))
                    {
                        Player.Image = JumpKa.Image;
                        HitKanan = 0;
                        ActMark = 4;
                    }
                }
            }
            #endregion
        }


        // Method Di Bawah untuk yang ada 1 player
        public void Move(List<PictureBox> ListPort, PictureBox Play)
        {
            for (int i = 0; i < ListDomba.Count; i++)
            {

                if (Play.Bounds.IntersectsWith(ListDomba[i].Lamb.Bounds))
                {
                    ListDomba[i].Lamb.Visible = false;
                }
                // ini untuk transport hewan
                for (int j = 0; j < ListPort.Count; j++)
                {
                    // cek bisa atau tidak mentransfer
                    if (ListDomba[i].Lamb.Bounds.IntersectsWith(ListPort[j].Bounds) && ListPort[j].Visible == true && ListDomba[i].HitPort == true && ListPort[j].Visible == true)
                    {
                        ListDomba[i].HitPort = false; // ini supaya tidak looping forever di portal
                        ListDomba[i].Jalan = false; // ini supaya tidak jalan binatang ketika di portal

                        // ini supaya sama posisinya dengan portal
                        ListDomba[i].Lamb.Left = ListPort[j].Left;
                        ListDomba[i].Lamb.Top = ListPort[j].Top;

                        ListPort[j].Visible = false; // buat portalnya tidak kelihatan
                        ListDomba[i].Portal = ListPort[j]; // masukkan portal yang sama ke list hewan, karena indexnya portal susah diambil jadi dimasukkan saja ke list hewan

                        // ubah gambar ke transport
                        if (ListDomba[i].MajuKi == true || ListDomba[i].MajuAt == true)
                        {
                            ListDomba[i].Lamb.Image = ListDomba[i].TransKi.Image;
                        }
                        else if (ListDomba[i].MajuKa == true || ListDomba[i].MajuBa == true)
                        {
                            ListDomba[i].Lamb.Image = ListDomba[i].TransKa.Image;
                        }

                        //start timernya
                        ListDomba[i].Tim.Start();
                    }
                }

                // untuk jalan
                if (ListDomba[i].Jalan == true)
                {
                    ListDomba[i].Jalan = false; // ada alasannya... di bawah
                    for (int j = 0; j < ListPort.Count; j++)
                    {
                        // cek kalo dia kenna portal tapi hitport salah
                        if (ListDomba[i].HitPort == false && ListDomba[i].Lamb.Bounds.IntersectsWith(ListPort[j].Bounds))
                        {
                            // jalankan hewannya
                            GerakDomba(ref ListDomba[i].HitPort, Play,
                            ListDomba[i].KKTop, ListDomba[i].KKLeft, ListDomba[i].ABTop, ListDomba[i].ABLeft, ListDomba[i].LimKiri, ListDomba[i].LimAtas, ref ListDomba[i].HitKanan, ref ListDomba[i].HitKiri, ListDomba[i].Ka, ListDomba[i].Ki, ListDomba[i].JumpKa, ListDomba[i].JumpKi, ListDomba[i].TransKa, ListDomba[i].TransKi,
                            ref ListDomba[i].Speed, ref ListDomba[i].Counter, ListDomba[i].Lamb, ref ListDomba[i].MajuKa, ref ListDomba[i].MajuKi, ref ListDomba[i].MajuBa, ref ListDomba[i].MajuAt, ref ListDomba[i].BerKanan, ref ListDomba[i].BerKiri, ref ListDomba[i].BerAtas, ref ListDomba[i].BerBawah,
                            ref ListDomba[i].Mbelo, ref ListDomba[i].tandaKanan, ref ListDomba[i].tandaKiri, ref ListDomba[i].tandabawa, ref ListDomba[i].tandaatas, ref ListDomba[i].kanan, ref ListDomba[i].bawa, ref ListDomba[i].kiri, ref ListDomba[i].atas, ref ListDomba[i].kecilkanan, ref ListDomba[i].kecilbawa, ref ListDomba[i].kecilkiri,
                            ref ListDomba[i].kecilatas, ref ListDomba[i].MAtas, ref  ListDomba[i].MBawa, ref ListDomba[i].MKiri, ref ListDomba[i].MKanan);
                            ListDomba[i].Jalan = true; // dibenarkan kembali

                            // di sini di cek kalo hewannya masih sentuh atau tidak portalnya kalo tidak hitportnya jadi benar 
                            if (ListDomba[i].Lamb.Bounds.IntersectsWith(ListPort[j].Bounds))
                            { }
                            else
                            {
                                ListDomba[i].HitPort = true;
                            }
                        }
                    }

                    // ini terjadi kalo di atas salah semua
                    if (ListDomba[i].Jalan == false)
                    {
                        // jalan yang di sini dengan yang di atas itu sama(Reaksi gerak domba)
                        GerakDomba(ref ListDomba[i].HitPort, Play,
                        ListDomba[i].KKTop, ListDomba[i].KKLeft, ListDomba[i].ABTop, ListDomba[i].ABLeft, ListDomba[i].LimKiri, ListDomba[i].LimAtas, ref ListDomba[i].HitKanan, ref ListDomba[i].HitKiri, ListDomba[i].Ka, ListDomba[i].Ki, ListDomba[i].JumpKa, ListDomba[i].JumpKi, ListDomba[i].TransKa, ListDomba[i].TransKi,
                        ref ListDomba[i].Speed, ref ListDomba[i].Counter, ListDomba[i].Lamb, ref ListDomba[i].MajuKa, ref ListDomba[i].MajuKi, ref ListDomba[i].MajuBa, ref ListDomba[i].MajuAt, ref ListDomba[i].BerKanan, ref ListDomba[i].BerKiri, ref ListDomba[i].BerAtas, ref ListDomba[i].BerBawah,
                        ref ListDomba[i].Mbelo, ref ListDomba[i].tandaKanan, ref ListDomba[i].tandaKiri, ref ListDomba[i].tandabawa, ref ListDomba[i].tandaatas, ref ListDomba[i].kanan, ref ListDomba[i].bawa, ref ListDomba[i].kiri, ref ListDomba[i].atas, ref ListDomba[i].kecilkanan, ref ListDomba[i].kecilbawa, ref ListDomba[i].kecilkiri,
                        ref ListDomba[i].kecilatas, ref ListDomba[i].MAtas, ref  ListDomba[i].MBawa, ref ListDomba[i].MKiri, ref ListDomba[i].MKanan);
                    }
                    ListDomba[i].Jalan = true; // di benarkan kembali
                }
            }
        }

        public void GerakDomba(ref bool Hitport, PictureBox Enemy,
        int KKTop, int KKLeft, int ABTop, int ABLeft, int LimKiri, int LimAtas, ref int HitKanan, ref int HitKiri, PictureBox Ka, PictureBox Ki, PictureBox JumpKa,
        PictureBox JumpKi, PictureBox TransKa, PictureBox TransKi, ref int Spd, ref int Count, PictureBox Player, ref bool MajuKa, ref bool MajuKi, ref bool MajuBa, ref bool MajuAt,
         ref bool BerKanan, ref bool BerKiri, ref bool BerAtas, ref bool BerBawah, ref bool Mark, ref int tandaKanan, ref int tandaKiri, ref int tandabawa, ref int tandaatas, ref int kanan, ref int bawa, ref int kiri, ref int atas,
        ref int kecilkanan, ref int kecilbawa, ref int kecilkiri, ref int kecilatas, ref bool MAtas, ref bool MBawa, ref bool MKiri, ref bool MKanan)
        {
            // gerak di lorong

            // Cek label mana yang paling dekat dengan hewan, baru simpan indexnya
            #region cek terdekat
            for (int i = 0; i < ListLabel.Count; i++)
            {
                if (kecilkanan > (ListLabel[i].Left) - Player.Right && (ListLabel[i].Left) - Player.Right >= 0 && (ListLabel[i].Top) < Player.Bottom && (ListLabel[i].Bottom) > Player.Top && ListLabel[i].Visible == true)
                {
                    kecilkanan = (ListLabel[i].Left) - Player.Right;
                    tandaKanan = i;
                }
                if (kecilbawa > (ListLabel[i].Top) - Player.Bottom && (ListLabel[i].Top) - Player.Bottom >= 0 && (ListLabel[i].Left) < Player.Right && (ListLabel[i].Right) > Player.Left && ListLabel[i].Visible == true)
                {
                    kecilbawa = (ListLabel[i].Top) - Player.Bottom;
                    tandabawa = i;
                }
                if (kecilkiri > Player.Left - (ListLabel[i].Right) && Player.Left - (ListLabel[i].Right) >= 0 && (ListLabel[i].Top) < Player.Bottom && (ListLabel[i].Bottom) > Player.Top && ListLabel[i].Visible == true)
                {
                    kecilkiri = Player.Left - (ListLabel[i].Right);
                    tandaKiri = i;
                }
                if (kecilatas > Player.Top - (ListLabel[i].Bottom) && Player.Top - (ListLabel[i].Bottom) >= 0 && (ListLabel[i].Left) < Player.Right && (ListLabel[i].Right) > Player.Left && ListLabel[i].Visible == true)
                {
                    kecilatas = Player.Top - (ListLabel[i].Bottom);
                    tandaatas = i;
                }
            }
            #endregion

            // Cek apakah bisa belok kiri kanan atas ato bawah
            #region cek belok
            if (kecilkiri >= LimKiri)
            {
                MKiri = true;
            }
            else if (kecilkiri < LimKiri)
            {
                MKiri = false;
            }

            if (kecilkanan >= LimKiri)
            {
                MKanan = true;
            }
            else if (kecilkanan < LimKiri)
            {
                MKanan = false;
            }

            if (kecilbawa >= LimAtas)
            {
                MBawa = true;
            }
            else if (kecilbawa < LimAtas)
            {
                MBawa = false;
            }

            if (kecilatas >= LimAtas)
            {
                MAtas = true;
            }
            else if (kecilatas < LimAtas)
            {
                MAtas = false;
            }
            #endregion

            // Pengaturan beloknya
            #region Belokkkkk
            // ini kalo pertama kali jalan jadi pemilihannya acak
            if (MajuKa == false && MajuKi == false && MajuBa == false && MajuAt == false)
            {
                Random Pilih = new Random();
                int Acak = Pilih.Next(1, 5);
                PemilihanAcak(Acak, ref MajuKa, ref MajuKi, ref MajuBa, ref MajuAt, ref MAtas, ref  MBawa, ref MKiri, ref MKanan);
                //MajuKi = true;

                // masukkan nilai di int ataskanan dkk
                atas = tandaatas;
                bawa = tandabawa;
                kiri = tandaKiri;
                kanan = tandaKanan;
                Player.Image = Ki.Image;
            }

            // ini kalo sudah jalan
            if (MajuKa == true || MajuKi == true || MajuBa == true || MajuAt == true)
            {
                // di sini hewan namanya palyer, lalu player yang asli yang kita kontrol namanya enemy(karna bagi hewan kita enemy wkwkwk)

                // ini untuk pilih acak 
                Random Pilih = new Random();
                int Acak = Pilih.Next(1, 4); // pemilihan perempatan
                int Acak2 = Pilih.Next(1, 3); // pemilihan pertigaan
                int SesihKiri = Enemy.Left - Player.Left; // maksudnya selisih kirinya playernyda hewan
                int SesihAtas = Enemy.Top - Player.Top; // sama sesih atas tapi ini atas

                CekBelok(ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas, ref Mark);


                // ini buat ke positif
                if (SesihKiri < 0)
                {
                    SesihKiri *= -1;
                }
                if (SesihAtas < 0)
                {
                    SesihAtas *= -1;
                }

                // ini untuk tau posisi tengah player
                int BatKi = Player.Left + Player.Width / 2;
                int BatAt = Player.Top + Player.Height / 2;

                #region Atas

                // prinsipnya atas dengan arah lainnya itu sama jadi hanya dijelaskan yang atas saja, kalo bawah semua berkebalikan dengan atas
                // kalo kiri kanan juga sama dengan atas bawah, jadi anggak top jadi left, bottom jadi right, sisa diubah begitu jadi untuk kiri dan kanan

                // kalo jalan dari arah atas
                if (MajuAt == true && Mark == true)
                {
                    if (Easy == true || Medium == true || Hard == true)
                    {
                        if (Medium == true)
                        {
                            // maksudnya    seperti hewan berhadapan persis dgn enemy,    ini maksudnya di atasnya hewan tidak ada label,       ini maksudnya di atas hewannya ada enemy
                            if (Enemy.Left < Player.Right && Enemy.Right > Player.Left && ListLabel[tandaatas].Bottom < Enemy.Bottom && Enemy.Bottom < Player.Top)
                            {
                                //maksudnya   batasnya sampai hewan belok memutar kalo enemy masih di bawahlabel di kiri kanannya           atau selisih dekatnya 200
                                if ((ListLabel[tandaKiri].Top - 70 < Enemy.Bottom && ListLabel[tandaKanan].Top - 70 < Enemy.Bottom) || Player.Bottom - Enemy.Top <= 200)
                                {
                                    MKiri = false;
                                    MKanan = false;
                                    MAtas = false;
                                    Hitport = true; // ini ketika dia berhadapan dengan transport
                                }
                            }
                        }

                        if (Hard == true)
                        {
                            // ini mirip medium tapi ruang lingkupnya diperluas, yang berubah cuma di bawah ini saja:   Maksudnya radius berbaliknya bukan kalo hanya pas di depannya, yang ini batas kirinya dan kanannya sampe 200
                            if (((Enemy.Left < Player.Right && Player.Right - Enemy.Left <= 200) || (Enemy.Right > Player.Left && Enemy.Right - Player.Left < 200)) && ListLabel[tandaatas].Bottom < Enemy.Bottom && Enemy.Bottom < Player.Top)
                            {
                                if ((ListLabel[tandaKiri].Top - 70 < Enemy.Bottom && ListLabel[tandaKanan].Top - 70 < Enemy.Bottom) || Player.Bottom - Enemy.Top <= 300)
                                {
                                    MKiri = false;
                                    MKanan = false;
                                    MAtas = false;
                                    Hitport = true;
                                }
                            }
                        }
                    }
                    // di bawah ini semuanya kemungkinan yang ada kalo bergerak ke atas lalu ada perbelokan
                    // ini kalo perbelokan kiri
                    if (MKiri == true && MAtas == false && MKanan == false)
                    {
                        // arahnya lihat di bawah                              sini!!(Maksunya betulan bawahnya pas) ada kata kiri artinya arahnya itu
                        BerBalik(ref Count, Spd, Player, -ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    // hanya perbelokan kanan
                    else if (MKanan == true && MAtas == false && MKiri == false)
                    {
                        BerBalik(ref Count, Spd, Player, -ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    // pertigaan antara depan dan kiri
                    else if (MKiri == true && MAtas == true && MKanan == false)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Top + 100 > Enemy.Bottom)  // maksudnya radiusnya sampe 100 pixel ke bawah dari hewan
                            {
                                // radiusnya sampe 200 ke kiri dan 200 ke atas dgn hewan masih lebih ke kanan
                                if (BatKi - 200 < Enemy.Right && Player.Top - 200 < Enemy.Bottom && Player.Left > Enemy.Right)
                                {
                                    // ini kalo di sampingnya persis
                                    if (Player.Bottom > Enemy.Top && Player.Top < Enemy.Bottom)
                                    {
                                        Acak2 = 2;
                                    }
                                    // ini di atasnya persis
                                    else if (Player.Left < Enemy.Right && Player.Right > Enemy.Left)
                                    {
                                        Acak2 = 1;
                                    }
                                    // ini kalo selisih atas lebih kecil di ke atas
                                    else if (SesihKiri > SesihAtas)
                                    {
                                        Acak2 = 2;
                                    }
                                    // ini kalo selisih kiri lebih kecil jadi ke kiri
                                    else if (SesihKiri < SesihAtas)
                                    {
                                        Acak2 = 1;
                                    }

                                }
                                // ini kalo enemy di kanannya hewan
                                else if (Enemy.Right > Player.Left)
                                {
                                    Acak2 = 1;
                                }
                            }
                        }

                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            // kalo begini artinya maju tetap
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false; // di falsekan supaya tidak belok lagi

                            // di kasi samakan supaya marknya tdk benar(lihat method "cek belok")
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    // pertigaan depan kanan
                    else if (MKanan == true && MAtas == true && MKiri == false)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            // ini mirip dengan di atas
                            if (Player.Top + 100 > Enemy.Bottom)
                            {
                                if (BatKi + 200 > Enemy.Left && Player.Top - 200 < Enemy.Bottom && Player.Right < Enemy.Right)
                                {
                                    if (Player.Bottom > Enemy.Top && Player.Top < Enemy.Bottom)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (Player.Left < Enemy.Right && Player.Right > Enemy.Left)
                                    {
                                        Acak2 = 2;
                                    }
                                    else if (SesihKiri > SesihAtas)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (SesihKiri < SesihAtas)
                                    {
                                        Acak2 = 2;
                                    }

                                }
                                else if (Enemy.Right < Player.Left)
                                {
                                    Acak2 = 2;
                                }
                            }
                        }

                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 1)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                            BerKiri = true;
                        }
                    }
                    // pertigaan kiri dan kanan
                    else if (MKanan == true && MAtas == false && MKiri == true)
                    {
                        // maksudnya hewan ke kanan kalo playernya lebih kiri dari hewannya dan sebaliknya
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Top + 100 > Enemy.Bottom)
                            {
                                if (BatKi > Enemy.Right)
                                {
                                    Acak2 = 2;
                                }
                                else if (BatKi < Enemy.Left)
                                {
                                    Acak2 = 1;
                                }
                            }
                        }

                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }

                    }
                    // perempatan
                    else if (MKanan == true && MAtas == true && MKiri == true)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Top + 100 > Enemy.Bottom)
                            {
                                // radius kalo musuh sampe dalam 200 pixel di samping kiri hewan, harus p.lef>en.rig(bawa) karna suppaya harus di kirinya
                                if (BatKi - 200 < Enemy.Right && Player.Left > Enemy.Right)
                                {
                                    Acak = Acak2 + 1; // maksudnya bukan 1
                                }
                                // ini sama dengan di atas syaratnya
                                else if (BatKi + 200 > Enemy.Left && Enemy.Left > Player.Right)
                                {
                                    Acak = Acak2; // maksudnya tidak boleh 2
                                    if (Acak == 2)
                                    {
                                        Acak++;
                                    }
                                }
                                else
                                {
                                    Acak = Acak2; // ini kalo bukan gerak depan
                                }
                            }
                        }

                        if (Acak == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 2)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 3)
                        {
                            // sama di atas
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    // ini kalo jalan buntu
                    else if (MKiri == false && MAtas == false && MKanan == false)
                    {
                        Count++; // ini supaya tidak langsung putar harus lebih dekat dulu
                        if (Count > LimAtas / 8) // ada sampai nilai tertentu supaya bisa belok kembali
                        {
                            MajuBa = true;
                            MajuAt = false;
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = true;
                            Count = 0;
                        }
                    }
                }
                #endregion

                #region Bawah

                // sama dengan di atas(Prinsipnnya mirip atas)
                else if (MajuBa == true && Mark == true)
                {
                    if (Easy == true || Medium == true || Hard == true)
                    {
                        if (Medium == true)
                        {
                            if (Enemy.Left < Player.Right && Enemy.Right > Player.Left && ListLabel[tandabawa].Top > Enemy.Bottom && Enemy.Top > Player.Bottom)
                            {
                                if ((ListLabel[tandaKiri].Bottom + 70 > Enemy.Top && ListLabel[tandaKanan].Bottom + 70 > Enemy.Top) || Enemy.Bottom - Player.Top <= 200)
                                {
                                    MKiri = false;
                                    MKanan = false;
                                    MBawa = false;
                                    Hitport = true;
                                }
                            }
                        }

                        if (Hard == true)
                        {
                            if (((Enemy.Left < Player.Right && Player.Right - Enemy.Left <= 200) || (Enemy.Right > Player.Left && Player.Left - Enemy.Right <= 200)) && ListLabel[tandabawa].Top > Enemy.Bottom && Enemy.Top > Player.Bottom)
                            {
                                if ((ListLabel[tandaKiri].Bottom + 70 > Enemy.Top && ListLabel[tandaKanan].Bottom + 70 > Enemy.Top) || Enemy.Bottom - Player.Top <= 300)
                                {
                                    MKiri = false;
                                    MKanan = false;
                                    MBawa = false;
                                    Hitport = true;
                                }
                            }
                        }
                    }

                    if (MKiri == true && MBawa == false && MKanan == false)
                    {
                        BerBalik(ref Count, Spd, Player, +ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    else if (MKanan == true && MBawa == false && MKiri == false)
                    {
                        BerBalik(ref Count, Spd, Player, +ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    else if (MKiri == true && MBawa == true && MKanan == false)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Bottom - 100 < Enemy.Bottom)
                            {
                                if (BatKi - 200 < Enemy.Right && Player.Bottom + 200 > Enemy.Top && Player.Left > Enemy.Right)
                                {
                                    if (Player.Bottom > Enemy.Top && Player.Top < Enemy.Bottom)
                                    {
                                        Acak2 = 2;
                                    }
                                    else if (Player.Left < Enemy.Right && Player.Right > Enemy.Left)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (SesihKiri > SesihAtas)
                                    {
                                        Acak2 = 2;
                                    }
                                    else if (SesihKiri < SesihAtas)
                                    {
                                        Acak2 = 1;
                                    }
                                }
                                else if (Enemy.Right > Player.Left)
                                {
                                    Acak2 = 1;
                                }
                            }
                        }

                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    else if (MKanan == true && MBawa == true && MKiri == false)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Bottom - 100 < Enemy.Top)
                            {
                                if (BatAt + 200 > Enemy.Left && Player.Bottom + 200 > Enemy.Top && Player.Right < Enemy.Right)
                                {
                                    if (Player.Bottom > Enemy.Top && Player.Top < Enemy.Bottom)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (Player.Left < Enemy.Right && Player.Right > Enemy.Left)
                                    {
                                        Acak2 = 2;
                                    }
                                    else if (SesihKiri > SesihAtas)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (SesihKiri < SesihAtas)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                                else if (Enemy.Right < Player.Left)
                                {
                                    Acak2 = 2;
                                }
                            }
                        }

                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 1)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    else if (MKanan == true && MBawa == false && MKiri == true)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Bottom - 100 < Enemy.Bottom)
                            {
                                if (BatKi > Enemy.Right)
                                {
                                    Acak2 = 2;
                                }
                                else if (BatKi < Enemy.Left)
                                {
                                    Acak2 = 1;
                                }
                            }
                        }

                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }

                    }
                    else if (MKanan == true && MBawa == true && MKiri == true)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Bottom - 100 < Enemy.Bottom)
                            {
                                if (BatKi - 200 < Enemy.Right && Player.Left > Enemy.Right)
                                {
                                    Acak = Acak2 + 1;
                                }
                                else if (BatKi + 200 > Enemy.Left && Enemy.Left > Player.Right)
                                {
                                    Acak = Acak2;
                                    if (Acak == 2)
                                    {
                                        Acak++;
                                    }
                                }
                                else
                                {
                                    Acak = Acak2;
                                }
                            }
                        }

                        if (Acak == 1)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 3)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }

                    else if (MKiri == false && MBawa == false && MKanan == false)
                    {
                        Count++;
                        if (Count > LimAtas / 8)
                        {
                            MajuBa = false;
                            MajuAt = true; ;
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = true;
                            Count = 0;
                        }
                    }
                }
                #endregion

                #region Kanan

                // sama dengan di atas
                else if (MajuKa == true && Mark == true)
                {
                    if (Easy == true || Medium == true || Hard == true)
                    {
                        if (Medium == true)
                        {
                            if (Enemy.Top < Player.Bottom && Enemy.Bottom > Player.Top && ListLabel[tandaKanan].Left > Enemy.Right && Enemy.Left > Player.Right)
                            {
                                if ((ListLabel[tandaatas].Right + 70 > Enemy.Left && ListLabel[tandabawa].Right + 70 > Enemy.Right) || Enemy.Right - Player.Left <= 200)
                                {
                                    MKanan = false;
                                    MAtas = false;
                                    MBawa = false;
                                    Hitport = true;
                                }
                            }
                        }
                        if (Hard == true)
                        {
                            if (((Enemy.Top < Player.Bottom && Player.Bottom - Enemy.Top <= 200) || (Enemy.Bottom > Player.Top && Player.Top - Enemy.Bottom <= 200)) && ListLabel[tandaKanan].Left > Enemy.Right && Enemy.Left > Player.Right)
                            {
                                if ((ListLabel[tandaatas].Right + 70 > Enemy.Left && ListLabel[tandabawa].Right + 70 > Enemy.Right) || Enemy.Right - Player.Left <= 300)
                                {
                                    MKanan = false;
                                    MAtas = false;
                                    MBawa = false;
                                    Hitport = true;
                                }
                            }
                        }
                    }
                    if (MAtas == true && MKanan == false && MBawa == false)
                    {
                        BerBalik(ref Count, Spd, Player, +KKTop, +KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    else if (MBawa == true && MKanan == false && MAtas == false)
                    {
                        BerBalik(ref Count, Spd, Player, -KKTop, +KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    else if (MBawa == true && MKanan == true && MAtas == false)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Right - 100 < Enemy.Right)
                            {
                                if (BatKi - 200 < Enemy.Bottom && Player.Right + 200 > Enemy.Left && Player.Top > Enemy.Bottom)
                                {
                                    if (Player.Right > Enemy.Left && Player.Left < Enemy.Right)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (Player.Top < Enemy.Bottom && Player.Bottom > Enemy.Top)
                                    {
                                        Acak2 = 2;
                                    }
                                    else if (SesihKiri > SesihAtas)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (SesihKiri < SesihAtas)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                                else if (Enemy.Bottom > Player.Top)
                                {
                                    Acak2 = 2;
                                }
                            }
                        }

                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -KKTop, +KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    else if (MBawa == false && MKanan == true && MAtas == true)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Right - 100 < Enemy.Right)
                            {
                                if (BatKi - 200 < Enemy.Bottom && Player.Right + 200 > Enemy.Left && Player.Top > Enemy.Bottom)
                                {
                                    if (Player.Right > Enemy.Left && Player.Left < Enemy.Right)
                                    {
                                        Acak2 = 2;
                                    }
                                    else if (Player.Top < Enemy.Bottom && Player.Bottom > Enemy.Top)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (SesihKiri > SesihAtas)
                                    {
                                        Acak2 = 2;
                                    }
                                    else if (SesihKiri < SesihAtas)
                                    {
                                        Acak2 = 1;
                                    }
                                }
                                else if (Enemy.Bottom > Player.Top)
                                {
                                    Acak2 = 1;
                                }
                            }
                        }

                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +KKTop, +KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 1)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    else if (MBawa == true && MKanan == false && MAtas == true)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Right - 100 < Enemy.Right)
                            {
                                if (BatAt > Enemy.Bottom)
                                {
                                    Acak2 = 1;
                                }
                                else if (BatAt < Enemy.Top)
                                {
                                    Acak2 = 2;
                                }
                            }
                        }

                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -KKTop, +KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);

                        }
                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +KKTop, +KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                    }
                    else if (MBawa == true && MKanan == true && MAtas == true)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Right - 100 < Enemy.Right)
                            {
                                if (BatAt - 200 < Enemy.Bottom && Player.Top > Enemy.Bottom)
                                {
                                    Acak = Acak2 + 1;
                                }
                                else if (BatAt + 200 > Enemy.Top && Enemy.Top > Player.Bottom)
                                {
                                    Acak = Acak2;
                                    if (Acak == 2)
                                    {
                                        Acak++;
                                    }
                                }
                                else
                                {
                                    Acak = Acak2;
                                }
                            }
                        }

                        if (Acak == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -KKTop, +KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +KKTop, +KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 3)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }

                    else if (MBawa == false && MKanan == false && MAtas == false)
                    {
                        Count++;
                        if (Count > LimKiri / 8)
                        {
                            MajuKa = false;
                            MajuKi = true;
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = true;
                            Count = 0;
                        }
                    }
                }

                #endregion

                #region Kiri
                // sama dengan di atas
                else if (MajuKi == true)
                {
                    if (Easy == true || Medium == true || Hard == true)
                    {
                        if (Medium == true)
                        {
                            if (Enemy.Top < Player.Bottom && Enemy.Bottom > Player.Top && ListLabel[tandaKiri].Right < Enemy.Right && Enemy.Right < Player.Left)
                            {
                                if ((ListLabel[tandaatas].Left - 70 < Enemy.Right && ListLabel[tandabawa].Left - 70 < Enemy.Right) || Player.Right - Enemy.Left <= 200)
                                {
                                    MKiri = false;
                                    MBawa = false;
                                    MAtas = false;
                                    Hitport = true;
                                }
                            }
                        }

                        if (Hard == true)
                        {

                            if (((Enemy.Top < Player.Bottom && Player.Bottom - Enemy.Top <= 200) || (Enemy.Bottom > Player.Top && Player.Top - Enemy.Bottom <= 200)) && ListLabel[tandaKiri].Right < Enemy.Right && Enemy.Right < Player.Left)
                            {
                                if ((ListLabel[tandaatas].Left - 70 < Enemy.Right && ListLabel[tandabawa].Left - 70 < Enemy.Right) || Player.Right - Enemy.Left <= 300)
                                {
                                    MKiri = false;
                                    MBawa = false;
                                    MAtas = false;
                                    Hitport = true;
                                }
                            }
                        }
                    }

                    if (Mark == true)
                    {
                        if (MBawa == true && MKiri == false && MAtas == false)
                        {
                            BerBalik(ref Count, Spd, Player, -KKTop, -KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        else if (MAtas == true && MKiri == false && MBawa == false)
                        {
                            BerBalik(ref Count, Spd, Player, +KKTop, -KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        else if (MBawa == true && MKiri == true && MAtas == false)
                        {
                            if (Easy == true || Medium == true || Hard == true)
                            {
                                if (Player.Left + 100 > Enemy.Right)
                                {
                                    if (BatAt - 200 < Enemy.Bottom && Player.Left - 200 < Enemy.Right && Player.Top > Enemy.Bottom)
                                    {
                                        if (Player.Right > Enemy.Left && Player.Left < Enemy.Right)
                                        {
                                            Acak2 = 2;
                                        }
                                        else if (Player.Top < Enemy.Bottom && Player.Bottom > Enemy.Top)
                                        {
                                            Acak2 = 1;
                                        }
                                        else if (SesihKiri > SesihAtas)
                                        {
                                            Acak2 = 2;
                                        }
                                        else if (SesihKiri < SesihAtas)
                                        {
                                            Acak2 = 1;
                                        }

                                    }
                                    else if (Enemy.Right > Player.Left)
                                    {
                                        Acak2 = 1;
                                    }
                                }
                            }
                            if (Acak2 == 1)
                            {
                                BerBalik(ref Count, Spd, Player, -KKTop, -KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak2 == 2)
                            {
                                BerBawah = true;
                                BerAtas = true;
                                BerKanan = true;
                                BerKiri = true;
                                Mark = false;
                                kanan = tandaKanan;
                                atas = tandaatas;
                                bawa = tandabawa;
                                kiri = tandaKiri;
                            }
                        }
                        else if (MBawa == false && MKiri == true && MAtas == true)
                        {
                            if (Easy == true || Medium == true || Hard == true)
                            {
                                if (Player.Left + 100 > Enemy.Right)
                                {
                                    if (BatAt - 200 < Enemy.Bottom && Player.Left - 200 < Enemy.Right && Player.Top > Enemy.Bottom)
                                    {
                                        if (Player.Right > Enemy.Left && Player.Left < Enemy.Right)
                                        {
                                            Acak2 = 1;
                                        }
                                        else if (Player.Top < Enemy.Bottom && Player.Bottom > Enemy.Top)
                                        {
                                            Acak2 = 2;
                                        }
                                        else if (SesihKiri > SesihAtas)
                                        {
                                            Acak2 = 1;
                                        }
                                        else if (SesihKiri < SesihAtas)
                                        {
                                            Acak2 = 2;
                                        }

                                    }
                                    else if (Enemy.Right > Player.Left)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                            }

                            if (Acak2 == 2)
                            {
                                BerBalik(ref Count, Spd, Player, +KKTop, -KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak2 == 1)
                            {
                                BerBawah = true;
                                BerAtas = true;
                                BerKanan = true;
                                BerKiri = true;
                                Mark = false;
                                kanan = tandaKanan;
                                atas = tandaatas;
                                bawa = tandabawa;
                                kiri = tandaKiri;

                            }
                        }
                        else if (MBawa == true && MKiri == false && MAtas == true)
                        {
                            if (Easy == true || Medium == true || Hard == true)
                            {
                                if (Player.Right - 100 < Enemy.Right)
                                {
                                    if (BatAt > Enemy.Bottom)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (BatAt < Enemy.Top)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                            }

                            if (Acak2 == 1)
                            {
                                BerBalik(ref Count, Spd, Player, -KKTop, -KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak2 == 2)
                            {
                                BerBalik(ref Count, Spd, Player, +KKTop, -KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                        }
                        else if (MBawa == true && MKiri == true && MAtas == true)
                        {
                            if (Easy == true || Medium == true || Hard == true)
                            {
                                if (Player.Right - 100 < Enemy.Right)
                                {
                                    if (BatAt > Enemy.Bottom)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (BatAt < Enemy.Top)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                            }

                            if (Acak == 1)
                            {
                                BerBalik(ref Count, Spd, Player, -KKTop, -KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak == 2)
                            {
                                BerBalik(ref Count, Spd, Player, +KKTop, -KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak == 3)
                            {
                                BerBawah = true;
                                BerAtas = true;
                                BerKanan = true;
                                BerKiri = true;
                                Mark = false;
                                kanan = tandaKanan;
                                atas = tandaatas;
                                bawa = tandabawa;
                                kiri = tandaKiri;
                            }

                        }
                        else if (MBawa == false && MKiri == false && MAtas == false)
                        {
                            Count++;
                            if (Count > LimKiri / 8)
                            {
                                MajuKi = false;
                                MajuKa = true;
                                BerBawah = true;
                                BerAtas = true;
                                BerKanan = true;
                                BerKiri = true;
                                Mark = true;
                                Count = 0;
                            }
                        }
                    }
                }
            }
            if (MajuAt == true)
            {
                Player.Top -= Spd; // kecepatannya

                // kembalikan selisihnya seperti semula
                kecilbawa = asd;
                kecilkanan = asd;
                kecilkiri = asd;
            }
            else if (MajuBa == true)
            {
                Player.Top += Spd;
                kecilatas = asd;
                kecilkanan = asd;
                kecilkiri = asd;
            }
            else if (MajuKi == true)
            {
                Player.Left -= Spd;
                kecilbawa = asd;
                kecilkanan = asd;
                kecilatas = asd;

                // ini supaya animasi arah jalannya benar
                if (HitKiri == 0)
                {
                    HitKiri++;
                    HitKanan = 0;
                    Player.Image = Ki.Image;
                }

                // ini kalo kenna label supaya kelihatan loncat
                for (int i = 0; i < ListLabel.Count; i++)
                {
                    if (Player.Bounds.IntersectsWith(ListLabel[i].Bounds))
                    {
                        Player.Image = JumpKi.Image;
                        HitKiri = 0;
                    }
                }
            }
            else if (MajuKa == true)
            {
                Player.Left += Spd;
                kecilbawa = asd;
                kecilatas = asd;
                kecilkiri = asd;
                if (HitKanan == 0)
                {
                    HitKanan++;
                    HitKiri = 0;
                    Player.Image = Ka.Image;
                }
                for (int i = 0; i < ListLabel.Count; i++)
                {
                    if (Player.Bounds.IntersectsWith(ListLabel[i].Bounds))
                    {
                        Player.Image = JumpKa.Image;
                        HitKanan = 0;
                    }
                }
            }

                #endregion

            #endregion
        }

        // Method ini digunakan untuk 2 player

        public void Move(List<PictureBox> ListPort, List<PictureBox> Player)
        {
            if (Player.Count == 2)
            {

                for (int i = 0; i < ListPower.Count; i++)
                {
                    foreach (PictureBox Play in Player)
                    {
                        if (Play.Bounds.IntersectsWith(ListPower[i].Pict.Bounds) && ListPower[i].Pict.Visible == true && SpawnPower == false)
                        {
                            ind = i;
                            DelPower = true;
                            ListPower[i].Pict.Visible = false;
                            if (ListPower[i].Pow == 1)
                            {
                                UseIncSpeed();
                            }
                            if (ListPower[i].Pow == 2)
                            {
                                UseDecSpeed();
                            }
                            if (ListPower[i].Pow == 3)
                            {
                                UseNoSpeed();
                            }
                            ListPower.RemoveAt(i);
                            break;
                        }
                    }
                }

                // mulai jalan tiap hewan
                for (int i = 0; i < ListDomba.Count; i++)
                {
                    // ini untuk transport hewan
                    for (int j = 0; j < ListPort.Count; j++)
                    {
                        // cek bisa atau tidak mentransfer
                        if (ListDomba[i].Lamb.Bounds.IntersectsWith(ListPort[j].Bounds) && ListPort[j].Visible == true && ListDomba[i].HitPort == true && ListPort[j].Visible == true)
                        {
                            ListDomba[i].HitPort = false; // ini supaya tidak looping forever di portal
                            ListDomba[i].Jalan = false; // ini supaya tidak jalan binatang ketika di portal

                            // ini supaya sama posisinya dengan portal
                            ListDomba[i].Lamb.Left = ListPort[j].Left;
                            ListDomba[i].Lamb.Top = ListPort[j].Top;

                            ListPort[j].Visible = false; // buat portalnya tidak kelihatan
                            ListDomba[i].Portal = ListPort[j]; // masukkan portal yang sama ke list hewan, karena indexnya portal susah diambil jadi dimasukkan saja ke list hewan

                            // ubah gambar ke transport
                            if (ListDomba[i].MajuKi == true || ListDomba[i].MajuAt == true)
                            {
                                ListDomba[i].Lamb.Image = ListDomba[i].TransKi.Image;
                                ListDomba[i].Mark = 5;
                            }
                            else if (ListDomba[i].MajuKa == true || ListDomba[i].MajuBa == true)
                            {
                                ListDomba[i].Lamb.Image = ListDomba[i].TransKa.Image;
                                ListDomba[i].Mark = 6;
                            }

                            //start timernya
                            ListDomba[i].Tim.Start();


                        }
                    }

                    // untuk jalan
                    if (ListDomba[i].Jalan == true)
                    {
                        ListDomba[i].Jalan = false; // ada alasannya... di bawah
                        for (int j = 0; j < ListPort.Count; j++)
                        {
                            // cek kalo dia kenna portal tapi hitport salah
                            if (ListDomba[i].HitPort == false && ListDomba[i].Lamb.Bounds.IntersectsWith(ListPort[j].Bounds))
                            {
                                // jalankan hewannya
                                GerakDomba(Player[1], ref ListDomba[i].HitPort, Player[0], ref ListDomba[i].Mark, ListDomba[i].KKTop, ListDomba[i].KKLeft, ListDomba[i].ABTop, ListDomba[i].ABLeft, ListDomba[i].LimKiri, ListDomba[i].LimAtas,
                                    ref ListDomba[i].HitKanan, ref ListDomba[i].HitKiri, ListDomba[i].Ka, ListDomba[i].Ki, ListDomba[i].JumpKa,
                                    ListDomba[i].JumpKi, ListDomba[i].TransKa, ListDomba[i].TransKi, ref ListDomba[i].Speed, ref ListDomba[i].Counter,
                                    ListDomba[i].Lamb, ListLabel, ref ListDomba[i].MajuKa, ref ListDomba[i].MajuKi, ref ListDomba[i].MajuBa,
                                    ref ListDomba[i].MajuAt, ref ListDomba[i].BerKanan, ref ListDomba[i].BerKiri, ref ListDomba[i].BerAtas,
                                    ref ListDomba[i].BerBawah, ref ListDomba[i].Mbelo, ref ListDomba[i].tandaKanan, ref ListDomba[i].tandaKiri,
                                    ref ListDomba[i].tandabawa, ref ListDomba[i].tandaatas, ref ListDomba[i].kanan, ref ListDomba[i].bawa,
                                    ref ListDomba[i].kiri, ref ListDomba[i].atas, ref ListDomba[i].kecilkanan, ref ListDomba[i].kecilbawa,
                                    ref ListDomba[i].kecilkiri, ref ListDomba[i].kecilatas, ref ListDomba[i].MAtas, ref  ListDomba[i].MBawa,
                                    ref ListDomba[i].MKiri, ref ListDomba[i].MKanan);
                                ListDomba[i].Jalan = true; // dibenarkan kembali

                                // di sini di cek kalo hewannya masih sentuh atau tidak portalnya kalo tidak hitportnya jadi benar 
                                if (ListDomba[i].Lamb.Bounds.IntersectsWith(ListPort[j].Bounds))
                                { }
                                else
                                {
                                    ListDomba[i].HitPort = true;
                                }
                            }
                        }

                        // ini terjadi kalo di atas salah semua
                        if (ListDomba[i].Jalan == false)
                        {
                            // jalan yang di sini dengan yang di atas itu sama(Reaksi gerak domba)
                            GerakDomba(Player[1], ref ListDomba[i].HitPort, Player[0], ref ListDomba[i].Mark, ListDomba[i].KKTop, ListDomba[i].KKLeft, ListDomba[i].ABTop, ListDomba[i].ABLeft, ListDomba[i].LimKiri,
                                ListDomba[i].LimAtas, ref ListDomba[i].HitKanan, ref ListDomba[i].HitKiri, ListDomba[i].Ka, ListDomba[i].Ki,
                                ListDomba[i].JumpKa, ListDomba[i].JumpKi, ListDomba[i].TransKa, ListDomba[i].TransKi, ref ListDomba[i].Speed,
                                ref ListDomba[i].Counter, ListDomba[i].Lamb, ListLabel, ref ListDomba[i].MajuKa, ref ListDomba[i].MajuKi,
                                ref ListDomba[i].MajuBa, ref ListDomba[i].MajuAt, ref ListDomba[i].BerKanan, ref ListDomba[i].BerKiri,
                                ref ListDomba[i].BerAtas, ref ListDomba[i].BerBawah, ref ListDomba[i].Mbelo, ref ListDomba[i].tandaKanan,
                                ref ListDomba[i].tandaKiri, ref ListDomba[i].tandabawa, ref ListDomba[i].tandaatas, ref ListDomba[i].kanan,
                                ref ListDomba[i].bawa, ref ListDomba[i].kiri, ref ListDomba[i].atas, ref ListDomba[i].kecilkanan, ref ListDomba[i].kecilbawa,
                                ref ListDomba[i].kecilkiri, ref ListDomba[i].kecilatas, ref ListDomba[i].MAtas, ref  ListDomba[i].MBawa,
                                ref ListDomba[i].MKiri, ref ListDomba[i].MKanan);
                        }
                        ListDomba[i].Jalan = true; // di benarkan kembali

                        for (int k = 0; k < Player.Count; k++)
                        {
                            if (Player[k].Bounds.IntersectsWith(ListDomba[i].Lamb.Bounds))
                            {
                                CatchAni = i;
                                Catch = true;
                                ListDomba[i].Lamb.Visible = false;
                                ListDomba.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < ListDomba.Count; i++)
                {
                    // ini untuk transport hewan
                    for (int j = 0; j < ListPort.Count; j++)
                    {
                        // cek bisa atau tidak mentransfer
                        if (ListDomba[i].Lamb.Bounds.IntersectsWith(ListPort[j].Bounds) && ListPort[j].Visible == true && ListDomba[i].HitPort == true && ListPort[j].Visible == true)
                        {
                            ListDomba[i].HitPort = false; // ini supaya tidak looping forever di portal
                            ListDomba[i].Jalan = false; // ini supaya tidak jalan binatang ketika di portal

                            // ini supaya sama posisinya dengan portal
                            ListDomba[i].Lamb.Left = ListPort[j].Left;
                            ListDomba[i].Lamb.Top = ListPort[j].Top;

                            ListPort[j].Visible = false; // buat portalnya tidak kelihatan
                            ListDomba[i].Portal = ListPort[j]; // masukkan portal yang sama ke list hewan, karena indexnya portal susah diambil jadi dimasukkan saja ke list hewan

                            // ubah gambar ke transport
                            if (ListDomba[i].MajuKi == true || ListDomba[i].MajuAt == true)
                            {
                                ListDomba[i].Lamb.Image = ListDomba[i].TransKi.Image;
                                ListDomba[i].Mark = 5;
                            }
                            else if (ListDomba[i].MajuKa == true || ListDomba[i].MajuBa == true)
                            {
                                ListDomba[i].Lamb.Image = ListDomba[i].TransKa.Image;
                                ListDomba[i].Mark = 6;
                            }

                            //start timernya
                            ListDomba[i].Tim.Start();


                        }
                    }

                    // untuk jalan
                    if (ListDomba[i].Jalan == true)
                    {
                        ListDomba[i].Jalan = false; // ada alasannya... di bawah
                        for (int j = 0; j < ListPort.Count; j++)
                        {
                            // cek kalo dia kenna portal tapi hitport salah
                            if (ListDomba[i].HitPort == false && ListDomba[i].Lamb.Bounds.IntersectsWith(ListPort[j].Bounds))
                            {
                                // jalankan hewannya
                                GerakDomba(ref ListDomba[i].Mark, ListDomba[i].KKTop, ListDomba[i].KKLeft, ListDomba[i].ABTop, ListDomba[i].ABLeft, ListDomba[i].LimKiri, ListDomba[i].LimAtas,
                                    ref ListDomba[i].HitKanan, ref ListDomba[i].HitKiri, ListDomba[i].Ka, ListDomba[i].Ki, ListDomba[i].JumpKa,
                                    ListDomba[i].JumpKi, ListDomba[i].TransKa, ListDomba[i].TransKi, ref ListDomba[i].Speed, ref ListDomba[i].Counter,
                                    ListDomba[i].Lamb, ListLabel, ref ListDomba[i].MajuKa, ref ListDomba[i].MajuKi, ref ListDomba[i].MajuBa,
                                    ref ListDomba[i].MajuAt, ref ListDomba[i].BerKanan, ref ListDomba[i].BerKiri, ref ListDomba[i].BerAtas,
                                    ref ListDomba[i].BerBawah, ref ListDomba[i].Mbelo, ref ListDomba[i].tandaKanan, ref ListDomba[i].tandaKiri,
                                    ref ListDomba[i].tandabawa, ref ListDomba[i].tandaatas, ref ListDomba[i].kanan, ref ListDomba[i].bawa,
                                    ref ListDomba[i].kiri, ref ListDomba[i].atas, ref ListDomba[i].kecilkanan, ref ListDomba[i].kecilbawa,
                                    ref ListDomba[i].kecilkiri, ref ListDomba[i].kecilatas, ref ListDomba[i].MAtas, ref  ListDomba[i].MBawa,
                                    ref ListDomba[i].MKiri, ref ListDomba[i].MKanan);
                                ListDomba[i].Jalan = true; // dibenarkan kembali

                                // di sini di cek kalo hewannya masih sentuh atau tidak portalnya kalo tidak hitportnya jadi benar 
                                if (ListDomba[i].Lamb.Bounds.IntersectsWith(ListPort[j].Bounds))
                                { }
                                else
                                {
                                    ListDomba[i].HitPort = true;
                                }
                            }
                        }

                        // ini terjadi kalo di atas salah semua
                        if (ListDomba[i].Jalan == false)
                        {
                            // jalan yang di sini dengan yang di atas itu sama(Reaksi gerak domba)
                            GerakDomba(ref ListDomba[i].Mark, ListDomba[i].KKTop, ListDomba[i].KKLeft, ListDomba[i].ABTop, ListDomba[i].ABLeft, ListDomba[i].LimKiri,
                                ListDomba[i].LimAtas, ref ListDomba[i].HitKanan, ref ListDomba[i].HitKiri, ListDomba[i].Ka, ListDomba[i].Ki,
                                ListDomba[i].JumpKa, ListDomba[i].JumpKi, ListDomba[i].TransKa, ListDomba[i].TransKi, ref ListDomba[i].Speed,
                                ref ListDomba[i].Counter, ListDomba[i].Lamb, ListLabel, ref ListDomba[i].MajuKa, ref ListDomba[i].MajuKi,
                                ref ListDomba[i].MajuBa, ref ListDomba[i].MajuAt, ref ListDomba[i].BerKanan, ref ListDomba[i].BerKiri,
                                ref ListDomba[i].BerAtas, ref ListDomba[i].BerBawah, ref ListDomba[i].Mbelo, ref ListDomba[i].tandaKanan,
                                ref ListDomba[i].tandaKiri, ref ListDomba[i].tandabawa, ref ListDomba[i].tandaatas, ref ListDomba[i].kanan,
                                ref ListDomba[i].bawa, ref ListDomba[i].kiri, ref ListDomba[i].atas, ref ListDomba[i].kecilkanan, ref ListDomba[i].kecilbawa,
                                ref ListDomba[i].kecilkiri, ref ListDomba[i].kecilatas, ref ListDomba[i].MAtas, ref  ListDomba[i].MBawa,
                                ref ListDomba[i].MKiri, ref ListDomba[i].MKanan);
                        }
                        ListDomba[i].Jalan = true; // di benarkan kembali

                        //for (int k = 0; k < Player.Count; k++)
                        //{
                        //    if (Player[k].Bounds.IntersectsWith(ListDomba[i].Lamb.Bounds))
                        //    {
                        //        ListDomba[i].Lamb.Visible = false;
                        //        ListDomba.RemoveAt(i);
                        //        break;
                        //    }
                        //}
                    }
                }
            }
        }
        public void GerakDomba(PictureBox Enemy2, ref bool Hitport, PictureBox Enemy, ref int ActMark, int KKTop, int KKLeft, int ABTop, int ABLeft, int LimKiri, int LimAtas, ref int HitKanan, ref int HitKiri,
            PictureBox Ka, PictureBox Ki, PictureBox JumpKa, PictureBox JumpKi, PictureBox TransKa, PictureBox TransKi, ref int Spd, ref int Count,
            PictureBox Player, List<Label> ListLabel, ref bool MajuKa, ref bool MajuKi, ref bool MajuBa, ref bool MajuAt, ref bool BerKanan,
            ref bool BerKiri, ref bool BerAtas, ref bool BerBawah, ref bool Mark, ref int tandaKanan, ref int tandaKiri, ref int tandabawa,
            ref int tandaatas, ref int kanan, ref int bawa, ref int kiri, ref int atas, ref int kecilkanan, ref int kecilbawa, ref int kecilkiri,
            ref int kecilatas, ref bool MAtas, ref bool MBawa, ref bool MKiri, ref bool MKanan)
        {
            #region cek terdekat
            for (int i = 0; i < ListLabel.Count; i++)
            {
                if (kecilkanan > (ListLabel[i].Left) - Player.Right && (ListLabel[i].Left) - Player.Right >= 0 && (ListLabel[i].Top) < Player.Bottom && (ListLabel[i].Bottom) > Player.Top && ListLabel[i].Visible == true)
                {
                    kecilkanan = (ListLabel[i].Left) - Player.Right;
                    tandaKanan = i;
                }
                if (kecilbawa > (ListLabel[i].Top) - Player.Bottom && (ListLabel[i].Top) - Player.Bottom >= 0 && (ListLabel[i].Left) < Player.Right && (ListLabel[i].Right) > Player.Left && ListLabel[i].Visible == true)
                {
                    kecilbawa = (ListLabel[i].Top) - Player.Bottom;
                    tandabawa = i;
                }
                if (kecilkiri > Player.Left - (ListLabel[i].Right) && Player.Left - (ListLabel[i].Right) >= 0 && (ListLabel[i].Top) < Player.Bottom && (ListLabel[i].Bottom) > Player.Top && ListLabel[i].Visible == true)
                {
                    kecilkiri = Player.Left - (ListLabel[i].Right);
                    tandaKiri = i;
                }
                if (kecilatas > Player.Top - (ListLabel[i].Bottom) && Player.Top - (ListLabel[i].Bottom) >= 0 && (ListLabel[i].Left) < Player.Right && (ListLabel[i].Right) > Player.Left && ListLabel[i].Visible == true)
                {
                    kecilatas = Player.Top - (ListLabel[i].Bottom);
                    tandaatas = i;
                }
            }
            #endregion

            // Cek apakah bisa belok kiri kanan atas ato bawah
            #region cek belok
            if (kecilkiri >= LimKiri)
            {
                MKiri = true;
            }
            else if (kecilkiri < LimKiri)
            {
                MKiri = false;
            }

            if (kecilkanan >= LimKiri)
            {
                MKanan = true;
            }
            else if (kecilkanan < LimKiri)
            {
                MKanan = false;
            }

            if (kecilbawa >= LimAtas)
            {
                MBawa = true;
            }
            else if (kecilbawa < LimAtas)
            {
                MBawa = false;
            }

            if (kecilatas >= LimAtas)
            {
                MAtas = true;
            }
            else if (kecilatas < LimAtas)
            {
                MAtas = false;
            }
            #endregion

            // Pengaturan beloknya
            #region Belokkkkk
            // ini kalo pertama kali jalan jadi pemilihannya acak
            if (MajuKa == false && MajuKi == false && MajuBa == false && MajuAt == false)
            {
                Random Pilih = new Random();
                int Acak = Pilih.Next(1, 5);
                PemilihanAcak(Acak, ref MajuKa, ref MajuKi, ref MajuBa, ref MajuAt, ref MAtas, ref  MBawa, ref MKiri, ref MKanan);
                //MajuKi = true;

                // masukkan nilai di int ataskanan dkk
                atas = tandaatas;
                bawa = tandabawa;
                kiri = tandaKiri;
                kanan = tandaKanan;
                Player.Image = Ki.Image;
                ActMark = 1;
            }

            // ini kalo sudah jalan
            if (MajuKa == true || MajuKi == true || MajuBa == true || MajuAt == true)
            {
                // di sini hewan namanya palyer, lalu player yang asli yang kita kontrol namanya enemy(karna bagi hewan kita enemy wkwkwk)

                // ini untuk pilih acak 
                Random Pilih = new Random();
                int Acak = Pilih.Next(1, 4); // pemilihan perempatan
                int Acak2 = Pilih.Next(1, 3); // pemilihan pertigaan
                int SesihKiri = Enemy.Left - Player.Left; // maksudnya selisih kirinya playernyda hewan
                int SesihAtas = Enemy.Top - Player.Top; // sama sesih atas tapi ini atas

                // ini buat ke positif
                if (SesihKiri < 0)
                {
                    SesihKiri *= -1;
                }
                if (SesihAtas < 0)
                {
                    SesihAtas *= -1;
                }

                // ini untuk tau posisi tengah player
                int BatKi = Player.Left + Player.Width / 2;
                int BatAt = Player.Top + Player.Height / 2;

                CekBelok(ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas, ref Mark);

                List<PictureBox> ListP = new List<PictureBox> { Enemy, Enemy2 };

                #region Atas

                // prinsipnya atas dengan arah lainnya itu sama jadi hanya dijelaskan yang atas saja, kalo bawah semua berkebalikan dengan atas
                // kalo kiri kanan juga sama dengan atas bawah, jadi anggak top jadi left, bottom jadi right, sisa diubah begitu jadi untuk kiri dan kanan

                // kalo jalan dari arah atas
                if (MajuAt == true && Mark == true)
                {
                    foreach (PictureBox Pla in ListP)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Medium == true)
                            {

                                // maksudnya    seperti hewan berhadapan persis dgn Pla,    ini maksudnya di atasnya hewan tidak ada label,       ini maksudnya di atas hewannya ada Pla
                                if (((Pla.Left < Player.Right && Player.Right - Pla.Left <= 75) || (Pla.Right > Player.Left && Pla.Right - Player.Left <= 75)) && ListLabel[tandaatas].Bottom < Pla.Bottom && Pla.Bottom < Player.Top)
                                {
                                    //maksudnya   batasnya sampai hewan belok memutar kalo Pla masih di bawahlabel di kiri kanannya           atau selisih dekatnya 200
                                    if ((ListLabel[tandaKiri].Top - 70 < Pla.Bottom && ListLabel[tandaKanan].Top - 70 < Pla.Bottom) || Player.Bottom - Pla.Top <= 200)
                                    {
                                        MKiri = false;
                                        MKanan = false;
                                        MAtas = false;
                                        Hitport = true; // ini ketika dia berhadapan dengan transport
                                    }
                                }
                            }

                            if (Hard == true)
                            {
                                // ini mirip medium tapi ruang lingkupnya diperluas, yang berubah cuma di bawah ini saja:   Maksudnya radius berbaliknya bukan kalo hanya pas di depannya, yang ini batas kirinya dan kanannya sampe 200
                                if (((Pla.Left < Player.Right && Player.Right - Pla.Left <= 200) || (Pla.Right > Player.Left && Pla.Right - Player.Left < 200)) && ListLabel[tandaatas].Bottom < Pla.Bottom && Pla.Bottom < Player.Top)
                                {
                                    if ((ListLabel[tandaKiri].Top - 70 < Pla.Bottom && ListLabel[tandaKanan].Top - 70 < Pla.Bottom) || Player.Bottom - Pla.Top <= 300)
                                    {
                                        MKiri = false;
                                        MKanan = false;
                                        MAtas = false;
                                        Hitport = true;
                                    }
                                }
                            }
                        }
                    }
                    // di bawah ini semuanya kemungkinan yang ada kalo bergerak ke atas lalu ada perbelokan
                    // ini kalo perbelokan kiri
                    if (MKiri == true && MAtas == false && MKanan == false)
                    {
                        // arahnya lihat di bawah                              sini!!(Maksunya betulan bawahnya pas) ada kata kiri artinya arahnya itu
                        BerBalik(ref Count, Spd, Player, -ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    // hanya perbelokan kanan
                    else if (MKanan == true && MAtas == false && MKiri == false)
                    {
                        BerBalik(ref Count, Spd, Player, -ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    // pertigaan antara depan dan kiri
                    else if (MKiri == true && MAtas == true && MKanan == false)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Top + 100 > Enemy.Bottom || Player.Top + 100 > Enemy2.Bottom)  // maksudnya radiusnya sampe 100 pixel ke bawah dari hewan
                            {
                                // radiusnya sampe 200 ke kiri dan 200 ke atas dgn hewan masih lebih ke kanan
                                if (BatKi - 200 < Enemy.Right && Player.Top - 200 < Enemy.Bottom && Player.Left > Enemy.Right && BatKi - 200 < Enemy2.Right && Player.Top - 200 < Enemy2.Bottom && Player.Left > Enemy2.Right)
                                {

                                    // ini kalo di sampingnya persis
                                    if (Player.Bottom > Enemy.Top && Player.Top < Enemy.Bottom && Player.Bottom > Enemy2.Top && Player.Top < Enemy2.Bottom)
                                    {
                                        Acak2 = 2;
                                    }
                                    // ini di atasnya persis
                                    else if (Player.Left < Enemy.Right && Player.Right > Enemy.Left && Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                    {
                                        Acak2 = 1;
                                    }
                                }
                                else if (BatKi - 200 < Enemy.Right && Player.Top - 200 < Enemy.Bottom && Player.Left > Enemy.Right)
                                {

                                    // ini kalo di sampingnya persis
                                    if (Player.Bottom > Enemy.Top && Player.Top < Enemy.Bottom)
                                    {
                                        Acak2 = 2;
                                    }
                                    // ini di atasnya persis
                                    else if (Player.Left < Enemy.Right && Player.Right > Enemy.Left)
                                    {
                                        Acak2 = 1;
                                    }
                                }
                                else if (BatKi - 200 < Enemy2.Right && Player.Top - 200 < Enemy2.Bottom && Player.Left > Enemy2.Right)
                                {

                                    // ini kalo di sampingnya persis
                                    if (Player.Bottom > Enemy2.Top && Player.Top < Enemy2.Bottom)
                                    {
                                        Acak2 = 2;
                                    }
                                    // ini di atasnya persis
                                    else if (Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                    {
                                        Acak2 = 1;
                                    }
                                }
                                // ini kalo enemy di kanannya hewan
                                else if (Enemy.Right > Player.Left)
                                {
                                    Acak2 = 1;
                                }
                            }
                        }

                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            // kalo begini artinya maju tetap
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false; // di falsekan supaya tidak belok lagi

                            // di kasi samakan supaya marknya tdk benar(lihat method "cek belok")
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    // pertigaan depan kanan
                    else if (MKanan == true && MAtas == true && MKiri == false)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            // ini mirip dengan di atas
                            if (Player.Top + 100 > Enemy.Bottom || Player.Top + 100 > Enemy.Bottom)
                            {
                                if (BatKi + 200 > Enemy.Left && Player.Top - 200 < Enemy.Bottom && Player.Right < Enemy.Right && BatKi + 200 > Enemy2.Left && Player.Top - 200 < Enemy2.Bottom && Player.Right < Enemy2.Right)
                                {
                                    if (Player.Bottom > Enemy.Top && Player.Top < Enemy.Bottom && Player.Bottom > Enemy2.Top && Player.Top < Enemy2.Bottom)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (Player.Left < Enemy.Right && Player.Right > Enemy2.Left && Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                    {
                                        Acak2 = 2;
                                    }
                                    //else if ((Player.Left < Enemy.Right && Player.Right > Enemy.Left && Player.Bottom > Enemy2.Top && Player.Top < Enemy2.Bottom) ||
                                    //   (Player.Bottom > Enemy.Top && Player.Top < Enemy.Bottom && Player.Left < Enemy2.Right && Player.Right > Enemy2.Left))
                                    //{
                                    //    Acak2 = 0;
                                    //    Player.Top += 10;
                                    //    MKiri = false;
                                    //    MKanan = false;
                                    //    MAtas = false;
                                    //    Hitport = true;
                                    //}
                                }
                                else if (BatKi + 200 > Enemy.Left && Player.Top - 200 < Enemy.Bottom && Player.Right < Enemy.Right)
                                {
                                    if (Player.Bottom > Enemy.Top && Player.Top < Enemy.Bottom)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (Player.Left < Enemy.Right && Player.Right > Enemy.Left)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                                else if (BatKi + 200 > Enemy2.Left && Player.Top - 200 < Enemy2.Bottom && Player.Right < Enemy2.Right)
                                {
                                    if (Player.Bottom > Enemy2.Top && Player.Top < Enemy2.Bottom)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                                else if (Enemy.Right < Player.Left)
                                {
                                    Acak2 = 2;
                                }
                            }
                        }

                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 1)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                            BerKiri = true;
                        }
                    }
                    // pertigaan kiri dan kanan
                    else if (MKanan == true && MAtas == false && MKiri == true)
                    {
                        // maksudnya hewan ke kanan kalo playernya lebih kiri dari hewannya dan sebaliknya
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Top + 100 > Enemy.Bottom && Player.Top + 100 > Enemy2.Bottom)
                            {
                                if (BatKi > Enemy.Right && BatKi > Enemy2.Right)
                                {
                                    Acak2 = 2;
                                }
                                else if (BatKi < Enemy.Left && BatKi < Enemy2.Left)
                                {
                                    Acak2 = 1;
                                }
                            }
                        }

                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }

                    }
                    // perempatan
                    else if (MKanan == true && MAtas == true && MKiri == true)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Top + 100 > Enemy.Bottom || Player.Top + 100 > Enemy2.Bottom)
                            {
                                // radius kalo musuh sampe dalam 200 pixel di samping kiri hewan, harus p.lef>en.rig(bawa) karna suppaya harus di kirinya
                                if ((BatKi - 200 < Enemy.Right && Player.Left > Enemy.Right && BatKi - 200 < Enemy2.Right && Player.Left > Enemy2.Right)
                                    || BatKi - 200 < Enemy.Right && Player.Left > Enemy.Right || BatKi - 200 < Enemy2.Right && Player.Left > Enemy2.Right)
                                {
                                    Acak = Acak2 + 1; // maksudnya bukan 1
                                }
                                else if ((BatKi - 200 < Enemy.Right && Player.Left > Enemy.Right && BatKi + 200 > Enemy2.Left && Enemy2.Left > Player.Right)
                                    || (BatKi + 200 > Enemy.Left && Enemy.Left > Player.Right && BatKi - 200 < Enemy2.Right && Player.Left > Enemy2.Right))
                                {
                                    Acak = 3;
                                }
                                // ini sama dengan di atas syaratnya
                                else if ((BatKi + 200 > Enemy.Left && Enemy.Left > Player.Right && BatKi + 200 > Enemy2.Left && Enemy2.Left > Player.Right) ||
                                    BatKi + 200 > Enemy.Left && Enemy.Left > Player.Right || BatKi + 200 > Enemy2.Left && Enemy2.Left > Player.Right)
                                {
                                    Acak = Acak2; // maksudnya tidak boleh 2
                                    if (Acak == 2)
                                    {
                                        Acak++;
                                    }
                                }
                                else if ((Player.Left < Enemy.Right && Player.Right > Enemy.Left && BatKi - 200 < Enemy2.Right && Player.Left > Enemy2.Right)
                                    || (Player.Left < Enemy2.Right && Player.Right > Enemy2.Left && BatKi - 200 < Enemy.Right && Player.Left > Enemy.Right))
                                {
                                    Acak = 2;
                                }
                                else if ((BatKi + 200 > Enemy.Left && Enemy.Left > Player.Right && Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                || (Player.Left < Enemy.Right && Player.Right > Enemy.Left && BatKi + 200 > Enemy2.Left && Enemy2.Left > Player.Right))
                                {
                                    Acak = 1;
                                }
                                else if ((Player.Left < Enemy.Right && Player.Right > Enemy.Left && Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                    || Player.Left < Enemy.Right && Player.Right > Enemy.Left || Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                {
                                    Acak = Acak2; // ini kalo bukan gerak depan
                                }
                            }
                        }

                        if (Acak == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 2)
                        {
                            BerBalik(ref Count, Spd, Player, -ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuAt, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 3)
                        {
                            // sama di atas
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    // ini kalo jalan buntu
                    else if (MKiri == false && MAtas == false && MKanan == false)
                    {
                        Count++; // ini supaya tidak langsung putar harus lebih dekat dulu
                        if (Count > LimAtas / 8) // ada sampai nilai tertentu supaya bisa belok kembali
                        {
                            MajuBa = true;
                            MajuAt = false;
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = true;
                            Count = 0;
                        }
                    }
                }
                #endregion

                #region Bawah

                // sama dengan di atas(Prinsipnnya mirip atas)
                else if (MajuBa == true && Mark == true)
                {
                    foreach (PictureBox Pla in ListP)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Medium == true)
                            {
                                //if (Pla.Left < Player.Right && Pla.Right > Player.Left && ListLabel[tandabawa].Top > Pla.Bottom && Pla.Top > Player.Bottom)
                                if (((Pla.Left < Player.Right && Player.Right - Pla.Left <= 75) || (Pla.Right > Player.Left && Player.Left - Pla.Right >= 75)) && ListLabel[tandabawa].Top > Pla.Bottom && Pla.Top > Player.Bottom)
                                {
                                    if ((ListLabel[tandaKiri].Bottom + 70 > Pla.Top && ListLabel[tandaKanan].Bottom + 70 > Pla.Top) || Pla.Bottom - Player.Top <= 200)
                                    {
                                        MKiri = false;
                                        MKanan = false;
                                        MBawa = false;
                                        Hitport = true;
                                    }
                                }
                            }

                            if (Hard == true)
                            {
                                if (((Pla.Left < Player.Right && Player.Right - Pla.Left <= 200) || (Pla.Right > Player.Left && Player.Left - Pla.Right >= 200)) && ListLabel[tandabawa].Top > Pla.Bottom && Pla.Top > Player.Bottom)
                                {
                                    if ((ListLabel[tandaKiri].Bottom + 70 > Pla.Top && ListLabel[tandaKanan].Bottom + 70 > Pla.Top) || Pla.Bottom - Player.Top <= 300)
                                    {
                                        MKiri = false;
                                        MKanan = false;
                                        MBawa = false;
                                        Hitport = true;
                                    }
                                }
                            }
                        }
                    }

                    if (MKiri == true && MBawa == false && MKanan == false)
                    {
                        BerBalik(ref Count, Spd, Player, +ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    else if (MKanan == true && MBawa == false && MKiri == false)
                    {
                        BerBalik(ref Count, Spd, Player, +ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    else if (MKiri == true && MBawa == true && MKanan == false)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Bottom + 100 > Enemy.Top || Player.Bottom + 100 > Enemy2.Top)  // maksudnya radiusnya sampe 100 pixel ke bawah dari hewan
                            {
                                // radiusnya sampe 200 ke kiri dan 200 ke atas dgn hewan masih lebih ke kanan
                                if (BatKi - 200 < Enemy.Right && Player.Bottom - 200 < Enemy.Top && Player.Left > Enemy.Right && BatKi - 200 < Enemy2.Right && Player.Bottom - 200 < Enemy2.Bottom && Player.Left > Enemy2.Right)
                                {

                                    // ini kalo di sampingnya persis
                                    if (Player.Top > Enemy.Bottom && Player.Bottom < Enemy.Top && Player.Top > Enemy2.Bottom && Player.Bottom < Enemy2.Top)
                                    {
                                        Acak2 = 2;
                                    }
                                    // ini di atasnya persis
                                    else if (Player.Left < Enemy.Right && Player.Right > Enemy.Left && Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                    {
                                        Acak2 = 1;
                                    }
                                }
                                else if (BatKi - 200 < Enemy.Right && Player.Bottom - 200 < Enemy.Top && Player.Left > Enemy.Right)
                                {

                                    // ini kalo di sampingnya persis
                                    if (Player.Top > Enemy.Bottom && Player.Bottom < Enemy.Top)
                                    {
                                        Acak2 = 2;
                                    }
                                    // ini di atasnya persis
                                    else if (Player.Left < Enemy.Right && Player.Right > Enemy.Left)
                                    {
                                        Acak2 = 1;
                                    }
                                }
                                else if (BatKi - 200 < Enemy2.Right && Player.Bottom - 200 < Enemy2.Top && Player.Left > Enemy2.Right)
                                {

                                    // ini kalo di sampingnya persis
                                    if (Player.Top > Enemy2.Bottom && Player.Bottom < Enemy2.Top)
                                    {
                                        Acak2 = 2;
                                    }
                                    // ini di atasnya persis
                                    else if (Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                    {
                                        Acak2 = 1;
                                    }
                                }
                                // ini kalo enemy di kanannya hewan
                                else if (Enemy.Right > Player.Left)
                                {
                                    Acak2 = 1;
                                }
                            }
                        }

                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    else if (MKanan == true && MBawa == true && MKiri == false)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Bottom - 100 < Enemy.Top || Player.Bottom - 100 < Enemy2.Top)
                            {
                                if (BatAt + 200 > Enemy.Left && Player.Bottom + 200 > Enemy.Top && Player.Right < Enemy.Right && BatAt + 200 > Enemy2.Left && Player.Bottom + 200 > Enemy2.Top && Player.Right < Enemy2.Right)
                                {
                                    if (Player.Bottom > Enemy.Top && Player.Top < Enemy.Bottom && Player.Bottom > Enemy2.Top && Player.Top < Enemy2.Bottom)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (Player.Left < Enemy.Right && Player.Right > Enemy.Left && Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                                else if (BatAt + 200 > Enemy.Left && Player.Bottom + 200 > Enemy.Top && Player.Right < Enemy.Right)
                                {
                                    if (Player.Bottom > Enemy.Top && Player.Top < Enemy.Bottom)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (Player.Left < Enemy.Right && Player.Right > Enemy.Left)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                                else if (BatAt + 200 > Enemy2.Left && Player.Bottom + 200 > Enemy2.Top && Player.Right < Enemy2.Right)
                                {
                                    if (Player.Bottom > Enemy2.Top && Player.Top < Enemy2.Bottom)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                                else if (Enemy.Right < Player.Left)
                                {
                                    Acak2 = 2;
                                }
                            }
                        }

                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 1)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    else if (MKanan == true && MBawa == false && MKiri == true)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Top - 100 > Enemy.Bottom && Player.Top - 100 > Enemy2.Bottom)
                            {
                                if (BatKi > Enemy.Right && BatKi > Enemy2.Right)
                                {
                                    Acak2 = 2;
                                }
                                else if (BatKi < Enemy.Left && BatKi < Enemy2.Left)
                                {
                                    Acak2 = 1;
                                }
                            }
                        }

                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }

                    }
                    else if (MKanan == true && MBawa == true && MKiri == true)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Top + 100 > Enemy.Bottom || Player.Top + 100 > Enemy2.Bottom)
                            {
                                // radius kalo musuh sampe dalam 200 pixel di samping kiri hewan, harus p.lef>en.rig(bawa) karna suppaya harus di kirinya
                                if ((BatKi - 00 < Enemy.Right && Player.Left > Enemy.Right && BatKi - 300 < Enemy2.Right && Player.Left > Enemy2.Right)
                                || BatKi - 00 < Enemy.Right && Player.Left > Enemy.Right || BatKi - 300 < Enemy2.Right && Player.Left > Enemy2.Right)
                                {
                                    Acak = Acak2 + 1; // maksudnya bukan 1
                                }
                                else if ((BatKi - 300 < Enemy.Right && Player.Left > Enemy.Right && BatKi + 300 > Enemy2.Left && Enemy2.Left > Player.Right)
                                    || (BatKi + 300 > Enemy.Left && Enemy.Left > Player.Right && BatKi - 300 < Enemy2.Right && Player.Left > Enemy2.Right))
                                {
                                    Acak = 3;
                                }
                                // ini sama dengan di atas syaratnya
                                else if ((BatKi + 300 > Enemy.Left && Enemy.Left > Player.Right && BatKi + 300 > Enemy2.Left && Enemy2.Left > Player.Right)
                                    || BatKi + 300 > Enemy.Left && Enemy.Left > Player.Right || BatKi + 300 > Enemy2.Left && Enemy2.Left > Player.Right)
                                {
                                    Acak = Acak2; // maksudnya tidak boleh 2
                                    if (Acak == 2)
                                    {
                                        Acak++;
                                    }
                                }
                                else if ((Player.Left < Enemy.Right && Player.Right > Enemy.Left && BatKi - 300 < Enemy2.Right && Player.Left > Enemy2.Right)
                                    || (Player.Left < Enemy2.Right && Player.Right > Enemy2.Left && BatKi - 300 < Enemy.Right && Player.Left > Enemy.Right))
                                {
                                    Acak = 2;
                                }
                                else if ((BatKi + 300 > Enemy.Left && Enemy.Left > Player.Right && Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                || (Player.Left < Enemy.Right && Player.Right > Enemy.Left && BatKi + 300 > Enemy2.Left && Enemy2.Left > Player.Right))
                                {
                                    Acak = 1;
                                }
                                else if ((Player.Left < Enemy.Right && Player.Right > Enemy.Left && Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                    || Player.Left < Enemy.Right && Player.Right > Enemy.Left || Player.Left < Enemy2.Right && Player.Right > Enemy2.Left)
                                {
                                    Acak = Acak2; // ini kalo bukan gerak depan
                                }
                            }
                        }

                        if (Acak == 1)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, +ABLeft, ref kiri, ref tandaKiri, ref MajuKi, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +ABTop, -ABLeft, ref kanan, ref tandaKanan, ref MajuKa, ref MajuBa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 3)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }

                    else if (MKiri == false && MBawa == false && MKanan == false)
                    {
                        Count++;
                        if (Count > LimAtas / 8)
                        {
                            MajuBa = false;
                            MajuAt = true; ;
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = true;
                            Count = 0;
                        }
                    }
                }
                #endregion

                #region Kanan

                // sama dengan di atas
                else if (MajuKa == true && Mark == true)
                {
                    foreach (PictureBox Pla in ListP)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Medium == true)
                            {
                                //if (Pla.Top < Player.Bottom && Pla.Bottom > Player.Top && ListLabel[tandaKanan].Left > Pla.Right && Pla.Left > Player.Right)
                                if (((Pla.Top < Player.Bottom && Player.Bottom - Pla.Top <= 75) || (Pla.Bottom > Player.Top && Player.Top - Pla.Bottom >= 75)) && ListLabel[tandaKanan].Left > Pla.Right && Pla.Left > Player.Right)
                                {
                                    if ((ListLabel[tandaatas].Right + 70 > Pla.Left && ListLabel[tandabawa].Right + 70 > Pla.Right) || Pla.Right - Player.Left <= 200)
                                    {
                                        MKanan = false;
                                        MAtas = false;
                                        MBawa = false;
                                        Hitport = true;
                                    }
                                }
                            }
                            if (Hard == true)
                            {
                                if (((Pla.Top < Player.Bottom && Player.Bottom - Pla.Top <= 200) || (Pla.Bottom > Player.Top && Player.Top - Pla.Bottom <= 200)) && ListLabel[tandaKanan].Left > Pla.Right && Pla.Left > Player.Right)
                                {
                                    if ((ListLabel[tandaatas].Right + 70 > Pla.Left && ListLabel[tandabawa].Right + 70 > Pla.Right) || Pla.Right - Player.Left <= 300)
                                    {
                                        MKanan = false;
                                        MAtas = false;
                                        MBawa = false;
                                        Hitport = true;
                                    }
                                }
                            }
                        }
                    }
                    if (MAtas == true && MKanan == false && MBawa == false)
                    {
                        BerBalik(ref Count, Spd, Player, +KKTop, +KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    else if (MBawa == true && MKanan == false && MAtas == false)
                    {
                        BerBalik(ref Count, Spd, Player, -KKTop, +KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                    }
                    else if (MBawa == true && MKanan == true && MAtas == false)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Right - 100 < Enemy.Right || Player.Right - 100 < Enemy2.Right)
                            {
                                if (BatKi - 200 < Enemy.Bottom && Player.Right + 200 > Enemy.Left && Player.Top > Enemy.Bottom && BatKi - 200 < Enemy2.Bottom && Player.Right + 200 > Enemy2.Left && Player.Top > Enemy2.Bottom)
                                {
                                    if (Player.Right > Enemy.Left && Player.Left < Enemy.Right && Player.Right > Enemy2.Left && Player.Left < Enemy2.Right)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (Player.Top < Enemy.Bottom && Player.Bottom > Enemy.Top && Player.Top < Enemy2.Bottom && Player.Bottom > Enemy2.Top)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                                else if (BatKi - 200 < Enemy2.Bottom && Player.Right + 200 > Enemy2.Left && Player.Top > Enemy2.Bottom)
                                {
                                    if (Player.Right > Enemy2.Left && Player.Left < Enemy2.Right)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (Player.Top < Enemy2.Bottom && Player.Bottom > Enemy2.Top)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                                else if (BatKi - 200 < Enemy.Bottom && Player.Right + 200 > Enemy.Left && Player.Top > Enemy.Bottom)
                                {
                                    if (Player.Right > Enemy.Left && Player.Left < Enemy.Right)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (Player.Top < Enemy.Bottom && Player.Bottom > Enemy.Top)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                                else if (Enemy.Bottom > Player.Top)
                                {
                                    Acak2 = 2;
                                }
                            }
                        }

                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -KKTop, +KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 2)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    else if (MBawa == false && MKanan == true && MAtas == true)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Right - 100 < Enemy.Right || Player.Right - 100 < Enemy2.Right)
                            {
                                if (BatKi - 200 < Enemy.Bottom && Player.Right + 200 > Enemy.Left && Player.Top > Enemy2.Bottom && BatKi - 200 < Enemy2.Bottom && Player.Right + 200 > Enemy2.Left && Player.Top > Enemy2.Bottom)
                                {
                                    if (Player.Right > Enemy.Left && Player.Left < Enemy.Right && Player.Right > Enemy2.Left && Player.Left < Enemy2.Right)
                                    {
                                        Acak2 = 2;
                                    }
                                    else if (Player.Top < Enemy.Bottom && Player.Bottom > Enemy.Top && Player.Top < Enemy2.Bottom && Player.Bottom > Enemy2.Top)
                                    {
                                        Acak2 = 1;
                                    }
                                }
                                else if (BatKi - 200 < Enemy2.Bottom && Player.Right + 200 > Enemy2.Left && Player.Top > Enemy2.Bottom)
                                {
                                    if (Player.Right > Enemy2.Left && Player.Left < Enemy2.Right)
                                    {
                                        Acak2 = 2;
                                    }
                                    else if (Player.Top < Enemy2.Bottom && Player.Bottom > Enemy2.Top)
                                    {
                                        Acak2 = 1;
                                    }
                                }
                                else if (BatKi - 200 < Enemy.Bottom && Player.Right + 200 > Enemy.Left && Player.Top > Enemy.Bottom)
                                {
                                    if (Player.Right > Enemy.Left && Player.Left < Enemy.Right)
                                    {
                                        Acak2 = 2;
                                    }
                                    else if (Player.Top < Enemy.Bottom && Player.Bottom > Enemy.Top)
                                    {
                                        Acak2 = 1;
                                    }
                                }
                                else if (Enemy.Bottom > Player.Top)
                                {
                                    Acak2 = 1;
                                }
                            }
                        }

                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +KKTop, +KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak2 == 1)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }
                    else if (MBawa == true && MKanan == false && MAtas == true)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Right - 100 < Enemy.Right && Player.Right - 100 < Enemy2.Right)
                            {
                                if (BatAt > Enemy.Bottom && BatAt > Enemy2.Bottom)
                                {
                                    Acak2 = 1;
                                }
                                else if (BatAt < Enemy.Top && BatAt < Enemy2.Top)
                                {
                                    Acak2 = 2;
                                }
                            }
                        }

                        if (Acak2 == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -KKTop, +KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);

                        }
                        if (Acak2 == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +KKTop, +KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                    }
                    else if (MBawa == true && MKanan == true && MAtas == true)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Player.Right - 100 < Enemy.Right || Player.Right - 100 < Enemy2.Right)
                            {
                                if ((BatAt - 200 < Enemy.Bottom && Player.Top > Enemy.Bottom && BatAt - 200 < Enemy2.Bottom && Player.Top > Enemy2.Bottom)
                                    || BatAt - 200 < Enemy.Bottom && Player.Top > Enemy.Bottom || BatAt - 200 < Enemy2.Bottom && Player.Top > Enemy2.Bottom)
                                {
                                    Acak = Acak2 + 1;
                                }
                                else if ((BatAt - 200 < Enemy.Bottom && Player.Top > Enemy.Bottom && BatAt + 200 > Enemy2.Top && Enemy2.Top > Player.Bottom)
                                    || (BatAt + 200 > Enemy.Top && Enemy.Top > Player.Bottom && BatAt + 200 > Enemy2.Top && BatAt - 200 < Enemy2.Bottom && Player.Top > Enemy2.Bottom))
                                {
                                    Acak = 3;
                                }
                                else if ((BatAt + 200 > Enemy.Top && Enemy.Top > Player.Bottom && BatAt + 200 > Enemy2.Top && Enemy2.Top > Player.Bottom)
                                    || BatAt + 200 > Enemy.Top && Enemy.Top > Player.Bottom || BatAt + 200 > Enemy2.Top && Enemy2.Top > Player.Bottom)
                                {
                                    Acak = Acak2;
                                    if (Acak == 2)
                                    {
                                        Acak++;
                                    }
                                }
                                else if ((Player.Right > Enemy.Left && Player.Left < Enemy.Right && BatAt - 200 < Enemy2.Bottom && Player.Top > Enemy2.Bottom) ||
                                    (BatAt - 200 < Enemy.Bottom && Player.Top > Enemy.Bottom && Player.Right > Enemy2.Left && Player.Left < Enemy2.Right))
                                {
                                    Acak = 2;
                                }
                                else if ((Player.Right > Enemy.Left && Player.Left < Enemy.Right && BatAt + 200 > Enemy2.Top && Enemy2.Top > Player.Bottom)
                                    || (BatAt + 200 > Enemy.Top && Enemy.Top > Player.Bottom && Player.Right > Enemy2.Left && Player.Left < Enemy2.Right))
                                {
                                    Acak = 1;
                                }
                                else if ((Player.Right > Enemy.Left && Player.Left < Enemy.Right && Player.Right > Enemy2.Left && Player.Left < Enemy2.Right)
                                    || Player.Right > Enemy.Left && Player.Left < Enemy.Right || Player.Right > Enemy2.Left && Player.Left < Enemy2.Right)
                                {
                                    Acak = Acak2;
                                }
                            }
                        }
                        if (Acak == 1)
                        {
                            BerBalik(ref Count, Spd, Player, -KKTop, +KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 2)
                        {
                            BerBalik(ref Count, Spd, Player, +KKTop, +KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKa, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        if (Acak == 3)
                        {
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = false;
                            kanan = tandaKanan;
                            atas = tandaatas;
                            bawa = tandabawa;
                            kiri = tandaKiri;
                        }
                    }

                    else if (MBawa == false && MKanan == false && MAtas == false)
                    {
                        Count++;
                        if (Count > LimKiri / 8)
                        {
                            MajuKa = false;
                            MajuKi = true;
                            BerBawah = true;
                            BerAtas = true;
                            BerKanan = true;
                            BerKiri = true;
                            Mark = true;
                            Count = 0;
                        }
                    }
                }

                #endregion

                #region Kiri
                // sama dengan di atas
                else if (MajuKi == true)
                {
                    foreach (PictureBox Pla in ListP)
                    {
                        if (Easy == true || Medium == true || Hard == true)
                        {
                            if (Medium == true)
                            {
                                if (((Pla.Top < Player.Bottom && Player.Bottom - Pla.Top <= 75) || (Pla.Bottom > Player.Top && Player.Bottom - Pla.Top <= 75)) && ListLabel[tandaKiri].Right < Pla.Right && Pla.Right < Player.Left)
                                {
                                    if ((ListLabel[tandaatas].Left - 70 < Pla.Right && ListLabel[tandabawa].Left - 70 < Pla.Right) || Player.Right - Pla.Left <= 200)
                                    {
                                        MKiri = false;
                                        MBawa = false;
                                        MAtas = false;
                                        Hitport = true;
                                    }
                                }
                            }

                            if (Hard == true)
                            {

                                if (((Pla.Top < Player.Bottom && Player.Bottom - Pla.Top <= 200) || (Pla.Bottom > Player.Top && Player.Top - Pla.Bottom <= 200)) && ListLabel[tandaKiri].Right < Pla.Right && Pla.Right < Player.Left)
                                {
                                    if ((ListLabel[tandaatas].Left - 70 < Pla.Right && ListLabel[tandabawa].Left - 70 < Pla.Right) || Player.Right - Pla.Left <= 300)
                                    {
                                        MKiri = false;
                                        MBawa = false;
                                        MAtas = false;
                                        Hitport = true;
                                    }
                                }
                            }
                        }
                    }

                    if (Mark == true)
                    {
                        if (MBawa == true && MKiri == false && MAtas == false)
                        {
                            BerBalik(ref Count, Spd, Player, -KKTop, -KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        else if (MAtas == true && MKiri == false && MBawa == false)
                        {
                            BerBalik(ref Count, Spd, Player, +KKTop, -KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                        }
                        else if (MBawa == true && MKiri == true && MAtas == false)
                        {
                            if (Easy == true || Medium == true || Hard == true)
                            {
                                if (Player.Left + 100 > Enemy.Right || Player.Left + 100 > Enemy2.Right)
                                {
                                    if (BatAt - 200 < Enemy.Bottom && Player.Left - 200 < Enemy.Right && Player.Top > Enemy.Bottom && BatAt - 200 < Enemy2.Bottom && Player.Left - 200 < Enemy2.Right && Player.Top > Enemy2.Bottom)
                                    {
                                        if (Player.Right > Enemy.Left && Player.Left < Enemy.Right && Player.Right > Enemy2.Left && Player.Left < Enemy2.Right)
                                        {
                                            Acak2 = 2;
                                        }
                                        else if (Player.Top < Enemy.Bottom && Player.Bottom > Enemy.Top && Player.Top < Enemy2.Bottom && Player.Bottom > Enemy2.Top)
                                        {
                                            Acak2 = 1;
                                        }
                                    }
                                    else if (BatAt - 200 < Enemy2.Bottom && Player.Left - 200 < Enemy2.Right && Player.Top > Enemy2.Bottom)
                                    {
                                        if (Player.Right > Enemy2.Left && Player.Left < Enemy2.Right)
                                        {
                                            Acak2 = 2;
                                        }
                                        else if (Player.Top < Enemy2.Bottom && Player.Bottom > Enemy2.Top)
                                        {
                                            Acak2 = 1;
                                        }
                                    }
                                    else if (BatAt - 200 < Enemy.Bottom && Player.Left - 200 < Enemy.Right && Player.Top > Enemy.Bottom)
                                    {
                                        if (Player.Right > Enemy.Left && Player.Left < Enemy.Right)
                                        {
                                            Acak2 = 2;
                                        }
                                        else if (Player.Top < Enemy.Bottom && Player.Bottom > Enemy.Top)
                                        {
                                            Acak2 = 1;
                                        }
                                    }
                                    else if (Enemy.Right > Player.Left)
                                    {
                                        Acak2 = 1;
                                    }
                                }
                            }
                            if (Acak2 == 1)
                            {
                                BerBalik(ref Count, Spd, Player, -KKTop, -KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak2 == 2)
                            {
                                BerBawah = true;
                                BerAtas = true;
                                BerKanan = true;
                                BerKiri = true;
                                Mark = false;
                                kanan = tandaKanan;
                                atas = tandaatas;
                                bawa = tandabawa;
                                kiri = tandaKiri;
                            }
                        }
                        else if (MBawa == false && MKiri == true && MAtas == true)
                        {
                            if (Easy == true || Medium == true || Hard == true)
                            {
                                if (Player.Left + 100 > Enemy.Right || Player.Left + 100 > Enemy2.Right)
                                {
                                    if (BatAt - 200 < Enemy.Bottom && Player.Left - 200 < Enemy.Right && Player.Top > Enemy.Bottom && BatAt - 200 < Enemy2.Bottom && Player.Left - 200 < Enemy2.Right && Player.Top > Enemy2.Bottom)
                                    {
                                        if (Player.Right > Enemy.Left && Player.Left < Enemy.Right && Player.Right > Enemy2.Left && Player.Left < Enemy2.Right)
                                        {
                                            Acak2 = 1;
                                        }
                                        else if (Player.Top < Enemy.Bottom && Player.Bottom > Enemy.Top && Player.Top < Enemy2.Bottom && Player.Bottom > Enemy2.Top)
                                        {
                                            Acak2 = 2;
                                        }
                                    }
                                    else if (BatAt - 200 < Enemy2.Bottom && Player.Left - 200 < Enemy2.Right && Player.Top > Enemy2.Bottom)
                                    {
                                        if (Player.Right > Enemy2.Left && Player.Left < Enemy2.Right)
                                        {
                                            Acak2 = 1;
                                        }
                                        else if (Player.Top < Enemy2.Bottom && Player.Bottom > Enemy2.Top)
                                        {
                                            Acak2 = 2;
                                        }
                                    }
                                    else if (BatAt - 200 < Enemy.Bottom && Player.Left - 200 < Enemy.Right && Player.Top > Enemy.Bottom)
                                    {
                                        if (Player.Right > Enemy.Left && Player.Left < Enemy.Right)
                                        {
                                            Acak2 = 1;
                                        }
                                        else if (Player.Top < Enemy.Bottom && Player.Bottom > Enemy.Top)
                                        {
                                            Acak2 = 2;
                                        }
                                    }
                                    else if (Enemy.Right > Player.Left)
                                    {
                                        Acak2 = 2;
                                    }
                                }
                            }

                            if (Acak2 == 2)
                            {
                                BerBalik(ref Count, Spd, Player, +KKTop, -KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak2 == 1)
                            {
                                BerBawah = true;
                                BerAtas = true;
                                BerKanan = true;
                                BerKiri = true;
                                Mark = false;
                                kanan = tandaKanan;
                                atas = tandaatas;
                                bawa = tandabawa;
                                kiri = tandaKiri;

                            }
                        }
                        else if (MBawa == true && MKiri == false && MAtas == true)
                        {
                            if (Easy == true || Medium == true || Hard == true)
                            {
                                if (Player.Right - 100 < Enemy.Right || Player.Right - 100 < Enemy2.Right)
                                {
                                    if (BatAt > Enemy.Bottom && BatAt > Enemy2.Bottom)
                                    {
                                        Acak2 = 1;
                                    }
                                    else if (BatAt < Enemy.Top && BatAt < Enemy2.Top)
                                    {
                                        Acak2 = 2;
                                    }
                                    else if ((BatAt < Enemy.Top && BatAt > Enemy2.Bottom) || (BatAt > Enemy.Bottom && BatAt < Enemy2.Top))
                                    {
                                        Acak2 = 0;
                                        MajuKi = false;
                                        MajuKa = true;
                                        BerBawah = false;
                                        BerAtas = false;
                                        BerKanan = false;
                                        BerKiri = false;
                                        Mark = true;
                                    }
                                }
                            }

                            if (Acak2 == 1)
                            {
                                BerBalik(ref Count, Spd, Player, -KKTop, -KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak2 == 2)
                            {
                                BerBalik(ref Count, Spd, Player, +KKTop, -KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                        }
                        else if (MBawa == true && MKiri == true && MAtas == true)
                        {
                            if (Easy == true || Medium == true || Hard == true)
                            {
                                if (Player.Right - 100 < Enemy.Right || Player.Right - 100 < Enemy2.Right)
                                {
                                    if ((BatAt - 200 < Enemy.Bottom && Player.Top > Enemy.Bottom && BatAt - 200 < Enemy2.Bottom && Player.Top > Enemy2.Bottom)
                                        || BatAt - 200 < Enemy.Bottom && Player.Top > Enemy.Bottom || BatAt - 200 < Enemy2.Bottom && Player.Top > Enemy2.Bottom)
                                    {
                                        Acak = Acak2 + 1;
                                    }
                                    else if ((BatAt - 200 < Enemy.Bottom && Player.Top > Enemy.Bottom && BatAt + 200 > Enemy2.Top && Enemy2.Top > Player.Bottom)
                                        || (BatAt + 200 > Enemy.Top && Enemy.Top > Player.Bottom && BatAt + 200 > Enemy2.Top && BatAt - 200 < Enemy2.Bottom && Player.Top > Enemy2.Bottom))
                                    {
                                        Acak = 3;
                                    }
                                    else if ((BatAt + 200 > Enemy.Top && Enemy.Top > Player.Bottom && BatAt + 200 > Enemy2.Top && Enemy2.Top > Player.Bottom)
                                        || BatAt + 200 > Enemy.Top && Enemy.Top > Player.Bottom || BatAt + 200 > Enemy2.Top && Enemy2.Top > Player.Bottom)
                                    {
                                        Acak = Acak2;
                                        if (Acak == 2)
                                        {
                                            Acak++;
                                        }
                                    }
                                    else if ((Player.Right > Enemy.Left && Player.Left < Enemy.Right && BatAt - 200 < Enemy2.Bottom && Player.Top > Enemy2.Bottom) ||
                                        (BatAt - 200 < Enemy.Bottom && Player.Top > Enemy.Bottom && Player.Right > Enemy2.Left && Player.Left < Enemy2.Right))
                                    {
                                        Acak = 2;
                                    }
                                    else if ((Player.Right > Enemy.Left && Player.Left < Enemy.Right && BatAt + 200 > Enemy2.Top && Enemy2.Top > Player.Bottom)
                                        || (BatAt + 200 > Enemy.Top && Enemy.Top > Player.Bottom && Player.Right > Enemy2.Left && Player.Left < Enemy2.Right))
                                    {
                                        Acak = 1;
                                    }
                                    else if ((Player.Right > Enemy.Left && Player.Left < Enemy.Right && Player.Right > Enemy2.Left && Player.Left < Enemy2.Right)
                                        || Player.Right > Enemy.Left && Player.Left < Enemy.Right || Player.Right > Enemy2.Left && Player.Left < Enemy2.Right)
                                    {
                                        Acak = Acak2;
                                    }
                                }
                            }

                            if (Acak == 1)
                            {
                                BerBalik(ref Count, Spd, Player, -KKTop, -KKLeft, ref bawa, ref tandabawa, ref MajuBa, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak == 2)
                            {
                                BerBalik(ref Count, Spd, Player, +KKTop, -KKLeft, ref atas, ref tandaatas, ref MajuAt, ref MajuKi, ref BerKanan, ref BerKiri, ref BerAtas, ref BerBawah, ref Mark, ref tandaKanan, ref tandaKiri, ref tandabawa, ref tandaatas, ref kanan, ref bawa, ref kiri, ref atas);
                            }
                            if (Acak == 3)
                            {
                                BerBawah = true;
                                BerAtas = true;
                                BerKanan = true;
                                BerKiri = true;
                                Mark = false;
                                kanan = tandaKanan;
                                atas = tandaatas;
                                bawa = tandabawa;
                                kiri = tandaKiri;
                            }

                        }
                        else if (MBawa == false && MKiri == false && MAtas == false)
                        {
                            Count++;
                            if (Count > LimKiri / 8)
                            {
                                MajuKi = false;
                                MajuKa = true;
                                BerBawah = true;
                                BerAtas = true;
                                BerKanan = true;
                                BerKiri = true;
                                Mark = true;
                                Count = 0;
                            }
                        }
                    }
                }
            }
                #endregion
            if (MajuAt == true)
            {
                Player.Top -= Spd; // kecepatannya
                // kembalikan selisihnya seperti semula
                kecilbawa = asd;
                kecilkanan = asd;
                kecilkiri = asd;
            }
            else if (MajuBa == true)
            {
                Player.Top += Spd;
                kecilatas = asd;
                kecilkanan = asd;
                kecilkiri = asd;
            }
            else if (MajuKi == true)
            {
                Player.Left -= Spd;
                kecilbawa = asd;
                kecilkanan = asd;
                kecilatas = asd;

                // ini supaya animasi arah jalannya benar
                if (HitKiri == 0)
                {
                    HitKiri++;
                    HitKanan = 0;
                    Player.Image = Ki.Image;
                    ActMark = 1;
                }

                // ini kalo kenna label supaya kelihatan loncat
                for (int i = 0; i < ListLabel.Count; i++)
                {
                    if (Player.Bounds.IntersectsWith(ListLabel[i].Bounds))
                    {
                        Player.Image = JumpKi.Image;
                        ActMark = 3;
                        HitKiri = 0;
                    }
                }
            }
            else if (MajuKa == true)
            {
                Player.Left += Spd;
                kecilbawa = asd;
                kecilatas = asd;
                kecilkiri = asd;
                if (HitKanan == 0)
                {
                    HitKanan++;
                    HitKiri = 0;
                    Player.Image = Ka.Image;
                    ActMark = 2;
                }
                for (int i = 0; i < ListLabel.Count; i++)
                {
                    if (Player.Bounds.IntersectsWith(ListLabel[i].Bounds))
                    {
                        Player.Image = JumpKa.Image;
                        HitKanan = 0;
                        ActMark = 4;
                    }
                }
            }

            #endregion
        }
                    

        public void BerBalik(ref int Count, int Speed,PictureBox Player, int angkaTop, int angkaLeft, ref int Urut, ref int tandaUrut, ref bool Arah, ref bool Asal, ref bool BerKanan, ref bool BerKiri, ref bool BerAtas, ref bool BerBawah, ref bool Mark, ref int tandaKanan, ref int tandaKiri, ref int tandabawa, ref int tandaatas, ref int kanan, ref int bawa, ref int kiri, ref int atas)
        {
                Player.Top += angkaTop; // supaya buat di tengah
               Player.Left += angkaLeft; // supaya buat hewan jalan di tenga
                Urut = tandaUrut; // cth kiri == tanda kiri

            // arah jalannya, yang misnya belok dari maju ke kiri: arah itu kiri; asal itu maju
                Arah = true;
                Asal = false;

                BerBawah = true;
                BerAtas = true;
                BerKanan = true;
                BerKiri = true;
                Mark = false; // supaya nda belok terus di tempat yang sama
                kanan = tandaKanan;
                atas = tandaatas;
                bawa = tandabawa;
                kiri = tandaKiri;
                Count = 0; // ini untuk kembalikan pengaturan jalan buntu
        }
        public void CekBelok(ref bool BerKanan, ref bool BerKiri, ref bool BerAtas, ref bool BerBawah, ref int tandaKanan, ref int tandaKiri, ref int tandabawa, ref int tandaatas, ref int kanan, ref int bawa, ref int kiri, ref int atas, ref bool Mark)
        {
            // periksan kalo label yang terdekat dengan hewan sudah berubah atau tidak
            if(Mark == false &&(kanan != tandaKanan || kiri != tandaKiri ||  bawa != tandabawa || atas != tandaatas))
            {
                // samakan semuanya lalu Marknya dibenarkan supaya bisa berbelok
                kanan = tandaKanan;
                atas = tandaatas;
                bawa = tandabawa;
                kiri = tandaKiri;
                Mark = true;
            }
        }
        public void PemilihanAcak(int Acak, ref bool MajuKa, ref bool MajuKi, ref bool MajuBa, ref bool MajuAt, ref bool MAtas, ref bool MBawa, ref bool MKiri, ref bool MKanan)
        {
            // pengaturan gerakan acak ketika awal jalan
            // penjelasan:
            // misnya kalo 1
            // kalo bisa belok kiri belok kiri... selesai, kalo tdk cek bisa belok kanan ato tdk
            // kalo bisa belok kanan belok kanan... selesai, kalo tdk bisa cek bisa belok atas ato tdk
            // kalo bisa belok atas belok atas... selesai, kalo tdk bisa cek belok bawah bisa tao tdk
            // kalo bisa belok bawah
            // kalo tdk bisa semua tdk bergerak deh wkwk
            // untuk yang acaknya 2,3,4 prinsipnya sama dgn 1 tapi awalnya bukan kiri...
            if (Acak == 1)
            {
                if (MKiri == true)
                {
                    MajuKi = true;

                }
                else if (MKiri == false)
                {
                    if (MKanan == true)
                    {
                        MajuKa = true;

                    }
                    else if (MKanan == false)
                    {
                        if (MAtas == true)
                        {
                            MajuAt = true;

                        }
                        else if (MAtas == false)
                        {
                            if (MBawa == true)
                            {
                                MajuBa = true;
                                //MKiri = false;
                                //MKanan = false;
                                //MAtas = false;
                            }
                            else if (MBawa == false)
                            {

                            }
                        }
                    }
                }
            }
            else if (Acak == 2)
            {
                if (MBawa == true)
                {
                    MajuBa = true;
                    //MKiri = false;
                    //MKanan = false;
                    //MAtas = false;
                }
                else if (MBawa == false)
                {
                    if (MKiri == true)
                    {
                        MajuKi = true;

                    }
                    else if (MKiri == false)
                    {
                        if (MKanan == true)
                        {
                            MajuKa = true;

                        }
                        else if (MKanan == false)
                        {
                            if (MAtas == true)
                            {
                                MajuAt = true;

                            }
                            else if (MAtas == false)
                            {

                            }
                        }
                    }
                }
            }
            else if (Acak == 3)
            {
                if (MAtas == true)
                {
                    MajuAt = true;
                    //MKiri = false;
                    //MKanan = false;
                    //MBawa = false;
                }
                else if (MAtas == false)
                {
                    if (MBawa == true)
                    {
                        MajuBa = true;
                        //MKiri = false;
                        //MKanan = false;
                        //MAtas = false;
                    }
                    else if (MBawa == false)
                    {
                        if (MKiri == true)
                        {
                            MajuKi = true;
                            //MKanan = false;
                            //MAtas = false;
                            //MBawa = false;
                        }
                        else if (MKiri == false)
                        {
                            if (MKanan == true)
                            {
                                MajuKa = true;
                                //MKiri = false;
                                //MAtas = false;
                                //MBawa = false;
                            }
                            else if (MKanan == false)
                            {

                            }
                        }
                    }
                }
            }
            else
            {
                if (MKanan == true)
                {
                    MajuKa = true;
                    //MKiri = false;
                    //MAtas = false;
                    //MBawa = false;
                }
                else if (MKanan == false)
                {
                    if (MAtas == true)
                    {
                        MajuAt = true;
                        //MKiri = false;
                        //MKanan = false;
                        //MBawa = false;
                    }
                    else if (MAtas == false)
                    {
                        if (MBawa == true)
                        {
                            MajuBa = true;
                            //MKiri = false;
                            //MKanan = false;
                            //MAtas = false;
                        }
                        else if (MBawa == false)
                        {
                            if (MKiri == true)
                            {
                                MajuKi = true;
                                //MKanan = false;
                                //MAtas = false;
                                //MBawa = false;
                            }
                            else if (MKiri == false)
                            {

                            }
                        }
                    }
                }
            }
        }



        public void GerakKanan(int BatasForm, PictureBox Player, int btsKiri, ref int kecilatas, ref int kecilbawa, ref int kecilkanan, ref int kecilkiri, Panel Panel, Label Ujung, string Chara)
        {
            if (Player.ImageLocation != Chara)
            {
                Player.ImageLocation = Chara;
            }
            kecilkiri = asd;
            kecilatas = asd;
            kecilbawa = asd;
            for (int i = 0; i < ListLabel.Count; i++)
            {

                if (kecilkanan > (ListLabel[i].Left) - Player.Right && (ListLabel[i].Left) - Player.Right >= 0 && (ListLabel[i].Top) < Player.Bottom && (ListLabel[i].Bottom) > Player.Top && ListLabel[i].Visible == true)
                {
                    kecilkanan = (ListLabel[i].Left) - Player.Right;
                    tandaKanan = i;
                }

            }
            if ((ListLabel[tandaKanan].Left) <= Player.Right)
            {

            }
            else
            {
                int selik = (ListLabel[tandaKanan].Left) - Player.Right;
                if (selik < SPdPlayer && selik >= 0)
                {
                    if (Panel.Right > BatasForm)
                    {
                        if (Player.Left > btsKiri)
                        {
                            Panel.Left -= selik;
                            Player.Left += selik;
                        }
                        else
                        {

                            Player.Left += selik;
                        }
                    }
                    else
                    {
                        if (Player.Right < Ujung.Left)
                        {
                            Player.Left += selik;
                        }
                    }
                }
                else
                {
                    if (Panel.Right > BatasForm)
                    {
                        if (Player.Left + Panel.Left > btsKiri)
                        {
                            Panel.Left -= SPdPlayer;
                            Player.Left += SPdPlayer;
                        }
                        else
                        {
                            Player.Left += SPdPlayer;
                        }
                    }
                    else
                    {
                        if (Player.Right < Ujung.Left)
                        {
                            Player.Left += SPdPlayer;
                        }
                    }
                }
            }
        }

        public void GerakKiri(PictureBox Player, int btsKiri, ref int kecilatas, ref int kecilbawa, ref int kecilkanan, ref int kecilkiri, Panel Panel, Label Ujung, string Chara)
        {
            if (Player.ImageLocation != Chara)
            {
                Player.ImageLocation = Chara;
            }
            kecilatas = asd;
            kecilbawa = asd;
            kecilkanan = asd;

            for (int i = 0; i < ListLabel.Count; i++)
            {

                if (kecilkiri > Player.Left - (ListLabel[i].Right) && Player.Left - (ListLabel[i].Right) >= 0 && (ListLabel[i].Top) < Player.Bottom && (ListLabel[i].Bottom) > Player.Top && ListLabel[i].Visible == true)
                {
                    kecilkiri = Player.Left - (ListLabel[i].Right);
                    tandaKiri = i;
                }
            }
            if ((ListLabel[tandaKiri].Right) >= Player.Left)
            {

            }
            else
            {
                int selik = Player.Left - (ListLabel[tandaKiri].Right);
                if (selik <= SPdPlayer && selik >= 0)
                {
                    if (Panel.Left < 0)
                    {
                        if (Player.Left < btsKiri)
                        {
                            Panel.Left += selik;
                            Player.Left -= selik;
                        }
                        else
                        {
                            Player.Left -= selik;
                        }
                    }
                    else
                    {
                        if (Player.Left > Ujung.Right)
                        {
                            Player.Left -= selik;
                        }
                    }
                }
                else
                {
                    if (Panel.Left < 0)
                    {
                        if (Player.Left + Panel.Left < btsKiri)
                        {
                            Panel.Left += SPdPlayer;
                            Player.Left -= SPdPlayer;
                        }
                        else
                        {
                            Player.Left -= SPdPlayer;
                        }
                    }
                    else
                    {
                        if (Player.Left > Ujung.Right)
                        {
                            Player.Left -= SPdPlayer;
                        }
                    }
                }
            }
        }
        public void GerakBawah(int BatasForm, PictureBox Player, int btsAtas, ref int kecilatas, ref int kecilbawa, ref int kecilkanan, ref int kecilkiri, Panel Panel, Label Ujung)
        {
            kecilkiri = asd;
            kecilatas = asd;
            kecilkanan = asd;

            for (int i = 0; i < ListLabel.Count; i++)
            {

                if (kecilbawa > (ListLabel[i].Top) - Player.Bottom && (ListLabel[i].Top) - Player.Bottom >= 0 && (ListLabel[i].Left) < Player.Right && (ListLabel[i].Right) > Player.Left && ListLabel[i].Visible == true)
                {
                    kecilbawa = (ListLabel[i].Top) - Player.Bottom;
                    tandabawa = i;
                }
            }
            if ((ListLabel[tandabawa].Top) <= Player.Bottom)
            {

            }
            else
            {
                int selik = (ListLabel[tandabawa].Top) - Player.Bottom;
                if (selik < SPdPlayer && selik >= 0)
                {
                    if (Panel.Bottom > BatasForm)
                    {
                        if (Player.Top < btsAtas)
                        {
                            Player.Top += selik;

                        }
                        else
                        {
                            Panel.Top -= selik;
                            Player.Top += selik;
                        }
                    }
                    else
                    {
                        if (Player.Bottom < Ujung.Top)
                        {
                            Player.Top += selik;
                        }
                    }
                }
                else
                {
                    if (Panel.Bottom > BatasForm)
                    {
                        if (Player.Top < btsAtas)
                        {
                            Player.Top += SPdPlayer;
                        }
                        else
                        {
                            Panel.Top -= SPdPlayer;
                            Player.Top += SPdPlayer;
                        }
                    }
                    else
                    {
                        if (Player.Bottom < Ujung.Top)
                        {
                            Player.Top += SPdPlayer;
                        }
                    }
                }
            }
        }
        public void GerakAtas(PictureBox Player, int btsAtas, ref int kecilatas, ref int kecilbawa, ref int kecilkanan, ref int kecilkiri, Panel Panel, Label Ujung)
        {
            kecilkiri = asd;
            kecilbawa = asd;
            kecilkanan = asd;

            for (int i = 0; i < ListLabel.Count; i++)
            {

                if (kecilatas > Player.Top - (ListLabel[i].Bottom) && Player.Top - (ListLabel[i].Bottom) >= 0 && (ListLabel[i].Left) < Player.Right && (ListLabel[i].Right) > Player.Left && ListLabel[i].Visible == true)
                {
                    kecilatas = Player.Top - (ListLabel[i].Bottom);
                    tandaatas = i;
                }
            }
            if ((ListLabel[tandaatas].Bottom) >= Player.Top)
            {

            }
            else
            {
                int selik = Player.Top - (ListLabel[tandaatas].Bottom);
                if (selik < SPdPlayer && selik >= 0)
                {
                    if (Panel.Top < 0)
                    {
                        if (Player.Top > 125)
                        {
                            Player.Top -= selik;

                        }
                        else
                        {
                            Panel.Top += selik;
                            Player.Top -= selik;
                        }
                    }
                    else
                    {
                        if (Player.Top > Ujung.Bottom)
                        {
                            Player.Top -= selik;

                        }
                    }
                }
                else
                {
                    if (Panel.Top < 0)
                    {
                        if (Player.Top + Panel.Top > btsAtas)
                        {
                            Player.Top -= SPdPlayer;

                        }
                        else
                        {
                            Panel.Top += SPdPlayer;
                            Player.Top -= SPdPlayer;
                        }
                    }
                    else
                    {
                        if (Player.Top > Ujung.Bottom)
                        {
                            Player.Top -= SPdPlayer;

                        }
                    }
                }

            }
        }

        } 
    public class PowerUp
    {
        public int Pow { get; set; }
        public PictureBox Pict = new PictureBox();
        public Label lab = new Label();
    }
}