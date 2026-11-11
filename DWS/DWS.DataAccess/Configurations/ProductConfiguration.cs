
using DWS.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DWS.DataAccess.Configurations;

// Configures the database structure and relationships for the Product entity.
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    // Defines how the Product entity will be mapped to the database.
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Sets the database table name.
        builder.ToTable("Products");

        // Defines the primary key.
        builder.HasKey(p => p.ProductId);

        // Configures the product name as required.
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);

        // Configures the model number as required.
        builder.Property(p => p.ModelNumber)
            .IsRequired()
            .HasMaxLength(100);

        // Sets the maximum length of the optional description.
        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        // Configures the product price with 18 digits and 2 decimal places.
        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        // Sets the default value of IsActive to true.
        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);

        // Configures CreatedAt as a required datetime2 column.
        builder.Property(p => p.CreatedAt)
            .HasColumnType("datetime2")
            .IsRequired();

        // Defines the relationship between Product and Brand.
        // Each product belongs to one brand, while one brand can have many products.
        builder.HasOne(p => p.Brand)
            .WithMany()
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        // Defines the relationship between Product and Category.
        // Each product belongs to one category, while one category can have many products.
        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
