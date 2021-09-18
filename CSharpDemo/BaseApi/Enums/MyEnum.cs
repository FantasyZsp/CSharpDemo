using System;
using CSharpDemo.BaseApi.DTO;

namespace CSharpDemo.BaseApi.Enums
{
    public enum MyEnum
    {
        First = 1,
        Second = 2
    }


    public static class MyEnumTest
    {
        public static void Test()
        {
            Console.WriteLine((int) MyEnum.First);


            Console.WriteLine(MyEnum.Second);
            Console.WriteLine(new DemoDTO().Id);
            Console.WriteLine(Enum.IsDefined(typeof(MyEnum), 100));
            var vvv = Enum.ToObject(typeof(MyEnum), 100); // 不会报错
            Console.WriteLine(vvv);
        }
    }
}