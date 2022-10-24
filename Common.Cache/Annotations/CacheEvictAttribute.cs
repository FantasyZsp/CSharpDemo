using AspectCore.DynamicProxy;
using Common.Supports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Cache.Annotations;

[AttributeUsage(AttributeTargets.Method)]
public class CacheEvictAttribute : AbstractInterceptorAttribute
{
    private readonly string _key;
    public string Prefix { get; set; }
    public string CacheClientName { get; set; }
    public bool BeforeInvocation { get; }

    public CacheEvictAttribute(string key)
    {
        base.Order = -1; // 优先于事务注解生效
        _key = key;
        BeforeInvocation = false;
    }

    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        var logger = context.ServiceProvider.GetRequiredService<ILogger<CacheEvictAttribute>>();

        var cacheProperties = context.ServiceProvider.GetRequiredService<CacheProperties>();
        if (!cacheProperties.Enable)
        {
            logger.LogWarning("cache feature has been disable");
            await next(context);
            logger.LogInformation("business success");
            return;
        }

        var cacheKey = context.ExtractDynamicKey(_key, Prefix);
        logger.LogInformation("get key {Key} for evict", cacheKey);

        var commonWritableCache = context.GetCacheClient(CacheClientName);
        if (commonWritableCache == null)
        {
            throw new ArgumentException("no cache client for {Name}", CacheClientName ?? "NoAssignedName");
        }

        if (BeforeInvocation)
        {
            try
            {
                await commonWritableCache.Remove(cacheKey);
                logger.LogWarning("cache Removed for {CacheKey}", cacheKey);
            }
            catch (Exception ignore)
            {
                logger.LogWarning(ignore, "cache remove ex for {CacheKey}. ex {ExMsg}", cacheKey, ignore.Message);
            }
        }

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

        if (!BeforeInvocation)
        {
            try
            {
                await commonWritableCache.Remove(cacheKey);
                logger.LogWarning("cache Removed for {CacheKey}", cacheKey);
            }
            catch (Exception ignore)
            {
                logger.LogWarning(ignore, "cache remove ex for {CacheKey}. ex {ExMsg}", cacheKey, ignore.Message);
            }
        }
    }
}