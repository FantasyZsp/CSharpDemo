using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.DeSerializer
{
    public class TestJson
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TestJson(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test()
        {
            var jObject = new JObject();
            jObject["Test"] = "test";
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            JsonConvert.DefaultSettings = () => setting;

            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(jObject, setting));
        }


        [Fact]
        public void TestJsonNull()
        {
            Assert.Throws<ArgumentNullException>(() => { JsonConvert.DeserializeObject(null); });
        }
    }
}