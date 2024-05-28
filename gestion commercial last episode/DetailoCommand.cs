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

namespace gestion_commercial_last_episode
{
    public partial class DetailoCommand : Form
    {
        private connexion myConnection;
        public DetailoCommand()
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
            txtQtCmd.Clear();
            txtPxVt.Clear();
            txtTotal.Clear();

        }
        private void ChargerDonnees()
        {
            string query = "SELECT \r\n    DC.IDligneCMD,\r\n    C.NumCMD,\r\n    C.Commentaires,\r\n    P.IDProduit,\r\n    P.Designation,\r\n        DC.PrixVT,\r\n DC.Qt,\r\n   DC.Total\r\nFROM \r\n    [Detail commandes] AS DC \r\nJOIN \r\n    Commandes AS C ON DC.NumCMD = C.NumCMD\r\nJOIN \r\n    Produits AS P ON DC.IDProduit = P.IDProduit;";
            SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection());

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            dataGridDetailCmd.DataSource = dataTable;

            // Hide the IDProduit and NumCMD columns
            dataGridDetailCmd.Columns["IDProduit"].Visible = false;
            dataGridDetailCmd.Columns["NumCMD"].Visible = false;
        }

        private void guna2CircleButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            DetailoCommand DtCmd = new DetailoCommand();
            this.Hide();
            Form1 Clients = new Form1();
            Clients.Show();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ChargerDonnees();
            ClearAll();
        }
        
        private void dataGridDetailCmd_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            myConnection.OpenConnection();
            if (dataGridDetailCmd.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridDetailCmd.SelectedRows[0];

                // Les données sont stockées dans les cellules de la ligne sélectionnée
                string CMDCommentaires = selectedRow.Cells["Commentaires"].Value.ToString();
                string ProduitDesignation = selectedRow.Cells["Designation"].Value.ToString();
                string PrixVT = selectedRow.Cells["PrixVT"].Value.ToString();
                string QTCMD = selectedRow.Cells["Qt"].Value.ToString();
                string Total = selectedRow.Cells["Total"].Value.ToString();

                // Afficher les données dans les TextBox DteCmd
                comboCommentairesCMD.Text = CMDCommentaires;
                comboDesignation.Text = ProduitDesignation;
                txtPxVt.Text = PrixVT;
                txtQtCmd.Text = QTCMD;
                txtTotal.Text = Total;

            }

        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            ClearAll();
            
        }

        private void txtQtCmd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtQtCmd.Text == "" || txtPxVt.Text == "")
                {
                    MessageBox.Show("Keep something empty !!! ", "Empty", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else
                {
                    Int64 Qty = Int64.Parse(txtQtCmd.Text);
                    Int64 Prix = Int64.Parse(txtPxVt.Text);
                    txtTotal.Text = (Prix * Qty).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DetailoCommand_Load(object sender, EventArgs e)
        {
            ChargerDonnees() ;
            loadCommentaireData();
            loadProduitDesignationData();
        }
        private void loadCommentaireData()
        {
            try
            {
                myConnection.OpenConnection();
                string query = "SELECT Commentaires FROM Commandes";
                SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection());
                SqlDataReader dr = cmd.ExecuteReader();

                // Clear combo box items before adding new items
                comboDesignation.Items.Clear();

                List<string> clientNames = new List<string>(); // List to store all client names

                // Populate the combo box with client first names
                while (dr.Read())
                {
                    string firstCommentaire = dr["Commentaires"].ToString();
                    comboCommentairesCMD.Items.Add(firstCommentaire);
                    clientNames.Add(firstCommentaire); // Add the client name to the list
                }

                // Set the display member to the item itself (client first name)
                comboCommentairesCMD.DisplayMember = "Commentaires";

                // Set the first value in the combo box to be the name of the first client
                if (clientNames.Count > 0)
                {
                    comboCommentairesCMD.SelectedIndex = 0;
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

        private void loadProduitDesignationData()
        {
            try
            {
                myConnection.OpenConnection();
                string query = "SELECT Designation FROM Produits";
                SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection());
                SqlDataReader dr = cmd.ExecuteReader();

                // Clear combo box items before adding new items
                comboDesignation.Items.Clear();

                List<string> clientNames = new List<string>(); // List to store all client names

                // Populate the combo box with client first names
                while (dr.Read())
                {
                    string firstDesignation = dr["Designation"].ToString();
                    comboDesignation.Items.Add(firstDesignation);
                    clientNames.Add(firstDesignation); // Add the client name to the list
                }

                // Set the display member to the item itself (client first name)
                comboDesignation.DisplayMember = "Designation";

                // Set the first value in the combo box to be the name of the first client
                if (clientNames.Count > 0)
                {
                    comboDesignation.SelectedIndex = 0;
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

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DetailoCommand DtCmd = new DetailoCommand();
            DtCmd.Show();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtQtCmd.Text == "" || txtPxVt.Text == "" || txtTotal.Text == "")
            {
                MessageBox.Show("Missing information !!!");
            }
            else
            {
                try
                {
                    myConnection.OpenConnection();

                    string query = "INSERT INTO [dbo].[Detail commandes] (IDProduit, NumCMD, Qt, PrixVT, Total) VALUES (@IDProduit, @NumCMD, @Qt, @PrixVT, @Total)";

                    using (SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection()))
                    {
                        // Retrieve the selected product's ID
                        string selectProduitCommentaire = comboDesignation.SelectedItem.ToString();
                        string selectProduitIdQuery = "SELECT IDProduit FROM Produits WHERE Designation = @Designation";
                        using (SqlCommand selectProduitID = new SqlCommand(selectProduitIdQuery, myConnection.GetConnection()))
                        {
                            selectProduitID.Parameters.AddWithValue("@Designation", selectProduitCommentaire);
                            int produitID = (int)selectProduitID.ExecuteScalar();
                            cmd.Parameters.AddWithValue("@IDProduit", produitID);
                        }

                        // Retrieve the selected command's ID
                        string selectCMDCommentaire = comboCommentairesCMD.SelectedItem.ToString();
                        string selectCMDIdQuery = "SELECT NumCMD FROM Commandes WHERE Commentaires = @Commentaires";
                        using (SqlCommand selectCMDID = new SqlCommand(selectCMDIdQuery, myConnection.GetConnection()))
                        {
                            selectCMDID.Parameters.AddWithValue("@Commentaires", selectCMDCommentaire);
                            int cmdID = (int)selectCMDID.ExecuteScalar();
                            cmd.Parameters.AddWithValue("@NumCMD", cmdID);
                        }

                        // Set parameters for the insert query
                        cmd.Parameters.AddWithValue("@Qt", txtQtCmd.Text);
                        cmd.Parameters.AddWithValue("@PrixVT", txtPxVt.Text);
                        cmd.Parameters.AddWithValue("@Total", txtTotal.Text);

                        cmd.ExecuteNonQuery();
                        ChargerDonnees();
                        ClearAll();
                    }

                    MessageBox.Show("Detail Command added successfully!!! ;)");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding Detail Command: -_- " + ex.Message);
                }
                finally
                {
                    myConnection.CloseConnection();
                }
            }
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridDetailCmd.SelectedRows.Count > 0)
            {
                myConnection.OpenConnection();

                int selectedIndex = dataGridDetailCmd.SelectedRows[0].Index;
                int IDligneCMD = Convert.ToInt32(dataGridDetailCmd.Rows[selectedIndex].Cells["IDligneCMD"].Value);

                // Get the updated values from the textboxes
                string QtCMD = txtQtCmd.Text;
                string PrixCMD = txtPxVt.Text;
                string TotalCMD = txtTotal.Text;

                // Retrieve the selected product's ID
                string selectProduitCommentaire = comboDesignation.SelectedItem.ToString();
                string selectProduitIdQuery = "SELECT IDProduit FROM Produits WHERE Designation = @Designation";
                int produitID;
                using (SqlCommand selectProduitID = new SqlCommand(selectProduitIdQuery, myConnection.GetConnection()))
                {
                    selectProduitID.Parameters.AddWithValue("@Designation", selectProduitCommentaire);
                    produitID = (int)selectProduitID.ExecuteScalar();
                }

                // Retrieve the selected command's ID
                string selectCMDCommentaire = comboCommentairesCMD.SelectedItem.ToString();
                string selectCMDIdQuery = "SELECT NumCMD FROM Commandes WHERE Commentaires = @Commentaires";
                int cmdID;
                using (SqlCommand selectCMDID = new SqlCommand(selectCMDIdQuery, myConnection.GetConnection()))
                {
                    selectCMDID.Parameters.AddWithValue("@Commentaires", selectCMDCommentaire);
                    cmdID = (int)selectCMDID.ExecuteScalar();
                }

                if (string.IsNullOrEmpty(QtCMD) || string.IsNullOrEmpty(PrixCMD) || string.IsNullOrEmpty(TotalCMD))
                {
                    MessageBox.Show("Veuillez remplir tous les champs avant de procéder à la modification.");
                    return; // Exit the method
                }

                string updateQuery = "UPDATE [dbo].[Detail commandes] SET PrixVT = @PrixVT, Qt = @Qt, Total = @Total, IDProduit = @IDProduit, NumCMD = @NumCMD WHERE IDligneCMD = @IDligneCMD";

                try
                {
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, myConnection.GetConnection()))
                    {
                        updateCommand.Parameters.AddWithValue("@IDligneCMD", IDligneCMD);
                        updateCommand.Parameters.AddWithValue("@PrixVT", PrixCMD);
                        updateCommand.Parameters.AddWithValue("@Qt", QtCMD);
                        updateCommand.Parameters.AddWithValue("@Total", TotalCMD);
                        updateCommand.Parameters.AddWithValue("@IDProduit", produitID);
                        updateCommand.Parameters.AddWithValue("@NumCMD", cmdID);
                        updateCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Enregistrement modifié avec succès.");
                    ChargerDonnees(); // Refresh the DataGridView after modification       
                    ClearAll();
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
            else
            {
                MessageBox.Show("Veuillez sélectionner un enregistrement à modifier.");
            }
        }




        private void txtTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridDetailCmd.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridDetailCmd.SelectedRows[0].Index;
                int IDligneCMD = Convert.ToInt32(dataGridDetailCmd.Rows[selectedIndex].Cells["IDligneCMD"].Value);

                string deleteQuery = "DELETE FROM [dbo].[Detail commandes] WHERE IDligneCMD = @IDligneCMD";

                try
                {
                    myConnection.OpenConnection();

                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, myConnection.GetConnection()))
                    {
                        deleteCommand.Parameters.AddWithValue("@IDligneCMD", IDligneCMD);
                        deleteCommand.ExecuteNonQuery();
                        MessageBox.Show("Enregistrement supprimé avec succès.");
                    }

                    ChargerDonnees(); // Refresh the DataGridView after deletion
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
                }
                finally
                {
                    myConnection.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un enregistrement à supprimer.");
            }
        }

    }
}
