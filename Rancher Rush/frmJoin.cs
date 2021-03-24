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
    public partial class frmJoin : Form
    {
        string User;
        public frmJoin()
        {
            InitializeComponent();
        }

        public frmJoin(string username)
        {
            InitializeComponent();
            User = username;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            frmGameMulti frmOpen = new frmGameMulti(User, textIp.Text);
            frmOpen.Owner = this;
            this.Hide();
            frmOpen.ShowDialog();
            
        }

        private void frmJoin_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }
    }
}
