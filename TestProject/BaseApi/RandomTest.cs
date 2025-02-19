using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class RandomTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public RandomTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void RandomTest2()
    {
        var uniqueNumbers = new HashSet<int>();
        var random = new Random();

        while (uniqueNumbers.Count < 600)
        {
            var randomNumber = random.Next(100000, 1000000);
            uniqueNumbers.Add(randomNumber);
        }

        const string filePath = "random_numbers.txt";
        var absolutePath = Path.GetFullPath(filePath);

        try
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (int num in uniqueNumbers)
                {
                    writer.WriteLine(num);
                }
            }

            _testOutputHelper.WriteLine($"数据已成功写入文件: {absolutePath}");
        }
        catch (Exception ex)
        {
            _testOutputHelper.WriteLine($"写入文件时出现错误: {ex.Message}");
        }
    }
}