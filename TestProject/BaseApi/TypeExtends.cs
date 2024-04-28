using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class TypeExtends
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TypeExtends(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test()
    {
        var three = new Three();
        _testOutputHelper.WriteLine(three.Name);
    }
}

internal abstract class One
{
    public readonly string Name;

    internal One()
    {
        Name = GetType().Name.ToString();
    }
}

internal abstract class Two : One
{
}

internal class Three : Two
{
}