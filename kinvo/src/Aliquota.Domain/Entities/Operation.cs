using Aliquota.Domain.Enums;

namespace Aliquota.Domain.Entities;

public class Operation
{
    public Operation()
    {
        
    }

    public Operation(decimal amount, OperationType type)
    {
        Id = Guid.NewGuid();
        Amount = amount;
        Type = type;
        CreatedAt = DateTime.Now;
    }
    
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public OperationType Type { get; set; }
    public DateTime CreatedAt { get; set; }
}