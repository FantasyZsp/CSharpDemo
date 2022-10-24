using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using Autofac.Core;
using Common.Supports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Cache.Annotations;

/// <summary>
/// 缓存kv。如果kv已存在，直接返回v，不需要执行目标方法；如果不存在，执行目标方法后，将方法返回值作为value存入缓存中。
/// </summary>
/// <remarks>
/// 鸣谢：https://blog.csdn.net/qq_18721495/article/details/117839018，提供了处理异步方法返回值的方式。
/// </remarks>
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
        var logger = context.ServiceProvider.GetRequiredService<ILogger<CacheableAttribute>>();
        // 判定要不要执行缓存逻辑
        var invokeCache = NeedInvokeCache(context, out var readableCache);

        var cacheKey = context.ExtractDynamicKey(_key, Prefix);
        if (invokeCache)
        {
            logger.LogInformation("get key {Key} for cacheable", cacheKey);
            var returnType = context.GetReturnType();

            var method = readableCache.GetType().GetMethod("Get")!.MakeGenericMethod(returnType);

            dynamic task = method!.Invoke(readableCache, new object[] {cacheKey});

            if (task != null)
            {
                var cacheValue = await task!;
                if (cacheValue != null)
                {
                    logger.LogDebug("cacheHits for {CacheKey}", cacheKey);
                    context.ReturnValue = ResultFactory.Get(cacheValue, returnType, context.IsAsync());
                    return; // 缓存命中了，返回
                }
            }

            logger.LogDebug("cacheMiss for {CacheKey}", cacheKey);
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

        var contextReturnValue = await context.ExtractReturnValue();
        try
        {
            await readableCache.Put(cacheKey, contextReturnValue, Ttl);
            logger.LogWarning("cache success for {CacheKey}", cacheKey);
        }
        catch (Exception ignore)
        {
            logger.LogWarning("cache ex for {CacheKey} when puts. ex {ExMsg}", cacheKey, ignore);
        }
    }

    /// <summary>
    /// 是否执行缓存逻辑：
    /// 1、目标方法签名有实质返回值；
    /// 2、配置需要缓存；
    /// 3、找得到对应的客户端(ex)
    /// </summary>
    /// <param name="context"></param>
    /// <param name="readableCache"></param>
    /// <returns></returns>
    private bool NeedInvokeCache(AspectContext context, out ICache readableCache)
    {
        readableCache = null;
        var methodReturnType = context.GetReturnParameter().Type;
        var isVoidReturn = methodReturnType == typeof(void) || methodReturnType == typeof(Task) || methodReturnType == typeof(ValueTask);

        if (isVoidReturn)
        {
            return false;
        }

        var cacheProperties = context.ServiceProvider.GetRequiredService<CacheProperties>();
        if (!cacheProperties.Enable)
        {
            return false;
        }

        readableCache = context.GetCacheClient(CacheClientName);
        if (readableCache == null)
        {
            throw new DependencyResolutionException($"no cache client for {CacheClientName ?? "NoAssignedName"}");
        }

        return readableCache != null;
    }
}