using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Transaction.Entities;

public class Transaction
{
    [Key]
    [Name("transaction_id")]
    public string TransactionId { get; set; } = string.Empty;
    [Name("name")]
    public string Name { get; set; } = string.Empty;
    [Name("email")]
    public string Email { get; set; } = string.Empty;
    [Name("amount")]
    public string Amount{ get; set; } = string.Empty;
    [Name("transaction_date")]
    public DateTime TransactionDate { get; set; }
    [Name("client_location")]
    public string ClientLocation { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
