using System;

namespace DWS.Web.UI.Models.ViewModels
{
    // ViewModel for displaying brand information in the admin panel.
    public class AdminBrandViewModel
    {
        // Unique identifier of the brand.
        public int BrandId { get; set; }

        // Stores the brand name.
        public string Name { get; set; } = string.Empty;

        // Stores the brand description.
        public string? Description { get; set; }

        // Indicates whether the brand is active.
        public bool IsActive { get; set; }

        // Stores the brand creation date.
        public DateTime CreatedAt { get; set; }
    }
}