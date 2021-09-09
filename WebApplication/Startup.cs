using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Services;

namespace WebApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("WebApplication.Startup.ConfigureServices invoke");
            services.AddMvc().AddControllersAsServices();
            services.AddSingleton<MyService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage(); // 如果是开发环境，错误页面将呈现堆栈以及其他辅助排错的信息；如果不开启，会返回500
            // }

            app.UseRouting();
            //
            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            // });
            // 将路由映射到controller，没有 MapControllers 这个，是无法通过预期的url访问到controller的
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}