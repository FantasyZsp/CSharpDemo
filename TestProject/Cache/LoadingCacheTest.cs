using System;
using System.Threading.Tasks;
using TestProject.BaseApi.Models;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Cache;
#nullable enable
public class LoadingCacheTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public LoadingCacheTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task LoadingCacheTestCase()
    {
        var loadingCache = new LoadingCache<string, string>(Task.FromResult!, TimeSpan.FromMilliseconds(1000));
        var async = await loadingCache.GetOrLoadAsync("string");
        _testOutputHelper.WriteLine(async);

        var tryGetAsync = await loadingCache.TryGetAsync("xxxx");
        _testOutputHelper.WriteLine(tryGetAsync.HasValue.ToString());

        string? content = await loadingCache.TryGetAsync("xxxx");
        var content2 = await loadingCache.TryGetAsync("xxxx");
        Assert.Null(content);
        Assert.True(content2 == null);
        await Task.CompletedTask;
    }

   
}