using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Locks;

public class LockDemo
{
    private readonly ITestOutputHelper _testOutputHelper;

    public LockDemo(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Test()
    {
        var count = 0;
        var countNoSafe = 0;
        var tasks = new List<Task>();
        for (var i = 0; i < 3000; i++)
        {
            // tasks.Add(Task.Run(() =>
            // {
            //     var tryLock = localLock.TryLock();
            //     if (tryLock)
            //     {
            //         count++;
            //         localLock.UnLock();
            //     }
            // }));

            var i1 = i;
            tasks.Add(Task.Run(() =>
            {
                var localLock = new TinyLock("lock-" + $"{i1 % 1}");
                localLock.Lock();
                count++;
                localLock.UnLock();
            }));
            tasks.Add(Task.Run(() => { countNoSafe++; }));
        }

        Task.WaitAll(tasks.ToArray());
        await Task.CompletedTask;
        _testOutputHelper.WriteLine("count " + count.ToString());
        _testOutputHelper.WriteLine("countNoSafe " + countNoSafe.ToString());
        _testOutputHelper.WriteLine("MemoryLockPool " + TinyLock.LockPool.Count());
    }

    [Fact]
    public async Task TestAsync()
    {
        var count = 0;
        var tasks = new List<Task>();
        for (var i = 0; i < 3000; i++)
        {
            var i1 = i;
            tasks.Add(Task.Run(async () =>
            {
                await using var localLock = new TinyLock("lock-" + $"{i1 % 1}");
                await localLock.LockAsync();
                count++;
                await Task.Delay(1);
                // await localLock.UnLockAsync();
            }));
        }

        Task.WaitAll(tasks.ToArray());
        await Task.CompletedTask;
        _testOutputHelper.WriteLine("count " + count.ToString());
        _testOutputHelper.WriteLine("MemoryLockPool " + TinyLock.LockPool.Count());
    }

    [Fact]
    public async Task TestInitLock()
    {
        for (var i = 0; i < 3000; i++)
        {
            var tinyLock = new TinyLock("lock-" + $"{i}");
            tinyLock.Lock();
        }

        _testOutputHelper.WriteLine("over");
        await Task.CompletedTask;
    }

    [Fact]
    public async Task TestV2()
    {
        var count = 0;
        var tasks = new List<Task>();
        for (var i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                var tryLock = MemoryLockUtil.TryLock("lock");
                if (tryLock)
                {
                    count++;
                    MemoryLockUtil.Unlock("lock");
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                MemoryLockUtil.Lock("lock");
                count++;
                MemoryLockUtil.Unlock("lock");
            }));
        }

        Task.WaitAll(tasks.ToArray());
        await Task.CompletedTask;
        _testOutputHelper.WriteLine(count.ToString());
    }
}