
namespace DWS.Entities.Models;

// Represents a product available in the warranty portal.
public class Product
{
    // Primary key of the Product table.
    public int ProductId { get; set; }

    // Stores the product name.
    public string Name { get; set; } = string.Empty;

    // Stores the product model number.
    public string ModelNumber { get; set; } = string.Empty;

    // Stores additional information about the product.
    public string? Description { get; set; }

    // Foreign key referencing the Brand table.
    public int BrandId { get; set; }

    // Foreign key referencing the Category table.
    public int CategoryId { get; set; }

    // Stores the product price.
    public decimal Price { get; set; }

    // Indicates whether the product is currently available.
    public bool IsActive { get; set; } = true;

    // Stores the date and time when the product was created.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property representing the product's brand.
    public Brand Brand { get; set; } = null!;

    // Navigation property representing the product's category.
    public Category Category { get; set; } = null!;
}
