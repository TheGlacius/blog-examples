using RealWorldSolution.Models;
using RealWorldSolution.Repositories;

namespace RealWorldSolution.Migration.Discoverers;

public class ClerkDiscoverer(ClerkRepository repository) : IDiscoverer
{
    public void Discover(MigrationContext context)
    {
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var lawFirmId in context.GetMappingsFor<LawFirm>().Keys)
        {
            var clerks = repository.GetByLawFirmId(lawFirmId);
            foreach (var clerk in clerks)
            {
                if (!context.HasMapping<Clerk>(clerk.Id))
                {
                    context.DiscoverEntity<Clerk>(clerk.Id);
                }
            }
        }
    }
}