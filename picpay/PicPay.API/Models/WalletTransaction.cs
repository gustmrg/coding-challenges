namespace PicPay.API.Models;

public class WalletTransaction
{
    public Guid WalletsId { get; set; }
    public Guid TransactionsId { get; set; }
    public Wallet Wallet { get; set; } = null!;
    public Transaction Transaction { get; set; } = null!;
}