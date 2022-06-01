using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class SortTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SortTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test_Sort()
    {
        var info = new Info(true, DateTime.Now);
        var info2 = new Info(false, DateTime.Now.AddHours(1));
        var info3 = new Info(false, DateTime.Now.AddHours(2));
        var info4 = new Info(true, DateTime.Now.AddHours(3));

        var spaceList = new List<Info> {info2, info, info3, info4};
        spaceList.Sort((left, right) =>
        {
            if (left.IsPrimary.GetValueOrDefault())
            {
                return -1;
            }

            if (right.IsPrimary.GetValueOrDefault())
            {
                return 1;
            }

            var priCompareTo = right.IsPrimary.GetValueOrDefault().CompareTo(left.IsPrimary.GetValueOrDefault());
            if (priCompareTo == 0)
            {
                var timeCompare = right.BindingTime.GetValueOrDefault().CompareTo(left.BindingTime.GetValueOrDefault());
                return timeCompare;
            }

            return priCompareTo;
        });
        _testOutputHelper.WriteLine($"{JsonConvert.SerializeObject(spaceList)}");
        Task.Delay(100);
    }

    [Fact]
    public void Test_GroupAndFetchMax()
    {
        var info = new Info(true, DateTime.Now);
        var info2 = new Info(false, DateTime.Now.AddHours(1));
        var info3 = new Info(false, DateTime.Now.AddHours(2));
        var info4 = new Info(true, DateTime.Now.AddHours(3));

        var spaceList = new List<Info> {info2, info, info3, info4};
        var maxList
            = spaceList.GroupBy(item => item.IsPrimary)
                .Select(e => e.OrderByDescending(item => item.BindingTime).First()).OrderByDescending(item => item.BindingTime).ToList();
        _testOutputHelper.WriteLine($"{JsonConvert.SerializeObject(maxList)}");
        Task.Delay(100);
    }
}

public class Info
{
    public bool? IsPrimary { get; }
    public DateTime? BindingTime { get; }

    public Info(bool? isPrimary, DateTime? bindingTime)
    {
        IsPrimary = isPrimary;
        BindingTime = bindingTime;
    }
}