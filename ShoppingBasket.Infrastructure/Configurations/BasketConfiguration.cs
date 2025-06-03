using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Infrastructure.Configurations;

public class BasketConfiguration : IEntityTypeConfiguration<Basket> 
{
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired()
            .HasColumnType("varchar(40)")
            .HasMaxLength(40);
        
        builder.Property(x => x.CustomerId)
            .HasColumnType("varchar(40)")
            .HasMaxLength(40);
        
        builder.Property(x => x.GuestId)
            .HasColumnType("varchar(40)")
            .HasMaxLength(40);
        
        builder.Property(x => x.CreatedAt)
            .HasColumnType("DATETIME")
            .IsRequired();
        
        builder.Property(x => x.ClosedAt)
            .HasColumnType("DATETIME")
            .IsRequired(false);
        
        builder.Property(x => x.CanceledAt)
            .HasColumnType("DATETIME")
            .IsRequired(false);
        
    }
}