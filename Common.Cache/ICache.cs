using Common.Supports;

namespace Common.Cache;

public interface ICache : IWritableCache<string, object>, IReadableCache<string>, IOrdered, INamed
{
    Task PutAsync(string key, object value, long ttl);
}