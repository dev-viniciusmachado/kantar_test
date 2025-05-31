using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Domain.Services.DiscountPolicy;

public interface IDiscountPolicy
{
    Discount? ValidateDiscountApplication(Basket basket, Discount discount);
}