namespace ShoppingBasket.Aplication.common;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key,  CancellationToken token = default) where T : class;
    Task<T?> GetAsync<T>(string key, string id, CancellationToken token = default) where T : class;
    Task<T> GetAsync<T>(string key, Func<Task<T>> factory, TimeExpiration timeExpiration = TimeExpiration.None, CancellationToken token = default) where T : class;
    Task<T> GetAsync<T>(string key, string id, Func<Task<T>> factory, TimeExpiration timeExpiration = TimeExpiration.None, CancellationToken token = default) where T : class;

    Task SetAsync<T>(string key, T value, TimeExpiration timeExpiration = TimeExpiration.None, CancellationToken token = default) where T : class;
    Task SetAsync<T>(string key, string id, T value, TimeExpiration timeExpiration = TimeExpiration.None, CancellationToken token = default) where T : class;
    Task RemoveAsync(string key, CancellationToken token = default);
    Task RemoveAsync(string key, string id, CancellationToken token = default);
}