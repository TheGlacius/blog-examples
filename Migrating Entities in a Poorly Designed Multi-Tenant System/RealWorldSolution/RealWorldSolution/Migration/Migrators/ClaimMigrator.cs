using RealWorldSolution.Models;
using RealWorldSolution.Repositories;

namespace RealWorldSolution.Migration.Migrators;

public class ClaimMigrator(ClaimRepository repository) : IMigrator
{
    public void Migrate(MigrationContext context)
    {
        foreach (var claim in context.GetDiscoveredEntitiesOfType<Claim>())
        {
            var discoveredDebtorParent = context
                .GetDiscoveredEntitiesOfType<Debtor>()
                .SingleOrDefault(debtor => debtor.Id == claim.DebtorId);

            if (discoveredDebtorParent is null)
            {
                continue;
            }
            
            var destinationDebtor = context
                .GetDestinationEntity(discoveredDebtorParent);
            
            claim.LawFirmId = destinationDebtor.LawFirmId;
            claim.DebtorId = destinationDebtor.Id;

            repository.Update(claim);
        }
    }
}