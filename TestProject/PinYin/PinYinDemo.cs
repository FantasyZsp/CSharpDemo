using System.Text.RegularExpressions;
using Common;
using Common.Extensions;
using DotNetCommon.Extensions;
using hyjiacan.py4n;
using Microsoft.International.Converters.PinYinConverter;
using ToolGood.Words;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.PinYin;

public class PinYinDemo
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PinYinDemo(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void WordsHelperTest()
    {
        var yinhang = WordsHelper.GetPinyin("银行 行不行");
        var xingbuxing = WordsHelper.GetPinyin("行不行");
        var pinyin = WordsHelper.GetPinyin("我爱中国");
        var firstPinyin = WordsHelper.GetFirstPinyin("我爱中国");
        var multiPinyin = WordsHelper.GetPinyin("中洋和天下");
        var multiFirstPinyin = WordsHelper.GetFirstPinyin("中洋和天下");
        var allPinyin = WordsHelper.GetAllPinyin('洋');
        var charPinyin = WordsHelper.GetAllPinyin('-');
        _testOutputHelper.WriteLine(yinhang);
        _testOutputHelper.WriteLine(xingbuxing);
        _testOutputHelper.WriteLine(pinyin);
        _testOutputHelper.WriteLine(firstPinyin);
        _testOutputHelper.WriteLine(multiPinyin);
        _testOutputHelper.WriteLine(multiFirstPinyin);
        _testOutputHelper.WriteLine(allPinyin.ToJson());
        _testOutputHelper.WriteLine(charPinyin.ToJson());
    }

    [Fact]
    public void WordsHelperNotGoodCaseTest()
    {
        var pinyin = WordsHelper.GetPinyin("绿色");
        _testOutputHelper.WriteLine(pinyin);

        _testOutputHelper.WriteLine(new ChineseChar('绿').Pinyins.ToJson());
        _testOutputHelper.WriteLine(new ChineseChar('地').Pinyins.ToJson());


        _testOutputHelper.WriteLine(WordsHelper.GetAllPinyin('乐', true).ToJson());
        _testOutputHelper.WriteLine(WordsHelper.GetPinyin("如果是一个完整的语句它就不能正确地分辨读音，比如智者乐水仁者乐山。", false));
        _testOutputHelper.WriteLine(WordsHelper.GetPinyin("不能正确地分辨读音，比如智者乐水仁者乐山。", false));
        _testOutputHelper.WriteLine(WordsHelper.GetPinyin("分辨读音，比如智者乐水仁者乐山。", false));
        _testOutputHelper.WriteLine(WordsHelper.GetPinyin("读音，比如智者乐水仁者乐山。", false));
        _testOutputHelper.WriteLine(WordsHelper.GetPinyin("比如智者乐水仁者乐山。", false));
        _testOutputHelper.WriteLine(WordsHelper.GetPinyin("智者乐水仁者乐山。", false));
        _testOutputHelper.WriteLine(WordsHelper.GetPinyin("智者乐水", false));
        _testOutputHelper.WriteLine(WordsHelper.GetPinyin("智者乐水。", false));
        _testOutputHelper.WriteLine(WordsHelper.GetPinyin("智者乐水，仁者乐山。", false));
        _testOutputHelper.WriteLine(WordsHelper.GetPinyin("比如“智者乐水，仁者乐山。”", false));
        ;
    }

    [Fact]
    public void WordsHelperMultiTest2()
    {
        var pinyin22 = WordsHelper.GetPinyin("乐页5村");
        _testOutputHelper.WriteLine(pinyin22);
        var pinyin = WordsHelper.GetPinyin("正弘山10号楼1单元");
        _testOutputHelper.WriteLine(pinyin);
        var fisrpinyin = WordsHelper.GetPinyin("正弘山10号楼1单元");
        _testOutputHelper.WriteLine(fisrpinyin);

        var zhangsun = WordsHelper.GetPinyin("安乐公寓");
        _testOutputHelper.WriteLine(zhangsun);
    }

    [Fact]
    public void ChineseCharTest()
    {
        _testOutputHelper.WriteLine(new ChineseChar('绿').Pinyins.ToJson());
        _testOutputHelper.WriteLine(new ChineseChar('地').Pinyins.ToJson());
    }

    [Fact]
    public void Pinyin4NetTest()
    {
        // 设置拼音输出格式
        const PinyinFormat format = PinyinFormat.WITHOUT_TONE | PinyinFormat.LOWERCASE | PinyinFormat.WITH_U_UNICODE;
        var hanzi = '李';

        var pinyin = Pinyin4Net.GetPinyin(hanzi);

        _testOutputHelper.WriteLine(pinyin.ToJson());

        var pinyinFormat = Pinyin4Net.GetPinyin(hanzi, format);
        _testOutputHelper.WriteLine(pinyinFormat.ToJson());


        var firstPinyin = Pinyin4Net.GetFirstPinyin(hanzi);
        _testOutputHelper.WriteLine(firstPinyin);
        ;
        var hanZiByPinyin = Pinyin4Net.GetHanzi("li", true);
        _testOutputHelper.WriteLine(hanZiByPinyin.ToJson());
    }


    [Fact]
    public void TestNumberToChinese()
    {
        int number = 12;
        string chineseNumber = NumberToChineseUtils.ToChinese(number);
        _testOutputHelper.WriteLine(chineseNumber); // Output: 一万二千三百四十五
    }


    [Fact]
    public void TestMain()
    {
        var input = "后三   〇一住宅123小区";
        var nameFormatted = input.RetainChineseLettersAndDigits();
        var pinyin = WordsHelper.GetPinyin(nameFormatted);
        var firstPinyin = WordsHelper.GetFirstPinyin(nameFormatted);
        _testOutputHelper.WriteLine(nameFormatted);
        _testOutputHelper.WriteLine(pinyin);
        _testOutputHelper.WriteLine(firstPinyin);
    }

    [Fact]
    public void TestBlank()
    {
        var input = "";
        var nameFormatted = input.RetainChineseLettersAndDigits();
        var pinyin = WordsHelper.GetPinyin(nameFormatted);
        var firstPinyin = WordsHelper.GetFirstPinyin(nameFormatted);
        _testOutputHelper.WriteLine(nameFormatted);
        _testOutputHelper.WriteLine(pinyin);
        _testOutputHelper.WriteLine(firstPinyin);
    }
}