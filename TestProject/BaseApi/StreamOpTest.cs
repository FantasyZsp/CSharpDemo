using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi
{
    public class StreamOpTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public StreamOpTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test_ToDic()
        {
            var list = new List<string>
            {
                "test",
                "test"
            };
            var dictionary = list.Distinct().ToDictionary(str => str);
            _testOutputHelper.WriteLine(dictionary.ToString());


            var firstOrDefault = list.FirstOrDefault(ss => ss == "xx");

            _testOutputHelper.WriteLine("xxxxx");
            _testOutputHelper.WriteLine(firstOrDefault ??= "xx");
            _testOutputHelper.WriteLine(firstOrDefault);
        }

        [Fact]
        public void Test_StreamMinMax()
        {
            DateTime? time = null;
            _testOutputHelper.WriteLine(((DateTime?) null).GetValueOrDefault().ToString(CultureInfo.InvariantCulture));


            var list = new List<DateTime>
            {
                // new(1970, 1, 1),
                // new(1980, 2, 2),
                // new(2020, 3, 3),
                // new(2021, 8, 8)
            };
            // var min = list.Where(dt => dt != new DateTime(1970, 1, 1)).Select(dt => dt).Min(); // 当没有元素时，min将会抛错
            var dateTimes = list
                .Where(dt => dt != new DateTime(1970, 1, 1))
                .Select(dt => dt)
                .ToList();
            Assert.Throws<InvalidOperationException>(() => dateTimes.Min());
            dateTimes.Sort();
            DateTime? min = dateTimes.Any() ? dateTimes.Min() : null;
            DateTime? max = dateTimes.Any() ? dateTimes.Max() : null;
            // var max = list.Where(dt => dt != new DateTime(2021, 8, 8)).Select(dt => dt).Max();
            _testOutputHelper.WriteLine(min?.ToString(CultureInfo.InvariantCulture) ?? "null");
            _testOutputHelper.WriteLine(max?.ToString(CultureInfo.InvariantCulture) ?? "null");
            // _testOutputHelper.WriteLine(max.ToString(CultureInfo.InvariantCulture));
        }
    }
}