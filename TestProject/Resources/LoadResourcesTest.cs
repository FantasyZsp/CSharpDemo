using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Resources;

// extern alias Common2;

public class LoadResourcesTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public LoadResourcesTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestLoad()
    {
        var resourceStream = typeof(LoadResourcesTest).Assembly.GetManifestResourceStream("TestProject.Resources.test.txt");

        if (resourceStream == null)
        {
            _testOutputHelper.WriteLine("no contents");
            return;
        }

        var reader = new StreamReader(resourceStream);
        var contents = reader.ReadToEnd();
        _testOutputHelper.WriteLine(contents);
    }

    [Fact]
    public void TestLoadWhichFile()
    {
        var contents = Common.Resources.ResourcesLoader.Load();
        _testOutputHelper.WriteLine(contents);
    }
}