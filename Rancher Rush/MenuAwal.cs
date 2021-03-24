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
    public partial class Menu : Form
    {
        string username;
        public Menu()
        {
            InitializeComponent();
        }
        public Menu(string user)
        {
            InitializeComponent();
            username = user;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            string path = Application.StartupPath + "/" + playerTableAdapter.SelectChara(username);
            pBChara.ImageLocation = path;

            lblLevel.Text = playerTableAdapter.SelectLevel(username).ToString();
            lblName.Text = playerTableAdapter.SelectName(username);
        }

        private void btnSingle_Click(object sender, EventArgs e)
        {
            SinglePlayer frmOpen = new SinglePlayer(username);
            frmOpen.Owner = this;
            frmOpen.ShowDialog();
            this.Hide();
        }

        private void btnMulti_Click(object sender, EventArgs e)
        {
            int no = 1;
            if(no == 1)
            {
                panel1.Visible = true;
                no = 0;
            }
            else
            {
                panel1.Visible = false;
                no = 1;
            }
        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            MultiPlayerCreate frmOpen = new MultiPlayerCreate(username);
            frmOpen.Owner = this;
            this.Hide();
            frmOpen.ShowDialog();
           
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            frmJoin frmOpen = new frmJoin(username);
            frmOpen.Owner = this;
            this.Hide();
            frmOpen.ShowDialog();
            
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void Menu_Shown(object sender, EventArgs e)
        {
            string path = Application.StartupPath + "/" + playerTableAdapter.SelectChara(username);
            pBChara.ImageLocation = path;

            lblLevel.Text = playerTableAdapter.SelectLevel(username).ToString();
            lblName.Text = playerTableAdapter.SelectName(username);
        }
    }
}
