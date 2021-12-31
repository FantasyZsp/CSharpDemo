using System;
using Snowflake.Core;
using Xunit;
using Xunit.Abstractions;
using IdWorker = SqlSugar.DistributedSystem.Snowflake.IdWorker;

namespace TestProject.Snowflake
{
    public class SnowflakeTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public SnowflakeTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test_Id()
        {
            var currentTimeMillis = TimeExtensions.CurrentTimeMillis();
            _testOutputHelper.WriteLine(currentTimeMillis.ToString());
            _testOutputHelper.WriteLine((1288834974657L << 22).ToString());
            _testOutputHelper.WriteLine((1L << 22).ToString());
            _testOutputHelper.WriteLine((currentTimeMillis - 1288834974657L << 22).ToString());

            var idWorker = new IdWorker(1, 1);
            var nextId = idWorker.NextId();
            _testOutputHelper.WriteLine(nextId.ToString());
            nextId = idWorker.NextId();
            _testOutputHelper.WriteLine(nextId.ToString());
            nextId = idWorker.NextId();
            _testOutputHelper.WriteLine(nextId.ToString());
            nextId = idWorker.NextId();
            _testOutputHelper.WriteLine(nextId.ToString());
            nextId = idWorker.NextId();
            _testOutputHelper.WriteLine(nextId.ToString());
            nextId = idWorker.NextId();
            _testOutputHelper.WriteLine(nextId.ToString());

            var id = 1476390201176756224;
            _testOutputHelper.WriteLine((nextId - id).ToString());
            
        }
    }
}