
using System;
using DWS.Entities.Enums;

namespace DWS.Entities.Models;

// Represents the history of status changes and updates for a service request.
public class ServiceHistory
{
    // Primary key of the ServiceHistory table.
    public int ServiceHistoryId { get; set; }

    // Foreign key referencing the related service request.
    public int ServiceRequestId { get; set; }

    // Stores the previous status before the update.
    // Null means this is the first recorded status.
    public ServiceRequestStatus? PreviousStatus { get; set; }

    // Stores the new status after the update.
    public ServiceRequestStatus NewStatus { get; set; }

    // Foreign key referencing the user who made the update.
    public int? UpdatedById { get; set; }

    // Stores additional remarks about the status update.
    public string? Remarks { get; set; }

    // Stores the date and time when the update was recorded.
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property representing the related service request.
    public ServiceRequest ServiceRequest { get; set; } = null!;

    // Navigation property representing the user who made the update.
    public ApplicationUser? UpdatedBy { get; set; }
}
