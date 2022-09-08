using System.Collections.Concurrent;
using System.Reflection;

namespace Common.Supports;

public static class ResultFactory
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> TypeofTaskResultMethod = new();

    public static object Get(object result, Type returnType, bool isAsync)
    {
        if (isAsync)
        {
            return TypeofTaskResultMethod
                .GetOrAdd(returnType, t => typeof(Task)
                    .GetMethods()
                    .First(p => p.Name == "FromResult" && p.ContainsGenericParameters)
                    .MakeGenericMethod(returnType))
                .Invoke(null, new[] {result});
        }

        return result;
    }
}