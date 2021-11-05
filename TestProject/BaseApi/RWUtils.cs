using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TestProject.BaseApi
{
    public class RwUtils
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public RwUtils(ITestOutputHelper testOutputHelper)
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

        [Fact]
        public void DateTime_DeSerialize_Test()
        {
            int? page = 10;
            page ??= 0;
            page += 1;
            _testOutputHelper.WriteLine(page.ToString());
        }

        [Fact]
        public void ConcurrentDic_RW_Test()
        {
            var concurrentDictionary = new ConcurrentDictionary<string, string>();
            var concurrentQueue = new ConcurrentQueue<string>();

            Parallel.Invoke(() =>
            {
                Thread.Sleep(1000);
                concurrentQueue.Enqueue("111");
            });
            Parallel.Invoke(() =>
            {
                Thread.Sleep(1000);
                concurrentQueue.Enqueue("222");
            });

            Parallel.Invoke(() =>
            {
                Thread.Sleep(1000);
                concurrentQueue.Enqueue("333");
            });

            Thread.Sleep(2000);

            foreach (var s in concurrentQueue)
            {
                _testOutputHelper.WriteLine(s);
            }
        }

        [Fact]
        public void StringReplace_Test()
        {
            const string src = "aaa";
            var replace = src.Replace("a", "b");
            _testOutputHelper.WriteLine(replace);
            
            
        }
    }
}