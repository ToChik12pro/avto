using Npgsql;

namespace avto
{
    public class SupabaseService
    {
        private readonly string _connectionString;

        public SupabaseService()
        {
            // Замените строкой подключения из конфигурации
            _connectionString = "User Id=postgres.rszhykplvuzblnknphqf;Password=AVTOSHOW45657;Server=aws-0-eu-central-1.pooler.supabase.com;Port=5432;Database=postgres;";
        }

        public async Task<int> AddUser(string username, string password, string email, string phone, string address)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new NpgsqlCommand(
                    "INSERT INTO \"User\" (\"Username\", \"Password\", \"Email\", \"Phone\", \"Address\") VALUES (@Username, @Password, @Email, @Phone, @Address) RETURNING \"UserId\"",
                    connection);
                command.Parameters.AddWithValue("Username", username);
                command.Parameters.AddWithValue("Password", password);
                command.Parameters.AddWithValue("Email", email);
                command.Parameters.AddWithValue("Phone", phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("Address", address ?? (object)DBNull.Value);

                // Получение UserId вновь добавленного пользователя
                var userId = (int)await command.ExecuteScalarAsync();
                return userId;
            }
        }

        public async Task AddClient(int userId, string name, string email, string phone, string address)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new NpgsqlCommand(
                    "INSERT INTO \"Client\" (\"UserId\", \"Name\", \"Email\", \"Phone\", \"Address\") VALUES (@UserId, @Name, @Email, @Phone, @Address)",
                    connection);
                command.Parameters.AddWithValue("UserId", userId);
                command.Parameters.AddWithValue("Name", name);
                command.Parameters.AddWithValue("Email", email);
                command.Parameters.AddWithValue("Phone", phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("Address", address ?? (object)DBNull.Value);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
