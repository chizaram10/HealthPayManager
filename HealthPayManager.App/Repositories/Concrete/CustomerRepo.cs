using HealthPayManager.App.Data.Entities;
using HealthPayManager.App.Repositories.Interface;
using System.Data.SqlClient;

namespace HealthPayManager.App.Repositories.Concrete
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        private readonly string _tableName = "Customers";

        public CustomerRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("HealthPayManagerConnection");
        }

        public async Task CreateCustomersAsync(Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = $"INSERT INTO {_tableName} (Name, PatientId, TimeCreated, TimeUpdated) VALUES (@Name, @PatientId, @TimeCreated, @TimeUpdated);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", customer.Name);
                    command.Parameters.AddWithValue("@PatientId", customer.PatientId);
                    command.Parameters.AddWithValue("@TimeCreated", customer.TimeCreated);
                    command.Parameters.AddWithValue("@TimeUpdated", customer.TimeUpdated);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteCustomersAsync(long id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = $"DELETE FROM {_tableName} WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Customer> ReadCustomerByIdAsync(long id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = $"SELECT * FROM {_tableName} WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Customer
                            {
                                Id = (long)reader["Id"],
                                Name = reader["Name"].ToString()!,
                                PatientId = reader["PatientId"].ToString()!,
                                TimeCreated = (DateTime)reader["TimeCreated"],
                                TimeUpdated = (DateTime)reader["TimeUpdated"]
                            };
                        }
                    }
                }
            }

            return null!;
        }

        public async Task<IEnumerable<Customer>> ReadCustomersAsync(string searchText)
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = $"SELECT * FROM {_tableName} WHERE Name LIKE '%{searchText}%';";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                Id = (long)reader["Id"],
                                Name = reader["Name"].ToString()!,
                                PatientId = reader["PatientId"].ToString()!,
                                TimeCreated = (DateTime)reader["TimeCreated"],
                                TimeUpdated = (DateTime)reader["TimeUpdated"]
                            });
                        }
                    }
                }
            }

            return customers;
        }

        public async Task<IEnumerable<string>> ReadCustomersPatientIdsAsync()
        {
            List<string> patientIds = new List<string>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = $"SELECT PatientId FROM {_tableName};";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            patientIds.Add(reader["PatientId"].ToString()!);
                        }
                    }
                }
            }

            return patientIds;
        }

        public async Task UpdateCustomersAsync(Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = $"UPDATE {_tableName} SET Name = @Name, PatientId = @PatientId, TimeUpdated = @TimeUpdated WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", customer.Id);
                    command.Parameters.AddWithValue("@Name", customer.Name);
                    command.Parameters.AddWithValue("@PatientId", customer.PatientId);
                    command.Parameters.AddWithValue("@TimeUpdated", customer.TimeUpdated);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
