using Microsoft.EntityFrameworkCore;
using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Aplication.common;

public interface IContext : IAsyncDisposable, IDisposable
{
    public DbSet<Product> Products { get; }
    public DbSet<Basket> Baskets { get; }
    public DbSet<BasketItem> BasketItems { get; }
    public DbSet<Discount> Discounts { get; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}