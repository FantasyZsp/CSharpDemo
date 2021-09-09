using System;
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
        public string HelloWorld([FromServices] MyService myService) // [FromServices] DI
        {
            return
                $"Hello, {typeof(MyService)} with {myService.Invoke()}! + {Interlocked.Increment(ref _counter)}";
        }
    }
}