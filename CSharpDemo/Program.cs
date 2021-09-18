using CSharpDemo.BaseApi.Enums;
using CSharpDemo.BaseApi.Linq;
using CSharpDemo.BaseApi.Strings;

namespace CSharpDemo
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            StringExample.TestString();
            MyEnumTest.Test();
            LinqExample.TestLinq();
        }
    }
}