using Common.Supports;

namespace Common.Lock;

public interface ILockClient : IOrdered, INamed
{
    Task Lock(TimeSpan waitTime);
}