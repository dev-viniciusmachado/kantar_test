namespace ShoppingBasket.Domain.Entities;

public class BasketItem : Entity
{
    public Guid BasketId { get; set; }
    public virtual Basket Basket { get; private set; }
    
    public Guid ProductId { get; private set; }
    public virtual Product Product { get; private set; }
    
    public Guid? DiscountId { get; private set; }
    public virtual Discount? Discount { get; private set; }

    public decimal PriceWithDiscount => Discount is null ? Product.Price.Amount : 
        Product.Price.Amount - (Product.Price.Amount * Discount.Rate / 100);
    
    
    protected BasketItem(Guid id,Guid basketId, Guid productId, Guid? discountId = null) : base(id)
    {
        BasketId = basketId;
        ProductId = productId;
        DiscountId = discountId;
    }

    public void ApplyDiscount(Discount discount)
    {
        DiscountId = discount.Id;
        Discount = discount;
    }
    
    public void RemoveDiscount()
    {
        DiscountId = null;
        Discount = null;
    }

    public static BasketItem CreateItem(Basket Basket, Product Product, Discount? discount = null)
    {
        var item = new BasketItem(Guid.NewGuid(),Basket.Id, Product.Id, discount?.Id);
        
        item.Basket = Basket;
        item.Product = Product;
        item.Discount = discount;
        
        return item;
    }
}