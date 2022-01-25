using System;
using System.Threading.Tasks;
using Common.Dto;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Route("/api/[controller]")]
    public class SimpleController : ControllerBase
    {
        [HttpGet]
        [Obsolete]
        public async Task<Girl> FindById(string id)
        {
            var requestHeader = HttpContext.Request.Headers["Authorization"];
            var requestHeader2 = HttpContext.Request.Headers["CustomHeader"];

            return await Task.FromResult(new Girl {Id = id, Name = $"{requestHeader.ToString()} ==  {requestHeader2.ToString()}", Age = 18});
        }
    }
}