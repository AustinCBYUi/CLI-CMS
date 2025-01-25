using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

/*
 * DatabaseHelper.cs
 * 
 * @Author: Austin Campbell
*/



namespace CLI_CMS.src
{
    /// <summary>
    /// Database helper class to handle CRUD operations for clients and users.
    /// </summary>
    internal class DatabaseHelper
    {
        private readonly string _connectionString;

        /// <summary>
        /// Database helper constructor to initialize the connection string.
        /// </summary>
        /// <param name="dbPath">Path</param>
        public DatabaseHelper(string dbPath)
        {
            _connectionString = $"Data Source={dbPath};";
            InitializeDatabase();
        }


        /// <summary>
        /// Initialize database, create tables if they do not exist. Must delete the database file to recreate tables if
        /// modifying data storage.
        /// </summary>
        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                //Example: Create a table
                var createTableCmd = connection.CreateCommand();
                createTableCmd.CommandText =
                    @"
                    CREATE TABLE IF NOT EXISTS clients (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        first_name TEXT NOT NULL,
                        last_name TEXT NOT NULL,
                        email TEXT NOT NULL,
                        phone TEXT NOT NULL,
                        address TEXT NOT NULL
                    );
                    ";

                var createTableCmd2 = connection.CreateCommand();
                createTableCmd2.CommandText =
                    @"
                        CREATE TABLE IF NOT EXISTS users (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            username TEXT NOT NULL,
                            password TEXT NOT NULL
                        );
                    ";
                createTableCmd.ExecuteNonQuery();
                createTableCmd2.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Register a user into the database to have access to logging in.
        /// </summary>
        /// <param name="username">Username (str)</param>
        /// <param name="password">Password (str)</param>
        public void AddUser(string username, string password)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @"
                        INSERT INTO users (username, password)
                        VALUES ($username, $password);
                    ";
                cmd.Parameters.AddWithValue("$username", username);
                cmd.Parameters.AddWithValue("$password", password);
                cmd.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Finds a registered user in the database.
        /// </summary>
        /// <param name="username">Username of registered user (str)</param>
        /// <param name="password">Hashed password (str)</param>
        /// <returns></returns>
        public bool FindUser(string username, string password)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @"
                        SELECT * FROM users
                        WHERE username = $username;
                    ";
                cmd.Parameters.AddWithValue("$username", username);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (Hasher.Verify(password, reader.GetString(2)))
                    {
                        return true;
                    }
                }
                return false;
            }
        }


        /* Client CRUD Operations */

