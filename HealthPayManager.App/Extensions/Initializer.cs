using static System.Formats.Asn1.AsnWriter;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthPayManager.App.Extensions
{
    public class Initializer
    {
        public static async Task Seed(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HealthPayManagerConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (TableIsEmpty("Customers", connection, transaction))
                        {
                            await InsertCustomers(connection, transaction);
                        }
                        if (TableIsEmpty("Payments", connection, transaction))
                        {
                            await InsertPayments(connection, transaction);
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static async Task InsertCustomers(SqlConnection connection, SqlTransaction transaction)
        {
            var command = connection.CreateCommand();
            command.Transaction = transaction;

            command.CommandText = "INSERT INTO Customers (Name, PatientId, TimeCreated, TimeUpdated) VALUES (@Name, @PatientId, @TimeCreated, @TimeUpdated)";

            command.Parameters.Add("@Name", SqlDbType.NVarChar);
            command.Parameters.Add("@PatientId", SqlDbType.NVarChar);
            command.Parameters.Add("@TimeCreated", SqlDbType.DateTime);
            command.Parameters.Add("@TimeUpdated", SqlDbType.DateTime);

            command.Parameters["@Name"].Value = "John Doe";
            command.Parameters["@PatientId"].Value = "HTH-12345";
            command.Parameters["@TimeCreated"].Value = DateTime.UtcNow;
            command.Parameters["@TimeUpdated"].Value = DateTime.UtcNow;

            await command.ExecuteNonQueryAsync();
        }

        private static async Task InsertPayments(SqlConnection connection, SqlTransaction transaction)
        {
            var command = connection.CreateCommand();
            command.Transaction = transaction;

            command.CommandText = "INSERT INTO Payments (Amount, CustomerId, TimeCreated, TimeUpdated) VALUES (@Amount, @CustomerId, @TimeCreated, @TimeUpdated)";
            
            command.Parameters.Add("@Amount", SqlDbType.Decimal);
            command.Parameters.Add("@CustomerId", SqlDbType.BigInt);
            command.Parameters.Add("@TimeCreated", SqlDbType.DateTime);
            command.Parameters.Add("@TimeUpdated", SqlDbType.DateTime);

            command.Parameters["@Amount"].Value = 100.0m;
            command.Parameters["@CustomerId"].Value = 1;
            command.Parameters["@TimeCreated"].Value = DateTime.UtcNow;
            command.Parameters["@TimeUpdated"].Value = DateTime.UtcNow;

            await command.ExecuteNonQueryAsync();
        }

        private static bool TableIsEmpty(string tableName, SqlConnection connection, SqlTransaction transaction)
        {
            var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = $"SELECT COUNT(*) FROM {tableName}";

            var result = command.ExecuteScalar();
            return Convert.ToInt32(result) == 0;
        }
    }
}
