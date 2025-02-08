namespace Example.Migration.Discoverers;

public interface IDiscoverer
{
    public void Discover(MigrationContext context);
}