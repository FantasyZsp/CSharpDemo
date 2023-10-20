namespace Common;

public class NumberToChineseUtils
{
    private static readonly string[] ChineseDigits = new string[]
    {
        "零", "一", "二", "三", "四", "五", "六", "七", "八", "九"
    };

    private static readonly string[] Units = new string[]
    {
        "", "十", "百", "千", "万", "亿"
    };

    public static string ToChinese(int number)
    {
        if (number == 0)
        {
            return ChineseDigits[0];
        }

        string result = "";
        int unitIndex = 0;

        while (number > 0)
        {
            int digit = number % 10;
            if (digit != 0)
            {
                result = ChineseDigits[digit] + Units[unitIndex] + result;
            }
            else if (unitIndex > 0 && result[0] != ChineseDigits[0][0])
            {
                result = ChineseDigits[digit] + result;
            }

            number /= 10;
            unitIndex++;
        }

        return result;
    }
}