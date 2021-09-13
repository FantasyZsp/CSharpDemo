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
            // TODO 外部化配置 packageRepositoryHost
            var packageRepositoryHost = new Uri("http://localhost:5000/");
            builder.RegisterHttpApi<IGirlWebApiClient>()
                .ConfigureHttpApiConfig(configOptions => configOptions.HttpHost = packageRepositoryHost);
        }
    }
}