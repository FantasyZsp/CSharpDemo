using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebApplication
{
    /**
     *配置项执行顺序，与声明顺序无关，除了UseStart和ConfigureServices之间
     * 
     * WebHostDefaults:0
     * HostConfiguration:1
     * AppConfiguration:2
     * WebApplication.Startup.ConfigureServices invoke
     * Services:3
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
                // .UseServiceProviderFactory(new AutofacServiceProviderFactory()) // 使用autofac IOC容器
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    Console.WriteLine($"WebHostDefaults:{_invoked++}");
                    webHostBuilder
                        // useStart内部调用了ConfigureServices，所以和 ConfigureServices 方法谁在前谁先执行。
                        // 因为本质上是在追加action。
                        .UseStartup<Startup>();
                })
                .ConfigureHostConfiguration(configurationBuilder =>
                {
                    Console.WriteLine($"HostConfiguration:{_invoked++}");
                })
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    Console.WriteLine($"AppConfiguration:{_invoked++}");
                })
                .ConfigureServices(collection => { Console.WriteLine($"Services:{_invoked++}"); });
    }
}