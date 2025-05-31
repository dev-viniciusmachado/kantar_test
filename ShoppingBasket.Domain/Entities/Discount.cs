using ShoppingBasket.Domain.Enums;

namespace ShoppingBasket.Domain.Entities;

public class Discount : Entity
{
    public string Name { get; private set; }
    public Guid ProductId { get; private set; }
    public virtual Product Product { get; private set; }
    public int? Quantity { get; private set; }
    public decimal Rate { get; private set; }
    public DiscountType Type { get; private set; }
    public Guid? ProductConditionalId { get; private set; }
    public virtual Product? ProductConditional { get; private set; }
    public int? QuantityConditional { get; private set; }
    public bool Active { get; private set; }
    

    protected Discount(Guid id, string name, Guid productId, decimal rate, bool active, DiscountType type,
        int? quantity,
        Guid? productConditionalId, int? quantityConditional) : base(id)
    {
        ProductId = productId;
        Name = name;
        Type = type;
        Rate = rate;
        ProductConditionalId = productConditionalId;
        QuantityConditional = quantityConditional;
        Active = active;
        Quantity = quantity;
    }

    public static Discount CreateDirecltyDiscount(string name, Guid productId, decimal rate, bool active)
    {
        return new Discount(Guid.NewGuid(), name, productId, rate, active, DiscountType.DirecltyDiscount, null, productId,
            null);
    }
    public static Discount CreateMultiBuyDiscount(string name, Guid productId, decimal rate, bool active, int quantity, Guid? productConditionalId, int? quantityConditional)
    {
        return new Discount(Guid.NewGuid(), name, productId, rate, active, DiscountType.MultiBuyDiscount, quantity,
            productConditionalId, quantityConditional);
    }
}