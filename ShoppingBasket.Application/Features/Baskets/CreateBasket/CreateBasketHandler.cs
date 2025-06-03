using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingBasket.Aplication.common;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Services.DiscountPolicy;

namespace ShoppingBasket.Aplication.Features.Baskets.CreateBasket;

public class CreateBasketHandler(ILogger<CreateBasketHandler> logger, IContext context,
    IEnumerable<IDiscountPolicy> discountPolicies)
    : IRequestHandler<CreateBasketCommand, Guid>
{
    public async Task<Guid> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var basket = request.CustomerId.HasValue
                ? Basket.Create(request.CustomerId.Value)
                : Basket.CreateForGuess(request.GuestId.Value);

            context.Baskets.Add(basket);
            
            var product = await context.Products
                .FirstOrDefaultAsync(p => p.Id.Equals(request.ProductId), cancellationToken);

            if (product is null)
            {
                logger.LogInformation("Product with ID {ProductId} not found", request.ProductId);
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found");
            }

            var discount = await context.Discounts
                .FirstOrDefaultAsync(w => w.Active && w.ProductConditionalId.Equals(request.ProductId));
        
            var itemsToAdd = new List<BasketItem>();
            for(var i = 0; i < request.Quantity; i++)
            {
                itemsToAdd.Add(basket.AddProduct(product, discountPolicies, discount));
            }

            context.BasketItems.AddRange(itemsToAdd);
            await context.SaveChangesAsync(cancellationToken);
            
            return basket.Id;
        }
        catch (Exception e)
        {
            logger.LogError("An error occurred while creating a basket: {Message}", e.Message);
            throw;
        }
    }
}