using System;

namespace DWS.Web.UI.Models.ViewModels
{
    // ViewModel for displaying complete service request details.
    public class ServiceRequestDetailsViewModel
    {
        // Unique service request ID.
        public int ServiceRequestId { get; set; }

        // Customer information.
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        // Product information.
        public string ProductName { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }

        // Service request information.
        public string IssueTitle { get; set; } = string.Empty;
        public string IssueDescription { get; set; } = string.Empty;
        public string CustomerNotes { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = string.Empty;

        // Staff assignment information.
        public string AssignedStaffName { get; set; } = "Not Assigned";

        // Warranty information.
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public bool IsWarrantyActive { get; set; }

        // Admin and staff remarks.
        public string AdminRemarks { get; set; } = string.Empty;
        public string StaffNotes { get; set; } = string.Empty;
    }
}