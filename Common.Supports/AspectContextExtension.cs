using AspectCore.DynamicProxy;

namespace Common.Supports;

public static class AspectContextExtension
{
    public static string ExtractDynamicKey(this AspectContext context, string spel, string prefix)
    {
        var key = SpelExpressionParserUtils.GenerateKeyByEl(context, spel);

        return string.IsNullOrEmpty(prefix) ? key : prefix + ":" + key;
    }

    public static Type GetReturnType(this AspectContext context)
    {
        return context.IsAsync() ? context.ServiceMethod.ReturnType.GetGenericArguments().First() : context.ServiceMethod.ReturnType;
    }


    public static async Task<object> ExtractReturnValue(this AspectContext context)
    {
        var contextReturnValue = context.IsAsync() ? await context.UnwrapAsyncReturnValue() : context.ReturnValue;
        return contextReturnValue;
    }
}