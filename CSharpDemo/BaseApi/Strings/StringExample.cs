using System;

namespace CSharpDemo.BaseApi.Strings
{
    public class StringExample
    {
        public static void TestString()
        {
            const string prefix = "prefix";
            var innerPath = "innerPath";
            // innerPath = "innerPath";

            var example = "prefix/sssss.zip";
            Console.WriteLine(example.StartsWith(prefix + "/"));
            Console.WriteLine(example.Substring((prefix + "/").Length));
            var path = $"{prefix}/{innerPath ?? "myPath" + ".zip"}";
            Console.WriteLine();
        }
    }
}