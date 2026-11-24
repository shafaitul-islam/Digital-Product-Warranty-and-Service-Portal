using System;
using System.Collections.Generic;

namespace DWS.Web.UI.Models.ViewModels
{
    // ViewModel used to display all data on the Admin Dashboard.
    public class AdminDashboardViewModel
    {
        // Total registered users in the system.
        public int TotalUsers { get; set; }

        // Total products available in the system.
        public int TotalProducts { get; set; }

        // Total currently active warranties.
        public int ActiveWarranties { get; set; }

        // Total service requests in the system.
        public int TotalRequests { get; set; }

        // Total service requests waiting for admin action.
        public int PendingRequests { get; set; }

        // Total service requests currently being processed.
        public int InProgressRequests { get; set; }

        // Total completed service requests.
        public int CompletedRequests { get; set; }

        // Recent service requests displayed in the dashboard table.
        public List<RecentServiceRequestViewModel> RecentServiceRequests { get; set; }
            = new List<RecentServiceRequestViewModel>();

        // Monthly warranty registration data for the dashboard chart.
        public List<MonthlyWarrantyDataViewModel> MonthlyWarrantyData { get; set; }
            = new List<MonthlyWarrantyDataViewModel>();
    }

    // ViewModel for a single recent service request row.
    public class RecentServiceRequestViewModel
    {
        // Unique service request ID.
        public int ServiceRequestId { get; set; }

        // Customer name.
        public string CustomerName { get; set; } = string.Empty;

        // Product name.
        public string ProductName { get; set; } = string.Empty;

        // Date of the service request.
        public DateTime RequestDate { get; set; }

        // Current service request status.
        public string Status { get; set; } = string.Empty;
    }

    // ViewModel for monthly warranty registration chart data.
    public class MonthlyWarrantyDataViewModel
    {
        // Month name displayed on the chart.
        public string Month { get; set; } = string.Empty;

        // Total registrations for the month.
        public int TotalRegistrations { get; set; }
    }
}