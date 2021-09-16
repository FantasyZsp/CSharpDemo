using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AutofacExample
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
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}