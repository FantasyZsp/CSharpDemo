using System;
using System.Net.Http;
using System.Threading;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using DotNetCommon.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.RateLimit;

public class SharedConcurrencyLimiter
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SharedConcurrencyLimiter(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private async Task<string> SlowOne(HttpClient client)
    {
        var httpResponseMessage = await client.GetAsync("https://decogateway-dev.123kanfang.com/taskprocessapi/houseshape/GetHouseInfo?packageId=huxingtu_8a231177983d48b7bbfef3ae53c0e9f6");
        return httpResponseMessage.ToJson();
    }

    [Fact]
    public async Task TestSlidingWindowRateLimiter()
    {
        _testOutputHelper.WriteLine("Test");
        var slidingWindowRateLimiter = new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions()
        {
            AutoReplenishment = true,
            PermitLimit = 12,
            SegmentsPerWindow = 4,
            QueueLimit = 100,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            Window = TimeSpan.FromSeconds(1)
        });

        var times = 0;
        var timesNo = 0;
        var taskCount = 100;
        var max = taskCount;

        var task = Task.Run(async () =>
        {
            while (taskCount != 0)
            {
                _testOutputHelper.WriteLine($"main loop taskCount is {taskCount}");
                await Task.Delay(1000);
            }

            _testOutputHelper.WriteLine("finish");
        });

        for (int i = 0; i < max; i++)
        {
            await Task.Delay(100);

            Task.Run(async () =>
            {
                var acquireAsync = await slidingWindowRateLimiter.AcquireAsync();
                if (acquireAsync.IsAcquired)
                {
                    var increment = Interlocked.Increment(ref times);

                    _testOutputHelper.WriteLine($"{increment} yes {DateTime.Now:HH:mm:ss.fff}");
                }
                else
                {
                    var increment = Interlocked.Increment(ref timesNo);

                    _testOutputHelper.WriteLine($"{increment} no");
                }

                Interlocked.Decrement(ref taskCount);
            });
        }

        // fixedWindowRateLimiter.AttemptAcquire()

        await task;
        _testOutputHelper.WriteLine("waiting finish");
    }

    [Fact]
    public async Task TestTokenBucketRateLimiter()
    {
        _testOutputHelper.WriteLine("Test");
        var slidingWindowRateLimiter = new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions()
        {
            ReplenishmentPeriod = TimeSpan.FromSeconds(1),
            TokensPerPeriod = 5,
            AutoReplenishment = true,
            TokenLimit = 5,
            QueueLimit = 100,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
        });

        var times = 0;
        var timesNo = 0;
        var taskCount = 100;
        var max = taskCount;

        var task = Task.Run(async () =>
        {
            while (taskCount != 0)
            {
                _testOutputHelper.WriteLine($"main loop taskCount is {taskCount}");
                await Task.Delay(1000);
            }

            _testOutputHelper.WriteLine("finish");
        });

        for (int i = 0; i < max; i++)
        {

            Task.Run(async () =>
            {
                var acquireAsync = await slidingWindowRateLimiter.AcquireAsync();
                if (acquireAsync.IsAcquired)
                {
                    var increment = Interlocked.Increment(ref times);

                    _testOutputHelper.WriteLine($"{increment} yes {DateTime.Now:HH:mm:ss.fff}");
                }
                else
                {
                    var increment = Interlocked.Increment(ref timesNo);

                    _testOutputHelper.WriteLine($"{increment} no");
                }

                Interlocked.Decrement(ref taskCount);
            });
        }

        await task;
        _testOutputHelper.WriteLine("waiting finish");
    }

    [Fact]
    public async Task TestFixedWindowRateLimiter()
    {
        _testOutputHelper.WriteLine("Test");
        var fixedWindowRateLimiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions()
        {
            AutoReplenishment = true, PermitLimit = 5, QueueLimit = 100, QueueProcessingOrder = QueueProcessingOrder.OldestFirst, Window = TimeSpan.FromSeconds(1)
        });

        int times = 0;
        int timesNo = 0;
        int taskCount = 100;
        int max = taskCount;
        for (int i = 0; i < max; i++)
        {
            Task.Run(async () =>
            {
                var acquireAsync = await fixedWindowRateLimiter.AcquireAsync();
                if (acquireAsync.IsAcquired)
                {
                    var increment = Interlocked.Increment(ref times);

                    _testOutputHelper.WriteLine($"{increment} yes");
                }
                else
                {
                    var increment = Interlocked.Increment(ref timesNo);

                    _testOutputHelper.WriteLine($"{increment} no");
                }

                Interlocked.Decrement(ref taskCount);
            });
        }

        // fixedWindowRateLimiter.AttemptAcquire()


        _testOutputHelper.WriteLine("waiting finish");

        while (taskCount != 0)
        {
            _testOutputHelper.WriteLine($"main loop taskCount is {taskCount}");
            await Task.Delay(1000);
        }

        _testOutputHelper.WriteLine("finish");
    }
}