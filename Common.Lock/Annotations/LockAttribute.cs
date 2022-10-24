using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using Autofac.Core;
using Common.Supports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Lock.Annotations;

[AttributeUsage(AttributeTargets.Method)]
public class LockAttribute : AbstractInterceptorAttribute
{
    public string Prefix { get; set; }
    private readonly string _key;
    public string LockClientName { get; set; }
    public TimeSpan WaitTime { get; set; } // 等待获取锁的时间,30s
    public TimeSpan LeaseTime { get; set; } // 自动释放锁的时间，默认-1s代表不自动释放


    public LockAttribute(string key)
    {
        base.Order = -1; // 优先于事务注解生效
        _key = key;
        WaitTime = TimeSpan.FromSeconds(30);
        LeaseTime = TimeSpan.FromSeconds(-1);
    }


    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        var logger = context.ServiceProvider.GetRequiredService<ILogger<LockAttribute>>();
        // 判定要不要执行缓存逻辑
        var invokeLock = NeedInvokeLock(context, out var readableCache);

        var cacheKey = context.ExtractDynamicKey(_key, Prefix);
        if (invokeLock)
        {
            logger.LogInformation("get key {Key} for lock", cacheKey);

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
    }

    /// <summary>
    /// 是否执行缓存逻辑：
    /// 1、目标方法签名有实质返回值；
    /// 2、配置需要缓存；
    /// 3、找得到对应的客户端(ex)
    /// </summary>
    /// <param name="context"></param>
    /// <param name="lockClient"></param>
    /// <returns></returns>
    private bool NeedInvokeLock(AspectContext context, out ILockClient lockClient)
    {
        lockClient = null;
        var methodReturnType = context.GetReturnParameter().Type;
        var isVoidReturn = methodReturnType == typeof(void) || methodReturnType == typeof(Task) || methodReturnType == typeof(ValueTask);

        if (isVoidReturn)
        {
            return false;
        }

        // var cacheProperties = context.ServiceProvider.GetRequiredService<CacheProperties>();
        // if (!cacheProperties.Enable)
        // {
        //     return false;
        // }

        // lockClient = context.GetCacheClient(LockClientName);
        // if (lockClient == null)
        // {
        //     throw new DependencyResolutionException($"no cache client for {LockClientName ?? "NoAssignedName"}");
        // }

        return lockClient != null;
    }
}