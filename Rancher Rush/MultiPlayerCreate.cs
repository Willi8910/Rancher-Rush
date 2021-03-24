using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworksApi.TCP.SERVER;

namespace Rancher_Rush
{
    public delegate void UpdateListBox(string txt);
    public partial class MultiPlayerCreate : Form
    {

        Server Serv;
        bool TimRead = false;

        Random Nilai = new Random();

        Timer Bla = new Timer(); // untuk masukkan timer ke class
        Aksi Reaksi = new Aksi(); // class

        // untuk buat simpan binatang
        Domba Sheep = new Domba();
        Domba Ber = new Domba();
        Domba Bar = new Domba();
        Domba She = new Domba();
        Domba Be = new Domba();
        Domba Ba = new Domba();

        // listnya
        List<PictureBox> ListPort; // untuk simpan portal
        List<PictureBox> ListPict;
        List<Player> ListPl = new List<Player>();

        List<PictureBox> ListPlayer;
        List<PictureBox> Power;

        int indx = 0;
        string User; //oper username

        int wak = 0;
        Menu frmMenu;

        public MultiPlayerCreate()
        {
            InitializeComponent();
        }

        public MultiPlayerCreate(string username)
        {
            InitializeComponent();
            User = username;
            frmMenu = new Menu(User);
        }

        #region Method
        public void AddListBox(string txt)
        {
            if (listBoxInfo.InvokeRequired)
            {
                Invoke(new UpdateListBox(AddListBox), new object[] { txt });
            }
            else
            {
                listBoxInfo.Items.Add(txt);
            }
        }

        public void StartTimer()
        {
            Jalan.Start(); // timer_1 di pemanggilnya wkwk
            PowerUPp.Start();
            Waktu.Start();
        }
        #endregion

        #region Timer
        private void PowerUPp_Tick(object sender, EventArgs e)
        {
            Random Acak = new Random();
            int Powe = Acak.Next(1, 4);
            int Who = Acak.Next(0, Power.Count);
            Power[Who].Visible = true;

            PowerUp PPP = new PowerUp(); ;
            PPP.Pict = Power[Who];
            PPP.Pow = Powe;

            Reaksi.ListPower.Add(PPP);

            if (Powe == 1)
            {
                //Reaksi.ListPower[Who].Pow = 1;
                Power[Who].Text = "Increase";
                PPP.Pict.Image = Properties.Resources.Speed;
            }
            if (Powe == 2)
            {
                //Reaksi.ListPower[Who].Pow = 2;
                Power[Who].Text = "Decrease";
                PPP.Pict.Image = Properties.Resources.Slow;
            }
            if (Powe == 3)
            {
                //Reaksi.ListPower[Who].Pow = 3;
                Power[Who].Text = "Stop";
                PPP.Pict.Image = Properties.Resources.Stop;
            }

            Reaksi.SpawnPower = true;
            indx = Powe;
        }

        private void Jalan_Tick(object sender, EventArgs e)
        {
          
            Reaksi.Move(ListPort, ListPlayer);

            //// mulai jalan tiap hewan
            if (ListPl.Count == 2)
            {
                if(wak == 3)
                {
                    Serv.BroadCast("Lose You Lose");
                    Jalan.Stop();
                    Waktu.Stop();
                    wak = 0;
                }
                for (int i = 0; i < Reaksi.ListDomba.Count; i++)
                {

                    Serv.SendTo(ListPl[0].Name, "Hewan " + i + " " + Reaksi.ListDomba[i].Lamb.Left + " " + Reaksi.ListDomba[i].Lamb.Top + " " + Reaksi.ListDomba[i].Mark.ToString());
                    Serv.SendTo(ListPl[1].Name, "Hewan " + i + " " + Reaksi.ListDomba[i].Lamb.Left + " " + Reaksi.ListDomba[i].Lamb.Top + " " + Reaksi.ListDomba[i].Mark.ToString());

                    Serv.SendTo(ListPl[0].Name, "Pos " + ListPl[1].Left + " " + ListPl[1].Top);
                    Serv.SendTo(ListPl[1].Name, "Pos " + ListPl[0].Left + " " + ListPl[0].Top);

                    pb1.Left = ListPl[0].Left;
                    pb1.Top = ListPl[0].Top;

                    pb2.Left = ListPl[1].Left;
                    pb2.Top = ListPl[1].Top;
                }
                Serv.BroadCast("SpeedPlayer " + Reaksi.SPdPlayer);

                if (Reaksi.SpawnPower == true)
                {
                    Serv.BroadCast("SpawnPower " + Reaksi.ListPower[Reaksi.ListPower.Count - 1].Pict.Left + " " +
                        Reaksi.ListPower[Reaksi.ListPower.Count - 1].Pict.Top + " " + Reaksi.ListPower[Reaksi.ListPower.Count - 1].Pow);
                    Reaksi.SpawnPower = false;
                }
                if (Reaksi.DelPower == true)
                {
                    Serv.BroadCast("DelPower " + Reaksi.ind);
                    Reaksi.DelPower = false;
                }
                if (Reaksi.Catch == true)
                {
                    Serv.BroadCast("Catch " + Reaksi.CatchAni);
                    Reaksi.Catch = false;
                }
                if (Reaksi.ListDomba.Count == 0)
                {
                    Serv.BroadCast("Crst Selamat Kamu Menang");
                    Jalan.Stop();
                }
            }
        }

