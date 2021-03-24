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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        string username;

        private void lblRegister_Click(object sender, EventArgs e)
        {
            frmRegister frmDaftar = new frmRegister(this);
            this.Hide();
            frmDaftar.ShowDialog();
        }

        private void lblRegister_MouseEnter(object sender, EventArgs e)
        {
            lblRegister.Height += 2;
            lblRegister.Width += 2;
            lblRegister.Top -= 1;
            lblRegister.Left -= 1;
        }

        private void lblRegister_MouseLeave(object sender, EventArgs e)
        {
            lblRegister.Height -= 2;
            lblRegister.Width -= 2;
            lblRegister.Top += 1;
            lblRegister.Left += 1;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            username = txtUsername.Text;
            if (playerTableAdapter.CekPass(username) == txtPassword.Text &&
                playerTableAdapter.Cek(txtUsername.Text) == txtUsername.Text)
            {
                if (playerTableAdapter.CekName(username) != "noName")
                {
                    Menu frmOpen = new Menu(username);
                    frmOpen.Owner = this;
                    this.Hide();
                    frmOpen.ShowDialog();

                }
                else if (playerTableAdapter.CekName(username) == "noName")
                {
                    SelectChara frmOpen = new SelectChara(username);
                    frmOpen.Owner = this;
                    this.Hide();
                    frmOpen.ShowDialog();
                }
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'gameDataSet.Player' table. You can move, or remove it, as needed.
            this.playerTableAdapter.Fill(this.gameDataSet.Player);

        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

   
    }
}
