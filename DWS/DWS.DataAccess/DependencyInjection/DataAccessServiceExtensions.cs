
using DWS.DataAccess.Context;
using DWS.DataAccess.Interfaces;
using DWS.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DWS.DataAccess.DependencyInjection;

// Provides extension methods for registering Data Access services.
public static class DataAccessServiceExtensions
{
    // Registers the database context, repositories, and Unit of Work.
    public static IServiceCollection AddDataAccessServices(
        this IServiceCollection services,
        string connectionString)
    {
        // Registers ApplicationDbContext with SQL Server.
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Registers the generic repository for all entity types.
        services.AddScoped(
            typeof(IGenericRepository<>),
            typeof(GenericRepository<>));

        // Registers Unit of Work with a scoped lifetime.
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Returns the service collection for further registrations.
        return services;
    }
}
