using System;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    public class HelloWordController : Controller
    {
        public HelloWordController()
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