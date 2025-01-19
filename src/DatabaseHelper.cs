using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace CLI_CMS.src
{
    internal class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string dbPath)
        {
            _connectionString = $"Data Source={dbPath};";
            InitializeDatabase();
        }


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
                        name TEXT NOT NULL,
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

        public void AddClient(string name, string email, string phone, string address)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @"
                        INSERT INTO clients (name, email, phone, address)
                        VALUES ($name, $email, $phone, $address);
                    ";
                cmd.Parameters.AddWithValue("$name", name);
                cmd.Parameters.AddWithValue("$email", email);
                cmd.Parameters.AddWithValue("$phone", phone);
                cmd.Parameters.AddWithValue("$address", address);
                cmd.ExecuteNonQuery();
            }
        }


        public void DeleteClient(string name)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @"
                        DELETE FROM clients
                        WHERE name = $name;
                    ";
                cmd.Parameters.AddWithValue("$name", name);
                cmd.ExecuteNonQuery();
            }
        }


        public void UpdateClient(string name, string email, string phone, string address)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @"
                        UPDATE clients
                        SET email = $email, phone = $phone, address = $address
                        WHERE name = $name;
                    ";
                cmd.Parameters.AddWithValue("$name", name);
                cmd.Parameters.AddWithValue("$email", email);
                cmd.Parameters.AddWithValue("$phone", phone);
                cmd.Parameters.AddWithValue("$address", address);
                cmd.ExecuteNonQuery();
            }
        }


        public void ViewClient(string name)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @"
                        SELECT * FROM clients
                        WHERE name = $name;
                    ";
                cmd.Parameters.AddWithValue("$name", name);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"Name: {reader.GetString(1)}");
                    Console.WriteLine($"Email: {reader.GetString(2)}");
                    Console.WriteLine($"Phone: {reader.GetString(3)}");
                    Console.WriteLine($"Address: {reader.GetString(4)}");
                }
            }
        }



    }
}
