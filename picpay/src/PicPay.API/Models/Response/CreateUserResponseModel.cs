using PicPay.API.Entities;

namespace PicPay.API.Models.Response;

public class CreateUserResponseModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string DocumentNumber { get; set; }
    public string Email { get; set; }
    public bool IsSeller { get; set; }
    public Wallet Wallet { get; set; }
}