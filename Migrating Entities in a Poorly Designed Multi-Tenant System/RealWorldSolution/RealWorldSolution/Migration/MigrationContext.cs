namespace RealWorldSolution.Migration;

public class MigrationContext
{
    private readonly Dictionary<Type, Dictionary<object, object>> _mappings = new();

    public IEnumerable<T> GetDiscoveredEntitiesOfType<T>() where T : class
    {
        _mappings
            .TryGetValue(typeof(T), out var discoveredEntities);
            
        return discoveredEntities?
            .Keys
            .Select(o => (T) o) ?? [];
    }

    public void RegisterMapping<T>(T source, T destination) where T : class
    {
        if (!_mappings.ContainsKey(typeof(T)))
        {
            _mappings[typeof(T)] = new Dictionary<object, object>();
        }
        _mappings[typeof(T)].Add(source, destination);
    }
    
    public void DiscoverEntity<T>(T entity) where T : class
    {
        if (!_mappings.ContainsKey(typeof(T)))
        {
            _mappings[typeof(T)] = [];
        }
        _mappings[typeof(T)].Add(entity, entity);
    }

    public bool IsDiscovered<T>(T entity) where T : class
    {
        return _mappings
            .TryGetValue(typeof(T), out var discoveredEntitiesOfType) && 
               discoveredEntitiesOfType.ContainsKey(entity);
    }

    public T GetDestinationEntity<T>(T entity) where T : class
    {
        return (T) _mappings[typeof(T)][entity];
    }

    public int GetDiscoveredCount()
    {
        return _mappings
            .Values
            .SelectMany(mappings => mappings.Values)
            .Count();
    }
}