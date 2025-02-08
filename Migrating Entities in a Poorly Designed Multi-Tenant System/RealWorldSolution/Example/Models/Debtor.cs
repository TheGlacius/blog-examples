namespace Example.Models;

public class Debtor
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid LawFirmId { get; set; }
    public required Guid ClerkId { get; set; }
    
    // Other fields
}