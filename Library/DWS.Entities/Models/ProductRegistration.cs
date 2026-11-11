
using System;
using DWS.Entities.Enums;

namespace DWS.Entities.Models;

// Represents a customer's product registration for warranty activation.
public class ProductRegistration
{
    // Primary key of the ProductRegistration table.
    public int ProductRegistrationId { get; set; }

    // Foreign key referencing the registered product.
    public int ProductId { get; set; }

    // Foreign key referencing the customer who registered the product.
    public int CustomerId { get; set; }

    // Stores the product's unique serial number.
    public string SerialNumber { get; set; } = string.Empty;

    // Stores the date when the customer purchased the product.
    public DateTime PurchaseDate { get; set; }

    // Stores the date and time when the product was registered.
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    // Stores the current registration approval status.
    public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;

    // Stores the uploaded invoice image path, if provided.
    public string? InvoiceImagePath { get; set; }

    // Foreign key referencing the admin who reviewed the registration.
    public int? ReviewedById { get; set; }

    // Stores the date and time when the registration was reviewed.
    public DateTime? ReviewedAt { get; set; }

    // Stores remarks or rejection reasons provided by the admin.
    public string? AdminRemarks { get; set; }

    // Navigation property representing the registered product.
    public Product Product { get; set; } = null!;

    // Navigation property representing the customer.
    public ApplicationUser Customer { get; set; } = null!;

    // Navigation property representing the reviewing admin.
    public ApplicationUser? ReviewedBy { get; set; }
}
