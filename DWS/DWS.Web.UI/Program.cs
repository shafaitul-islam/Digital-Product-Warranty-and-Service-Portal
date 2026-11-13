
using DWS.DataAccess.Context;
using DWS.DataAccess.DependencyInjection;
using DWS.Entities.Models;
using DWS.Web.UI.Seeders;
using Microsoft.AspNetCore.Identity;

// Creates the application builder.
var builder = WebApplication.CreateBuilder(args);

// Registers MVC services for Controllers and Views.
builder.Services.AddControllersWithViews();

// Reads the SQL Server connection string from appsettings.json.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not found.");

// Registers the database context, repositories, and Unit of Work.
builder.Services.AddDataAccessServices(connectionString);

// Configures ASP.NET Core Identity with ApplicationUser and integer-based roles.
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
    {
        // Defines password security requirements.
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;

        // Ensures each user has a unique email address.
        options.User.RequireUniqueEmail = true;

        // Email confirmation is not required until an email service is configured.
        options.SignIn.RequireConfirmedEmail = false;
    })
    // Stores Identity users and roles in the application's SQL Server database.
    .AddEntityFrameworkStores<ApplicationDbContext>()
    // Enables Identity token generation.
    .AddDefaultTokenProviders();

// Configures the authentication cookie.
builder.Services.ConfigureApplicationCookie(options =>
{
    // Redirects unauthenticated users to the Login page.
    options.LoginPath = "/Account/Login";

    // Redirects unauthorized users to the Access Denied page.
    options.AccessDeniedPath = "/Account/AccessDenied";

    // Sets the authentication cookie expiration time to 60 minutes.
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

    // Renews the cookie expiration when the user remains active.
    options.SlidingExpiration = true;
});

// Builds the application.
var app = builder.Build();

// Creates the default application roles in the database.
await RoleSeeder.SeedRolesAsync(app.Services);

// Configures error handling and security headers for production.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirects HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Enables routing.
app.UseRouting();

// Checks the user's authentication cookie.
app.UseAuthentication();

// Applies authorization rules after authentication.
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
