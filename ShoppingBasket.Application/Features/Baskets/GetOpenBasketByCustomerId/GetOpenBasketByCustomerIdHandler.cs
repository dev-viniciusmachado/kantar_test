using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingBasket.Aplication.common;

namespace ShoppingBasket.Aplication.Features.Baskets.GetOpenBasketByCustomerId;

public class GetOpenBasketByCustomerIdHandler(ILogger<GetOpenBasketByCustomerIdHandler> logger, IContext context) : 
    IRequestHandler<GetOpenBasketByCustomerIdRequest, BasketResponse?>
{
    public async Task<BasketResponse?> Handle(GetOpenBasketByCustomerIdRequest request, CancellationToken cancellationToken)
    {
        return await context.Baskets
            .Where(w => w.CustomerId == request.CustomerId && w.ClosedAt == null)
            .Include(b => b.Items)
            .ThenInclude(item => item.Product)
            .Include(b => b.Items)
            .ThenInclude(item => item.Discount)
            .Select(basket => BasketResponse.MapToResponse(basket))
            .FirstOrDefaultAsync(cancellationToken);
    }
}