using System;
using System.Globalization;
using System.Text;
using DotNetCommon.Extensions;
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
            _testOutputHelper.WriteLine((b == 0).ToString()); // false
            _testOutputHelper.WriteLine((b != 0).ToString());
            _testOutputHelper.WriteLine((b.GetValueOrDefault() != 0).ToString());
        }

        [Fact]
        public void TestDecimal()
        {
            float? a = null;
            var convert = ToDecimal(a, 2);
            _testOutputHelper.WriteLine(convert.ToString(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Truncate()
        {
            var d = 1231111112002 / 1024.0 / 1024 * 100 / 100;
            _testOutputHelper.WriteLine(Math.Round(d, 2).ToString());
        }

        [Fact]
        public void DivideZero()
        {
            var number = 11.2;
            var d = (int) number % 10;
            _testOutputHelper.WriteLine(d.ToString(CultureInfo.InvariantCulture));
        }

        public static decimal ToDecimal(float? fl, int decimals = 2)
        {
            return decimal.Round(Convert.ToDecimal(fl), decimals);
        }

        [Fact]
        public void TestNullEquals()
        {
            uint? a = 100;
            uint b = 1;
            uint? c = null;
            uint? d = 0;
            _testOutputHelper.WriteLine(c.HasValue.ToString());
            _testOutputHelper.WriteLine(d.HasValue.ToString());
            _testOutputHelper.WriteLine(d.GetType().BaseType.Name);


            _testOutputHelper.WriteLine(a.GetValueOrDefault().ToString());
            _testOutputHelper.WriteLine(b.ToString());

            _testOutputHelper.WriteLine(c.ToString());
            _testOutputHelper.WriteLine(d.ToString());
        }

        [Fact]
        public void Test_Float2Double()
        {
            float area = 162.85f;
            double dd = area;
            var rightOne = double.Parse(area.ToString());

            var result = area * 1.2f;

            _testOutputHelper.WriteLine(area.ToString());
            _testOutputHelper.WriteLine(dd.ToString());
            _testOutputHelper.WriteLine(rightOne.ToString());
            _testOutputHelper.WriteLine(result.ToString());
        }

        [Fact]
        public void Test_FloatCalculate()
        {
            float num1 = 0.1f;
            float num2 = 0.2f;
            decimal numfrom1 = (decimal) num1;
            decimal numfrom2 = (decimal) num2;
            var numfrom3 = numfrom1 + numfrom2;
            _testOutputHelper.WriteLine(numfrom3.ToString());
            _testOutputHelper.WriteLine(((float) numfrom3).ToString());
        }

        [Fact]
        public void Test_FloatCalculate2()
        {
            float? num1 = null;
            float num2 = 0.2f;
            decimal? numfrom1 = (decimal?) num1;
            decimal numfrom2 = (decimal) num2;
            var numfrom3 = numfrom1 + numfrom2; // numfrom3 is null 
            // _testOutputHelper.WriteLine(numfrom3.ToString());
            // _testOutputHelper.WriteLine(((float) numfrom3).ToString());
        }

        [Fact]
        public void Test_UIntMax()
        {
            _testOutputHelper.WriteLine(UInt32.MaxValue.ToString());
            _testOutputHelper.WriteLine(ConvertUIntToBase64(UInt32.MaxValue - 1));
        }

        public static string ConvertUIntToBase64(uint intNum)
        {
            return Convert.ToBase64String(BitConverter.GetBytes(intNum)).Base64UrlEncode();
        }

        [Fact]
        public void Test_compareDoubleAndInt()
        {
            double xx = 3.0;
            int yy = 3;
            _testOutputHelper.WriteLine((xx <= yy).ToString());
        }
    }

    public static class Base64
    {
        public static string ToBase64(this string raw) => Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));

        public static string FromBase64(this string base64) => Encoding.UTF8.GetString(Convert.FromBase64String(base64));

        public static string Base64UrlDecode(this string urlEncodedEncodedBase64)
        {
            urlEncodedEncodedBase64 = urlEncodedEncodedBase64.Replace('-', '+').Replace('_', '/');
            switch (urlEncodedEncodedBase64.Length % 4)
            {
                case 0:
                    return urlEncodedEncodedBase64;
                case 2:
                    urlEncodedEncodedBase64 += "==";
                    goto case 0;
                case 3:
                    urlEncodedEncodedBase64 += "=";
                    goto case 0;
                default:
                    throw new Exception("Illegal base64url string!");
            }
        }

        public static string Base64UrlEncode(this string plainBase64) => plainBase64.Split('=')[0].Replace('+', '-').Replace('/', '_');
    }
}