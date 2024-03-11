namespace Transaction.Common.DTO;

public class TransactionDTO
{
    public string TransactionId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public string ClientLocation { get; set; } = string.Empty;
}
