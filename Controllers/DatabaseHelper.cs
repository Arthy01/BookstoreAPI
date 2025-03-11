using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;

namespace BookstoreAPI.Controllers
{
    public class DatabaseHelper : IDisposable
    {
        private readonly MySqlConnection _connection;

        public DatabaseHelper()
        {
            string connectionString = "server=localhost;port=3306;database=bookstore;user=integration;password=integrationpw0815;";
            _connection = new MySqlConnection(connectionString);
        }

        // Verbindung öffnen
        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        // Verbindung schließen
        private void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public DataTable ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                OpenConnection();
                using (MySqlCommand cmd = new MySqlCommand(query, _connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler bei der Datenbankabfrage: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return dt;
        }

        public int ExecuteNonQuery(string query, Dictionary<string, object>? parameters = null)
        {
            int rowsAffected = 0;
            try
            {
                OpenConnection();
                using (MySqlCommand cmd = new MySqlCommand(query, _connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Ausführen der SQL-Anweisung: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return rowsAffected;
        }

        public object? ExecuteScalar(string query, Dictionary<string, object>? parameters = null)
        {
            object? result = null;
            try
            {
                OpenConnection();
                using (MySqlCommand cmd = new MySqlCommand(query, _connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    result = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler bei der ExecuteScalar-Abfrage: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
