namespace ShoppingBasket.Aplication.Features.Baskets;

public record PagedBasketResponse()
{
    public int TotalCount { get; init; }
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public IEnumerable<BasketResponse> Baskets { get; init; } = new List<BasketResponse>();
}