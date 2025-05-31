using ShoppingBasket.Domain.Enums;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Domain.Entities;

public class Product : Entity
{
    public string Name { get; private set; }
    public Price Price { get; private set; }
    public string ImagePath { get; private set; } 
    public ProductCategory Category { get; private set; }
    
    public virtual List<Discount> Discounts { get; private set; }
    
    private Product(Guid id) : base(id) { }
    protected Product(Guid id, string name, Price price, string imagePath, ProductCategory category) : base(id)
    {
        Name = name;
        Price = price;
        ImagePath = imagePath;
        Category = category;
    }
    
    public static Product Create(string name, Price price, string imagePath, ProductCategory category)
    {
        return new Product(Guid.NewGuid(), name, price, imagePath, category);
    }
}