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

    [Fact]
    public void Test_SortByAssignedOrder()
    {
        var dic = new List<string>
        {
            "客厅", "客餐厅", "餐厅", "主卧", "次卧"
        };
        var info = new Info("1", "次卧");
        var info2 = new Info("2", "主卧");
        var info3 = new Info("3", "餐厅");
        var info4 = new Info("4", "客厅");
        var infoxx = new Info("4", "xxx");

        var spaceList = new List<Info> {info2, info, info3, info4, infoxx};
        var infos = spaceList.OrderBy((left) => dic.IndexOf(left.Text)).ToList();

        _testOutputHelper.WriteLine($"{JsonConvert.SerializeObject(infos)}");

        var text = "xchzww2_24cc301efc3d4e2897d817210a35faba_deco_10990/ThumbnailImages/厨房_WlTSvsqK_0111.jpg";

        var jpgNode = text.Substring(text.LastIndexOf("/", StringComparison.Ordinal) + 1);
        var roomName = jpgNode.Substring(0, jpgNode.IndexOf("_", StringComparison.Ordinal));
        var start = jpgNode.LastIndexOf("_", StringComparison.Ordinal) + 1;
        var end = jpgNode.LastIndexOf(".", StringComparison.Ordinal);

        var number = jpgNode.Substring(start, end - start);
        _testOutputHelper.WriteLine(number);


        Task.Delay(100);
    }
}

public class Info
{
    public bool? IsPrimary { get; }
    public DateTime? BindingTime { get; }
    public string Text { get; }
    public string Id { get; }

    public Info(bool? isPrimary, DateTime? bindingTime)
    {
        IsPrimary = isPrimary;
        BindingTime = bindingTime;
    }

    public Info(string id, string text)
    {
        Id = id;
        Text = text;
    }
}