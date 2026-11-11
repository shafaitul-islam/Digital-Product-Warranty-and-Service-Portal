
using System;

namespace DWS.Entities.Models;

// Represents a notification sent to an application user.
public class Notification
{
    // Primary key of the Notification table.
    public int NotificationId { get; set; }

    // Foreign key referencing the user who receives the notification.
    public int UserId { get; set; }

    // Stores the notification title.
    public string Title { get; set; } = string.Empty;

    // Stores the notification message.
    public string Message { get; set; } = string.Empty;

    // Indicates whether the user has read the notification.
    public bool IsRead { get; set; } = false;

    // Stores the date and time when the notification was created.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property representing the notification recipient.
    public ApplicationUser User { get; set; } = null!;
}
