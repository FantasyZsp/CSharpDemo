using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject.Locks;

/// <summary>
/// 基于key的内存锁
/// </summary>
public abstract class MemoryLockUtil
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> LockFactory = new(128, 8192);

    public static bool TryLock(string lockKey)
    {
        return GetOrAdd(lockKey).Wait(0);
    }

    public static bool TryLock(string lockKey, int millisecondsTimeout)
    {
        return GetOrAdd(lockKey).Wait(millisecondsTimeout);
    }

    public static bool TryLock(string lockKey, TimeSpan timeout)
    {
        return GetOrAdd(lockKey).Wait(timeout);
    }

    public async Task<bool> TryLockAsync(string lockKey)
    {
        return await GetOrAdd(lockKey).WaitAsync(0);
    }

    public static async Task<bool> TryLockAsync(string lockKey, int millisecondsTimeout)
    {
        return await GetOrAdd(lockKey).WaitAsync(millisecondsTimeout);
    }

    public static async Task<bool> TryLockAsync(string lockKey, TimeSpan timeout)
    {
        return await GetOrAdd(lockKey).WaitAsync(timeout);
    }

    /// <summary>
    /// 加锁的逻辑：
    /// 1、幂等的拿到一个对象，调用lock；如果阻塞了，应该阻塞在lock上而不是构造锁对象上。
    /// 解锁的逻辑
    /// 1、锁对象释放。为了释放空间，需要删掉在map中的引用。删完会导致后续进来的拿到的锁对象是新的。从而导致和已拿到锁对象的线程并发控制失效；
    /// </summary>
    /// <param name="lockKey"></param>
    public static void Lock(string lockKey)
    {
        GetOrAdd(lockKey).Wait();
    }

    public static Task LockAsync(string lockKey)
    {
        return GetOrAdd(lockKey).WaitAsync();
    }

    public static Task GetLockAsync(string lockKey)
    {
        return GetOrAdd(lockKey).WaitAsync();
    }

    public static void Unlock(string lockKey)
    {
        GetOrAdd(lockKey).Release();
        LockFactory.TryRemove(lockKey, out _);
    }


    private static SemaphoreSlim GetOrAdd(string lockKey)
    {
        return LockFactory.GetOrAdd(lockKey, _ => new SemaphoreSlim(1, 1));
    }
}