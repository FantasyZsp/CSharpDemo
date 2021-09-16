using System;
using Autofac;
using WebApiClient.Extensions.Autofac;
using WebApplication.Client.Girl.Rest;

namespace WebApplication
{
    public class WebApiClientRegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var host = new Uri("http://localhost:5000/");
            builder.RegisterHttpApi<IGirlWebApiClient>()
                .ConfigureHttpApiConfig(configOptions => configOptions.HttpHost = host);
        }
    }
}