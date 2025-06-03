using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Enums;

namespace ShoppingBasket.Domain.Services.DiscountPolicy;

public class MultiBuyDiscountPolicy : IDiscountPolicy
{
    public Discount? ValidateDiscountApplication(Basket basket, Discount discount)
    {
        if (discount.Type != DiscountType.MultiBuyDiscount) return null;

        var productQuantity = basket.Items.Count(p => p.ProductId == discount.ProductId && p.DiscountId != null);

        var conditionalProductQuantity = discount.ProductConditionalId.HasValue
            ? basket.Items.Count(p => p.ProductId == discount.ProductConditionalId.Value)
            : 0;
        
        if(conditionalProductQuantity is 0) return null;
        
        var numberOfDiscounts = conditionalProductQuantity / discount.QuantityConditional;

        // Check if the product quantity meets the discount requirements
        if (productQuantity >= numberOfDiscounts || conditionalProductQuantity < discount.QuantityConditional)
            return null;
        
        return discount;
    }
}