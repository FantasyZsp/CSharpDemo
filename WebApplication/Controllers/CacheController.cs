using System.Threading.Tasks;
using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Services;

namespace WebApplication.Controllers;

[Route("[controller]/[action]")]
public class CacheController : Controller
{
    private readonly CacheService _cacheService;

    public CacheController(CacheService cacheService)
    {
        _cacheService = cacheService;
    }


    [HttpGet]
    public async Task<Girl> GetById(string id)
    {
        return await _cacheService.GetById(id);
    }
}