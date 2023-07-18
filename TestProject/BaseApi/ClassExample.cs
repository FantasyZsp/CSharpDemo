using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class ClassExample
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ClassExample(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public async Task ParentTest()
    {
        var son = new Son();
        _testOutputHelper.WriteLine(son.Run());

        await Task.Delay(1000);
    }
}

public interface IParent
{
    public string RunOnce()
    {
        return "Parent";
    }
}

public static class ParentExtensions
{
    public static string Run(this IParent parent)
    {
        return parent.RunOnce();
    }
}

public class Son : IParent
{
    public string RunOnce()
    {
        return "Son";
    }
}