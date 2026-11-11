
namespace DWS.Entities.Models;

// Represents a product brand in the application.
public class Brand
{
    // Primary key of the Brand table.
    public int BrandId { get; set; }

    // Stores the brand name, such as Samsung or Apple.
    public string Name { get; set; } = string.Empty;

    // Stores additional information about the brand.
    public string? Description { get; set; }

    // Indicates whether the brand is currently active.
    public bool IsActive { get; set; } = true;

    // Stores the date and time when the brand was created.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
