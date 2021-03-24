using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworksApi.TCP.CLIENT;

namespace Rancher_Rush
{
    public delegate void AddListView(string txt, Color col);
    public partial class frmGameMulti : Form
    {
        int Speed = 5;
        int Position = 0;
        int asd = 10000;
        int tandaatas = 0;
        int tandabawa = 0;
        int tandaKanan = 0;
        int tandaKiri = 0;


        Client Clien = new Client();
        List<Hewan> ListHewan = new List<Hewan>();
        int kecilAtas = 10000;
        int kecilBawa = 10000;
        int kecilKiri = 10000;
        int kecilKanan = 10000;

        List<Label> ListLabel;
        List<PowerUp> ListP = new List<PowerUp>();

        string IP; // oper IP
        string username; // oper username
        string user2;

        string path = Application.StartupPath + "/";
        Menu frmMenu;

        public frmGameMulti()
        {
            InitializeComponent();
        }

        public frmGameMulti(string user, string Ip)
        {
            InitializeComponent();
            IP = Ip;
            username = user;
            frmMenu = new Menu(username);
        }

        private void frmGameMulti_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            ListLabel = new List<Label> { l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12, l13,l14, lo, lop };

            Hewan Ani1 = new Hewan();
            Ani1.Animal = p2;
            ListHewan.Add(Ani1);
            Hewan Ani2 = new Hewan();
            Ani2.Animal = Player;
            ListHewan.Add(Ani2);
            Hewan Ani3 = new Hewan();
            Ani3.Animal = AltPlayer;
            ListHewan.Add(Ani3);
            Hewan Ani1a = new Hewan();
            Ani1a.Animal = s1;
            ListHewan.Add(Ani1a);
            Hewan Ani2a = new Hewan();
            Ani2a.Animal = s2;
            ListHewan.Add(Ani2a);
            Hewan Ani3a = new Hewan();
            Ani3a.Animal = s3;
            ListHewan.Add(Ani3a);

            Clien.EncryptionEnabled = false;
            Clien.OnClientConnected += new OnClientConnectedDelegate(Clien_OnClientConnected);
            Clien.OnClientConnecting += new OnClientConnectingDelegate(Clien_OnClientConnecting);
            Clien.OnClientDisconnected += new OnClientDisconnectedDelegate(Clien_OnClientDisconnected);
            Clien.OnClientError += new OnClientErrorDelegate(Clien_OnClientError);
            Clien.OnClientFileSending += new OnClientFileSendingDelegate(Clien_OnClientFileSending);
            Clien.OnDataReceived += new OnClientReceivedDelegate(Clien_OnDataReceived);

            if(username == "")
            {
                username = " ";
            }

            Clien.ClientName = username;
            Clien.ServerIp = IP;
            Clien.ServerPort = "90";
            Clien.Connect();
            Tim.Start();
        }

        #region Game

        public void DeletePower(int idx)
        {
            DeletePow(idx);
        }

        public void CreatePow(int Left, int Top, int Pow)
        {
            CrePow(Left, Top, Pow);
        }

        delegate void PlacePlayers(PictureBox pla, int le, int to);
        public void Placing(PictureBox pla, int le, int to)
        {
            if (pla.InvokeRequired)
            {
                PlacePlayers d = new PlacePlayers(Placing);
                this.Invoke(d, new object[] { pla, le, to });
            }
            else
            {
                pla.Left = le;
                pla.Top = to;
            }
        }


        delegate void SizePlayers(PictureBox pla, int widt, int heig);
        public void SizePlacing(PictureBox pla, int widt, int heig)
        {
            if (pla.InvokeRequired)
            {
                SizePlayers d = new SizePlayers(SizePlacing);
                this.Invoke(d, new object[] { pla, widt, heig });
            }
            else
            {
                pla.Width = widt;
                pla.Height = heig;
            }
        }


        delegate void DelPower(int Pow);
        public void DeletePow(int Pow)
        {
            if (Pan.InvokeRequired)
            {
                DelPower d = new DelPower(DeletePower);
                this.Invoke(d, new object[] { Pow });
            }
            else
            {
                Pan.Controls.Remove(ListP[Pow].Pict);
                ListP.RemoveAt(Pow);
            }
        }

