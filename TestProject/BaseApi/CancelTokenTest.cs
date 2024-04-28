using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class CancelTokenTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CancelTokenTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CancelTest()
    {
        var cancellationToken = new CancellationTokenSource();
        var delayTask = Task.Delay(1000, cancellationToken.Token);
        cancellationToken.Cancel();
        Assert.True(delayTask.IsCanceled);
        Assert.True(delayTask.IsCompleted);
        Assert.False(delayTask.IsFaulted);
        await Assert.ThrowsAsync<TaskCanceledException>(async () => { await delayTask; });
    }

    [Fact]
    public async Task CancelEx_Test()
    {
        var cancellationToken = new CancellationTokenSource();
        var delayTask = Task.Delay(1000, cancellationToken.Token);


        var asyncTask = Task.Run(async () =>
        {
            try
            {
                await delayTask;
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.Message);
            }
        });

        cancellationToken.Cancel();
        Assert.True(delayTask.IsCanceled);
        Assert.True(delayTask.IsCompleted);
        Assert.False(delayTask.IsFaulted);
        await Assert.ThrowsAsync<TaskCanceledException>(async () => { await delayTask; });

        await asyncTask;
    }
}