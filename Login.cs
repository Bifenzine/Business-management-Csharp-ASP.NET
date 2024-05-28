using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace gestion_commercial_last_episode
{
    public partial class Login : Form
    {
        private connexion myConnection;
        public Login()
        {
            InitializeComponent();
            myConnection = new connexion();  // Instantiate Connexion class
        }

        private void ClearUser()
        {
            txtUser.Clear();
            
           
        }
        private void ClearPass()
        {
            txtPassword.Clear();


        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 fr = new Form1();
            fr.Show();
            
        }

        private void btnClearPassword_Click(object sender, EventArgs e)
        {
            ClearUser();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void btnClearUser_Click(object sender, EventArgs e)
        {
            ClearPass();
        }

        private void Login_Load_1(object sender, EventArgs e)
        {

        }
    }
}
