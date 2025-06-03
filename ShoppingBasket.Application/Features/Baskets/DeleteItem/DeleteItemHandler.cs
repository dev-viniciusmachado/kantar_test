using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingBasket.Aplication.common;
using ShoppingBasket.Domain.Services.DiscountPolicy;

namespace ShoppingBasket.Aplication.Features.Baskets.DeleteItem;

public class DeleteItemHandler(
    ILogger<DeleteItemHandler> logger,
    IContext context,
    ICacheService cacheService,
    IEnumerable<IDiscountPolicy> discountPolicies) : IRequestHandler<DeleteItemCommand, Guid>
{
    public async Task<Guid> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
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

        var itemsToRemove = basket.RemoveItems(product, request.DiscountId, discountPolicies, discount);

        if (!itemsToRemove.Any())
        {
            logger.LogInformation("No items found for product ID {ProductId} in basket ID {BasketId}",
                request.ProductId, request.BasketId);
            throw new KeyNotFoundException(
                $"No items found for product ID {request.ProductId} in basket ID {request.BasketId}");
        }

        await context.BasketItems
            .Where(bi => bi.BasketId == basket.Id && itemsToRemove.Contains(bi.Id))
            .ExecuteDeleteAsync(cancellationToken);

        //clear cache
        await cacheService.RemoveAsync("basket", request.BasketId.ToString(), cancellationToken);

        return basket.Id;
    }
}