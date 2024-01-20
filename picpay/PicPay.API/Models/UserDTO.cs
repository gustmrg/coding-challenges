namespace PicPay.API.Models;

public class UserDTO
{
    public UserDTO(Guid id, string fullName, string documentNumber, string email, bool isSeller, Guid walletId)
    {
        Id = id;
        FullName = fullName;
        DocumentNumber = documentNumber;
        Email = email;
        IsSeller = isSeller;
        WalletId = walletId;
    }

    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string DocumentNumber { get; set; }
    public string Email { get; set; }
    public bool IsSeller { get; set; }
    public Guid WalletId { get; set; }
}