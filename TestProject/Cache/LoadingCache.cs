using System;
using System.Threading;
using System.Threading.Tasks;
using ZiggyCreatures.Caching.Fusion;

namespace TestProject.Cache;
#nullable enable
public class LoadingCache<TKey, TValue>
{
    public IFusionCache Cache { get; }
    private int LoadSoftTime { get; }
    private int LoadHardTime { get; }
    private int? NullCacheTime { get; init; }
    private readonly Func<TKey, Task<TValue?>> _valueFactory;
    public readonly Func<TKey, Task<string>> KeyBuilder;

    public LoadingCache(Func<TKey, Task<TValue?>> valueFactory, TimeSpan? expireTime = null, Func<TKey, Task<string>>? keyBuilder = null, int softTime = 100, int hardTime = 5000)
    {
        ArgumentNullException.ThrowIfNull(valueFactory);
        keyBuilder = CheckKeyBuilder(keyBuilder);

        _valueFactory = valueFactory;
        KeyBuilder = keyBuilder;
        LoadSoftTime = softTime;
        LoadHardTime = hardTime;

        Cache = new FusionCache(new FusionCacheOptions
        {
            DefaultEntryOptions = new FusionCacheEntryOptions
            {
                Duration = expireTime ?? TimeSpan.FromHours(1), // 缓存时间
                JitterMaxDuration = TimeSpan.FromSeconds(3), // 追加随机过期时间
                EagerRefreshThreshold = 0.8f, // 在百分比时间后如果有访问，就开始提前异步加载缓存

                // 故障回退
                IsFailSafeEnabled = true, // 故障回退旧值
                FailSafeMaxDuration = TimeSpan.FromHours(2), // FailSafe触发时，旧值最大存活时间
                FailSafeThrottleDuration = TimeSpan.FromSeconds(30), // FailSafe触发时，旧值直接可用且不访问factory的存活时间

                // 规避慢加载
                FactorySoftTimeout = TimeSpan.FromMilliseconds(LoadSoftTime), // 此时间内加载不完就使用旧值
                FactoryHardTimeout = TimeSpan.FromMilliseconds(LoadHardTime) // 如果没有旧值，加载最多这么久，否则报错
            }
        });
    }

    public LoadingCache(Func<TKey, Task<TValue?>> valueFactory, IFusionCache cache, Func<TKey, Task<string>>? keyBuilder = null)
    {
        ArgumentNullException.ThrowIfNull(valueFactory);
        keyBuilder = CheckKeyBuilder(keyBuilder);
        _valueFactory = valueFactory;
        KeyBuilder = keyBuilder;
        Cache = cache;
    }

    public async Task<TValue?> GetOrLoadAsync(TKey key)
    {
        ArgumentNullException.ThrowIfNull(key);
        var realKey = await KeyBuilder(key);
        // 调用异步方法获取值
        return await Cache.GetOrSetAsync(realKey, async (FusionCacheFactoryExecutionContext<TValue?> ctx, CancellationToken _) =>
        {
            var value = await _valueFactory(key);

            // 处理null值的超时时间
            if (value is null && NullCacheTime != null)
            {
                ctx.Options.Duration = TimeSpan.FromMilliseconds(NullCacheTime.Value);
            }

            return value;
        });
    }

    internal async Task<Optional<TValue?>> TryGetAsync(TKey key)
    {
        ArgumentNullException.ThrowIfNull(key);
        var realKey = await KeyBuilder(key);
        var maybeValue = await Cache.TryGetAsync<TValue?>(realKey);
        var finalValue = maybeValue.HasValue ? maybeValue.Value : default;
        return Optional<TValue?>.FromHasValue(finalValue, maybeValue.HasValue);
    }

    public async Task<TValue?> GetOrDefaultAsync(TKey key, TValue? defaultValue)
    {
        ArgumentNullException.ThrowIfNull(key);
        var realKey = await KeyBuilder(key);
        return await Cache.GetOrDefaultAsync(realKey, defaultValue);
    }


    private static Func<TKey, Task<string>> CheckKeyBuilder(Func<TKey, Task<string>>? keyBuilder)
    {
        if (keyBuilder == null && typeof(TKey) == typeof(string))
        {
            keyBuilder = DefaultStringBuilder;
        }
        else
        {
            ArgumentNullException.ThrowIfNull(keyBuilder);
        }

        return keyBuilder;
    }

    public static readonly Func<TKey, Task<string>> DefaultStringBuilder = str => Task.FromResult(str?.ToString())!;
}