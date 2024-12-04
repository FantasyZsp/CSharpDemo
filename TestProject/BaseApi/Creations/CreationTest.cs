using System;
using System.Diagnostics;
using DotNetCommon.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi.Creations;

public class CreationTest
{
    public static ITestOutputHelper _testOutputHelper;

    public CreationTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void NewInstance()
    {
        var simpleDto = new SimpleDto()
        {
            Age = 20
        };
        _testOutputHelper.WriteLine(simpleDto.ToJsonFast());
    }
}

public class SimpleDto
{
    public int Age { get; set; } = 10;


    public SimpleDto()
    {
        CreationTest._testOutputHelper.WriteLine($"age is {Age}");
    }
}