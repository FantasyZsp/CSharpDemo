using System;

namespace WebApplication.Services
{
    public class EchoService : IEchoService
    {
        public void echo()
        {
            Console.WriteLine("EchoService invoke");
        }
    }
}