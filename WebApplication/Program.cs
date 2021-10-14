using System;
using System.Threading;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;

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
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    Console.WriteLine($"WebHostDefaults:{_invoked++}");
                    webHostBuilder
                        // useStart内部调用了ConfigureServices，所以和 ConfigureServices 方法谁在前谁先执行。
                        // 因为本质上是在追加action。
                        .UseStartup<Startup>();
                })
                .ConfigureHostConfiguration(configurationBuilder => { Console.WriteLine($"HostConfiguration:{_invoked++}"); })
                .ConfigureAppConfiguration(configurationBuilder => { Console.WriteLine($"AppConfiguration:{_invoked++}"); })
                .ConfigureServices(collection => { Console.WriteLine($"Services:{_invoked++}"); });


        private static void ConfigLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.With(new ThreadIdEnricher())
                .Enrich.WithProperty("Version", "1.0.0")
                .MinimumLevel.Debug()
                // .WriteTo.Console()
                // .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm} [{Level:u3}] [{ThreadId}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                .CreateLogger();
            var jobLog = Log.ForContext("app", "dotnetDemo");

            jobLog.Debug("Disk quota {Quota} MB exceeded by {User}, {Hi} {app}", 1024, new
            {
                Id = 1,
                Name = "user"
            }, "hello");
            jobLog.Warning("Disk quota {Quota} MB exceeded by {@User}, {@Hi}", 1024, new
            {
                Id = 1,
                Name = "user"
            }, "hello");
        }

        private class ThreadIdEnricher : ILogEventEnricher
        {
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "ThreadId", Thread.CurrentThread.ManagedThreadId));
            }
        }
    }
}