using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi.Tasks;

public class ConcurrentDemo
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ConcurrentDemo(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public async Task TestCalculate()
    {
        _testOutputHelper.WriteLine($"start");
        List<Task> tasks = new List<Task>();
        for (int i = 0; i < 1_0000; i++)
        {
            int k = i;
            tasks.Add(Task.Run(async () => { await Task.Delay(3000); }));
        }

        _testOutputHelper.WriteLine($"finish");
        await Task.Delay(10000);
        Task.WaitAll(tasks.ToArray());
        _testOutputHelper.WriteLine($"finish2");

    }


    [Fact]
    public async Task Test()
    {
        _testOutputHelper.WriteLine($"main with {Thread.CurrentThread.ManagedThreadId}");

        await Method3();

        _testOutputHelper.WriteLine($"main after await with {Thread.CurrentThread.ManagedThreadId}");


        Method1();
        // Method2();
        _testOutputHelper.WriteLine("main sleep");

        Thread.Sleep(3000);
    }

    public async Task Method1()
    {
        // return Task.Run(() =>
        // {
        _testOutputHelper.WriteLine($"Method1 before  Method2() with {Thread.CurrentThread.ManagedThreadId}");


        Method2();
        _testOutputHelper.WriteLine($"Method1 after  Method2() with {Thread.CurrentThread.ManagedThreadId}");

        for (int i = 0; i < 5; i++)
        {
            _testOutputHelper.WriteLine($" Method1 before await with {Thread.CurrentThread.ManagedThreadId}  {i}");
            await Task.Delay(500);
            _testOutputHelper.WriteLine($" Method1 with {Thread.CurrentThread.ManagedThreadId}  {i}");
        }


        // return Task.CompletedTask;
        // });
    }

    public async Task Method2()
    {
        _testOutputHelper.WriteLine($"Method2 begin invoke  await Method3() with {Thread.CurrentThread.ManagedThreadId}");

        await Method3();
        _testOutputHelper.WriteLine($"Method2 after invoke await Method3() with {Thread.CurrentThread.ManagedThreadId}");
        for (int i = 0; i < 25; i++)
        {
            // Task.Delay(1000).Wait();
            _testOutputHelper.WriteLine($" Method2 with {Thread.CurrentThread.ManagedThreadId}  {i}");
        }
    }

    public async Task Method3()
    {
        _testOutputHelper.WriteLine($"invoke Method3() with {Thread.CurrentThread.ManagedThreadId}");

        // await Task.Run(() => { _testOutputHelper.WriteLine($" Method3 with {Thread.CurrentThread.ManagedThreadId}  "); });
        await Task.CompletedTask;

        _testOutputHelper.WriteLine($"finish invoke Method3() with {Thread.CurrentThread.ManagedThreadId}");
    }
}