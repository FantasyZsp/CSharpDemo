using System.Threading.Tasks;
using Common.Cache;
using Common.Cache.Annotations;
using Common.Dto;

namespace WebApplication.Services;

public class CacheService
{
    [Cacheable(key: "#id", Prefix = "test", Order = 1, Ttl = 10_000)]
    public virtual async Task<Girl> GetById(string id)
    {
        return await Task.FromResult(new Girl {Id = id, Name = "name", Age = 18});
    }
}