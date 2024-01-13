namespace PicPay.API.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string DocumentNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsSeller { get; set; }
}