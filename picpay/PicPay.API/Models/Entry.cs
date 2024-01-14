using System.Text.Json.Serialization;

namespace PicPay.API.Models;

public class Entry
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public Guid TransactionId { get; set; }
    public Guid WalletId { get; set; }
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public virtual Transaction Transaction { get; set; } = null!;
    public virtual Wallet Wallet { get; set; } = null!;
}