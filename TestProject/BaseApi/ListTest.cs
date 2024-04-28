using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCommon.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class ListTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ListTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test_DateTime_Equals()
    {
        var ints = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            ints.Add(i);
        }


        ints.RemoveAt(0);
        ints.RemoveAt(8);
        _testOutputHelper.WriteLine(ints.ToJson());
    }


    [Fact]
    public async Task Test_SelectAsync()
    {
        var its = new List<int>();
        for (var i = 0; i < 10; i++)
        {
            its.Add(i);
        }


        var whenAll = await Task.WhenAll(its.Select(async ii => await MyTask(ii)));
        var json = whenAll.ToJson();
        _testOutputHelper.WriteLine(json);
    }

    private static async Task<string> MyTask(object input)
    {
        await Task.Delay(1000);
        return await Task.FromResult(input.ToString());
    }
}