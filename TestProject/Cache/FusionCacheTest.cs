using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetCommon.Extensions;
using TestProject.BaseApi.Models;
using Xunit;
using Xunit.Abstractions;
using ZiggyCreatures.Caching.Fusion;

namespace TestProject.Cache;

public class FusionCacheTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public FusionCacheTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public async Task TestFusionCache_Concurrent()
    {
        var cache = new FusionCache(new FusionCacheOptions());
        await Parallel.ForEachAsync(Enumerable.Range(0, 100), async (_, __) =>
        {
            await Task.Run(async () =>
            {
                var orSetAsync = await cache.GetOrSetAsync("key", async ee =>
                {
                    _testOutputHelper.WriteLine("invoked");
                    return await Task.FromResult("value");
                }, token: __);
                _testOutputHelper.WriteLine(orSetAsync);
            }, __);
        });


        await Task.CompletedTask;
    }

    /// <summary>
    /// 可以缓存null
    /// </summary>
    [Fact]
    public async Task TestFusionCache_CacheLoadReturnNull()
    {
        var cache = new FusionCache(new FusionCacheOptions());
        var orSetAsync = await cache.GetOrSetAsync("nullable", async _ => await Task.FromResult((string) null));
        Assert.Null(orSetAsync);

        var orSet = cache.GetOrSet("nullable", (string) null);
        Assert.Null(orSet);

        var orSet2 = cache.TryGet<string>("nullable");
        Assert.True(orSet2.HasValue);
        var orSet2Value = orSet2.Value;
        _testOutputHelper.WriteLine((orSet2Value == null).ToString());
    }

    /// <summary>
    /// 缓存项过期时间
    /// </summary>
    [Fact]
    public async Task TestItemExpire()
    {
        var cache = new FusionCache(new FusionCacheOptions());
        var cacheItemWithExpire = await CacheItemWithExpire(cache,4); // 4秒过期,3秒加载
        _testOutputHelper.WriteLine((cacheItemWithExpire.GetValueOrDefault() - DateTime.Now).TotalMilliseconds.ToString());
        var cacheItemWithExpire2 = await CacheItemWithExpire(cache,4); // 4秒过期
        Assert.Equal(cacheItemWithExpire, cacheItemWithExpire2);
        _testOutputHelper.WriteLine((cacheItemWithExpire2.GetValueOrDefault() - DateTime.Now).TotalMilliseconds.ToString());
        
        await Task.Delay(3500);
        var refreshing = await CacheItemWithExpire(cache,4); // 触发加载，返回旧值
        Assert.Equal(cacheItemWithExpire, refreshing);
        await Task.Delay(200);
        var refreshed = await CacheItemWithExpire(cache,4);  // 返回加载完毕的
        Assert.NotEqual(refreshed, refreshing);
        _testOutputHelper.WriteLine((refreshed.GetValueOrDefault() - DateTime.Now).TotalMilliseconds.ToString());

    }

    private static async Task<DateTime?> CacheItemWithExpire(IFusionCache cache, int key)
    {
       return await cache.GetOrSetAsync(key.ToString(), async (FusionCacheFactoryExecutionContext<DateTime?> ctx, CancellationToken ct) =>
        {
            var expireAt = await LoadAndSetExpireTime(key, ctx);
            return expireAt;
        });
    }

    private static async Task<DateTime?> LoadAndSetExpireTime(int key, FusionCacheFactoryExecutionContext<DateTime?> ctx)
    {
        var expireAt = await Task.FromResult(Build(key));
        if (expireAt is null)
        {
            ctx.Options.Duration = TimeSpan.FromMinutes(5);
        }
        else
        {
            var duration = expireAt.GetValueOrDefault() - DateTime.Now ;
            var eagerDuration = duration - TimeSpan.FromSeconds(1);
            ctx.Options.Duration = duration;
            // ctx.Options.FailSafeThrottleDuration = TimeSpan.FromMilliseconds(100); // 故障免load
            // ctx.Options.FailSafeMaxDuration = TimeSpan.FromSeconds(1); // 旧值存在
            ctx.Options.FactorySoftTimeout = TimeSpan.FromMilliseconds(100);
            ctx.Options.FactoryHardTimeout = TimeSpan.FromSeconds(100);
            ctx.Options.EagerRefreshThreshold = (float?) (eagerDuration.TotalMilliseconds / duration.TotalMilliseconds);  
        }

        return expireAt;
    }

    private static DateTime? Build(int count)
    {
        return DateTime.Now.AddSeconds(count);
    }

    /// <summary>
    /// 可以缓存null
    /// </summary>
    [Fact]
    public async Task TestDispose()
    {
        var cache = new FusionCache(new FusionCacheOptions());


        var orSetAsync = await cache.GetOrSetAsync("nullable", async _ => await Task.FromResult((string) null));
        Assert.Null(orSetAsync);
        Task.Run(() => { cache.Dispose(); });
        await Task.Delay(100);

        var orSet = cache.GetOrSet("nullable", (string) null);
        Assert.Null(orSet);

        var orSet2 = cache.TryGet<string>("nullable");
        Assert.True(orSet2.HasValue);
        var orSet2Value = orSet2.Value;
        _testOutputHelper.WriteLine((orSet2Value == null).ToString());
    }

    [Fact]
    public async Task TestFusionCache_Expire()
    {
        using var cache = new FusionCache(new FusionCacheOptions()
        {
            DefaultEntryOptions = new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMilliseconds(1000), // 缓存存在时间
                IsFailSafeEnabled = true
            }
        });
        var name = "123";

        var stopwatch = Stopwatch.StartNew();
        var kid = await cache.GetOrSetAsync(
            "kid:" + name,
            async _ => await GetFromDbAsync(name, 200)
        );
        stopwatch.Stop();
        _testOutputHelper.WriteLine($"{stopwatch.ElapsedMilliseconds}ms {kid.ToJson()}");


        await Task.Delay(1100);

        stopwatch.Restart();
        var kid2 = await cache.GetOrSetAsync(
            "kid:" + name,
            async _ => await GetFromDbAsync(name)
        );

        stopwatch.Stop();
        _testOutputHelper.WriteLine($"{stopwatch.ElapsedMilliseconds}ms {kid2.ToJson()}");

        await Task.CompletedTask;
    }

    /// <summary>
    /// 本地缓存
    /// failsafe
    /// avoidlongtimefactory
    /// </summary>
    [Fact]
    public async Task TestFusionCache_LocalCache()
    {
        using var cache = new FusionCache(new FusionCacheOptions()
        {
            DefaultEntryOptions = new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMinutes(1),

                IsFailSafeEnabled = true,
                FailSafeMaxDuration = TimeSpan.FromHours(2), // FailSafe触发时，旧值最大存活时间
                FailSafeThrottleDuration = TimeSpan.FromSeconds(30), // FailSafe触发时，旧值直接可用且不访问factory的存活时间

                // FACTORY TIMEOUTS
                FactorySoftTimeout = TimeSpan.FromMilliseconds(200), // 此时间内加载不完就使用旧值
                FactoryHardTimeout = TimeSpan.FromMilliseconds(500) // 如果没有旧值，加载最多这么久，否则报错
            }
        });
        var name = "123";

        var stopwatch = new Stopwatch();

        // 第一次缓存miss时，factory执行超过了 FactoryHardTimeout ，触发SyntheticTimeoutException
        await Assert.ThrowsAsync<SyntheticTimeoutException>(async () =>
        {
            var kid = await cache.GetOrSetAsync(BuildCacheKey(name), async _ => await GetFromDbAsync(name, 501));
            _testOutputHelper.WriteLine($"{stopwatch.ElapsedMilliseconds}ms {kid.ToJson()}");
        });

        var orDefaultAsync = await cache.GetOrDefaultAsync(name, Kid.DefaultGGKid);
        Assert.Same(Kid.DefaultGGKid, orDefaultAsync);
        await Task.Delay(510); // 这里等待第一次factory完成时间。但实际上，第一次没有回退值可用，factory执行超过hard后，也不会后台执行任务了。

        var orDefaultAsync2 = await cache.GetOrDefaultAsync(name, Kid.DefaultGGKid);
        Assert.Same(Kid.DefaultGGKid, orDefaultAsync2);


        stopwatch.Restart();
        var kid2 = await cache.GetOrSetAsync(
            "kid:" + name,
            async _ => await GetFromDbAsync(name, 1000)
        );

        stopwatch.Stop();
        _testOutputHelper.WriteLine($"{stopwatch.ElapsedMilliseconds}ms {kid2.ToJson()}");

        await Task.CompletedTask;
    }

    private static string BuildCacheKey(string name)
    {
        return "kid:" + name;
    }

    private async Task<Kid> GetFromDbAsync(string name, int delayTime = 0)
    {
        await Task.Delay(delayTime);
        _testOutputHelper.WriteLine("GetFromDbAsync invoked");
        return Kid.Of(name);
    }
}