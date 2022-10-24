namespace Common.Cache;

public interface ICache : IWritableCache<string, object>, IReadableCache<string>
{
    string MyCacheName();
    int MyOrder();

    Task PutAsync(string key, object value, long ttl);
}