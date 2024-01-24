using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject.Locks;

/// <summary>
/// 基于key的内存锁
/// 不可重入
/// </summary>
public class TinyLock : IDisposable, IAsyncDisposable
{
    public static readonly RefCounterPool<string, SemaphoreSlim> LockPool = new();

    private readonly string _lockKey;
    private readonly SemaphoreSlim _semaphore;

    public TinyLock(string lockKey)
    {
        _lockKey = lockKey;
        _semaphore = InitLock();
    }

    public bool TryLock()
    {
        return _semaphore.Wait(0);
    }

    public bool TryLock(int millisecondsTimeout)
    {
        return _semaphore.Wait(millisecondsTimeout);
    }

    public bool TryLock(TimeSpan timeout)
    {
        return _semaphore.Wait(timeout);
    }

    public async Task<bool> TryLockAsync()
    {
        return await _semaphore.WaitAsync(0);
    }

    public async Task<bool> TryLockAsync(int millisecondsTimeout)
    {
        return await _semaphore.WaitAsync(millisecondsTimeout);
    }

    public async Task<bool> TryLockAsync(TimeSpan timeout)
    {
        return await _semaphore.WaitAsync(timeout);
    }

    public void Lock()
    {
        _semaphore.Wait();
    }

    public Task LockAsync()
    {
        return _semaphore.WaitAsync();
    }

    public async Task<TinyLock> GetLockAsync()
    {
        await _semaphore.WaitAsync();
        return this;
    }

    public void UnLock()
    {
        InternalRelease();
    }

    public Task UnLockAsync()
    {
        InternalRelease();
        return Task.CompletedTask;
    }

    #region Dispose

    public void Dispose()
    {
        UnLock();

        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await UnLockAsync();

        GC.SuppressFinalize(this);
    }

    // ~MemoryLock() => Dispose();

    #endregion Dispose


    private SemaphoreSlim InitLock()
    {
        return LockPool.IncrementRef(_lockKey, _ => new SemaphoreSlim(1, 1));
    }


    private void InternalRelease()
    {
        _semaphore.Release();

        // 尝试销毁
        LockPool.TryRemove(_lockKey)?.Dispose();
    }
}