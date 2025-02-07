using RealWorldSolution.Models;
using RealWorldSolution.Repositories;

namespace RealWorldSolution.Migration.Discoverers;

public class DebtorDiscoverer(DebtorRepository repository) : IDiscoverer
{
    public void Discover(MigrationContext context)
    {
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var clerk in context.GetDiscoveredEntitiesOfType<Clerk>())
        {
            var debtors = repository.GetByClerkId(clerk.Id);
            foreach (var debtor in debtors)
            {
                if (!context.IsDiscovered(debtor))
                {
                    context.DiscoverEntity(debtor);
                }
            }
        }
    }
}