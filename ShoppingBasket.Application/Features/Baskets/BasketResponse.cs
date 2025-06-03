using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Aplication.Features.Baskets;

public record BasketResponse(
    Guid Id,
    Guid OwnerId,
    decimal TotalPrice,
    decimal TotalPriceWithDiscount,
    DateTime CreatedAt,
    DateTime? ClosedAt,
    IEnumerable<BasketItemResponse> Items)
{
    public static BasketResponse MapToResponse(Basket entity)
    {
        return new BasketResponse(
            entity.Id,
            entity.OwnerId,
            entity.TotalPrice,
            entity.TotalPriceWithDiscount,
            entity.CreatedAt,
            entity.ClosedAt,
            BasketItemResponse.MapToResponse(entity.Items.OrderBy(o => o.Product.Name)));
    }
}
