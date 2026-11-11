
using DWS.DataAccess.Context;
using DWS.DataAccess.DependencyInjection;
using DWS.Entities.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Reads the SQL Server connection string from appsettings.json.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not found.");

// Registers the database context, repositories, and Unit of Work.
builder.Services.AddDataAccessServices(connectionString);

// Configures ASP.NET Core Identity with custom ApplicationUser and integer role IDs.
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
    {
        // Requires each user account to have a unique email address.
        options.User.RequireUniqueEmail = true;

        // Configures password security requirements.
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
    })
    // Connects Identity to ApplicationDbContext using Entity Framework Core.
    .AddEntityFrameworkStores<ApplicationDbContext>()

    // Enables Identity token generation for account-related operations.
    .AddDefaultTokenProviders();

// Registers MVC controllers and Razor Views.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configures the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // Redirects users to the custom error page in production.
    app.UseExceptionHandler("/Home/Error");

    // Enables HTTP Strict Transport Security.
    app.UseHsts();
}

// Redirects HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Enables routing for incoming HTTP requests.
app.UseRouting();

// Enables authentication to identify logged-in users.
app.UseAuthentication();

// Enables authorization to check user permissions and roles.
app.UseAuthorization();

// Maps static assets such as CSS, JavaScript, and images.
app.MapStaticAssets();

// Configures the default MVC route.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Starts the application.
app.Run();
