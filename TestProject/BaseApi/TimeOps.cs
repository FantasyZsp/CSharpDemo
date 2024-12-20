﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi
{
    public class TimeOps
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TimeOps(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test_DateTime_Equals()
        {
            var dateTime = new DateTime(1970, 1, 1);
            var dateTime2 = new DateTime(1970, 1, 1);
            _testOutputHelper.WriteLine((dateTime == dateTime2).ToString());
        }
        [Fact]
        public void Test_DateTime_Parse()
        {
            DateTime dateTime = DateTime.ParseExact("20240710120407", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            // 将 DateTime 对象转换为 Unix 时间戳
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
            long unixTimestamp = dateTimeOffset.ToUnixTimeMilliseconds();
            _testOutputHelper.WriteLine(unixTimestamp.ToString());
        }

        [Fact]
        public void Test_DateTime_AddHours()
        {
            var value = Math.Pow(3, 17);
            var addHours = DateTime.UtcNow.AddHours(value);
            _testOutputHelper.WriteLine(addHours.ToString(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Test_DateTime_Date()
        {
            _testOutputHelper.WriteLine(DateTime.Now.Date.ToString("yyyyMMdd"));
        }

        [Fact]
        public void Test_DateTime_Seconds()
        {
            var dateTime = DateTime.UtcNow;
            var dateTime2 = DateTime.UtcNow;
            _testOutputHelper.WriteLine((dateTime == dateTime2).ToString());
            _testOutputHelper.WriteLine(dateTime.ToString(CultureInfo.InvariantCulture));
            _testOutputHelper.WriteLine(dateTime.Hour.ToString());
            _testOutputHelper.WriteLine(dateTime.Minute.ToString());
            _testOutputHelper.WriteLine(dateTime.Second.ToString());

            _testOutputHelper.WriteLine("当天");
            var totalSecondsToday = dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second;
            _testOutputHelper.WriteLine(totalSecondsToday.ToString());
            _testOutputHelper.WriteLine((totalSecondsToday % 330).ToString());
            _testOutputHelper.WriteLine((totalSecondsToday - totalSecondsToday % 330).ToString());
            _testOutputHelper.WriteLine((totalSecondsToday / 330 * 330).ToString());


            _testOutputHelper.WriteLine("对比");
            var totalSecondsTodayMod = dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second + 24 * 60 * 60;
            _testOutputHelper.WriteLine(totalSecondsTodayMod.ToString());
            _testOutputHelper.WriteLine((totalSecondsTodayMod % 330).ToString());
            _testOutputHelper.WriteLine(((totalSecondsTodayMod - totalSecondsTodayMod % 330) % (24 * 60 * 60)).ToString());
            _testOutputHelper.WriteLine(((totalSecondsTodayMod / 330 * 330) % (24 * 60 * 60)).ToString());

            _testOutputHelper.WriteLine("能整除60的秒调度");
        }

        [Fact]
        public void Test_DateTime_FormatMinutes()
        {
            var dateTime = DateTime.UtcNow;
            _testOutputHelper.WriteLine(dateTime.ToString(CultureInfo.InvariantCulture));

            const int intervalMinute = 3;

            _testOutputHelper.WriteLine("format");
            var dateTimeMinuteFormatted = dateTime.Minute / intervalMinute * intervalMinute;
            _testOutputHelper.WriteLine("minutes at " + dateTimeMinuteFormatted);

            var time = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTimeMinuteFormatted, 0);
            _testOutputHelper.WriteLine(time.ToString(CultureInfo.InvariantCulture));
        }


        [Fact]
        public void Test_DateTime_TotalSeconds()
        {
            var dateTime = DateTime.Now;
            var utcNow = DateTime.UtcNow;

            _testOutputHelper.WriteLine(GetTimeStamp(dateTime).ToString());
            _testOutputHelper.WriteLine(GetTimeStamp(utcNow).ToString());
            _testOutputHelper.WriteLine((GetTimeStamp(dateTime) - GetTimeStamp(utcNow)).ToString());
            _testOutputHelper.WriteLine(DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
            _testOutputHelper.WriteLine(DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
        }

        public long GetTimeStamp(DateTime dateTime)
        {
            TimeSpan ts = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
}