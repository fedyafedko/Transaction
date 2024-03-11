using Transaction.Common.DTO;

namespace Transaction.BLL.Services.Interfaces;

public interface ITransactionService
{
    Task<byte[]> GetTransactionsFile(List<string> columnsName);
    Task<List<TransactionDTO>> Get2023TransactionsInUserTZ(string userTimeZone);
    Task<List<TransactionDTO>> Get2024TransactionsInUserTZ(string userTimeZone);
    Task<Dictionary<string, List<TransactionDTO>>> Get2023TransactionsByClientTZ();
    Task<Dictionary<string, List<TransactionDTO>>> Get2024TransactionsByClientTZ();
}
