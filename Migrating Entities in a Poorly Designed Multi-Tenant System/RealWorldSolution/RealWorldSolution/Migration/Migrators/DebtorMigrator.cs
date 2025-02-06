using RealWorldSolution.Models;
using RealWorldSolution.Repositories;

namespace RealWorldSolution.Migration.Migrators;

public class DebtorMigrator(DebtorRepository repository) : IMigrator
{
    public void Migrate(MigrationContext context)
    {
        foreach (var (sourceId, _) in context.GetMappingsFor<Debtor>())
        {
            var debtor = repository.GetById(sourceId);

            debtor.LawFirmId = context.GetMappedId<LawFirm>(debtor.LawFirmId);
            debtor.ClerkId = context.GetMappedId<Clerk>(debtor.ClerkId);

            repository.Update(debtor);
        }
    }
}