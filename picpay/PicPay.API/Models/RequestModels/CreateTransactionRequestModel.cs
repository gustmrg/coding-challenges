using System.ComponentModel.DataAnnotations;

namespace PicPay.API.Models.RequestModels;

public class CreateTransactionRequestModel
{
    [Required]
    public decimal Value { get; set; }
    
    [Required]
    public int PayerId { get; set; }
    
    [Required]
    public int PayeeId { get; set; }
}