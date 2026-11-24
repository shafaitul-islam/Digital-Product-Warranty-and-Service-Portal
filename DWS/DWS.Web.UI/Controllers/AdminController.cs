
using DWS.DataAccess.Context;
using DWS.Entities.Enums;
using DWS.Web.UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DWS.Web.UI.Controllers
{
    // Handles all admin dashboard and management operations.
    public class AdminController : Controller
    {
        // Database context used to access application data.
        private readonly ApplicationDbContext _context;

        // Constructor injection of ApplicationDbContext.
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Displays the Admin Dashboard.
        public async Task<IActionResult> Dashboard()
        {
            // Get the current date for checking active warranties.
            var today = DateTime.UtcNow.Date;

            // Create the dashboard ViewModel.
            var dashboardData = new AdminDashboardViewModel();

            // Count all registered users.
            dashboardData.TotalUsers = await _context.Users.CountAsync();

            // Count all products.
            dashboardData.TotalProducts = await _context.Products.CountAsync();

            // Count currently valid and active warranties.
            dashboardData.ActiveWarranties = await _context.Warranties
                .CountAsync(w =>
                    w.IsActive &&
                    w.StartDate <= today &&
                    w.EndDate >= today);

            // Count all service requests.
            dashboardData.TotalRequests = await _context.ServiceRequests
                .CountAsync();

            // Count pending service requests.
            dashboardData.PendingRequests = await _context.ServiceRequests
                .CountAsync(s => s.Status == ServiceRequestStatus.Pending);

            // Count service requests currently in progress.
            dashboardData.InProgressRequests = await _context.ServiceRequests
                .CountAsync(s => s.Status == ServiceRequestStatus.InProgress);

            // Count completed service requests.
            dashboardData.CompletedRequests = await _context.ServiceRequests
                .CountAsync(s => s.Status == ServiceRequestStatus.Completed);

            // Fetch the five most recent service requests.
            dashboardData.RecentServiceRequests = await _context.ServiceRequests
                .OrderByDescending(s => s.RequestDate)
                .Take(5)
                .Select(s => new RecentServiceRequestViewModel
                {
                    ServiceRequestId = s.ServiceRequestId,

                    // Get the customer's full name.
                    CustomerName = s.ProductRegistration.Customer.FullName,

                    // Get the registered product's name.
                    ProductName = s.ProductRegistration.Product.Name,

                    // Get the service request date.
                    RequestDate = s.RequestDate,

                    // Convert the request status to a readable string.
                    Status = s.Status.ToString()
                })
                .ToListAsync();

            // Calculate the starting month for the last 12 months.
            var startDate = new DateTime(
                today.Year,
                today.Month,
                1).AddMonths(-11);

            // Calculate the start of the next month.
            var endDate = new DateTime(
                today.Year,
                today.Month,
                1).AddMonths(1);

            // Fetch product registration dates from the last 12 months.
            var registrationDates = await _context.ProductRegistrations
                .Where(r =>
                    r.RegistrationDate >= startDate &&
                    r.RegistrationDate < endDate)
                .Select(r => r.RegistrationDate)
                .ToListAsync();

            // Group registrations by year and month.
            var monthlyCounts = registrationDates
                .GroupBy(r => new { r.Year, r.Month })
                .ToDictionary(
                    g => (g.Key.Year, g.Key.Month),
                    g => g.Count());

            // Generate data for all 12 months.
            dashboardData.MonthlyWarrantyData = Enumerable.Range(0, 12)
                .Select(i =>
                {
                    // Get the current month in the 12-month range.
                    var monthDate = startDate.AddMonths(i);

                    // Get the registration count for this month.
                    monthlyCounts.TryGetValue(
                        (monthDate.Year, monthDate.Month),
                        out var registrationCount);

                    // Create monthly chart data.
                    return new MonthlyWarrantyDataViewModel
                    {
                        Month = monthDate.ToString("MMM yyyy"),
                        TotalRegistrations = registrationCount
                    };
                })
                .ToList();

            // Pass dashboard data to the View.
            return View(dashboardData);
        }

        // Displays all products to the admin.
        public async Task<IActionResult> Products()
        {
            // Fetch products with their brand and category information.
            var products = await _context.Products
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new AdminProductViewModel
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    ModelNumber = p.ModelNumber,

                    // Get brand and category names.
                    BrandName = p.Brand != null
                        ? p.Brand.Name
                        : "N/A",

                    CategoryName = p.Category != null
                        ? p.Category.Name
                        : "N/A",

                    Price = p.Price,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();

            // Pass the product list to the view.
            return View(products);
        }

        // Displays all service requests to the admin.
        public async Task<IActionResult> ServiceRequests()
        {
            // Fetch all service requests from the database.
            var serviceRequests = await _context.ServiceRequests
                .OrderByDescending(s => s.RequestDate)
                .Select(s => new ServiceRequestItemViewModel
                {
                    ServiceRequestId = s.ServiceRequestId,

                    // Get customer information.
                    CustomerName = s.ProductRegistration.Customer.FullName,
                    CustomerEmail = s.ProductRegistration.Customer.Email ?? string.Empty,

                    // Get product information.
                    ProductName = s.ProductRegistration.Product.Name,
                    SerialNumber = s.ProductRegistration.SerialNumber,

                    // Get service request details.
                    IssueTitle = s.IssueTitle,
                    RequestDate = s.RequestDate,
                    Status = s.Status.ToString(),

                    // Get assigned staff name when available.
                    AssignedStaffName = s.AssignedStaff != null
                        ? s.AssignedStaff.FullName
                        : "Not Assigned"
                })
                .ToListAsync();

            // Create the ViewModel.
            var viewModel = new AdminServiceRequestViewModel
            {
                ServiceRequests = serviceRequests
            };

            // Pass data to the Service Requests view.
            return View(viewModel);
        }

        // Displays the details of a specific service request.
        public async Task<IActionResult> Details(int id)
        {
            // Fetch the requested service request with related data.
            var request = await _context.ServiceRequests
                .Include(s => s.ProductRegistration)
                    .ThenInclude(r => r.Customer)
                .Include(s => s.ProductRegistration)
                    .ThenInclude(r => r.Product)
                .Include(s => s.AssignedStaff)
                .FirstOrDefaultAsync(s => s.ServiceRequestId == id);

            // Return NotFound when the request does not exist.
            if (request == null)
            {
                return NotFound();
            }

            // Get the related product registration.
            var registration = request.ProductRegistration;

            // Fetch warranty information for the product registration.
            var warranty = await _context.Warranties
                .FirstOrDefaultAsync(w =>
                    w.ProductRegistrationId == registration.ProductRegistrationId);

            // Get the current date for warranty validation.
            var today = DateTime.UtcNow.Date;

            // Create the details ViewModel.
            var viewModel = new ServiceRequestDetailsViewModel
            {
                ServiceRequestId = request.ServiceRequestId,

                // Customer information.
                CustomerName = registration.Customer.FullName,
                CustomerEmail = registration.Customer.Email ?? string.Empty,

                // Product information.
                ProductName = registration.Product.Name,
                SerialNumber = registration.SerialNumber,
                PurchaseDate = registration.PurchaseDate,

                // Service request information.
                IssueTitle = request.IssueTitle,
                IssueDescription = request.IssueDescription,
                CustomerNotes = request.CustomerNotes ?? string.Empty,
                RequestDate = request.RequestDate,
                Status = request.Status.ToString(),

                // Staff assignment information.
                AssignedStaffName = request.AssignedStaff != null
                    ? request.AssignedStaff.FullName
                    : "Not Assigned",

                // Warranty information.
                WarrantyStartDate = warranty != null
                    ? warranty.StartDate
                    : null,

                WarrantyEndDate = warranty != null
                    ? warranty.EndDate
                    : null,

                IsWarrantyActive = warranty != null
                    && warranty.IsActive
                    && warranty.StartDate <= today
                    && warranty.EndDate >= today,

                // Admin and staff remarks.
                AdminRemarks = request.AdminRemarks ?? string.Empty,
                StaffNotes = request.StaffNotes ?? string.Empty
            };

            // Pass the details ViewModel to the view.
            return View(viewModel);
        }
    }
}
