using Guna.UI2.WinForms;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace gestion_commercial_last_episode
{
    public partial class Commandes : Form
    {

        private connexion myConnection;
       
        public Commandes()
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
            txtCommentaireCmd.Text = "";
            txtEtatCmd.Text = "";
            txtDate.Value = DateTime.Now; // Optionally reset the date field to the current date
            loadData();
        }

        private void ChargerDonnees()
        {
            string query = "SELECT Clients.IDClient, Clients.Nom, Commandes.NumCMD ,Commandes.DteCmd , Commandes.EtatCMD, Commandes.Commentaires\r\nFROM Clients\r\n JOIN Commandes ON Clients.IDClient = Commandes.IDClient;";
            SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection());


            SqlDataAdapter adapter = new SqlDataAdapter(cmd);


            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);


            dataGridCommands.DataSource = dataTable;
            dataGridCommands.Columns["IDClient"].Visible = false;
        }

        private void loadData()
        {
            try
            {
                myConnection.OpenConnection();
                string query = "SELECT Nom FROM Clients";
                SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection());
                SqlDataReader dr = cmd.ExecuteReader();

                // Clear combo box items before adding new items
                comboClient.Items.Clear();

                List<string> clientNames = new List<string>(); // List to store all client names

                // Populate the combo box with client first names
                while (dr.Read())
                {
                    string clientFirstName = dr["Nom"].ToString();
                    comboClient.Items.Add(clientFirstName);
                    clientNames.Add(clientFirstName); // Add the client name to the list
                }

                // Set the display member to the item itself (client first name)
                comboClient.DisplayMember = "Nom";

                // Set the first value in the combo box to be the name of the first client
                if (clientNames.Count > 0)
                {
                    comboClient.SelectedIndex = 0;
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



        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtCp_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtDate.Text == "" || txtCommentaireCmd.Text == "" || txtEtatCmd.Text == "" || comboClient.SelectedItem == null)
            {
                MessageBox.Show("Missing information!!!");
            }
            else
            {
                try
                {
                    myConnection.OpenConnection();

                    string query = "INSERT INTO Commandes (IDClient, DteCmd, Commentaires, EtatCMD) VALUES (@IDClient, @DteCmd, @Commentaires, @EtatCmd)";

                    using (SqlCommand cmd = new SqlCommand(query, myConnection.GetConnection()))
                    {
                        string selectClientName = comboClient.SelectedItem.ToString();

                        // Retrieve the IDClient from the database based on the selected client name
                        string selectClientIdQuery = "SELECT IDClient FROM Clients WHERE Nom = @Nom";
                        using (SqlCommand selectClientIdCmd = new SqlCommand(selectClientIdQuery, myConnection.GetConnection()))
                        {
                            selectClientIdCmd.Parameters.AddWithValue("@Nom", selectClientName);
                            int selectClientID = (int)selectClientIdCmd.ExecuteScalar();

                            // Set parameters for the insert query
                            cmd.Parameters.AddWithValue("@IDClient", selectClientID);
                        }

                        // Set other parameters for the insert query
                        cmd.Parameters.AddWithValue("@DteCmd", txtDate.Value.Date);
                        cmd.Parameters.AddWithValue("@Commentaires", txtCommentaireCmd.Text);
                        cmd.Parameters.AddWithValue("@EtatCmd", txtEtatCmd.Text);

                        cmd.ExecuteNonQuery();
                        ChargerDonnees();
                        ClearAll();
                    }

                    MessageBox.Show("Command added successfully!!! ;)");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding Command: -_- " + ex.Message);
                }
                finally
                {
                    myConnection.CloseConnection();
                }
            }
        }


        private void guna2CircleButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            Commandes cmd = new Commandes();
            this.Hide();
            Form1 Clients = new Form1();
            Clients.Show();
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ChargerDonnees();
            ClearAll();
        }

        private void Commandes_Load(object sender, EventArgs e)
        {
            ChargerDonnees();
            ClearAll();
            loadData();
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Form1 Clients = new Form1();
            Clients.Show();
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridCommands.SelectedRows.Count > 0)
            {
                try
                {
                    myConnection.OpenConnection();

                    int selectedIndex = dataGridCommands.SelectedRows[0].Index;
                    int NumCMD = Convert.ToInt32(dataGridCommands.Rows[selectedIndex].Cells["NumCMD"].Value);

                    // Get the updated values from the textboxes
                    string dateCmdString = txtDate.Text;
                    string CommentaireCmd = txtCommentaireCmd.Text;
                    string EtatCmd = txtEtatCmd.Text;

                    DateTime dateCmd;
                    if (!DateTime.TryParse(dateCmdString, out dateCmd))
                    {
                        // Display an error message with the input date string and the expected format
                        MessageBox.Show($"Impossible de parser la date. Entrée : {dateCmdString}, Format Attendu : jj/MM/aaaa");
                        return; // Exit the method
                    }

                    if (string.IsNullOrEmpty(CommentaireCmd) || string.IsNullOrEmpty(EtatCmd))
                    {
                        MessageBox.Show("Veuillez remplir tous les champs avant de procéder à la modification.");
                        return; // Exit the method
                    }

                    string updateQuery = "UPDATE Commandes SET DteCmd = @DteCmd, Commentaires = @Commentaires, EtatCMD = @EtatCMD WHERE NumCMD = @NumCMD";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, myConnection.GetConnection()))
                    {
                        updateCommand.Parameters.AddWithValue("@DteCmd", dateCmd);
                        updateCommand.Parameters.AddWithValue("@Commentaires", CommentaireCmd);
                        updateCommand.Parameters.AddWithValue("@EtatCMD", EtatCmd);
                        updateCommand.Parameters.AddWithValue("@NumCMD", NumCMD);

                        updateCommand.ExecuteNonQuery();
                        MessageBox.Show("Enregistrement modifié avec succès.");
                        ChargerDonnees(); // Refresh the DataGridView after modification
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
            else
            {
                MessageBox.Show("Veuillez sélectionner un enregistrement à modifier.");
            }
        }









        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                myConnection.OpenConnection();

                if (dataGridCommands.SelectedRows.Count > 0)
                {
                    int selectedIndex = dataGridCommands.SelectedRows[0].Index;
                    int NumCMD = Convert.ToInt32(dataGridCommands.Rows[selectedIndex].Cells["NumCMD"].Value);

                    string deleteQuery = "DELETE FROM Commandes WHERE NumCMD = @NumCMD";
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, myConnection.GetConnection()))
                    {
                        deleteCommand.Parameters.AddWithValue("@NumCMD", NumCMD);

                        deleteCommand.ExecuteNonQuery();
                        MessageBox.Show("Enregistrement supprimé avec succès.");
                        ChargerDonnees(); // Refresh the DataGridView after deletion
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner un enregistrement à supprimer.");
                }
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


        private void dataGridCommands_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            myConnection.OpenConnection();
            if (dataGridCommands.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridCommands.SelectedRows[0];

                // Les données sont stockées dans les cellules de la ligne sélectionnée
                string CliName = selectedRow.Cells["Nom"].Value.ToString();
                string DateCMD = selectedRow.Cells["DteCmd"].Value.ToString();
                string CommentaireCMD = selectedRow.Cells["Commentaires"].Value.ToString();
                string EtatCMD = selectedRow.Cells["EtatCMD"].Value.ToString();

                // Afficher les données dans les TextBox DteCmd
                comboClient.Text = CliName;
                txtDate.Text = DateCMD;
                txtCommentaireCmd.Text = CommentaireCMD;
                txtEtatCmd.Text = EtatCMD;
                
            }
        }

        private void comboClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
