namespace PicPay.API.Models;

public class Transaction
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public int PayerId { get; set; }
    public int PayeeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Entry> Entries { get; set; } = new();
    public List<Wallet> Wallets { get; set; } = new();
}