using AspectCore.DynamicProxy;

namespace Common.Supports;

public static class AspectContextExtension
{
    public static string ExtractDynamicKey(this AspectContext context, string spel, string prefix)
    {
        var key = SpelExpressionParserUtils.GenerateKeyByEl(context, spel);

        return string.IsNullOrEmpty(prefix) ? key : prefix + ":" + key;
    }
}