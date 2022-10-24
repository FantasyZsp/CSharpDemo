namespace Common.Cache;

public interface IReadableCache<in TK>
{
    Task<object> Get(TK key);
}