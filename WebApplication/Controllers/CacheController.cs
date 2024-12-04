using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Dto;
using DotNetCommon.Data;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Filters;
using WebApplication.Services;
using YW.ModelManagement.Common;

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

    [HttpGet]
    [NameParamCollectorFilter(ParamPath = "#girl.Id")]
    public async Task<Result<List<Girl>>> GetByDto(Girl girl)
    {
        var byDto = await _cacheService.GetByDto(new Girl() {Id = girl.Id});
        return Result.Ok(new List<Girl>() {byDto});
    }

    [HttpGet]
    [NameParamCollectorFilter(ParamPath = "#girl.Id")]
    public async Task<Result<Page<Girl>>> GetByDto2(Girl girl)
    {
        var byDto = await _cacheService.GetByDto(new Girl() {Id = girl.Id});
        return Result.Ok(new Page<Girl>() {TotalCount = 1, List = new List<Girl>() {byDto}});
    }

    [HttpGet]
    [NameParamCollectorFilter(ParamPath = "#girl.Id")]
    public async Task<Result<PageExt<Girl>>> GetByDto3(Girl girl)
    {
        var byDto = await _cacheService.GetByDto(new Girl() {Id = girl.Id});
        return Result.Ok(new PageExt<Girl>() {Page = new Page<Girl>() {TotalCount = 1, List = new List<Girl>() {byDto}}});
    }
    [HttpPost]
    [NameParamCollectorFilter(ParamPath = "#girl.Id")]
    public async Task<Result<PageExt<Girl>>> GetByDtoPost([FromBody] Girl girl)
    {
        var byDto = await _cacheService.GetByDto(new Girl() {Id = girl.Id});
        return Result.Ok(new PageExt<Girl>() {Page = new Page<Girl>() {TotalCount = 1, List = new List<Girl>() {byDto}}});
    }

    [HttpPut]
    public async Task<Girl> Put(string id)
    {
        return await _cacheService.Put(id);
    }

    [HttpDelete]
    public async Task<Girl> Remove(string id)
    {
        return await _cacheService.Remove(id);
    }
}