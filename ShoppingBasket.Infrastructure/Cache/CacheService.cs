using Microsoft.Extensions.Caching.Memory;
using ShoppingBasket.Aplication.common;

namespace ShoppingBasket.Infrastructure.Cache;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    
    public CacheService(IMemoryCache memoryCache)
    {
        _cache = memoryCache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken token = default) where T : class
    {
        var cacheValue = _cache.Get<T>(key);

        if (cacheValue is null) return null;

        return cacheValue;
    }

    public async Task<T?> GetAsync<T>(string key, string id, CancellationToken token = default) where T : class
    {
        var cacheValue = _cache.Get<T>($"{key}_{id}");

        if (cacheValue is null) return null;

        return cacheValue;
    }

    public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, TimeExpiration timeExpiration = TimeExpiration.None, CancellationToken token = default)
        where T : class
    {
        T? cachedValue = await GetAsync<T>(key, token);

        if (cachedValue is not null)
            return cachedValue;

        cachedValue = await factory();

        await SetAsync<T>(key, cachedValue, timeExpiration, token);
        return cachedValue;
    }

    public async Task<T> GetAsync<T>(string key, string id, Func<Task<T>> factory, TimeExpiration timeExpiration = TimeExpiration.None, CancellationToken token = default)
        where T : class => await GetAsync($"{key}_{id}", factory,timeExpiration, token);

    public Task SetAsync<T>(string key, T value, TimeExpiration timeExpiration = TimeExpiration.None, CancellationToken token = default) where T : class
    {
        _cache.Set<T>(key, value, GetCacheEntryOptions(timeExpiration));
        return Task.CompletedTask;
    }

    public Task SetAsync<T>(string key, string id, T value, TimeExpiration timeExpiration = TimeExpiration.None, CancellationToken token = default) where T : class
    {
        _cache.Set<T>($"{key}_{id}", value, GetCacheEntryOptions(timeExpiration));
        return Task.CompletedTask;
    } 

    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, string id, CancellationToken token = default)
    {
        _cache.Remove($"{key}_{id}"); 
        return Task.CompletedTask;
    } 
    
    private MemoryCacheEntryOptions GetCacheEntryOptions(TimeExpiration timeExpiration)
    {
        return timeExpiration switch
        {
            TimeExpiration.None => new MemoryCacheEntryOptions(),
            TimeExpiration.FiveMinutes => new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            },
            TimeExpiration.FifteenMinutes => new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            },
            TimeExpiration.ThirtyMinutes => new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            },
            TimeExpiration.OneHour => new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            },
            _ => new MemoryCacheEntryOptions()
        };
    }
}