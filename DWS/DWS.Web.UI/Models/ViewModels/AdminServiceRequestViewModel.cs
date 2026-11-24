
using System;
using System.Collections.Generic;

namespace DWS.Web.UI.Models.ViewModels
{
    // ViewModel for displaying service requests in the admin panel.
    public class AdminServiceRequestViewModel
    {
        // List of service requests displayed in the table.
        public List<ServiceRequestItemViewModel> ServiceRequests { get; set; }
            = new List<ServiceRequestItemViewModel>();
    }

    // ViewModel for a single service request.
    public class ServiceRequestItemViewModel
    {
        // Unique service request ID.
        public int ServiceRequestId { get; set; }

        // Customer name.
        public string CustomerName { get; set; } = string.Empty;

        // Customer email.
        public string CustomerEmail { get; set; } = string.Empty;

        // Product name.
        public string ProductName { get; set; } = string.Empty;

        // Product serial number.
        public string SerialNumber { get; set; } = string.Empty;

        // Issue title.
        public string IssueTitle { get; set; } = string.Empty;

        // Date when the service request was submitted.
        public DateTime RequestDate { get; set; }

        // Current service request status.
        public string Status { get; set; } = string.Empty;

        // Assigned staff member name.
        public string AssignedStaffName { get; set; } = "Not Assigned";
    }
}