        delegate void CrPower(int Left, int Top, int Pow);
        public void CrePow(int Left, int Top, int Pow)
        {
            if (Pan.InvokeRequired)
            {
                CrPower d = new CrPower(CrePow);
                this.Invoke(d, new object[] { Left, Top, Pow });
            }
            else
            {
                PictureBox Bla = new PictureBox();
                Bla.Left = Left;
                Bla.Top = Top;
                Bla.Width = 50;
                Bla.Height = 50;
                

                if (Pow == 1)
                {
                    Bla.Image = Properties.Resources.Speed;
                }
                if (Pow == 2)
                {
                    //Bla.Text = "Decrease";
                    Bla.Image = Properties.Resources.Slow;
                }
                if (Pow == 3)
                {
                    //Bla.Text = "Stop";
                    Bla.Image = Properties.Resources.Stop;
                }
                Bla.SizeMode = PictureBoxSizeMode.StretchImage;
                PowerUp Powee = new PowerUp();
                Powee.Pict = Bla;
                Powee.Pow = Pow;
                Pan.Controls.Add(Bla);
                ListP.Add(Powee);
            }
        }

        delegate void CatchAni(int idx);
        public void CatchAnimal(int idx)
        {
            if (Pan.InvokeRequired)
            {
                CatchAni d = new CatchAni(CatchAnimal);
                this.Invoke(d, new object[] { idx });
            }
            else
            {
                Pan.Controls.Remove(ListHewan[idx].Animal);
                ListHewan.RemoveAt(idx);
            }
        }

        delegate void Setter(PictureBox pb, int angka);
        public void SetPos(PictureBox pb ,int angka)
        {
            if (pb.InvokeRequired)
            {
                Setter d = new Setter(SetPos);
                this.Invoke(d, new object[] { pb, angka });
            }
            else
            {
                if (angka < 0 && pb.ImageLocation != characterTableAdapter1.PilihKiri(user2))
                {
                    pb.ImageLocation = characterTableAdapter1.PilihKiri(user2);
                }
               else if (angka > 0 && pb.ImageLocation != characterTableAdapter1.PilihKanan(user2))
                    {
                        pb.ImageLocation = characterTableAdapter1.PilihKanan(user2);
                    }
            }
        }
        #endregion

        #region Client
        

        void Clien_OnDataReceived(object Sender, ClientReceivedArguments R)
        {
            string bla = R.ReceivedData;
            string[] bila = bla.Split();
            if (bila[0] == "Pos")
            {
                Placing(pb2, int.Parse(bila[1]), int.Parse(bila[2]));
                Position = pb2.Left - int.Parse(bila[1]);
                SetPos(pb2, Position);
                //pb2.Left = int.Parse(bila[1]);
                //pb2.Top = int.Parse(bila[2]);
            }
            if (bila[0] == "RePos")
            {
                Placing(pb1, int.Parse(bila[1]), int.Parse(bila[2]));

                //pb1.Left = int.Parse(bila[1]);
                //pb1.Top = int.Parse(bila[2]);
            }
            if (bila[0] == "Hewan") //******
            {
                int idx = int.Parse(bila[1]);
                ListHewan[idx].CheckMark(int.Parse(bila[4]));
                Placing(ListHewan[idx].Animal, int.Parse(bila[2]), int.Parse(bila[3]));
                //ListHewan[idx].Animal.Left = int.Parse(bila[2]);
                //ListHewan[idx].Animal.Top = int.Parse(bila[3]);

            }
            if (bila[0] == "StHewan")//******
            {
                int idx = int.Parse(bila[1]);
                int Speed = int.Parse(bila[4]);
                ListHewan[idx].Decide(Speed);
                SizePlacing(ListHewan[idx].Animal, int.Parse(bila[2]), int.Parse(bila[3]));
                //ListHewan[idx].Animal.Width = int.Parse(bila[2]);
                //ListHewan[idx].Animal.Height = int.Parse(bila[3]);             
            }
            if (bila[0] == "Color")
            {
                if (bila[1] == "1")
                {
                    //pb1.BackColor = Color.Aqua;
                    pb1.ImageLocation = path + characterTableAdapter1.PilihPlayer(username);
                }
                else if (bila[1] == "2")
                {
                    //pb1.BackColor = Color.Red;
                    pb1.ImageLocation = path + characterTableAdapter1.PilihPlayer(username);
                }

                if (bila[2] == "1")
                {
                    user2 = bila[3];
                    //pb2.BackColor = Color.Aqua;
                    pb2.ImageLocation = path + characterTableAdapter1.PilihPlayer(user2);
                }
                else if (bila[2] == "2")
                {
                    user2 = bila[3];
                    //pb2.BackColor = Color.Red;
                    pb2.ImageLocation = path + characterTableAdapter1.PilihPlayer(user2);
                }
            }
            if (bila[0] == "SpeedPlayer")//******
            {
                Speed = int.Parse(bila[1]);
            }
            if (bila[0] == "SpawnPower")//******
            {
                CreatePow(int.Parse(bila[1]), int.Parse(bila[2]), int.Parse(bila[3]));
            }
            if (bila[0] == "DelPower")//******
            {
                DeletePower(int.Parse(bila[1]));
            }
            if (bila[0] == "Catch")//******
            {
                CatchAnimal(int.Parse(bila[1]));
            }
            if (bila[0] == "Crst")//******
            {
                MessageBox.Show(bila[1] + " " + bila[2] + " " + bila[3]);
            }

            if(bila[0] == "Lose")
            {
                MessageBox.Show("You Lose!");
                frmMenu.Show();
                System.Environment.Exit(System.Environment.ExitCode);
            }
        }

