using RealWorldSolution.Models;
using RealWorldSolution.Repositories;

namespace RealWorldSolution.Migration.Migrators;

public class ClaimMigrator(ClaimRepository repository) : IMigrator
{
    public void Migrate(MigrationContext context)
    {
        foreach (var (sourceId, _) in context.GetMappingsFor<Claim>())
        {
            var claim = repository.GetById(sourceId);

            claim.LawFirmId = context.GetMappedId<LawFirm>(claim.LawFirmId);
            claim.DebtorId = context.GetMappedId<Debtor>(claim.DebtorId);
            
            repository.Update(claim);
        }
    }
}