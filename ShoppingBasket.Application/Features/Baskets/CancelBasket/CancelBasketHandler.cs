using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingBasket.Aplication.common;

namespace ShoppingBasket.Aplication.Features.Baskets.CancelBasket;

public class CancelBasketHandler(ILogger<CancelBasketHandler> logger, IContext context) : IRequestHandler<CancelBasketCommand>
{
    public async Task Handle(CancelBasketCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Canceling basket with ID {BasketId}", request.BasketId);
        
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

        basket.Cancel();
        await context.SaveChangesAsync();

        logger.LogInformation("Basket with ID {BasketId} cancel successfully", request.BasketId);
    }
}