using System;
using System.Collections;
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

    [Fact]
    public void TestSetSameKey()
    {
        var concurrentDictionary = new ConcurrentDictionary<string, Kid>();
        concurrentDictionary.TryAdd("1", new Kid()
        {
            Name = "newName",
            Age = 1,
            Sex = SexEnum.MM
        });
        concurrentDictionary.TryAdd("1", Kid.DefaultGGKid);
        concurrentDictionary.TryAdd("2", Kid.DefaultGGKid);
        Assert.Throws<ArgumentException>(() => ((IDictionary) concurrentDictionary).Add("1", Kid.DefaultGGKid));


        concurrentDictionary["1"] = Kid.DefaultGGKid;

        _testOutputHelper.WriteLine(concurrentDictionary.ToJson());
    }
}