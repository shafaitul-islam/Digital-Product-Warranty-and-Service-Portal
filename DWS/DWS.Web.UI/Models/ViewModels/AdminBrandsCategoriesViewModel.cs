namespace DWS.Web.UI.Models.ViewModels
{
    // ViewModel for managing brands and categories together.
    public class AdminBrandsCategoriesViewModel
    {
        // Stores all brand information.
        public List<AdminBrandViewModel> Brands { get; set; } = new();

        // Stores all category information.
        public List<AdminCategoryViewModel> Categories { get; set; } = new();
    }
}