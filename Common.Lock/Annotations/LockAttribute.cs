using AspectCore.DynamicProxy;
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
    public TimeSpan WaitTime { get; } // 等待获取锁的时间。默认-1s代表永远阻塞等待
    public TimeSpan LeaseTime { get; } // 自动释放锁的时间，默认-1s代表不自动释放

    public override bool AllowMultiple => true;

    public LockAttribute(string key)
    {
        base.Order = -1; // 优先于事务注解生效
        _key = key;
        WaitTime = TimeSpan.FromSeconds(-1);
        LeaseTime = TimeSpan.FromSeconds(-1);
    }


    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        var logger = context.ServiceProvider.GetRequiredService<ILogger<LockAttribute>>();
        // 判定要不要执行缓存逻辑
        var invokeLock = NeedInvokeLock(context, out var lockClient);
        var lockKey = context.ExtractDynamicKey(_key, Prefix);

        var waitForeverWhenHeldByOtherThread = WaitTime == TimeSpan.FromSeconds(-1);


        if (invokeLock)
        {
            logger.LogInformation("get key {Key} for lock", lockKey);
            await DoLock(waitForeverWhenHeldByOtherThread, lockClient, logger, lockKey);
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
        finally
        {
            // todo unlock if lock success
        }
    }

    private async Task DoLock(bool waitForeverWhenHeldByOtherThread, ILockClient lockClient, ILogger<LockAttribute> logger, string lockKey)
    {
        var lockResultExpected = true;
        Exception throwable = null;
        // todo lock
        if (waitForeverWhenHeldByOtherThread)
        {
            try
            {
                await lockClient.Lock(WaitTime);
                logger.LogDebug("lock success: {LockName}", lockKey);
            }
            catch (Exception ex)
            {
                lockResultExpected = false;
                throwable = ex;
                logger.LogWarning("lock failed unexpected, lockName: {}, error reason:{}", lockKey, ex.Message);
            }
        }
        else
        {
            // var lockSuccess = true;
            // try
            // {
            //     lockSuccess =  lock.tryLock(waitTime, leaseTime, timeUnit);
            // }
            // catch (Throwable ex)
            // {
            //     lockResultExpected = false;
            //     throwable = ex;
            //     log.error("try lock failed unexpected, lockName: {}, error reason:{}", lockKey, ex.getMessage());
            // }
            //
            // if (!lockSuccess)
            // {
            //     Constructor < ?> constructor = exceptionClass.getConstructor(String.class);
            //     RuntimeException exception = (RuntimeException) constructor.newInstance(exceptionMessage);
            //     log.error("try lock failed: {}", lockKey);
            //     throw exception;
            // }
            // else
            // {
            //     log.debug("try lock success: {}", lockKey);
            // }
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
        lockClient = GetLockClient(context, LockClientName);
        if (lockClient == null)
        {
            throw new DependencyResolutionException($"no lock client for {LockClientName ?? "NoAssignedName"}");
        }

        return lockClient != null;
    }

    private static ILockClient GetLockClient(AspectContext context, string lockClientName)
    {
        var lockClients = context.ServiceProvider.GetServices<ILockClient>();
        var lockClient = lockClients
            .OrderBy(item => item.Order())
            .FirstOrDefault(item => string.IsNullOrEmpty(lockClientName) || item.Name() == lockClientName);
        return lockClient;
    }
}