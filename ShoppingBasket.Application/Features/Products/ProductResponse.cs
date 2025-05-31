using ShoppingBasket.Aplication.Features.Discounts;
using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Aplication.Features.Products;

public record ProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    string imagePath,
    IEnumerable<DiscountResponse> discount = null)
{
    public static ProductResponse MapToResponse(Product entity)
    {
        return new ProductResponse(
            entity.Id,
            entity.Name,
            entity.Price.Amount,
            entity.ImagePath,
            entity.Discounts?.Select(DiscountResponse.MapToResponse) ?? Enumerable.Empty<DiscountResponse>());
    }
}