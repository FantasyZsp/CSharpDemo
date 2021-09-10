using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    // [ApiController] // 继承了 ControllerBase，可以不打注解
    [Route("api/[controller]/[action]")]
    public class HelloWorldController : Controller
    {
        private static long _counter;
        // private readonly ILifetimeScope _autofacContainer;

        public HelloWorldController(
        )
        {
            // _autofacContainer = autofacContainer;
            Console.WriteLine(
                "HelloWordController constructor invoke"); // netcore中Controller是非单例对象，每次请求都是创建新的，但内部依赖的bean还是跟随本身的配置，如 MyService 还是单例（但如果同类型有多个，可能优先注入了瞬时的）。
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
            // var resolveNamed = _autofacContainer.ResolveNamed<string>("autofac"); // ex,TODO 查看异常原因


            var enumerable = myServices as MyService[] ?? myServices.ToArray();
            return
                $"Hello,{this.GetHashCode()}, {typeof(MyService)} with {enumerable.Length}:{string.Join(",", enumerable.Select(s => s.GetHashCode().ToString()).ToArray())}! + {Interlocked.Increment(ref _counter)}";
        }
    }
}