﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi
{
    public class StringExample
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public StringExample(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestStringMatch()
        {
            var path = "abc";
            var text = "abcd";
            var textWithUnderLine = "abc_d";
            var longText = "abcdd_";
            _testOutputHelper.WriteLine(text.StartsWith(path).ToString());
            _testOutputHelper.WriteLine(textWithUnderLine.StartsWith(text).ToString());
            _testOutputHelper.WriteLine(longText.StartsWith(textWithUnderLine).ToString());
        }

        [Fact]
        public void Test_RevertStringWithSplit()
        {
            const string path = "/CQAAAA/FgAAAA/";
            const string text = "/CAAAAA/KQAAAA/TgAAAA/gAAAAA/";
            const string textWithUnderLine = "/CQAAAA/";
            const string blank = " ";

            // ReverseStringWithSplit(path);
            // ReverseStringWithSplit(text);
            // ReverseStringWithSplit(textWithUnderLine);
            ReverseStringWithSplit(blank);
        }

        [Fact]
        public void Test_IdList()
        {
            var from = 1476475001044602885;
            var to = 1476390201176756224;
            if (from > to)
            {
                from = to;
                to = 1476475001044602885;
            }

            var diff = (to - from);
            _testOutputHelper.WriteLine(diff.ToString());
            var interval = diff / 14;

            var list = new List<long>();
            for (var i = 0; i < 14; i++)
            {
                list.Add(from + i * interval);
            }

            list.Reverse();
            foreach (var l in list)
            {
                _testOutputHelper.WriteLine(l.ToString());
            }
        }

        [Fact]
        public void Test_StringList()
        {
            var test = "testxxx/test.zip";
            var test2 = "test.zip";

            test = test[..test.LastIndexOf('/')];
            test2 = test2[..test2.LastIndexOf('/')];
            // var message2 = test.Substring(0, test.LastIndexOf('/'));


            _testOutputHelper.WriteLine(test);
            _testOutputHelper.WriteLine(test2);
        }

        private void ReverseStringWithSplit(string path)
        {
            // _testOutputHelper.WriteLine("===");
            // _testOutputHelper.WriteLine(path);

            // _testOutputHelper.WriteLine(JsonConvert.SerializeObject(path.Split("/").Where(str => str != "").ToArray()));
            // var reversedPathArray = path.Split("/").Where(str => str != "").ToArray().Reverse();
            var reversedPathArray = path.Split("/").Reverse();
            var reversedPath = JsonConvert.SerializeObject(reversedPathArray);
            // _testOutputHelper.WriteLine(reversedPath);
            var result = "";
            foreach (var str in reversedPathArray)
            {
                result += str;
                result += "/";
            }

            result = result[..^1];
            _testOutputHelper.WriteLine(result);
            _testOutputHelper.WriteLine(result.Length.ToString());
        }
    }
}