
using System;

namespace DWS.Entities.Models;

// Represents the warranty issued for an approved product registration.
public class Warranty
{
    // Primary key of the Warranty table.
    public int WarrantyId { get; set; }

    // Foreign key referencing the approved product registration.
    public int ProductRegistrationId { get; set; }

    // Stores the date when the warranty becomes active.
    public DateTime StartDate { get; set; }

    // Stores the date when the warranty expires.
    public DateTime EndDate { get; set; }

    // Stores the warranty duration in months.
    public int DurationInMonths { get; set; }

    // Stores a unique token used for warranty QR code generation and verification.
    public string QrCodeToken { get; set; } = Guid.NewGuid().ToString("N");

    // Indicates whether the warranty is currently active.
    public bool IsActive { get; set; } = true;

    // Stores the date and time when the warranty was created.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property representing the related product registration.
    public ProductRegistration ProductRegistration { get; set; } = null!;
}
