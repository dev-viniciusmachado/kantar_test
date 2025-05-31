using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingBasket.Aplication.common;
using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Aplication.Features.Baskets.CreateBasket;

public class CreateBasketHandler(ILogger<CreateBasketHandler> logger, IContext context)
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