/// <summary>
/// Adds a client to the database
/// </summary>
/// <param name="first_name">First name of client (str)</param>
/// <param name="last_name">Last name of client (str)</param>
/// <param name="email">Email of client (str)</param>
/// <param name="phone">Phone number of client (str)</param>
/// <param name="address">Address of client (str)</param>
public void AddClient(string first_name, string last_name, string email, string phone, string address)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @"
                        INSERT INTO clients (first_name, last_name, email, phone, address)
                        VALUES ($first_name, $last_name, $email, $phone, $address);
                    ";
                cmd.Parameters.AddWithValue("$first_name", first_name);
                cmd.Parameters.AddWithValue("$last_name", last_name);
                cmd.Parameters.AddWithValue("$email", email);
                cmd.Parameters.AddWithValue("$phone", phone);
                cmd.Parameters.AddWithValue("$address", address);
                cmd.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Deletes a client based on first name or last name searched.
        /// </summary>
        /// <param name="first_name">Optional: Searching based off first name, set last name to null</param>
        /// <param name="last_name">Optional: Searching based off last name, set first name to null</param>
        public void DeleteClient(string first_name = "", string last_name = "")
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                if (first_name != null)
                {
                    cmd.CommandText =
                    @"
                        DELETE FROM clients
                        WHERE first_name = $first_name;
                    ";
                    cmd.Parameters.AddWithValue("$first_name", first_name);
                    cmd.ExecuteNonQuery();
                }
                else if (last_name != null)
                {
                    cmd.CommandText =
                    @"
                        DELETE FROM clients
                        WHERE last_name = $last_name;
                    ";
                    cmd.Parameters.AddWithValue("$last_name", last_name);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Updates a client's information based on first name or last name searched.
        /// </summary>
        /// <param name="first_name">First name as a string</param>
        /// <param name="last_name">Last name as a string</param>
        /// <param name="email">Email as a string</param>
        /// <param name="phone">Phone number as a string</param>
        /// <param name="address">Address as a string</param>
        public void UpdateClient(int id, string first_name = null, string last_name = null, string email = null, string phone = null, string address = null)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                if (first_name != null)
                {
                    cmd.CommandText =
                        @"
                            UPDATE clients
                            SET first_name = $first_name, last_name = $last_name, email = $email, phone = $phone, address = $address
                            WHERE id = $id;
                        ";
                    cmd.Parameters.AddWithValue("$id", id);
                    cmd.Parameters.AddWithValue("$first_name", first_name);
                    cmd.Parameters.AddWithValue("$last_name", last_name);
                    cmd.Parameters.AddWithValue("$email", email);
                    cmd.Parameters.AddWithValue("$phone", phone);
                    cmd.Parameters.AddWithValue("$address", address);
                    cmd.ExecuteNonQuery();
                }
                else if (last_name != null)
                {
                    cmd.CommandText =
                        @"
                            UPDATE clients
                            SET first_name = $first_name, last_name = $last_name, email = $email, phone = $phone, address = $address
                            WHERE last_name = $last_name;
                        ";
                    cmd.Parameters.AddWithValue("$first_name", first_name);
                    cmd.Parameters.AddWithValue("$last_name", last_name);
                    cmd.Parameters.AddWithValue("$email", email);
                    cmd.Parameters.AddWithValue("$phone", phone);
                    cmd.Parameters.AddWithValue("$address", address);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public int GetClientId(string first_name = null, string last_name = null)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();

                if (first_name != null)
                {

                    cmd.CommandText = @"
                        SELECT id FROM clients
                        WHERE first_name = $first_name;
                        ";

                    cmd.Parameters.AddWithValue("$first_name", first_name);
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (last_name != null)
                {
                    cmd.CommandText = @"
                        SELECT id FROM clients
                        WHERE last_name = $last_name;
                        ";

                    cmd.Parameters.AddWithValue("$last_name", last_name);
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Views all clients stored within the database.
        /// </summary>
        public void ViewClients()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @"
                        SELECT * FROM clients;
                    ";
                //cmd.Parameters.AddWithValue("$name", name);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("\n");
                    Console.WriteLine($"First Name: {reader.GetString(1)}");
                    Console.WriteLine($"Last Name: {reader.GetString(2)}");
                    Console.WriteLine($"Email: {reader.GetString(3)}");
                    Console.WriteLine($"Phone: {reader.GetString(4)}");
                    Console.WriteLine($"Address: {reader.GetString(5)}");
                    Console.WriteLine("\n");
                }
            }
        }


        /// <summary>
        /// Views a client by first name or last name as a single entry.
        /// </summary>
        /// <param name="first_name">Optional: First name param, set to null if searching by last name.</param>
        /// <param name="last_name">Optional: Last name param, set to null if searching by first name.</param>
        public void ViewAClient(string first_name = "", string last_name = "")
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                if (first_name != null)
                {
                    cmd.CommandText =
                    @"
                        SELECT * FROM clients
                        WHERE first_name = $first_name;
                    ";
                    cmd.Parameters.AddWithValue("$first_name", first_name);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine($"First Name: {reader.GetString(1)}");
                        Console.WriteLine($"Last Name: {reader.GetString(2)}");
                        Console.WriteLine($"Email: {reader.GetString(3)}");
                        Console.WriteLine($"Phone: {reader.GetString(4)}");
                        Console.WriteLine($"Address: {reader.GetString(5)}");
                        Console.WriteLine("\n");
                    }
                }
                else if (last_name != null)
                {
                    cmd.CommandText =
                    @"
                        SELECT * FROM clients
                        WHERE last_name = $last_name;
                    ";
                    cmd.Parameters.AddWithValue("$last_name", last_name);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine($"First Name: {reader.GetString(1)}");
                        Console.WriteLine($"Last Name: {reader.GetString(2)}");
                        Console.WriteLine($"Email: {reader.GetString(3)}");
                        Console.WriteLine($"Phone: {reader.GetString(4)}");
                        Console.WriteLine($"Address: {reader.GetString(5)}");
                        Console.WriteLine("\n");
                    }
                }
            }
        }


        /// <summary>
        /// Generates a summary report of the data from the clients table.
        /// </summary>
        public void GenerateReport()
        {
            string query = "SELECT * FROM clients";

            using (var connection = new SqliteConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        using (StreamWriter writer = new StreamWriter("clients.csv"))
                        {
                            writer.WriteLine("First Name,Last Name,Email,Phone,Address");
                            while (reader.Read())
                            {
                                writer.WriteLine($"{reader.GetString(1)},{reader.GetString(2)},{reader.GetString(3)},{reader.GetString(4)},{reader.GetString(5)}");
                            }
                        }
                        Console.WriteLine("\nReport generated successfully\n");
                    }
                    else
                    {
                        Console.WriteLine("\nNo clients found\n");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
