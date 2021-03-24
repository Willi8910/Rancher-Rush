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
    public partial class frmRegister : Form
    {
        frmLogin frmHome;
        public frmRegister()
        {
            InitializeComponent();
        }

        public frmRegister(frmLogin frmDaftar)
        {
            InitializeComponent();
            frmHome = frmDaftar;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string repass = txtRePass.Text;

            if (txtUsername.Text == "" || txtPassword.Text == "" || txtRePass.Text == "")
            {
                txtUsername.Text = "";
                txtPassword.Text = "";
                txtRePass.Text = "";
            }
            else
            {
                this.playerTableAdapter.RegisterInsert(username, password, "noName");
                this.playerTableAdapter.Fill(this.gameDataSet.Player);
            }
            this.Hide();
            frmHome.Show();
            
        }

        private void frmRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
