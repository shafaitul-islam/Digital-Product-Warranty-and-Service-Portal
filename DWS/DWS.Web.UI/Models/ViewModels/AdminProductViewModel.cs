using System;

namespace DWS.Web.UI.Models.ViewModels
{
    // ViewModel for displaying products in the admin panel.
    public class AdminProductViewModel
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ModelNumber { get; set; } = string.Empty;

        public string BrandName { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}