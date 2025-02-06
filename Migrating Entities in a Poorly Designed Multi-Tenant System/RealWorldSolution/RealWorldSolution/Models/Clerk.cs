namespace RealWorldSolution.Models;

public class Clerk
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid LawFirmId { get; set; }
    
    // Other fields
}