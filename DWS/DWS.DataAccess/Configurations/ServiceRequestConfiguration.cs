
using DWS.Entities.Enums;
using DWS.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DWS.DataAccess.Configurations;

// Configures the database structure and relationships for ServiceRequest.
public class ServiceRequestConfiguration
    : IEntityTypeConfiguration<ServiceRequest>
{
    // Defines how ServiceRequest is mapped to the database.
    public void Configure(EntityTypeBuilder<ServiceRequest> builder)
    {
        // Sets the database table name.
        builder.ToTable("ServiceRequests");

        // Defines the primary key.
        builder.HasKey(sr => sr.ServiceRequestId);

        // Configures the issue title as required.
        builder.Property(sr => sr.IssueTitle)
            .IsRequired()
            .HasMaxLength(150);

        // Configures the issue description as required.
        builder.Property(sr => sr.IssueDescription)
            .IsRequired()
            .HasMaxLength(2000);

        // Configures the request date as a required datetime2 column.
        builder.Property(sr => sr.RequestDate)
            .HasColumnType("datetime2")
            .IsRequired();

        // Sets Pending as the default service request status.
        // Uses 0 as the sentinel value to distinguish an unset status.
        builder.Property(sr => sr.Status)
            .HasDefaultValue(ServiceRequestStatus.Pending)
            .HasSentinel((ServiceRequestStatus)0)
            .IsRequired();

        // Sets the maximum length of customer notes.
        builder.Property(sr => sr.CustomerNotes)
            .HasMaxLength(1000);

        // Configures the assignment date as an optional datetime2 column.
        builder.Property(sr => sr.AssignedAt)
            .HasColumnType("datetime2");

        // Configures the completion date as an optional datetime2 column.
        builder.Property(sr => sr.CompletedAt)
            .HasColumnType("datetime2");

        // Sets the maximum length of service staff notes.
        builder.Property(sr => sr.StaffNotes)
            .HasMaxLength(2000);

        // Sets the maximum length of admin remarks.
        builder.Property(sr => sr.AdminRemarks)
            .HasMaxLength(1000);

        // Defines the relationship between ServiceRequest and ProductRegistration.
        // Each service request belongs to one product registration.
        builder.HasOne(sr => sr.ProductRegistration)
            .WithMany()
            .HasForeignKey(sr => sr.ProductRegistrationId)
            .OnDelete(DeleteBehavior.NoAction);

        // Defines the optional relationship with the assigned service staff member.
        builder.HasOne(sr => sr.AssignedStaff)
            .WithMany()
            .HasForeignKey(sr => sr.AssignedStaffId)
            .OnDelete(DeleteBehavior.NoAction);

        // Defines the optional relationship with the admin who assigned the request.
        builder.HasOne(sr => sr.AssignedBy)
            .WithMany()
            .HasForeignKey(sr => sr.AssignedById)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
