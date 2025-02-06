using RealWorldSolution.Migration.Discoverers;

namespace RealWorldSolution.Migration.Migrators;

public interface IMigrator
{
    public void Migrate(Migration.MigrationContext context);
}