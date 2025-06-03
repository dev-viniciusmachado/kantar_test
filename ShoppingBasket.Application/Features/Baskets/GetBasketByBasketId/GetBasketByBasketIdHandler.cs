using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingBasket.Aplication.common;

namespace ShoppingBasket.Aplication.Features.Baskets.GetBasketByBasketId;

public class GetBasketByBasketIdHandler(
    ILogger<GetBasketByBasketIdHandler> logger,
    IContext context,
    ICacheService cacheService)
    : IRequestHandler<GetBasketByBasketIdRequest, BasketResponse>
{
    public async Task<BasketResponse> Handle(GetBasketByBasketIdRequest request, CancellationToken cancellationToken)
    {
        return await cacheService.GetAsync("basket", request.BasketId.ToString(), async () =>
        {
            logger.LogInformation("Fetching basket {basketId} from the database", request.BasketId);
            var basket = await context.Baskets
                .Where(w => w.Id == request.BasketId)
                .Include(b => b.Items)
                .ThenInclude(item => item.Product)
                .Include(b => b.Items)
                .ThenInclude(item => item.Discount)
                .Select(basket => BasketResponse.MapToResponse(basket))
                .FirstOrDefaultAsync(cancellationToken);
            
            if (basket is null)
            {
                logger.LogWarning("Basket with ID {BasketId} not found", request.BasketId);
                throw new KeyNotFoundException($"Basket with ID {request.BasketId} not found");
            }

            return basket;
        }, TimeExpiration.OneHour, cancellationToken);
    }
}