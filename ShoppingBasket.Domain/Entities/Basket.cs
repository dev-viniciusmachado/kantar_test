using ShoppingBasket.Domain.Services;
using ShoppingBasket.Domain.Services.DiscountPolicy;

namespace ShoppingBasket.Domain.Entities;

public class Basket : Entity
{
    public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();   
    private List<BasketItem> _items = new List<BasketItem>();
    public Guid? CustomerId { get; private set; }
    public Guid? GuestId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    
    public Guid OwnerId => CustomerId ?? GuestId ?? throw new InvalidOperationException("Basket must have an owner (CustomerId or GuestId).");

    public decimal TotalPrice=>                                                      
        Math.Round(Items.Sum(item => item.Product.Price.Amount), 2, MidpointRounding.AwayFromZero); 
    public decimal TotalPriceWithDiscount =>
        Math.Round(Items.Sum(item => item.PriceWithDiscount), 2, MidpointRounding.AwayFromZero);

    protected Basket(Guid id, Guid? customerId, Guid? guestId, DateTime createdAt, DateTime? closedAt) : base(id)
    {
        CustomerId = customerId;
        CreatedAt = createdAt;
        ClosedAt = closedAt;
        GuestId = guestId;
    }

    public BasketItem AddProduct(Product product, IEnumerable<IDiscountPolicy> policies, Discount? discount = null)
    {
        if (product == null) throw new ArgumentNullException(nameof(product), "Product cannot be null.");

        var item = BasketItem.CreateItem(this, product);
        _items.Add(item);

        if (discount is not null)
        {
            ApplyDiscount(policies, discount);
        }

        return item;
    }
    
    public IEnumerable<Guid> RemoveItems(Product product, IEnumerable<IDiscountPolicy> policies, Discount? discount = null)
    {
        if (product == null) throw new ArgumentNullException(nameof(product), "Product cannot be null.");

        // Remove items without discounts matching the product ID
        var itemsToRemove = Items.Where(i => i.ProductId == product.Id).Select(s => s.Id).ToList();;

        if (!itemsToRemove.Any()) return [];
        
        _items.RemoveAll(item => itemsToRemove.Contains(item.Id));
        
        // Handle discount removal and reapply policies if a discount is provided
        if (discount is not null)
        {
            foreach (var item in Items.Where(w => w.DiscountId == discount.Id))
            {
                item.RemoveDiscount();
            }
            
            ApplyDiscount(policies, discount);
        }

        return itemsToRemove;
    }

    public void Close()
    {
        ClosedAt = DateTime.UtcNow;
    }
    private void ApplyDiscount(IEnumerable<IDiscountPolicy> policies, Discount discount)
    {
        if (discount == null)
            throw new ArgumentNullException(nameof(discount));

        if (policies == null)
            throw new ArgumentNullException(nameof(policies));

        
        foreach (var item in Items.Where(w => w.ProductId == discount.ProductId && w.Discount is null))
        {
            Discount appliedDiscount = policies
                .Select(p => p.ValidateDiscountApplication(this, discount))
                .FirstOrDefault(d => d != null);

            if (appliedDiscount is not null)
            {
                item.ApplyDiscount(appliedDiscount);
            }
        }
    }

    public static Basket Create(Guid customerId)
    {
        if (customerId == Guid.Empty) throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));

        return new Basket(Guid.NewGuid(), customerId, null, DateTime.UtcNow, null);
    }

    public static Basket CreateForGuess(Guid guestId)
    {
        if (guestId == Guid.Empty) throw new ArgumentException("Guest ID cannot be empty.", nameof(guestId));
        
        return new Basket(Guid.NewGuid(), null, guestId, DateTime.UtcNow, null);
    }
}