        private void Checker_Tick(object sender, EventArgs e)
        {
            if (TimRead == true)
            {
                StartTimer();
                Checker.Stop();
                TimRead = false;

            }
        }
        #endregion

        private void MultiPlayerCreate_Load(object sender, EventArgs e)
        {
            Power = new List<PictureBox> { o1, o2, o3, o4, o5 };

            //foreach (Label Lab in Power)
            //{
            //    PowerUp PPU = new PowerUp();
            //    PPU.lab = Lab;
            //    Reaksi.ListPower.Add(PPU);
            //}

            ListPict = new List<PictureBox> { pb1, pb2, pb3, pb4 };
            Reaksi.ListLabel = new List<Label> { l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12, l13,l14, lo, lop };
            ListPort = new List<PictureBox> { c1, c2, c3, c4 };

            //pemilihan hewan serta specnya
            Random Pilih = new Random(); // pilih random cepatnya
            int Cepat; // kecepatan lari hewan
            Cepat = 3;
            Reaksi.Hitung(Sheep, Cepat, p2, ListPort);
            Cepat = 4;
            Reaksi.Hitung(Ber, Cepat, Player, ListPort);
            Cepat = 5;
            Reaksi.Hitung(Bar, Cepat, AltPlayer, ListPort);
            Cepat = 3;
            Reaksi.Hitung(She, Cepat, s1, ListPort);
            Cepat = 4;
            Reaksi.Hitung(Be, Cepat, s2, ListPort);
            Cepat = 5;
            Reaksi.Hitung(Ba, Cepat, s3, ListPort);

            Checker.Start();

            ListPlayer = new List<PictureBox> { pb1, pb2 };
        }

        #region Server
        void Serv_OnServerError(object Sender, ErrorArguments R)
        {
            AddListBox(R.ErrorMessage);
            AddListBox(R.Exception);
        }

        void Serv_OnDataReceived(object Sender, ReceivedArguments R)
        {
            string bla = R.ReceivedData;
            string[] bila = bla.Split();
            if (bila[0] == "Pos")
            {
                if (R.Name == ListPl[0].Name)
                {
                    ListPl[0].Left = int.Parse(bila[1]);
                    ListPl[0].Top = int.Parse(bila[2]);
                }
                else if (R.Name == ListPl[1].Name)
                {
                    ListPl[1].Left = int.Parse(bila[1]);
                    ListPl[1].Top = int.Parse(bila[2]);
                }
            }
        }

        void Serv_OnClientDisconnected(object Sender, DisconnectedArguments R)
        {
            AddListBox(R.Name + " Has Disconnected");
            if (ListPl[0].Name == R.Name)
            {
                ListPl.RemoveAt(0);
            }
            else
            {
                ListPl.RemoveAt(1);
            }
            Jalan.Stop();
            PowerUPp.Stop();
            //Checker.Start();
        }

        void Serv_OnClientConnected(object Sender, ConnectedArguments R)
        {
            if (ListPl.Count < 2)
            {
                AddListBox(R.Name + " Has Connected");
                Random Acak = new Random();
                int bl1 = Acak.Next(ListPl.Count, 4);
                PictureBox View = new PictureBox();
                if (ListPl.Count == 0)
                {
                    View = pb1;
                }
                else
                {
                    View = pb2;
                }
                Player Play = new Player(R.Name, ListPict[bl1].Left, ListPict[bl1].Top, View);
                ListPl.Add(Play);
                Serv.SendTo(R.Name, "RePos " + ListPict[bl1].Left + " " + ListPict[bl1].Top);
                if (ListPl.Count == 2)
                {
                    for (int i = 0; i < Reaksi.ListDomba.Count; i++)
                    {
                        Serv.BroadCast("StHewan " + i + " " + Reaksi.ListDomba[i].Lamb.Width + " " + Reaksi.ListDomba[i].Lamb.Height + " " + Reaksi.ListDomba[i].Speed);
                    }
                    Serv.SendTo(ListPl[0].Name, "Color " + 1 + " " + 2 + " " + ListPl[1].Name);
                    Serv.SendTo(ListPl[1].Name, "Color " + 2 + " " + 1 + " " + ListPl[0].Name);
                    TimRead = true;
                }
            }
            else
            {
                Serv.SendTo(R.Name, "Sorry you cannot join the game");
            }

      
        }
#endregion

        private void MultiPlayerCreate_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Serv = new Server(textIp.Text, "90");
            Serv.EncryptionEnabled = false;
            Serv.OnClientConnected += new OnConnectedDelegate(Serv_OnClientConnected);
            Serv.OnClientDisconnected += new OnDisconnectedDelegate(Serv_OnClientDisconnected);
            Serv.OnDataReceived += new OnReceivedDelegate(Serv_OnDataReceived);
            Serv.OnServerError += new OnErrorDelegate(Serv_OnServerError);
            Serv.Start();

            frmGameMulti frmOpen = new frmGameMulti(User, textIp.Text);
            frmOpen.ShowDialog();
            
        }

        private void Waktu_Tick(object sender, EventArgs e)
        {
            wak += 1;
            
        }
    }
}
