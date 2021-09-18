using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpDemo.BaseApi.Linq
{
    public static class LinqExample
    {
        public static void TestLinq()
        {
            var list = new List<(string, int)>();
            for (var i = 0; i < 100; i++)
            {
                list.Add((i.ToString(), i));
            }

            Console.WriteLine(list.Count);

            var list1 = list.Select(v =>
                new
                {
                    v11 = v.Item1,
                    v22 = v.Item2.ToString()
                }
            ).ToList();
        }
    }
}