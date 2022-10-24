namespace Common.Cache;

public interface IReadableCache<in TK>
{
    Task<TV> Get<TV>(TK key);
}