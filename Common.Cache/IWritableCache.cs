namespace Common.Cache;

public interface IWritableCache<in TK, TV> where TV : class, new()
{
    Task<TV> Put(TK key, TV value, long ttl);

    Task<bool> Remove(TK key);
}