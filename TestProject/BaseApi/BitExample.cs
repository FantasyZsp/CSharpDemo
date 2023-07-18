using System;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class BitExample
{
    private readonly ITestOutputHelper _testOutputHelper;

    public BitExample(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public void BitAndTest()
    {
        int a = 1032;
        int b = 8;

        _testOutputHelper.WriteLine((a & ~b).ToString());
    }
}