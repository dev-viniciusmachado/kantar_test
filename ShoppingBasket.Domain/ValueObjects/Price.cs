namespace ShoppingBasket.Domain.ValueObjects;

public record Price
{
    public Price(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Price cannot be negative.");
        }
        
        Amount = amount;
    }

    public decimal Amount { get; init; }
}