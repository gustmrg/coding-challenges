namespace Aliquota.Domain.Entities;

public class Investiment
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public decimal? InterestAmount { get; set; }
    public IEnumerable<Operation> Operations { get; set; } = new List<Operation>();
    public DateTime CreatedAt { get; set; }
}