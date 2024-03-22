using HealthPayManager.App.Data.Entities;
using HealthPayManager.App.Repositories.Interface;
using System.Data.SqlClient;

namespace HealthPayManager.App.Repositories.Concrete
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        private readonly string? _tableName = "Payments";

        public PaymentRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("HealthPayManagerConnection");
        }

        public async Task CreatePaymentsAsync(Payment payment)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = $"INSERT INTO {_tableName} (Amount, CustomerId, TimeCreated, TimeUpdated) VALUES (@Amount, @CustomerId, @TimeCreated, @TimeUpdated);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Amount", payment.Amount);
                    command.Parameters.AddWithValue("@CustomerId", payment.CustomerId);
                    command.Parameters.AddWithValue("@TimeCreated", payment.TimeCreated);
                    command.Parameters.AddWithValue("@TimeUpdated", payment.TimeUpdated);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeletePaymentsAsync(long id)
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

        public async Task<Payment> ReadPaymentByIdAsync(long id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = $"SELECT * FROM {_tableName} WHERE CustomerID = @Id;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Payment
                            {
                                Id = (long)reader["Id"],
                                Amount = (decimal)reader["Amount"],
                                CustomerId = (long)reader["CustomerId"],
                                TimeCreated = (DateTime)reader["TimeCreated"],
                                TimeUpdated = (DateTime)reader["TimeUpdated"]
                            };
                        }
                    }
                }
            }

            return null!;
        }

        public async Task<IEnumerable<Payment>> ReadPaymentsAsync()
        {
            List<Payment> payment = new List<Payment>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = $"SELECT * FROM {_tableName}";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            payment.Add(new Payment
                            {
                                Id = (long)reader["Id"],
                                Amount = (decimal)reader["Amount"],
                                CustomerId = (long)reader["CustomerId"],
                                TimeCreated = (DateTime)reader["TimeCreated"],
                                TimeUpdated = (DateTime)reader["TimeUpdated"]
                            });
                        }
                    }
                }
            }

            return payment;
        }

        public async Task<IEnumerable<Payment>> ReadPaymentsByCustomerIdAsync(long customerId)
        {
            List<Payment> payment = new List<Payment>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = $"SELECT * FROM {_tableName} WHERE CustomerId = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            payment.Add(new Payment
                            {
                                Id = (long)reader["Id"],
                                Amount = (decimal)reader["Amount"],
                                CustomerId = (long)reader["CustomerId"],
                                TimeCreated = (DateTime)reader["TimeCreated"],
                                TimeUpdated = (DateTime)reader["TimeUpdated"]
                            });
                        }
                    }
                }
            }

            return payment;
        }

        public async Task UpdatePaymentsAsync(Payment payment)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = $"UPDATE {_tableName} SET Amount = @Amount, CustomerId = @CustomerId, TimeUpdated = @TimeUpdated WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", payment.Id);
                    command.Parameters.AddWithValue("@Amount", payment.Amount);
                    command.Parameters.AddWithValue("@CustomerId", payment.CustomerId);
                    command.Parameters.AddWithValue("@TimeUpdated", payment.TimeUpdated);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
