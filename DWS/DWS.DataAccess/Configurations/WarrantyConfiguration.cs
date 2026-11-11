
using DWS.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DWS.DataAccess.Configurations;

// Configures the database structure and relationships for the Warranty entity.
public class WarrantyConfiguration : IEntityTypeConfiguration<Warranty>
{
    // Defines how the Warranty entity will be mapped to the database.
    public void Configure(EntityTypeBuilder<Warranty> builder)
    {
        // Sets the database table name.
        builder.ToTable("Warranties");

        // Defines the primary key.
        builder.HasKey(w => w.WarrantyId);

        // Configures the start date as a required SQL Server date column.
        builder.Property(w => w.StartDate)
            .HasColumnType("date")
            .IsRequired();

        // Configures the end date as a required SQL Server date column.
        builder.Property(w => w.EndDate)
            .HasColumnType("date")
            .IsRequired();

        // Configures the warranty duration as a required integer.
        builder.Property(w => w.DurationInMonths)
            .IsRequired();

        // Configures the QR code token as required with a maximum length of 32 characters.
        builder.Property(w => w.QrCodeToken)
            .IsRequired()
            .HasMaxLength(32);

        // Ensures that each warranty has a unique QR code token.
        builder.HasIndex(w => w.QrCodeToken)
            .IsUnique();

        // Sets the default value of IsActive to true.
        builder.Property(w => w.IsActive)
            .HasDefaultValue(true);

        // Configures CreatedAt as a required datetime2 column.
        builder.Property(w => w.CreatedAt)
            .HasColumnType("datetime2")
            .IsRequired();

        // Defines a one-to-one relationship between Warranty and ProductRegistration.
        // Each warranty belongs to exactly one product registration.
        builder.HasOne(w => w.ProductRegistration)
            .WithOne()
            .HasForeignKey<Warranty>(w => w.ProductRegistrationId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
