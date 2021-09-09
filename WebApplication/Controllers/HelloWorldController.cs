using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HelloWorldController : Controller
    {
        private static long _counter;
        private readonly ILifetimeScope _autofacContainer;

        public HelloWorldController(ILifetimeScope autofacContainer)
        {
            _autofacContainer = autofacContainer;
            Console.WriteLine(
                "HelloWordController constructor invoke"); // TODO  用了autofac，当前Controller 变成了非单例对象，但是 MyService 还是单例。
        }


        [HttpGet("hi")] // 声明自己独有的路径,拼接在 Route#Template 的后面
        public string HelloWorld2([FromServices] MyService myService) // [FromServices] DI。优先注入瞬时的
        {
            return
                $"Hello, {typeof(MyService)} with {myService.Invoke()}:{myService.GetHashCode()}! + {Interlocked.Increment(ref _counter)}";
        }

        [HttpGet("hi")]
        public string HelloWorld([FromServices] IEnumerable<MyService> myServices) // [FromServices] DI
        {
            var resolveNamed = _autofacContainer.ResolveNamed<string>("autofac"); // ex,TODO 查看异常原因


            var enumerable = myServices as MyService[] ?? myServices.ToArray();
            return
                $"Hello,{resolveNamed}, {typeof(MyService)} with {enumerable.Length}:{string.Join(",", enumerable.Select(s => s.GetHashCode().ToString()).ToArray())}! + {Interlocked.Increment(ref _counter)}";
        }
    }
}