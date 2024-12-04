using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DotNetCommon.Extensions;
using Newtonsoft.Json;
using SqlSugar.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class StringExample
{
    private readonly ITestOutputHelper _testOutputHelper;

    public StringExample(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestStringDefault()
    {
        string test = default;
        string? nullableStr = default;
        string? nullableStr2 = default(string?);
        _testOutputHelper.WriteLine(test ?? "1111");
        _testOutputHelper.WriteLine(nullableStr ?? "2222");
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
    public void TestStringMatch2()
    {
        var path = "abc";
        if (path is not ("abc" or "ccc" or "eee"))
        {
            _testOutputHelper.WriteLine("hits");
        }
        else
        {
            _testOutputHelper.WriteLine("miss");
        }
    }

    [Fact]
    public void TestStringContains()
    {
        const string path = "abc";
        Assert.Throws<ArgumentNullException>(() => path.Contains(null).ToString());
    }

    [Fact]
    public void Test_Equals()
    {
        string a = null;
        string b = null;
        _testOutputHelper.WriteLine((a == b).ToString());
        _testOutputHelper.WriteLine(((a ?? "test") == (b ?? "test")).ToString());
    }


    [Fact]
    public void TestStringSub()
    {
        var layout = "1室1厅11厨";
        var currentLayout = layout;

        var indexOf = layout.IndexOf("厅");
        if (indexOf != -1)
        {
            _testOutputHelper.WriteLine(111.ToString());
            _testOutputHelper.WriteLine(layout.Substring(0, indexOf + 1));
        }


        _testOutputHelper.WriteLine(indexOf.ToString());
        _testOutputHelper.WriteLine(layout);
        _testOutputHelper.WriteLine(currentLayout);
    }

    [Fact]
    public void TestStringSubId()
    {
        var layout = "1室1厅11厨";
        var formattedLayout = Format(layout);
        formattedLayout = Format(layout);
        _testOutputHelper.WriteLine(formattedLayout);
    }

    [Fact]
    public void TestStringSubIdSub0()
    {
        var layout = "2室1厅1卫1厨 ";
        var formattedLayout = Format(layout);
        formattedLayout = FormatWithoutZeroTing(layout);
        _testOutputHelper.WriteLine(formattedLayout);
    }

    public static string Format(string layout)
    {
        if (!string.IsNullOrEmpty(layout))
        {
            var indexOf = layout.IndexOf("厅", StringComparison.Ordinal);
            if (indexOf != -1)
            {
                layout = layout.Substring(0, indexOf + 1);
            }
        }

        return layout;
    }

    public static string FormatWithoutZeroTing(string layout)
    {
        if (!string.IsNullOrEmpty(layout))
        {
            var indexOf = layout.IndexOf("厅", StringComparison.Ordinal);
            if (indexOf != -1) // 有"厅"截取到"厅"。如果layout不规范，这里的结果可能也不规范。
            {
                layout = layout.Substring(0, indexOf + 1);

                var indexOfShi = layout.IndexOf("室", StringComparison.Ordinal);
                if (indexOfShi != -1) // 如果结果是厅，有室有厅
                {
                    var numbersOfTing = layout.Substring(indexOfShi + 1, indexOf - indexOfShi - 1);
                    if (numbersOfTing == "0") // 0厅
                    {
                        layout = layout.Substring(0, indexOfShi + 1);
                    }
                }
            }
        }


        return layout;
    }

    [Fact]
    public void Test_StringStartWith()
    {
        var test = "abcd";
        var longOne = "abcde";
        var long2 = "abcd_f";

        _testOutputHelper.WriteLine(long2.StartsWith(longOne).ToString());
    }

    [Fact]
    public void Test_RevertStringWithSplit()
    {
        const string path = "/CQAAAA/FgAAAA/";
        const string text = "/CAAAAA/KQAAAA/TgAAAA/gAAAAA/";
        const string textWithUnderLine = "/CQAAAA/";
        const string blank = " ";

        ReverseStringWithSplit("xxx");
        ReverseStringWithSplit(path);
        ReverseStringWithSplit(text);
        ReverseStringWithSplit(textWithUnderLine);
        ReverseStringWithSplit(blank);
    }

    [Fact]
    public void Test_PrefixStringArrayWithSplit()
    {
        const string path = "/CQAAAA/FgAAAA/";
        const string text = "/CAAAAA/KQAAAA/TgAAAA/gAAAAA/";
        const string textWithUnderLine = "/CQAAAA/";
        const string blank = " ";

        PrefixStringArrayWithSplit("xxx");
        PrefixStringArrayWithSplit(path);
        PrefixStringArrayWithSplit(text);
        PrefixStringArrayWithSplit(textWithUnderLine);
        PrefixStringArrayWithSplit(blank);
    }

    [Fact]
    public void Test_PrefixStringArrayWithSplit2()
    {
        const string path = "/CQAAAA/FgAAAA/";
        const string text = "/CAAAAA/KQAAAA/TgAAAA/gAAAAA/";
        const string textWithUnderLine = "/CQAAAA/";
        const string blank = " ";

        PrefixStringArrayWithSplit2("xxx");
        PrefixStringArrayWithSplit2(path);
        PrefixStringArrayWithSplit2(text);
        PrefixStringArrayWithSplit2(textWithUnderLine);
        PrefixStringArrayWithSplit2(blank);
    }

    [Fact]
    public void Test_GetFirstSplitted()
    {
        const string path = "/CQAAAA/FgAAAA/";
        const string text = "/CAAAAA/KQAAAA/TgAAAA/gAAAAA/";
        const string textWithUnderLine = "/CQAAAA/";
        const string blank = " ";


        var strings = path.Split("/");
        var text2 = text.Split("/");
        var textWithUnderLine2 = textWithUnderLine.Split("/");
        _testOutputHelper.WriteLine(strings[1]);
        _testOutputHelper.WriteLine(text2[1]);
        _testOutputHelper.WriteLine(textWithUnderLine2[1]);
        var substring = textWithUnderLine.Substring(1);
        var s = substring.Substring(0, substring.IndexOf("/", StringComparison.Ordinal));
        _testOutputHelper.WriteLine(s);
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

    [Fact]
    public void Test_StringList2()
    {
        const string test =
            "托儿所,幼儿园,幼稚园,菜店,菜场,副食,百货,小学,中学,大学,学生,医院,诊所,银行,家具,水果,超市,公交,公司,饭店,市场,卖场,饮食,邮电,储蓄,居委会,电影院,文化馆,副食,书店,药房,饭店,派出所,图书馆,婚姻,宿舍,直营店,补习班,校区,办公室,网吧,五金,电器,商行,经营部,驾校,产业园,鲜果,油漆,加油站,购物,批发,工程,警察,建筑,蔬菜,肉,日用,职业,经理,研究生,消防,税务,党群,监测,充电站,林场,出租,教学,仪器,草坪,党校,剧院,创新,机场,火车,动车,酒楼,玩具,项目,高新技术,工业,天然气,专卖店,机动车,交易,农贸,生鲜,海鲜,禽,火锅,科技园";

        var strings = test.Split(',');
        _testOutputHelper.WriteLine(strings.ToJson());
        _testOutputHelper.WriteLine(strings.Length.ToString());
        _testOutputHelper.WriteLine(strings.Distinct().Count().ToString());
        var groupBy = strings.ToList().GroupBy(ss => ss).Where((ss) => ss.Count() > 1).Select((ss, count) => ss.Key).ToList();
        _testOutputHelper.WriteLine(groupBy.ToJson());
    }

    private void ReverseStringWithSplit(string path)
    {
        var reversedPathArray = path.Split("/").Reverse();
        var reversedPath = JsonConvert.SerializeObject(reversedPathArray);
        _testOutputHelper.WriteLine(reversedPath);
        var result = "";
        foreach (var str in reversedPathArray)
        {
            result += str;
            result += "/";
        }

        _testOutputHelper.WriteLine(result);
        result = result[..^1];
        _testOutputHelper.WriteLine(result);
        _testOutputHelper.WriteLine(result.Length.ToString());
    }

    private void PrefixStringArrayWithSplit(string path)
    {
        var prefixArray = path.Split("/");
        prefixArray = prefixArray.Where(ss => ss != "").ToArray();
        var prefixArrayStr = JsonConvert.SerializeObject(prefixArray);
        _testOutputHelper.WriteLine(prefixArrayStr);
        var result = new List<string>();
        var lastResult = "/";

        foreach (var prefixSplit in prefixArray)
        {
            lastResult = lastResult + prefixSplit + "/";

            result.Add(lastResult);
        }

        _testOutputHelper.WriteLine(JsonConvert.SerializeObject(result));
    }

    private void PrefixStringArrayWithSplit2(string path)
    {
        var lastIndexOf = path.LastIndexOf("/", StringComparison.Ordinal);
        _testOutputHelper.WriteLine(lastIndexOf.ToString());
    }


    [Fact]
    public void Test_NullStr()
    {
        var name = "name";

        _testOutputHelper.WriteLine($"name={name}");

        string name2 = null;

        _testOutputHelper.WriteLine($"name2={name2}");
    }


    // private const string Source = "BMP5V6TN1WCF42HU03DZ8KSQXYJR7E9GAL";
    private const string Source = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";


    public static string UIntToBase34(uint num) //num为需要转换的十进制数
    {
        var code = new char[6];
        var remainder = num;
        for (var i = 0; i < code.Length; i++)
        {
            var index = remainder / (uint) Math.Pow(34, code.Length - i - 1);
            remainder %= (uint) Math.Pow(34, code.Length - i - 1);
            code[i] = Source[(int) index];
        }

        return new string(code);
    }

    public static uint Base34ToInt(string code) //num为需要转换的十进制数
    {
        uint sum = 0;
        for (var i = 0; i < code.Length; i++)
        {
            var c = code[i]; // 字符
            var indexOf = Source.IndexOf(c); // index代表了位值，0就是数值0,1就是数值1，再乘以权重就是等值数额
            var pow = Math.Pow(34, code.Length - i - 1);
            sum += (uint) (indexOf * pow);
        }

        return sum;
    }

    [Fact]
    public void Test()
    {
        _testOutputHelper.WriteLine($"{Convert.ToString(16729765, 2)}");
        _testOutputHelper.WriteLine($"{Convert.ToInt32("111111110100011010100101", 2)}");
    }

    public static uint EncodeIdWith30Bits(uint id)
    {
        Assert.True(id <= 10_7374_1823, "too bigger, must little than 10_7374_1823");
        id ^= 16729765;
        // 11111111_0100011010100101,异或后，末位原来是0的会变成1，是1的变成0
        var sixLowest = (byte) (id & 0x3F); // 存下低6位
        var newId = id >> 6; // 整体右移6位
        newId |= (uint) (sixLowest << 24); // 源低六位补齐到高8位
        return newId;
    }

    public static uint DecodeIdWith30Bits(uint id)
    {
        var sixHigh = (id >> 24) & 0xFF; // 存下高8位(实际上高两位都是0)
        var newId = (id << 8) >> 2; // 整体左移6位(这里先移动8是为了把高8位里的低二位清零)
        newId |= sixHigh; // 低位补上6位
        return newId
               ^ 16729765
            ;
    }

    // [Fact]
    [Theory]
    [InlineData(1)]
    [InlineData(11)]
    [InlineData(111)]
    [InlineData(1111)]
    [InlineData(11111)]
    [InlineData(22222)]
    [InlineData(11417)]
    [InlineData(34)]
    public void Test_CreateNum(uint testNum)
    {
        var num = UIntToBase34(testNum);
        _testOutputHelper.WriteLine($"{testNum} => {num}");
        _testOutputHelper.WriteLine($"{num} => {Base34ToInt(num)}");
    }

    // [Fact]
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    // [InlineData(11)]
    // [InlineData(111)]
    // [InlineData(1111)]
    // [InlineData(11111)]
    // [InlineData(22222)]
    // [InlineData(11417)]
    // [InlineData(34)]
    public void Test_EncodeId(uint testNum)
    {
        // uint testNum = 16729765;
        // uint testNum = 16729766;
        _testOutputHelper.WriteLine($"{0x3F_FF_FF_FF}");

        // var num = EncodeId(testNum);
        // _testOutputHelper.WriteLine($"{testNum} => {num}");
        // var numRes = DecodeId(num);
        // _testOutputHelper.WriteLine($"{num} => {numRes}");
        //
        // var res = UIntToBase34(num);
        // _testOutputHelper.WriteLine($"{num} => {res}");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(11)]
    [InlineData(111)]
    [InlineData(1111)]
    [InlineData(11111)]
    [InlineData(22222)]
    [InlineData(11417)]
    [InlineData(111111)]
    [InlineData(1111111)]
    [InlineData(11111111)]
    [InlineData(111111111)]
    [InlineData(10_7374_1823)]
    // [InlineData(11_1111_1111)]
    [InlineData(34)]
    public void Test_EncodeIdWith30bits(uint testNum)
    {
        var binaryViewStr = $"{Convert.ToString(testNum, 2)}";
        _testOutputHelper.WriteLine($"源二进制：{testNum} => {binaryViewStr},  二进制转回源: {Convert.ToInt32(binaryViewStr, 2)}");

        var encodeIdWith30Bits = EncodeIdWith30Bits(testNum);
        var uIntToBase34 = UIntToBase34(encodeIdWith30Bits);

        _testOutputHelper.WriteLine($"{testNum} 30bits编码到整型 => {encodeIdWith30Bits}, 0b_:{Convert.ToString(encodeIdWith30Bits, 2)}, uIntToBase34: {uIntToBase34}");


        var decodeIdWith30Bits = DecodeIdWith30Bits(encodeIdWith30Bits);
        var base34ToInt = Base34ToInt(uIntToBase34);

        _testOutputHelper.WriteLine($"{encodeIdWith30Bits} 30bits解码到整型 {decodeIdWith30Bits},  0b_: {Convert.ToString(decodeIdWith30Bits, 2)} , base34ToInt: {base34ToInt}");

        Assert.True(testNum == decodeIdWith30Bits);
    }

    public string Codec(uint testNum)
    {
        var encodeIdWith30Bits = EncodeIdWith30Bits(testNum);
        var uIntToBase34 = UIntToBase34(encodeIdWith30Bits);
        return uIntToBase34;
    }

    public uint Codec(string base34)
    {
        var encodeIdWith30Bits = Base34ToInt(base34);
        var decodeId = DecodeIdWith30Bits(encodeIdWith30Bits);
        return decodeId;
    }

    [Fact]
    public void TestCodec()
    {
        // const uint XorConst = 16729765;
        // const string Source = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
        const int count = 100_0000;
        var codeMap = new Dictionary<uint, string>(count);
        var decMap = new Dictionary<string, uint>(count);
        var digitMap = new Dictionary<string, uint>();

        for (uint i = 0; i <= count; i++)
        {
            var code = Codec(i);
            codeMap[i] = code;
            if (code.All(char.IsDigit))
            {
                digitMap[code] = i;
            }

            // _testOutputHelper.WriteLine(code);

            var dec = Codec(code);
            decMap[code] = dec;
        }

        _testOutputHelper.WriteLine(codeMap.Count.ToString());
        _testOutputHelper.WriteLine(decMap.Count.ToString());
        _testOutputHelper.WriteLine(digitMap.Count.ToString());
        Assert.True(codeMap.Count == decMap.Count);
    }

    [Fact]
    public void TestCharLength()
    {
        const string str = "😄☺😃👆";
        _testOutputHelper.WriteLine("length: {0}", str.Length);

        var stringInfo = new StringInfo(str);
        _testOutputHelper.WriteLine("length: {0}", stringInfo.LengthInTextElements);
        var textElementEnumerator = StringInfo.GetTextElementEnumerator(str);
        while (textElementEnumerator.MoveNext())
        {
            var textElement = textElementEnumerator.GetTextElement();
            _testOutputHelper.WriteLine("char: {0}, length: {1}", textElement, textElement.Length);


            for (var i = 0; i < textElement.Length; i++)
            {
                var thisChar = textElement[i];
                _testOutputHelper.WriteLine("char: {0}, code: {1}", thisChar, thisChar.ToString());
                var bytes = Encoding.Unicode.GetBytes(new[] {thisChar});


                _testOutputHelper.WriteLine("char: {0}, code: {1}", thisChar, thisChar.ToString());
            }
        }
    }
}