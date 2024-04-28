using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Cache;

public class MemoryCacheTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public MemoryCacheTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task MemoryCache()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        await memoryCache.GetOrCreateAsync("key", async ee => await Task.FromResult("value"));

        await Task.CompletedTask;
    }
}