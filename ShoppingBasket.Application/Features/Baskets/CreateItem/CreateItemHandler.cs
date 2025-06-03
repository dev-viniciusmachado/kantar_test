using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingBasket.Aplication.common;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Enums;
using ShoppingBasket.Domain.Services.DiscountPolicy;

namespace ShoppingBasket.Aplication.Features.Baskets.CreateItem;

public class CreateItemHandler(
    ILogger<CreateItemHandler> logger,
    IContext context,
    ICacheService cacheService,
    IEnumerable<IDiscountPolicy> discountPolicies) : IRequestHandler<CreateItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var basket = await context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.Id == request.BasketId, cancellationToken);

        if (basket is null)
        {
            logger.LogInformation("Basket with ID {BasketId} not found", request.BasketId);
            throw new KeyNotFoundException($"Basket with ID {request.BasketId} not found");
        }

        if (basket.ClosedAt is not null)
        {
            logger.LogInformation("Basket with ID {BasketId} is already finished", request.BasketId);
            throw new InvalidOperationException($"Basket with ID {request.BasketId} is already finished");
        }
        
        var product = await context.Products
            .FirstOrDefaultAsync(p => p.Id.Equals(request.ProductId), cancellationToken);

        if (product is null)
        {
            logger.LogInformation("Product with ID {ProductId} not found", request.ProductId);
            throw new KeyNotFoundException($"Product with ID {request.ProductId} not found");
        }

        var discount = await context.Discounts
            .FirstOrDefaultAsync(w => w.Active && 
                                      (w.ProductConditionalId == request.ProductId) ||
                                      (w.ProductId == request.ProductId)
                                      );
        
        var itemsToAdd = new List<BasketItem>();
        for(var i = 0; i < request.Quantity; i++)
        {
            itemsToAdd.Add(basket.AddProduct(product, discountPolicies, discount));
        }

        context.BasketItems.AddRange(itemsToAdd);
        await context.SaveChangesAsync(cancellationToken);
        
        //clear cache
        await cacheService.RemoveAsync("basket", request.BasketId.ToString(), cancellationToken);
        
        return basket.Id;
    }
}