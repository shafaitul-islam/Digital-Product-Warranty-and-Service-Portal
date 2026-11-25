using DWS.DataAccess.Context;
using DWS.Entities.Enums;
using DWS.Entities.Models;
using DWS.Web.UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var today = DateTime.UtcNow.Date;

            var dashboardData = new AdminDashboardViewModel();

            dashboardData.TotalUsers = await _context.Users.CountAsync();

            dashboardData.TotalProducts = await _context.Products.CountAsync();

            dashboardData.ActiveWarranties = await _context.Warranties
                .CountAsync(w =>
                    w.IsActive &&
                    w.StartDate <= today &&
                    w.EndDate >= today);

            dashboardData.TotalRequests = await _context.ServiceRequests
                .CountAsync();

            dashboardData.PendingRequests = await _context.ServiceRequests
                .CountAsync(s => s.Status == ServiceRequestStatus.Pending);

            dashboardData.InProgressRequests = await _context.ServiceRequests
                .CountAsync(s => s.Status == ServiceRequestStatus.InProgress);

            dashboardData.CompletedRequests = await _context.ServiceRequests
                .CountAsync(s => s.Status == ServiceRequestStatus.Completed);

            dashboardData.RecentServiceRequests = await _context.ServiceRequests
                .OrderByDescending(s => s.RequestDate)
                .Take(5)
                .Select(s => new RecentServiceRequestViewModel
                {
                    ServiceRequestId = s.ServiceRequestId,
                    CustomerName = s.ProductRegistration.Customer.FullName,
                    ProductName = s.ProductRegistration.Product.Name,
                    RequestDate = s.RequestDate,
                    Status = s.Status.ToString()
                })
                .ToListAsync();

            var startDate = new DateTime(
                today.Year,
                today.Month,
                1).AddMonths(-11);

            var endDate = new DateTime(
                today.Year,
                today.Month,
                1).AddMonths(1);

            var registrationDates = await _context.ProductRegistrations
                .Where(r =>
                    r.RegistrationDate >= startDate &&
                    r.RegistrationDate < endDate)
                .Select(r => r.RegistrationDate)
                .ToListAsync();

            var monthlyCounts = registrationDates
                .GroupBy(r => new { r.Year, r.Month })
                .ToDictionary(
                    g => (g.Key.Year, g.Key.Month),
                    g => g.Count());

            dashboardData.MonthlyWarrantyData = Enumerable.Range(0, 12)
                .Select(i =>
                {
                    var monthDate = startDate.AddMonths(i);

                    monthlyCounts.TryGetValue(
                        (monthDate.Year, monthDate.Month),
                        out var registrationCount);

                    return new MonthlyWarrantyDataViewModel
                    {
                        Month = monthDate.ToString("MMM yyyy"),
                        TotalRegistrations = registrationCount
                    };
                })
                .ToList();

            return View(dashboardData);
        }

        // Displays all products to the admin.
        public async Task<IActionResult> Products()
        {
            var products = await _context.Products
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new AdminProductViewModel
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    ModelNumber = p.ModelNumber,

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

            return View(products);
        }

        // Displays all service requests to the admin.
        public async Task<IActionResult> ServiceRequests()
        {
            var serviceRequests = await _context.ServiceRequests
                .OrderByDescending(s => s.RequestDate)
                .Select(s => new ServiceRequestItemViewModel
                {
                    ServiceRequestId = s.ServiceRequestId,

                    CustomerName = s.ProductRegistration.Customer.FullName,
                    CustomerEmail = s.ProductRegistration.Customer.Email
                        ?? string.Empty,

                    ProductName = s.ProductRegistration.Product.Name,
                    SerialNumber = s.ProductRegistration.SerialNumber,

                    IssueTitle = s.IssueTitle,
                    RequestDate = s.RequestDate,
                    Status = s.Status.ToString(),

                    AssignedStaffName = s.AssignedStaff != null
                        ? s.AssignedStaff.FullName
                        : "Not Assigned"
                })
                .ToListAsync();

            var viewModel = new AdminServiceRequestViewModel
            {
                ServiceRequests = serviceRequests
            };

            return View(viewModel);
        }

        // Displays the details of a specific service request.
        public async Task<IActionResult> Details(int id)
        {
            var request = await _context.ServiceRequests
                .Include(s => s.ProductRegistration)
                    .ThenInclude(r => r.Customer)
                .Include(s => s.ProductRegistration)
                    .ThenInclude(r => r.Product)
                .Include(s => s.AssignedStaff)
                .FirstOrDefaultAsync(s => s.ServiceRequestId == id);

            if (request == null)
            {
                return NotFound();
            }

            var registration = request.ProductRegistration;

            var warranty = await _context.Warranties
                .FirstOrDefaultAsync(w =>
                    w.ProductRegistrationId ==
                    registration.ProductRegistrationId);

            var today = DateTime.UtcNow.Date;

            var viewModel = new ServiceRequestDetailsViewModel
            {
                ServiceRequestId = request.ServiceRequestId,

                CustomerName = registration.Customer.FullName,
                CustomerEmail = registration.Customer.Email
                    ?? string.Empty,

                ProductName = registration.Product.Name,
                SerialNumber = registration.SerialNumber,
                PurchaseDate = registration.PurchaseDate,

                IssueTitle = request.IssueTitle,
                IssueDescription = request.IssueDescription,
                CustomerNotes = request.CustomerNotes ?? string.Empty,
                RequestDate = request.RequestDate,
                Status = request.Status.ToString(),

                AssignedStaffName = request.AssignedStaff != null
                    ? request.AssignedStaff.FullName
                    : "Not Assigned",

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

                AdminRemarks = request.AdminRemarks ?? string.Empty,
                StaffNotes = request.StaffNotes ?? string.Empty
            };

            return View(viewModel);
        }

        // GET: Admin/CreateProduct
        // Displays the form for creating a new product.
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var viewModel = new CreateProductViewModel
            {
                Brands = await _context.Brands
                    .Where(b => b.IsActive)
                    .OrderBy(b => b.Name)
                    .Select(b => new SelectListItem
                    {
                        Value = b.BrandId.ToString(),
                        Text = b.Name
                    })
                    .ToListAsync(),

                Categories = await _context.Categories
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.Name)
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.Name
                    })
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Admin/CreateProduct
        // Saves a new product to the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(
            CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Brands = await _context.Brands
                    .Where(b => b.IsActive)
                    .OrderBy(b => b.Name)
                    .Select(b => new SelectListItem
                    {
                        Value = b.BrandId.ToString(),
                        Text = b.Name
                    })
                    .ToListAsync();

                model.Categories = await _context.Categories
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.Name)
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.Name
                    })
                    .ToListAsync();

                return View(model);
            }

            var product = new Product
            {
                Name = model.Name.Trim(),
                ModelNumber = model.ModelNumber.Trim(),
                Description = model.Description?.Trim(),
                BrandId = model.BrandId,
                CategoryId = model.CategoryId,
                Price = model.Price,
                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Product created successfully.";

            return RedirectToAction(nameof(Products));
        }

        // GET: Admin/BrandsCategories
        // Displays all brands and categories.
        public async Task<IActionResult> BrandsCategories()
        {
            var viewModel = new AdminBrandsCategoriesViewModel
            {
                Brands = await _context.Brands
                    .OrderBy(b => b.Name)
                    .Select(b => new AdminBrandViewModel
                    {
                        BrandId = b.BrandId,
                        Name = b.Name,
                        Description = b.Description,
                        IsActive = b.IsActive,
                        CreatedAt = b.CreatedAt
                    })
                    .ToListAsync(),

                Categories = await _context.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new AdminCategoryViewModel
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name,
                        Description = c.Description,
                        IsActive = c.IsActive,
                        CreatedAt = c.CreatedAt
                    })
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // GET: Admin/CreateBrand
        // Displays the form for creating a new brand.
        [HttpGet]
        public IActionResult CreateBrand()
        {
            var model = new CreateBrandViewModel
            {
                IsActive = true
            };

            return View(model);
        }

        // POST: Admin/CreateBrand
        // Saves a new brand to the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBrand(
            CreateBrandViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var brandName = model.Name.Trim();

            // Check if the brand already exists.
            bool brandExists = await _context.Brands
                .AnyAsync(b => b.Name.ToLower() == brandName.ToLower());

            if (brandExists)
            {
                ModelState.AddModelError(
                    nameof(model.Name),
                    "This brand already exists.");

                return View(model);
            }

            var brand = new Brand
            {
                Name = brandName,
                Description = model.Description?.Trim(),
                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Brands.Add(brand);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Brand created successfully.";

            return RedirectToAction(nameof(BrandsCategories));
        }
    }
}