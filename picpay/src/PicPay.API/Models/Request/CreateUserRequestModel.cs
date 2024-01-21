using System.ComponentModel.DataAnnotations;

namespace PicPay.API.Models.Request;

public class CreateUserRequestModel
{
    public CreateUserRequestModel(string fullName, string documentNumber, string email, string password)
    {
        FullName = fullName;
        DocumentNumber = documentNumber;
        Email = email;
        Password = password;
    }
    
    [Required]
    public string FullName { get; set; }
    
    [Required]
    public string DocumentNumber { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}