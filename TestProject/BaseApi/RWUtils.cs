using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TestProject.BaseApi
{
    public class RWUtils
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public RWUtils(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ReadOnlyDic_Test()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("x", "x");
            dic.Add("1", "1");
            Assert.Throws<ArgumentException>(() => dic.Add("1", "1"));
            dic.TryAdd("1", "1");
            _testOutputHelper.WriteLine(dic.ToString());
        }

        [Fact]
        public void DateTime_Test()
        {
            var dateTime = DateTime.Now;
            var addMinutes = dateTime.AddMinutes(3);

            _testOutputHelper.WriteLine(((int) addMinutes.Subtract(DateTime.Now).TotalSeconds).ToString());
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(dateTime));
            _testOutputHelper.WriteLine(JsonSerializer.Serialize(dateTime));
        }

        [Fact]
        public void DateTime_Serialize_Test()
        {
            var dateTime = DateTime.UtcNow;
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(dateTime));
            _testOutputHelper.WriteLine(JsonSerializer.Serialize(dateTime));
        }
    }
}