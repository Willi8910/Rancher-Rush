using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

namespace Rancher_Rush
{
    public class Player
    {
        public string Name { set; get; }
        public int Left { set; get; }
        public int Top { set; get; }

        public PictureBox Gambar { set; get; }

        public Player()
        {
            Name = "No Name";
            Left = 0;
            Top = 0;
        }

        public Player(PictureBox Pict)
        {
            Name = "No Name";
            Left = 0;
            Top = 0;
            Gambar = Pict;
        }

        public Player(string Nama, int Guesss, int Winner, PictureBox View )
        {
            Name = Nama;
            Left = Guesss;
            Top = Winner;
            Gambar = View;
        }
    }
}
