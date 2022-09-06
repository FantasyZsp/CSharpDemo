using System;
using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

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
     * WebApplication.Startup.ConfigureContainer invoke
     * WebApplication.Startup.Configure invoke
     *
     * 
     */
    public static class Program
    {
        private static int _invoked = 0;

        public static void Main(string[] args)
        {
            ConfigLogger();
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) // 使用autofac IOC容器
                .UseSerilog()
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    Console.WriteLine($"WebHostDefaults:{_invoked++}");
                    webHostBuilder
                        // useStart内部调用了ConfigureServices，所以和 ConfigureServices 方法谁在前谁先执行。
                        // 因为本质上是在追加action。
                        .UseStartup<Startup>();
                });

        private static void ConfigLogger()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.WithThreadName()
                .CreateLogger();
            var jobLog = Log.ForContext("app", "dotnetDemo");
            jobLog.Information("Disk quota {Quota} MB exceeded by {@User}, {@Hi}", 1024, new
            {
                Id = 1,
                Name = "user"
            }, "hello");
        }
    }
}