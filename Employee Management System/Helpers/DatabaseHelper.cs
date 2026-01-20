using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;


namespace Employee_Management_System.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool RegisterUser(string username, string email, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    var query = @"
                        INSERT INTO Users (Username, PasswordHash, Email, CreatedDate) 
                        VALUES (@Username, @PasswordHash, @Email, GETDATE())";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@PasswordHash", HashPassword(password));
                        command.Parameters.AddWithValue("@Email", email);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
                catch (SqlException ex)
                {
                    // Handle duplicate username/email
                    throw new Exception("Registration failed: " + ex.Message);
                }
            }
        }

        public bool ValidateUser(string username, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"
                    SELECT UserID, PasswordHash 
                    FROM Users 
                    WHERE Username = @Username AND IsActive = 1";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader["PasswordHash"].ToString();
                            string inputHash = HashPassword(password);

                            return storedHash == inputHash;
                        }
                    }
                }
            }
            return false;
        }

        public void UpdateLastLogin(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE Users SET LastLoginDate = GETDATE() WHERE Username = @Username";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool UserExists(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
    }
}