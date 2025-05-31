using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingBasket.Aplication.common;

namespace ShoppingBasket.Aplication.Features.Baskets.CloseBasket;

public class CloseBasketHandler(ILogger<CloseBasketHandler> logger, IContext context) : IRequestHandler<CloseBasketCommand>
{
    public async Task Handle(CloseBasketCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Closing basket with ID {BasketId}", request.BasketId);
        
        var basket = context.Baskets.Find(request.BasketId);
        if (basket is null)
        {
            logger.LogWarning("Basket with ID {BasketId} not found", request.BasketId);
            throw new KeyNotFoundException($"Basket with ID {request.BasketId} not found");
        }
        
        if (basket.ClosedAt is not null)
        {
            logger.LogInformation("Basket with ID {BasketId} is already finished", request.BasketId);
            throw new InvalidOperationException($"Basket with ID {request.BasketId} is already finished");
        }

        basket.Close();
        await context.SaveChangesAsync();

        logger.LogInformation("Basket with ID {BasketId} closed successfully", request.BasketId);
    }
}