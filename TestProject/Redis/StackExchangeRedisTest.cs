using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Spring.Expressions;
using StackExchange.Redis;
using TestProject.SpEL;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Redis;

public class StackExchangeRedisTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public StackExchangeRedisTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Test()
    {
        ThreadPool.SetMinThreads(1, 1);
        ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);

        _testOutputHelper.WriteLine($"workerThreads {workerThreads}, completionPortThreads {completionPortThreads}");
        const string redisString = "127.0.0.1:6379,password=123456,abortConnect=false";
        var redis = await ConnectionMultiplexer.ConnectAsync(redisString);

        if (redis.IsConnected)
        {
            _testOutputHelper.WriteLine("xxx");
        }

        var list = Enumerable.Range(1, 10000).ToList();


        await Parallel.ForEachAsync(list, new ParallelOptions() {MaxDegreeOfParallelism = 1000}, async (number, _) =>
        {
            var stringSetAsync = await redis.GetDatabase(0).StringSetAsync("tmp:testTimeOut:" + number, number, TimeSpan.FromMilliseconds(30000));
            var values =  redis.GetDatabase(0).StringGetAsync("tmp:testTimeOut:" + number);
            // _testOutputHelper.WriteLine(values);
        });
        //
        // var tasks = new List<Task>();
        // for (var i = 0; i < 10000; i++)
        // {
        //     var tmpJ = i;
        //     tasks.Add(
        //         Task.Run(async () =>
        //         {
        //             var stringSetAsync = await redis.GetDatabase(0).StringSetAsync("tmp:testTimeOut:" + tmpJ, tmpJ, TimeSpan.FromMilliseconds(30000));
        //             var values = await redis.GetDatabase(0).StringGetAsync("tmp:testTimeOut:" + tmpJ);
        //             _testOutputHelper.WriteLine(values);
        //         })
        //     );
        // }

        // Task.WaitAll(tasks.ToArray());
        await Task.Delay(1000);
    }
}