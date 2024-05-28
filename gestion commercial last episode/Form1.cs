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
using TheArtOfDevHtmlRenderer.Adapters;
using static Jenga.Theme;


namespace gestion_commercial_last_episode
{
    public partial class Form1 : Form
    {
        private connexion myConnection;
        public Form1()
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

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void ChargerDonnees()
        {
            string query = "SELECT * FROM Clients";
            SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection());

           
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

         
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

         
            guna2DataGridView1.DataSource = dataTable;
        }

        private void ClearAll()
        {
            txtName.Clear();
            txtAdresse.Clear();
            txtCp.Clear();
            txtVille.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                myConnection.OpenConnection();

                string query = "INSERT INTO Clients (Nom, Adresse, Cp, Ville) VALUES (@Nom, @Adresse, @Cp, @Ville)";

                using (SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Nom", txtName.Text);
                    cmd.Parameters.AddWithValue("@Adresse", txtAdresse.Text);
                    cmd.Parameters.AddWithValue("@Cp", txtCp.Text);
                    cmd.Parameters.AddWithValue("@Ville", txtVille.Text);

                    cmd.ExecuteNonQuery();
                    ChargerDonnees();
                    ClearAll();
                }

                MessageBox.Show("Client added successfully!!! ;)");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding client: -_- " + ex.Message);
            }
            finally
            {
                myConnection.CloseConnection();
            }

        }

        int id;

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            id = int.Parse(guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            txtName.Text = guna2DataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtAdresse.Text = guna2DataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            txtCp.Text = guna2DataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtVille.Text = guna2DataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            ChargerDonnees();
            ClearAll();
                
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            // Open the database connection when the form loads
            myConnection.OpenConnection();
            
            ChargerDonnees();
            ClearAll();


        }
        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // if (guna2DataGridView1.SelectedRows.Count > 0)
            // {
            //     // Récupérez l'ID de la ligne sélectionnée (assurez-vous d'avoir une colonne d'ID dans votre DataGridView)
            //     int selectedRowIndex = guna2DataGridView1.SelectedRows[0].Index;
            //     int selectedClientId = Convert.ToInt32(guna2DataGridView1["id", selectedRowIndex].Value);
            //
            //     string query = "UPDATE Clients SET Nom = @Nom, Adresse = @Adresse, Cp = @Cp, Ville = @Ville  WHERE Id = @Id";
            //     SqlCommand command = new SqlCommand(query, myConnection.GetConnection());
            //     command.Parameters.AddWithValue("@Nom", txtName.Text);
            //     command.Parameters.AddWithValue("@Adresse", txtAdresse.Text);
            //     command.Parameters.AddWithValue("@Cp", txtCp.Text);
            //     command.Parameters.AddWithValue("@Ville", txtVille.Text);
            //     command.Parameters.AddWithValue("@Id", selectedClientId);
            //     command.ExecuteNonQuery();
            //     ChargerDonnees();
            //     ClearAll();
            // }
            // 
            myConnection.OpenConnection();
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = guna2DataGridView1.SelectedRows[0].Index;
                int ClientID = Convert.ToInt32(guna2DataGridView1.Rows[selectedIndex].Cells["IDClient"].Value);

                // Get the updated values from the textboxes
                string nomCli = txtName.Text;
                string adresseCli = txtAdresse.Text;
                string cpCli = txtCp.Text;
                string villeCli = txtVille.Text;



                string updateQuery = "UPDATE Clients SET Nom = @Nom, Adresse = @Adresse, Cp = @Cp, Ville = @Ville WHERE IDClient = @IDClient";
                SqlCommand updateCommand = new SqlCommand(updateQuery, myConnection.GetConnection());
                updateCommand.Parameters.AddWithValue("@IDClient", ClientID);
                updateCommand.Parameters.AddWithValue("@Nom", nomCli);
                updateCommand.Parameters.AddWithValue("@Adresse", adresseCli);
                updateCommand.Parameters.AddWithValue("@Cp", cpCli);
                updateCommand.Parameters.AddWithValue("@Ville", villeCli);

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
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = guna2DataGridView1.SelectedRows[0].Index;
                int IDClient = Convert.ToInt32(guna2DataGridView1.Rows[selectedIndex].Cells["IDClient"].Value);

                string deleteQuery = "DELETE FROM Clients WHERE IDClient = @IDClient";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, myConnection.GetConnection());
                deleteCommand.Parameters.AddWithValue("@IDClient", IDClient);

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

        private void guna2DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];

                // Les données sont stockées dans les cellules de la ligne sélectionnée
                string Nom = selectedRow.Cells["Nom"].Value.ToString();
                string Adresse = selectedRow.Cells["Adresse"].Value.ToString();
                string Cp = selectedRow.Cells["Cp"].Value.ToString();
                string Ville = selectedRow.Cells["Ville"].Value.ToString();
                // Afficher les données dans les TextBox
                txtName.Text = Nom;
                txtAdresse.Text = Adresse;
                txtCp.Text = Cp;
                txtVille.Text = Ville;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ChargerDonnees();
            ClearAll();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
            ClearAll();
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void guna2DataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2CircleButton4_Click(object sender, EventArgs e)
        {
            Form1 Clients = new Form1();
            this.Hide();
        }

        private void guna2CircleButton4_Click_1(object sender, EventArgs e)
        {
            
            Form1 Clients = new Form1();
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

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Familles Fm = new Familles();
            Fm.Show();
            this.Hide();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Produits Prd = new Produits();
            Prd.Show();
            this.Hide();
        }
    }
}

