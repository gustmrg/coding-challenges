namespace PicPay.API.Models;

public class TransactionDTO
{
    public TransactionDTO()
    {
        
    }
    
    public TransactionDTO(Guid id, string amount, UserDTO payer, UserDTO payee, DateTime createdAt)
    {
        Id = id;
        Amount = amount;
        Payer = payer;
        Payee = payee;
        CreatedAt = createdAt;
    }

    public Guid Id { get; set; }
    public string Amount { get; set; }
    public UserDTO Payer { get; set; }
    public UserDTO Payee { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<EntryDTO>? Entries { get; set; }
}