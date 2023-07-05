using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi.Tasks;

public class TaskDemo
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TaskDemo(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Test()
    {
        var guid = GetGuid();
        _testOutputHelper.WriteLine($" {Thread.CurrentThread.IsThreadPoolThread} [{Thread.CurrentThread.Name}] [{Thread.CurrentThread.ManagedThreadId}]");


        // var task = Task.Run(async () =>
        // {
        //     for (int i = 0; i < 10; i++)
        //     {
        //         Console.WriteLine($"{await GetGuid()} {Thread.CurrentThread.IsThreadPoolThread} {Thread.CurrentThread.Name} {Thread.CurrentThread.ManagedThreadId}");
        //     }
        // });

        await guid;

        await Task.Delay(2000);
    }

    private async Task<string> GetGuid()
    {
        await Task.Delay(1000);

        var result = Guid.NewGuid().ToString();
        _testOutputHelper.WriteLine(result);
        await Go();
        return result;
    }

    private async Task Go()
    {
        await Task.CompletedTask;
        _testOutputHelper.WriteLine($"Go invoked by {Thread.CurrentThread.Name} {Thread.CurrentThread.ManagedThreadId}");
    }
}