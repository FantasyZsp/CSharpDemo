using System;
using System.Threading.Tasks;
using Flurl.Http;
using Polly;
using Polly.Fallback;
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
    public async Task PollyRetryTest2()
    {
        try
        {
            var res = await Policy<Kid>
                .Handle<Exception>()
                .FallbackAsync(_ => throw new FormatException("test"))
                .WrapAsync(Policy
                    .Handle<RetryableException>()
                    .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Min(1, i * 3)), (exception, _, retryCount, _) => { _testOutputHelper.WriteLine(retryCount.ToString()); }))
                // var res = await Policy
                //     .Handle<RetryableException>()
                //     .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Max(2, i * 3)), (exception, _, retryCount, _) =>
                //     {
                //         _testOutputHelper.WriteLine(retryCount.ToString());
                //     })
                //     .WrapAsync(FallbackPolicy<Kid>
                //         .Handle<Exception>()
                //         .FallbackAsync(_ => throw new FormatException("test")))
                .ExecuteAsync(async () =>
                {
                    try
                    {
                        _testOutputHelper.WriteLine("invoke");
                        await Task.CompletedTask;
                        DoSth();
                        return Kid.DefaultGGKid;
                    }
                    catch (FlurlHttpException ex) when (ex.StatusCode != 200)
                    {
                        throw new RetryableException(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new RetryableException(ex.Message);
                    }
                });
            _testOutputHelper.WriteLine(res.ToString());
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
        }

        _testOutputHelper.WriteLine("finish");
    }

    public void DoSth()
    {
        throw new RetryableException("xxx");
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
                _testOutputHelper.WriteLine((exception.Exception == null).ToString());
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
            .WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(Math.Min(500, 200 * (int) Math.Pow(2, i - 1))));
    }

    private Task<Kid> KidException()
    {
        _testOutputHelper.WriteLine($"invoked at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
        throw new KidException("error");
    }
}

public class RetryableException : Exception
{
    public RetryableException(string message)
        : base(message)
    {
    }
}