using System.Collections.Concurrent;

public class InMemoryCache<TKey, TValue>
{
    private readonly ConcurrentDictionary<TKey, CacheItem<TValue>> _cache = new();
    private readonly Timer _cleanupTimer;

    public InMemoryCache(TimeSpan cleanupInterval = default)
    {
        if (cleanupInterval == default)
            cleanupInterval = TimeSpan.FromMinutes(5); // по умолчанию очистка раз в 5 минут

        _cleanupTimer = new Timer(Cleanup, null, cleanupInterval, cleanupInterval);
    }

    public void Set(TKey key, TValue value, TimeSpan ttl)
    {
        var expiryTime = DateTime.UtcNow.Add(ttl);
        var cacheItem = new CacheItem<TValue>(value, expiryTime);
        _cache[key] = cacheItem;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (_cache.TryGetValue(key, out var cacheItem))
        {
            if (DateTime.UtcNow < cacheItem.ExpiryTime)
            {
                value = cacheItem.Value;
                return true;
            }
            else
            {
                // Если элемент просрочен, удаляем его
                _cache.TryRemove(key, out _);
            }
        }

        value = default(TValue);
        return false;
    }

    public bool Remove(TKey key)
    {
        return _cache.TryRemove(key, out _);
    }

    public void Clear()
    {
        _cache.Clear();
    }

    private void Cleanup(object state)
    {
        var now = DateTime.UtcNow;
        foreach (var kvp in _cache)
        {
            if (now >= kvp.Value.ExpiryTime)
            {
                _cache.TryRemove(kvp.Key, out _);
            }
        }
    }

    public int Count => _cache.Count;

    // Вспомогательный класс для хранения значения и времени истечения
    private class CacheItem<T>
    {
        public T Value { get; }
        public DateTime ExpiryTime { get; }

        public CacheItem(T value, DateTime expiryTime)
        {
            Value = value;
            ExpiryTime = expiryTime;
        }
    }
}
