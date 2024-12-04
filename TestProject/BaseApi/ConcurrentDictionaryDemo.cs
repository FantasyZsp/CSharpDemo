using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
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
        Assert.True(orAdd == null);
        Assert.True(concurrentDictionary.ContainsKey("zzz1"));
        var tryGetValue = concurrentDictionary.TryGetValue("zzz1", out var xxx);
        Assert.True(tryGetValue);
        Assert.True(xxx == null);
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

    [Fact]
    public void DicNullKey()
    {
        var concurrentDictionary = new ConcurrentDictionary<(string, string), Kid>();
        concurrentDictionary.TryAdd(("1", "1"), new Kid()
        {
            Name = "newName",
            Age = 1,
            Sex = SexEnum.MM
        });
        var value = concurrentDictionary.FirstOrDefault(e => e.Key.Item1 == "2").Value;

        _testOutputHelper.WriteLine(value?.ToString());
    }
}