using System;
using System.Threading.Tasks;
using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Client.Girl.Rest;
using WebApplication.model;

namespace WebApplication.Controllers
{
    [Route("/api/[controller]")]
    public class GirlController : ControllerBase
    {
        private readonly IGirlWebApiClient _girlWebApiClient;

        public GirlController(IGirlWebApiClient girlWebApiClient)
        {
            _girlWebApiClient = girlWebApiClient;
            Console.WriteLine($"GirlController construct with _girlWebApiClient hashcode {_girlWebApiClient.GetHashCode()}");
        }

        [HttpGet]
        public async Task<Girl> FindById(string id)
        {
            return new Girl {Id = id, Name = "FindById", Age = 18};
        }

        [HttpGet("json")]
        public async Task<JsonResult> FindByIdJson(string id)
        {
            return new JsonResult(new RestResult<Girl>
            {
                State = 200,
                Code = "code",
                Msg = "msg",
                Payload = new Girl {Id = id, Name = "FindByIdJson", Age = 18}
            });
        }


        [HttpGet("byRestClient")]
        public async Task<Girl> ByRestClient(string id)
        {
            return await _girlWebApiClient.FindById("byRestClient");
        }

        [HttpGet("byRestClientJson")]
        public async Task<RestResult<Girl>> ByRestClientJson(string id)
        {
            return await _girlWebApiClient.FindByIdJson("byRestClientJson");
        }
    }
}