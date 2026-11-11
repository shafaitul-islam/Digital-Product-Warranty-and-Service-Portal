
using DWS.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DWS.DataAccess.Configurations;

// Configures the database structure and relationships for Notification.
public class NotificationConfiguration
    : IEntityTypeConfiguration<Notification>
{
    // Defines how Notification is mapped to the database.
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        // Sets the database table name.
        builder.ToTable("Notifications");

        // Defines the primary key.
        builder.HasKey(n => n.NotificationId);

        // Configures the notification title as required.
        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(150);

        // Configures the notification message as required.
        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(2000);

        // Sets the default value of IsRead to false.
        builder.Property(n => n.IsRead)
            .HasDefaultValue(false);

        // Configures CreatedAt as a required datetime2 column.
        builder.Property(n => n.CreatedAt)
            .HasColumnType("datetime2")
            .IsRequired();

        // Defines the relationship between Notification and ApplicationUser.
        // Each notification belongs to one user.
        builder.HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // Creates an index to improve user notification queries.
        builder.HasIndex(n => new { n.UserId, n.IsRead });
    }
}
