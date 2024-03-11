using Dapper;
using OfficeOpenXml;
using System.Data;
using Transaction.BLL.Extensions;
using Transaction.BLL.Services.Interfaces;
using Transaction.Common.DTO;

namespace Transaction.BLL.Services;

public class TransactionService : ITransactionService
{
    private readonly IDbConnection _dbConnection;

    public TransactionService(IDbConnection connection)
    {
        _dbConnection = connection;
    }

    public async Task<byte[]> GetTransactionsFile(List<string> columnsName)
    {
        var query = $"SELECT * FROM Transactions";
        var transactions = await _dbConnection.QueryAsync<TransactionDTO>(query);
        var result = GenerateExcelFile(transactions.ToList(), columnsName);

        return result;
    }

    public async Task<List<TransactionDTO>> Get2023TransactionsInUserTZ(string userTimeZone)
    {
        var query = @"
            SELECT *
            FROM Transactions
            WHERE YEAR(TransactionDate) = 2023;
        ";

        var transactionsFor2023 = await _dbConnection.QueryAsync<TransactionDTO>(query);
        var result = new List<TransactionDTO>();
        foreach (var transaction in transactionsFor2023)
        {
            var timeZone = transaction.ClientLocation.ParseToTimeZone();

            if (timeZone == userTimeZone)
                result.Add(transaction);
        }

        return result;
    }

    public async Task<Dictionary<string, List<TransactionDTO>>> Get2023TransactionsByClientTZ()
    {
        var result = new Dictionary<string, List<TransactionDTO>>();

        var query = @"
            SELECT *
            FROM Transactions
            WHERE YEAR(TransactionDate) = 2023;
        ";

        var transactionsFor2023 = await _dbConnection.QueryAsync<TransactionDTO>(query);
        foreach (var transaction in transactionsFor2023)
        {
            var timeZone = transaction.ClientLocation.ParseToTimeZone();

            if (!result.ContainsKey(timeZone))
                result.Add(timeZone, new List<TransactionDTO>());

            result[timeZone].Add(transaction);
        }

        return result;
    }

    public async Task<List<TransactionDTO>> Get2024TransactionsInUserTZ(string userTimeZone)
    {
        var query = @"
            SELECT *
            FROM Transactions
            WHERE YEAR(TransactionDate) = 2024;
        ";

        var transactionsFor2023 = await _dbConnection.QueryAsync<TransactionDTO>(query);
        var result = new List<TransactionDTO>();
        foreach (var transaction in transactionsFor2023)
        {
            var timeZone = transaction.ClientLocation.ParseToTimeZone();

            if (timeZone == userTimeZone)
                result.Add(transaction);
        }

        return result;
    }

    public async Task<Dictionary<string, List<TransactionDTO>>> Get2024TransactionsByClientTZ()
    {
        var result = new Dictionary<string, List<TransactionDTO>>();

        var query = @"
            SELECT *
            FROM Transactions
            WHERE YEAR(TransactionDate) = 2024;
        ";

        var transactionsFor2023 = await _dbConnection.QueryAsync<TransactionDTO>(query);
        foreach (var transaction in transactionsFor2023)
        {
            var timeZone = transaction.ClientLocation.ParseToTimeZone();

            if (!result.ContainsKey(timeZone))
                result.Add(timeZone, new List<TransactionDTO>());

            result[timeZone].Add(transaction);
        }

        return result;
    }

    private byte[] GenerateExcelFile(List<TransactionDTO> transactions, List<string> selectedColumns)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Transactions");

            for (int i = 0; i < selectedColumns.Count; i++)
            {
                worksheet.Cells[1, i + 1].Value = selectedColumns[i];
            }

            int rowIndex = 2;
            foreach (var transaction in transactions)
            {
                for (int i = 0; i < selectedColumns.Count; i++)
                {
                    var columnName = selectedColumns[i];
                    var property = typeof(Entities.Transaction).GetProperty(columnName);

                    if (property == null)
                        throw new Exception($"Property {columnName} not found");

                    var cellValue = property.GetValue(transaction);
                    worksheet.Cells[rowIndex, i + 1].Value = cellValue;
                }
                rowIndex++;
            }

            worksheet.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }
    }
}
