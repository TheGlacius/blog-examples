using Example.Models;
using Example.Repositories;

namespace Example.Migration.Discoverers;

public class ClerkDiscoverer(ClerkRepository repository) : IDiscoverer
{
    public void Discover(MigrationContext context)
    {
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