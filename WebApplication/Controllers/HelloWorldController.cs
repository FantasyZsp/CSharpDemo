using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HelloWorldController : Controller
    {
        private static long _counter;

        public HelloWorldController()
        {
            Console.WriteLine("HelloWordController constructor invoke");
        }


        [HttpGet("hi")] // 声明自己独有的路径,拼接在 Route#Template 的后面
        public string HelloWorld([FromServices] MyService myService) // [FromServices] DI。优先注入瞬时的
        {
            return
                $"Hello, {typeof(MyService)} with {myService.Invoke()}:{myService.GetHashCode()}! + {Interlocked.Increment(ref _counter)}";
        }

        [HttpGet("hi")]
        public string HelloWorld2([FromServices] IEnumerable<MyService> myServices) // [FromServices] DI
        {
            var enumerable = myServices as MyService[] ?? myServices.ToArray();
            return
                $"Hello, {typeof(MyService)} with {enumerable.Length}:{string.Join(",", enumerable.Select(s => s.GetHashCode().ToString()).ToArray())}! + {Interlocked.Increment(ref _counter)}";
        }
    }
}