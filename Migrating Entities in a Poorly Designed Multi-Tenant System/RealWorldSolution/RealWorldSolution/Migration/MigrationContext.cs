namespace RealWorldSolution.Migration;

public class MigrationContext()
{
    private readonly Dictionary<Type, Dictionary<Guid, Guid>> _mappings = new();

    public void AddMapping<T>(Guid sourceId, Guid targetId) where T : class
    {
        if (!_mappings.ContainsKey(typeof(T)))
        {
            _mappings[typeof(T)] = new Dictionary<Guid, Guid>();
        }
        _mappings[typeof(T)][sourceId] = targetId;
    }

    public void DiscoverEntity<T>(Guid id) where T : class
    {
        AddMapping<T>(id, id);
    }

    public Guid GetMappedId<T>(Guid sourceId) where T : class
    {
        return _mappings[typeof(T)][sourceId];
    }

    public bool HasMapping<T>(Guid sourceId) where T : class
    {
        return _mappings.TryGetValue(typeof(T), out var map) && map.ContainsKey(sourceId);
    }

    public Dictionary<Guid, Guid> GetMappingsFor<T>() where T : class
    {
        return _mappings.TryGetValue(typeof(T), out var map) ? map : new Dictionary<Guid, Guid>();
    }

    public IEnumerable<Guid> GetEntities()
    {
        return _mappings.Values.SelectMany(m => m.Keys);
    }
}