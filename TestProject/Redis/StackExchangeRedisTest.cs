using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetCommon.Extensions;
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
        ThreadPool.SetMinThreads(50, 1);
        ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);

        _testOutputHelper.WriteLine($"workerThreads {workerThreads}, completionPortThreads {completionPortThreads}");
        const string redisString = "127.0.0.1:6379,password=123456,abortConnect=false";
        // var options = ConfigurationOptions.Parse(redisString);

        // options.SocketManager =
        //     new SocketManager("test", 100, SocketManager.SocketManagerOptions.UseThreadPool);

        var redis = await ConnectionMultiplexer.ConnectAsync(redisString);

        if (redis.IsConnected)
        {
            _testOutputHelper.WriteLine("xxx");
        }

        var list1 = Enumerable.Range(1, 50).ToList();
        var syncOps = Parallel.ForEachAsync(list1, new ParallelOptions() {MaxDegreeOfParallelism = 100}, async (number, _) => { await SyncOp(); });

        var list = Enumerable.Range(1, 500).ToList();

        int count = 0;
        var forEachAsync = Parallel.ForEachAsync(list, new ParallelOptions() {MaxDegreeOfParallelism = 100}, async (number, _) =>
        {
            var increment = Interlocked.Increment(ref count);
            _testOutputHelper.WriteLine($"{increment}");
            try
            {
                var lockKey = (increment % 2).ToString();
                if (await GetLockAsync(lockKey))
                {
                    await UnLockAsync(lockKey);
                }
                var stringSetAsync = await redis.GetDatabase(0).StringSetAsync("tmp:testTimeOut1:" + number, number, TimeSpan.FromMilliseconds(30000));
                var values = await redis.GetDatabase(0).StringGetAsync("tmp:testTimeOut1:" + number);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
                throw;
            }
        });

        await syncOps;

        await forEachAsync;

        await Task.Delay(1000);
        _testOutputHelper.WriteLine("finish");
        return;

        async Task<bool> GetLockAsync(string lockKey, TimeSpan? expireTime = null)
        {
            expireTime ??= TimeSpan.FromSeconds(30);
            while (!await redis.GetDatabase(0).StringSetAsync(lockKey, "1", expireTime, When.NotExists))
            {
                await Task.Delay(100);
            }

            return true;
        }

        async Task<bool> UnLockAsync(string lockKey)
        {
            await redis.GetDatabase(0).KeyDeleteAsync(lockKey);

            return true;
        }
    }

    async Task SyncOp()
    {
        // _testOutputHelper.WriteLine("SyncOp");
        await Task.CompletedTask;
        Thread.Sleep(100);
    }
}