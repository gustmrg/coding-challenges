using System.ComponentModel.DataAnnotations;

namespace PicPay.API.Models.Request;

public class CreateTransactionRequestModel
{
    [Required]
    public decimal Value { get; set; }
    
    [Required]
    public Guid PayerId { get; set; }
    
    [Required]
    public Guid PayeeId { get; set; }
}