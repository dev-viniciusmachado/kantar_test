using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Aplication.Features.Discounts;

public record DiscountResponse(Guid Id, string Name, string Rate)
{
    public static DiscountResponse? MapToResponse(Discount entity)
    {
        return new DiscountResponse(entity.Id, entity.Name, $"{(int)entity.Rate}%");
    }
}