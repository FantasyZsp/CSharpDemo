using System.Threading.Tasks;
using AlibabaCloud.OpenApiClient.Models;
using AlibabaCloud.OpenApiUtil;
using AlibabaCloud.SDK.Oss20190517;
using AlibabaCloud.SDK.Oss20190517.Models;
using Xunit;
using Client = AlibabaCloud.SDK.Oss20190517.Client;

namespace TestProject.Oss;

public class OssTest
{
    [Fact]
    public async Task TestAsync()
    {
        var client = new Client(new Config());
        // var listObjectsAsync = await client.ListObjectsV2Async("vrhouse", new ListObjectsRequest());
        // client.GetObjectAsync()
        return;
    }
}