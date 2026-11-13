
using System;
using System.Linq;
using System.Threading.Tasks;
using DWS.Framework.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DWS.Web.UI.Seeders;

// Creates the default application roles in the database.
public static class RoleSeeder
{
    // Checks and creates the required roles when the application starts.
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        // Creates a separate dependency injection scope.
        using var scope = serviceProvider.CreateScope();

        // Gets the role manager from the dependency injection container.
        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole<int>>>();

        // Defines all roles required by the application.
        string[] roles =
        {
            UserRoles.Admin,
            UserRoles.Customer,
            UserRoles.ServiceStaff
        };

        // Checks each role and creates it only if it does not exist.
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                // Creates a new role in the database.
                var result = await roleManager.CreateAsync(
                    new IdentityRole<int>(roleName));

                // Stops application startup if role creation fails.
                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        result.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Failed to create role '{roleName}': {errors}");
                }
            }
        }
    }
}
