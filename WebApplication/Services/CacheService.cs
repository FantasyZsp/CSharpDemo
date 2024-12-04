using System.Threading.Tasks;
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
    
    
    [Cacheable(key: "#girl.id", Prefix = "test", Order = 1, Ttl = 10_000)]
    public virtual async Task<Girl> GetByDto(Girl girl)
    {
        return await Task.FromResult(new Girl {Id = girl.Id, Name = "name", Age = 18});
    }

    [CachePut(key: "#id", Prefix = "test", Order = 1, Ttl = 10_000)]
    public virtual async Task<Girl> Put(string id)
    {
        return await Task.FromResult(new Girl {Id = id, Name = "putData", Age = 18});
    }


    [CacheEvict(key: "#id", Prefix = "test", Order = 1)]
    public virtual async Task<Girl> Remove(string id)
    {
        return await Task.FromResult(new Girl {Id = id, Name = "removeData", Age = 18});
    }
}