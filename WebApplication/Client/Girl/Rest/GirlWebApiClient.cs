using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;
using WebApplication.model;

namespace WebApplication.Client.Girl.Rest
{
    public interface IGirlWebApiClient : IHttpApi
    {
        [HttpGet("/api/girl")]
        public Task<Common.Dto.Girl> FindById(string id);

        [HttpGet("/api/girl/json")]
        public Task<RestResult<Common.Dto.Girl>> FindByIdJson(string id);
    }
}