using System.Text.RegularExpressions;

namespace Common.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// 用于转拼音前的字符处理
    /// </summary>
    public const string ChinesePrepareForPinyinPattern = @"[^\u4E00-\u9FA5a-zA-Z0-9]";


    public static string RetainChineseLettersAndDigits(this string input)
    {
        if (input == null)
        {
            return "";
        }

        // 定义正则表达式模式，匹配非中文、英文和数字字符
        // 使用 Regex.Replace 方法将匹配的字符替换为空字符串
        var result = Regex.Replace(input, ChinesePrepareForPinyinPattern, "");
        return result;
    }
}