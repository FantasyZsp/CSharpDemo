using System;
using System.Collections.Concurrent;
using DotNetCommon.Extensions;
using TestProject.BaseApi.Models;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class ConcurrentDictionaryDemo
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ConcurrentDictionaryDemo(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test()
    {
        var concurrentDictionary = new ConcurrentDictionary<string, Kid>();

        concurrentDictionary.TryAdd("zzz", new Kid()
        {
            Name = "zzz",
            Age = 18,
            Sex = SexEnum.GG
        });
        var orAdd = concurrentDictionary.GetOrAdd("zzz", name =>
        {
            _testOutputHelper.WriteLine("compute");
            return new Kid()
            {
                Name = name,
                Age = 18,
                Sex = SexEnum.GG
            };
        });

        concurrentDictionary.TryUpdate("zzz", new Kid()
        {
            Name = "zzz",
            Age = 18,
            Sex = SexEnum.GG
        }, new Kid()
        {
            Name = "zzz",
            Age = 18,
            Sex = SexEnum.MM
        });
        _testOutputHelper.WriteLine(orAdd.ToJson());
    }

    [Fact]
    public void TestGetOrAddNull()
    {
        var concurrentDictionary = new ConcurrentDictionary<string, (Kid, DateTime)?>();

        concurrentDictionary.TryAdd("zzz", (new Kid()
        {
            Name = "zzz",
            Age = 18,
            Sex = SexEnum.GG
        }, DateTime.Now));
        var orAdd = concurrentDictionary.GetOrAdd("zzz1", name => null);
        _testOutputHelper.WriteLine((orAdd == null).ToString());
        _testOutputHelper.WriteLine(concurrentDictionary.ContainsKey("zzz1").ToString());


        var tryGetValue = concurrentDictionary.TryGetValue("zzz1", out var xxx);
        _testOutputHelper.WriteLine(tryGetValue.ToString());
    }
}