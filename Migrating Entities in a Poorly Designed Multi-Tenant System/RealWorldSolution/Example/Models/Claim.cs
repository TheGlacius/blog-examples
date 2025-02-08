namespace Example.Models;

public class Claim
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required Guid LawFirmId { get; set; }
    public required Guid DebtorId { get; set; }
    
    // Other fields
}