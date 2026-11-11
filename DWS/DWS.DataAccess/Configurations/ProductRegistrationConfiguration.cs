
using DWS.Entities.Enums;
using DWS.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DWS.DataAccess.Configurations;

// Configures the database structure and relationships for ProductRegistration.
public class ProductRegistrationConfiguration
    : IEntityTypeConfiguration<ProductRegistration>
{
    // Defines how ProductRegistration is mapped to the database.
    public void Configure(EntityTypeBuilder<ProductRegistration> builder)
    {
        // Sets the database table name.
        builder.ToTable("ProductRegistrations");

        // Defines the primary key.
        builder.HasKey(pr => pr.ProductRegistrationId);

        // Configures the serial number as required.
        builder.Property(pr => pr.SerialNumber)
            .IsRequired()
            .HasMaxLength(100);

        // Prevents duplicate serial numbers for the same product.
        builder.HasIndex(pr => new { pr.ProductId, pr.SerialNumber })
            .IsUnique();

        // Configures the purchase date as a SQL Server date column.
        builder.Property(pr => pr.PurchaseDate)
            .HasColumnType("date")
            .IsRequired();

        // Configures the registration date as a required datetime2 column.
        builder.Property(pr => pr.RegistrationDate)
            .HasColumnType("datetime2")
            .IsRequired();

        // Sets Pending as the default registration status.
        // Uses 0 as the sentinel value to distinguish an unset status.
        builder.Property(pr => pr.Status)
            .HasDefaultValue(RegistrationStatus.Pending)
            .HasSentinel((RegistrationStatus)0)
            .IsRequired();

        // Sets the maximum length of the optional invoice image path.
        builder.Property(pr => pr.InvoiceImagePath)
            .HasMaxLength(500);

        // Configures the review date as an optional datetime2 column.
        builder.Property(pr => pr.ReviewedAt)
            .HasColumnType("datetime2");

        // Sets the maximum length of the admin remarks.
        builder.Property(pr => pr.AdminRemarks)
            .HasMaxLength(1000);

        // Defines the relationship between ProductRegistration and Product.
        // Each registration belongs to one product.
        builder.HasOne(pr => pr.Product)
            .WithMany()
            .HasForeignKey(pr => pr.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        // Defines the relationship between ProductRegistration and Customer.
        // Each registration belongs to one customer.
        builder.HasOne(pr => pr.Customer)
            .WithMany()
            .HasForeignKey(pr => pr.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

        // Defines the optional relationship between ProductRegistration and reviewing Admin.
        builder.HasOne(pr => pr.ReviewedBy)
            .WithMany()
            .HasForeignKey(pr => pr.ReviewedById)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
