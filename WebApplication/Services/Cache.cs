using System.Collections.Concurrent;
using System.Threading.Tasks;
using Common.Cache;

namespace WebApplication.Services;

public class Cache : ICache
{
    /// <summary>
    /// 需要支持淘汰策略，避免OOM
    /// </summary>
    private static readonly ConcurrentDictionary<string, object> CacheHolder = new();

    public async Task<object> Put(string key, object value, long ttl)
    {
        var hasValue = CacheHolder.TryGetValue(key, out var oldValue);
        CacheHolder[key] = value;
        return hasValue ? oldValue : null;
    }

    public async Task<bool> Remove(string key)
    {
        return CacheHolder.TryRemove(key, out _);
    }

    // public async Task<object> Get(string key)
    // {
    //     var hasValue = CacheHolder.TryGetValue(key, out var oldValue);
    //     return hasValue ? oldValue : null;
    // }

    public async Task<TV> Get<TV>(string key)
    {
        var hasValue = CacheHolder.TryGetValue(key, out var oldValue);
        return hasValue ? (TV) oldValue : default;
    }

    public string MyCacheName()
    {
        return "defaultSimpleCache";
    }

    public int MyOrder()
    {
        return 1;
    }

    public async Task PutAsync(string key, object value, long ttl)
    {
        CacheHolder[key] = value;
    }
}