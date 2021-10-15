using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using WebApplication.Client.Girl.Rest;
using ILogger = Serilog.ILogger;

namespace WebApplication.Controllers
{
    [Route("/api")]
    public class SerilogTestController : ControllerBase
    {
        private static int CallCount;

        private readonly ILogger _staticLogger = Log.ForContext<SerilogTestController>();
        private readonly ILogger<SerilogTestController> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public SerilogTestController(IGirlWebApiClient girlWebApiClient, ILogger<SerilogTestController> logger,
            IDiagnosticContext diagnosticContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _diagnosticContext = diagnosticContext ?? throw new ArgumentNullException(nameof(diagnosticContext));
            Console.WriteLine($"SerilogTestController construct with _girlWebApiClient hashcode {girlWebApiClient.GetHashCode()}" +
                              $" and {logger.GetHashCode()} and {_staticLogger.GetHashCode()}");
        }

        [HttpGet]
        public async Task<Girl> FindById(string id)
        {
            _diagnosticContext.Set("IndexCallCount", Interlocked.Increment(ref CallCount));

            _staticLogger.Information("staticLogger :Disk quota {Quota} MB exceeded by {@User}, {Hi} {app}", 1024, new
            {
                Id = 1,
                Name = "user"
            }, "hello");

            _logger.LogInformation("test");

            _logger.LogInformation("Disk quota {Quota} MB exceeded by {@User}, {IndexCallCount} ", 1024, new
            {
                Id = 1,
                Name = "user"
            }, CallCount);
            var requestHeader = HttpContext.Request.Headers["Authorization"];
            var requestHeader2 = HttpContext.Request.Headers["CustomHeader"];

            return await Task.FromResult(new Girl {Id = id, Name = $"{requestHeader.ToString()} ==  {requestHeader2.ToString()}", Age = 18});
        }
    }
}