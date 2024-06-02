using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) 
    : IBasketRepository
{
    private static string GetCacheKey(string userName) => $"{nameof(ShoppingCart)}:{userName}";

    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetCacheKey(userName);
        var cachedBasket = await cache.GetStringAsync(cacheKey, cancellationToken);
        
        if (!string.IsNullOrEmpty(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        }
        
        var basket = await repository.GetBasket(userName, cancellationToken);
        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket, cancellationToken);
        
        var cacheKey = GetCacheKey(basket.UserName);
        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(userName, cancellationToken);
        var cacheKey = GetCacheKey(userName);
        await cache.RemoveAsync(cacheKey, cancellationToken);

        return true;
    }
}