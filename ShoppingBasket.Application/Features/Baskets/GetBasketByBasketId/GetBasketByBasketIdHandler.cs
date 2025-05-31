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
            return await context.Baskets
                .Where(w => w.Id == request.BasketId)
                .Include(b => b.Items)
                .ThenInclude(item => item.Product)
                .Include(b => b.Items)
                .ThenInclude(item => item.Discount)
                .Select(basket => BasketResponse.MapToResponse(basket))
                .FirstAsync(cancellationToken);
        }, TimeExpiration.OneHour, cancellationToken);
    }
}