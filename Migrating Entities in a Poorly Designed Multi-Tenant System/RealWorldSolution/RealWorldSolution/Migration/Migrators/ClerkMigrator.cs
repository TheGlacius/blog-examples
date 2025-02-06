using RealWorldSolution.Models;
using RealWorldSolution.Repositories;

namespace RealWorldSolution.Migration.Migrators;

public class ClerkMigrator(ClerkRepository repository) : IMigrator
{
    public void Migrate(MigrationContext context)
    {
        foreach (var (sourceId, _) in context.GetMappingsFor<Clerk>())
        {
            var clerk = repository.GetById(sourceId);

            clerk.LawFirmId = context.GetMappedId<LawFirm>(clerk.LawFirmId);

            repository.Update(clerk);
        }
    }
}