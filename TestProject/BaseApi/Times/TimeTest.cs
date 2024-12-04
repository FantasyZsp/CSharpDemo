using System;
using System.Globalization;
using DotNetCommon.Extensions;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi.Times;

public class TimeTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TimeTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test()
    {
        var utcNow = DateTimeOffset.UtcNow.ToString();
        var now = DateTimeOffset.Now;
        _testOutputHelper.WriteLine(utcNow);
        _testOutputHelper.WriteLine(now.ToString());

        var foo = new TimeAndOffset
        {
            Time = now,
            Offset = TimeSpan.FromHours(7)
        };

        _testOutputHelper.WriteLine(JsonConvert.SerializeObject(foo));

        _testOutputHelper.WriteLine(DateTimeOffset.Parse("2024-05-29 14:29:31 +08:00").ToString());
        _testOutputHelper.WriteLine(DateTimeOffset.Parse("2024-05-29 14:29:31 +02:00").ToString());

        var timeOffset = new DateTimeOffset(DateTime.Parse("2024-05-29"), TimeSpan.Zero);
        _testOutputHelper.WriteLine(timeOffset.ToString());

        DateTimeOffset offset = DateTime.Now;
        DateTimeOffset utcOffset = DateTime.UtcNow;
        _testOutputHelper.WriteLine(offset.ToString());
        _testOutputHelper.WriteLine(utcOffset.ToString());

        DateTime dateTimeParse = DateTime.Parse("2024-05-29");
        DateTimeOffset offsetParse = dateTimeParse;
        _testOutputHelper.WriteLine(dateTimeParse.ToString());
        _testOutputHelper.WriteLine(offsetParse.ToString());
        var convertTimeToUtc = TimeZoneInfo.ConvertTimeToUtc(dateTimeParse);
        _testOutputHelper.WriteLine(convertTimeToUtc.ToString());

        var str = "{\"time\":\"2024/5/29 15:21:35 +08:00\",\"offset\":\"7\"}";
        var timeAndOffset = JsonConvert.DeserializeObject<TimeAndOffset>(str);
        var timeSpan = timeAndOffset.Offset!.Value!;
        _testOutputHelper.WriteLine(timeSpan.ToString());
        _testOutputHelper.WriteLine(timeAndOffset.ToString());

        _testOutputHelper.WriteLine(offset.UtcDateTime.ToString());
        _testOutputHelper.WriteLine(offset.LocalDateTime.ToString());

        ;
    }

    [Fact]
    public void TestOffset()
    {
        var dateTime = DateTime.Parse("2024-05-19");
        var dateTimeKind = dateTime.Kind;
        var dateKind = DateTime.Now.Date.Kind;

        var time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        var timeKind = time.Kind;

        _testOutputHelper.WriteLine(dateTimeKind.ToString());
        _testOutputHelper.WriteLine(dateKind.ToString());
        _testOutputHelper.WriteLine(timeKind.ToString());
        var dateTimeOffset = new DateTimeOffset(dateTime, TimeSpan.FromHours(2));
        _testOutputHelper.WriteLine(dateTimeOffset.ToString());
    }

    [Fact]
    public void TestOffset2()
    {
        var dateTime = DateTime.Parse("2024-05-19");
        Assert.Equal(DateTimeKind.Unspecified, dateTime.Kind);

        // local
        var dateTimeOffset = new DateTimeOffset(dateTime, TimeSpan.FromMinutes(480));
        Assert.Equal(DateTimeKind.Local, dateTimeOffset.LocalDateTime.Kind);
        _testOutputHelper.WriteLine(dateTimeOffset.LocalDateTime.ToString());
        _testOutputHelper.WriteLine(dateTimeOffset.LocalDateTime.ToJson());
        _testOutputHelper.WriteLine(dateTimeOffset.LocalDateTime.ToUniversalTime().ToJson());
        Assert.Equal(DateTimeKind.Unspecified, dateTimeOffset.DateTime.Kind);
        _testOutputHelper.WriteLine(dateTimeOffset.DateTime.ToString());
        _testOutputHelper.WriteLine(dateTimeOffset.DateTime.ToJson());
        _testOutputHelper.WriteLine(dateTimeOffset.DateTime.ToUniversalTime().ToJson());

        Assert.Equal(DateTimeKind.Utc, dateTimeOffset.UtcDateTime.Kind);
        _testOutputHelper.WriteLine(dateTimeOffset.UtcDateTime.ToString());
        _testOutputHelper.WriteLine(dateTimeOffset.UtcDateTime.ToJson());
        _testOutputHelper.WriteLine(dateTimeOffset.UtcDateTime.ToUniversalTime().ToJson());

        var timeOffset = DateTimeOffset.Parse("2024-05-19");
        Assert.Equal(DateTimeKind.Local, timeOffset.LocalDateTime.Kind);
    }

    [Fact]
    public void TestOffset3()
    {
        var date = DateOnly.Parse("2024-05-31T00:00:00");
        _testOutputHelper.WriteLine(date.ToString());
        var dateTime = DateTime.Parse("2024-05-31T00:00:00+02:00");
        var dateTimeKind = dateTime.Kind;
        _testOutputHelper.WriteLine(dateTimeKind.ToString());
        var dateTimeOffset = new DateTimeOffset(dateTime, TimeSpan.FromHours(8)); // 这里必须和实际时区对应
        _testOutputHelper.WriteLine(dateTimeOffset.ToString());
    }

    [Fact]
    public void TestDateOnlyKind()
    {
        var date = DateOnly.Parse("2024-05-31T00:00:00");
        _testOutputHelper.WriteLine(date.ToString());

        var time = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Unspecified);
        _testOutputHelper.WriteLine(time.Kind.ToString());


        var dateTime = DateTime.Parse("2024-05-31T00:00:00+02:00");
        var dateTimeKind = dateTime.Kind;
        _testOutputHelper.WriteLine(dateTimeKind.ToString());
        var dateTimeOffset = new DateTimeOffset(dateTime, TimeSpan.FromMinutes(480)); // 这里必须和实际时区对应
        _testOutputHelper.WriteLine(dateTimeOffset.ToString());
    }

    [Fact]
    public void TestDateOnlyParse()
    {
        var dateTime = DateTimeOffset.Parse("2024-05-31T16:00:00Z");
        _testOutputHelper.WriteLine(dateTime.DateTime.Kind.ToString());


        DateOnly.FromDateTime(DateTime.Parse("2024-05-31T01:00:00"));
        var date = DateOnly.FromDateTime(DateTime.Parse("2024-05-31"));

        var date2 = DateOnly.FromDateTime(DateTime.Parse("2024-05-31T01:00:00"));
        var date3 = DateOnly.FromDateTime(DateTime.Parse("2024-05-31T08:00:00"));

        var date4 = DateOnly.FromDateTime(DateTime.SpecifyKind(dateTime.Date, DateTimeKind.Unspecified));
        var date5 = DateOnly.FromDateTime(DateTime.Parse("2024-05-31T20:00:00"));
        _testOutputHelper.WriteLine(date.ToString());
        _testOutputHelper.WriteLine(date2.ToString());
        _testOutputHelper.WriteLine(date3.ToString());
        _testOutputHelper.WriteLine(date4.ToString());
        _testOutputHelper.WriteLine(date5.ToString());
    }

    [Fact]
    public void TestDateTimeOffsetParse()
    {
        var dateTimeOffset0 = DateTimeOffset.Parse("2024-05-31");
        var dateTimeOffset1 = DateTimeOffset.Parse("2024-05-31T16:00:00Z");
        var dateTimeOffset2 = DateTimeOffset.Parse("05/31/2024 23:00:00+2:00");
        var dateTimeOffset3 = DateTimeOffset.Parse("2024/05/31 16:00:00");
        _testOutputHelper.WriteLine(dateTimeOffset0.Offset.ToString());
        _testOutputHelper.WriteLine(dateTimeOffset1.Offset.ToString());
        _testOutputHelper.WriteLine(dateTimeOffset2.Offset.ToString());
        _testOutputHelper.WriteLine(dateTimeOffset3.Offset.ToString());
        _testOutputHelper.WriteLine(dateTimeOffset0.DateTime.ToString(CultureInfo.CurrentCulture));
        _testOutputHelper.WriteLine(dateTimeOffset1.DateTime.ToString(CultureInfo.CurrentCulture));
        _testOutputHelper.WriteLine(dateTimeOffset2.DateTime.ToString(CultureInfo.CurrentCulture));
        _testOutputHelper.WriteLine(dateTimeOffset3.DateTime.ToString(CultureInfo.CurrentCulture));

        Assert.Equal(DateOnly.FromDateTime(dateTimeOffset0.DateTime), DateOnly.FromDateTime(dateTimeOffset1.DateTime));
        Assert.Equal(DateOnly.FromDateTime(dateTimeOffset1.DateTime), DateOnly.FromDateTime(dateTimeOffset2.DateTime));
        Assert.Equal(DateOnly.FromDateTime(dateTimeOffset2.DateTime), DateOnly.FromDateTime(dateTimeOffset3.DateTime));

        _testOutputHelper.WriteLine(dateTimeOffset0.DateTime.Date.ToString(CultureInfo.CurrentCulture));
        _testOutputHelper.WriteLine(dateTimeOffset1.DateTime.Date.ToString(CultureInfo.CurrentCulture));
        _testOutputHelper.WriteLine(dateTimeOffset2.DateTime.Date.ToString(CultureInfo.CurrentCulture));
        _testOutputHelper.WriteLine(dateTimeOffset3.DateTime.Date.ToString(CultureInfo.CurrentCulture));
    }

    [Fact]
    public void GetMonthCrossedCount()
    {
        var dateTime = DateTime.Parse("2021-02-19");
        var dateTime2 = DateTime.Parse("2021-02-01");
        var dateTimeOffset = TimeAndOffset.GetMonthCrossedCount(dateTime, dateTime2);
        _testOutputHelper.WriteLine(dateTimeOffset.ToString());
    }

    [Fact]
    public void Convert()
    {
        DateTime dateTime = DateTime.Now;
        DateTime unspecifiedDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
        DateTimeOffset dateTimeOffset = new DateTimeOffset(unspecifiedDateTime, TimeSpan.FromHours(-5)); // 偏移量为 -5 小时
        _testOutputHelper.WriteLine(dateTimeOffset.ToString());
    }
}

public class TimeAndOffset
{
    public DateTimeOffset? Time { get; set; }
    public TimeSpan? Offset { get; set; }

    public DateTime? StartUtcTime => Time?.UtcDateTime;
    public DateTime? EndUtcTime => Time?.UtcDateTime;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static int GetMonthCrossedCount(DateTime date1, DateTime date2)
    {
        // Ensure date1 is the earlier date
        if (date1 > date2)
        {
            (date1, date2) = (date2, date1);
        }

        int yearDifference = date2.Year - date1.Year;
        int monthDifference = date2.Month - date1.Month;
        var difference = yearDifference * 12 + monthDifference + 1;
        return difference;
    }
}