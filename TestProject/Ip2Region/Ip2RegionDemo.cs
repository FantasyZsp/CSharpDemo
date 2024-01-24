using System;
using System.IO;
using IP2Region.Net.Abstractions;
using IP2Region.Net.XDB;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Ip2Region;

public class Ip2RegionDemo
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Ip2RegionDemo(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test()
    {
        // 定义相对路径到 Data 目录下的 Ip2Region.cs 文件
        var filePath = Path.Combine("Ip2Region/Data/ip2region.xdb");

        // 检查文件是否存在
        if (File.Exists(filePath))
        {
            // hits
            _testOutputHelper.WriteLine("File found " + filePath);
            ISearcher searcher = new Searcher(CachePolicy.File, filePath);
            var search = searcher.Search("123.149.3.177");
            _testOutputHelper.WriteLine(search);
        }
        else
        {
            _testOutputHelper.WriteLine("File not found: " + filePath);
        }
    }
}