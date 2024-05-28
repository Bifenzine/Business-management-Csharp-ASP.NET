using Guna.UI2.WinForms;
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
using static Jenga.Theme;

namespace gestion_commercial_last_episode
{
    public partial class Produits : Form
    {
        private connexion myConnection;
        public Produits()
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

        private void ChargerDonnees()
        {
            string query = "SELECT \r\np.IDProduit,\r\np.IDFam,\r\nf.Intitule,\r\np.Designation,\r\np.StkInvent,\r\np.stkActuel,\r\np.Prixuht,\r\np.Photo\r\nFROM \r\n    Produits AS p \r\nJOIN \r\n    Familles AS f ON p.IDFam = f.IDFAm";
            SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection());

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            dataGridProducts.DataSource = dataTable;

            // Hide the IDProduit and NumCMD columns
            dataGridProducts.Columns["IDFam"].Visible = false;
        }

        private void loadFamilleData()
        {
            try
            {
                myConnection.OpenConnection();
                string query = "SELECT Intitule FROM Familles";
                SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection());
                SqlDataReader dr = cmd.ExecuteReader();

                // Clear combo box items before adding new items
                comboIntitule.Items.Clear();

                List<string> clientNames = new List<string>(); // List to store all client names

                // Populate the combo box with client first names
                while (dr.Read())
                {
                    string firstCommentaire = dr["Intitule"].ToString();
                    comboIntitule.Items.Add(firstCommentaire);
                    clientNames.Add(firstCommentaire); // Add the client name to the list
                }

                // Set the display member to the item itself (client first name)
                comboIntitule.DisplayMember = "Intitule";

                // Set the first value in the combo box to be the name of the first client
                if (clientNames.Count > 0)
                {
                    comboIntitule.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            finally
            {
                myConnection.CloseConnection();
            }
        }
        private void ClearAll()
        {
            txtDesignation.Clear();
            txtStkInVente.Clear();
            txtStkActuel.Clear();
            txtPrUHT.Clear();
            txtPhoto.Clear();

        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2CircleButton4_Click(object sender, EventArgs e)
        {
            Produits Prd = new Produits();
            this.Hide();
            Form1 Clients = new Form1();
            Clients.Show();
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            ClearAll();
            ChargerDonnees();
        }

        private void Produits_Load(object sender, EventArgs e)
        {
            loadFamilleData();
            ChargerDonnees();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Produits Prd = new Produits();
            Prd.Show();
            this.Hide();
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                myConnection.OpenConnection();

                string query = "INSERT INTO Produits (IDFam,Designation, StkInvent, stkActuel, Prixuht,Photo) VALUES (@IDFam,@Designation, @StkInvent, @stkActuel, @Prixuht,@Photo)";

                using (SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection()))
                {
                    string selectIntitule = comboIntitule.SelectedItem.ToString();

                    // Retrieve the IDClient from the database based on the selected client name
                    string selectIdFamQuery = "SELECT IDFam FROM Familles WHERE Intitule = @Intitule";
                    using (SqlCommand selectIDFamFamilles = new SqlCommand(selectIdFamQuery, myConnection.GetConnection()))
                    {
                        selectIDFamFamilles.Parameters.AddWithValue("@Intitule", selectIntitule);
                        int selectFamID = (int)selectIDFamFamilles.ExecuteScalar();

                        // Set parameters for the insert query
                        cmd.Parameters.AddWithValue("@IDFam", selectFamID);
                    }

                    cmd.Parameters.AddWithValue("@Designation", txtDesignation.Text);
                    cmd.Parameters.AddWithValue("@StkInvent", txtStkInVente.Text);
                    cmd.Parameters.AddWithValue("@stkActuel", txtStkActuel.Text);
                    cmd.Parameters.AddWithValue("@Prixuht", txtPrUHT.Text);
                    cmd.Parameters.AddWithValue("@Photo", txtPhoto.Text);

                    cmd.ExecuteNonQuery();
                    ChargerDonnees();
                    ClearAll();
                }

                MessageBox.Show("Produits added successfully!!! ;)");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding Produits: -_- " + ex.Message);
            }
            finally
            {
                myConnection.CloseConnection();
            }
        }
       

        private void dataGridProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            myConnection.OpenConnection();
            if (dataGridProducts.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridProducts.SelectedRows[0];

                // Les données sont stockées dans les cellules de la ligne sélectionnée
                string Intitule = selectedRow.Cells["Intitule"].Value.ToString();
                string Designation = selectedRow.Cells["Designation"].Value.ToString();
                string StkInVente = selectedRow.Cells["StkInvent"].Value.ToString();
                string StkActuel = selectedRow.Cells["stkActuel"].Value.ToString();
                string PrixUHT = selectedRow.Cells["Prixuht"].Value.ToString();
                string Photo = selectedRow.Cells["Photo"].Value.ToString();

                // Afficher les données dans les TextBox DteCmd
                comboIntitule.Text = Intitule;
                txtDesignation.Text = Designation;
                txtStkInVente.Text = StkInVente;
                txtStkActuel.Text = StkActuel;
                txtPrUHT.Text = PrixUHT;
                txtPhoto.Text = Photo;

            }       
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridProducts.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridProducts.SelectedRows[0].Index;
                int IDProduit = Convert.ToInt32(dataGridProducts.Rows[selectedIndex].Cells["IDProduit"].Value);

                string deleteQuery = "DELETE FROM Produits WHERE IDProduit = @IDProduit";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, myConnection.GetConnection());
                deleteCommand.Parameters.AddWithValue("@IDProduit", IDProduit);

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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                myConnection.OpenConnection();

                if (dataGridProducts.SelectedRows.Count > 0)
                {
                    int selectedIndex = dataGridProducts.SelectedRows[0].Index;
                    int IDProduit = Convert.ToInt32(dataGridProducts.Rows[selectedIndex].Cells["IDProduit"].Value);

                    // Get the updated values from the textboxes
                    string designationPR = txtDesignation.Text;
                    string stkInVt = txtStkInVente.Text;
                    string stkActPR = txtStkActuel.Text;
                    string prixPR = txtPrUHT.Text;
                    string photoPr = txtPhoto.Text;
                    string intitule = comboIntitule.SelectedItem.ToString(); // Assuming you have a combo box for selecting Intitule

                    string updateQuery = "UPDATE Produits SET Designation = @Designation, StkInvent = @StkInvent, stkActuel = @stkActuel, Prixuht = @Prixuht, Photo = @Photo, IDFam = (SELECT IDFam FROM Familles WHERE Intitule = @Intitule) WHERE IDProduit = @IDProduit";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, myConnection.GetConnection()))
                    {
                        updateCommand.Parameters.AddWithValue("@IDProduit", IDProduit);
                        updateCommand.Parameters.AddWithValue("@Designation", designationPR);
                        updateCommand.Parameters.AddWithValue("@StkInvent", stkInVt);
                        updateCommand.Parameters.AddWithValue("@stkActuel", stkActPR);
                        updateCommand.Parameters.AddWithValue("@Prixuht", prixPR);
                        updateCommand.Parameters.AddWithValue("@Photo", photoPr);
                        updateCommand.Parameters.AddWithValue("@Intitule", intitule);

                        updateCommand.ExecuteNonQuery();
                        MessageBox.Show("Enregistrement modifié avec succès.");
                        ChargerDonnees(); // Refresh the DataGridView after modification
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner un enregistrement à modifier.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la modification : " + ex.Message);
            }
            finally
            {
                myConnection.CloseConnection();
            }
        }


    }
}
