using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpDemo.BaseApi.DTO;
using CSharpDemo.BaseApi.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CSharpDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine((int) MyEnum.First);
            Console.WriteLine(MyEnum.Second);
            Console.WriteLine(new DemoDTO().Id);
        }
    }
}