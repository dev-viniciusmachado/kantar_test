using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Enums;
using ShoppingBasket.Domain.Services;
using ShoppingBasket.Domain.Services.DiscountPolicy;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Tests.Unit;

public class BasketUnitTests
{
    private readonly IEnumerable<IDiscountPolicy> _policies = new List<IDiscountPolicy>
    {
        new DirecltyDiscountPolicy(),
        new MultiBuyDiscountPolicy()
    };

    private readonly Product bread = Product.Create("Bread", new Price(0.65m), "bread.jpg", ProductCategory.Bakery);
    private readonly Product soup = Product.Create("Soup", new Price(0.50m), "soup.jpg", ProductCategory.PreparedFoods);


    [Fact]
    public void ShouldApply10PercentDiscountOnApples()
    {
        var apple = Product.Create("Apple", new Price(1.00m), "apple.jpg", ProductCategory.Fruit);

        var basket = Basket.Create(Guid.NewGuid());

        var discount = Discount.CreateDirecltyDiscount(
            "10% off Apples",
            apple.Id,
            10,
            true
        );

        basket.AddProduct(apple, _policies, discount);
        basket.AddProduct(apple, _policies, discount);

        Assert.Equal(1.80m, basket.TotalPriceWithDiscount);
    }

    [Fact]
    public void ShouldApplyHalfPriceBreadForEvery2Soups()
    {
        var basket = Basket.Create(Guid.NewGuid());

        var discount = Discount.CreateMultiBuyDiscount(
            "Half Price Bread for every 2 Soups",
            bread.Id,
            50,
            true,
            1,
            soup.Id,
            2
        );
        basket.AddProduct(soup, _policies, discount);
        basket.AddProduct(soup, _policies, discount);
        basket.AddProduct(bread, _policies, discount);

        Assert.Equal(1.33m, basket.TotalPriceWithDiscount);
    }

    [Fact]
    public void ShouldApplyDiscountToOneBreadWhenThreeSoupsAndTwoBreads()
    {
        var basket = Basket.Create(Guid.NewGuid());

        var discount = Discount.CreateMultiBuyDiscount(
            "Half Price Bread for every 2 Soups",
            bread.Id,
            50,
            true,
            1,
            soup.Id,
            2
        );
        basket.AddProduct(soup, _policies, discount);
        basket.AddProduct(soup, _policies, discount);
        basket.AddProduct(soup, _policies, discount);
        basket.AddProduct(bread, _policies, discount);
        basket.AddProduct(bread, _policies, discount);
        Assert.Equal(2.48m, basket.TotalPriceWithDiscount);
    }

    [Fact]
    public void ShouldApplyDiscountToTwoBreadWhenFourSoupsAddedNonSequentially()
    {
        // Arrange
        var basket = Basket.Create(Guid.NewGuid());
        var discount = Discount.CreateMultiBuyDiscount(
            "Half Price Bread for every 2 Soups",
            bread.Id,
            50,
            true,
            1,
            soup.Id,
            2
        );

        // Act
        basket.AddProduct(soup, _policies, discount);
        basket.AddProduct(soup, _policies, discount);
        basket.AddProduct(soup, _policies, discount);
        basket.AddProduct(bread, _policies, discount);
        basket.AddProduct(bread, _policies, discount);
        basket.AddProduct(soup, _policies, discount);

        // Assert
        Assert.Equal(2.65m, basket.TotalPriceWithDiscount);
    }
    
    [Fact]
    public void ShouldApplyDiscountToOneExistedBreadWhenTwoSoupsAdded()
    {
        // Arrange
        var basket = Basket.Create(Guid.NewGuid());
        var discount = Discount.CreateMultiBuyDiscount(
            "Half Price Bread for every 2 Soups",
            bread.Id,
            50,
            true,
            1,
            soup.Id,
            2
        );

        // Act
        basket.AddProduct(bread, _policies, discount);
        basket.AddProduct(bread, _policies, discount);
        basket.AddProduct(bread, _policies, discount);
        basket.AddProduct(soup, _policies, discount);
        basket.AddProduct(soup, _policies, discount);

        // Assert
        Assert.Equal(2.63m, basket.TotalPriceWithDiscount);
    }

    [Fact]
    public void ShouldRemoveItemsAndRecalculateDiscounts()
    {
        // Arrange
        var basket = Basket.Create(Guid.NewGuid());
        var discount = Discount.CreateMultiBuyDiscount(
            "Half Price Bread for every 2 Soups",
            bread.Id,
            50,
            true,
            1,
            soup.Id,
            2
        );

        basket.AddProduct(soup, _policies, discount);
        basket.AddProduct(soup, _policies, discount);
        basket.AddProduct(bread, _policies, discount);
        // Act
        var removedItems = basket.RemoveItems(soup,null, _policies, discount);

        // Assert
        Assert.Equal(2, removedItems.Count());
        Assert.Equal(0.65m, basket.TotalPriceWithDiscount);
    }
}