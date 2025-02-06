using RealWorldSolution.Models;
using RealWorldSolution.Repositories;

namespace RealWorldSolution.Migration.Discoverers;

public class DebtorDiscoverer(DebtorRepository repository) : IDiscoverer
{
    public void Discover(MigrationContext context)
    {
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var clerkId in context.GetMappingsFor<Clerk>().Keys)
        {
            var debtors = repository.GetByClerkId(clerkId);
            foreach (var debtor in debtors)
            {
                if (!context.HasMapping<Debtor>(debtor.Id))
                {
                    context.DiscoverEntity<Debtor>(debtor.Id);
                }
            }
        }
    }
}