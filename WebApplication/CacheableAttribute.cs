using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Newtonsoft.Json;
using Spring.Expressions;

namespace WebApplication;

[AttributeUsage(AttributeTargets.Method)]
public class CacheableAttribute : AbstractInterceptorAttribute
{
    private readonly string _key;
    private readonly string _prefix;

    public CacheableAttribute(string prefix, string key)
    {
        base.Order = -1; // 优先于事务注解生效
        _prefix = prefix;
        _key = key;
    }

    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        var contextParameterValues = context.Parameters;
        var parametersLength = contextParameterValues.Length;
        var parameterInfos = context.ImplementationMethod.GetParameters();

        var paramMap = new Dictionary<string, object>(parametersLength);
        for (var i = 0; i < parametersLength; i++)
        {
            paramMap[parameterInfos[i].Name!] = contextParameterValues[i];
        }

        Console.WriteLine(JsonConvert.SerializeObject(paramMap));

        var cacheKey = GetCacheKey(paramMap);
        Console.WriteLine(cacheKey);
        await next(context);

        var contextReturnValue =  context.IsAsync() ? await context.UnwrapAsyncReturnValue() : context.ReturnValue;

        Console.WriteLine(JsonConvert.SerializeObject(contextReturnValue));
        // todo cache
    }

    private string GetCacheKey(IDictionary<string, object> paramMap)
    {
        var key = (string) ExpressionEvaluator.GetValue(null, _key, paramMap);

        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("cache key is required");
        }

        return _prefix + ":" + key;
    }
}