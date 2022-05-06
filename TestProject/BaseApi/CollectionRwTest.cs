using System;
using System.Collections.Generic;
using System.Linq;
using Common.Dto;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using static System.String;

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

        [Fact]
        public void Test_Except()
        {
            var set = new HashSet<string>
            {
                "1", "2", "7"
            };

            var subSet = new HashSet<string>
            {
                "2", "7"
            };

            var list2 = new List<string>
            {
                "0", "1", "2", "4", "5"
            };

            var result = list2.Except(set);

            _testOutputHelper.WriteLine($"{set.Count} {list2.Count} {result.Count()}");
            _testOutputHelper.WriteLine($"{JsonConvert.SerializeObject(set)} {JsonConvert.SerializeObject(list2)} {JsonConvert.SerializeObject(result)}");
            _testOutputHelper.WriteLine($"{subSet.IsSubsetOf(set)}");
            _testOutputHelper.WriteLine($"{set.IsSupersetOf(subSet)}");
        }

        [Fact]
        public void Test_GroupBy()
        {
            var girls = new List<Girl>
            {
                new Girl("1", "name", 1),
                new Girl("2", "name", 1),
                new Girl("3", "name2", 1),
                new Girl("4", "name2", 1),
                new Girl("5", "name2", 1),
                new Girl("6", "name3", 1)
            };

            var dictionary = girls.GroupBy(girl => girl.Name).ToDictionary(group => group.Key, group => group.ToList());
            _testOutputHelper.WriteLine($"{dictionary.GetType().Name}");
            _testOutputHelper.WriteLine($"{JsonConvert.SerializeObject(dictionary)}");
            foreach (var grouping in dictionary)
            {
                _testOutputHelper.WriteLine($"{grouping.Key}");
                _testOutputHelper.WriteLine($"{JsonConvert.SerializeObject(grouping)}");
            }

            _testOutputHelper.WriteLine($"{JsonConvert.SerializeObject(dictionary["name"])}");
        }

        [Fact]
        public void Test_Sort()
        {
            var girls = new List<Girl>
            {
                new Girl("5", "name2", 5),
                new Girl("2", "name", 2),
                new Girl("4", "name2", 4),
                new Girl("3", "name2", 3),
                new Girl("1", "name", 1),
                new Girl("1", "name11", 2),
                new Girl("6", "name3", 6)
            };

            girls.Sort((g1, g2) => g2.Age.CompareTo(g1.Age));
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(girls));
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(girls[0]));
            girls.Sort((g1, g2) => Compare(g1.Id, g2.Id, StringComparison.Ordinal));
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(girls));
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(girls[0]));
        }

        [Fact]
        public void Test_DistinctBy()
        {
            var girls = new List<Girl>
            {
                new Girl("5", "name2", 5),
                new Girl("2", "name", 2),
                new Girl("4", "name2", 4),
                new Girl("3", "name2", 3),
                new Girl("1", "name", 1),
                new Girl("1", "name11", 2),
                new Girl("6", "name3", 6)
            };

            var list = girls.DistinctBy(gg => gg.Name).ToList();
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(list));

            var list2 = new List<Girl>().DistinctBy(gg => gg.Name).ToList();
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(list2));

        } 
        
        [Fact]
        public void Test_DeserializeObject()
        {
            var girls = new List<Girl>
            {
                new Girl("5", "name2", 5),
                new Girl("2", "name", 2),
                new Girl("4", "name2", 4),
                new Girl("3", "name2", 3),
                new Girl("1", "name", 1),
                new Girl("1", "name11", 2),
                new Girl("6", "name3", 6)
            };

            var copiedList = JsonConvert.DeserializeObject<List<Girl>>(JsonConvert.SerializeObject(girls));

            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(copiedList));

        }
    }
}