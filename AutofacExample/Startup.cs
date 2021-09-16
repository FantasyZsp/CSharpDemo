using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacExample.DI.AutofacModule.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutofacExample
{
    /// <summary>
    /// 官方指导
    ///  https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/startup?view=aspnetcore-5.0
    /// </summary>
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }


        private readonly IWebHostEnvironment _webEnv;
        private readonly IHostEnvironment _hostEnv;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment webEnv, IHostEnvironment hostEnv, IConfiguration configuration)
        {
            _webEnv = webEnv;
            _hostEnv = hostEnv;
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); }); });

            var lifetimeScope = app.ApplicationServices.GetAutofacRoot();
            {
                // 获取依赖
                var thirdComponent = lifetimeScope.ResolveOptional<ThirdComponent>();
                thirdComponent?.Produce();

                var firstComponent = lifetimeScope.ResolveOptional<FirstComponent>();
                firstComponent?.Produce();
            }

            var appProperties = app.Properties;

            Console.WriteLine("xxx");
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // 直接扫描程序集
            builder.RegisterAssemblyModules(typeof(FirstComponent).Assembly);
            // builder.RegisterModule(new NumericComponentModule());

            // 获取所有程序集
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Console.WriteLine(assemblies);
        }
    }
}