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
            .Include(i => i.Items)
            .ThenInclude(p => p.Product)
            .Include(i => i.Items)
            .ThenInclude(d => d.Discount)
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

        var page = request.Page < 1 ? 1 : request.Page;
        var baskets = await query
            .Skip((page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var response = baskets.Select(b => BasketResponse.MapToResponse(b)).ToList();
        return new PagedBasketResponse
        {
            Baskets = response,
            TotalCount = totalCount,
            PageNumber = request.Page-1,
            PageSize = request.PageSize
        };
    }
}
