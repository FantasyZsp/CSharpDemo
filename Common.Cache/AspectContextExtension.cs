using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Cache;

public static class AspectContextExtension
{
    public static ICache GetCacheClient(this AspectContext context, string cacheClientName)
    {
        var caches = context.ServiceProvider.GetServices<ICache>();
        var commonWritableCache = caches
            .OrderBy(cache => cache.Order())
            .FirstOrDefault(cache => string.IsNullOrEmpty(cacheClientName) || cache.Name() == cacheClientName);
        return commonWritableCache;
    }
}