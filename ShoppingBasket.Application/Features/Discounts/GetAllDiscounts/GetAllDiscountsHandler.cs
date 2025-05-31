using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingBasket.Aplication.common;

namespace ShoppingBasket.Aplication.Features.Discounts;

public class GetAllDiscountsHandler : IRequestHandler<GetAllDiscountsRequest, IEnumerable<DiscountResponse>>
{
    private readonly ILogger<GetAllDiscountsHandler> _logger;
    private readonly IContext _context;
    private readonly ICacheService _cacheService;
    public GetAllDiscountsHandler(ILogger<GetAllDiscountsHandler> logger, IContext context, ICacheService cacheService)
    {
        _logger = logger;
        _context = context;
        _cacheService = cacheService;
    }
    
    public async Task<IEnumerable<DiscountResponse>> Handle(GetAllDiscountsRequest request, CancellationToken cancellationToken)
    {
        return await _cacheService.GetAsync<IEnumerable<DiscountResponse>>("discounts", async () =>
        {
            _logger.LogInformation("Fetching all discounts from the database");
            return await _context.Discounts
                .Select(p => new DiscountResponse(p.Id, p.Name, $"{(int)p.Rate}%"))
                .ToListAsync(cancellationToken);
        }, TimeExpiration.FifteenMinutes, cancellationToken);
    }
}