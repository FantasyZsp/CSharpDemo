using System;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class StringTemplateExample
{
    private readonly ITestOutputHelper _testOutputHelper;

    public StringTemplateExample(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public void ReplaceTest()
    {
        const string template = "https://demo.com/index.html?noCache=true&hid={packageId}&domain=//{domain}/&bucket={bucket}&taskId={taskId}&datatype=floorplandata";

        const string packageId = "123";
        const string domain = "example.com";
        const string bucket = "my-bucket";
        const string taskId = "456";

        var url = template.Replace("{packageId}", packageId)
            .Replace("{domain}", domain)
            .Replace("{bucket}", bucket)
            .Replace("{taskId}", taskId);

        _testOutputHelper.WriteLine(url);
    }
}