namespace PicPay.API.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string DocumentNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsSeller { get; set; }
    public Wallet Wallet { get; set; }
}