using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using Dapper;
using Transaction.Seeding.Interfaces;
using Transaction.Common.Configs;
using Microsoft.Extensions.Options;
using System.Data;

namespace Transaction.Seeding.Behaviours;

public class ExcelSeedingBehaviour : ISeedingBehaviour
{
    private readonly IDbConnection _dbConnection;
    private readonly ExcelConfig _excelConfig;

    public ExcelSeedingBehaviour(
        IOptions<ExcelConfig> excelConfig,
        IDbConnection dbConnection)
    {
        _excelConfig = excelConfig.Value;
        _dbConnection = dbConnection;
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

            foreach (var record in records)
            {
                var existingTransaction = await _dbConnection.QueryFirstOrDefaultAsync<Entities.Transaction>(
                    "SELECT * FROM Transactions WHERE TransactionId = @TransactionId", new { record.TransactionId });

                if (existingTransaction != null)
                {
                    var updateQuery = @"
                            UPDATE Transactions
                            SET Name = @Name, Email = @Email, Amount = @Amount, TransactionDate = @TransactionDate, ClientLocation = @ClientLocation
                            WHERE TransactionId = @TransactionId";
                    await _dbConnection.ExecuteAsync(updateQuery, record);
                }
                else
                {
                    var insertQuery = @"
                            INSERT INTO Transactions (TransactionId, Name, Email, Amount, TransactionDate, ClientLocation)
                            VALUES (@TransactionId, @Name, @Email, @Amount, @TransactionDate, @ClientLocation)";
                    await _dbConnection.ExecuteAsync(insertQuery, record);
                }
            }
        }
    }
}
