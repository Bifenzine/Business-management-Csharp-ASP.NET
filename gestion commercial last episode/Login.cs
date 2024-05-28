using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

            // Calculate the coordinates to center the form
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            int formWidth = this.Width;
            int formHeight = this.Height;

            int xPosition = (screenWidth - formWidth) / 2;
            int yPosition = (screenHeight - formHeight) / 2;

            // Set the StartPosition property to Manual
            this.StartPosition = FormStartPosition.Manual;

            // Set the form's location to the calculated coordinates
            this.Location = new Point(xPosition, yPosition);
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
            if (txtUser.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Missing information !!!");

            }
            else
            {
                try
                {
                    myConnection.OpenConnection();
                    SqlDataAdapter SDA = new SqlDataAdapter("Select Count (*) from [dbo].[User] where Nom_Us='" + txtUser.Text + "' and Password='" + txtPassword.Text + "'", myConnection.GetConnection());
                    DataTable dt = new DataTable();
                    SDA.Fill(dt);
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        this.Hide();
                        Form1 Clients = new Form1();
                        Clients.Show();
                        
                    }
                    else
                    {
                        MessageBox.Show("Wrong UserName or Password !!!");

                    }
                    myConnection.CloseConnection();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }

            }





            //this.Hide();
            //Form1 fr = new Form1();
            //fr.Show();
            
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
            txtPassword.UseSystemPasswordChar = true;
        }

        private void guna2CustomCheckBox1_Click(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox1.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }
    }
}
