﻿using Catalog.API.Exceptions;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, List<string> Category, string ImageFile) 
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsUpdated);

public class UpdateProductHandler(IDocumentSession session, ILogger<UpdateProductHandler> logger) 
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateProductHandler.Handle called with {@Command}", command);
        
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        
        if (product is null)
        {
            throw new ProductNotFoundException();
        }
        
        product.Name = command.Name;
        product.Description = command.Description;
        product.Price = command.Price;
        product.Category = command.Category;
        product.ImageFile = command.ImageFile;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}