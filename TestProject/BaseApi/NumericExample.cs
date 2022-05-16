using System;
using DotNetCommon.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi
{
    public class NumericExample
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public NumericExample(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestNull()
        {
            uint? a = 100;
            uint? b = null;
            _testOutputHelper.WriteLine(a.GetValueOrDefault().ToString());
            _testOutputHelper.WriteLine(b.GetValueOrDefault().ToString());
            _testOutputHelper.WriteLine((b == 0).ToString()); // false
            _testOutputHelper.WriteLine((b != 0).ToString());
            _testOutputHelper.WriteLine((b.GetValueOrDefault() != 0).ToString());
        }

        [Fact]
        public void TestNullEquals()
        {
            uint? a = 100;
            uint b = 1;
            uint? c = null;
            uint? d = 0;
            _testOutputHelper.WriteLine(c.HasValue.ToString());
            _testOutputHelper.WriteLine(d.HasValue.ToString());
            _testOutputHelper.WriteLine(d.GetType().BaseType.Name);


            _testOutputHelper.WriteLine(a.GetValueOrDefault().ToString());
            _testOutputHelper.WriteLine(b.ToString());

            _testOutputHelper.WriteLine(c.ToString());
            _testOutputHelper.WriteLine(d.ToString());
        }

        [Fact]
        public void Test_Float2Double()
        {
            float area = 162.85f;
            double dd = area;
            var rightOne = double.Parse(area.ToString());

            _testOutputHelper.WriteLine(area.ToString());
            _testOutputHelper.WriteLine(dd.ToString());
            _testOutputHelper.WriteLine(rightOne.ToString());
        }
    }
}