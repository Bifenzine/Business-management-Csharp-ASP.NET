using Guna.UI2.WinForms;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace gestion_commercial_last_episode
{
    public partial class Familles : Form
    {
        private connexion myConnection;
        public Familles()
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
        private void ClearAll()
        {
            txtIntitule.Clear();
            

        }

        private void ChargerDonnees()
        {
            string query = "SELECT * FROM [dbo].[Familles]";
            SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection());


            SqlDataAdapter adapter = new SqlDataAdapter(cmd);


            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);


            dataGridIntitule.DataSource = dataTable;
        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            Familles Fm = new Familles();
            this.Hide();
            Form1 Clients = new Form1();
            Clients.Show();
        }

        private void guna2CircleButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            ClearAll();
            ChargerDonnees();

        }

        private void Familles_Load(object sender, EventArgs e)
        {
            ChargerDonnees();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Familles Fm = new Familles();
            Fm.Show();
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Form1 Clients = new Form1();
            Clients.Show();
            this.Hide();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Commandes cmd = new Commandes();
            cmd.Show();
            this.Hide();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DetailoCommand DtCmd = new DetailoCommand();
            DtCmd.Show();
            this.Hide();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Produits Prd = new Produits();
            Prd.Show();
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                myConnection.OpenConnection();

                string query = "INSERT INTO [dbo].[Familles] (Intitule) VALUES (@Intitule)";

                using (SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Intitule", txtIntitule.Text);
                 



                    cmd.ExecuteNonQuery();
                    ChargerDonnees();
                    ClearAll();
                }

                MessageBox.Show("Intitule added successfully!!! ;)");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding Intitule: -_- " + ex.Message);
            }
            finally
            {
                myConnection.CloseConnection();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            myConnection.OpenConnection();
            if (dataGridIntitule.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridIntitule.SelectedRows[0].Index;
                int IDFAm = Convert.ToInt32(dataGridIntitule.Rows[selectedIndex].Cells["IDFAm"].Value);

                // Get the updated values from the textboxes
                
                string intituleFm = txtIntitule.Text;
                



                string updateQuery = "UPDATE Familles SET intitule = @intitule WHERE IDFAm = @IDFAm";
                SqlCommand updateCommand = new SqlCommand(updateQuery, myConnection.GetConnection());
                updateCommand.Parameters.AddWithValue("@IDFAm", IDFAm);
                updateCommand.Parameters.AddWithValue("@intitule", intituleFm);


                try
                {
                    updateCommand.ExecuteNonQuery();
                    MessageBox.Show("Enregistrement modifié avec succès.");
                    ChargerDonnees(); // Refresh the DataGridView after modification
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la modification : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un enregistrement à modifier.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            myConnection.OpenConnection();
            if (dataGridIntitule.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridIntitule.SelectedRows[0].Index;
                int IDFAm = Convert.ToInt32(dataGridIntitule.Rows[selectedIndex].Cells["IDFAm"].Value);

                string deleteQuery = "DELETE FROM Familles WHERE IDFAm = @IDFAm";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, myConnection.GetConnection());
                deleteCommand.Parameters.AddWithValue("@IDFAm", IDFAm);

                try
                {
                    deleteCommand.ExecuteNonQuery();
                    MessageBox.Show("Enregistrement supprimé avec succès.");
                    ChargerDonnees(); // Refresh the DataGridView after deletion
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un enregistrement à supprimer.");
            }
            ChargerDonnees();
        }

        private void dataGridIntitule_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            myConnection.OpenConnection();
            if (dataGridIntitule.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridIntitule.SelectedRows[0];

                // Les données sont stockées dans les cellules de la ligne sélectionnée
                string intituleFm = selectedRow.Cells["intitule"].Value.ToString();
                
                // Afficher les données dans les TextBox
                txtIntitule.Text = intituleFm;
               
            }
        }
    }
}
