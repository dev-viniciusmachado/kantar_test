using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Infrastructure.Configurations;

public class DiscountConfiguration : IEntityTypeConfiguration<Discount> 
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired()
            .HasColumnType("varchar(40)")
            .HasMaxLength(40);
        builder.Property(x => x.Name).IsRequired()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);
        builder.Property(x => x.ProductId).IsRequired()
            .HasColumnType("varchar(40)")
            .HasMaxLength(40);
        builder.Property(x => x.ProductConditionalId)
            .HasColumnType("varchar(40)")
            .HasMaxLength(40);
        
        builder.Property(x => x.Active).HasDefaultValue(false)
            .HasColumnType("bit");
        
        builder.Property(x => x.Rate).IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.Product)
            .WithMany(p => p.Discounts);
    }
}