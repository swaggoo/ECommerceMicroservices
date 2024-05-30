using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        await using var session = store.LightweightSession();
        
        if (await session.Query<Product>().AnyAsync(cancellation))
        { 
            return;
        }
        
        session.Store(GetPreconfiguredProducts());
        await session.SaveChangesAsync(cancellation);
    }
    
    private static IEnumerable<Product> GetPreconfiguredProducts()
    {
        return new List<Product>()
        {
            new()
            {
                Name = "Keyboard", Description = "Ergonomic keyboard", Price = 20,
                Category = new List<string> { "Electronics" }
            },
            new()
            {
                Name = "Mouse", Description = "Wireless mouse", Price = 10,
                Category = new List<string> { "Electronics" }
            },
            new()
            {
                Name = "Monitor", Description = "4k monitor", Price = 200,
                Category = new List<string> { "Electronics" }
            },
            new()
            {
                Name = "Desk", Description = "Standing desk", Price = 100,
                Category = new List<string> { "Furniture" }
            },
            new()
            {
                Name = "Chair", Description = "Ergonomic chair", Price = 50,
                Category = new List<string> { "Furniture" }
            },
            new()
            {
                Name = "Lamp", Description = "Desk lamp", Price = 5,
                Category = new List<string> { "Furniture" }
            },
            new()
            {
                Name = "Pen", Description = "Gel pen", Price = 1,
                Category = new List<string> { "Stationery" }
            },
            new()
            {
                Name = "Notebook", Description = "A5 notebook", Price = 3,
                Category = new List<string> { "Stationery" }
            }
        };
    }
        
}