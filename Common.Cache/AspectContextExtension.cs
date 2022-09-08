using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Cache;

public static class AspectContextExtension
{
    public static Type GetReturnType(this AspectContext context)
    {
        return context.IsAsync() ? context.ServiceMethod.ReturnType.GetGenericArguments().First() : context.ServiceMethod.ReturnType;
    }


    public static async Task<object> ExtractReturnValue(this AspectContext context)
    {
        var contextReturnValue = context.IsAsync() ? await context.UnwrapAsyncReturnValue() : context.ReturnValue;
        return contextReturnValue;
    }

    public static ICache GetCacheClient(this AspectContext context, string cacheClientName)
    {
        var caches = context.ServiceProvider.GetServices<ICache>();
        var commonWritableCache = caches
            .OrderBy(cache => cache.MyOrder())
            .FirstOrDefault(cache => string.IsNullOrEmpty(cacheClientName) || cache.MyCacheName() == cacheClientName);
        return commonWritableCache;
    }
}