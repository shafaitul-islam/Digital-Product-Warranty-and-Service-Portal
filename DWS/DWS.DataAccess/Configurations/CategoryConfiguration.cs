
using DWS.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DWS.DataAccess.Configurations;

// Configures the database structure and constraints for the Category entity.
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    // Defines how the Category entity will be mapped to the database.
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Sets the database table name.
        builder.ToTable("Categories");

        // Defines the primary key.
        builder.HasKey(c => c.CategoryId);

        // Configures the category name as required with a maximum length of 100 characters.
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Ensures that each category name is unique.
        builder.HasIndex(c => c.Name)
            .IsUnique();

        // Sets the maximum length of the optional description.
        builder.Property(c => c.Description)
            .HasMaxLength(500);

        // Sets the default value of IsActive to true.
        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        // Configures CreatedAt as a required datetime2 column.
        builder.Property(c => c.CreatedAt)
            .HasColumnType("datetime2")
            .IsRequired();
    }
}
