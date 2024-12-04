using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class ParallelExample
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ParallelExample(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Test()
    {
        var enumerable = Enumerable.Range(1, 100);

        try
        {
            await Parallel.ForEachAsync(enumerable, new ParallelOptions() {MaxDegreeOfParallelism = 10}, async (number, _) =>
            {
                if (number % 2 == 1)
                {
                    throw new ArgumentException($"{number} error");
                }
                else
                {
                    _testOutputHelper.WriteLine($"{number} invoked");
                }
            });
        }
        catch (AggregateException e)
        {
            _testOutputHelper.WriteLine("AggregateException");

            _testOutputHelper.WriteLine(e.Message);
            _testOutputHelper.WriteLine(e.GetType().ToString());
            // throw e;
        }

        catch (Exception e)
        {
            _testOutputHelper.WriteLine("Exception");
            _testOutputHelper.WriteLine(e.Message);
            _testOutputHelper.WriteLine(e.GetType().ToString());
            // throw e;
        }


        await Task.Delay(1000);
    }
}