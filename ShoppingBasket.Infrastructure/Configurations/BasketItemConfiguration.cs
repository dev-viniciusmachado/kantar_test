using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Infrastructure.Configurations;

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem> 
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).IsRequired()
            .HasColumnType("varchar(40)")
            .HasMaxLength(40);
        
        builder.Property(x => x.ProductId).IsRequired()
            .HasColumnType("varchar(40)")
            .HasMaxLength(40);
        
        builder.Property(x => x.DiscountId)
            .HasColumnType("varchar(40)")
            .HasMaxLength(40);
        
        builder.Property(x => x.BasketId).IsRequired()
            .HasColumnType("varchar(40)")
            .HasMaxLength(40);
    }
}