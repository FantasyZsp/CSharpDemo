using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebApplication
{
    /**
     *配置项执行顺序
     * 
     * WebHostDefaults:0
     * HostConfiguration:1
     * AppConfiguration:2
     * WebApplication.Startup.ConfigureServices invoke
     */
    public static class Program
    {
        private static int _invoked = 0;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    Console.WriteLine($"WebHostDefaults:{_invoked++}");
                    webHostBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    Console.WriteLine($"AppConfiguration:{_invoked++}");
                })
                .ConfigureHostConfiguration(configurationBuilder =>
                {
                    Console.WriteLine($"HostConfiguration:{_invoked++}");
                });
    }
}