using System;
using Common;
using Common.Dto;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi
{
    public class CopyOps
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CopyOps(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Copy()
        {
            var girl = new Girl
            {
                Id = "id",
                Name = "name",
                Age = 18,
            };
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(BeanUtils.Copy<Girl>(girl)));
        }

        [Fact]
        public void NewValue()
        {
            var defaultValue = new PocoDefaultValue { };
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(defaultValue));
        }

        [Fact]
        public void RefInnerChanged()
        {
            var demo = new PocoDefaultValue();
            Modify(demo);
            _testOutputHelper.WriteLine(demo.ToString());
        }

        private void Modify(PocoDefaultValue demo)
        {
            demo = new PocoDefaultValue()
            {
                Test = "inner"
            };
        }

        public class PocoDefaultValue
        {
            public string Test { get; set; } = "test";
        }
    }
}