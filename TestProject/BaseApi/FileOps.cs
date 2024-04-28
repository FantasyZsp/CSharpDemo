using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class FileOps
{
    private readonly ITestOutputHelper _testOutputHelper;

    public FileOps(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Test()
    {
        var readAllTextAsync = await File.ReadAllTextAsync("/Users/zhaosp/tt.txt");
        _testOutputHelper.WriteLine(readAllTextAsync);
    }
}