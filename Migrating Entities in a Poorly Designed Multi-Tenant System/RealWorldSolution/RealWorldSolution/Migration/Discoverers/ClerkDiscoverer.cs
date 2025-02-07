using RealWorldSolution.Models;
using RealWorldSolution.Repositories;

namespace RealWorldSolution.Migration.Discoverers;

public class ClerkDiscoverer(ClerkRepository repository) : IDiscoverer
{
    public void Discover(MigrationContext context)
    {
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var lawFirm in context.GetDiscoveredEntitiesOfType<LawFirm>())
        {
            var clerks = repository.GetByLawFirmId(lawFirm.Id);
            foreach (var clerk in clerks)
            {
                if (!context.IsDiscovered(clerk))
                {
                    context.DiscoverEntity(clerk);
                }
            }
        }
    }
}