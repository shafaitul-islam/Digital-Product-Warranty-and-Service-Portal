
using DWS.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DWS.DataAccess.Context;

// Represents the main database context of the application.
// It manages Identity users, roles, and application data.
public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    // Constructor: Receives database configuration options through dependency injection.
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Represents the Brands table.
    public DbSet<Brand> Brands => Set<Brand>();

    // Represents the Categories table.
    public DbSet<Category> Categories => Set<Category>();

    // Represents the Products table.
    public DbSet<Product> Products => Set<Product>();

    // Represents the ProductRegistrations table.
    public DbSet<ProductRegistration> ProductRegistrations => Set<ProductRegistration>();

    // Represents the Warranties table.
    public DbSet<Warranty> Warranties => Set<Warranty>();

    // Represents the ServiceRequests table.
    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();

    // Represents the ServiceHistories table.
    public DbSet<ServiceHistory> ServiceHistories => Set<ServiceHistory>();

    // Represents the Notifications table.
    public DbSet<Notification> Notifications => Set<Notification>();

    // Configures Identity and all application entity relationships.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Applies the default ASP.NET Core Identity database configurations.
        base.OnModelCreating(modelBuilder);

        // Automatically applies all IEntityTypeConfiguration classes
        // located in the DWS.DataAccess assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}
