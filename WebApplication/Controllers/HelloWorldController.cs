using System;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    // [ApiController]
    [Route("api/[controller]/[action]")]
    public class HelloWorldController : Controller
    {
        public HelloWorldController()
        {
            Console.WriteLine("HelloWordController constructor invoke");
        }


        [HttpGet]
        public string HelloWorld()
        {
            return "Hello, World!";
        }
    }
}