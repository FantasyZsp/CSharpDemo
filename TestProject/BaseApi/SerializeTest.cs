using Newtonsoft.Json;
using TestProject.FreeSql;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi
{
    public class SerializeTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public SerializeTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public static T DeepCopy<T>(T obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        [Fact]
        public void Test()
        {
            var customer = new Customer
            {
                Id = null,
                Name = "222-33",
                Email = "testEmail4",
                CardId = 1004,
                Mark = "mark4",
                Config = "config4"
            };
            var deepCopy = DeepCopy(customer);
            _testOutputHelper.WriteLine((deepCopy == customer).ToString());
        }
    }
}