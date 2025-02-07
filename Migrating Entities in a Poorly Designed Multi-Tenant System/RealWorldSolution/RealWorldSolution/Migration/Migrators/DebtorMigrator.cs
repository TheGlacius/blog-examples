using RealWorldSolution.Models;
using RealWorldSolution.Repositories;

namespace RealWorldSolution.Migration.Migrators;

public class DebtorMigrator(DebtorRepository repository) : IMigrator
{
    public void Migrate(MigrationContext context)
    {
        foreach (var debtor in context.GetDiscoveredEntitiesOfType<Debtor>())
        {
            var discoveredClerkParent = context
                .GetDiscoveredEntitiesOfType<Clerk>()
                .SingleOrDefault(clerk => clerk.Id == debtor.ClerkId);

            if (discoveredClerkParent is null)
            {
                continue;
            }
            
            var destinationClerk = context
                .GetDestinationEntity(discoveredClerkParent);
            
            debtor.LawFirmId = destinationClerk.LawFirmId;
            debtor.ClerkId = destinationClerk.Id;

            repository.Update(debtor);
        }
    }
}