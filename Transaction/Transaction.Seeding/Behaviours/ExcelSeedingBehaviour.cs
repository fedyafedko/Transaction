using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.Data.SqlClient;
using System.Globalization;
using Dapper;
using Transaction.Seeding.Interfaces;
using Transaction.Common.Configs;
using Microsoft.Extensions.Options;

namespace Transaction.Seeding.Behaviours;

public class ExcelSeedingBehaviour : ISeedingBehaviour
{
    private readonly ConnectionStringsConfig _connectionString;
    private readonly ExcelConfig _excelConfig;

    public ExcelSeedingBehaviour(
        IOptions<ConnectionStringsConfig> connectionString,
        IOptions<ExcelConfig> excelConfig)
    {
        _connectionString = connectionString.Value;
        _excelConfig = excelConfig.Value;
    }

    public async Task SeedAsync()
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        using (var reader = new StreamReader(_excelConfig.FilePath))
        using (var csv = new CsvReader(reader, config))
        {
            var records = csv.GetRecords<Entities.Transaction>();

            using (var connection = new SqlConnection(_connectionString.DefaultConnection))
            {
                await connection.OpenAsync();
                foreach (var record in records)
                {
                    var existingTransaction = await connection.QueryFirstOrDefaultAsync<Entities.Transaction>(
                        "SELECT * FROM Transactions WHERE TransactionId = @TransactionId", new { record.TransactionId });

                    if (existingTransaction != null)
                    {
                        var updateQuery = @"
                            UPDATE Transactions
                            SET Name = @Name, Email = @Email, Amount = @Amount, TransactionDate = @TransactionDate, ClientLocation = @ClientLocation
                            WHERE TransactionId = @TransactionId";
                        await connection.ExecuteAsync(updateQuery, record);
                    }
                    else
                    {
                        var insertQuery = @"
                            INSERT INTO Transactions (TransactionId, Name, Email, Amount, TransactionDate, ClientLocation)
                            VALUES (@TransactionId, @Name, @Email, @Amount, @TransactionDate, @ClientLocation)";
                        await connection.ExecuteAsync(insertQuery, record);
                    }
                }
            }
        }
    }
}
