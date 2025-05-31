using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingBasket.Aplication.common;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Enums;
using ShoppingBasket.Domain.ValueObjects;
using ShoppingBasket.Infrastructure.Configurations;

namespace ShoppingBasket.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IContext
{
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<Basket> Baskets { get; set; } = null!;
    public virtual DbSet<BasketItem> BasketItems { get; set; } = null!;
    public virtual DbSet<Discount> Discounts { get; set; } = null!;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(BasketConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(BasketItemConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(DiscountConfiguration).Assembly);
    }
}