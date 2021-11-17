using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi
{
    public class CollectionRwTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CollectionRwTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test_Any()
        {
            List<string> list = null;
            Assert.Throws<ArgumentNullException>(() => _testOutputHelper.WriteLine(list.Any().ToString()));
        }
    }
}