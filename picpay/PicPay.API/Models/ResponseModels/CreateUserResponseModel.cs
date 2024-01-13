namespace PicPay.API.Models.ResponseModels;

public class CreateUserResponseModel
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string DocumentNumber { get; set; }
    public string Email { get; set; }
    public bool IsSeller { get; set; }
    public Wallet Wallet { get; set; }
}