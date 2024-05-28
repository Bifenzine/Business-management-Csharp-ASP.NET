using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gestion_commercial_last_episode
{
    internal class connexion
    {
        private string connectionString;
        private SqlConnection connection;

        public connexion()
        {
            // Set your connection string
            connectionString = "Data Source=DESKTOP-MSOHDH6\\SQLEXPRESS;Initial Catalog=\"gestion commerciale\";Integrated Security=True;TrustServerCertificate=True";
            connection = new SqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                    MessageBox.Show("Hello Bifenzine :) Database connection opened successfully.");
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show("Error opening database connection :( " + ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    MessageBox.Show("Database connection closed successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error closing database connection: " + ex.Message);
            }
        }
        public SqlConnection GetConnection()
        {
            return connection;
        }
    }
}
