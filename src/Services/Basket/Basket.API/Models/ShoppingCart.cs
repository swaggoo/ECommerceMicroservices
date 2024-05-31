using Marten.Schema;

namespace Basket.API.Models;

public class ShoppingCart
{
    [Identity]
    public string UserName { get; set; } = default!;
    public ShoppingCartItem[] Items { get; set; } = default!;
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
    
    public ShoppingCart(string userName)
    {
        UserName = userName;
    }
    
    // Required for mapping
    public ShoppingCart()
    {
    }
}