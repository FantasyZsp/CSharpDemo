using AspectCore.DynamicProxy;
using Common.Supports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Cache;

[AttributeUsage(AttributeTargets.Method)]
public class CacheableAttribute : AbstractInterceptorAttribute
{
    private readonly string _key;
    public string Prefix { get; set; }
    public string CacheClientName { get; set; } // 如何在这里实现用name依赖查找到服务
    public long Ttl { get; set; } // ms

    public CacheableAttribute(string key)
    {
        base.Order = -1; // 优先于事务注解生效
        _key = key;
        Ttl = 30_000; // 30s
    }


    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        var logger = context.ServiceProvider.GetRequiredService<ILogger>();

        var cacheKey = ExtractDynamicKey(context);
        logger.LogInformation("get key {Key}", cacheKey);

        try
        {
            await next(context);
            logger.LogInformation("business success");
        }
        catch (Exception e)
        {
            logger.LogWarning("business ex, so no cache for this request.ex {ExMsg}", e);

            throw;
        }

        var contextReturnValue = await ExtractReturnValue(context);


        var commonWritableCache = GetWritableCache(context);
        if (commonWritableCache == null)
        {
            logger.LogWarning("no cache client for {Name}", CacheClientName ?? "NoAssignedName");
            return;
        }

        try
        {
            await commonWritableCache.Put(cacheKey, contextReturnValue, Ttl);
            logger.LogWarning("cache success for {CacheKey}", cacheKey);
        }
        catch (Exception ignore)
        {
            logger.LogWarning("cache ex for {CacheKey}. ex {ExMsg}", cacheKey, ignore);
        }
    }

    private ICommonWritableCache GetWritableCache(AspectContext context)
    {
        var commonWritableCaches = context.ServiceProvider.GetServices<ICommonWritableCache>();
        var commonWritableCache = commonWritableCaches
            .OrderBy(cache => cache.MyOrder())
            .FirstOrDefault(cache => string.IsNullOrEmpty(CacheClientName) || cache.MyCacheName() == CacheClientName);
        return commonWritableCache;
    }

    private static async Task<object> ExtractReturnValue(AspectContext context)
    {
        var contextReturnValue = context.IsAsync() ? await context.UnwrapAsyncReturnValue() : context.ReturnValue;
        return contextReturnValue;
    }

    private string ExtractDynamicKey(AspectContext context)
    {
        var key = SpelExpressionParserUtils.GenerateKeyByEl(context, _key);

        return string.IsNullOrEmpty(Prefix) ? key : Prefix + ":" + key;
    }
}