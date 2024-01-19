namespace PicPay.API.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public Guid PayerId { get; set; }
    public Guid PayeeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Entry> Entries { get; set; } = new();
    public List<Wallet> Wallets { get; set; } = new();
}