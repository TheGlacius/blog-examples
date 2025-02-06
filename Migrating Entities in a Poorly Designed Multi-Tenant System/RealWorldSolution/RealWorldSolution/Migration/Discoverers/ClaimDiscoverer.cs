using RealWorldSolution.Models;
using RealWorldSolution.Repositories;

namespace RealWorldSolution.Migration.Discoverers;

public class ClaimDiscoverer(ClaimRepository repository) : IDiscoverer
{
    public void Discover(MigrationContext context)
    {
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var debtorId in context.GetMappingsFor<Debtor>().Keys)
        {
            var claims = repository.GetByDebtorId(debtorId);
            foreach (var claim in claims)
            {
                if (!context.HasMapping<Claim>(claim.Id))
                {
                    context.DiscoverEntity<Claim>(claim.Id);
                }
            }
        }
    }
}