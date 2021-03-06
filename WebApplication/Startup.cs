using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebApplication.Middlewares;
using WebApplication.Services;

namespace WebApplication
{
    public class Startup
    {
        private ILifetimeScope AutofacContainer { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("WebApplication.Startup.ConfigureServices invoke");
            services.AddMvc().AddControllersAsServices();
            services.AddSingleton<MyService>();
            // services.AddTransient<MyService>();

            // services.AddHttpClient<MyHttpClient>(client => client.BaseAddress = new Uri("http://localhost:5001/"));

            // var baseAdder = new Uri("http://localhost:5000/");
            // services
            //     .AddHttpApi<IGirlWebApiClient>()
            //     .ConfigureHttpApi(configOptions => configOptions.HttpHost = baseAdder)
            //     // .ConfigureHttpClient(c => c.BaseAddress = baseAdder)
            //     ;

            ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine("WebApplication.Startup.Configure invoke");

            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            var iEchoService = AutofacContainer.ResolveNamed<IEchoService>("iEchoService");
            // var echoService = AutofacContainer.ResolveNamed<IEchoService>("echoService");
            // Console.WriteLine($"iEchoService is equal to echoService? {iEchoService == echoService}");


            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage(); // ??????????????????????????????????????????????????????????????????????????????????????????????????????????????????500
            // }

            app.UseRouting();
            app.UseMiddleware<ObsoleteControllerLogMiddleware>();

            //
            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            // });
            // ??????????????????controller????????? MapControllers ?????????????????????????????????url?????????controller???
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            Console.WriteLine("WebApplication.Startup.ConfigureContainer invoke");
            builder.RegisterType<EchoService>().Named<IEchoService>("iEchoService");
            builder.RegisterType<EchoService>().Named<EchoService>("echoService");
            // var host = new Uri("http://localhost:5000/");
            // builder.RegisterHttpApi<IGirlWebApiClient>()
            //     .ConfigureHttpApiConfig(configOptions => configOptions.HttpHost = host);
            builder.RegisterModule<WebApiClientRegisterModule>();

            // ????????????json??????
            var properties = Configuration.GetSection("MQConnections:Mq1").Get<MqProperties>();
            Console.WriteLine(JsonConvert.SerializeObject(properties));
        }
    }
}