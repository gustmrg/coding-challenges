namespace PicPay.API.Models;

public class EntryDTO
{
    public EntryDTO(Guid id, string amount, Guid transactionId, Guid walletId, DateTime createdAt)
    {
        Id = id;
        Amount = amount;
        TransactionId = transactionId;
        WalletId = walletId;
        CreatedAt = createdAt;
    }

    public Guid Id { get; set; }
    public string Amount { get; set; }
    public Guid TransactionId { get; set; }
    public Guid WalletId { get; set; }
    public DateTime CreatedAt { get; set; }
}