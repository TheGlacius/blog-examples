using RealWorldSolution.Models;
using RealWorldSolution.Repositories;

namespace RealWorldSolution.Migration.Migrators;

public class ClerkMigrator(ClerkRepository repository) : IMigrator
{
    public void Migrate(MigrationContext context)
    {
        foreach (var clerk in context.GetDiscoveredEntitiesOfType<Clerk>())
        {
            var discoveredLawFirmParent = context
                .GetDiscoveredEntitiesOfType<LawFirm>()
                .SingleOrDefault(lawFirm => lawFirm.Id == clerk.LawFirmId);
            
            if (discoveredLawFirmParent is null)
            {
                continue;
            }

            var destinationLawFirm = context
                .GetDestinationEntity(discoveredLawFirmParent);
            
            clerk.LawFirmId = destinationLawFirm.Id;

            repository.Update(clerk);
        }
    }
}