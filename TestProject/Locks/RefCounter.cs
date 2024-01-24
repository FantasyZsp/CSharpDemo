using System;
using System.Collections.Generic;
using System.Threading;

namespace TestProject.Locks;

internal class RefCounter<T>
{
    private int _refCount = 1;

    public RefCounter(T value) => Value = value;

    public T Value { get; }

    public int Increment() => Interlocked.Increment(ref _refCount);

    public int Decrement() => Interlocked.Decrement(ref _refCount);
}

public class RefCounterPool<TKey, TValue> where TValue : class
{
    private readonly IDictionary<TKey, RefCounter<TValue>> _dictionary = new Dictionary<TKey, RefCounter<TValue>>();

    public TValue IncrementRef(TKey key, Func<TKey, TValue> valueFactory)
    {
        if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));

        RefCounter<TValue> item;
        lock (_dictionary)
        {
            if (!_dictionary.TryGetValue(key, out item))
            {
                return (_dictionary[key] = new RefCounter<TValue>(valueFactory(key))).Value;
            }

            item.Increment();
        }

        return item.Value;
    }

    // 获取时不累加计数
    public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
    {
        if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));

        RefCounter<TValue> item;
        lock (_dictionary)
        {
            if (!_dictionary.TryGetValue(key, out item))
            {
                return (_dictionary[key] = new RefCounter<TValue>(valueFactory(key))).Value;
            }
        }

        return item.Value;
    }

    public TValue TryRemove(TKey key)
    {
        RefCounter<TValue> item;

        lock (_dictionary)
        {
            // 如果锁没有了，或者仍被其他人引用，就不销毁
            if (!_dictionary.TryGetValue(key, out item) || item.Decrement() > 0)
            {
                return null;
            }

            _dictionary.Remove(key);
        }

        return item.Value;
    }

    public int Count()
    {
        lock (_dictionary)
        {
            return _dictionary.Count;
        }
    }
}