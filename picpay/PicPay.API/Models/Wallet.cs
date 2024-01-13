using System.Text.Json.Serialization;

namespace PicPay.API.Models;

public class Wallet
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
    public int UserId { get; set; }
    
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}