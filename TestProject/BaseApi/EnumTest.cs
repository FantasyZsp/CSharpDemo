using System;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class EnumTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public EnumTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test()
    {
        _testOutputHelper.WriteLine(NumEnum.Eight.GetHashCode().ToString());
        _testOutputHelper.WriteLine((NumEnum.Eight.GetHashCode() & 1).ToString());
    }
}

internal enum NumEnum : uint
{
    One = 1 << 0,
    Two = 1 << 1,
    Four = 1 << 2,
    Eight = 1 << 3
}