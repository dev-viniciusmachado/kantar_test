using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Enums;

namespace ShoppingBasket.Domain.Services.DiscountPolicy;

public class DirecltyDiscountPolicy : IDiscountPolicy
{
    public Discount? ValidateDiscountApplication(Basket basket, Discount discount)
    {
        if (discount.Type is not DiscountType.DirecltyDiscount) return null;
        
        var product = basket.Items.FirstOrDefault(p => p.ProductId == discount.ProductId);
        if (product is null) return null;

        return discount; 
    }
}