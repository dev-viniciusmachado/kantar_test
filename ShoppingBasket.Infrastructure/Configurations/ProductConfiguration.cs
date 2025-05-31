using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).IsRequired()
            .HasColumnType("varchar(40)")
            .HasMaxLength(40);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);
        
        builder.Property(x => x.ImagePath)
            .IsRequired()
            .HasColumnType("varchar(255)")
            .HasMaxLength(255);
        
        builder.OwnsOne(c => c.Price, price =>
        {
            price.Property(e => e.Amount).HasColumnName("Price")
                .HasColumnType("decimal(18,2)").IsRequired();
        });
    }
}