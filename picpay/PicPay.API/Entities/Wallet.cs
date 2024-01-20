using System.Text.Json.Serialization;

namespace PicPay.API.Entities;

public class Wallet
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
    public Guid UserId { get; set; }
    
    [JsonIgnore]
    public virtual User User { get; set; } = null!;

    [JsonIgnore] 
    public List<Transaction> Transactions { get; set; } = new();

    [JsonIgnore] 
    public List<Entry> Entries { get; set; } = new();
}