
using DWS.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DWS.DataAccess.Configurations;

// Configures the database structure and relationships for ServiceHistory.
public class ServiceHistoryConfiguration
    : IEntityTypeConfiguration<ServiceHistory>
{
    // Defines how ServiceHistory is mapped to the database.
    public void Configure(EntityTypeBuilder<ServiceHistory> builder)
    {
        // Sets the database table name.
        builder.ToTable("ServiceHistories");

        // Defines the primary key.
        builder.HasKey(sh => sh.ServiceHistoryId);

        // Sets the maximum length of optional remarks.
        builder.Property(sh => sh.Remarks)
            .HasMaxLength(1000);

        // Configures UpdatedAt as a required datetime2 column.
        builder.Property(sh => sh.UpdatedAt)
            .HasColumnType("datetime2")
            .IsRequired();

        // Defines the relationship between ServiceHistory and ServiceRequest.
        // One service request can have multiple history records.
        builder.HasOne(sh => sh.ServiceRequest)
            .WithMany()
            .HasForeignKey(sh => sh.ServiceRequestId)
            .OnDelete(DeleteBehavior.NoAction);

        // Defines the optional relationship with the user who made the update.
        builder.HasOne(sh => sh.UpdatedBy)
            .WithMany()
            .HasForeignKey(sh => sh.UpdatedById)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
