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

namespace gestion_commercial_last_episode
{
    public partial class Form1 : Form
    {
        private connexion myConnection;
        public Form1()
        {
            InitializeComponent();
            myConnection = new connexion();  // Instantiate Connexion class
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
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                // Récupérez l'ID de la ligne sélectionnée (assurez-vous d'avoir une colonne d'ID dans votre DataGridView)
                int selectedRowIndex = guna2DataGridView1.SelectedRows[0].Index;
                int selectedClientId = Convert.ToInt32(guna2DataGridView1["id", selectedRowIndex].Value);

                string query = "UPDATE Clients SET Nom = @Nom, Adresse = @Adresse, Cp = @Cp, Ville = @Ville  WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, myConnection.GetConnection());
                command.Parameters.AddWithValue("@Nom", txtName.Text);
                command.Parameters.AddWithValue("@Adresse", txtAdresse.Text);
                command.Parameters.AddWithValue("@Cp", txtCp.Text);
                command.Parameters.AddWithValue("@Ville", txtVille.Text);
                command.Parameters.AddWithValue("@Id", selectedClientId);
                command.ExecuteNonQuery();
                ChargerDonnees();
                ClearAll();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           
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
    }
}

