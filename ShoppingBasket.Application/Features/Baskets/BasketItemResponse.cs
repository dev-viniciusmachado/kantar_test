using ShoppingBasket.Aplication.Features.Discounts;
using ShoppingBasket.Aplication.Features.Products;
using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Aplication.Features.Baskets;

public record BasketItemResponse(
    ProductResponse Product,
    decimal TotalPrice,
    decimal TotalPriceWithDiscount,
    int Quantity,
    DiscountResponse? DiscountApplayed)
{
    public static IEnumerable<BasketItemResponse> MapToResponse(IEnumerable<BasketItem> entities)
    {
        return entities.GroupBy(g => new { g.Product, g.Discount })
            .Select(s =>
                new BasketItemResponse(ProductResponse.MapToResponse(s.Key.Product),
                    s.Sum(s => s.Product.Price.Amount),
                    s.Sum(s => s.PriceWithDiscount),
                    s.Count(),
                    s.Key.Discount is not null ? DiscountResponse.MapToResponse(s.Key.Discount) : null
                )
            )
            .ToList();
    }
}