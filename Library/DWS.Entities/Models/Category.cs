
namespace DWS.Entities.Models;

// Represents a product category in the application.
public class Category
{
    // Primary key of the Category table.
    public int CategoryId { get; set; }

    // Stores the category name, such as Electronics or Mobile Phones.
    public string Name { get; set; } = string.Empty;

    // Stores additional information about the category.
    public string? Description { get; set; }

    // Indicates whether the category is currently active.
    public bool IsActive { get; set; } = true;

    // Stores the date and time when the category was created.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
