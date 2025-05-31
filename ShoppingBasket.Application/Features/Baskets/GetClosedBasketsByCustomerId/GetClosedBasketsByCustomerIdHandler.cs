using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingBasket.Aplication.common;

namespace ShoppingBasket.Aplication.Features.Baskets.GetClosedBasketsByCostumerId;

public class GetClosedBasketsByCustomerIdHandler(ILogger<GetClosedBasketsByCustomerIdHandler> logger, IContext context) : IRequestHandler<GetClosedBasketsByCustomerIdRequest, PagedBasketResponse>
{
    public async Task<PagedBasketResponse> Handle(GetClosedBasketsByCustomerIdRequest request, CancellationToken cancellationToken)
    {

        var query = context.Baskets
            .Where(b => b.CustomerId == request.CustomerId && b.ClosedAt != null);

        var totalCount = query.Count();

        query = request.OrderBy switch
        {
            "ClosedAt" => request.IsDescending
                ? query.OrderByDescending(o => o.ClosedAt)
                : query.OrderBy(o => o.ClosedAt),
            "TotalPrice" => request.IsDescending
                ? query.OrderByDescending(o => o.TotalPrice)
                : query.OrderBy(o => o.TotalPrice),
            "TotalPriceWithDiscount" => request.IsDescending
                ? query.OrderByDescending(o => o.TotalPriceWithDiscount)
                : query.OrderBy(o => o.TotalPriceWithDiscount),
            _ => query
        };

        var baskets = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(b => BasketResponse.MapToResponse(b))
            .ToListAsync();

        return new PagedBasketResponse
        {
            Baskets = baskets,
            TotalCount = totalCount,
            PageNumber = request.Page,
            PageSize = request.PageSize
        };
    }
}
