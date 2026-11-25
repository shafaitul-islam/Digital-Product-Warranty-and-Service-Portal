using System;

namespace DWS.Web.UI.Models.ViewModels
{
    // ViewModel for displaying category information in the admin panel.
    public class AdminCategoryViewModel
    {
        // Unique identifier of the category.
        public int CategoryId { get; set; }

        // Stores the category name.
        public string Name { get; set; } = string.Empty;

        // Stores the category description.
        public string? Description { get; set; }

        // Indicates whether the category is active.
        public bool IsActive { get; set; }

        // Stores the category creation date.
        public DateTime CreatedAt { get; set; }
    }
}