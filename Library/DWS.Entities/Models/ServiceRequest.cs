
using System;
using DWS.Entities.Enums;

namespace DWS.Entities.Models;

// Represents a service request submitted by a customer.
public class ServiceRequest
{
    // Primary key of the ServiceRequest table.
    public int ServiceRequestId { get; set; }

    // Foreign key referencing the customer's registered product.
    public int ProductRegistrationId { get; set; }

    // Stores a short title describing the reported problem.
    public string IssueTitle { get; set; } = string.Empty;

    // Stores detailed information about the product problem.
    public string IssueDescription { get; set; } = string.Empty;

    // Stores the date and time when the service request was submitted.
    public DateTime RequestDate { get; set; } = DateTime.UtcNow;

    // Stores the current status of the service request.
    public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.Pending;

    // Stores the customer's additional notes, if provided.
    public string? CustomerNotes { get; set; }

    // Foreign key referencing the assigned service staff member.
    public int? AssignedStaffId { get; set; }

    // Foreign key referencing the admin who assigned the request.
    public int? AssignedById { get; set; }

    // Stores when the request was assigned to service staff.
    public DateTime? AssignedAt { get; set; }

    // Stores when the service work was completed.
    public DateTime? CompletedAt { get; set; }

    // Stores progress updates or notes from service staff.
    public string? StaffNotes { get; set; }

    // Stores admin remarks or rejection reasons.
    public string? AdminRemarks { get; set; }

    // Navigation property representing the registered product.
    public ProductRegistration ProductRegistration { get; set; } = null!;

    // Navigation property representing the assigned service staff member.
    public ApplicationUser? AssignedStaff { get; set; }

    // Navigation property representing the admin who assigned the request.
    public ApplicationUser? AssignedBy { get; set; }
}
