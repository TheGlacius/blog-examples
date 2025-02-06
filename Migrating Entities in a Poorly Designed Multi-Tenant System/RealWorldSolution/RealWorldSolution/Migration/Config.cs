namespace RealWorldSolution.Migration;

public class Config
{
    public required Guid SourceLawOfficeId { get; init; }
    public required Guid DestinationLawOfficeId { get; init; }
}