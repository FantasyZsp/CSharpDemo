using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;

namespace WebApplication.Middlewares
{
    public class ObsoleteControllerLogMiddleware
    {
        private readonly RequestDelegate _next;

        public ObsoleteControllerLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            RecordObsoleteAccess(httpContext);
            await _next(httpContext);
        }

        private static void RecordObsoleteAccess(HttpContext httpContext)
        {
            var endpointFeature = httpContext.Features.Get<IEndpointFeature>();
            var endpoint = endpointFeature.Endpoint;
            if (endpoint == null) return;

            Console.WriteLine(endpoint.DisplayName);
            Console.WriteLine(httpContext.Request.GetDisplayUrl());
            if (endpoint is RouteEndpoint routeEndpoint)
            {
                Console.WriteLine(routeEndpoint.RoutePattern.RawText);
            }

            var obsoleteAttribute = endpoint.Metadata.GetMetadata<ObsoleteAttribute>();

            if (obsoleteAttribute != null)
            {
                Console.WriteLine(obsoleteAttribute.Message);
                Console.WriteLine(obsoleteAttribute.IsError);
                Console.WriteLine(httpContext.Request.Headers["Referer"]);
                Console.WriteLine(httpContext.Request.Headers["Origin"]);
                Console.WriteLine(httpContext.Request.Headers["User-Agent"]);
                Console.WriteLine(httpContext.Request.Headers["Host"]);
            }
        }
    }
}