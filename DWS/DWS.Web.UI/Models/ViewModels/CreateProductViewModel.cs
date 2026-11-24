using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DWS.Web.UI.Models.ViewModels
{
    // ViewModel for creating a new product.
    public class CreateProductViewModel
    {
        // Stores the product name.
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        // Stores the product model number.
        [Required(ErrorMessage = "Model number is required.")]
        [StringLength(50, ErrorMessage = "Model number cannot exceed 50 characters.")]
        public string ModelNumber { get; set; } = string.Empty;

        // Stores additional product information.
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        // Stores the selected brand ID.
        [Required(ErrorMessage = "Please select a brand.")]
        public int BrandId { get; set; }

        // Stores the available brands for the dropdown.
        public List<SelectListItem> Brands { get; set; } = new();

        // Stores the selected category ID.
        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }

        // Stores the available categories for the dropdown.
        public List<SelectListItem> Categories { get; set; } = new();

        // Stores the product price.
        [Required(ErrorMessage = "Product price is required.")]
        [Range(0.01, 99999999, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        // Indicates whether the product is active.
        public bool IsActive { get; set; } = true;
    }
}