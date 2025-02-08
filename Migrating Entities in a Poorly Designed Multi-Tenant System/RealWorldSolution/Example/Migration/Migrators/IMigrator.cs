namespace Example.Migration.Migrators;

public interface IMigrator
{
    public void Migrate(MigrationContext context);
}