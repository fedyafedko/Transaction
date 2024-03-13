using Transaction.Common.DTO;
using Transaction.Common.Requests;
using Transaction.Common.Responses;

namespace Transaction.BLL.Services.Interfaces;

public interface ITransactionService
{
    TimeZoneResponse GetTimeZoneByLocation(LocationRequest request);
    Task<byte[]> GetTransactionsFile(List<string> columnsName);
    Task<List<TransactionDTO>> Get2023TransactionsInUserTZ(string userTimeZone);
    Task<List<TransactionDTO>> Get2024TransactionsInUserTZ(string userTimeZone);
    Task<Dictionary<string, List<TransactionDTO>>> Get2023TransactionsByClientTZ();
    Task<Dictionary<string, List<TransactionDTO>>> Get2024TransactionsByClientTZ();
}
