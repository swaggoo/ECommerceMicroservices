namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository repository) 
    : IBasketRepository
{
    public Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        return repository.GetBasket(userName, cancellationToken);
    }

    public Task<ShoppingCart> StoreBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
    {
        return repository.StoreBasket(cart, cancellationToken);
    }

    public Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        return repository.DeleteBasket(userName, cancellationToken);
    }
}