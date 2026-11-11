
using DWS.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DWS.DataAccess.Configurations;

// Configures the database structure and constraints for the Brand entity.
public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    // Defines how the Brand entity will be mapped to the database.
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        // Sets the database table name.
        builder.ToTable("Brands");

        // Defines the primary key.
        builder.HasKey(b => b.BrandId);

        // Configures the brand name as required with a maximum length of 100 characters.
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Ensures that each brand name is unique.
        builder.HasIndex(b => b.Name)
            .IsUnique();

        // Sets the maximum length of the optional description.
        builder.Property(b => b.Description)
            .HasMaxLength(500);

        // Sets the default value of IsActive to true.
        builder.Property(b => b.IsActive)
            .HasDefaultValue(true);

        // Configures CreatedAt as a required datetime2 column.
        builder.Property(b => b.CreatedAt)
            .HasColumnType("datetime2")
            .IsRequired();
    }
}