        void Clien_OnClientFileSending(object Sender, ClientFileSendingArguments R)
        {
            // use this if you want to send a file
        }

        void Clien_OnClientError(object Sender, ClientErrorArguments R)
        {
            MessageBox.Show(R.ErrorMessage);
        }

        void Clien_OnClientDisconnected(object Sender, ClientDisconnectedArguments R)
        {
            MessageBox.Show(R.EventMessage);
            //Clien.Connect();
        }

        void Clien_OnClientConnecting(object Sender, ClientConnectingArguments R)
        {
            MessageBox.Show(R.EventMessage);
        }

        void Clien_OnClientConnected(object Sender, ClientConnectedArguments R)
        {
            MessageBox.Show(R.EventMessage);
        }
        #endregion

        #region Gerak
        private void Tim_Tick(object sender, EventArgs e)
        {
            if (Clien.IsConnected)
            {
                Clien.Send("Pos " + pb1.Left + " " + pb1.Top);
            }
        }

        private void frmGameMulti_KeyDown(object sender, KeyEventArgs e)
        {
            int Minus = -Pan.Top;
            int Verti = -Pan.Left;

            if (e.KeyCode == Keys.Left)
            {
                GerakKiri(ListLabel, pb1, this.ClientSize.Width / 2, ref kecilAtas, ref kecilBawa, ref kecilKanan, ref kecilKiri, Pan, l1, path + characterTableAdapter1.PilihKiri(username));
            }
            if (e.KeyCode == Keys.Right)
            {
                GerakKanan(ListLabel, this.ClientSize.Width, pb1, this.ClientSize.Width / 2, ref kecilAtas, ref kecilBawa, ref kecilKanan, ref kecilKiri, Pan, l3, path + characterTableAdapter1.PilihKanan(username));
            }
            if (e.KeyCode == Keys.Up)
            {
                GerakAtas(ListLabel, pb1, this.ClientSize.Height / 2, ref kecilAtas, ref kecilBawa, ref kecilKanan, ref kecilKiri, Pan, l2);
            }
            if (e.KeyCode == Keys.Down)
            {
                GerakBawah(ListLabel, this.ClientSize.Height, pb1, this.ClientSize.Height / 2, ref kecilAtas, ref kecilBawa, ref kecilKanan, ref kecilKiri, Pan, l4);
            }
        }
        
        public void GerakKanan(List<Label> ListLabel, int BatasForm, PictureBox Player, int btsKiri, ref int kecilatas, ref int kecilbawa, ref int kecilkanan, ref int kecilkiri, Panel Panel, Label Ujung, string Chara)
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

