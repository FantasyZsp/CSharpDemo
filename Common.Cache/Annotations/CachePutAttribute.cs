using AspectCore.DynamicProxy;
using Common.Supports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Cache.Annotations;

[AttributeUsage(AttributeTargets.Method)]
public class CachePutAttribute : AbstractInterceptorAttribute
{
    private readonly string _key;
    public string Prefix { get; set; }
    public string CacheClientName { get; set; } // 如何在这里实现用name依赖查找到服务
    public long Ttl { get; set; } // ms

    public CachePutAttribute(string key)
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

        var contextReturnValue = await context.ExtractReturnValue();


        var commonWritableCache = context.GetCacheClient(CacheClientName);
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


    private string ExtractDynamicKey(AspectContext context)
    {
        var key = SpelExpressionParserUtils.GenerateKeyByEl(context, _key);

        return string.IsNullOrEmpty(Prefix) ? key : Prefix + ":" + key;
    }
}