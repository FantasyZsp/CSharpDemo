using AspectCore.DynamicProxy;
using Common.Supports.Exceptions;
using Spring.Expressions;

namespace Common.Supports;

public static class SpelExpressionParserUtils
{
    public static string GenerateKeyByEl(AspectContext context, string expression)
    {
        if (expression is null or "")
        {
            throw new ExtractKeyException("expression is required");
        }

        var contextParameterValues = context.Parameters;
        var parametersLength = contextParameterValues.Length;
        var parameterInfos = context.ImplementationMethod.GetParameters();

        var paramMap = new Dictionary<string, object>(parametersLength);
        for (var i = 0; i < parametersLength; i++)
        {
            paramMap[parameterInfos[i].Name!] = contextParameterValues[i];
        }

        var key = (string) ExpressionEvaluator.GetValue(null, expression, paramMap);

        if (string.IsNullOrEmpty(key))
        {
            throw new ExtractKeyException("key is required");
        }

        return key;
    }
}