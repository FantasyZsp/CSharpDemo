using System;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using TestProject.BaseApi.Models;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Pollys;

public class PollyDemo
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PollyDemo(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task PollyTest()
    {
        var policyFallBack = Policy<Kid>.Handle<KidException>()
            .FallbackAsync(Kid.DefaultGGKid);

        var waitAndRetryAsync = WaitAndRetryAsync();
        var asyncPolicyWrap = policyFallBack.WrapAsync(waitAndRetryAsync);

        var executeAsync = await asyncPolicyWrap.ExecuteAsync(KidException);
        _testOutputHelper.WriteLine(executeAsync.ToString());
    }

    [Fact]
    public async Task PollyRetryTest()
    {
        var waitAndRetryAsync = WaitAndRetryAsync();
        var waitAndRetryAsyncResult = await waitAndRetryAsync.ExecuteAsync(KidException);
        _testOutputHelper.WriteLine(waitAndRetryAsyncResult.ToString());
    }

    [Fact]
    public async Task PollyRetry_Test()
    {
        var kid = await Policy
            // 1. 指定要处理什么异常
            .Handle<Exception>()
            //    或者指定需要处理什么样的错误返回
            .OrResult<Kid>(k => k.Sex == SexEnum.GG)
            // 2. 指定重试次数和重试策略
            .RetryAsync(3, (exception, retryCount, context) =>
            {
                _testOutputHelper.WriteLine(context.ToString());
                _testOutputHelper.WriteLine($"开始第 {retryCount} 次重试：");
            })
            // 3. 执行具体任务
            .ExecuteAsync(() => Task.FromResult(Kid.DefaultGGKid));
        _testOutputHelper.WriteLine(kid.ToString());
    }

    private static AsyncRetryPolicy WaitAndRetryAsync()
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(Math.Min(500, 200 * (int)Math.Pow(2, i - 1))));
    }

    private Task<Kid> KidException()
    {
        _testOutputHelper.WriteLine($"invoked at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
        throw new KidException("error");
    }
}