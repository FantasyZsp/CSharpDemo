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
    }
}