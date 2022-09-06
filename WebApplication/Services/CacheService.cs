using System.Threading.Tasks;
using Common.Dto;

namespace WebApplication.Services;

public class CacheService
{
    [Cacheable(prefix: "test", key: "#id")]
    public virtual async Task<Girl> GetById(string id)
    {
        return await Task.FromResult(new Girl {Id = id, Name = "name", Age = 18});
    }
}