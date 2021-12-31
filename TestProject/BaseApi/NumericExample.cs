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
        }
    }
}