                if (kecilkanan > (ListLabel[i].Left ) - Player.Right && (ListLabel[i].Left ) - Player.Right >= 0 && (ListLabel[i].Top ) < Player.Bottom && (ListLabel[i].Bottom ) > Player.Top && ListLabel[i].Visible == true)
                {
                    kecilkanan = (ListLabel[i].Left ) - Player.Right;
                    tandaKanan = i;
                }

            }
            if ((ListLabel[tandaKanan].Left ) <= Player.Right)
            {

            }
            else
            {
                int selik = (ListLabel[tandaKanan].Left ) - Player.Right;
                if (selik < Speed && selik >= 0)
                {
                    if (Panel.Right > BatasForm)
                    {
                        if (Player.Left > btsKiri)
                        {
                            Panel.Left -= selik;
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
                        if (Player.Left > btsKiri)
                        {
                            Panel.Left -= Speed;
                        }
                        else
                        {
                            Player.Left += Speed;
                        }
                    }
                    else
                    {
                        if (Player.Right < Ujung.Left)
                        {
                            Player.Left += Speed;
                        }
                    }
                }
            }
        }
        public void GerakKiri(List<Label> ListLabel, PictureBox Player, int btsKiri, ref int kecilatas, ref int kecilbawa, ref int kecilkanan, ref int kecilkiri, Panel Panel, Label Ujung, string Chara)
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

                if (kecilkiri > Player.Left - (ListLabel[i].Right ) && Player.Left - (ListLabel[i].Right ) >= 0 && (ListLabel[i].Top ) < Player.Bottom && (ListLabel[i].Bottom ) > Player.Top && ListLabel[i].Visible == true)
                {
                    kecilkiri = Player.Left - (ListLabel[i].Right );
                    tandaKiri = i;
                }
            }
            if ((ListLabel[tandaKiri].Right ) >= Player.Left)
            {

            }
            else
            {
                int selik = Player.Left - (ListLabel[tandaKiri].Right );
                if (selik <= Speed && selik >= 0)
                {
                    if (Panel.Left < 0)
                    {
                        if (Player.Left < btsKiri)
                        {
                            Panel.Left += selik;
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
                        if (Player.Left < btsKiri)
                        {
                            Panel.Left += Speed;
                        }
                        else
                        {
                            Player.Left -= Speed;
                        }
                    }
                    else
                    {
                        if (Player.Left > Ujung.Right)
                        {
                            Player.Left -= Speed;
                        }
                    }
                }
            }
        }
        public void GerakBawah(List<Label> ListLabel, int BatasForm, PictureBox Player, int btsAtas, ref int kecilatas, ref int kecilbawa, ref int kecilkanan, ref int kecilkiri, Panel Panel, Label Ujung)
        {
            kecilkiri = asd;
            kecilatas = asd;
            kecilkanan = asd;

            for (int i = 0; i < ListLabel.Count; i++)
            {

                if (kecilbawa > (ListLabel[i].Top ) - Player.Bottom && (ListLabel[i].Top ) - Player.Bottom >= 0 && (ListLabel[i].Left ) < Player.Right && (ListLabel[i].Right ) > Player.Left && ListLabel[i].Visible == true)
                {
                    kecilbawa = (ListLabel[i].Top ) - Player.Bottom;
                    tandabawa = i;
                }
            }
            if ((ListLabel[tandabawa].Top ) <= Player.Bottom)
            {

            }
            else
            {
                int selik = (ListLabel[tandabawa].Top ) - Player.Bottom;
                if (selik < Speed && selik >= 0)
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
                            Player.Top += Speed;
                        }
                        else
                        {
                            Panel.Top -= Speed;

                        }
                    }
                    else
                    {
                        if (Player.Bottom < Ujung.Top)
                        {
                            Player.Top += Speed;
                        }
                    }
                }
            }
        }
        public void GerakAtas(List<Label> ListLabel, PictureBox Player, int btsAtas, ref int kecilatas, ref int kecilbawa, ref int kecilkanan, ref int kecilkiri, Panel Panel, Label Ujung)
        {
            kecilkiri = 10000;
            kecilbawa = 10000;
            kecilkanan = 10000;

            for (int i = 0; i < ListLabel.Count; i++)
            {

                if (kecilatas > Player.Top - (ListLabel[i].Bottom ) && Player.Top - (ListLabel[i].Bottom ) >= 0 && (ListLabel[i].Left ) < Player.Right && (ListLabel[i].Right ) > Player.Left && ListLabel[i].Visible == true)
                {
                    kecilatas = Player.Top - (ListLabel[i].Bottom );
                    tandaatas = i;
                }
            }
            if ((ListLabel[tandaatas].Bottom ) >= Player.Top)
            {

            }
            else
            {
                int selik = Player.Top - (ListLabel[tandaatas].Bottom );
                if (selik < Speed && selik >= 0)
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
                        if (Player.Top > 125)
                        {
                            Player.Top -= Speed;

                        }
                        else
                        {
                            Panel.Top += Speed;

                        }
                    }
                    else
                    {
                        if (Player.Top > Ujung.Bottom)
                        {
                            Player.Top -= Speed;

                        }
                    }
                }
            }
        }

        public void Decide(int Speed, Hewan Ani)
        {
            #region Domba Putih
            if (Speed == 3)
            {
                // domba putih
                Ani.Ki.Image = Properties.Resources.D2Ki;
                Ani.Ka.Image = Properties.Resources.D2Ka;
                Ani.JumpKa.Image = Properties.Resources.D2JumpKa;
                Ani.JumpKi.Image = Properties.Resources.D2JumpKi;
                Ani.TransKi.Image = Properties.Resources.D2TransKi;
                Ani.TransKa.Image = Properties.Resources.D2TransKa;
                Ani.TransBack.Image = Properties.Resources.D2Back;
            }
            #endregion

            #region Domba Hitam
            else if (Speed == 4)
            {
                // domba hitam
                Ani.Ki.Image = Properties.Resources.D1Ki;
                Ani.Ka.Image = Properties.Resources.D1Ka;
                Ani.JumpKa.Image = Properties.Resources.D1JumpKa;
                Ani.JumpKi.Image = Properties.Resources.D1JumpKi;
                Ani.TransKi.Image = Properties.Resources.D1TransKi;
                Ani.TransKa.Image = Properties.Resources.D1TransKa;
                Ani.TransBack.Image = Properties.Resources.D1Back;
            }
            #endregion

            #region Kambing
            else if (Speed == 5)
            {
                // kambing
                Ani.Ki.Image = Properties.Resources.D3Ki;
                Ani.Ka.Image = Properties.Resources.D3Ka;
                Ani.JumpKa.Image = Properties.Resources.D3JumpKa;
                Ani.TransKi.Image = Properties.Resources.D3TransKi;
                Ani.TransKa.Image = Properties.Resources.D3TransKa;
                Ani.JumpKi.Image = Properties.Resources.D3JumpKi;
                Ani.TransBack.Image = Properties.Resources.D3Back;
            }
            #endregion

            #region Kuda
            else if (Speed == 7)
            {
                // kuda
                Ani.Ki.Image = Properties.Resources.D4Ki;
                Ani.Ka.Image = Properties.Resources.D4Ka;
                Ani.JumpKa.Image = Properties.Resources.D4JumpKa;
                Ani.JumpKi.Image = Properties.Resources.D4JumpKi;
                Ani.TransKi.Image = Properties.Resources.D4TransKi;
                Ani.TransKa.Image = Properties.Resources.D4TransKa;
                Ani.TransBack.Image = Properties.Resources.D4Back;
            }
            #endregion

            #region Ayam Raksasa
            else if (Speed == 6)
            {
                // chocobo(kalo yang main FF) ato supupunya ayam lah wkwk
                Ani.Ki.Image = Properties.Resources.D5Ki;
                Ani.Ka.Image = Properties.Resources.D5Ka;
                Ani.JumpKa.Image = Properties.Resources.D5JumpKa;
                Ani.JumpKi.Image = Properties.Resources.D5JumpKi;
                Ani.TransKi.Image = Properties.Resources.D5TransKi;
                Ani.TransKa.Image = Properties.Resources.D5TransKa;
                Ani.TransBack.Image = Properties.Resources.D5Back;
            }
            #endregion
        }

        public void CheckMark(int Changes, Hewan Ani)
        {
            if (Ani.Mark != Changes)
            {
                Ani.Mark = Changes;
                switch (Ani.Mark)
                {
                    case 1:
                        Ani.Animal.Image = Ani.Ki.Image;
                        break;
                    case 2:
                        Ani.Animal.Image = Ani.Ka.Image;
                        break;
                    case 3:
                        Ani.Animal.Image = Ani.JumpKi.Image;
                        break;
                    case 4:
                        Ani.Animal.Image = Ani.JumpKa.Image;
                        break;
                    case 5:
                        Ani.Animal.Image = Ani.TransKi.Image;
                        break;
                    case 6:
                        Ani.Animal.Image = Ani.TransKa.Image;
                        break;
                    case 7:
                        Ani.Animal.Image = Ani.TransBack.Image;
                        break;
                    default: Ani.Animal.Image = Ani.Ki.Image;
                        break;
                }
            }
        }
        #endregion

        private void frmGameMulti_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

    }
}
