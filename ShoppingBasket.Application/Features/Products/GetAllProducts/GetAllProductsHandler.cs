using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingBasket.Aplication.common;
using ShoppingBasket.Aplication.Features.Discounts;
using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Aplication.Features.Products.GetAllProducts;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsRequest, IEnumerable<ProductResponse>>
{
    private readonly ILogger<GetAllProductsHandler> _logger;
    private readonly IContext _context;
    private readonly ICacheService _cacheService;

    public GetAllProductsHandler(ILogger<GetAllProductsHandler> logger, IContext context, ICacheService cacheService)
    {
        _logger = logger;
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<ProductResponse>> Handle(GetAllProductsRequest request,
        CancellationToken cancellationToken)
    {
        return await _cacheService.GetAsync<IEnumerable<ProductResponse>>("products", async () =>
        {
            _logger.LogInformation("Fetching all products from the database");
            return await _context.Products
                .Include(i => i.Discounts)
                .Select(p =>
                    new ProductResponse(
                        p.Id,
                        p.Name,
                        p.Price.Amount,
                        p.ImagePath,
                        p.Discounts.Select(d => new DiscountResponse(d.Id, d.Name, $"{(int)d.Rate}%")
                        )))
                .ToListAsync(cancellationToken);
        }, TimeExpiration.FifteenMinutes, cancellationToken);
    }
}