namespace PicPay.API.Models;

public class Transaction
{
    public Guid Id { get; set; }
    public decimal Value { get; set; }
    public int PayerId { get; set; }
    public int PayeeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Entry> Entries { get; set; } = new();
}